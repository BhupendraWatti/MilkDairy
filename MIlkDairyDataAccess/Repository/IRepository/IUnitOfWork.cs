using MIlkDairy.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIlkDairy.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IDetailsRepository DetailsRepo { get; }
        ICompanyRepository CompanyRepo { get; }
        IProductRepository ProductRepo { get; } 
        IShoppingCartRepository ShoppingCartRepo { get; }
        IApplicationUserRepository applicationUserRepo { get; }
        IsubcategoiesMilkRepository milkProp {get;}
        IOrderDetailRepository OrderDetail { get; }
        IOrderHeaderRepository OrderHeader { get; }

        void Save();
    }
}
