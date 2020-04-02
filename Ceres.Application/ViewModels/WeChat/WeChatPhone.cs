using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    /// <summary>
    /// 微信手机号结构
    /// </summary>
    public class WeChatPhone<T>
    {
        /// <summary>
        /// 用户绑定的手机号（国外手机号会有区号）
        /// </summary>
        public string phoneNumber { get; set; }

        /// <summary>
        /// 没有区号的手机号
        /// </summary>
        public string purePhoneNumber { get; set; }

        /// <summary>
        /// 区号
        /// </summary>
        public string countryCode { get; set; }
        public T watermark { get; set; }
    }

    /// <summary>
    /// 微信水印
    /// </summary>
    public class Watermark
    {
        public string appid { get; set; }
        public string timestamp { get; set; }
    }
}
