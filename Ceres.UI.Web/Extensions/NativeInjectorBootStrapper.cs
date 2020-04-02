using Ceres.Application.Interfaces;
using Ceres.Application.Services;
using Ceres.Domain.CommandHandler;
using Ceres.Domain.Commands;
using Ceres.Domain.Core.Bus;
using Ceres.Domain.Interfaces;
using Ceres.Infrastruct.Bus;
using Ceres.Infrastruct.Context;
using Ceres.Infrastruct.Repository;
using Ceres.Infrastruct.UoW;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ceres.UI.Web.Extensions
{
    //依赖注入
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {

            // 注入 应用层Application
            services.AddScoped<IBomFoodMainAppService, BomFoodMainAppService>();
            services.AddScoped<ISupporterAppService, SupporterAppService>();
            

            // 命令总线Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            // 领域层 - 领域命令
            // 将命令模型和命令处理程序匹配
            services.AddScoped<IRequestHandler<UpdateSupporterPasswordCommand, Unit>, SupporterCommandHandler>();
            services.AddScoped<IRequestHandler<GetAllSupporterCommand, Unit>, SupporterCommandHandler>();



            // 注入 基础设施层 - 数据层
            services.AddScoped<IBomFoodMainRepository, BomFoodMainRepository>();
            services.AddScoped<BomContext>();//上下文

            //services.AddScoped<ISptSupporterMainRepository, SptSupporterMainRepository>();
            services.AddScoped<SupporterContext>();//上下文
            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }
    }
}
