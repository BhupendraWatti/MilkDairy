using MilkDairy.DataAccess.Data;
using MIlkDairy.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIlkDairy.DataAccess.Repository
{
    public class UnitOfWorkRepository : IUnitOfWork
    {
        public AppDbContext _db;
        public IDetailsRepository DetailsRepo { get; private set; }
        public IProductRepository ProductRepo { get; private set; }
        public ICompanyRepository CompanyRepo { get; private set; }

        public IApplicationUserRepository applicationUserRepo { get; private set; }
        public IShoppingCartRepository ShoppingCartRepo { get; private set; }
        public IsubcategoiesMilkRepository milkProp { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public UnitOfWorkRepository( AppDbContext db)
        {
            _db = db;
            DetailsRepo = new DetailsRepository( _db );
            ProductRepo = new ProductRepository( _db );
            CompanyRepo = new CompanyRepository( _db );
            applicationUserRepo = new ApplicationUserRepository( _db ); 
            milkProp = new subcategoiesMilkRepository( _db );
            ShoppingCartRepo = new ShoppingCartRepository( _db );
            OrderDetail = new OrderDetailRepository( _db );
            OrderHeader = new OrderHeaderRepository( _db );
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
