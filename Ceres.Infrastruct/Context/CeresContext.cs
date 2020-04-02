using Ceres.Domain.Models;
using Ceres.Infrastruct.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ceres.Infrastruct.Context
{
    public class CeresContext:DbContext
    {
        //代理
        public DbSet<Agenter> Agenter { get; set; }

        //客服
        public DbSet<Supporter> Supporter { get; set; }

        //公司服务
        public DbSet<Service> Service { get; set; }

        //客户
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerJob> CustomerJob { get; set; }
        public DbSet<CustomerAssistDing> CustomerAssistDing { get; set; }
        public DbSet<CustomerService> CustomerService { get; set; }
        public DbSet<Food> Food { get; set; }
        public DbSet<Component> Component { get; set; }
        public DbSet<FoodComponent> FoodComponent { get; set; }
        public DbSet<CustomerDislikeFood> CustomerDislikeFood { get; set; }
        public DbSet<CustomerDiet> CustomerDiet { get; set; }

        //微信授权
        public DbSet<WeChatAuthorize> WeChatAuthorize { get; set; }



        //Migration的时候，此处注销，原因是Mercury系统中已经存在了，
        public DbSet<UserInformation> UserInformation { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Questionnaire> Questionnaire { get; set; }
        public DbSet<Question> Question { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AgenterMap());

            modelBuilder.ApplyConfiguration(new SupporterMap());

            modelBuilder.ApplyConfiguration(new ServiceMap());

            modelBuilder.ApplyConfiguration(new CustomerMap());
            modelBuilder.ApplyConfiguration(new CustomerJobMap());

            modelBuilder.ApplyConfiguration(new CustomerAssistDingMap());

            modelBuilder.ApplyConfiguration(new CustomerServiceMap());

            modelBuilder.ApplyConfiguration(new FoodMap());
            modelBuilder.ApplyConfiguration(new ComponentMap());
            modelBuilder.ApplyConfiguration(new FoodComponentMap());

            modelBuilder.ApplyConfiguration(new CustomerDislikeFoodMap());
            modelBuilder.ApplyConfiguration(new CustomerDietMap());

            modelBuilder.ApplyConfiguration(new WeChatAuthorizeMap());

            //modelBuilder.Entity<CST_Customer_Main>().HasMany(c=>c.CST_Customer_Strength)
            //modelBuilder.Entity<CST_Customer_Main>().HasMany<CST_Customer_Strength>(c => c.CST_Customer_Strength);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 从 appsettings.json 中获取配置信息
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // 定义要使用的数据库
            optionsBuilder.UseSqlServer(config.GetConnectionString("MercurySqlConnection"));
            //optionsBuilder.UseSqlServer(DbConfig.InitConn(config.GetConnectionString("MercurySqlConnection_file"), config.GetConnectionString("DefaultConnection")));
        }
    }
}
