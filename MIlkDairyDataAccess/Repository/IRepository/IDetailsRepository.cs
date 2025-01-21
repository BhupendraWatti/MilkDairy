using MilkDairy.DataAccess.Repository.IRepository;
using MilkDairy.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIlkDairy.DataAccess.Repository.IRepository
{
    public interface IDetailsRepository : IRepository<Details>
    {
        void Updatea(Details obj);
        void Save();
    }
}
