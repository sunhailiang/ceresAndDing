using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class GetOneCustomerTodayPhysiqueResponse
    {
        /// <summary>
        /// 体脂测试时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 体脂测试状态
        /// </summary>
        public int PhysiqueStatus { get; set; } = -1;


        /// <summary>
        /// 体脂测试状态描述
        /// </summary>
        public string PhysiqueStatusDescription { get; set; } = "今天未完成体脂测试，可以执行体脂测试";
    }
}
