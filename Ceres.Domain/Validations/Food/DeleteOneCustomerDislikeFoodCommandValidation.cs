using Ceres.Domain.Commands;
using FluentValidation;
using System;

namespace Ceres.Domain.Validations
{
    public class DeleteOneCustomerDislikeFoodCommandValidation : CustomerDislikeFoodCommandValidation<DeleteOneCustomerDislikeFoodCommand>
    {
        public DeleteOneCustomerDislikeFoodCommandValidation()
        {
            ValidateCustomerOid();
        }
    }
}
