using Ceres.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Domain.Models
{
    public class CustomerJob: AggregationRoot
    {
        protected CustomerJob()
        { }

        public CustomerJob(Guid oid,Job job,Guid customerOid)
        {
            OID = oid;
            Job = job;
            CustomerOid = customerOid;
        }

        public Job Job { get; private set; }
        public Guid CustomerOid { get; private set; }
    }
    public class Job : ValueObject<Job>
    {
        public string Name { get; private set; }
        public string Strength { get; private set; }
        protected Job()
        { }
        public Job(string name, string strength)
        {
            Name = name;
            Strength = strength;
        }

        protected override bool EqualsCore(Job other)
        {
            throw new NotImplementedException();
        }
    }
}
