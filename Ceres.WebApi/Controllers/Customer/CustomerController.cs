using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ceres.Application.Interfaces;
using Ceres.Application.ViewModels;
using Ceres.Domain.Core.Notifications;
using Ceres.WebApi.AuthHelper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ceres.WebApi.Controllers
{
    /// <summary>
    /// 客户
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerAppService _customerAppService;
        private readonly IAnswerAppService _answerAppService;
        private readonly IWeChatAuthorizeAppService _weChatAuthorizeAppService;
        // 将领域通知处理程序注入Controller
        private readonly DomainNotificationHandler _notifications;

        public CustomerController(ICustomerAppService customerAppService, 
            IAnswerAppService answerAppService,
            IWeChatAuthorizeAppService weChatAuthorizeAppService,
            INotificationHandler<DomainNotification> notifications)
        {
            _customerAppService = customerAppService;
            _answerAppService = answerAppService;
            _weChatAuthorizeAppService = weChatAuthorizeAppService;
            // 强类型转换
            _notifications = (DomainNotificationHandler)notifications;
        }

        /// <summary>
        /// 新增一个客户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<string> Post([FromBody] CreateOneCustomerRequest request)
        {
            WebApiResultEntity<string> result;
            try
            {
                _customerAppService.CreateOneCustomer(request);

                // 是否存在消息通知
                if (_notifications.HasNotifications())
                {
                    // 从通知处理程序中，获取全部通知信息，并返回给response
                    var notificacoes = _notifications.GetNotifications();
                    StringBuilder sb = new StringBuilder();
                    notificacoes.ForEach(c => sb.Append(c.Value));
                    return result = new WebApiResultEntity<string> { success = false, message = "新增失败", response = sb.ToString() };
                }

                result = new WebApiResultEntity<string> { success = true, message = "新增成功", response = "新增成功！" };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<string> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 获取指定客户基本信息
        /// </summary>
        /// <param name="oid">客户OID</param>
        /// <returns></returns>
        [HttpGet("{oid}")]
        public WebApiResultEntity<GetOneCustomerBasicInformationResponse> Get(Guid oid)
        {
            WebApiResultEntity<GetOneCustomerBasicInformationResponse> result;
            try
            {
                var models = _customerAppService.GetOneCustomerBasicInformation(oid);

                if (models == null)
                {
                    result = new WebApiResultEntity<GetOneCustomerBasicInformationResponse> { success = false, message = "用户不存在" };
                    return result;
                }
                result = new WebApiResultEntity<GetOneCustomerBasicInformationResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<GetOneCustomerBasicInformationResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 获取指定客户的体重列表，客服网页版使用
        /// </summary>
        /// <param name="oid">客户OID</param>
        /// <param name="dataCount">获取最新的数量</param>
        /// <returns></returns>
        [HttpGet("WeightList/{oid}")]
        public WebApiResultEntity<GetOneCustomerWeightListResponse> GetWeightList(Guid oid, int dataCount=5)
        {
            WebApiResultEntity<GetOneCustomerWeightListResponse> result;
            try
            {
                var models = _answerAppService.GetOneCustomerWeightList(oid, dataCount);

                if (models == null)
                {
                    result = new WebApiResultEntity<GetOneCustomerWeightListResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.WeightList.Count() <= 0)
                {
                    result = new WebApiResultEntity<GetOneCustomerWeightListResponse> { success = false, message = "暂不存在体重数据" };
                    return result;
                }

                result = new WebApiResultEntity<GetOneCustomerWeightListResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<GetOneCustomerWeightListResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 获取指定客户的体重列表，小程序客户使用
        /// </summary>
        /// <param name="oid">客户OID</param>
        /// <param name="dataCount">获取最新的数量</param>
        /// <returns></returns>
        [HttpGet("Customer/WeightList/{oid}")]
        public WebApiResultEntity<GetOneCustomerWXWeightListResponse> GetWXWeightList(Guid oid, int dataCount = 15)
        {
            WebApiResultEntity<GetOneCustomerWXWeightListResponse> result;
            try
            {
                var models = _answerAppService.GetOneCustomerWXWeightList(oid, dataCount);

                if (models == null)
                {
                    result = new WebApiResultEntity<GetOneCustomerWXWeightListResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.WeightList.Count() <= 0)
                {
                    result = new WebApiResultEntity<GetOneCustomerWXWeightListResponse> { success = false, message = "暂不存在体重数据" };
                    return result;
                }

                result = new WebApiResultEntity<GetOneCustomerWXWeightListResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<GetOneCustomerWXWeightListResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 获取指定客户的BMI列表
        /// </summary>
        /// <param name="oid">客户OID</param>
        /// <param name="dataCount">获取最新的数量</param>
        /// <returns></returns>
        [HttpGet("BMIList/{oid}")]
        public WebApiResultEntity<GetOneCustomerBMIListResponse> GetBMIList(Guid oid, int dataCount = 5)
        {
            WebApiResultEntity<GetOneCustomerBMIListResponse> result;
            try
            {
                var models = _answerAppService.GetOneCustomerBMIList(oid, dataCount);

                if (models == null)
                {
                    result = new WebApiResultEntity<GetOneCustomerBMIListResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.BMIList.Count() <= 0)
                {
                    result = new WebApiResultEntity<GetOneCustomerBMIListResponse> { success = false, message = "暂不存在BMI数据" };
                    return result;
                }

                result = new WebApiResultEntity<GetOneCustomerBMIListResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<GetOneCustomerBMIListResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 分页获取指定客户的所有体质测试结果列表------来源于Mercury程序中【TraditionalMedicalConstitutionController】
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("PhysiqueList/{oid}")]
        public WebApiResultEntity<QueryOneCustomerPhysiqueListByPageResponse> GetPhysiqueList(Guid oid, int pageIndex = 1, int pageSize = 10)
        {
            WebApiResultEntity<QueryOneCustomerPhysiqueListByPageResponse> result;
            try
            {
                var models = _answerAppService.QueryOneCustomerPhysiqueListByPage(oid, pageIndex - 1, pageSize);

                if (models == null)
                {
                    result = new WebApiResultEntity<QueryOneCustomerPhysiqueListByPageResponse> { success = false, message = "系统错误" };
                    return result;
                }

                if (models.PhysiqueList.Data.Count() <= 0)
                {
                    result = new WebApiResultEntity<QueryOneCustomerPhysiqueListByPageResponse> { success = false, message = "暂不存在体质测试结果数据" };
                    return result;
                }

                result = new WebApiResultEntity<QueryOneCustomerPhysiqueListByPageResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<QueryOneCustomerPhysiqueListByPageResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 获取指定客户的体质测试结果------来源于Mercury程序中【TraditionalMedicalConstitutionController】
        /// </summary>
        /// <param name="oid">客户OID</param>
        /// <returns>返回最新体质测试结果</returns>
        [HttpGet("Physique/{oid}")]
        public WebApiResultEntity<GetOneCustomerPhysiqueResponse> GetPhysique(Guid oid)
        {
            WebApiResultEntity<GetOneCustomerPhysiqueResponse> result;
            try
            {
                var models = _answerAppService.GetOneCustomerPhysique(oid);

                if (models == null)
                {
                    result = new WebApiResultEntity<GetOneCustomerPhysiqueResponse> { success = false, message = "暂不存在体质测试结果数据" };
                    return result;
                }

                result = new WebApiResultEntity<GetOneCustomerPhysiqueResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<GetOneCustomerPhysiqueResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 获取指定客户的每日理想总能量
        /// </summary>
        /// <param name="oid">客户OID</param>
        /// <returns>返回最新体质测试结果</returns>
        [HttpGet("DailyEnergy/{oid}")]
        public WebApiResultEntity<GetOneCustomerDailyEnergyResponse> GetDailyEnergy(Guid oid)
        {
            WebApiResultEntity<GetOneCustomerDailyEnergyResponse> result;
            try
            {
                var models = _answerAppService.GetOneCustomerDailyEnergy(oid);

                if (models == null)
                {
                    result = new WebApiResultEntity<GetOneCustomerDailyEnergyResponse> { success = false, message = "数据缺失，无法提供服务" };
                    return result;
                }

                result = new WebApiResultEntity<GetOneCustomerDailyEnergyResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<GetOneCustomerDailyEnergyResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 查询指定手机号是否为VIP用户
        /// </summary>
        /// <param name="cellphone">用户手机号</param>
        /// <returns></returns>
        [HttpGet("IsVIP/{cellphone}")]
        public WebApiResultEntity<GetOneVIPCustomerResponse> GetOneVIPCustomer(string cellphone)
        {
            WebApiResultEntity<GetOneVIPCustomerResponse> result;
            try
            {
                var models = _customerAppService.GetOneVIPCustomer(cellphone);

                if (models == null)
                {
                    result = new WebApiResultEntity<GetOneVIPCustomerResponse> { success = true, message = "当前用户不是VIP，无法进行打卡" ,response=new GetOneVIPCustomerResponse()};
                    return result;
                }

                result = new WebApiResultEntity<GetOneVIPCustomerResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<GetOneVIPCustomerResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 查询用户今天是否可以执行体脂测试
        /// </summary>
        /// <param name="cellphone">用户手机号</param>
        /// <returns></returns>
        [HttpGet("Physique/Today/{cellphone}")]
        public WebApiResultEntity<GetOneCustomerTodayPhysiqueResponse> GetOneCustomerTodayPhysique(string cellphone)
        {
            WebApiResultEntity<GetOneCustomerTodayPhysiqueResponse> result;
            try
            {
                if (
                    (DateTime.Now.AddMinutes(30).Date== DateTime.Now.AddDays(1).Date) 
                    ||
                    (DateTime.Now.AddHours(-1).Date < DateTime.Now.Date)
                    )
                {
                    GetOneCustomerTodayPhysiqueResponse forbiddenAnswer = new GetOneCustomerTodayPhysiqueResponse();
                    forbiddenAnswer.PhysiqueStatus = 0;
                    forbiddenAnswer.PhysiqueStatusDescription = "夜间服务器维护时间【23：30-01：00】，禁止体脂测试";
                    forbiddenAnswer.CreateTime = DateTime.Now;
                    result = new WebApiResultEntity<GetOneCustomerTodayPhysiqueResponse> { success = true, message = "查询成功", response=forbiddenAnswer };
                    return result;
                }

                var models = _answerAppService.GetOneCustomerTodayPhysique(cellphone);

                if (models == null)
                {
                    result = new WebApiResultEntity<GetOneCustomerTodayPhysiqueResponse> { success = true, message = "今天未完成体脂测试，可以执行体脂测试",response=new GetOneCustomerTodayPhysiqueResponse() };
                    return result;
                }

                result = new WebApiResultEntity<GetOneCustomerTodayPhysiqueResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<GetOneCustomerTodayPhysiqueResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 微信授权获取随机数
        /// </summary>
        /// <param name="code">微信code</param>
        /// <returns></returns>
        [HttpGet("Authorize/{code}")]
        public WebApiResultEntity<string> GetRandomString(string code)
        {
            WebApiResultEntity<string> result;
            try
            {
                string url= Appsettings.app(new string[] { "WxAccess", "Url" });
                string appId = Appsettings.app(new string[] { "WxAccess", "AppId" });
                string appSecret = Appsettings.app(new string[] { "WxAccess", "AppSecret" });

                var httpClient = new HttpClient();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "appid=" + appId + "&secret=" + appSecret + "&js_code=" + code + "&grant_type=authorization_code");
                request.Method = "GET";
                request.ContentType = "text/html;charset=utf-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, System.Text.Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                //var obj = new
                //{
                //    data = retString,
                //    Success = true
                //};
                //Formatting microsoftDataFormatSettings = default(Formatting);
                //string resultd = JsonConvert.SerializeObject(obj, microsoftDataFormatSettings);
                WeChatResponse wechatResponse = JsonConvert.DeserializeObject<WeChatResponse>(retString);

                if (wechatResponse.errcode!=null&&wechatResponse.errcode!= "0")
                {
                    return result = new WebApiResultEntity<string> { success = false, message = "授权失败" };
                }

                var randomOID = Guid.NewGuid();

                //添加到数据库中
                CreateOneWeChatAuthorizeRequest req = new CreateOneWeChatAuthorizeRequest();
                req.OID = randomOID;
                req.Code2Session = JsonConvert.SerializeObject(wechatResponse);
                _weChatAuthorizeAppService.CreateOneWeChatAuthorize(req);

                // 是否存在消息通知
                if (_notifications.HasNotifications())
                {
                    // 从通知处理程序中，获取全部通知信息，并返回给response
                    var notificacoes = _notifications.GetNotifications();
                    StringBuilder sb = new StringBuilder();
                    notificacoes.ForEach(c => sb.Append(c.Value));
                    return result = new WebApiResultEntity<string> { success = false, message = "授权失败", response = sb.ToString() };
                }

                result = new WebApiResultEntity<string> { success = true, message = "授权成功，即将解密", response = randomOID.ToString() };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<string> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 微信授权获取手机号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Cellphone")]
        public WebApiResultEntity<string> GetCellphone([FromBody] WeChatGetCellphoneRequest request)
        {
            WebApiResultEntity<string> result;
            try
            {
                //获取微信返回值
                var model = _weChatAuthorizeAppService.GetValidChatAuthorize(request.RandomString);
                if(model==null)
                {
                    return result = new WebApiResultEntity<string> { success = false, message = "无法解密" };
                }

                //解析session_key
                WeChatResponse weChatResponse = JsonConvert.DeserializeObject<WeChatResponse>(model.Code2Session);
                if (weChatResponse == null)
                {
                    return result = new WebApiResultEntity<string> { success = false, message = "无法解密" };
                }


                //解密
                byte[] encryptedDatas = Convert.FromBase64String(request.EncryptedData);
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Key = Convert.FromBase64String(weChatResponse.session_key);
                rijndaelCipher.IV = Convert.FromBase64String(request.IV);
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
                byte[] plainText = transform.TransformFinalBlock(encryptedDatas, 0, encryptedDatas.Length);
                string weChatPhoneString = Encoding.Default.GetString(plainText);

                //序列化获取手机号码
                WeChatPhone<Watermark> weChatPhone = JsonConvert.DeserializeObject<WeChatPhone<Watermark>>(weChatPhoneString);
                if (weChatPhone == null)
                {
                    return result = new WebApiResultEntity<string> { success = false, message = "无法解密" };
                }

                //添加到数据库中
                UpdateWeChatAuthorizeRequest req = new UpdateWeChatAuthorizeRequest();
                req.RandomString = request.RandomString;
                req.EncryptedData = request.EncryptedData;
                req.IV = request.IV;
                req.PhoneJson = JsonConvert.SerializeObject(weChatPhone);
                
                _weChatAuthorizeAppService.UpdateWeChatAuthorize(req);

                // 是否存在消息通知
                if (_notifications.HasNotifications())
                {
                    // 从通知处理程序中，获取全部通知信息，并返回给response
                    var notificacoes = _notifications.GetNotifications();
                    StringBuilder sb = new StringBuilder();
                    notificacoes.ForEach(c => sb.Append(c.Value));
                    return result = new WebApiResultEntity<string> { success = false, message = "无法解密", response = sb.ToString() };
                }

                result = new WebApiResultEntity<string> { success = true, message = "查询成功", response = weChatPhone.phoneNumber, };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<string> { success = false, message = "系统错误" };
            }
            return result;
        }
    }
}