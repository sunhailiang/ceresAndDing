using System;
using Ceres.Domain.Commands;
using FluentValidation;

namespace Ceres.Domain.Validations
{
    public abstract class CustomerDingValidation<T> : AbstractValidator<T> where T : CustomerDingCommand
    {
        protected void ValidateCustomerOid()
        {
            RuleFor(c => c.CustomerOid)
                .NotEqual(Guid.Empty).WithMessage("当前客户不能为空");
        }
    }
}
