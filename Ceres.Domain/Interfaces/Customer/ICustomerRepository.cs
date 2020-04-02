using Ceres.Domain.Models;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Interfaces
{
    public interface ICustomerRepository: IRepository<Customer>
    {
        public Customer GetByCellphone(string cellphone);

        public IEnumerable<Customer> QueryCustomerListByPage(int pageIndex,int pageSize);

        public IEnumerable<Customer> QueryCustomerListByPage(string userName, string cellphone, Guid serviceOid, Guid supporterOid, Guid agenterOid, int pageIndex, int pageSize);

        public int QueryCustomerListCount();
        public int QueryCustomerListCount(string userName, string cellphone, Guid serviceOid, Guid supporterOid, Guid agenterOid);
        public IEnumerable<Customer> QueryOneSupporterCustomerList(Guid supporterOid);
        public int QueryOneSupporterCustomerListCount(Guid supporterOid);
        public IEnumerable<Customer> QueryOneSupporterDoneDietListByDaily(Guid supporterOid, DateTime dateTime);
        public int QueryOneSupporterDoneDietListCountByDailyCount(Guid supporterOid, DateTime dateTime);
        public IEnumerable<Customer> QueryOneSupporterDoneDingListByDaily(Guid supporterOid,Guid firstQustionGuid, DateTime dateTime);

    }
}
