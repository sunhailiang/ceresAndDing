using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class QueryFoodListByPageResponse
    {
        /// <summary>
        /// 食材列表
        /// </summary>
        public PageModel<FoodByPage> FoodList { get; set; }
    }
}
