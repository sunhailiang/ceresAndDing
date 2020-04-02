using Ceres.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ceres.Infrastruct.Mappings
{
    /// <summary>
    /// Supporter map类
    /// </summary>
    public class SupporterMap : IEntityTypeConfiguration<Supporter>
    {
        /// <summary>
        /// 实体属性配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Supporter> builder)
        {
            //实体属性Map
            builder.Property(c => c.OID)
                .HasColumnName("OID")
                .IsRequired();

            builder.Property(c => c.LoginName)
                .HasColumnType("varchar(30)")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(c => c.Cellphone)
                .HasColumnType("varchar(15)")
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(c => c.Password)
                .HasColumnType("varchar(40)")
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(c => c.UserName)
                .HasColumnType("varchar(30)")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(c => c.Image)
                .HasColumnType("varchar(500)")
                .HasMaxLength(500);

            builder.Property(c => c.CreateTime)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(c => c.Status)
                .HasColumnType("int")
                .IsRequired();
        }
    }
}
