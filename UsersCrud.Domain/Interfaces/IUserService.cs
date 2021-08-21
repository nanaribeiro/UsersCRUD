using System.Threading.Tasks;
using UsersCrud.Domain.DTO;
using UsersCrud.Domain.Entities;

namespace UsersCrud.Domain.ServicesInterfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Método para adição de novo usuário
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        Task<UserResponseDTO> AddNewUser(UserDTO userDto);

        /// <summary>
        /// Método para obter um usuário pelo seu ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserEntity GetById(int id);
    }
}
