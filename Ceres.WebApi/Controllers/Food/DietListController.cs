using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceres.Application.Interfaces;
using Ceres.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ceres.WebApi.Controllers.Food
{
    /// <summary>
    /// 食谱列表
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DietListController : ControllerBase
    {
        private readonly IDietAppService _dietAppService;

        public DietListController(IDietAppService dietAppService)
        {
            _dietAppService = dietAppService;
        }

        /// <summary>
        /// 分页获取指定客户的所有食谱列表
        /// </summary>
        /// <param name="oid">客户OID</param>
        /// <param name="pageIndex">页码索引，从1开始</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        [HttpGet("{oid}")]
        public WebApiResultEntity<QueryOneCustomerDietListByPageResponse> GetOneCustomerDietList(Guid oid,int pageIndex = 1, int pageSize = 10)
        {
            WebApiResultEntity<QueryOneCustomerDietListByPageResponse> result;
            try
            {
                var models = _dietAppService.QueryOneCustomerDietListByPage(oid,pageIndex - 1, pageSize);
                if (models == null)
                {
                    result = new WebApiResultEntity<QueryOneCustomerDietListByPageResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.DietList.Data.Count() <= 0)
                {
                    result = new WebApiResultEntity<QueryOneCustomerDietListByPageResponse> { success = false, message = "暂不存在食谱" };
                    return result;
                }

                result = new WebApiResultEntity<QueryOneCustomerDietListByPageResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<QueryOneCustomerDietListByPageResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 分页获取所有食谱列表
        /// </summary>
        /// <param name="pageIndex">页码索引，从1开始</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<QueryDietListByPageResponse> GetDietList(int pageIndex = 1, int pageSize = 10)
        {
            WebApiResultEntity<QueryDietListByPageResponse> result;
            try
            {
                var models = _dietAppService.QueryDietListByPage(pageIndex - 1, pageSize);
                if (models == null)
                {
                    result = new WebApiResultEntity<QueryDietListByPageResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.DietList.Data.Count() <= 0)
                {
                    result = new WebApiResultEntity<QueryDietListByPageResponse> { success = false, message = "暂不存在食谱" };
                    return result;
                }

                result = new WebApiResultEntity<QueryDietListByPageResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<QueryDietListByPageResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 条件分页获取所有有效的食谱列表
        /// </summary>
        /// <param name="customerName">客户名称</param>
        /// <param name="cellphone">客户手机号</param>
        /// <param name="serviceOid">服务OID</param>
        /// <param name="supporterOid">客服OID</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="endTime">查询截至时间</param>
        /// <param name="pageIndex">页码索引，从1开始</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        [HttpGet("Condition")]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<QueryDietListByPageResponse> GetConditionDietList(string customerName, string cellphone, Guid serviceOid,Guid supporterOid,DateTime startTime,DateTime endTime, int pageIndex = 1, int pageSize = 10)
        {
            WebApiResultEntity<QueryDietListByPageResponse> result;
            try
            {
                var models = _dietAppService.QueryDietListByPage(customerName, cellphone, serviceOid, supporterOid, startTime,endTime, pageIndex - 1, pageSize);

                if (models == null)
                {
                    result = new WebApiResultEntity<QueryDietListByPageResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.DietList.Data.Count() <= 0)
                {
                    result = new WebApiResultEntity<QueryDietListByPageResponse> { success = false, message = "暂不存在食谱" };
                    return result;
                }

                result = new WebApiResultEntity<QueryDietListByPageResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<QueryDietListByPageResponse> { success = false, message = "系统错误" };
            }
            return result;
        }
    }
}