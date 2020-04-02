using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ceres.Application.Interfaces;
using Ceres.Application.ViewModels;
using Ceres.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ceres.WebApi.Controllers.Food
{
    /// <summary>
    /// 配餐
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CompoundFoodController : ControllerBase
    {
        private readonly ICompoundFoodAppService _compoundFoodAppService;
        // 将领域通知处理程序注入Controller
        private readonly DomainNotificationHandler _notifications;

        public CompoundFoodController(ICompoundFoodAppService compoundFoodAppService,
            INotificationHandler<DomainNotification> notifications)
        {
            _compoundFoodAppService = compoundFoodAppService;
            // 强类型转换
            _notifications = (DomainNotificationHandler)notifications;
        }

        /// <summary>
        /// 获取每日3大营养素百分比
        /// </summary>
        /// <returns></returns>
        [HttpGet("ComponentPercentage")]
        public WebApiResultEntity<GetComponentPercentageByDailyEnergyResponse> GetComponentPercentage()
        {
            WebApiResultEntity<GetComponentPercentageByDailyEnergyResponse> result;
            try
            {
                var models = _compoundFoodAppService.GetComponentPercentageByDailyEnergy();

                if (models == null)
                {
                    result = new WebApiResultEntity<GetComponentPercentageByDailyEnergyResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.ComponentList.Count() <= 0)
                {
                    result = new WebApiResultEntity<GetComponentPercentageByDailyEnergyResponse> { success = false, message = "暂不存在3大营养素百分比计算方法" };
                    return result;
                }

                result = new WebApiResultEntity<GetComponentPercentageByDailyEnergyResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<GetComponentPercentageByDailyEnergyResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 获取每日九宫格g数据，343方式分配
        /// </summary>
        /// <param name="dailyEnergy">每日理想体重所需热量数（kcal/kg）</param>
        /// <param name="pNameCode">蛋白质</param>
        /// <param name="pValue">蛋白质百分比数值，不包括%</param>
        /// <param name="fNameCode">脂肪</param>
        /// <param name="fValue">脂肪百分比数值，不包括%</param>
        /// <param name="cNameCode">碳水化合物</param>
        /// <param name="cValue">碳水化合物百分比数值，不包括%</param>
        /// <returns></returns>
        [HttpGet("DailyFoodComponent/{dailyEnergy}")]
        public WebApiResultEntity<GetDailyFoodComponentResponse> GetDailyFoodComponent(float dailyEnergy,string pNameCode="P",float pValue=14,string fNameCode="F",float fValue=27, string cNameCode = "C", float cValue = 59)
        {
            WebApiResultEntity<GetDailyFoodComponentResponse> result;
            try
            {
                if ((pValue + fValue + cValue) != 100 || pValue <= 0 || fValue <= 0 || cValue <= 0)
                {
                    result = new WebApiResultEntity<GetDailyFoodComponentResponse> { success = false, message = "每日3大营养素百分比出现错误" };
                    return result;
                }

                GetDailyFoodComponentResponse models = _compoundFoodAppService.GetDailyFoodComponent(dailyEnergy, pNameCode, pValue, fNameCode, fValue, cNameCode, cValue);
                if (models == null)
                {
                    result = new WebApiResultEntity<GetDailyFoodComponentResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.MealList==null||models.MealList.Count() <= 0)
                {
                    result = new WebApiResultEntity<GetDailyFoodComponentResponse> { success = false, message = "暂不存在九宫格计算方法" };
                    return result;
                }

                result = new WebApiResultEntity<GetDailyFoodComponentResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<GetDailyFoodComponentResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 获取指定客户的推荐食材
        /// </summary>
        /// <param name="oid">客户OID</param>
        /// <param name="nameCode">营养成分简称</param>
        /// <param name="componentValue">营养成分数值</param>
        /// <param name="dataCount">获取数量，单类型食材数量</param>
        /// <returns></returns>
        [HttpGet("Recommend/{oid}/{nameCode}/{componentValue}")]
        public WebApiResultEntity<GetRecommendFoodResponse> GetRecommendFood(Guid oid,string nameCode,float componentValue,int dataCount = 10)
        {
            WebApiResultEntity<GetRecommendFoodResponse> result;
            try
            {
                if (dataCount <= 0)
                {
                    result = new WebApiResultEntity<GetRecommendFoodResponse> { success = false, message = "参数出现错误" };
                    return result;
                }

                GetRecommendFoodResponse models = _compoundFoodAppService.GetRecommendFood(oid,nameCode,componentValue,dataCount);
                if (models == null)
                {
                    result = new WebApiResultEntity<GetRecommendFoodResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.ClassifyFoodList == null || models.ClassifyFoodList.Count() <= 0)
                {
                    result = new WebApiResultEntity<GetRecommendFoodResponse> { success = false, message = "暂不存在推荐食材" };
                    return result;
                }

                result = new WebApiResultEntity<GetRecommendFoodResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<GetRecommendFoodResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

    }
}