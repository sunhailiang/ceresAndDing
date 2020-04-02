using Ceres.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ceres.Infrastruct.Mappings
{
    /// <summary>
    /// CustomerAssistDing map类
    /// </summary>
    public class CustomerAssistDingMap : IEntityTypeConfiguration<CustomerAssistDing>
    {
        /// <summary>
        /// 实体属性配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<CustomerAssistDing> builder)
        {
            //实体属性Map
            builder.Property(c => c.OID)
                .HasColumnName("OID")
                .IsRequired();

            builder.Property(c => c.QuestionnaireGuid)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(c => c.CustomerOid)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(c => c.SupporterOid)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(c => c.AssistTime)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(c => c.CreateTime)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(c => c.Status)
                .HasColumnType("int")
                .IsRequired();
        }
    }
}
