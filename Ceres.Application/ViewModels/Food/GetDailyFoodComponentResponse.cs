using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class GetDailyFoodComponentResponse
    {
        /// <summary>
        /// 每日营养成分列表
        /// </summary>
        public List<MealFoodComponent> MealList { get; set; }
    }
}
