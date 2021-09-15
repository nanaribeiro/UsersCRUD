using Audit.SaveChanges;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using UsersCrud.Domain.DTO;
using UsersCrud.Infra.Data.Configurations;

namespace UsersCrud.Infra.Data.Contexts
{
    /// <summary>
    /// Contexto do serviço de usuários
    /// </summary>
    public class DataContext : DbContextWithAuditing
    {
        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="options">Opções do contexto.</param>
        public DataContext(DbContextOptions options) : base(options)
        {
        }        

        /// <summary>
        /// Método que faz a criação dos modelos do contexto.
        /// </summary>
        /// <param name="modelBuilder">Instância do contrutor de modelos do contexto.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AuditConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                this.UserName = Username.UserName;
                var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            
        }    
    }
}
