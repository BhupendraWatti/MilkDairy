using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using MilkDairy.DataAccess.Data;
using MilkDairy.Model.Models;
using MilkDairy.Utility;
using MIlkDairy.DataAccess.Repository.IRepository;
using System.Diagnostics;
using System.Security.Claims;

namespace MilkDairy.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            
            IEnumerable<Product> products = _unitOfWork.ProductRepo.GetAll();
            return View(products);
        }
        public IActionResult Details(int productID)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.ProductRepo.Get(u => u.Id == productID),
                Count = 1,
                ProductID = productID
            };
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart cartItem)
        {
            var claimIdentityId = (ClaimsIdentity?)User.Identity;
            var userID = claimIdentityId.FindFirst(ClaimTypes.NameIdentifier).Value;  
            cartItem.ApplicationUserID = userID;

            ShoppingCart shoppingcartFromDb = _unitOfWork.ShoppingCartRepo.Get(u=>u.ApplicationUserID == userID && u.ProductID == cartItem.ProductID);

            if (shoppingcartFromDb != null)
            {
                shoppingcartFromDb.Count += cartItem.Count;
                _unitOfWork.ShoppingCartRepo.Update(shoppingcartFromDb);
                _unitOfWork.Save();
            }
            else
            {
                _unitOfWork.ShoppingCartRepo.Add(cartItem);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCartRepo.GetAll(u => u.ApplicationUserID == userID).Count());
            }
            
            return RedirectToAction(nameof(Index));
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
