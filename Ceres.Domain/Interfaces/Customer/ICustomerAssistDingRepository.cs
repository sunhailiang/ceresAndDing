using Ceres.Domain.Models;
using System;

namespace Ceres.Domain.Interfaces
{
    public interface ICustomerAssistDingRepository : IRepository<CustomerAssistDing>
    {
        public CustomerAssistDing GetByCustomerOid(Guid questionnaireGuid,Guid customerOid, DateTime startTime);

        //客服最后操作的协助打卡
        public CustomerAssistDing GetLastestOperateBySupporter(Guid supporterOid);

        //客户最新的打卡记录
        public CustomerAssistDing GetLastestAssistDing(Guid customerOid);
    }
}
