using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    /// <summary>
    /// 问答详情
    /// </summary>
    public class MecuryAnswer
    {
        /// <summary>
        /// 问题的OID
        /// </summary>
        public Guid QuestionGuid { get; set; }

        /// <summary>
        /// 问题
        /// </summary>
        public string Question { get; set; }

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
