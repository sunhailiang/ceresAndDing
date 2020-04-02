using Ceres.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Ceres.Application.Interfaces
{
    public interface IAnswerAppService : IDisposable
    {
        //获取指定客户的体重列表
        GetOneCustomerWeightListResponse GetOneCustomerWeightList(Guid oid,int dataCount);
        GetOneCustomerBMIListResponse GetOneCustomerBMIList(Guid oid, int dataCount);
        QueryOneCustomerPhysiqueListByPageResponse QueryOneCustomerPhysiqueListByPage(Guid oid, int pageIndex, int pageSize);
        GetOneCustomerPhysiqueResponse GetOneCustomerPhysique(Guid oid);
        GetOneCustomerDailyEnergyResponse GetOneCustomerDailyEnergy(Guid oid);

        //条件分页查询指定客户的打卡列表
        QueryOneCustomerDingListByPageResponse QueryOneCustomerDingListByPage(Guid customerOid, int pageIndex, int pageSize);

        //新增一个协助打卡
        void CreateOneCustomerAssistDing(CreateOneCustomerAssistDingRequest request);

        //新增一个打卡
        void CreateOneCustomerDing(CreateOneCustomerDingRequest request);

        //删除一个协助打卡
        void DeleteOneCustomerAssistDing(DeleteOneCustomerAssistDingRequest request);

        //查询指定客户的当天打卡信息
        GetOneCustomerTodayDingResponse GetOneCustomerTodayDing(Guid customerOid);

        //查询指定客户的微信中展示的体重列表
        GetOneCustomerWXWeightListResponse GetOneCustomerWXWeightList(Guid customerOid, int dataCount);

        //查询指定客户的当天体脂测试信息
        GetOneCustomerTodayPhysiqueResponse GetOneCustomerTodayPhysique(string cellphone);
    }
}
