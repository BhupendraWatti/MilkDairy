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
        private readonly IUnitOfWork _unitOfWork;
        public DetailsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }
        public IActionResult Index()
        {
            List<Details> objList = _unitOfWork.DetailsRepo.GetAll().ToList();
            return View(objList);
        }
        public IActionResult Upsert(int? id)
        {
            Details details = new Details();
            if (id != null && id > 0)
            {
                details = _unitOfWork.DetailsRepo.Get(u => u.ID == id);
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
                    _unitOfWork.DetailsRepo.Add(obj);
                    TempData["Success"] = "Your details have been Added";
                }
                else
                {
                    _unitOfWork.DetailsRepo.Updatea(obj);
                    TempData["Success"] = "Your details has been Updated";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        #region API
        [HttpGet]
        public IActionResult GetAll()
        {
            var listOfDetails = _unitOfWork.DetailsRepo.GetAll().Select(p => new
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
            var detailsItemtoDelete = _unitOfWork.DetailsRepo.Get(u => u.ID == id);
            if (id == 0)
            {
                return Json(new { success = false, Message = "Error while Deleting" });
            }
            _unitOfWork.DetailsRepo.Remove(detailsItemtoDelete);
            _unitOfWork.Save();
            return Json(new { success = true, Message = "Deleted successfuly" });
            #endregion
        }
    }
}