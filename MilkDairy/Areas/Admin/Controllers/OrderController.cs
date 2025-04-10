using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MilkDairy.Model.Models;
using MilkDairy.Utility;
using MIlkDairy.DataAccess.Repository.IRepository;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;
using System.Security.Claims;

namespace MilkDairy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderHeaderVM Orderheadervm { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Details(int orderId) {

            Orderheadervm = new()
            {
                orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                orderDetails = _unitOfWork.OrderDetail.GetAll(u => u.Id == orderId, includeProperties: "Product")
            };
            return View(Orderheadervm);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Emplyoee)]
        public IActionResult UpdateOrderDetails(int orderID)
        {
            var OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == Orderheadervm.orderHeader.Id);
            OrderHeader.Name = Orderheadervm.orderHeader.Name;
            OrderHeader.PhoneNumber = Orderheadervm.orderHeader.PhoneNumber;
            OrderHeader.StreetAddress = Orderheadervm.orderHeader.StreetAddress;
            OrderHeader.City = Orderheadervm.orderHeader.City;
            OrderHeader.State = Orderheadervm.orderHeader.State;
            OrderHeader.PostalCode = Orderheadervm.orderHeader.PostalCode;
            if (!string.IsNullOrEmpty(Orderheadervm.orderHeader.Carrier))
            {
                OrderHeader.Carrier = Orderheadervm.orderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(Orderheadervm.orderHeader.TrackingNumber)) {
                OrderHeader.TrackingNumber = Orderheadervm.orderHeader.TrackingNumber;
            }
            _unitOfWork.OrderHeader.Updatea(OrderHeader);
            _unitOfWork.Save();

            TempData["Success"] = "Update Successfuly";
            return RedirectToAction(nameof(Details), new { orderid = OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Emplyoee)]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStatus(Orderheadervm.orderHeader.Id, SD.StatusInProcess);
            _unitOfWork.Save();
            TempData["Success"] = "Update Successfuly";
            return RedirectToAction(nameof(Details), new { orderid = Orderheadervm.orderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Emplyoee)]
        public IActionResult ShipOrder()
        {
            var Orderheader = _unitOfWork.OrderHeader.Get(u => u.Id == Orderheadervm.orderHeader.Id);
            Orderheader.TrackingNumber = Orderheadervm.orderHeader.TrackingNumber;
            Orderheader.Carrier = Orderheadervm.orderHeader.Carrier;
            Orderheader.OrderStatus = SD.StatusShipped;
            Orderheader.ShippingDate = DateTime.Now;
            if (Orderheader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                Orderheader.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
            }
            _unitOfWork.OrderHeader.Updatea(Orderheader);
            _unitOfWork.Save();
            TempData["Success"] = "Shipped Successfuly";
            return RedirectToAction(nameof(Details), new { orderid = Orderheadervm.orderHeader.Id });
        }

        public IActionResult CancelOrder()
        {
            var Orderheader = _unitOfWork.OrderHeader.Get(u => u.Id == Orderheadervm.orderHeader.Id);
            if (Orderheader.PaymentStatus == SD.PaymentStatusApproved)
            {
                var option = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = Orderheader.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(option);
                _unitOfWork.OrderHeader.UpdateStatus(Orderheader.Id, SD.StatusCancelled, SD.StatusRefund);

            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(Orderheader.Id, SD.StatusCancelled, SD.StatusCancelled);
            }
            _unitOfWork.Save();

            TempData["Success"] = "Order Canclled Successfuly";
            return RedirectToAction(nameof(Details), new { orderid = Orderheadervm.orderHeader.Id });

        }

        public IActionResult DalyPayment() {

            Orderheadervm.orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == Orderheadervm.orderHeader.Id, includeProperties: "ApplicationUser");
            Orderheadervm.orderDetails = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == Orderheadervm.orderHeader.Id, includeProperties: "Product");
            var domain = "https://localhost:7150/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderheaderId={Orderheadervm.orderHeader.Id}",
                CancelUrl = domain + $"admin/order/details?orderid={Orderheadervm.orderHeader.Id}",
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                Mode = "payment",
            };
            foreach (var item in Orderheadervm.orderDetails)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
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
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdatestripePaymentID(Orderheadervm.orderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        public IActionResult PaymentConfirmation(int orderheaderId)
        {

            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderheaderId);
            if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdatestripePaymentID(orderheaderId, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(orderheaderId, orderHeader.OrderStatus, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }



            }
            return View(orderheaderId);
        }
             #region API call
            [HttpGet]
            public IActionResult GetAll(string status)
            {
                IEnumerable<OrderHeader> OHObj = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();

                if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Emplyoee))
                {
                    OHObj = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
                }
                else
                {
                    var claimIdentity = (ClaimsIdentity)User.Identity;
                    var UserId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                    OHObj = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == UserId, includeProperties: "ApplicationUser");
                }

                switch (status)
                {

                    case "process":
                        OHObj = OHObj.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
                        break;

                    case "pending":
                        OHObj = OHObj.Where(u => u.OrderStatus == SD.StatusInProcess);
                        break;

                    case "completed":
                        OHObj = OHObj.Where(u => u.OrderStatus == SD.StatusShipped);
                        break;
                    case "approved":
                        OHObj = OHObj.Where(u => u.OrderStatus == SD.StatusApproved);

                        break;
                    default:
                        break;

                }


                return Json(new { data = OHObj });
            }

            #endregion
        }
    }
