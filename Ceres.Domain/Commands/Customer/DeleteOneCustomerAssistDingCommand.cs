using Ceres.Domain.Models;
using Ceres.Domain.Validations;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Commands
{
    public class DeleteOneCustomerAssistDingCommand : CustomerAssistDingCommand
    {
        public DeleteOneCustomerAssistDingCommand(Guid oid)
        {
            OID = oid;
        }

        public override bool IsValid()
        {
            ValidationResult = new DeleteOneCustomerAssistDingCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
