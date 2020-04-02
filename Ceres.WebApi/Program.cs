using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceres.Infrastruct.Context;
using Ceres.WebApi.Initializer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ceres.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();//系统默认
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<CeresContext>();
                    DbInitializer.Seed(context);
                }
                catch (Exception)
                {
                    //添加日志
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //硬编码配置端口号，发布后无法修改
                    //webBuilder.UseUrls("http://*:5000");
                    //webBuilder.UseUrls("http://*:5000", "https://*:5001");
                    //webBuilder.UseIIS();

                    IConfiguration Configuration = new ConfigurationBuilder()
                       .SetBasePath(Environment.CurrentDirectory)
                       .AddJsonFile("appsettings.json")
                       .Build();

                    //string httpUrls = Configuration["Url:Https"];
                    //webBuilder.UseUrls(httpUrl, httpUrls);

                    //josn文件存储形式，发布后可以修改
                    string httpUrl = Configuration["Url:Http"];
                    webBuilder.UseUrls(httpUrl);
                    webBuilder.UseStartup<Startup>();
                });
    }
}
