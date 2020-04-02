using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ceres.Application.ViewModels
{
    public class DeleteOneCustomerAssistDingRequest
    {
        /// <summary>
        /// 第一个回答问题的OID，用于追溯，助于删除本次打卡记录
        /// </summary>
        [Required(ErrorMessage = "打卡OID不能为空")]
        public Guid FirstAnswerGuid { get; set; }
    }
}
