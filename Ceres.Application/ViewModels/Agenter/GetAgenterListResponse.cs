using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class GetAgenterListResponse
    {
        /// <summary>
        /// 代理OID
        /// </summary>
        public Guid OID { get; set; }

        /// <summary>
        /// 代理名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 代理性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 代理手机号
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        /// 代理头像
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 所在省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 所在市
        /// </summary>
        public string City { get; set; }
    }
}
