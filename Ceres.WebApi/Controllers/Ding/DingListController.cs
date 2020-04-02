using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceres.Application.Interfaces;
using Ceres.Application.ViewModels;
using Ceres.WebApi.AuthHelper;
using Ceres.WebApi.OSSHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ceres.WebApi.Controllers.Ding
{
    /// <summary>
    /// 打卡列表
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DingListController : ControllerBase
    {
        private readonly IAnswerAppService _answerAppService;

        public DingListController(IAnswerAppService answerAppService)
        {
            _answerAppService = answerAppService;
        }

        /// <summary>
        /// 分页获取指定客户的所有打卡列表
        /// </summary>
        /// <param name="customerOid">客户OID</param>
        /// <param name="pageIndex">页码索引，从1开始</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        [HttpGet("{customerOid}")]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<QueryOneCustomerDingListByPageResponse> GetOneCustomerDingList(Guid customerOid, int pageIndex = 1, int pageSize = 10)
        {
            WebApiResultEntity<QueryOneCustomerDingListByPageResponse> result;
            try
            {
                var models = _answerAppService.QueryOneCustomerDingListByPage(customerOid, pageIndex - 1, pageSize);
                if (models == null)
                {
                    result = new WebApiResultEntity<QueryOneCustomerDingListByPageResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.DingList.Data.Count() <= 0)
                {
                    result = new WebApiResultEntity<QueryOneCustomerDingListByPageResponse> { success = false, message = "暂不存在打卡列表" };
                    return result;
                }

                //增加打卡图片地址，整合Content中数据为String格式
                string imageUrl = Appsettings.app(new string[] { "FilesConfiguration", "ImageUrl" });
                foreach (var dingData in models.DingList.Data)
                {
                    foreach(var ding in dingData.DingList)
                    {
                        if(ding.QuestionType=="PhotoGraph")
                        {
                            //处理图片地址问题
                            PhotoGraphHelper.PhotoGraphQuestionType(imageUrl,ding);
                        }
                        else
                        {
                            PhotoGraphHelper.OtherQuestionType(ding);
                        }
                    }
                }

                result = new WebApiResultEntity<QueryOneCustomerDingListByPageResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<QueryOneCustomerDingListByPageResponse> { success = false, message = "系统错误" };
            }
            return result;
        }
        
    }
}