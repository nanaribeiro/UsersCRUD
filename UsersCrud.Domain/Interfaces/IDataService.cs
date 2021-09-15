using System.Threading;
using System.Threading.Tasks;

namespace UsersCrud.Domain.Interfaces
{
    /// <summary>
    /// Interface para a classe que cria o usuário padrão do sistema
    /// </summary>
    public interface IDataService
    {
        Task CreateAdminUser(CancellationToken cancellationToken = default);
    }
}
