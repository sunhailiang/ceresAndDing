using Ceres.Domain.Models;
using Ceres.Domain.Validations;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Commands
{
    public class DeleteOneCustomerDietCommand : CustomerDietCommand
    {
        public DeleteOneCustomerDietCommand(Guid oid,Discard discard,SupportOperate supportOperate)
        {
            OID = oid;
            Discard = discard;
            LastOperate = supportOperate;
        }

        public override bool IsValid()
        {
            ValidationResult = new DeleteOneCustomerDietCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
