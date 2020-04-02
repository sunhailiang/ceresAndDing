using Ceres.Domain.Interfaces;
using Ceres.Domain.Models;
using Ceres.Infrastruct.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ceres.Infrastruct.Repository
{
    public class FoodRepository : Repository<Food>, IFoodRepository
    {
        public FoodRepository(CeresContext context)
            : base(context)
        {

        }

        public IEnumerable<Food> GetFoodByClassify(string classify,int click)
        {
            return DbSet.AsNoTracking().Where(c => c.Classify == classify &&c.Click==click) ;
        }

        public IEnumerable<Food> QueryFoodListByPage(string foodName, int pageIndex, int pageSize)
        {
            return DbSet.AsNoTracking().Where(c => c.Status == 0&&c.Name.Contains(foodName)).OrderByDescending(c => c.Click).Skip(pageIndex * pageSize).Take(pageSize); ;
        }

        public int QueryFoodListCount(string foodName)
        {
            return DbSet.AsNoTracking().Where(c => c.Status == 0 && c.Name.Contains(foodName)).Count();
        }
    }
}
