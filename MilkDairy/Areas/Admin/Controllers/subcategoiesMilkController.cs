using Microsoft.AspNetCore.Mvc;
using MilkDairy.Model.Models;
using MIlkDairy.DataAccess.Repository.IRepository;

namespace MilkDairy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class subcategoiesMilkController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public subcategoiesMilkController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<subcategoiesMilk> sub = _unitOfWork.milkProp.GetAll().ToList();
            return View(sub);
        }
        public IActionResult Upsert(int? obj)
        {

            subcategoiesMilk submilkobj = new subcategoiesMilk();
            if (obj == null && obj > 0)
            {
                submilkobj = _unitOfWork.milkProp.Get(u => u.MilkId == obj);
                if (obj == null)
                {
                    return NotFound();
                }
            }
            return View(submilkobj);
        }
        [HttpPost]
        public IActionResult Upsert(subcategoiesMilk obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.MilkId == 0)
                {
                    _unitOfWork.milkProp.Add(obj);
                    TempData["Success"] = "Successfuly added new items to list";
                }
                else
                {
                    _unitOfWork.milkProp.Updatea(obj);
                    TempData["Success"] = "Successfuly edit the item";
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
            var submilkList = _unitOfWork.milkProp.GetAll().Select(p => new
            {
                p.MilkId,
                p.MilkName,
                p.MilkType,
                p.FatContent,
                p.ProteinContent,
                p.IsOrganic,
                p.PackagingType
            }).ToList();
            return Json(new {data = submilkList});
        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var submilktype = _unitOfWork.milkProp.Get(u=>u.MilkId==id);
            if (submilktype == null)
            {
                return Json(new { success = false, Message = "Error while deleting" });
            }
            _unitOfWork.milkProp.Remove(submilktype);
            _unitOfWork.Save();
            return Json(new { success = true, Message = "Successfuly deleted" });
        }

        #endregion
    }
}
