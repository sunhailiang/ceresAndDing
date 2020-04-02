using Ceres.Domain.Commands;
using FluentValidation;
using System;

namespace Ceres.Domain.Validations
{
    public class DeleteOneCustomerDietCommandValidation : CustomerDietValidation<DeleteOneCustomerDietCommand>
    {
        public DeleteOneCustomerDietCommandValidation()
        {
            ValidateOID();
            ValidateOperateOid();
        }
    }
}
