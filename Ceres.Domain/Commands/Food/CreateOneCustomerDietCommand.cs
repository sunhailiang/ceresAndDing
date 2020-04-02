using Ceres.Domain.Models;
using Ceres.Domain.Validations;
using System;

namespace Ceres.Domain.Commands
{
    public class CreateOneCustomerDietCommand: CustomerDietCommand
    {
        public CreateOneCustomerDietCommand(Guid dietOid,Guid customerOid, Guid serviceOid, Recommend recommend,Current current, string currentDiet, Guid supporterOid)
        {
            OID = dietOid;
            CustomerOid = customerOid;
            ServiceOid = serviceOid;
            Recommend = recommend;
            Current = current;
            CurrentDiet = currentDiet;
            SupporterOid = supporterOid;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreateOneCustomerDietCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
