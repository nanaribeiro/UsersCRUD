using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UsersCrud.CrossCutting.Helpers;
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
        /// Instância da classe com o segredo do token
        /// </summary>
        private readonly AppSettings _appSettings;

        /// <summary>
        /// Instância do serviço do AutoMapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Método construtor da classe de serviços do usuário
        /// </summary>
        /// <param name="userRepository">Repositório para persistir dados do usuário</param>
        /// <param name="mapper">Serviço de mapeamento automático AutoMapper</param>
        public UserService(IBaseRepository<UserEntity> userRepository, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Método para adicionar um novo usuário na base de dados
        /// </summary>
        /// <param name="userDto">Dados do novo usuário</param>
        /// <returns></returns>
        public Task<UserResponseDTO> AddNewUser(UserDTO userDto)
        {
            ValidateUserData(userDto);
            var userEntity = this._mapper.Map<UserEntity>(userDto);
            userEntity.PasswordHash = userDto.Password.Encode();
            _userRepository.Insert(userEntity);
            _userRepository.SaveChanges();
            var token = GenerateJwtToken(userEntity);

            return Task.FromResult(new UserResponseDTO() {Id =  userEntity.Id, Token = token, UserName = userEntity.UserName });
        }

        /// <summary>
        /// Método para obter um usuário pelo seu Id
        /// </summary>
        /// <param name="id">Identificador do usuário</param>
        /// <returns></returns>
        public UserEntity GetById(int id)
        {
            return _userRepository.Select(id);
        }

        /// <summary>
        /// Método para validação dos dados do usuário
        /// </summary>
        /// <param name="userDto">Dados do usuário a serem validados</param>
        public void ValidateUserData(UserDTO userDto)
        {
            if (string.IsNullOrEmpty(userDto.UserName))
                throw new Exception("O nome do usuário precisa ser informado");

            if(string.IsNullOrEmpty(userDto.Email))
                throw new Exception("O email precisa ser informado");

            if (string.IsNullOrEmpty(userDto.Password))
                throw new Exception("A senha precisa ser informada");

            if (string.IsNullOrEmpty(userDto.PhoneNumber))
                throw new Exception("O número de telefone precisa ser informado");

            if (userDto.Password.Length < 6 || userDto.Password.Length > 20)
                throw new Exception("A senha deve ter entre 6 a 20 caracteres");

            string userNamePattern = @"^[a-zA-Z0-9]{5,50}$";
            Regex rg = new Regex(userNamePattern);
            MatchCollection matchedUserName = rg.Matches(userDto.UserName);

            if (matchedUserName.Count == 0)
                throw new Exception("O nome de usuário deve ter entre 5 a 50 caracteres alfanúmericos");
        }

        private string GenerateJwtToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            //Pegar o segredo do appsettings
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
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
