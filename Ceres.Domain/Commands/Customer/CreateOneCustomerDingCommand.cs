using Ceres.Domain.Models;
using Ceres.Domain.Validations;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Commands
{
    public class CreateOneCustomerDingCommand : CustomerDingCommand
    {
        public CreateOneCustomerDingCommand(Guid customerOid)
        {
            CustomerOid = customerOid;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreateOneCustomerDingCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }
}
