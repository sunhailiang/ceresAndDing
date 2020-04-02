using Ceres.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Ceres.Application.Interfaces
{
    public interface IDietAppService : IDisposable
    {
        //新增一条食谱
        void CreateOneCustomerDiet(CreateOneCustomerDietRequest request);

        //删除一条食谱
        void DeleteOneCustomerDiet(DeleteOneCustomerDietRequest request);

        //条件分页查询指定客户的食谱列表
        QueryOneCustomerDietListByPageResponse QueryOneCustomerDietListByPage(Guid customerOid, int pageIndex, int pageSize);

        //条件分页查询所有食谱列表
        QueryDietListByPageResponse QueryDietListByPage(int pageIndex, int pageSize);

        //条件分页查询食谱列表
        QueryDietListByPageResponse QueryDietListByPage(string customerName, string cellphone, Guid serviceOid, Guid supporterOid, DateTime startTime, DateTime endTime, int pageIndex, int pageSize);
    }
}
