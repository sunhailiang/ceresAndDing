using Ceres.Domain.Core.Models;
using System;

namespace Ceres.Domain.Models
{
    public class CustomerDiet : AggregationRoot
    {
        protected CustomerDiet()
        { }

        public CustomerDiet(Guid oid,Guid customerOid,Guid serviceOid,Recommend recommend, Current current, string currentDiet,Guid supporterOid,int status,Discard discard,DateTime createTime,SupportOperate lastOperate)
        {
            OID = oid;
            CustomerOid = customerOid;
            ServiceOid = serviceOid;
            Recommend = recommend;
            Current = current;
            CurrentDiet = currentDiet;
            SupporterOid = supporterOid;
            Status = status;
            Discard = discard;
            CreateTime = createTime;
            LastOperate = lastOperate;
        }

        public CustomerDiet(Guid oid, Guid customerOid, Guid serviceOid, Recommend recommend,Current current, string currentDiet, Guid supporterOid, int status, DateTime createTime, SupportOperate lastOperate)
        {
            OID = oid;
            CustomerOid = customerOid;
            ServiceOid = serviceOid;
            Recommend = recommend;
            Current = current;
            CurrentDiet = currentDiet;
            SupporterOid = supporterOid;
            Status = status;
            CreateTime = createTime;
            LastOperate = lastOperate;
        }

        public Guid CustomerOid { get; private set; }
        public Guid ServiceOid { get; private set; }
        public Recommend Recommend { get; private set; }
        public Current Current { get; private set; }
        public string CurrentDiet { get; private set; }
        public Guid SupporterOid { get; private set; }
        public int Status { get; private set; }
        public Discard Discard { get; private set; }
        public DateTime CreateTime { get; private set; }
        public SupportOperate LastOperate { get; private set; }

        /// <summary>
        /// 更新废弃原因
        /// </summary>
        /// <param name="discard"></param>
        public void UpdateDiscard(Discard discard,SupportOperate lastoperate)
        {
            this.Status = -1;
            this.Discard = discard;
            this.LastOperate = lastoperate;
        }
    }

    public class SupportOperate : ValueObject<SupportOperate>
    {
        public Guid Oid { get; private set; }
        public DateTime Time { get; private set; }

        protected SupportOperate()
        { }
        public SupportOperate(Guid oid,DateTime time)
        {
            Oid = oid;
            Time = time;
        }

        protected override bool EqualsCore(SupportOperate other)
        {
            throw new NotImplementedException();
        }
    }

    public class Discard : ValueObject<Discard>
    {
        public string Reason { get; private set; }
        protected Discard()
        { }

        public Discard(string reason)
        {
            Reason = reason;
        }

        protected override bool EqualsCore(Discard other)
        {
            throw new NotImplementedException();
        }
    }

    public class Recommend : ValueObject<Recommend>
    {
        public string DailyEnergy { get; private set; }
        public string DailyComponentPercentage { get; private set; }
        public string DailyFoodComponent { get; private set; }

        protected Recommend()
        { }
        public Recommend(string dailyEnergy, string dailyComponentPercentage,string dailyFoodComponent)
        {
            DailyEnergy = dailyEnergy;
            DailyComponentPercentage = dailyComponentPercentage;
            DailyFoodComponent = dailyFoodComponent;
        }

        protected override bool EqualsCore(Recommend other)
        {
            throw new NotImplementedException();
        }
    }

    public class Current : ValueObject<Current>
    {
        public string DailyEnergy { get; private set; }
        public string DailyComponentPercentage { get; private set; }

        
        protected Current()
        { }
        public Current(string dailyEnergy, string dailyComponentPercentage)
        {
            DailyEnergy = dailyEnergy;
            DailyComponentPercentage = dailyComponentPercentage;
        }

        protected override bool EqualsCore(Current other)
        {
            throw new NotImplementedException();
        }
    }


}
