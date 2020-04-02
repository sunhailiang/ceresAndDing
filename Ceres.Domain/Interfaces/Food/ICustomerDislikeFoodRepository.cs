using Ceres.Domain.Models;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Interfaces
{
    public interface ICustomerDislikeFoodRepository : IRepository<CustomerDislikeFood>
    {
        public IEnumerable<CustomerDislikeFood> GetCustomerDislikeFoodByCustomerOid(Guid customerOid);

        public CustomerDislikeFood GetCustomerDislikeFood(Guid customerOid,Guid foodOid);

        public IEnumerable<CustomerDislikeFood> QueryCustomerDislikeFoodListByPage(Guid customerOid,int pageIndex,int pageSize);
        public int QueryCustomerDislikeFoodCount(Guid customerOid);

        public CustomerDislikeFood GetSupporterOperateLastestDislikeFood(Guid operaterOid);
    }
}