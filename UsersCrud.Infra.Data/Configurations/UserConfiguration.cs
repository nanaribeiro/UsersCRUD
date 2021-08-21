using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsersCrud.Domain.Entities;

namespace UsersCrud.Infra.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {

        /// <summary>
        /// Método construtor.
        /// </summary>
        public UserConfiguration()
        {
        }

        /// <summary>
        /// Método de construção da configuração.
        /// </summary>
        /// <param name="builder">Instância do construtor de configurações.</param>
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users", "dto");

            builder.HasKey(x => x.Id).HasName("PK_USER_ID");
            builder.HasIndex(x => new { x.UserName, x.Email }).IsUnique().HasDatabaseName("IDX_USER_KEY");

            builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
            builder.Property(x => x.PasswordHash).IsRequired();
            builder.Property(x => x.PhoneNumber).IsRequired();
            builder.Property(x => x.UserName).HasMaxLength(256).IsRequired();
        }
    }
}
