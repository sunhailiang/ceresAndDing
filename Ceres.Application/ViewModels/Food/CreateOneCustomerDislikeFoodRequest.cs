using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class CreateOneCustomerDislikeFoodRequest
    {
        /// <summary>
        /// 客户OID
        /// </summary>
        [Required(ErrorMessage = "客户OID不能为空")]
        public Guid CustomerOid { get; set; }

        /// <summary>
        /// 客服OID
        /// </summary>
        [Required(ErrorMessage = "客服OID不能为空")]
        public Guid OperaterOid { get; set; }

        /// <summary>
        /// 不喜欢食材列表
        /// </summary>
        public List<Guid> DislikeFoodList { get; set; }
    }
}
