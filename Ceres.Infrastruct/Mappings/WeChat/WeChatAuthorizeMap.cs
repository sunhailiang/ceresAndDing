using Ceres.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ceres.Infrastruct.Mappings
{
    /// <summary>
    /// WeChatAuthorize map类
    /// </summary>
    public class WeChatAuthorizeMap : IEntityTypeConfiguration<WeChatAuthorize>
    {
        /// <summary>
        /// 实体属性配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<WeChatAuthorize> builder)
        {
            //实体属性Map
            builder.Property(c => c.OID)
                .HasColumnName("OID")
                .IsRequired();

            builder.Property(c => c.Code2Session)
                .HasColumnType("varchar(500)")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(c => c.EncryptedData)
                .HasColumnType("varchar(500)")
                .HasMaxLength(500);

            builder.Property(c => c.IV)
                .HasColumnType("varchar(500)")
                .HasMaxLength(500);

            builder.Property(c => c.PhoneJson)
                .HasColumnType("varchar(500)")
                .HasMaxLength(500);

            builder.Property(c => c.CreateTime)
                .HasColumnType("datetime")
                .IsRequired();
        }
    }
}
