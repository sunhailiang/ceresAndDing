using Ceres.Domain.Core.Commands;
using Ceres.Domain.Models;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Commands
{
    public abstract class CustomerDingCommand : Command
    {
        public Guid CustomerOid { get; protected set; }

        public List<MiddleDing> MiddleDingList { get; set; }
    }
}
