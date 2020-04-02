using Ceres.Domain.Models;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Interfaces
{
    public interface IFoodRepository : IRepository<Food>
    {
        public IEnumerable<Food> GetFoodByClassify(string classify,int click);

        public IEnumerable<Food> QueryFoodListByPage(string foodName,int pageIndex, int pageSize);

        public int QueryFoodListCount(string foodName);
    }
}
