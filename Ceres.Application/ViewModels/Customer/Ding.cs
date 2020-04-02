using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    /// <summary>
    /// 打卡详情
    /// </summary>
    public class Ding
    {
        /// <summary>
        /// 打卡问题的OID
        /// </summary>
        public Guid QuestionGuid { get; set; }

        /// <summary>
        /// 问题
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// 问题类型
        /// </summary>
        public string QuestionType { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// 回答时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
