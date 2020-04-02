using Ceres.Domain.Interfaces;
using Ceres.Domain.Models;
using Ceres.Infrastruct.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ceres.Infrastruct.Repository
{
    public class AgenterRepository : Repository<Agenter>, IAgenterRepository
    {
        public AgenterRepository(CeresContext context)
            : base(context)
        {

        }

        public IEnumerable<Agenter> GetAllValidAgenters()
        {
            return DbSet.AsNoTracking().Where(c => c.Status == 0);
        }

        public Agenter GetAgenterByAgenterName(string agenterName)
        {
            return DbSet.AsNoTracking().Where(c => c.UserName == agenterName).FirstOrDefault();
        }

        public IEnumerable<Agenter> QueryAgenterList(string agenterName)
        {
            return DbSet.AsNoTracking().Where(c => c.UserName.Contains(agenterName) && c.Status == 0);
        }
    }
}
