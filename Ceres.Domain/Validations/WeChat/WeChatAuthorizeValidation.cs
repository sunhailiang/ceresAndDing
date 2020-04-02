using System;
using Ceres.Domain.Commands;
using FluentValidation;

namespace Ceres.Domain.Validations
{
    public abstract class WeChatAuthorizeValidation<T> : AbstractValidator<T> where T : WeChatAuthorizeCommand
    {
        //验证Guid
        protected void ValidateOID()
        {
            RuleFor(c => c.OID)
                .NotEqual(Guid.Empty);
        }

        protected void ValidateCode2Session()
        {
            RuleFor(c => c.Code2Session)
                .NotEmpty().WithMessage("授权Code不能为空");
        }

        protected void ValidateEncryptedData()
        {
            RuleFor(c => c.EncryptedData)
                .NotEmpty().WithMessage("加密数据不能为空");
        }

        protected void ValidateIV()
        {
            RuleFor(c => c.IV)
                .NotEmpty().WithMessage("加密算法的初始向量不能为空");
        }

        protected void ValidatePhoneJson()
        {
            RuleFor(c => c.PhoneJson)
                .NotEmpty().WithMessage("手机号Json不能为空");
        }
    }
}
