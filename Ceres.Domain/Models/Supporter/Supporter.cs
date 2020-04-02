using Ceres.Domain.Core.Models;
using System;

namespace Ceres.Domain.Models
{
    public class Supporter:AggregationRoot
    {
        protected Supporter()
        { }

        public Supporter(Guid oid, string loginName, string cellphone, string password, string userName, string image,DateTime createTime, int status)
        {
            OID = oid;
            LoginName = loginName;
            Cellphone = cellphone;
            Password = password;
            UserName = userName;
            Image = image;
            CreateTime = createTime;
            Status = status;
        }

        public string LoginName { get; private set; }
        public string Cellphone { get; private set; }
        public string Password { get; private set; }
        public string UserName { get; private set; }
        public string Image { get; private set; }
        public DateTime CreateTime { get; private set; }
        public int Status { get; private set; }

        public void UpdatePassword(string password)
        {
            this.Password = password;
        }

        
    }
}
