using Ceres.Domain.Commands;
using FluentValidation;
using System;

namespace Ceres.Domain.Validations
{
    public class DeleteOneCustomerAssistDingCommandValidation : CustomerAssistDingValidation<DeleteOneCustomerAssistDingCommand>
    {
        public DeleteOneCustomerAssistDingCommandValidation()
        {
            ValidateOID();
        }
    }
}
