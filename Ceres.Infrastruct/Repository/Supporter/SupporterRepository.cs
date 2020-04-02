using Ceres.Domain.Interfaces;
using Ceres.Domain.Models;
using Ceres.Infrastruct.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ceres.Infrastruct.Repository
{
    public class SupporterRepository : Repository<Supporter>, ISupporterRepository
    {
        public SupporterRepository(CeresContext context)
            : base(context)
        {

        }

        public IEnumerable<Supporter> GetAllValidSupporters()
        {
            return DbSet.AsNoTracking().Where(c => c.Status==0);
        }

        public Supporter GetByCellphone(string cellphone,string password)
        {
            return DbSet.AsNoTracking().FirstOrDefault(c => c.Cellphone == cellphone && c.Password == password);
        }

        public Supporter GetByLoginName(string loginName, string password)
        {
            return DbSet.AsNoTracking().FirstOrDefault(c => c.LoginName == loginName && c.Password == password);
        }

        public Supporter GetByOID(Guid oid, string password)
        {
            return DbSet.AsNoTracking().FirstOrDefault(c => c.OID == oid && c.Password == password);
        }

        public Supporter GetSupporterBySupporterName(string supporterName)
        {
            return DbSet.AsNoTracking().Where(c => c.UserName == supporterName).FirstOrDefault();
        }
    }
}
