using Ceres.Domain.Core.Models;
using System;

namespace Ceres.Domain.Models
{
    public class FoodComponent : AggregationRoot
    {
        protected FoodComponent()
        { }

        public FoodComponent(Guid oid,Guid foodOid,Guid componentOid,float value,int status)
        {
            OID = oid;
            FoodOid = foodOid;
            ComponentOid = componentOid;
            Value = value;
            Status = status;
        }

        public Guid FoodOid { get; private set; }
        public Guid ComponentOid { get; private set; }
        public float Value { get; private set; }
        public int Status { get; private set; }
    }
}
