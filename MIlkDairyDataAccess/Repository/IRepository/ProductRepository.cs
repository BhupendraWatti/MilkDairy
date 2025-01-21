using MilkDairy.DataAccess.Data;
using MilkDairy.DataAccess.Repository.IRepository;
using MilkDairy.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIlkDairy.DataAccess.Repository.IRepository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private AppDbContext _db;
        public ProductRepository(AppDbContext db) : base(db) 
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Updatea(Product obj)
        {
            _db.Product.Update(obj);
        }
    }
}
