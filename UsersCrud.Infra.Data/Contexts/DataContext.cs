using Microsoft.EntityFrameworkCore;
using UsersCrud.Infra.Data.Configurations;

namespace UsersCrud.Infra.Data.Contexts
{
    /// <summary>
    /// Contexto do serviço de usuários
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="options">Opções do contexto.</param>
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        /// <summary>
        /// Método que faz a criação dos modelos do contexto.
        /// </summary>
        /// <param name="modelBuilder">Instância do contrutor de modelos do contexto.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }

    }
}
