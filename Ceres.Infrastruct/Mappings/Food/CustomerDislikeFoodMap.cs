using Ceres.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ceres.Infrastruct.Mappings
{
    /// <summary>
    /// CustomerDislikeFood map类
    /// </summary>
    public class CustomerDislikeFoodMap : IEntityTypeConfiguration<CustomerDislikeFood>
    {
        /// <summary>
        /// 实体属性配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<CustomerDislikeFood> builder)
        {
            //实体属性Map
            builder.Property(c => c.OID)
                .HasColumnName("OID")
                .IsRequired();

            builder.Property(c => c.CustomerOid)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(c => c.FoodOid)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(c => c.OperaterOid)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(c => c.CreateTime)
                .HasColumnType("datetime")
                .IsRequired();
        }
    }
}
