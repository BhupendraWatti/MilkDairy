using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using MilkDairy.Model.Models;
using MIlkDairy.DataAccess.Repository.IRepository;
using System.Text.RegularExpressions;


namespace MilkDairy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IProductRepository _productrepo;
        private IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductRepository db, IWebHostEnvironment webHostEnvironment)
        {
            _productrepo = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objlist = _productrepo.GetAll().ToList();
            return View(objlist);
        }

        public IActionResult Upsert(int? id)
        {
            Product product = new Product();
            if (id != null && id > 0)
            {
                product = _productrepo.Get(u => u.Id == id);
                if (product == null)
                {
                    return NotFound();
                }
            }
            return View(product);
        }
        [HttpPost]
        public IActionResult Upsert(Product obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                // Clean up the description if necessary
                if (!string.IsNullOrEmpty(obj.Description))
                {
                    obj.Description = Regex.Replace(obj.Description, @"<\/?p>", "").Trim();
                }

                string wwwRootPath = _webHostEnvironment.WebRootPath;

                // Check if a new image is uploaded
                if (file != null && file.Length > 0)
                {
                    // Define the path for saving images
                    string productPath = Path.Combine(wwwRootPath, "images", "Product");
                    string filename = Guid.NewGuid().ToString() + "_" + Path.GetExtension(file.FileName);

                    // If an old image exists, delete it
                    if (!string.IsNullOrEmpty(obj.ImgURL))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.ImgURL.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save the new image
                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Update the ImgURL with the new image path
                    obj.ImgURL = Path.Combine("/images", "Product", filename).Replace("\\", "/");
                }
                else
                {
                    // If no image file is uploaded, retrieve the existing product and keep the ImgURL unchanged
                    if (obj.Id != 0)
                    {
                        var existingProduct = _productrepo.Get(p => p.Id == obj.Id);  // Retrieve the existing product by ID
                        if (existingProduct != null)
                        {
                            obj.ImgURL = existingProduct.ImgURL;  // Retain the existing image URL if no new image is uploaded
                        }
                    }
                }

                if (obj.Id == 0)
                {
                    _productrepo.Add(obj);
                    TempData["Success"] = "Your new product has been added";
                }
                else
                {
                    _productrepo.Updatea(obj);
                    TempData["Success"] = "Your product has been updated";
                }
                _productrepo.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        #region API call
        [HttpGet]
        public IActionResult GetAll()
        {
            var objlist = _productrepo.GetAll().Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.Brand,
                p.WeightVolume,
                p.Ingredients,
                p.UnitPrice,
                p.UnitsInStock,
                p.IsAvailable
            }).ToList();

            return Json(new { data = objlist });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToDeleted = _productrepo.Get(u => u.Id == id);
            if (productToDeleted == null)
            {
                return Json(new { success = false, Message = "Error while Deleteing" });
            }
            var oldImagePath =
            Path.Combine(_webHostEnvironment.WebRootPath, productToDeleted.ImgURL.TrimStart('/'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _productrepo.Remove(productToDeleted);
            _productrepo.Save();

            return Json(new { success = true, Message = "Delete Successful" });
        }
        #endregion
    }
}
