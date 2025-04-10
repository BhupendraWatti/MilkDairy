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
    public class subcategoiesMilkRepository : Repository<subcategoiesMilk>, IsubcategoiesMilkRepository
    {
        private AppDbContext _db;
        public subcategoiesMilkRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        
        public void Updatea(subcategoiesMilk obj)
        {
            _db.MilkSubTypes.Update(obj);
        }
    }
}
