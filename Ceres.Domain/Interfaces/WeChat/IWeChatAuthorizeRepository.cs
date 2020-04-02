using Ceres.Domain.Models;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Interfaces
{
    public interface IWeChatAuthorizeRepository:IRepository<WeChatAuthorize>
    {
        public WeChatAuthorize GetValidWeChatAuthorize(Guid oid);
    }
}
