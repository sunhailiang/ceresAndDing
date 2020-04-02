using Ceres.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Ceres.Application.Interfaces
{
    public interface IWeChatAuthorizeAppService:IDisposable
    {
        //获取一个微信授权的Code信息
        GetOneWeChatAuthorizeResponse GetValidChatAuthorize(Guid oid);

        //更新授权加密数据
        void UpdateWeChatAuthorize(UpdateWeChatAuthorizeRequest request);

        //新增一个微信授权Code信息
        void CreateOneWeChatAuthorize(CreateOneWeChatAuthorizeRequest request);
    }
}
