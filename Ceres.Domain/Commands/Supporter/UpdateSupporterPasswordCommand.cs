using Ceres.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Domain.Commands
{
    public class UpdateSupporterPasswordCommand : SupporterCommand
    {
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; protected set; }

        public UpdateSupporterPasswordCommand(Guid oid, string oldPassword, string newPasswrod)
        {
            OID = oid;
            Password = oldPassword;
            NewPassword = newPasswrod;
        }
        public override bool IsValid()
        {
            ValidationResult = new UpdateSupporterPasswordCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
