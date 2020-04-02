using Ceres.Domain.Interfaces;
using Ceres.Domain.Models;
using Ceres.Infrastruct.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ceres.Infrastruct.Repository
{
    public class CustomerJobRepository : Repository<CustomerJob>, ICustomerJobRepository
    {
        public CustomerJobRepository(CeresContext context)
            : base(context)
        {

        }

        public CustomerJob GetCustomerJobByCustomerOid(Guid customerOid)
        {
            return DbSet.AsNoTracking().Where(c=>c.CustomerOid==customerOid).FirstOrDefault();
        }
    }
}
