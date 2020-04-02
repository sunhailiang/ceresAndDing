using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class UpdateSupporterPasswordRequest
    {
        /// <summary>
        /// 客服OID
        /// </summary>
        [Required(ErrorMessage = "客服OID不能为空")]
        public Guid OID { get; set; }

        /// <summary>
        /// 客服旧密码
        /// </summary>
        [Required(ErrorMessage = "客服密码不能为空")]
        [MinLength(2, ErrorMessage = "客服密码长度至少2位")]
        [MaxLength(40, ErrorMessage = "客服密码长度最多40位")]
        public string OldPassword { get; set; }

        /// <summary>
        /// 客服新密码
        /// </summary>
        [Required(ErrorMessage = "客服密码不能为空")]
        [MinLength(2, ErrorMessage = "客服密码长度至少2位")]
        [MaxLength(40, ErrorMessage = "客服密码长度最多40位")]
        public string NewPassword { get; set; }
    }
}
