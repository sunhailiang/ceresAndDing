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
namespace Ceres.WebApi.Controllers.Service
{
    /// <summary>
    /// 服务列表
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceListController : ControllerBase
    {
        private readonly IServiceAppService _serviceAppService;

        public ServiceListController(IServiceAppService serviceAppService)
        {
            _serviceAppService = serviceAppService;
        }

        /// <summary>
        /// 获取所有有效的服务列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<IEnumerable<GetServiceListResponse>> Get()
        {
            WebApiResultEntity<IEnumerable<GetServiceListResponse>> result;
            try
            {
                var models = _serviceAppService.GetServiceList();

                if (models == null)
                {
                    result = new WebApiResultEntity<IEnumerable<GetServiceListResponse>> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.Count() <= 0)
                {
                    result = new WebApiResultEntity<IEnumerable<GetServiceListResponse>> { success = false, message = "暂不存在服务" };
                    return result;
                }

                //增加服务图片地址
                string imageUrl = Appsettings.app(new string[] { "FilesConfiguration", "ImageUrl" });
                foreach (var service in models)
                {
                    service.Image = imageUrl + service.Image;
                }

                result = new WebApiResultEntity<IEnumerable<GetServiceListResponse>> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<IEnumerable<GetServiceListResponse>> { success = false, message = "系统错误" };
            }
            return result;
        }

    }
}