using AutoMapper;
using UsersCrud.Domain.DTO;
using UsersCrud.Domain.Entities;

namespace UsersCrud.Domain
{
    /// <summary>
    /// Classe de mapeamento do AutoMapper para mapear DTOs e entidades
    /// </summary>
    public class MapData : Profile
    {
        public MapData()
        {
            User();
        }
        private void User()
        {
            this.CreateMap<UserDTO, UserEntity>();
            this.CreateMap<UserEntity, UserResponseDTO>();
            this.CreateMap<UserAuthenticationDTO, UserEntity>();
        }
    }
}
