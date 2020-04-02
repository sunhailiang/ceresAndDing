using Ceres.Domain.Commands;
using FluentValidation;
using System;

namespace Ceres.Domain.Validations
{
    public class CreateOneCustomerCommandValidation : CustomerValidation<CreateOneCustomerCommand>
    {
        public CreateOneCustomerCommandValidation()
        {
            ValidateOID();
            ValidateUserName();
            ValidateProvince();
            ValidateCity();
            ValidateHeight();
            ValidateWeight();
            //ValidateCellphone();//手机号不验证，从数据库中获取
            ValidateAgenterOid();
            ValidateSupporterOid();
            ValidateLastOperaterOid();
            ValidateJobName();
            ValidateJobStrength();

            RuleFor(c => c.ServiceOid)//额外的验证
                .NotEqual(Guid.Empty).WithMessage("服务OID不能为空");
        }
    }
}
