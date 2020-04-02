﻿using Ceres.Domain.Models;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Interfaces
{
    public interface ICustomerJobRepository : IRepository<CustomerJob>
    {
        public CustomerJob GetCustomerJobByCustomerOid(Guid customerOid);
    }
}
