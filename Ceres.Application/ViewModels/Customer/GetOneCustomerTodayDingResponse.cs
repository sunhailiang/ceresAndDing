using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class GetOneCustomerTodayDingResponse
    {
        /// <summary>
        /// 打卡时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 打卡状态
        /// </summary>
        public int DingStatus { get; set; } = -1;


        /// <summary>
        /// 打卡状态描述
        /// </summary>
        public string DingStatusDescription { get; set; } = "今天未打卡，可以执行打卡";

        /// <summary>
        /// 打卡详情列表
        /// </summary>
        public List<Ding> DingList { get; set; }
    }
}
