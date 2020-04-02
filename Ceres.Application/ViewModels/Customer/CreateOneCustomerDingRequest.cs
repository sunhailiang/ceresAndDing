using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ceres.Application.ViewModels
{
    public class CreateOneCustomerDingRequest
    {
        /// <summary>
        /// 客户OID
        /// </summary>
        [Required(ErrorMessage = "客户OID不能为空")]
        public Guid CustomerOid { get; set; }

        /// <summary>
        /// 打卡列表
        /// </summary>
        public List<MiddleDing> MiddleDingList { get; set; }
    }
}
