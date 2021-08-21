using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UsersCrud.Domain.DTO;
using UsersCrud.Domain.Entities;
using UsersCrud.Domain.Interfaces;
using UsersCrud.Domain.ServicesInterfaces;
using UsersCrud.Infra.Shared;

namespace UsersCrud.Services.Services
{
    /// <summary>
    /// Implementação do serviço de usuários
    /// </summary>
    public class UserService: IUserService
    {
        /// <summary>
        /// Instância do repositório para persistir dados
        /// </summary>
        private readonly IBaseRepository<UserEntity> _userRepository;

        /// <summary>
        /// Instância do serviço do AutoMapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Método construtor da classe de serviços do usuário
        /// </summary>
        /// <param name="userRepository">Repositório para persistir dados do usuário</param>
        /// <param name="mapper">Serviço de mapeamento automático AutoMapper</param>
        public UserService(IBaseRepository<UserEntity> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Método para adicionar um novo usuário na base de dados
        /// </summary>
        /// <param name="userDto">Dados do novo usuário</param>
        /// <returns></returns>
        public Task<Guid> AddNewUser(UserDTO userDto)
        {
            ValidateUserData(userDto);
            var userEntity = this._mapper.Map<UserEntity>(userDto);
            userEntity.PasswordHash = userDto.Password.Encode();
            _userRepository.Insert(userEntity);
            _userRepository.SaveChanges();
            return Task.FromResult(userEntity.Id);
        }

        /// <summary>
        /// Método para validação dos dados do usuário
        /// </summary>
        /// <param name="userDto">Dados do usuário a serem validados</param>
        public void ValidateUserData(UserDTO userDto)
        {
            //Verificar primeiro se os campos obrigatórios estão preenchidos
        }

        private string generateJwtToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            //Pegar o segredo do appsettings
            var key = Encoding.ASCII.GetBytes("new secret");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
