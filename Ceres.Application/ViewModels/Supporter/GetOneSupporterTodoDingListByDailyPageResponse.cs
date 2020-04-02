using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class GetOneSupporterTodoDingListByDailyPageResponse
    {
        /// <summary>
        /// 指定日期
        /// </summary>
        public DateTime TodoDate { get; set; }

        /// <summary>
        /// 归属客服
        /// </summary>
        public string SupporterName { get; set; }

        /// <summary>
        /// 每日打卡未完成列表
        /// </summary>
        public PageModel<TodoListByPage> TodoDingList { get; set; }
    }
}
