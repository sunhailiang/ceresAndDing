using Ceres.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ceres.Infrastruct.Mappings
{
    /// <summary>
    /// FoodComponent map类
    /// </summary>
    public class FoodComponentMap : IEntityTypeConfiguration<FoodComponent>
    {
        /// <summary>
        /// 实体属性配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<FoodComponent> builder)
        {
            //实体属性Map
            builder.Property(c => c.OID)
                .HasColumnName("OID")
                .IsRequired();

            builder.Property(c => c.FoodOid)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(c => c.ComponentOid)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(c => c.Value)
                .HasColumnType("float")
                .IsRequired();

            builder.Property(c => c.Status)
                .HasColumnType("int")
                .IsRequired();
        }
    }
}
