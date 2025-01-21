using Microsoft.AspNetCore.Mvc;
using MilkDairy.DataAccess.Data;
using MilkDairy.Model.Models;
using MIlkDairy.DataAccess.Repository.IRepository;
using NuGet.Protocol.Plugins;


namespace MilkDairy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DetailsController : Controller
    {
        private readonly IDetailsRepository _detialsRepo;
        public DetailsController(IDetailsRepository db)
        {
            _detialsRepo = db;
        }
        public IActionResult Index()
        {
            List<Details> objList = _detialsRepo.GetAll().ToList();
            return View(objList);
        }
        public IActionResult Upsert(int? id)
        {
            Details details = new Details();
            if (id != null && id > 0)
            {
                details = _detialsRepo.Get(u => u.ID == id);
                if (details == null)
                {
                    return NotFound();
                }
            }

            return View(details);

        }
        [HttpPost]
        public IActionResult Upsert(Details obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.ID == 0)
                {
                    _detialsRepo.Add(obj);
                    TempData["Success"] = "Your details have been Added";
                }
                else
                {
                    _detialsRepo.Updatea(obj);
                    TempData["Success"] = "Your details has been Updated";
                }
                _detialsRepo.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        #region API
        [HttpGet]
        public IActionResult GetAll()
        {
            var listOfDetails = _detialsRepo.GetAll().Select(p => new
            {
                p.ID,
                p.Items,
                p.MFD,
                p.EXD,
                p.Price,
                p.Quantity

            }).ToList();

            return Json(new { data = listOfDetails });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var detailsItemtoDelete = _detialsRepo.Get(u => u.ID == id);
            if (id == 0)
            {
                return Json(new { success = false, Message = "Error while Deleting" });
            }
            _detialsRepo.Remove(detailsItemtoDelete);
            _detialsRepo.Save();
            return Json(new { success = true, Message = "Deleted successfuly" });
            #endregion
        }
    }
}