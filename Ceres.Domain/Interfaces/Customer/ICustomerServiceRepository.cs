using Ceres.Domain.Models;
using System;

namespace Ceres.Domain.Interfaces
{
    public interface ICustomerServiceRepository:IRepository<CustomerService>
    {
        public CustomerService GetCustomerServiceByCustomerOid(Guid customerOid);
    }
}
