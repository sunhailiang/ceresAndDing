using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class GetOneCustomerDailyEnergyResponse
    {
        /// <summary>
        /// 能量数值
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// 能量单位
        /// </summary>
        public string Unit { get; set; }
    }
}
