using Ceres.Domain.Models;
using Ceres.Domain.Validations;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Commands
{
    public class CreateOneCustomerAssistDingCommand : CustomerAssistDingCommand
    {
        public CreateOneCustomerAssistDingCommand(Guid customerOid, Guid supporterOid,DateTime assistTime)
        {
            CustomerOid = customerOid;
            SupporterOid = supporterOid;
            AssistTime = assistTime;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreateOneCustomerAssistDingCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
