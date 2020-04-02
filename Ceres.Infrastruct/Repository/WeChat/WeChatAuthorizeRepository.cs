using Ceres.Domain.Interfaces;
using Ceres.Domain.Models;
using Ceres.Infrastruct.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ceres.Infrastruct.Repository
{
    public class WeChatAuthorizeRepository : Repository<WeChatAuthorize>, IWeChatAuthorizeRepository
    {
        public WeChatAuthorizeRepository(CeresContext context)
            : base(context)
        {

        }

        public WeChatAuthorize GetValidWeChatAuthorize(Guid oid)
        {
            return DbSet.AsNoTracking().Where(c=>c.OID==oid &&c.CreateTime.AddMinutes(10)>=DateTime.Now).FirstOrDefault();
        }
    }
}
