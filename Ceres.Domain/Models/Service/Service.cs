using Ceres.Domain.Core.Models;
using System;

namespace Ceres.Domain.Models
{
    public class Service : AggregationRoot
    {
        protected Service()
        { }

        public Service(Guid oid,string name,string description,string image, int status)
        {
            OID = oid;
            Name = name;
            Description = description;
            Image = image;
            Status = status;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Image { get; private set; }
        public int Status { get; private set; }
    }
}
