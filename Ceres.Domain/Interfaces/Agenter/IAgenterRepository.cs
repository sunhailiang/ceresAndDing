using Ceres.Domain.Models;
using System.Collections.Generic;

namespace Ceres.Domain.Interfaces
{
    public interface IAgenterRepository : IRepository<Agenter>
    {
        public IEnumerable<Agenter> GetAllValidAgenters();

        public IEnumerable<Agenter> QueryAgenterList(string agenterName);
        public Agenter GetAgenterByAgenterName(string agenterName);
    }
}
