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

namespace Ceres.WebApi.Controllers
{
    /// <summary>
    /// 客服
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SupporterController : ControllerBase
    {
        private readonly ISupporterAppService _supporterAppService;
        // 将领域通知处理程序注入Controller
        private readonly DomainNotificationHandler _notifications;

        public SupporterController(ISupporterAppService supporterAppService,
            INotificationHandler<DomainNotification> notifications)
        {
            _supporterAppService = supporterAppService;
            // 强类型转换
            _notifications = (DomainNotificationHandler)notifications;
        }

        /// <summary>
        /// 分页获取指定客服在指定日期下的所有未完成的配餐代办事项
        /// </summary>
        /// <param name="supporterOid">客服OID</param>
        /// <param name="dateTime">指定日期</param>
        /// <param name="pageIndex">页码索引，从1开始</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        [HttpGet("TodoDietList/{supporterOid}/{dateTime}")]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<GetOneSupporterTodoDietListByDailyPageResponse> GetTodoDietList(Guid supporterOid,DateTime dateTime, int pageIndex = 1, int pageSize = 10)
        {
            WebApiResultEntity<GetOneSupporterTodoDietListByDailyPageResponse> result;
            try
            {
                var models = _supporterAppService.GetOneSupporterTodoDietListByDailyPage(supporterOid, dateTime, pageIndex - 1, pageSize);

                if (models == null)
                {
                    result = new WebApiResultEntity<GetOneSupporterTodoDietListByDailyPageResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.TodoDietList.Data.Count() <= 0)
                {
                    result = new WebApiResultEntity<GetOneSupporterTodoDietListByDailyPageResponse> { success = false, message = "暂不存在配餐代办事项" };
                    return result;
                }

                result = new WebApiResultEntity<GetOneSupporterTodoDietListByDailyPageResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<GetOneSupporterTodoDietListByDailyPageResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 获取指定客服在指定日期下的所有未完成的配餐代办事项总数量
        /// </summary>
        /// <param name="supporterOid">客服OID</param>
        /// <param name="dateTime">指定日期</param>
        /// <returns></returns>
        [HttpGet("TodoDietListCount/{supporterOid}/{dateTime}")]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<GetOneSupporterTodoDietListCountByDailyResponse> GetTodoDietListCount(Guid supporterOid, DateTime dateTime)
        {
            WebApiResultEntity<GetOneSupporterTodoDietListCountByDailyResponse> result;
            try
            {
                var models = _supporterAppService.GetOneSupporterTodoDietListCountByDaily(supporterOid, dateTime);

                if (models == null)
                {
                    result = new WebApiResultEntity<GetOneSupporterTodoDietListCountByDailyResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.TodoDietCount <= 0)
                {
                    result = new WebApiResultEntity<GetOneSupporterTodoDietListCountByDailyResponse> { success = false, message = "暂不存在配餐代办事项" };
                    return result;
                }

                result = new WebApiResultEntity<GetOneSupporterTodoDietListCountByDailyResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<GetOneSupporterTodoDietListCountByDailyResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 分页获取指定客服在指定日期下的所有未完成的打卡代办事项
        /// </summary>
        /// <param name="supporterOid">客服OID</param>
        /// <param name="dateTime">指定日期</param>
        /// <param name="pageIndex">页码索引，从1开始</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        [HttpGet("TodoDingList/{supporterOid}/{dateTime}")]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<GetOneSupporterTodoDingListByDailyPageResponse> GetTodoDingList(Guid supporterOid, DateTime dateTime, int pageIndex = 1, int pageSize = 10)
        {
            WebApiResultEntity<GetOneSupporterTodoDingListByDailyPageResponse> result;
            try
            {
                var models = _supporterAppService.GetOneSupporterTodoDingListByDailyPage(supporterOid, dateTime, pageIndex - 1, pageSize);

                if (models == null)
                {
                    result = new WebApiResultEntity<GetOneSupporterTodoDingListByDailyPageResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.TodoDingList.Data.Count() <= 0)
                {
                    result = new WebApiResultEntity<GetOneSupporterTodoDingListByDailyPageResponse> { success = false, message = "暂不存在协助打卡代办事项" };
                    return result;
                }

                result = new WebApiResultEntity<GetOneSupporterTodoDingListByDailyPageResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<GetOneSupporterTodoDingListByDailyPageResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 获取指定客服在指定日期下的所有未完成的打卡代办事项总数量
        /// </summary>
        /// <param name="supporterOid">客服OID</param>
        /// <param name="dateTime">指定日期</param>
        /// <returns></returns>
        [HttpGet("TodoDingListCount/{supporterOid}/{dateTime}")]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<GetOneSupporterTodoDingListCountByDailyResponse> GetTodoDingListCount(Guid supporterOid, DateTime dateTime)
        {
            WebApiResultEntity<GetOneSupporterTodoDingListCountByDailyResponse> result;
            try
            {
                var models = _supporterAppService.GetOneSupporterTodoDingListCountByDaily(supporterOid, dateTime);

                if (models == null)
                {
                    result = new WebApiResultEntity<GetOneSupporterTodoDingListCountByDailyResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.TodoDingCount <= 0)
                {
                    result = new WebApiResultEntity<GetOneSupporterTodoDingListCountByDailyResponse> { success = false, message = "暂不存在打卡代办事项" };
                    return result;
                }

                result = new WebApiResultEntity<GetOneSupporterTodoDingListCountByDailyResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<GetOneSupporterTodoDingListCountByDailyResponse> { success = false, message = "系统错误" };
            }
            return result;
        }
    }
}