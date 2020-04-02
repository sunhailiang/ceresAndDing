using Ceres.Domain.Interfaces;
using Ceres.Domain.Models;
using Ceres.Infrastruct.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ceres.Infrastruct.Repository
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(CeresContext context)
            : base(context)
        {

        }

        public IEnumerable<Service> GetAllValidServices()
        {
            return DbSet.AsNoTracking().Where(c => c.Status == 0);
        }

        public Service GetServiceByCustomerOid(Guid customerOid)
        {
            var result = (from c in this.Db.CustomerService.AsNoTracking()
                          join s in this.Db.Service.AsNoTracking()
                          on c.ServiceOid equals s.OID
                          where c.CustomerOid == customerOid
                          select s).FirstOrDefault();
            return result;
        }

        public Service GetServiceByServiceName(string serviceName)
        {
            return DbSet.AsNoTracking().Where(c => c.Name == serviceName).FirstOrDefault();
        }
    }
}
