using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class GetOneWeChatAuthorizeResponse
    {
        /// <summary>
        /// 随机数OID
        /// </summary>
        public Guid OID { get; set; }

        /// <summary>
        /// 微信Code，Json数据
        /// </summary>
        public string Code2Session { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
