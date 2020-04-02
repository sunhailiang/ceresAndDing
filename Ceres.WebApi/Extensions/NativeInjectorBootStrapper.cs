using Ceres.Application.Interfaces;
using Ceres.Application.Services;
using Ceres.Domain.CommandHandler;
using Ceres.Domain.Commands;
using Ceres.Domain.Core.Bus;
using Ceres.Domain.Core.Notifications;
using Ceres.Domain.Interfaces;
using Ceres.Infrastruct.Bus;
using Ceres.Infrastruct.Context;
using Ceres.Infrastruct.Repository;
using Ceres.Infrastruct.UoW;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ceres.WebApi.Extensions
{
    // 依赖注入
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {

            // 注入 应用层Application
            services.AddScoped<ISupporterAppService, SupporterAppService>();
            services.AddScoped<IAgenterAppService, AgenterAppService>();
            services.AddScoped<IUserInformationAppService, UserInformationAppService>();
            services.AddScoped<IServiceAppService, ServiceAppService>();
            services.AddScoped<ICustomerAppService, CustomerAppService>();
            services.AddScoped<IAnswerAppService, AnswerAppService>();
            services.AddScoped<ICompoundFoodAppService, CompoundFoodAppService>();
            services.AddScoped<IFoodAppService, FoodAppService>();
            services.AddScoped<IDietAppService, DietAppService>();
            services.AddScoped<IWeChatAuthorizeAppService, WeChatAuthorizeAppService>();

            // 命令总线Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            // Domain - Events
            // 将事件模型和事件处理程序匹配注入

            // 领域通知
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // 领域层 - 领域命令
            // 将命令模型和命令处理程序匹配
            services.AddScoped<IRequestHandler<UpdateSupporterPasswordCommand, Unit>, SupporterCommandHandler>();

            services.AddScoped<IRequestHandler<CreateOneCustomerCommand, Unit>, CustomerCommandHandler>();

            services.AddScoped<IRequestHandler<CreateOneCustomerDietCommand, Unit>, CustomerDietCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteOneCustomerDietCommand, Unit>, CustomerDietCommandHandler>();
            services.AddScoped<IRequestHandler<CreateOneCustomerDislikeFoodCommand, Unit>, CustomerDietCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteOneCustomerDislikeFoodCommand, Unit>, CustomerDietCommandHandler>();

            services.AddScoped<IRequestHandler<CreateOneCustomerAssistDingCommand, Unit>, CustomerDingCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteOneCustomerAssistDingCommand, Unit>, CustomerDingCommandHandler>();

            services.AddScoped<IRequestHandler<CreateOneCustomerDingCommand, Unit>, CustomerDingCommandHandler>();

            services.AddScoped<IRequestHandler<CreateOneWeChatAuthorizeCommand, Unit>, WeChatAuthorizeCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateWeChatAuthorizeCommand, Unit>, WeChatAuthorizeCommandHandler>();

            // 领域层 - Memory
            services.AddSingleton<IMemoryCache>(factory =>
            {
                var cache = new MemoryCache(new MemoryCacheOptions());
                return cache;
            });


            // 注入 基础设施层 - 数据层
            services.AddScoped<IAgenterRepository, AgenterRepository>();

            services.AddScoped<ISupporterRepository, SupporterRepository>();

            services.AddScoped<IServiceRepository, ServiceRepository>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerJobRepository, CustomerJobRepository>();

            services.AddScoped<ICustomerAssistDingRepository, CustomerAssistDingRepository>();

            services.AddScoped<ICustomerServiceRepository, CustomerServiceRepository>();

            services.AddScoped<IFoodRepository, FoodRepository>();
            services.AddScoped<IComponentRepository, ComponentRepository>();
            services.AddScoped<IFoodComponentRepository, FoodComponentRepository>();

            services.AddScoped<ICustomerDislikeFoodRepository, CustomerDislikeFoodRepository>();
            services.AddScoped<ICustomerDietRepository, CustomerDietRepository>();

            services.AddScoped<IWeChatAuthorizeRepository, WeChatAuthorizeRepository>();

            // Mercury系统中的数据
            services.AddScoped<IUserInformationRepository, UserInformationRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<IQuestionnaireRepository, QuestionnaireRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();

            // 工作单元
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // 上下文
            services.AddScoped<CeresContext>();
        }
    }
}
