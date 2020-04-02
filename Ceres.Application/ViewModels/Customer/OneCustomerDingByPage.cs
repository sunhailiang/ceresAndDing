using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{

    public class OneCustomerDingByPage
    {
        /// <summary>
        /// 打卡当前ID编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 打卡时间
        /// </summary>
        public DateTime DingTime { get; set; }

        /// <summary>
        /// 协助打卡人
        /// </summary>
        public string Assister { get; set; }

        /// <summary>
        /// 第一个回答问题的OID，用于追溯，助于删除本次打卡记录
        /// </summary>
        public Guid FirstAnswerGuid { get; set; }

        /// <summary>
        /// 打卡详情列表
        /// </summary>
        public List<Ding> DingList { get; set; }
    }
}
