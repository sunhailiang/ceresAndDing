using Ceres.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Ceres.Application.Interfaces
{
    public interface IFoodAppService : IDisposable
    {
        //条件分页查询食材列表
        QueryFoodListByPageResponse QueryFoodListByPage(string foodName, float foodValue,int pageIndex, int pageSize);

        //条件分页获取所有有效的带客户是否喜欢标识的食材列表
        QueryFoodListWithDislikeFlagByPageResponse QueryFoodListWithDislikeFlagByPage(Guid oid, string foodName, int pageIndex, int pageSize);

        //分页获取指定客户不喜欢的食材列表
        QueryDislikeFoodListByPageResponse QueryDislikeFoodListByPage(Guid oid, int pageIndex, int pageSize);

        //新增多条客户不喜欢的食材列表
        void CreateOneCustomerDislikeFood(CreateOneCustomerDislikeFoodRequest request);

        //删除多条客户不喜欢的食材列表
        void DeleteOneCustomerDislikeFood(DeleteOneCustomerDislikeFoodRequest request);
    }
}
