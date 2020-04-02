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
    /// 食谱
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DietController : ControllerBase
    {
        private readonly IDietAppService _dietAppService;
        // 将领域通知处理程序注入Controller
        private readonly DomainNotificationHandler _notifications;

        public DietController(IDietAppService dietAppService,
            INotificationHandler<DomainNotification> notifications)
        {
            _dietAppService = dietAppService;
            // 强类型转换
            _notifications = (DomainNotificationHandler)notifications;
        }

        /// <summary>
        /// 新增一条食谱
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<string> PostDiet([FromBody] CreateOneCustomerDietRequest request)
        {
            WebApiResultEntity<string> result;
            try
            {
                _dietAppService.CreateOneCustomerDiet(request);

                // 是否存在消息通知
                if (_notifications.HasNotifications())
                {
                    // 从通知处理程序中，获取全部通知信息，并返回给response
                    var notificacoes = _notifications.GetNotifications();
                    StringBuilder sb = new StringBuilder();
                    notificacoes.ForEach(c => sb.Append(c.Value));
                    return result = new WebApiResultEntity<string> { success = false, message = "新增食谱失败", response = sb.ToString() };
                }

                result = new WebApiResultEntity<string> { success = true, message = "新增食谱成功", response = "新增食谱成功！" };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<string> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 删除一条食谱
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<string> DeleteDiet([FromBody] DeleteOneCustomerDietRequest request)
        {
            WebApiResultEntity<string> result;
            try
            {
                _dietAppService.DeleteOneCustomerDiet(request);

                // 是否存在消息通知
                if (_notifications.HasNotifications())
                {
                    // 从通知处理程序中，获取全部通知信息，并返回给response
                    var notificacoes = _notifications.GetNotifications();
                    StringBuilder sb = new StringBuilder();
                    notificacoes.ForEach(c => sb.Append(c.Value));
                    return result = new WebApiResultEntity<string> { success = false, message = "删除食谱失败", response = sb.ToString() };
                }

                result = new WebApiResultEntity<string> { success = true, message = "删除食谱成功", response = "删除食谱成功！" };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<string> { success = false, message = "系统错误" };
            }
            return result;
        }
    }
}