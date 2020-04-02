using Ceres.Domain.Core.Commands;
using Ceres.Domain.Models;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Commands
{
    public abstract class CustomerDietCommand : Command
    {
        public Guid OID { get; protected set; }
        public Guid CustomerOid { get; protected set; }
        public Guid ServiceOid { get; protected set; }
        public Recommend Recommend { get; protected set; }
        public Current Current { get; protected set; }
        public string CurrentDiet { get; protected set; }
        public Guid SupporterOid { get; protected set; }
        public int Status { get; protected set; }
        public Discard Discard { get; protected set; }
        public DateTime CreateTime { get; protected set; }
        public SupportOperate LastOperate { get; protected set; }
        public List<CustomerDislikeFood> DislikeList { get; set; }
    }
}
