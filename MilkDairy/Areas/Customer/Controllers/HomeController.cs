using Microsoft.AspNetCore.Mvc;
using MilkDairy.DataAccess.Data;
using MilkDairy.Model.Models;
using MIlkDairy.DataAccess.Repository.IRepository;
using System.Diagnostics;

namespace MilkDairy.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _db;

        public HomeController(ILogger<HomeController> logger, IProductRepository db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _db.GetAll();
            return View(products);
        }public IActionResult Details(int productID)
        {
            Product productsdetail = _db.Get(u=>u.Id== productID);
            return View(productsdetail);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
