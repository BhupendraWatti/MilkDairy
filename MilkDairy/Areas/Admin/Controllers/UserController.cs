
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkDairy.DataAccess.Data;
using MilkDairy.Model;
using MilkDairy.Model.Models;
using MilkDairy.Utility;
using MIlkDairy.DataAccess.Repository.IRepository;

namespace MilkDairy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly AppDbContext _db;
        public UserController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }



        #region API call

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var users = _db.ApplicationUsers.Include(u => u.Company).ToList();
                return Json(new { data = users });
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework)
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion


    }
}
