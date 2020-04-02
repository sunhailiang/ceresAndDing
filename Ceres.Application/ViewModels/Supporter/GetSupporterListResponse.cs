using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class GetSupporterListResponse
    {
        /// <summary>
        /// 客服OID
        /// </summary>
        public Guid OID { get; set; }

        /// <summary>
        /// 客服用户名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 客服真实名称，非客服用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 客服头像
        /// </summary>
        public string Image { get; set; }
    }
}
