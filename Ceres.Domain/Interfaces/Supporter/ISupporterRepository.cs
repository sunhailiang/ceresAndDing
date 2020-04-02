using Ceres.Domain.Models;
using System.Collections.Generic;

namespace Ceres.Domain.Interfaces
{
    public interface ISupporterRepository : IRepository<Supporter>
    {
        public Supporter GetByCellphone(string cellphone,string password);
        public Supporter GetByLoginName(string loginName, string password);
        public IEnumerable<Supporter> GetAllValidSupporters();
        public Supporter GetSupporterBySupporterName(string supporterName);

    }
}
