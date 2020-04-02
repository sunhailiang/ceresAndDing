using Ceres.Domain.Commands;
using FluentValidation;
using System;

namespace Ceres.Domain.Validations
{
    public class CreateOneWeChatAuthorizeCommandValidation : WeChatAuthorizeValidation<CreateOneWeChatAuthorizeCommand>
    {
        public CreateOneWeChatAuthorizeCommandValidation()
        {
            ValidateOID();
            ValidateCode2Session();
        }
    }
}
