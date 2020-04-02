using Ceres.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ceres.Infrastruct.Mappings
{
    /// <summary>
    /// CustomerDiet map类
    /// </summary>
    public class CustomerDietMap : IEntityTypeConfiguration<CustomerDiet>
    {
        /// <summary>
        /// 实体属性配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<CustomerDiet> builder)
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

            builder.OwnsOne(a => a.Recommend,
                            b=> 
                            {
                                b.Property(c=>c.DailyEnergy)
                                .HasColumnType("varchar(40)")
                                .HasMaxLength(40)
                                .IsRequired();

                                b.Property(c => c.DailyComponentPercentage)
                                .HasColumnType("varchar(5000)")
                                .HasMaxLength(5000)
                                .IsRequired();

                                b.Property(c => c.DailyFoodComponent)
                                .HasColumnType("varchar(5000)")
                                .HasMaxLength(5000)
                                .IsRequired();
                            }
                            );

            builder.OwnsOne(a => a.Current,
                b =>
                {
                    b.Property(c => c.DailyEnergy)
                    .HasColumnType("varchar(40)")
                    .HasMaxLength(40)
                    .IsRequired();

                    b.Property(c => c.DailyComponentPercentage)
                    .HasColumnType("varchar(5000)")
                    .HasMaxLength(5000)
                    .IsRequired();
                }
                );

            builder.Property(c => c.CurrentDiet)
                .HasColumnType("varchar(5000)")
                .HasMaxLength(5000);

            builder.Property(c => c.SupporterOid)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(c => c.Status)
                .HasColumnType("int")
                .IsRequired();

            builder.OwnsOne(a => a.Discard,
                            b =>
                            {
                                b.Property(c => c.Reason)
                                .HasColumnType("varchar(500)")
                                .HasMaxLength(500);
                            }
                            );

            builder.Property(c => c.CreateTime)
                .HasColumnType("datetime")
                .IsRequired();

            builder.OwnsOne(a => a.LastOperate,
                            b =>
                            {
                                b.Property(c => c.Oid)
                                .HasColumnType("uniqueidentifier")
                                .IsRequired();

                                b.Property(c => c.Time)
                                .HasColumnType("datetime")
                                .IsRequired();
                            }
                            );
        }
    }
}
