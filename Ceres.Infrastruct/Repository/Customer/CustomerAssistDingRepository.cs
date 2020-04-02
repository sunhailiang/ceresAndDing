using Ceres.Domain.Interfaces;
using Ceres.Domain.Models;
using Ceres.Infrastruct.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ceres.Infrastruct.Repository
{
    public class CustomerAssistDingRepository : Repository<CustomerAssistDing>, ICustomerAssistDingRepository
    {
        public CustomerAssistDingRepository(CeresContext context)
            : base(context)
        {

        }

        public CustomerAssistDing GetByCustomerOid(Guid questionnaireGuid, Guid customerOid, DateTime startTime)
        {
            var result = DbSet.AsNoTracking()
                .Where
                (
                c => c.QuestionnaireGuid == questionnaireGuid
                && c.CustomerOid == customerOid
                && c.AssistTime >= startTime
                && c.AssistTime <= startTime.AddMinutes(1)
                && c.Status==0
                )
                .OrderByDescending(c => c.CreateTime).LastOrDefault()//原因是当有2个或者多个查询到时，应该获取时间早的那个
                ;
            return result;
        }

        public CustomerAssistDing GetLastestAssistDing(Guid customerOid)
        {
            return DbSet.AsNoTracking()
                .Where
                (
                c => c.CustomerOid == customerOid
                )
                .OrderByDescending(c => c.CreateTime).FirstOrDefault();
        }

        public CustomerAssistDing GetLastestOperateBySupporter(Guid supporterOid)
        {
            return DbSet.AsNoTracking()
                .Where
                (
                c => c.SupporterOid == supporterOid
                )
                .OrderByDescending(c => c.CreateTime).FirstOrDefault();
        }
    }
}
