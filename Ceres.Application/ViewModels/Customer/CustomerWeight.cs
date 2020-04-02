using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class CustomerWeight
    {
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime RecordTime { get; set; }

        /// <summary>
        /// 体重数值，单位kg，范围为30kg～300kg，不在范围内，按照0kg处理
        /// </summary>
        public float Weight { get; set; }
    }
}
