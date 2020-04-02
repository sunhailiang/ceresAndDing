using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class UploadDingPicRequest
    {
        /// <summary>
        /// 上传的Base64数组
        /// </summary>
        public List<string> Base64Pic { get; set; }
    }
}
