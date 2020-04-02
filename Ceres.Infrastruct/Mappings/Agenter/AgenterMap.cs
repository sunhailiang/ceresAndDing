using Ceres.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ceres.Infrastruct.Mappings
{
    /// <summary>
    /// Agenter map类
    /// </summary>
    public class AgenterMap : IEntityTypeConfiguration<Agenter>
    {
        /// <summary>
        /// 实体属性配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Agenter> builder)
        {
            //实体属性Map
            builder.Property(c => c.OID)
                .HasColumnName("OID")
                .IsRequired();

            builder.Property(c => c.UserName)
                .HasColumnType("varchar(30)")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(c => c.Sex)
                .HasColumnType("int");

            builder.Property(c => c.Cellphone)
                .HasColumnType("varchar(15)")
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(c => c.Image)
                .HasColumnType("varchar(500)")
                .HasMaxLength(500);

            builder.OwnsOne(a => a.Address,
                            b =>
                            {
                                b.Property(c => c.Province)
                                .HasColumnType("varchar(40)")
                                .HasMaxLength(40)
                                .IsRequired();

                                b.Property(c => c.City)
                                .HasColumnType("varchar(40)")
                                .HasMaxLength(40)
                                .IsRequired();
                            }
                            );

            builder.Property(c => c.CreateTime)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(c => c.Status)
                .HasColumnType("int")
                .IsRequired();
        }
    }
}
