using System;
using System.ComponentModel.DataAnnotations;

namespace Ceres.Application.ViewModels
{
    public class CreateOneCustomerRequest
    {
        /// <summary>
        /// 客户OID
        /// </summary>
        [Required(ErrorMessage = "客户OID不能为空")]
        public Guid OID { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [Required(ErrorMessage = "客户名称不能为空")]
        [MinLength(2, ErrorMessage = "客户名称长度至少2位")]
        [MaxLength(30, ErrorMessage = "客户名称长度最多30位")]
        public string UserName { get; set; }

        /// <summary>
        /// 客户性别
        /// </summary>
        [Required(ErrorMessage = "性别不能为空")]
        [Range(0,1,ErrorMessage ="性别为0-女性，或者1-男性")]
        public int Sex { get; set; }

        /// <summary>
        /// 客户年龄
        /// </summary>
        [Required(ErrorMessage = "年龄不能为空")]
        [Range(10, 150, ErrorMessage = "年龄范围为10～150岁")]
        public int Age { get; set; }

        /// <summary>
        /// 所在省
        /// </summary>
        [Required(ErrorMessage = "客户所在省不能为空")]
        public string Province { get; set; }

        /// <summary>
        /// 所在城市
        /// </summary>
        [Required(ErrorMessage = "客户所在城市不能为空")]
        public string City { get; set; }

        /// <summary>
        /// 客户身高（单位cm）
        /// </summary>
        [Range(50,300,ErrorMessage ="身高单位cm,范围为50cm～300cm")]
        public float InitHeight { get; set; }

        /// <summary>
        /// 客户体重（单位kg）
        /// </summary>
        [Range(30, 300, ErrorMessage = "体重单位kg,范围为30kg～300kg")]
        public float InitWeight { get; set; }

        /// <summary>
        /// 客服Oid
        /// </summary>
        [Required(ErrorMessage = "客服OID不能为空")]
        public Guid SupporterOid { get; set; }

        /// <summary>
        /// 工作名称
        /// </summary>
        [Required(ErrorMessage = "工作名称不能为空")]
        public string JobName { get; set; }

        /// <summary>
        /// 工作强度等级
        /// </summary>
        [Required(ErrorMessage = "工作强度等级不能为空")]
        [Range(1, 4, ErrorMessage = "工作强度等级，1-较轻体力，2-轻体力，3-中体力，4-重体力")]
        public string JobStrength { get; set; }

        /// <summary>
        /// 客户服务OID
        /// </summary>
        [Required(ErrorMessage = "客户服务OID不能为空")]
        public Guid ServiceOid { get; set; }

        /// <summary>
        /// 归属代理OID
        /// </summary>
        [Required(ErrorMessage = "归属代理OID不能为空")]
        public Guid AgenterOid { get; set; }
    }
}
