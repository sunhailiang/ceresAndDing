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
    /// 客服密码
    /// </summary>
    [Route("api/Supporter/[controller]")]
    [ApiController]
    public class ResetPasswordController : ControllerBase
    {
        private readonly ISupporterAppService _supporterAppService;
        // 将领域通知处理程序注入Controller
        private readonly DomainNotificationHandler _notifications;
        public ResetPasswordController(ISupporterAppService supporterAppService, INotificationHandler<DomainNotification> notifications)
        {
            _supporterAppService = supporterAppService;

            // 强类型转换
            _notifications = (DomainNotificationHandler)notifications;
        }

        /// <summary>
        /// 更新客服密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<string> Post([FromBody] UpdateSupporterPasswordRequest request)
        {
            WebApiResultEntity<string> result;
            try
            {
                _supporterAppService.UpdateSupporterPassword(request);

                // 是否存在消息通知
                if (_notifications.HasNotifications())
                {
                    // 从通知处理程序中，获取全部通知信息，并返回给response
                    var notificacoes = _notifications.GetNotifications();
                    StringBuilder sb = new StringBuilder();
                    notificacoes.ForEach(c => sb.Append(c.Value));
                    return result = new WebApiResultEntity<string> { success = false, message = "更新失败", response = sb.ToString() };
                }

                result = new WebApiResultEntity<string> { success = true, message = "更新成功", response = "更新成功！请重新登陆" };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<string> { success = false, message = "系统错误" };
            }
            return result;

        }
    }
}