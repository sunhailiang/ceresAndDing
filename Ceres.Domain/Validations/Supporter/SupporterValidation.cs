using System;
using Ceres.Domain.Commands;
using FluentValidation;

namespace Ceres.Domain.Validations
{
    /// <summary>
    /// 定义基于 SupporterCommand 的抽象基类 SupporterValidation
    /// 继承 抽象类 AbstractValidator
    /// 注意需要引用 FluentValidation
    /// 注意这里的 T 是命令模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SupporterValidation<T>: AbstractValidator<T> where T : SupporterCommand
    {
        //验证Guid
        protected void ValidateOID()
        {
            RuleFor(c => c.OID)
                .NotEqual(Guid.Empty);
        }

        //验证LoginName
        protected void ValidateLoginName()
        {
            //定义规则，c 就是当前 SupporterCommand 类
            RuleFor(c => c.LoginName)
                .NotEmpty().WithMessage("登录名不能为空")//判断不能为空，如果为空则显示Message
                .Length(2, 30).WithMessage("姓名在2~30个字符之间");//定义 LoginName 的长度
        }

        //验证手机号
        protected void ValidateCellphone()
        {
            RuleFor(c => c.Cellphone)
                .NotEmpty()
                .Must(HavePhone)
                .WithMessage("手机号应该为11位！");
        }

        //验证密码
        protected void ValidatePassword()
        {
            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("密码不能为空！");
        }

        // 表达式
        protected static bool HavePhone(string phone)
        {
            return phone.Length == 11;
        }
    }
}
