using Ceres.Application.ViewModels;
using System;
using System.Collections.Generic;


namespace Ceres.Application.Interfaces
{
    public interface ISupporterAppService:IDisposable
    {
        //Supporter 手机号登陆
        SupporterLoginResponse SupporterCellphoneLogin(SupporterLoginRequest request);
        
        //Supporter 用户名登陆
        SupporterLoginResponse SupporterLoginNameLogin(SupporterLoginRequest request);

        //获取所有的Supporter
        IEnumerable<GetSupporterListResponse> GetSupporterList();

        //更新客服密码
        void UpdateSupporterPassword(UpdateSupporterPasswordRequest request);

        //分页获取指定客服在指定日期下的所有未完成的配餐代办事项
        GetOneSupporterTodoDietListByDailyPageResponse GetOneSupporterTodoDietListByDailyPage(Guid supporterOid, DateTime dateTime, int pageIndex, int pageSize);

        //获取指定客服在指定日期下的所有未完成的配餐总数量
        GetOneSupporterTodoDietListCountByDailyResponse GetOneSupporterTodoDietListCountByDaily(Guid supporterOid, DateTime dateTime);

        //分页获取指定客服在指定日期下的所有未完成的打卡代办事项
        GetOneSupporterTodoDingListByDailyPageResponse GetOneSupporterTodoDingListByDailyPage(Guid supporterOid, DateTime dateTime, int pageIndex, int pageSize);

        //获取指定客服在指定日期下的所有未完成的打卡总数量
        GetOneSupporterTodoDingListCountByDailyResponse GetOneSupporterTodoDingListCountByDaily(Guid supporterOid, DateTime dateTime);


    }
}
