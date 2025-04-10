using MilkDairy.DataAccess.Data;
using MilkDairy.DataAccess.Repository.IRepository;
using MilkDairy.Model.Models;
using MIlkDairy.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIlkDairy.DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private AppDbContext _db;
        public OrderDetailRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Updatea(OrderDetail obj)
        {
            _db.Update(obj);
        }
    }
}
