using Ceres.Domain.Commands;
using FluentValidation;
using System;

namespace Ceres.Domain.Validations
{
    public class CreateOneCustomerAssistDingCommandValidation : CustomerAssistDingValidation<CreateOneCustomerAssistDingCommand>
    {
        public CreateOneCustomerAssistDingCommandValidation()
        {
            ValidateCustomerOid();
            ValidateSupporterOid();
        }
    }
}
