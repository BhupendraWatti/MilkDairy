using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MilkDairy.Model;
using MilkDairy.Model.Models;
using Microsoft.AspNetCore.Identity;

namespace MilkDairy.DataAccess.Data
{

    public class AppDbContext : IdentityDbContext<ApplicationUser> 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Details> Details { get; set; }
        public DbSet<Product> Product{ get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<subcategoiesMilk> MilkSubTypes {get; set; }

        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Details>().HasData(
                new Details { ID = 1, Items = "Milk", MFD = new DateTime(2024, 02, 24), EXD = new DateTime(2024, 02, 25), Price = 33,Quantity = 40 },
                new Details { ID = 2, Items = "Paneer", MFD = new DateTime(2024, 02, 24), EXD = new DateTime(2024, 02, 27), Price = 600, Quantity = 10},
                new Details { ID = 3, Items = "Curd", MFD = new DateTime(2024, 02, 24), EXD = new DateTime(2024, 02, 27), Price = 90, Quantity = 20},
                new Details { ID = 4, Items = "Ghee", MFD = new DateTime(2024, 02, 24), EXD = new DateTime(2024, 06, 24), Price = 146, Quantity= 10 }
                );

            modelBuilder.Entity<Product>().HasData(
                 new Product
                 {
                     Id = 1,
                     Name = "Dairy Milk Silk",
                     Description = "Smooth, creamy milk chocolate.",
                     Brand = "Cadbury",
                     WeightVolume = "150g",
                     Ingredients = "Milk Solids, Sugar, Cocoa Butter, Cocoa Mass, Emulsifiers (Soy Lecithin, 476), Flavours.",
                     UnitPrice = 150.00f,
                     UnitsInStock = 100,
                     IsAvailable = true,
                     ImgURL=""
                    
                 },
            new Product
            {
                Id = 2,
                Name = "Dairy Milk Fruit & Nut",
                Description = "Milk chocolate with raisins and almonds.",
                Brand = "Cadbury",
                WeightVolume = "160g",
                Ingredients = "Milk Solids, Sugar, Cocoa Butter, Cocoa Mass, Raisins (12%), Almonds (8%), Emulsifiers (Soy Lecithin, 476), Flavours.",
                UnitPrice = 170.00f,
                UnitsInStock = 75,
                IsAvailable = true,
                ImgURL = ""

            },
            new Product
            {
                Id = 3,
                Name = "Dairy Milk Roast Almond",
                Description = "Milk chocolate with roasted almonds.",
                Brand = "Amul",
                WeightVolume = "150g",
                Ingredients = "Milk Solids, Sugar, Cocoa Butter, Cocoa Mass, Almonds (15%), Emulsifiers (Soy Lecithin, 476), Flavours.",
                UnitPrice = 160.00f,
                UnitsInStock = 50,
                IsAvailable = false,
                ImgURL = ""

            },
            new Product
            {
                Id = 4,
                Name = "Dairy Milk Crackle",
                Description = "Milk chocolate with rice crisps.",
                Brand = "Cadbury",
                WeightVolume = "140g",
                Ingredients = "Milk Solids, Sugar, Cocoa Butter, Cocoa Mass, Rice Crisps (10%), Emulsifiers (Soy Lecithin, 476), Flavours.",
                UnitPrice = 140.00f,
                UnitsInStock = 120,
                IsAvailable = true,
                ImgURL = ""

            },
            new Product
            {
                Id = 5,
                Name = "Dairy Milk Silk Bubbly",
                Description = "Aerated milk chocolate for a light, bubbly texture.",
                Brand = "cadbury",
                WeightVolume = "120g",
                Ingredients = "Milk Solids, Sugar, Cocoa Butter, Cocoa Mass, Emulsifiers (Soy Lecithin, 476), Flavours.",
                UnitPrice = 130.00f,
                UnitsInStock = 90,
                IsAvailable = true,
                ImgURL = ""

            },
            new Product
            {
                Id = 6,
                Name = "Dairy Milk Lickables",
                Description = "Milk chocolate with a toy inside.",
                Brand = "Amul",
                WeightVolume = "30g",
                Ingredients = "Milk Solids, Sugar, Cocoa Butter, Cocoa Mass, Emulsifiers (Soy Lecithin, 476), Flavours.",
                UnitPrice = 50.00f,
                UnitsInStock = 200,
                IsAvailable = true,
                ImgURL = "L:/TeraBoxDownload/C#/Advances C# projects/MilkDairy/wwwroot/images/Product/0dca2d5c-dcba-4980-a8ce-fa2b30ed6d36_.jpg"

            }
            );
            modelBuilder.Entity<ShoppingCart>()
            .HasOne(s => s.Product).WithMany().HasForeignKey(s => s.ProductID).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
