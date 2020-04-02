using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceres.Application.Interfaces;
using Ceres.Application.ViewModels;
using Ceres.WebApi.AuthHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ceres.WebApi.Controllers
{
    /// <summary>
    /// 客服列表
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SupporterListController : ControllerBase
    {
        private readonly ISupporterAppService _supporterAppService;

        public SupporterListController(ISupporterAppService supporterAppService)
        {
            _supporterAppService = supporterAppService;
        }

        /// <summary>
        /// 获取所有有效的客服列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<IEnumerable<GetSupporterListResponse>> Get()
        {
            WebApiResultEntity<IEnumerable<GetSupporterListResponse>> result;
            try
            {
                var models = _supporterAppService.GetSupporterList();

                if (models == null)
                {
                    result = new WebApiResultEntity<IEnumerable<GetSupporterListResponse>> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.Count() <= 0)
                {
                    result = new WebApiResultEntity<IEnumerable<GetSupporterListResponse>> { success = false, message = "暂不存在客服" };
                    return result;
                }

                //增加客服头像地址
                string imageUrl = Appsettings.app(new string[] { "FilesConfiguration", "ImageUrl" });
                foreach (var supporter in models)
                {
                    supporter.Image = imageUrl + supporter.Image;
                }

                result = new WebApiResultEntity<IEnumerable<GetSupporterListResponse>> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<IEnumerable<GetSupporterListResponse>> { success = false, message = "系统错误" };
            }
            return result;
        }

    }
}