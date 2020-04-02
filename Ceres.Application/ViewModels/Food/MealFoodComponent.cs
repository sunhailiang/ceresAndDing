using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class MealFoodComponent
    {
        /// <summary>
        /// 每餐名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 每餐英文名称
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 每餐营养成分列表
        /// </summary>
        public List<FoodComponent> ComponentList { get; set; }
    }
}
