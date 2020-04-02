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
    /// 代理列表
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AgenterListController : ControllerBase
    {
        private readonly IAgenterAppService _agenterAppService;

        public AgenterListController(IAgenterAppService agenterAppService)
        {
            _agenterAppService = agenterAppService;
        }

        /// <summary>
        /// 获取所有有效的代理列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<IEnumerable<GetAgenterListResponse>> Get()
        {
            WebApiResultEntity<IEnumerable<GetAgenterListResponse>> result;
            try
            {
                var models = _agenterAppService.GetAgenterList();
                if (models == null)
                {
                    result = new WebApiResultEntity<IEnumerable<GetAgenterListResponse>> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.Count() <= 0)
                {
                    result = new WebApiResultEntity<IEnumerable<GetAgenterListResponse>> { success = false, message = "暂不存在代理" };
                    return result;
                }

                //增加代理头像地址
                string imageUrl = Appsettings.app(new string[] { "FilesConfiguration", "ImageUrl" });
                foreach(var agenter in models)
                {
                    agenter.Image = imageUrl + agenter.Image;
                }

                result = new WebApiResultEntity<IEnumerable<GetAgenterListResponse>> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<IEnumerable<GetAgenterListResponse>> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 按照代理名称查询代理列表
        /// </summary>
        /// <param name="agenterName">代理名称</param>
        /// <returns></returns>
        [HttpGet("{agenterName}")]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<IEnumerable<GetAgenterListResponse>> QueryAgenterList(string agenterName)
        {
            WebApiResultEntity<IEnumerable<GetAgenterListResponse>> result;
            try
            {
                var models = _agenterAppService.QueryAgenterList(agenterName);
                if (models == null)
                {
                    result = new WebApiResultEntity<IEnumerable<GetAgenterListResponse>> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.Count() <= 0)
                {
                    result = new WebApiResultEntity<IEnumerable<GetAgenterListResponse>> { success = false, message = "暂不存在代理" };
                    return result;
                }

                //增加代理头像地址
                string imageUrl = Appsettings.app(new string[] { "FilesConfiguration", "ImageUrl" });
                foreach (var agenter in models)
                {
                    agenter.Image = imageUrl + agenter.Image;
                }

                result = new WebApiResultEntity<IEnumerable<GetAgenterListResponse>> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<IEnumerable<GetAgenterListResponse>> { success = false, message = "系统错误" };
            }
            return result;
        }

    }
}