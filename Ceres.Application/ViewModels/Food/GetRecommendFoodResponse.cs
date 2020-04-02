using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class GetRecommendFoodResponse
    {
        /// <summary>
        /// 食材类型列表
        /// </summary>
        public List<ClassifyFood> ClassifyFoodList { get; set; }
    }
}
