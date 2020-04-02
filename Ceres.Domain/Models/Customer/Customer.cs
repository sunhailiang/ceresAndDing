using Ceres.Domain.Core.Models;
using System;

namespace Ceres.Domain.Models
{
    public class Customer:AggregationRoot
    {
        protected Customer()
        { }

        public Customer(Guid oid, string userName,int sex,int age, CustomerAddress address,string cellphone,float initHeight,float initWeight,
            Guid agenterOid,Guid supporterOid, Guid lastOperaterOid, DateTime createTime, int status)
        {
            OID = oid;
            UserName = userName;
            Sex = sex;
            Age = age;
            Address = address;
            Cellphone = cellphone;
            InitHeight = initHeight;
            InitWeight = initWeight;
            AgenterOid = agenterOid;
            SupporterOid = supporterOid;
            LastOperaterOid = lastOperaterOid;
            CreateTime = createTime;
            Status = status;

        }

        public string UserName { get; private set; }
        public int Sex { get; private set; }
        public int Age { get; private set; }
        public CustomerAddress Address { get; private set; }
        public string Cellphone { get; private set; }
        public float InitHeight { get; private set; }
        public float InitWeight { get; private set; }
        public DateTime CreateTime { get; private set; }
        public int Status { get; private set; }
        public Guid AgenterOid { get; private set; }
        public Guid SupporterOid { get; private set; }
        public Guid LastOperaterOid { get; private set; }
    }

    public class CustomerAddress : ValueObject<CustomerAddress>
    {
        public string Province { get; private set; }
        public string City { get; private set; }
        protected CustomerAddress()
        { }
        public CustomerAddress(string province, string city)
        {
            Province = province;
            City = city;
        }
        protected override bool EqualsCore(CustomerAddress other)
        {
            throw new NotImplementedException();
        }
    }
}
