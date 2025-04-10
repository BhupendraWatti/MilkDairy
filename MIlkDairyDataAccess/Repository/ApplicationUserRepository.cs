using MilkDairy.DataAccess.Data;
using MilkDairy.DataAccess.Repository.IRepository;
using MilkDairy.Model;
using MilkDairy.Model.Models;
using MIlkDairy.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIlkDairy.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private AppDbContext _db;
        public ApplicationUserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Updatea(ApplicationUser obj)
        {
            _db.ApplicationUsers.Update(obj);
        }
    }
}
