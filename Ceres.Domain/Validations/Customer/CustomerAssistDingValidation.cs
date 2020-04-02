using System;
using Ceres.Domain.Commands;
using FluentValidation;

namespace Ceres.Domain.Validations
{
    public abstract class CustomerAssistDingValidation<T> : AbstractValidator<T> where T : CustomerAssistDingCommand
    {
        //验证Guid
        protected void ValidateOID()
        {
            RuleFor(c => c.OID)
                .NotEqual(Guid.Empty);
        }

        protected void ValidateQuestionnaireGuid()
        {
            RuleFor(c => c.QuestionnaireGuid)
                .NotEqual(Guid.Empty).WithMessage("当前打卡问题集不能为空");
        }

        protected void ValidateCustomerOid()
        {
            RuleFor(c => c.CustomerOid)
                .NotEqual(Guid.Empty).WithMessage("当前客户不能为空");
        }

        protected void ValidateSupporterOid()
        {
            RuleFor(c => c.SupporterOid)
                .NotEqual(Guid.Empty).WithMessage("当前客服不能为空");
        }

        protected void ValidateCreateTime()
        {
            RuleFor(c => c.CreateTime)
                .NotNull().NotEmpty().WithMessage("协助打卡日期不能为空");
        }
    }
}
