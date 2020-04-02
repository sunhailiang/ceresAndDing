using System;
using Ceres.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ceres.Infrastruct.Mappings
{
    /// <summary>
    /// CustomerService map类
    /// </summary>
    public class CustomerServiceMap: IEntityTypeConfiguration<CustomerService>
    {
        /// <summary>
        /// 实体属性配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<CustomerService> builder)
        {
            //实体属性Map
            builder.Property(c => c.OID)
                .HasColumnName("OID")
                .IsRequired();

            builder.Property(c => c.CustomerOid)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(c => c.ServiceOid)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(c => c.Status)
                .HasColumnType("int")
                .IsRequired();
        }
    }
}
