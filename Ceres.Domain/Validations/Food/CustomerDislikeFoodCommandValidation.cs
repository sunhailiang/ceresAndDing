using System;
using Ceres.Domain.Commands;
using FluentValidation;

namespace Ceres.Domain.Validations
{
    public abstract class CustomerDislikeFoodCommandValidation<T> : AbstractValidator<T> where T : CustomerDislikeFoodCommand
    {
        protected void ValidateCustomerOid()
        {
            RuleFor(c => c.CustomerOid)
                .NotEqual(Guid.Empty).WithMessage("当前客户不能为空"); ;
        }

        protected void ValidateOperaterOid()
        {
            RuleFor(c => c.OperaterOid)
                .NotEqual(Guid.Empty).WithMessage("操作人员不能为空"); ;
        }
    }
}
