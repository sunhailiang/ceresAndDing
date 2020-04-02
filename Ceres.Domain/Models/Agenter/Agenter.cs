using Ceres.Domain.Core.Models;
using System;

namespace Ceres.Domain.Models
{
    public class Agenter : AggregationRoot
    {
        protected Agenter()
        { }

        public Agenter(Guid oid, string userName,int sex,string cellphone,string image,AgenterAddress address, DateTime createTime, int status)
        {
            OID = oid;
            UserName = userName;
            Sex = sex;
            Cellphone = cellphone;
            Image = image;
            Address = address;
            CreateTime = createTime;
            Status = status;
        }

        public string UserName { get; private set; }
        public int Sex { get; private set; }
        public string Cellphone { get; private set; }
        public string Image { get; private set; }
        public AgenterAddress Address { get; private set; }
        public DateTime CreateTime { get; private set; }
        public int Status { get; private set; }
    }

    public class AgenterAddress : ValueObject<AgenterAddress>
    {
        public string Province { get; private set; }
        public string City { get; private set; }
        protected AgenterAddress()
        { }
        public AgenterAddress(string province, string city)
        {
            Province = province;
            City = city;
        }
        protected override bool EqualsCore(AgenterAddress other)
        {
            throw new NotImplementedException();
        }
    }
}
