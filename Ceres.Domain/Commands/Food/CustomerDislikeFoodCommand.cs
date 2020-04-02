using Ceres.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Domain.Commands
{
    public abstract class CustomerDislikeFoodCommand : Command
    {
        public Guid OID { get; protected set; }
        public Guid CustomerOid { get; protected set; }
        public Guid FoodOid { get; protected set; }
        public Guid OperaterOid { get; protected set; }
        public DateTime CreateTime { get; protected set; }
        public List<Guid> DislikeFoodList { get;protected set; }
    }
}
