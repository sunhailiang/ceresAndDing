using Ceres.Domain.Models;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Interfaces
{
    public interface IServiceRepository : IRepository<Service>
    {
        public IEnumerable<Service> GetAllValidServices();
        public Service GetServiceByCustomerOid(Guid customerOid);

        public Service GetServiceByServiceName(string serviceName);
    }
}
