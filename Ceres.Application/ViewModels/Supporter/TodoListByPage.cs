using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class TodoListByPage
    {
        /// <summary>
        /// 客户OID
        /// </summary>
        public Guid UserOid { get; set; }

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
    }
}
