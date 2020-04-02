using System;
using Ceres.Domain.Commands;
using FluentValidation;

namespace Ceres.Domain.Validations
{
    public abstract class CustomerDietValidation<T> : AbstractValidator<T> where T : CustomerDietCommand
    {
        //验证Guid
        protected void ValidateOID()
        {
            RuleFor(c => c.OID)
                .NotEqual(Guid.Empty).WithMessage("当前配餐OID不能为空"); ; ;
        }

        protected void ValidateCustomerOid()
        {
            RuleFor(c => c.CustomerOid)
                .NotEqual(Guid.Empty).WithMessage("当前客户不能为空"); ;
        }

        protected void ValidateServiceOid()
        {
            RuleFor(c => c.ServiceOid)
                .NotEqual(Guid.Empty).WithMessage("当前服务信息不能为空"); ;
        }

        protected void ValidateRecommendDailyEnergy()
        {
            RuleFor(c => c.Recommend.DailyEnergy)
                .NotEmpty().WithMessage("每日推荐能量数值不能为空")
                .Must(CheckDailyEnergy)
                .WithMessage("每日推荐能量数值格式错误，应该为（xxkcal），包含单位");
        }

        protected void ValidateRecommendDailyComponentPercentage()
        {
            RuleFor(c => c.Recommend.DailyComponentPercentage)
                .NotEmpty().WithMessage("每日推荐营养成分列表不能为空");
        }

        protected void ValidateRecommendDailyFoodComponent()
        {
            RuleFor(c => c.Recommend.DailyFoodComponent)
                .NotEmpty().WithMessage("每日推荐营养成分不能为空");
        }

        protected void ValidateCurrentDailyEnergy()
        {
            RuleFor(c => c.Current.DailyEnergy)
                .NotEmpty().WithMessage("实际能量数值不能为空")
                .Must(CheckDailyEnergy)
                .WithMessage("实际能量数值格式错误，应该为（xxkcal），包含单位");
        }

        protected void ValidateCurrentDailyComponentPercentage()
        {
            RuleFor(c => c.Current.DailyComponentPercentage)
                .NotEmpty().WithMessage("实际营养成分列表不能为空");
        }

        protected void ValidateCurrentDiet()
        {
            RuleFor(c => c.CurrentDiet)
                .NotEmpty().WithMessage("当前推荐食材不能为空");
        }

        protected void ValidateSupporterOid()
        {
            RuleFor(c => c.SupporterOid)
                .NotEqual(Guid.Empty).WithMessage("客服人员不能为空"); ;
        }

        protected void ValidateOperateOid()
        {
            RuleFor(c => c.LastOperate.Oid)
                .NotEqual(Guid.Empty).WithMessage("当前操作人员不能为空"); ;
        }

        //表达式
        protected static bool CheckDailyEnergy(string dailyEnergy)
        {
            float temp=0f;
            return float.TryParse(dailyEnergy.Substring(0, dailyEnergy.Length - 4),out temp);
        }
    }
}
