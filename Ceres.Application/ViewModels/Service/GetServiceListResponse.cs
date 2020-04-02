using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class GetServiceListResponse
    {
        /// <summary>
        /// 服务OID
        /// </summary>
        public Guid OID { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 服务描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 服务图片
        /// </summary>
        public string Image { get; set; }
    }
}
