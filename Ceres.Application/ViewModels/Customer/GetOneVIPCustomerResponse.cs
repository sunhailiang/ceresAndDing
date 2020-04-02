using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class GetOneVIPCustomerResponse
    {
        /// <summary>
        /// 客户OID
        /// </summary>
        public Guid OID { get; set; }

        /// <summary>
        /// 客户手机号
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 客户默认身高（单位cm）
        /// </summary>
        public double DefaultHeight { get; set; } = 0f;

        /// <summary>
        /// 当前用户是否为VIP，默认为非VIP
        /// </summary>
        public bool IsVip { get; set; } = false;

        /// <summary>
        /// 当前用户是否为VIP的描述，非VIP无法打卡
        /// </summary>
        public string IsVipDescription { get; set; } = "当前用户不是VIP，无法进行打卡";
    }
}
