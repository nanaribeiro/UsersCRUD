using System;
using System.Threading.Tasks;
using UsersCrud.Domain.DTO;

namespace UsersCrud.Domain.ServicesInterfaces
{
    public interface IUserService
    {
        Task<Guid> AddNewUser(UserDTO userDto);
    }
}
