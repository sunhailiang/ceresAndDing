using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    /// <summary>
    /// 营养成分
    /// </summary>
    public class FoodComponent
    {
        /// <summary>
        /// 营养成分名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 营养成分英文名称
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 营养成分简称
        /// </summary>
        public string NameCode { get; set; }

        /// <summary>
        /// 营养成分数值
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// 营养成分单位
        /// </summary>
        public string Unit { get; set; }
    }
}
