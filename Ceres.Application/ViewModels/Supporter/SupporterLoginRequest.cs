using System;
using System.ComponentModel.DataAnnotations;

namespace Ceres.Application.ViewModels
{
    public class SupporterLoginRequest
    {

        /// <summary>
        /// 客服用户名，或者客服电话号码
        /// </summary>
        [Required(ErrorMessage = "客服用户名不能为空")]
        [MinLength(2, ErrorMessage = "客服用户名长度至少2位")]
        [MaxLength(30, ErrorMessage = "客服用户名长度最多30位")]
        public string LoginID { get; set; }
        /// <summary>
        /// 客服密码
        /// </summary>
        [Required(ErrorMessage = "客服密码不能为空")]
        [MinLength(2, ErrorMessage = "客服密码长度至少2位")]
        [MaxLength(40, ErrorMessage = "客服密码长度最多40位")]
        public string Password { get; set; }
    }
}
