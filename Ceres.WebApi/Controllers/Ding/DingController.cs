using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ceres.Application.Interfaces;
using Ceres.Application.ViewModels;
using Ceres.Domain.Core.Notifications;
using Ceres.WebApi.AuthHelper;
using Ceres.WebApi.OSSHelper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ceres.WebApi.Controllers.Ding
{
    /// <summary>
    /// 打卡
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DingController : ControllerBase
    {
        private readonly IAnswerAppService _answerAppService;
        // 将领域通知处理程序注入Controller
        private readonly DomainNotificationHandler _notifications;
        public DingController(IAnswerAppService answerAppService,
             INotificationHandler<DomainNotification> notifications)
        {
            _answerAppService = answerAppService;
            // 强类型转换
            _notifications = (DomainNotificationHandler)notifications;
        }

        /// <summary>
        /// 新增一条协助打卡数据，客服网页版使用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<string> PostDing([FromBody] CreateOneCustomerAssistDingRequest request)
        {
            WebApiResultEntity<string> result;
            try
            {
                _answerAppService.CreateOneCustomerAssistDing(request);

                // 是否存在消息通知
                if (_notifications.HasNotifications())
                {
                    // 从通知处理程序中，获取全部通知信息，并返回给response
                    var notificacoes = _notifications.GetNotifications();
                    StringBuilder sb = new StringBuilder();
                    notificacoes.ForEach(c => sb.Append(c.Value));
                    return result = new WebApiResultEntity<string> { success = false, message = "协助打卡失败", response = sb.ToString() };
                }

                result = new WebApiResultEntity<string> { success = true, message = "协助打卡成功", response = "协助打卡成功！" };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<string> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 新增一条打卡数据，小程序客户使用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Customer")]
        public WebApiResultEntity<string> PostCustomerDing([FromBody] CreateOneCustomerDingRequest request)
        {
            WebApiResultEntity<string> result;
            try
            {
                if (
                    (DateTime.Now.AddMinutes(10).Date == DateTime.Now.AddDays(1).Date)
                    ||
                    (DateTime.Now.AddHours(-1).Date < DateTime.Now.Date)
                    )
                {
                    result = new WebApiResultEntity<string> { success = false, message = "禁止打卡", response = "夜间服务器维护时间【23：50-01：00】，禁止打卡" };
                    return result;
                }

                _answerAppService.CreateOneCustomerDing(request);

                // 是否存在消息通知
                if (_notifications.HasNotifications())
                {
                    // 从通知处理程序中，获取全部通知信息，并返回给response
                    var notificacoes = _notifications.GetNotifications();
                    StringBuilder sb = new StringBuilder();
                    notificacoes.ForEach(c => sb.Append(c.Value));
                    return result = new WebApiResultEntity<string> { success = false, message = "用户打卡失败", response = sb.ToString() };
                }

                result = new WebApiResultEntity<string> { success = true, message = "用户打卡成功", response = "用户打卡成功！" };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<string> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 删除一条协助打卡数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<string> DeleteDing([FromBody] DeleteOneCustomerAssistDingRequest request)
        {
            WebApiResultEntity<string> result;
            try
            {
                _answerAppService.DeleteOneCustomerAssistDing(request);

                // 是否存在消息通知
                if (_notifications.HasNotifications())
                {
                    // 从通知处理程序中，获取全部通知信息，并返回给response
                    var notificacoes = _notifications.GetNotifications();
                    StringBuilder sb = new StringBuilder();
                    notificacoes.ForEach(c => sb.Append(c.Value));
                    return result = new WebApiResultEntity<string> { success = false, message = "删除打卡记录失败", response = sb.ToString() };
                }

                result = new WebApiResultEntity<string> { success = true, message = "删除打卡记录成功", response = "删除打卡记录成功！" };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<string> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 上传打卡图片，支持批量处理，Stream方式
        /// </summary>
        /// <returns></returns>
        [HttpPost("UploadStream/Pic")]
        [Authorize(Policy = "Supporter")]
        public WebApiResultEntity<UploadDingPicResponse> UploadStreamPic()
        {
            WebApiResultEntity<UploadDingPicResponse> result;
            try
            {
                string imageUrl = Appsettings.app(new string[] { "FilesConfiguration", "ImageUrl" });
                imageUrl = imageUrl + "/Image/MercuryAnswer";

                HttpRequest request = HttpContext.Request;
                IFormFileCollection fileList = request.Form.Files;

                //文件多少
                if(fileList.Count()<=0)
                {
                    return result = new WebApiResultEntity<UploadDingPicResponse> { success = false, message = "未选择任何图片" };
                }

                if (fileList.Count() > 5)
                {
                    return result = new WebApiResultEntity<UploadDingPicResponse> { success = false, message = "一次最多上传5张图片" };
                }

                //文件格式判定
                foreach (var file in fileList)
                {
                    if(Path.GetExtension(file.FileName).ToUpper()!=".JPG"
                        && Path.GetExtension(file.FileName).ToUpper() != ".PNG")
                    {
                        return result = new WebApiResultEntity<UploadDingPicResponse> { success = false, message = "图片仅支持.jpg以及.png" };
                    }
                }

                List<DingPic> dingPicList = new List<DingPic>();
                DingPic dingPic = new DingPic();
                foreach (var file in fileList)
                {
                    string remoteOid = Guid.NewGuid().ToString();
                    dingPic = new DingPic();
                    try
                    {
                        var serverPath =Path.Combine( Directory.CreateDirectory("userUploads").FullName, remoteOid + Path.GetExtension(file.FileName));
                        using (var stream = new System.IO.FileStream(serverPath, System.IO.FileMode.OpenOrCreate))
                        {
                            file.CopyTo(stream);
                        }

                        //上传图片
                        if (TencentHelper.UploadToTencent(serverPath,"/Image/MercuryAnswer"+"/"+ remoteOid + Path.GetExtension(file.FileName)) !=0)
                        {
                            return result = new WebApiResultEntity<UploadDingPicResponse> { success = false, message = "上传失败" };
                        }

                        //删除本地图片
                        try
                        {
                            System.IO.File.Delete(serverPath);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    catch (Exception)
                    {
                        return result = new WebApiResultEntity<UploadDingPicResponse> { success = false, message = "上传失败" };
                    }
                    dingPic.OriginalName = file.FileName;
                    dingPic.RemoteOid =Guid.Parse(remoteOid);
                    dingPic.RemoteName = remoteOid + Path.GetExtension(file.FileName);
                    dingPicList.Add(dingPic);
                }
                UploadDingPicResponse uploadResult = new UploadDingPicResponse();
                uploadResult.RemoteCommonPath = imageUrl;
                uploadResult.DingPicList = new List<DingPic>();
                uploadResult.DingPicList = dingPicList;

                result = new WebApiResultEntity<UploadDingPicResponse> { success = true, message = "上传打卡图片成功", response = uploadResult };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<UploadDingPicResponse> { success = false, message = "系统错误" };
            }
            return result;
        }


        /// <summary>
        /// 上传打卡图片，支持批量处理，Base64方式
        /// </summary>
        /// <returns></returns>
        [HttpPost("UploadBase64/Pic")]
        //[Authorize(Policy = "Supporter")]
        public WebApiResultEntity<UploadDingPicResponse> UploadBase64Pic([FromBody] UploadDingPicRequest request)
        {
            WebApiResultEntity<UploadDingPicResponse> result;
            try
            {
                string imageUrl = Appsettings.app(new string[] { "FilesConfiguration", "ImageUrl" });
                imageUrl = imageUrl + "/Image/MercuryAnswer";

                //文件多少
                if (request!=null &&request.Base64Pic.Count<=0)
                {
                    return result = new WebApiResultEntity<UploadDingPicResponse> { success = false, message = "未选择任何图片" };
                }

                if(request.Base64Pic.Count>5)
                {
                    return result = new WebApiResultEntity<UploadDingPicResponse> { success = false, message = "一次最多上传5张图片" };
                }

                //Base64解码
                List<string> encodeFile = new List<string>();
                foreach(var pic in request.Base64Pic)
                {
                    //encodeFile.Add(pic);
                    encodeFile.Add(System.Web.HttpUtility.UrlDecode(pic));
                }

                //文件格式判定
                foreach (var pic in encodeFile)
                {
                    try
                    {
                        var picExt=pic.Split(';')[0].Split('/')[1].ToUpper();
                        if(picExt!="JPEG"&& picExt != "JPG" && picExt!="PNG")
                        {
                            return result = new WebApiResultEntity<UploadDingPicResponse> { success = false, message = "图片仅支持【.jpg;.jpeg;.png】" };
                        }
                    }
                    catch
                    {
                        return result = new WebApiResultEntity<UploadDingPicResponse> { success = false, message = "图片仅支持【.jpg;.jpeg;.png】" };
                    }
                }

                //去除前段部分非法区data:image/jpg;base64,
                List<string> legalFile = new List<string>();
                List<string> legalFileExt = new List<string>();
                foreach(var pic in encodeFile)
                {
                    try
                    {
                        legalFileExt.Add("."+pic.Split(';')[0].Split('/')[1]);
                        legalFile.Add(pic.Split(',')[1]);
                    }
                    catch(Exception ex)
                    {
                        return result = new WebApiResultEntity<UploadDingPicResponse> { success = false, message = "上传失败" };
                    }
                }

                List<DingPic> dingPicList = new List<DingPic>();
                DingPic dingPic = new DingPic();
                for (int i=0;i<legalFile.Count;i++)
                {
                    string remoteOid = Guid.NewGuid().ToString();
                    dingPic = new DingPic();
                    try
                    {
                        var serverPath = Path.Combine(Directory.CreateDirectory("userUploads").FullName, remoteOid + legalFileExt[i]);
                        string fileString = legalFile[i];
                        
                        byte[] bytes= Convert.FromBase64String(fileString);
                        using (var fileStream = new System.IO.FileStream(serverPath, System.IO.FileMode.OpenOrCreate))
                        {
                            fileStream.Write(bytes,0, bytes.Length);
                            fileStream.Flush();
                            fileStream.Close();
                        }

                        //上传图片
                        if (TencentHelper.UploadToTencent(serverPath, "/Image/MercuryAnswer" + "/" + remoteOid + legalFileExt[i]) != 0)
                        {
                            return result = new WebApiResultEntity<UploadDingPicResponse> { success = false, message = "上传失败" };
                        }

                        //删除本地图片
                        try
                        {
                            System.IO.File.Delete(serverPath);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        return result = new WebApiResultEntity<UploadDingPicResponse> { success = false, message = "上传失败" };
                    }
                    dingPic.OriginalName = "未知";
                    dingPic.RemoteOid = Guid.Parse(remoteOid);
                    dingPic.RemoteName = remoteOid +legalFileExt[i];
                    dingPicList.Add(dingPic);
                }
                UploadDingPicResponse uploadResult = new UploadDingPicResponse();
                uploadResult.RemoteCommonPath = imageUrl;
                uploadResult.DingPicList = new List<DingPic>();
                uploadResult.DingPicList = dingPicList;

                result = new WebApiResultEntity<UploadDingPicResponse> { success = true, message = "上传打卡图片成功", response = uploadResult };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<UploadDingPicResponse> { success = false, message = "系统错误" };
            }
            return result;
        }

        /// <summary>
        /// 查询用户今天打卡详情
        /// </summary>
        /// <param name="customerOid">用户OID</param>
        /// <returns></returns>
        [HttpGet("Today/{customerOid}")]
        public WebApiResultEntity<GetOneCustomerTodayDingResponse> GetOneCustomerTodayDing(Guid customerOid)
        {
            WebApiResultEntity<GetOneCustomerTodayDingResponse> result;
            try
            {
                var models = _answerAppService.GetOneCustomerTodayDing(customerOid);

                if (models == null)
                {
                    result = new WebApiResultEntity<GetOneCustomerTodayDingResponse> { success = true, message = "今天还未开始打卡",response=new GetOneCustomerTodayDingResponse() };
                    return result;
                }

                //增加打卡图片地址，整合Content中数据为String格式
                string imageUrl = Appsettings.app(new string[] { "FilesConfiguration", "ImageUrl" });
                foreach (var ding in models.DingList)
                {
                    if (ding.QuestionType == "PhotoGraph")
                    {
                        //处理图片地址问题
                        PhotoGraphHelper.PhotoGraphQuestionType(imageUrl, ding);
                    }
                    else
                    {
                        PhotoGraphHelper.OtherQuestionType(ding);
                    }
                }

                result = new WebApiResultEntity<GetOneCustomerTodayDingResponse> { success = true, message = "查询成功", response = models };
            }
            catch (Exception)
            {
                result = new WebApiResultEntity<GetOneCustomerTodayDingResponse> { success = false, message = "系统错误" };
            }
            return result;
        }
    }
}