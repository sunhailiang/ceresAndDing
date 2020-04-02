using Ceres.Domain.Interfaces;
using Ceres.Domain.Models;
using Ceres.Infrastruct.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ceres.Infrastruct.Repository
{
    public class UserInformationRepository : Repository<UserInformation>, IUserInformationRepository
    {
        public UserInformationRepository(CeresContext context)
            : base(context)
        {

        }

        public UserInformation GetUserByPhoneNumber(string cellphone)
        {
            return DbSet.AsNoTracking().Where(c => c.PhoneNumber == cellphone).OrderByDescending(c => c.RecordID).FirstOrDefault();
        }

        public UserInformation GetUserByUserGuid(Guid userGuid)
        {
            return DbSet.AsNoTracking().Where(c => c.UserGuid == userGuid).OrderByDescending(c => c.RecordID).FirstOrDefault();
        }
    }
}
