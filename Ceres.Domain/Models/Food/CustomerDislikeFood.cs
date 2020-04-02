using Ceres.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Models
{
    public class CustomerDislikeFood : AggregationRoot
    {
        protected CustomerDislikeFood()
        { }
        public CustomerDislikeFood(Guid oid,Guid customerOid,Guid foodOid,Guid operaterOid,DateTime createTime)
        {
            OID = oid;
            CustomerOid = customerOid;
            FoodOid = foodOid;
            OperaterOid = operaterOid;
            CreateTime = createTime;
        }

        public CustomerDislikeFood(Guid foodOid)
        {
            FoodOid = foodOid;
        }

        public Guid CustomerOid { get; private set; }
        public Guid FoodOid { get; private set; }
        public Guid OperaterOid { get; private set;}
        public DateTime CreateTime { get; private set; }

        public void UpdateCreateTime(DateTime createTime)
        {
            this.CreateTime = createTime;
        }
    }
}
