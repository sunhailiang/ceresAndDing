using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class OneCustomerPhysiqueByPage
    {
        /// <summary>
        /// 当前ID编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime RecordTime { get; set; }

        /// <summary>
        /// 体质测试是否完整状态，0表示完整，-1表示不完整
        /// </summary>
        public int PhysiqueStatus { get; set; } = 0;

        /// <summary>
        /// 体质测试是否完整描述
        /// </summary>
        public string PhysiqueDescription { get; set; } = "完整";

        /// <summary>
        /// 体质结果列表
        /// </summary>
        public List<Physique> Physique { get; set; }
    }
}
