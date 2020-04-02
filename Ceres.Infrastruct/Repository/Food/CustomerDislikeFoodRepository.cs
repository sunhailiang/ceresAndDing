using Ceres.Domain.Interfaces;
using Ceres.Domain.Models;
using Ceres.Infrastruct.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ceres.Infrastruct.Repository
{
    public class CustomerDislikeFoodRepository : Repository<CustomerDislikeFood>, ICustomerDislikeFoodRepository
    {
        public CustomerDislikeFoodRepository(CeresContext context)
            : base(context)
        {

        }

        public CustomerDislikeFood GetCustomerDislikeFood(Guid customerOid, Guid foodOid)
        {
            return DbSet.AsNoTracking().Where(c => c.CustomerOid == customerOid && c.FoodOid == foodOid).FirstOrDefault() ;
        }

        public IEnumerable<CustomerDislikeFood> GetCustomerDislikeFoodByCustomerOid(Guid customerOid)
        {
            return DbSet.AsNoTracking().Where(c => c.CustomerOid == customerOid);
        }

        public IEnumerable<CustomerDislikeFood> QueryCustomerDislikeFoodListByPage(Guid customerOid, int pageIndex, int pageSize)
        {
            return DbSet.AsNoTracking().Where(c => c.CustomerOid == customerOid).OrderByDescending(c => c.CreateTime).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int QueryCustomerDislikeFoodCount(Guid customerOid)
        {
            return DbSet.AsNoTracking().Where(c => c.CustomerOid == customerOid).Count();
        }

        public CustomerDislikeFood GetSupporterOperateLastestDislikeFood(Guid operaterOid)
        {
            return DbSet.AsNoTracking().Where(c => c.OperaterOid == operaterOid).OrderByDescending(c => c.CreateTime).FirstOrDefault(); ;
        }
    }
}
