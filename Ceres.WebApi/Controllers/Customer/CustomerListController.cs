using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceres.Application.Interfaces;
using Ceres.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ceres.WebApi.Controllers.Customer
{
    /// <summary>
    /// 客户列表
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerListController : ControllerBase
    {
        private readonly ICustomerAppService _customerAppService;

        public CustomerListController(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }

        /// <summary>
        /// 分页获取所有有效的客户列表
        /// </summary>
        /// <param name="pageIndex">页码索引，从1开始</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<QueryCustomerListByPageResponse> Get(int pageIndex = 1, int pageSize = 10)
        {
            WebApiResultEntity<QueryCustomerListByPageResponse> result;
            try
            {
                var models = _customerAppService.QueryCustomerListByPage(pageIndex - 1, pageSize);

                if (models == null)
                {
                    result = new WebApiResultEntity<QueryCustomerListByPageResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.CustomerList.Data.Count() <= 0)
                {
                    result = new WebApiResultEntity<QueryCustomerListByPageResponse> { success = false, message = "暂不存在客户" };
                    return result;
                }

                result = new WebApiResultEntity<QueryCustomerListByPageResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<QueryCustomerListByPageResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 条件分页获取所有有效的客户列表
        /// </summary>
        /// <param name="customerName">客户名称</param>
        /// <param name="cellphone">客户手机号</param>
        /// <param name="serviceOid">服务OID</param>
        /// <param name="supporterOid">客服OID</param>
        /// <param name="agenterOid">代理OID</param>
        /// <param name="pageIndex">页码索引，从1开始</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        [HttpGet("Condition")]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<QueryCustomerListByPageResponse> Get(string customerName,string cellphone, Guid serviceOid, Guid supporterOid, Guid agenterOid,int pageIndex = 1, int pageSize = 10)
        {
            WebApiResultEntity<QueryCustomerListByPageResponse> result;
            try
            {
                var models = _customerAppService.QueryCustomerListByPage(customerName, cellphone, serviceOid, supporterOid, agenterOid, pageIndex - 1, pageSize);

                if (models == null)
                {
                    result = new WebApiResultEntity<QueryCustomerListByPageResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.CustomerList.Data.Count() <= 0)
                {
                    result = new WebApiResultEntity<QueryCustomerListByPageResponse> { success = false, message = "暂不存在客户" };
                    return result;
                }

                result = new WebApiResultEntity<QueryCustomerListByPageResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<QueryCustomerListByPageResponse> { success = false, message = "系统错误" };
            }
            return result;
        }
    }
}