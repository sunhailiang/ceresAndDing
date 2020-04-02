using Ceres.Domain.Commands;
using FluentValidation;
using System;

namespace Ceres.Domain.Validations
{
    public class UpdateWeChatAuthorizeCommandValidation : WeChatAuthorizeValidation<UpdateWeChatAuthorizeCommand>
    {
        public UpdateWeChatAuthorizeCommandValidation()
        {
            ValidateOID();
            ValidateEncryptedData();
            ValidateIV();
            ValidatePhoneJson();
        }
    }
}
