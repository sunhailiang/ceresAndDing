using Ceres.Domain.Core.Models;
using System;


namespace Ceres.Domain.Models
{
    public class CustomerAssistDing : AggregationRoot
    {
        protected CustomerAssistDing()
        { }

        public CustomerAssistDing(Guid oid,Guid questionnaireGuid,Guid customerOid,Guid supporterOid,DateTime assistTime,DateTime createTime,int status)
        {
            OID = oid;
            QuestionnaireGuid = questionnaireGuid;
            CustomerOid = customerOid;
            SupporterOid = supporterOid;
            AssistTime = assistTime;
            CreateTime = createTime;
            Status = status;
        }

        public Guid QuestionnaireGuid { get; private set; }
        public Guid CustomerOid { get; private set; }
        public Guid SupporterOid { get; private set; }
        public DateTime AssistTime { get; private set; }//15秒内都认为是协助打卡
        public DateTime CreateTime { get; private set; }//创建时间
        public int Status { get; private set; }//协助打卡状态，0-正常，-1-打卡失效

        public void UpdateStatus(int status)
        {
            this.Status = status;
        }
    }
}
