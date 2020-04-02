using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceres.Application.Interfaces;
using Ceres.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ceres.WebApi.Controllers.Customer
{
    /// <summary>
    /// 未筛选的客户
    /// </summary>
    [Route("api/Customer/[controller]")]
    [ApiController]
    public class OriginalCustomerController : ControllerBase
    {
        private readonly IUserInformationAppService _userInformationAppService;

        public OriginalCustomerController(IUserInformationAppService userInformationAppService)
        {
            _userInformationAppService = userInformationAppService;
        }

        /// <summary>
        /// 获取指定的客户信息
        /// </summary>
        /// <param name="cellphone">用户手机号码</param>
        /// <returns></returns>
        [HttpGet("{cellphone}")]
        public WebApiResultEntity<GetOneOriginalCustomerResponse> Get(string cellphone)
        {
            WebApiResultEntity<GetOneOriginalCustomerResponse> result;
            try
            {
                var models = _userInformationAppService.GetUserByPhoneNumber(cellphone);

                if (models == null)
                {
                    result = new WebApiResultEntity<GetOneOriginalCustomerResponse> { success = false, message = "用户不存在" };
                    return result;
                }

                result = new WebApiResultEntity<GetOneOriginalCustomerResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<GetOneOriginalCustomerResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        
    }
}