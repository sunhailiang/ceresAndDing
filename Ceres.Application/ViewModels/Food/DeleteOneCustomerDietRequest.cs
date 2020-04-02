using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ceres.Application.ViewModels
{
    public class DeleteOneCustomerDietRequest
    {
        /// <summary>
        /// 食谱OID
        /// </summary>
        [Required(ErrorMessage = "食谱OID不能为空")]
        public Guid OID { get; set; }

        /// <summary>
        /// 食谱删除原因
        /// </summary>
        [Required(ErrorMessage = "食谱删除原因不能为空")]
        public string DiscardReason { get; set; }

        /// <summary>
        /// 最后操作的客服OID
        /// </summary>
        [Required(ErrorMessage = "客服OID不能为空")]
        public Guid LastOperateOid { get; set; }

        /// <summary>
        /// 客户不喜欢的食物列表
        /// </summary>
        public List<DislikeFood> DislikeList { get; set; }
    }

    /// <summary>
    /// 客户不喜欢的食物
    /// </summary>
    public class DislikeFood
    {
        /// <summary>
        /// 食材OID
        /// </summary>
        public Guid FoodOid { get; set; }
    }
}
