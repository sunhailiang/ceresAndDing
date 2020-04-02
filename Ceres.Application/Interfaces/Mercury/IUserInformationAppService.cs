using Ceres.Application.ViewModels;
using System;
using System.Collections.Generic;


namespace Ceres.Application.Interfaces
{
    public interface IUserInformationAppService : IDisposable
    {
        //依据手机号查询未筛选的客户
        GetOneOriginalCustomerResponse GetUserByPhoneNumber(string cellphone);
    }
}
