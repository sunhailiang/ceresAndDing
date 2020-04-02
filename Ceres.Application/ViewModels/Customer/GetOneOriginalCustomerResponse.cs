using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class GetOneOriginalCustomerResponse
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
        /// 客户性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 客户年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 客户身高（单位cm）
        /// </summary>
        public double InitHeight { get; set; } 

        /// <summary>
        /// 客户体重（单位kg）
        /// </summary>
        public double InitWeight { get; set; }

        /// <summary>
        /// 所在省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 所在市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 当前用户是否为VIP
        /// </summary>
        public bool IsVip { get; set; }
    }
}
