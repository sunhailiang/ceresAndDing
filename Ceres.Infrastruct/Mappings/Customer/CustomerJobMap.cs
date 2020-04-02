using Ceres.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Infrastruct.Mappings
{
    public class CustomerJobMap : IEntityTypeConfiguration<CustomerJob>
    {
        /// <summary>
        /// 实体属性配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<CustomerJob> builder)
        {
            //实体属性Map
            builder.Property(c => c.OID)
                .HasColumnName("OID")
                .IsRequired();

            builder.OwnsOne(a => a.Job,
                            b =>
                            {
                                b.Property(c => c.Name)
                                .HasColumnType("varchar(40)")
                                .HasMaxLength(40)
                                .IsRequired();

                                b.Property(c => c.Strength)
                                .HasColumnType("varchar(40)")
                                .HasMaxLength(40)
                                .IsRequired();
                            }
                            );
        }
    }
}
