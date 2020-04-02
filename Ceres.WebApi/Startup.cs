using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceres.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using System.IO;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ceres.WebApi.AuthHelper;
using Microsoft.AspNetCore.Rewrite;

namespace Ceres.WebApi
{
    public class Startup
    {
        public string ApiName { get; set; } = "Ceres WebApi";

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            //设置跨域
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
                
            });


            services.AddSingleton(new Appsettings(Env.ContentRootPath));


            #region Swagger
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            services.AddSwaggerGen(c =>
             {
                 c.SwaggerDoc("V1", new OpenApiInfo
                 {
                     // {ApiName} 定义成全局变量，方便修改
                     Version = "V1",
                     Title = $"{ApiName} 接口文档——Netcore 3.0",
                     Description = $"{ApiName} HTTP API V1",
                 });
                 c.OrderActionsBy(o => o.RelativePath);

                 var xmlPath = Path.Combine(basePath, "Ceres.WebApi.xml");//这个就是刚刚配置的xml文件名
                 c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                 var xmlModelPath = Path.Combine(basePath, "Ceres.Application.xml");//这个就是Model层的xml文件名
                 c.IncludeXmlComments(xmlModelPath);

                 #region Token绑定到ConfigureServices
                 // 开启加权小锁
                 c.OperationFilter<AddResponseHeadersFilter>();
                 c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                 // 在header中添加token，传递到后台
                 c.OperationFilter<SecurityRequirementsOperationFilter>();


                 // 必须是 oauth2
                 c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                 {
                     Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                     Name = "Authorization",//jwt默认的参数名称
                     In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                     Type = SecuritySchemeType.ApiKey
                 });
                 #endregion
             });
            #endregion


            #region 基于策略的授权
            // 1【授权】、这个和上边的异曲同工，好处就是不用在controller中，写多个 roles 。
            // 然后这么写 [Authorize(Policy = "Admin")]
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Supporter", policy => policy.RequireRole("Supporter").Build());
                //options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                //options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                //options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));
                //options.AddPolicy("A_S_O", policy => policy.RequireRole("Admin", "System", "Others"));
                //options.AddPolicy("2个且", policy => policy.RequireRole("Admin").RequireRole("System"));
            });
            #endregion

            //读取配置文件
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            #region JWT【认证3+2】
            //JWT【认证3+2】
            services.AddAuthentication(x =>
            {
                //看这个单词熟悉么？没错，就是上边错误里的那个。
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })// 也可以直接写字符串，AddAuthentication("Bearer")
             .AddJwtBearer(o =>
             {
                 o.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,//密钥认证
                     IssuerSigningKey = signingKey,//参数配置在下边

                     ValidateIssuer = true,
                     ValidIssuer = audienceConfig["Issuer"],//发行人

                     ValidateAudience = true,
                     ValidAudience = audienceConfig["Audience"],//订阅人

                     ValidateLifetime = true,//验证生命周期
                     ClockSkew = TimeSpan.Zero,
                     RequireExpirationTime = true,//验证过期时间
                 };

             });
            #endregion


            // MVC视图
            //services.AddControllersWithViews();

            // Automapper 注入
            services.AddAutoMapperSetup();

            // Adding MediatR for Domain Events
            // 领域命令、领域事件等注入
            // 引用包 MediatR.Extensions.Microsoft.DependencyInjection
            services.AddMediatR(typeof(Startup));

            // .NET Core 原生依赖注入
            // 单写一层用来添加依赖项，从展示层 Presentation 中隔离
            // 基础设施层Repository,xxxContext，应用层AppService
            NativeInjectorBootStrapper.RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //一wwwroot中的index.html页面作为默认页面
            FileServerOptions fileServerOptions = new FileServerOptions();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseFileServer(fileServerOptions);


            //app.UseMiddleware<CorsMiddleware>();//跨域中间件

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/V1/swagger.json", $"{ApiName} V1");

                //路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，去launchSettings.json把launchUrl去掉，如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "doc";
                c.RoutePrefix = "";
            });
            #endregion

            //使用静态文件
            app.MapWhen(contenxt =>
            {
                return !contenxt.Request.Path.Value.StartsWith("/api");
            }, appBuilder =>
            {
                var option = new RewriteOptions();

                option.AddRewrite(".*", "/index.html", true);
                appBuilder.UseRewriter(option);

                appBuilder.UseStaticFiles();
            });

            //404跳转到登录页
            //app.UseMiddleware<Jump404Middleware>();

            app.UseRouting();

            app.UseCors("CorsPolicy");//跨域

            // 先开启认证
            app.UseAuthentication();
            // 然后是授权中间件
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // WebApi
                endpoints.MapControllers().RequireCors("CorsPolicy");

                // MVC
                //endpoints.MapControllerRoute("default", "{controller}/{action}/{id?}");

                //取消默认输出
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
            });
        }
    }
}
