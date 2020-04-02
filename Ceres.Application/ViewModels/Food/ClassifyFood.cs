using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    /// <summary>
    /// 食材类型
    /// </summary>
    public class ClassifyFood
    {
        /// <summary>
        /// 食材分类
        /// </summary>
        public string Classify { get; set; }

        /// <summary>
        /// 食材列表
        /// </summary>
        public List<Food> FoodList { get; set; }
    }
}
