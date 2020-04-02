using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class QueryOneCustomerDingListByPageResponse
    {
        /// <summary>
        /// 客户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 客户手机号
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 打卡列表
        /// </summary>
        public PageModel<OneCustomerDingByPage> DingList { get; set; }
    }
}
