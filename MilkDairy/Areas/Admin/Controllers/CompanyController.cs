
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkDairy.Model.Models;
using MilkDairy.Utility;
using MIlkDairy.DataAccess.Repository.IRepository;

namespace MilkDairy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> companies = _unitOfWork.CompanyRepo.GetAll().ToList();
            return View(companies);
        }
        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id != null && id > 0)
            {
                company = _unitOfWork.CompanyRepo.Get(u => u.Id == id);
                if (id == null)
                {
                    return NotFound();
                }
            }
            return View(company);
        }
        [HttpPost]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
                if (obj == null)
                {
                    _unitOfWork.CompanyRepo.Add(obj);
                    TempData["Success"] = "Success fully added";
                }
                else
                {
                    _unitOfWork.CompanyRepo.Update(obj);
                    TempData["Success"] = "Success fully Update the details";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    

    #region API call

    [HttpGet]
        public IActionResult GetAll() {
            var ListofCompany = _unitOfWork.CompanyRepo.GetAll().Select(p => new
            {
                p.Id,
                p.Name,
                p.StreetAddress,
                p.State,
                p.City,
                p.PostalCode,
                p.PhoneNumber
            }).ToList();

            return Json(new {data =  ListofCompany });
        }
        [HttpDelete]
        public IActionResult Delete(int id) {
            var deleteCompany = _unitOfWork.CompanyRepo.Get(u => u.Id == id);
            if(id == 0)
            {
                return Json(new { success=false, Message= "Error while deleting" });
            }
            _unitOfWork.CompanyRepo.Remove(deleteCompany);
            _unitOfWork.Save();
            return Json(new { success = true, Message = "Successfully deleted the item" });
        }


    }
        #endregion


}
