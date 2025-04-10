using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MilkDairy.DataAccess.Data;
using MilkDairy.Model;
using MilkDairy.Utility;

namespace MilkDairy.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _appDbContext;

        public DbInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext appDbContext
            ){
            _userManager = userManager;
            _roleManager = roleManager;
            _appDbContext = appDbContext;   
        }
        public void Initialize()
        {
            try
            {
                if (_appDbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _appDbContext.Database.Migrate();
                }
            }
            catch(Exception ex) { }

            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult()) // Check if roles exist
            {
                 _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Emplyoee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "AdminChintu@gmail.com",
                    Email = "ChintuDada@gmail.com",
                    Name = "Chintu Hero",
                    PhoneNumber = "1234567890",
                    Address = "Andheri nagri Pagal chock",
                    State = "Poha State",
                    Pincode = "143",
                    City = "Sapno ki duniya"
                }, "Admin123").GetAwaiter().GetResult();

                ApplicationUser user = _appDbContext.ApplicationUsers.FirstOrDefault(u => u.Email == "ChintuData@gmail.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }
            return;
        }
    }
}
