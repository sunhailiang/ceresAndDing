using Ceres.Domain.Core.Commands;
using Ceres.Domain.Models;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Commands
{
    public abstract class CustomerCommand : Command
    {
        public Guid OID { get; protected set; }
        public string UserName { get; protected set; }
        public int Sex { get; protected set; }
        public int Age { get; protected set; }
        public string Province { get; protected set; }
        public string City { get; protected set; }
        public string Cellphone { get; protected set; }
        public float InitHeight { get; protected set; }
        public float InitWeight { get; protected set; }
        public DateTime CreateTime { get; protected set; }
        public int Status { get; protected set; }
        public Guid AgenterOid { get; protected set; }
        public Guid SupporterOid { get; protected set; }
        public Guid LastOperaterOid { get; protected set; }
        public string JobName { get; protected set; }
        public string JobStrength { get; protected set; }
    }
}
