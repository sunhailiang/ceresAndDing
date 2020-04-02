using System;
using System.ComponentModel.DataAnnotations;

namespace Ceres.Application.ViewModels
{
    public class UpdateWeChatAuthorizeRequest
    {
        /// <summary>
        /// 随机数OID
        /// </summary>
        [Required(ErrorMessage = "随机数OID不能为空")]
        public Guid RandomString { get; set; }

        /// <summary>
        /// 加密数据
        /// </summary>
        [Required(ErrorMessage = "加密数据不能为空")]
        public string EncryptedData { get; set; }

        /// <summary>
        /// 加密算法的初始向量
        /// </summary>
        [Required(ErrorMessage = "加密算法的初始向量不能为空")]
        public string IV { get; set; }

        /// <summary>
        /// 手机号Json
        /// </summary>
        [Required(ErrorMessage = "手机号Json不能为空")]
        public string PhoneJson { get; set; }
    }
}
