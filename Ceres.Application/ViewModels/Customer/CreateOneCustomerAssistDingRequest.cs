using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ceres.Application.ViewModels
{
    public class CreateOneCustomerAssistDingRequest
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
        public Guid SupporterOid { get; set; }

        /// <summary>
        /// 协助打卡日期
        /// </summary>
        [Required(ErrorMessage = "协助打卡日期不能为空")]
        public DateTime AssistDate { get; set; }

        /// <summary>
        /// 打卡列表
        /// </summary>
        public List<MiddleDing> AssistDing { get; set; }
    }

    public class MiddleDing
    {
        /// <summary>
        /// 问题OID
        /// </summary>
        public Guid QuestionOID { get; set; }

        /// <summary>
        /// 回答
        /// </summary>
        public string AnswerContent { get; set; }
    }
}
