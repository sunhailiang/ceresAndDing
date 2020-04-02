using Ceres.Domain.Models;
using Ceres.Domain.Validations;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Commands
{
    public class CreateOneWeChatAuthorizeCommand:WeChatAuthorizeCommand
    {
        public CreateOneWeChatAuthorizeCommand(Guid oid, string code2Session)
        {
            OID = oid;
            Code2Session = code2Session;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreateOneWeChatAuthorizeCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
