using Ceres.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Ceres.Application.Interfaces
{
    public interface ICompoundFoodAppService : IDisposable
    {
        //获取三大营养素百分比
        GetComponentPercentageByDailyEnergyResponse GetComponentPercentageByDailyEnergy();

        //获取每日九宫格g数据
        GetDailyFoodComponentResponse GetDailyFoodComponent(float dailyEnergy, string pNameCode, float pValue, string fNameCode, float fValue, string cNameCode, float cValue);

        //获取指定客户的推荐食材
        GetRecommendFoodResponse GetRecommendFood(Guid oid, string nameCode, float componentValue, int dataCount);
    }
}
