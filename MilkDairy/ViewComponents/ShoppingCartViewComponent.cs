using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkDairy.Utility;
using MIlkDairy.DataAccess.Repository.IRepository;
using System.Security.Claims;
namespace MilkDairy.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfwork)
        {
            _unitOfWork = unitOfwork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimIdentityId = (ClaimsIdentity?)User.Identity;
            var claim = claimIdentityId.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                if (HttpContext.Session.GetInt32(SD.SessionCart) != null) { 
                HttpContext.Session.SetInt32(SD.SessionCart, 
                _unitOfWork.ShoppingCartRepo.GetAll(u => u.ApplicationUserID == claim.Value).Count());
                }
                return View(HttpContext.Session.GetInt32(SD.SessionCart));
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
