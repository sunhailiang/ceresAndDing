using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class OneCustomerMecuryAnswerByPage
    {
        /// <summary>
        /// 问答开始时间
        /// </summary>
        public DateTime AnswerTime { get; set; }


        /// <summary>
        /// 第一个回答问题的OID，用于追溯，助于删除本次打卡记录
        /// </summary>
        public Guid FirstAnswerGuid { get; set; }

        /// <summary>
        /// 问答详情列表
        /// </summary>
        public List<MecuryAnswer> MecuryAnswerList { get; set; }
    }
}
