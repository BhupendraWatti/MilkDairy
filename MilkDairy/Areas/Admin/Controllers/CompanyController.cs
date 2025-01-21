
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
        private readonly ICompanyRepository _companyRepository;
        public CompanyController(ICompanyRepository comapyRepo)
        {
            _companyRepository = comapyRepo;
        }
        public IActionResult Index()
        {
            List<Company> companies = _companyRepository.GetAll().ToList();
            return View(companies);
        }
        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id != null && id > 0)
            {
                company = _companyRepository.Get(u => u.Id == id);
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
                    _companyRepository.Add(obj);
                    TempData["Success"] = "Success fully added";
                }
                else
                {
                    _companyRepository.Update(obj);
                    TempData["Success"] = "Success fully Update the details";
                }
                _companyRepository.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    

    #region API call

    [HttpGet]
        public IActionResult GetAll() {
            var ListofCompany = _companyRepository.GetAll().Select(p => new
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
            var deleteCompany = _companyRepository.Get(u => u.Id == id);
            if(id == 0)
            {
                return Json(new { success=false, Message= "Error while deleting" });
            }
            _companyRepository.Remove(deleteCompany);
            _companyRepository.Save();
            return Json(new { success = true, Message = "Successfully deleted the item" });
        }


    }
        #endregion


}
