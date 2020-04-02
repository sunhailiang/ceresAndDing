using Ceres.Domain.Interfaces;
using Ceres.Domain.Models;
using Ceres.Infrastruct.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ceres.Infrastruct.Repository
{
    public class ComponentRepository : Repository<Component>, IComponentRepository
    {
        public ComponentRepository(CeresContext context)
            : base(context)
        {

        }
    }
}
