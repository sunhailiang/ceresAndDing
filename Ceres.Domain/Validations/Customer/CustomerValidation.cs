using System;
using Ceres.Domain.Commands;
using FluentValidation;

namespace Ceres.Domain.Validations
{
    public abstract class CustomerValidation<T> : AbstractValidator<T> where T : CustomerCommand
    {
        //验证Guid
        protected void ValidateOID()
        {
            RuleFor(c => c.OID)
                .NotEqual(Guid.Empty);
        }

        //验证UserName
        protected void ValidateUserName()
        {
            RuleFor(c => c.UserName)
                .NotEmpty().WithMessage("用户名不能为空")//判断不能为空，如果为空则显示Message
                .Length(2, 30).WithMessage("用户名在2~30个字符之间");
        }

        //验证地址
        protected void ValidateProvince()
        {
            RuleFor(c => c.Province)
                .NotEmpty().WithMessage("省不能为空")//判断不能为空，如果为空则显示Message
                .Length(1, 40).WithMessage("省份在1~40个字符之间");
        }

        protected void ValidateCity()
        {
            RuleFor(c => c.City)
                .NotEmpty().WithMessage("城市不能为空")//判断不能为空，如果为空则显示Message
                .Length(1, 40).WithMessage("城市在1~40个字符之间");
        }

        protected void ValidateHeight()
        {
            RuleFor(c => c.InitHeight)
                .NotEmpty().WithMessage("初始身高不能为空");//判断不能为空，如果为空则显示Message
        }

        protected void ValidateWeight()
        {
            RuleFor(c => c.InitWeight)
                .NotEmpty().WithMessage("初始体重不能为空");//判断不能为空，如果为空则显示Message
        }

        protected void ValidateCellphone()
        {
            RuleFor(c => c.Cellphone)
                .NotEmpty()
                .Must(HavePhone)
                .WithMessage("手机号应该为11位！");
        }
        protected void ValidateAgenterOid()
        {
            RuleFor(c => c.AgenterOid)
                .NotEqual(Guid.Empty).WithMessage("归属代理不能为空");
        }

        protected void ValidateSupporterOid()
        {
            RuleFor(c => c.SupporterOid)
                .NotEqual(Guid.Empty).WithMessage("客服不能为空");
        }

        protected void ValidateLastOperaterOid()
        {
            RuleFor(c => c.LastOperaterOid)
                .NotEqual(Guid.Empty).WithMessage("当前操作人员不能为空");
        }

        protected void ValidateJobName()
        {
            RuleFor(c => c.JobName)
                .NotEmpty().WithMessage("当前工作信息不能为空");
        }

        protected void ValidateJobStrength()
        {
            RuleFor(c => c.JobStrength)
                .NotEmpty().WithMessage("当前工作强度不能为空");
        }

        // 表达式
        protected static bool HavePhone(string phone)
        {
            return phone.Length == 11;
        }
    }
}
