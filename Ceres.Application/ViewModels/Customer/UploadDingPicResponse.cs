using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.ViewModels
{
    public class UploadDingPicResponse
    {
        /// <summary>
        /// 上传图片的远程路径，不包括文件名称
        /// </summary>
        public string RemoteCommonPath { get; set; }

        /// <summary>
        /// 上传图片列表
        /// </summary>
        public List<DingPic> DingPicList { get; set; }
    }

    public class DingPic
    { 
        /// <summary>
        /// 原来上传的图片的名称，包括后缀
        /// </summary>
        public string OriginalName { get; set; }
        /// <summary>
        /// 远程图片OID
        /// </summary>
        public Guid RemoteOid { get; set; }

        /// <summary>
        /// 远程图片名称，包括后缀
        /// </summary>
        public string RemoteName { get; set; }
    }

}
