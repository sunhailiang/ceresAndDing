using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class QueryFoodListWithDislikeFlagByPageResponse
    {
        /// <summary>
        /// 食材列表
        /// </summary>
        public PageModel<FoodWithDislikeFlagByPage> FoodList { get; set; }
    }

    public class FoodWithDislikeFlagByPage
    {
        /// <summary>
        /// 食材当前ID编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 食材OID
        /// </summary>
        public Guid OID { get; set; }

        /// <summary>
        /// 食材名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 食材重要性
        /// </summary>
        public int Click { get; set; }

        /// <summary>
        /// 客户是否喜欢状态，0表示正常，-1表示客户不喜欢
        /// </summary>
        public int CustomerDislikeStatus { get; set; } = 0;

        /// <summary>
        /// 客户是否喜欢描述
        /// </summary>
        public string CustomerDislikeDescription { get; set; } = "正常";
    }
}
