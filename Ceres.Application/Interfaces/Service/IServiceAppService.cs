using Ceres.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Ceres.Application.Interfaces
{
    public interface IServiceAppService : IDisposable
    {
        //获取所有的Service服务
        IEnumerable<GetServiceListResponse> GetServiceList();
    }
}
