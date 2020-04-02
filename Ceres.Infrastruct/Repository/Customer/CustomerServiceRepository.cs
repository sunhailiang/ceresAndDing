using Ceres.Domain.Interfaces;
using Ceres.Domain.Models;
using Ceres.Infrastruct.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Ceres.Infrastruct.Repository
{
    public class CustomerServiceRepository : Repository<CustomerService>, ICustomerServiceRepository
    {
        public CustomerServiceRepository(CeresContext context)
                : base(context)
        {

        }

        public CustomerService GetCustomerServiceByCustomerOid(Guid customerOid)
        {
            return DbSet.AsNoTracking().Where(c => c.CustomerOid == customerOid).FirstOrDefault();
        }
    }
}
