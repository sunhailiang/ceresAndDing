using Ceres.Domain.Models;
using Ceres.Domain.Validations;
using System;

namespace Ceres.Domain.Commands
{
    public class CreateOneCustomerCommand:CustomerCommand
    {
        public Guid ServiceOid { get; private set; }
        public CreateOneCustomerCommand(Guid oid,string userName, int sex,int age, string province,string city,float initHeight, float initWeight,
            Guid agenterOid,Guid supporterOid, Guid lastOperaterOid,string jobName,string jobStrength,Guid serviceOid)
        {
            OID = oid;
            UserName = userName;
            Sex = sex;
            Age = age;
            Province = province;
            City = city;
            InitHeight = initHeight;
            InitWeight = initWeight;
            AgenterOid = agenterOid;
            SupporterOid = supporterOid;
            LastOperaterOid = lastOperaterOid;
            JobName = jobName;
            JobStrength = jobStrength;
            ServiceOid = serviceOid;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreateOneCustomerCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
