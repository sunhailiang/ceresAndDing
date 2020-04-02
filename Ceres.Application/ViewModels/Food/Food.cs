using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    /// <summary>
    /// 食材
    /// </summary>
    public class Food
    {
        /// <summary>
        /// 食材OID
        /// </summary>
        public Guid OID { get; set; }

        /// <summary>
        /// 食材名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 食材数值
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// 食材单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 食材重要性
        /// </summary>
        public int Click { get; set; }

        /// <summary>
        /// 食材营养成分列表
        /// </summary>
        public List<FoodComponent> FoodComponentList { get; set; }
        
    }
}
