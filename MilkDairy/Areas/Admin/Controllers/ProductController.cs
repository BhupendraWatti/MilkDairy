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
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objlist = _unitOfWork.ProductRepo.GetAll().ToList();
            return View(objlist);
        }

        public IActionResult Upsert(int? id)
        {
            Product product = new Product();
            if (id != null && id > 0)
            {
                product = _unitOfWork.ProductRepo.Get(u => u.Id == id);
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
                        var existingProduct = _unitOfWork.ProductRepo.Get(p => p.Id == obj.Id);  // Retrieve the existing product by ID
                        if (existingProduct != null)
                        {
                            obj.ImgURL = existingProduct.ImgURL;  // Retain the existing image URL if no new image is uploaded
                        }
                    }
                }

                if (obj.Id == 0)
                {
                    _unitOfWork.ProductRepo.Add(obj);
                    TempData["Success"] = "Your new product has been added";
                }
                else
                {
                    _unitOfWork.ProductRepo.Updatea(obj);
                    TempData["Success"] = "Your product has been updated";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        #region API call
        [HttpGet]
        public IActionResult GetAll()
        {
            var objlist = _unitOfWork.ProductRepo.GetAll().Select(p => new
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
            var productToDeleted = _unitOfWork.ProductRepo.Get(u => u.Id == id);
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
            _unitOfWork.ProductRepo.Remove(productToDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, Message = "Delete Successful" });
        }
        #endregion
    }
}
