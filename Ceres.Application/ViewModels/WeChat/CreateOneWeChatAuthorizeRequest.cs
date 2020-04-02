using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ceres.Application.ViewModels
{
    public class CreateOneWeChatAuthorizeRequest
    {
        /// <summary>
        /// 随机数OID
        /// </summary>
        [Required(ErrorMessage = "随机数OID不能为空")]
        public Guid OID { get; set; }

        /// <summary>
        /// 微信Code
        /// </summary>
        [Required(ErrorMessage = "微信Code不能为空")]
        public string Code2Session { get; set; }
    }
}
