using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceres.Application;
using Ceres.Application.Interfaces;
using Ceres.Application.ViewModels;
using Ceres.WebApi.AuthHelper;
using Ceres.WebApi.AuthHelper.OverWrite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ceres.WebApi.Controllers
{
    /// <summary>
    /// 客服登陆
    /// </summary>
    [Route("api/Supporter/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ISupporterAppService _supporterAppService;
        public LoginController(ISupporterAppService supporterAppService)
        {
            _supporterAppService = supporterAppService;
        }

        /// <summary>
        /// 客服登陆
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public WebApiResultEntity<SupporterLoginResponse> Post([FromBody] SupporterLoginRequest request)
        {
            WebApiResultEntity<SupporterLoginResponse> result;
            try
            {
                var model = _supporterAppService.SupporterLoginNameLogin(request);
                if (model == null)
                {
                    model = _supporterAppService.SupporterCellphoneLogin(request);
                }

                if (model == null)
                {
                    result = new WebApiResultEntity<SupporterLoginResponse> { success = false, message = "客服用户名或者密码错误" };
                    return result;
                }

                //登录成功
                TokenModelJwt tokenModel = new TokenModelJwt { Uid = 1, Role = "Supporter" };
                var jwtStr = JwtHelper.IssueJwt(tokenModel);//登录，获取到一定规则的 Token 令牌

                //增加客服头像地址
                string imageUrl = Appsettings.app(new string[] { "FilesConfiguration", "ImageUrl" });
                model.Image = imageUrl + model.Image;

                result = new WebApiResultEntity<SupporterLoginResponse> { success = true, token = jwtStr, message = "登陆成功", response = model };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<SupporterLoginResponse> { success = false, message = "系统错误" };
            }
            return result;
                
        }
    }
}