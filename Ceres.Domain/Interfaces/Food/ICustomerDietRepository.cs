using Ceres.Domain.Models;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Interfaces
{
    public interface ICustomerDietRepository : IRepository<CustomerDiet>
    {
        public int QueryCustomerDietCountOnTheSameDayByCustomerOid(Guid customerOid,DateTime currentTime);
        public int QueryCustomerDietCountOnTheSameDayBySupporterOid(Guid supporterOid, DateTime currentTime);
        public CustomerDiet GetSupporterRecommendLastestDiet(Guid supporterOid);
        public CustomerDiet GetSupporterOperateLastestDiet(Guid lastOperaterOid);
        public int QueryDietListCount();
        public IEnumerable<CustomerDiet> QueryDietListByPage(int pageIndex, int pageSize);
        public int QueryCustomerDietListCount(Guid customerOid);
        public IEnumerable<CustomerDiet> QueryCustomerDietListByPage(Guid customerOid, int pageIndex, int pageSize);
        public IEnumerable<CustomerDiet> QueryCustomerDietListByPage(string customerName, string cellphone, Guid serviceOid, Guid supporterOid, DateTime startTime, DateTime endTime, int pageIndex, int pageSize);
        public int QueryCustomerDietListCount(string customerName, string cellphone, Guid serviceOid, Guid supporterOid, DateTime startTime, DateTime endTime);
    }
}
