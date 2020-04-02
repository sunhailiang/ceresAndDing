using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.WebApi
{
    public class WebApiResultEntity<T>
    {
        /// <summary>
        /// 操作是否成功，response是否可以解析
        /// </summary>
        public bool success { get; set; } = false;
        /// <summary>
        /// 令牌
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; } = "服务器异常";
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T response { get; set; }
    }
}
