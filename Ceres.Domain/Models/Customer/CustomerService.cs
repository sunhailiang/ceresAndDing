using Ceres.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Models
{
    public class CustomerService: AggregationRoot
    {
        protected CustomerService()
        { }
        public CustomerService(Guid oid,Guid customerOid,Guid servicerOid,int status)
        {
            OID = oid;
            CustomerOid = customerOid;
            ServiceOid = servicerOid;
            Status = status;
        }

        public Guid CustomerOid { get; private set; }
        public Guid ServiceOid { get; private set; }
        public int Status { get; private set; }
    }
}
