using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class QueryDislikeFoodListByPageResponse
    {
        /// <summary>
        /// 不喜欢的食材列表
        /// </summary>
        public PageModel<DislikeFoodByPage> DislikeFoodList { get; set; }
    }
}
