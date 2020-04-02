using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    /// <summary>
    /// 食谱
    /// </summary>
    public class Diet
    {
        /// <summary>
        /// 食谱OID
        /// </summary>
        public Guid OID { get; set; }

        /// <summary>
        /// 食谱当前ID编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 每日推荐能量数值（xxkcal），包含单位
        /// </summary>
        public string RecommendDailyEnergy { get; set; }

        /// <summary>
        /// 实际能量数值（xxkcal），包含单位
        /// </summary>
        public string CurrentDailyEnergy { get; set; }

        /// <summary>
        /// 当前推荐食材，包括早中晚，Json数据
        /// </summary>
        public string CurrentDiet { get; set; }

        /// <summary>
        /// 客服名称
        /// </summary>
        public string SupporterName { get; set; }

        /// <summary>
        /// 食谱状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 食谱状态描述
        /// </summary>
        public string StatusDescription { get; set; }

        /// <summary>
        /// 食谱备注
        /// </summary>
        public string DietNote { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后操作人
        /// </summary>
        public string LastOperater { get; set; }

        /// <summary>
        /// 最后操作时间
        /// </summary>
        public DateTime LatOperateTime { get; set; }
    }
}
