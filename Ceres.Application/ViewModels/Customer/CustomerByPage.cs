using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class CustomerByPage
    {
        /// <summary>
        /// 客户OID
        /// </summary>
        public Guid OID { get; set; }

        /// <summary>
        /// 客户当前ID编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 客户手机号
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        /// 客户性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 客户年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 所在省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 所在市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 服务类别
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 归属客服
        /// </summary>
        public string SupporterName { get; set; }

        /// <summary>
        /// 归属代理
        /// </summary>
        public string AgenterName { get; set; }

    }
}
