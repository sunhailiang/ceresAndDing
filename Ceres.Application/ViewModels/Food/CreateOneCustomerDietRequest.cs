using System;
using System.ComponentModel.DataAnnotations;

namespace Ceres.Application.ViewModels
{
    public class CreateOneCustomerDietRequest
    {
        /// <summary>
        /// 当前配餐OID
        /// </summary>
        [Required(ErrorMessage = "当前配餐OID不能为空")]
        public Guid DietOid { get; set; }

        /// <summary>
        /// 客户OID
        /// </summary>
        [Required(ErrorMessage = "客户OID不能为空")]
        public Guid CustomerOid { get; set; }

        /// <summary>
        /// 服务OID
        /// </summary>
        [Required(ErrorMessage = "服务OID不能为空")]
        public Guid ServiceOid { get; set; }

        /// <summary>
        /// 每日推荐能量数值（xxkcal），包含单位
        /// </summary>
        [Required(ErrorMessage = "每日推荐能量数值不能为空")]
        public string RecommendDailyEnergy { get; set; }

        /// <summary>
        /// 每日推荐营养成分列表，包括3大营养素百分比，Json数据
        /// </summary>
        [Required(ErrorMessage = "每日推荐3大营养素百分比不能为空")]
        public string RecommendDailyComponentPercentage { get; set; }

        /// <summary>
        /// 每日推荐营养成分，包括早中晚3大营养素g数，Json数据
        /// </summary>
        [Required(ErrorMessage = "每日推荐早中晚3大营养素g数不能为空")]
        public string RecommendDailyFoodComponent { get; set; }

        /// <summary>
        /// 实际能量数值（xxkcal），包含单位
        /// </summary>
        [Required(ErrorMessage = "实际能量数值不能为空")]
        public string CurrentDailyEnergy { get; set; }

        /// <summary>
        /// 实际营养成分列表，包括3大营养素百分比，查询字符串拼接方式
        /// </summary>
        [Required(ErrorMessage = "实际3大营养素百分比不能为空")]
        public string CurrentDailyComponentPercentage { get; set; }

        /// <summary>
        /// 当前推荐食材，包括早中晚，Json数据
        /// </summary>
        [Required(ErrorMessage = "当前推荐食材不能为空")]
        public string CurrentDiet { get; set; }

        /// <summary>
        /// 客服OID
        /// </summary>
        [Required(ErrorMessage = "客服OID不能为空")]
        public Guid SupporterOid { get; set; }
    }
}
