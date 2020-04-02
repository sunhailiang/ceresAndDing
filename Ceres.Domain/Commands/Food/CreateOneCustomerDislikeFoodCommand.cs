using Ceres.Domain.Models;
using Ceres.Domain.Validations;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Commands
{
    public class CreateOneCustomerDislikeFoodCommand: CustomerDislikeFoodCommand
    {
        
        public CreateOneCustomerDislikeFoodCommand(Guid customerOid, List<Guid> dislikeFoodList,Guid operaterOid)
        {
            CustomerOid = customerOid;
            DislikeFoodList = dislikeFoodList;
            OperaterOid = operaterOid;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreateOneCustomerDislikeFoodCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
