using Ceres.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Ceres.Application.Interfaces
{
    public interface IAgenterAppService : IDisposable
    {
        //获取所有的,有效的 Agenter
        IEnumerable<GetAgenterListResponse> GetAgenterList();

        //依据代理名称查询代理列表
        IEnumerable<GetAgenterListResponse> QueryAgenterList(string agenterName);
    }
}
