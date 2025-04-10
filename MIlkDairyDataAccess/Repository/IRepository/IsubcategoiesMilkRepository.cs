using MilkDairy.DataAccess.Repository.IRepository;
using MilkDairy.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIlkDairy.DataAccess.Repository.IRepository
{
    public interface IsubcategoiesMilkRepository : IRepository<subcategoiesMilk>
    {
        void Updatea(subcategoiesMilk obj);
       
    }
}
