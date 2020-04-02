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

            //���ÿ���
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
                     // {ApiName} �����ȫ�ֱ����������޸�
                     Version = "V1",
                     Title = $"{ApiName} �ӿ��ĵ�����Netcore 3.0",
                     Description = $"{ApiName} HTTP API V1",
                 });
                 c.OrderActionsBy(o => o.RelativePath);

                 var xmlPath = Path.Combine(basePath, "Ceres.WebApi.xml");//������Ǹո����õ�xml�ļ���
                 c.IncludeXmlComments(xmlPath, true);//Ĭ�ϵĵڶ���������false�������controller��ע�ͣ��ǵ��޸�

                 var xmlModelPath = Path.Combine(basePath, "Ceres.Application.xml");//�������Model���xml�ļ���
                 c.IncludeXmlComments(xmlModelPath);

                 #region Token�󶨵�ConfigureServices
                 // ������ȨС��
                 c.OperationFilter<AddResponseHeadersFilter>();
                 c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                 // ��header�����token�����ݵ���̨
                 c.OperationFilter<SecurityRequirementsOperationFilter>();


                 // ������ oauth2
                 c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                 {
                     Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�\"",
                     Name = "Authorization",//jwtĬ�ϵĲ�������
                     In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                     Type = SecuritySchemeType.ApiKey
                 });
                 #endregion
             });
            #endregion


            #region ���ڲ��Ե���Ȩ
            // 1����Ȩ����������ϱߵ�����ͬ�����ô����ǲ�����controller�У�д��� roles ��
            // Ȼ����ôд [Authorize(Policy = "Admin")]
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Supporter", policy => policy.RequireRole("Supporter").Build());
                //options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                //options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                //options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));
                //options.AddPolicy("A_S_O", policy => policy.RequireRole("Admin", "System", "Others"));
                //options.AddPolicy("2����", policy => policy.RequireRole("Admin").RequireRole("System"));
            });
            #endregion

            //��ȡ�����ļ�
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            #region JWT����֤3+2��
            //JWT����֤3+2��
            services.AddAuthentication(x =>
            {
                //�����������Ϥô��û�������ϱߴ�������Ǹ���
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })// Ҳ����ֱ��д�ַ�����AddAuthentication("Bearer")
             .AddJwtBearer(o =>
             {
                 o.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,//��Կ��֤
                     IssuerSigningKey = signingKey,//�����������±�

                     ValidateIssuer = true,
                     ValidIssuer = audienceConfig["Issuer"],//������

                     ValidateAudience = true,
                     ValidAudience = audienceConfig["Audience"],//������

                     ValidateLifetime = true,//��֤��������
                     ClockSkew = TimeSpan.Zero,
                     RequireExpirationTime = true,//��֤����ʱ��
                 };

             });
            #endregion


            // MVC��ͼ
            //services.AddControllersWithViews();

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

            //һwwwroot�е�index.htmlҳ����ΪĬ��ҳ��
            FileServerOptions fileServerOptions = new FileServerOptions();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseFileServer(fileServerOptions);


            //app.UseMiddleware<CorsMiddleware>();//�����м��

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/V1/swagger.json", $"{ApiName} V1");

                //·�����ã�����Ϊ�գ���ʾֱ���ڸ�������localhost:8001�����ʸ��ļ�,ע��localhost:8001/swagger�Ƿ��ʲ����ģ�ȥlaunchSettings.json��launchUrlȥ����������뻻һ��·����ֱ��д���ּ��ɣ�����ֱ��дc.RoutePrefix = "doc";
                c.RoutePrefix = "";
            });
            #endregion

            //ʹ�þ�̬�ļ�
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

            //404��ת����¼ҳ
            //app.UseMiddleware<Jump404Middleware>();

            app.UseRouting();

            app.UseCors("CorsPolicy");//����

            // �ȿ�����֤
            app.UseAuthentication();
            // Ȼ������Ȩ�м��
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // WebApi
                endpoints.MapControllers().RequireCors("CorsPolicy");

                // MVC
                //endpoints.MapControllerRoute("default", "{controller}/{action}/{id?}");

                //ȡ��Ĭ�����
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
            });
        }
    }
}
