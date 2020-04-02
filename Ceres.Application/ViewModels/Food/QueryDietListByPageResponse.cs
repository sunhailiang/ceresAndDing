using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class QueryDietListByPageResponse
    {
        /// <summary>
        /// 食谱列表
        /// </summary>
        public PageModel<DietByPage> DietList { get; set; }
    }
}
