using Ceres.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Ceres.Application.Interfaces
{
    public interface ICustomerAppService : IDisposable
    {
        //新增一个VIP客户
        void CreateOneCustomer(CreateOneCustomerRequest request);

        //分页查询客户列表
        QueryCustomerListByPageResponse QueryCustomerListByPage(int pageIndex, int pageSize);

        //条件分页查询客户列表
        QueryCustomerListByPageResponse QueryCustomerListByPage(string customerName, string cellphone, Guid serviceOid, Guid supporterOid, Guid agenterOid, int pageIndex, int pageSize);

        //查询指定用户的基本信息
        GetOneCustomerBasicInformationResponse GetOneCustomerBasicInformation(Guid oid);

        //查询一个客户是否是VIP
        GetOneVIPCustomerResponse GetOneVIPCustomer(string cellphone);
    }
}
