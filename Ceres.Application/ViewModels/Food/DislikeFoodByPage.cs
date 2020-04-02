using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    /// <summary>
    /// 不喜欢的食材
    /// </summary>
    public class DislikeFoodByPage
    {
        /// <summary>
        /// 食材当前ID编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 食材OID
        /// </summary>
        public Guid OID { get; set; }

        /// <summary>
        /// 食材名称
        /// </summary>
        public string Name { get; set; }

    }
}
