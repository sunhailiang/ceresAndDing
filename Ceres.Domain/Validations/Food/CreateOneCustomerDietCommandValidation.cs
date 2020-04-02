using Ceres.Domain.Commands;
using FluentValidation;
using System;

namespace Ceres.Domain.Validations
{
    public class CreateOneCustomerDietCommandValidation : CustomerDietValidation<CreateOneCustomerDietCommand>
    {
        public CreateOneCustomerDietCommandValidation()
        {
            ValidateOID();
            ValidateCustomerOid();
            ValidateServiceOid();
            ValidateRecommendDailyEnergy();
            ValidateRecommendDailyComponentPercentage();
            ValidateRecommendDailyFoodComponent();
            ValidateCurrentDailyEnergy();
            ValidateCurrentDailyComponentPercentage();
            ValidateCurrentDiet();
            ValidateSupporterOid();
        }
    }
}
