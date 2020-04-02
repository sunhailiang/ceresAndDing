using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceres.UI.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ceres.UI.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //// WebApi����ע��
            //services.AddControllers();

            // MVC��ͼ
            services.AddControllersWithViews();

            // Automapper ע��
            services.AddAutoMapperSetup();

            // Adding MediatR for Domain Events
            // ������������¼���ע��
            // ���ð� MediatR.Extensions.Microsoft.DependencyInjection
            services.AddMediatR(typeof(Startup));

            // .NET Core ԭ������ע��
            // ��дһ����������������չʾ�� Presentation �и���
            // ������ʩ��Repository,xxxContext��Ӧ�ò�AppService
            NativeInjectorBootStrapper.RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //��̬�ļ�
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //// WebApi
                //endpoints.MapControllers();

                // MVC
                endpoints.MapControllerRoute("default", "{controller=Supporter}/{action=Create}/{id?}");

                //ȡ��Ĭ�����
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
            });


        }
    }
}
