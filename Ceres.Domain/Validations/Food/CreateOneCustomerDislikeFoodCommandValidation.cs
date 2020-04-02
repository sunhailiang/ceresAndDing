using Ceres.Domain.Commands;
using FluentValidation;
using System;

namespace Ceres.Domain.Validations
{
    public class CreateOneCustomerDislikeFoodCommandValidation:CustomerDislikeFoodCommandValidation<CreateOneCustomerDislikeFoodCommand>
    {
        public CreateOneCustomerDislikeFoodCommandValidation()
        {
            ValidateCustomerOid();
            ValidateOperaterOid();
        }
    }
}
