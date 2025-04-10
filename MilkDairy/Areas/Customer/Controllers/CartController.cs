using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkDairy.Model;
using MilkDairy.Model.Models;
using MilkDairy.Utility;
using MIlkDairy.DataAccess.Repository.IRepository;
using Stripe.Checkout;
using System.Security.Claims;
using System.Transactions;

namespace MilkDairy.Areas.Customer.Controllers
{
        [Area("Customer")]
        [Authorize]
    public class CartController : Controller
    {
        private IUnitOfWork _unitofwork;
        private readonly string userId;

        [BindProperty]
        public ShoppingCartTotal  _shoppingCartTotal { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;

        }
        public IActionResult Index()
        {
            var claimIdentityId = (ClaimsIdentity?)User.Identity;
            var userID = claimIdentityId.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCartData = _unitofwork.ShoppingCartRepo.GetAll(u => u.ApplicationUserID == userID, includeProperties: "Product");
            _shoppingCartTotal = new()
            {
                shoppingCartList = _unitofwork.ShoppingCartRepo
    .GetAll(u => u.ApplicationUserID == userID, includeProperties: "Product").ToList(),
                OrderHeader = new()
            };
            foreach (var cart in _shoppingCartTotal.shoppingCartList) { 
                cart.price = GetDiscountBasedOnCount(cart);
                _shoppingCartTotal.OrderHeader.OrderTotal += (cart.price * cart.Count);
            }
            

            return View(_shoppingCartTotal);
        }
        private double GetDiscountBasedOnCount(ShoppingCart shoppingCart)

        {
            if (shoppingCart == null || shoppingCart.Product == null)
            {
                return 0; 
            }
            double discount = 0, totalPrice = 0, finalPrice = 0;
            if (shoppingCart.Count <= 5)
            {
                discount = 5;
            }
            else if ( shoppingCart.Count <= 10) 
            {
                discount = 10;
            }
            else if ( shoppingCart.Count <= 15) 
            {
                discount = 15;
            }
            else if (shoppingCart.Count <= 20)
            {
                discount = 20;
            }
            else
            {
                discount = 30;
            }
            totalPrice = shoppingCart.Product.UnitPrice;
            finalPrice = totalPrice * (1 - discount / 100);
            return finalPrice;
        }
        public IActionResult Plus(int cart)
        {
            var cartFromDb = _unitofwork.ShoppingCartRepo.Get(u => u.ID == cart);
            cartFromDb.Count+=1;
            _unitofwork.ShoppingCartRepo.Update(cartFromDb);
            _unitofwork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult minus(int cart)
        {
            var cartFromDb = _unitofwork.ShoppingCartRepo.Get(u=>u.ID == cart, tracked: true);
           
            if (cartFromDb.Count <= 1)
            {
                HttpContext.Session.SetInt32(SD.SessionCart, _unitofwork.ShoppingCartRepo.GetAll(u=>u.ApplicationUserID == cartFromDb.ApplicationUserID).Count()-1);
                _unitofwork.ShoppingCartRepo.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count-=1;
                _unitofwork.ShoppingCartRepo.Update(cartFromDb);
            }
            _unitofwork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult remove(int cart)
        {
            var cartFromDb = _unitofwork.ShoppingCartRepo.Get(u=>u.ID ==cart, tracked: true);
            HttpContext.Session.SetInt32(SD.SessionCart, _unitofwork.ShoppingCartRepo.GetAll(u => u.ApplicationUserID == cartFromDb.ApplicationUserID).Count() - 1);
            _unitofwork.ShoppingCartRepo.Remove(cartFromDb);
            _unitofwork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Checkout()
        {
            var claimIdentityId = (ClaimsIdentity?)User.Identity;
            var userID = claimIdentityId.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCartData = _unitofwork.ShoppingCartRepo.GetAll(u => u.ApplicationUserID == userID, includeProperties: "Product");
            _shoppingCartTotal = new()
            {
                shoppingCartList = _unitofwork.ShoppingCartRepo.GetAll(u => u.ApplicationUserID == userID, includeProperties: "Product").ToList(),
                OrderHeader = new()
            };
            _shoppingCartTotal.OrderHeader.ApplicationUser = _unitofwork.applicationUserRepo.Get(u => u.Id == userID);
            _shoppingCartTotal.OrderHeader.Name = _shoppingCartTotal.OrderHeader.ApplicationUser.Name;
            _shoppingCartTotal.OrderHeader.PhoneNumber = _shoppingCartTotal.OrderHeader.ApplicationUser.PhoneNumber;
            _shoppingCartTotal.OrderHeader.StreetAddress = _shoppingCartTotal.OrderHeader.ApplicationUser.Address;
            _shoppingCartTotal.OrderHeader.City = _shoppingCartTotal.OrderHeader.ApplicationUser.City;
            _shoppingCartTotal.OrderHeader.State = _shoppingCartTotal.OrderHeader.ApplicationUser.State;
            _shoppingCartTotal.OrderHeader.PostalCode = _shoppingCartTotal.OrderHeader.ApplicationUser.Pincode;

            foreach (var cart in _shoppingCartTotal.shoppingCartList)
            {
                cart.price = GetDiscountBasedOnCount(cart);
                _shoppingCartTotal.OrderHeader.OrderTotal += (cart.price * cart.Count);
            }
            return View(_shoppingCartTotal);
        }
        
        [HttpPost]
        [ActionName("Checkout")]
        public IActionResult CheckoutPost()
        {
            var claimIdentityId = (ClaimsIdentity?)User.Identity;
            var userID = claimIdentityId.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCartData = _unitofwork.ShoppingCartRepo.GetAll(u => u.ApplicationUserID == userID, includeProperties: "Product");
            _shoppingCartTotal.shoppingCartList = _unitofwork.ShoppingCartRepo
    .GetAll(u => u.ApplicationUserID == userID, includeProperties: "Product").ToList();

            _shoppingCartTotal.OrderHeader.OrderDate = System.DateTime.Now;
            _shoppingCartTotal.OrderHeader.ApplicationUserId = userID;

            ApplicationUser applicationUser = _unitofwork.applicationUserRepo.Get(u=>u.Id == userID);
            foreach (var cart in _shoppingCartTotal.shoppingCartList)
            {
                cart.price = GetDiscountBasedOnCount(cart);
                _shoppingCartTotal.OrderHeader.OrderTotal += (cart.price * cart.Count);
            }

            if(applicationUser.CompanyID.GetValueOrDefault() == 0)
            {
                _shoppingCartTotal.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                _shoppingCartTotal.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                _shoppingCartTotal.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                _shoppingCartTotal.OrderHeader.OrderStatus = SD.StatusApproved;
            }

            _unitofwork.OrderHeader.Add(_shoppingCartTotal.OrderHeader);
            _unitofwork.Save();

            foreach(var cart in _shoppingCartTotal.shoppingCartList)
            {
                OrderDetail orderDetail = new() {
                ProductId = cart.ProductID,
                OrderHeaderId = _shoppingCartTotal.OrderHeader.Id,
                Price = cart.price,
                Count = cart.Count,
                };
                _unitofwork.OrderDetail.Add(orderDetail);
                _unitofwork.Save();

            }

            if (applicationUser.CompanyID.GetValueOrDefault() == 0) {
                var domain = "https://localhost:7150/";
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = domain+ $"customer/cart/OrderConfirmation?id={_shoppingCartTotal.OrderHeader.Id}",
                    CancelUrl = domain+"customer/cart/index",
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                    Mode = "payment",
                };
                foreach (var item in _shoppingCartTotal.shoppingCartList)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                    
                }


                var service = new SessionService();
                Session session =  service.Create(options);
                _unitofwork.OrderHeader.UpdatestripePaymentID(_shoppingCartTotal.OrderHeader.Id, session.Id, session.PaymentIntentId);
                _unitofwork.Save();

                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }

            return RedirectToAction(nameof(OrderConfirmation), new { id = _shoppingCartTotal.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {

            OrderHeader orderHeader = _unitofwork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
            if(orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);  
                if(session.PaymentStatus.ToLower() == "paid")
                {
                    _unitofwork.OrderHeader.UpdatestripePaymentID(id, session.Id, session.PaymentIntentId);
                    _unitofwork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitofwork.Save();
                }
                HttpContext.Session.Clear();
            }

            List<ShoppingCart> shoppingCarts= _unitofwork.ShoppingCartRepo.GetAll(u=>u.ApplicationUserID == orderHeader.ApplicationUserId).ToList();

            _unitofwork.ShoppingCartRepo.RemoveRange(shoppingCarts);
            _unitofwork.Save();
            return View(id);
        }
    }
}
