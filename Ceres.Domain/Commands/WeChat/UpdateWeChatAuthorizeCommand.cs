using Ceres.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Domain.Commands
{
    public class UpdateWeChatAuthorizeCommand:WeChatAuthorizeCommand
    {
        public UpdateWeChatAuthorizeCommand(Guid oid,string encryptedData,string iv,string phoneJson)
        {
            OID = oid;
            EncryptedData = encryptedData;
            IV = iv;
            PhoneJson = phoneJson;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateWeChatAuthorizeCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
