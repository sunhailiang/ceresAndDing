using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class CustomerHeight
    {
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime RecordTime { get; set; }

        /// <summary>
        /// 身高数值，单位cm，范围为50cm～300cm，不在范围内，按照0cm处理
        /// </summary>
        public float Height { get; set; }
    }
}
