using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class GetComponentPercentageByDailyEnergyResponse
    {
        /// <summary>
        /// 每日营养成分列表
        /// </summary>
        public List<FoodComponent> ComponentList { get; set; }
    }
}
