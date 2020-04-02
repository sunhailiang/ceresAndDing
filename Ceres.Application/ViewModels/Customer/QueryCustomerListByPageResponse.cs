using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class QueryCustomerListByPageResponse
    {
        /// <summary>
        /// 客户列表
        /// </summary>
        public PageModel<CustomerByPage> CustomerList { get; set; }
        
    }
}
