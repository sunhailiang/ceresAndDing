using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ceres.Application.Interfaces;
using Ceres.Application.ViewModels;
using Ceres.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ceres.WebApi.Controllers.Food
{
    /// <summary>
    /// 食材列表
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FoodListController : ControllerBase
    {
        private readonly IFoodAppService _foodAppService;
        // 将领域通知处理程序注入Controller
        private readonly DomainNotificationHandler _notifications;

        public FoodListController(IFoodAppService foodAppService,
            INotificationHandler<DomainNotification> notifications)
        {
            _foodAppService = foodAppService;
            // 强类型转换
            _notifications = (DomainNotificationHandler)notifications;
        }

        /// <summary>
        /// 条件分页获取所有有效的食材列表
        /// </summary>
        /// <param name="foodName">食材名称</param>
        /// <param name="foodValue">食材数值</param>
        /// <param name="pageIndex">页码索引，从1开始</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        [HttpGet("Condition")]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<QueryFoodListByPageResponse> Get(string foodName, float foodValue, int pageIndex = 1, int pageSize = 10)
        {
            WebApiResultEntity<QueryFoodListByPageResponse> result;
            try
            {
                var models = _foodAppService.QueryFoodListByPage(foodName, foodValue, pageIndex - 1, pageSize);

                if (models == null)
                {
                    result = new WebApiResultEntity<QueryFoodListByPageResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.FoodList.Data.Count() <= 0)
                {
                    result = new WebApiResultEntity<QueryFoodListByPageResponse> { success = false, message = "暂不存在食材" };
                    return result;
                }

                result = new WebApiResultEntity<QueryFoodListByPageResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<QueryFoodListByPageResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 条件分页获取所有有效的带客户是否喜欢标识的食材列表
        /// </summary>
        /// <param name="oid">客户OID</param>
        /// <param name="foodName">食材名称</param>
        /// <param name="pageIndex">页码索引，从1开始</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        [HttpGet("Condition/{oid}/{foodName}")]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<QueryFoodListWithDislikeFlagByPageResponse> Get(Guid oid, string foodName,int pageIndex = 1, int pageSize = 10)
        {
            WebApiResultEntity<QueryFoodListWithDislikeFlagByPageResponse> result;
            try
            {
                var models = _foodAppService.QueryFoodListWithDislikeFlagByPage(oid,foodName, pageIndex - 1, pageSize);

                if (models == null)
                {
                    result = new WebApiResultEntity<QueryFoodListWithDislikeFlagByPageResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.FoodList.Data.Count() <= 0)
                {
                    result = new WebApiResultEntity<QueryFoodListWithDislikeFlagByPageResponse> { success = false, message = "暂不存在食材" };
                    return result;
                }

                result = new WebApiResultEntity<QueryFoodListWithDislikeFlagByPageResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<QueryFoodListWithDislikeFlagByPageResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 分页获取指定客户不喜欢的食材列表
        /// </summary>
        /// <param name="oid">客户OID</param>
        /// <param name="pageIndex">页码索引，从1开始</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        [HttpGet("Dislike/{oid}")]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<QueryDislikeFoodListByPageResponse> GetDislikeFood(Guid oid, int pageIndex = 1, int pageSize = 10)
        {
            WebApiResultEntity<QueryDislikeFoodListByPageResponse> result;
            try
            {
                var models = _foodAppService.QueryDislikeFoodListByPage(oid, pageIndex - 1, pageSize);

                if (models == null)
                {
                    result = new WebApiResultEntity<QueryDislikeFoodListByPageResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.DislikeFoodList.Data.Count() <= 0)
                {
                    result = new WebApiResultEntity<QueryDislikeFoodListByPageResponse> { success = false, message = "暂不存在食材" };
                    return result;
                }

                result = new WebApiResultEntity<QueryDislikeFoodListByPageResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<QueryDislikeFoodListByPageResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 新增多条客户不喜欢的食材列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Dislike")]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<string> PostDiet([FromBody] CreateOneCustomerDislikeFoodRequest request)
        {
            WebApiResultEntity<string> result;
            try
            {
                _foodAppService.CreateOneCustomerDislikeFood(request);

                // 是否存在消息通知
                if (_notifications.HasNotifications())
                {
                    // 从通知处理程序中，获取全部通知信息，并返回给response
                    var notificacoes = _notifications.GetNotifications();
                    StringBuilder sb = new StringBuilder();
                    notificacoes.ForEach(c => sb.Append(c.Value));
                    return result = new WebApiResultEntity<string> { success = false, message = "新增不喜欢食材失败", response = sb.ToString() };
                }

                result = new WebApiResultEntity<string> { success = true, message = "新增不喜欢食材成功", response = "新增不喜欢食材成功！" };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<string> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 删除多条客户不喜欢的食材列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Dislike/Delete")]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<string> PostDiet([FromBody] DeleteOneCustomerDislikeFoodRequest request)
        {
            WebApiResultEntity<string> result;
            try
            {
                _foodAppService.DeleteOneCustomerDislikeFood(request);

                // 是否存在消息通知
                if (_notifications.HasNotifications())
                {
                    // 从通知处理程序中，获取全部通知信息，并返回给response
                    var notificacoes = _notifications.GetNotifications();
                    StringBuilder sb = new StringBuilder();
                    notificacoes.ForEach(c => sb.Append(c.Value));
                    return result = new WebApiResultEntity<string> { success = false, message = "删除不喜欢食材失败", response = sb.ToString() };
                }

                result = new WebApiResultEntity<string> { success = true, message = "删除不喜欢食材成功", response = "删除不喜欢食材成功！" };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<string> { success = false, message = "系统错误" };
            }
            return result;
        }
    }
}