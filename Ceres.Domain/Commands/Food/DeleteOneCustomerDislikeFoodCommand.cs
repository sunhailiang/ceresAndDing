using Ceres.Domain.Models;
using Ceres.Domain.Validations;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Commands
{
    public class DeleteOneCustomerDislikeFoodCommand:CustomerDislikeFoodCommand
    {
        public DeleteOneCustomerDislikeFoodCommand(Guid customerOid, List<Guid> dislikeFoodList)
        {
            CustomerOid = customerOid;
            DislikeFoodList = dislikeFoodList;
        }

        public override bool IsValid()
        {
            ValidationResult = new DeleteOneCustomerDislikeFoodCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
