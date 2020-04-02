using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class DeleteOneCustomerDislikeFoodRequest
    {
        /// <summary>
        /// 客户OID
        /// </summary>
        [Required(ErrorMessage = "客户OID不能为空")]
        public Guid CustomerOid { get; set; }


        /// <summary>
        /// 不喜欢食材列表
        /// </summary>
        public List<Guid> DislikeFoodList { get; set; }
    }
}
