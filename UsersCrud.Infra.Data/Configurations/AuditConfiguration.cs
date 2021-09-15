using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsersCrud.Domain.Entities;

namespace UsersCrud.Infra.Data.Configurations
{
    public class AuditConfiguration : IEntityTypeConfiguration<AuditEntity>
    {
        /// <summary>
        /// Método construtor.
        /// </summary>
        public AuditConfiguration()
        {
        }

        /// <summary>
        /// Método de construção da configuração.
        /// </summary>
        /// <param name="builder">Instância do construtor de configurações.</param>
        public void Configure(EntityTypeBuilder<AuditEntity> builder)
        {
            builder.ToTable("Audit", "dto");

            builder.HasKey(x => x.Id).HasName("PK_AUDIT_ID");
            //builder.HasIndex(x => new { x.UserName, x.Email }).IsUnique().HasDatabaseName("IDX_USER_KEY");

            builder.Property(x => x.TableName).IsRequired();
            builder.Property(x => x.OldValues).IsRequired(false);
            builder.Property(x => x.NewValues).IsRequired(false);
            builder.Property(x => x.UserName).IsRequired();
            builder.Property(x => x.DateTime).IsRequired();
        }
    }
}
