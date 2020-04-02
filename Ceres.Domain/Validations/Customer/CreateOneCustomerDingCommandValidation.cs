using Ceres.Domain.Commands;
using FluentValidation;
using System;

namespace Ceres.Domain.Validations
{
    public class CreateOneCustomerDingCommandValidation : CustomerDingValidation<CreateOneCustomerDingCommand>
    {
        public CreateOneCustomerDingCommandValidation()
        {
            ValidateCustomerOid();
        }
    }
}
