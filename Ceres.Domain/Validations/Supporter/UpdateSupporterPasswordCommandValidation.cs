using Ceres.Domain.Commands;
using FluentValidation;

namespace Ceres.Domain.Validations
{
    public class UpdateSupporterPasswordCommandValidation : SupporterValidation<UpdateSupporterPasswordCommand>
    {
        public UpdateSupporterPasswordCommandValidation()
        {
            ValidateOID();//验证OID
            ValidatePassword();//验证密码

            //验证额外的东西
            RuleFor(c => c.NewPassword)
                .NotEmpty().WithMessage("新密码不能为空！")
                .Length(5, 30).WithMessage("新密码在5~30个字符之间");

        }
    }
}
