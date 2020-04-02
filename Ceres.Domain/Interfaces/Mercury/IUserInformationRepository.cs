using Ceres.Domain.Models;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Interfaces
{
    public interface IUserInformationRepository : IRepository<UserInformation>
    {
        public UserInformation GetUserByPhoneNumber(string cellphone);
        public UserInformation GetUserByUserGuid(Guid userGuid);
    }
}
