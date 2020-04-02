using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class QueryOneCustomerPhysiqueListByPageResponse
    {
        /// <summary>
        /// 体质测试结果列表
        /// </summary>
        public PageModel<OneCustomerPhysiqueByPage> PhysiqueList { get; set; }
    }
}
