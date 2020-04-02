using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class GetOneCustomerWXWeightListResponse
    {
        /// <summary>
        /// 归属客服名称
        /// </summary>
        public string SupporterName { get; set; }

        /// <summary>
        /// 归属客服手机号
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        /// 客户初次打卡体重（单位kg）
        /// </summary>
        public float InitWeight { get; set; }

        /// <summary>
        /// 客户初次打卡记录时间
        /// </summary>
        public DateTime InitRecordTime { get; set; }

        /// <summary>
        /// 体重列表
        /// </summary>
        public List<CustomerWeight> WeightList { get; set; }
    }
}
