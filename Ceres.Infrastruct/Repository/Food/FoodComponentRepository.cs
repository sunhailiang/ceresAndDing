using Ceres.Domain.Interfaces;
using Ceres.Domain.Models;
using Ceres.Infrastruct.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ceres.Infrastruct.Repository
{
    public class FoodComponentRepository : Repository<FoodComponent>, IFoodComponentRepository
    {
        public FoodComponentRepository(CeresContext context)
            : base(context)
        {

        }

        public IEnumerable<FoodComponent> GetFoodComponentByFoodOid(Guid foodOid)
        {
            return DbSet.AsNoTracking().Where(c => c.FoodOid == foodOid && c.Status == 0);
        }
    }
}
