using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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
            VerifyExistingUser(userDto);
            var userEntity = this._mapper.Map<UserEntity>(userDto);
            userEntity.PasswordHash = userDto.Password.Encode();
            _userRepository.Insert(userEntity);
            _userRepository.SaveChanges();

            return Task.FromResult(new UserResponseDTO { Email = userEntity.Email, Id = userEntity.Id, PhoneNumber = userEntity.PhoneNumber, UserName = userEntity.UserName});
        }

        /// <summary>
        /// Método para atualizar os dados básicos do usuário
        /// </summary>
        /// <param name="userId">Id único do usuário</param>
        /// <param name="userDto">Dados a serem modificados</param>
        /// <returns></returns>
        public Task<UserResponseDTO> UpdateUserData(Guid userId, UserUpdateDTO userDto)
        {
            var validateUser = new UserDTO() { Email = userDto.Email, Password = "senhavalida", PhoneNumber = userDto.PhoneNumber, UserName = userDto.UserName };
            ValidateUserData(validateUser);
            VerifyExistingUserForUpdate(userId, validateUser);

            var user = _userRepository.Select(userId);

            if (user == null)
                throw new Exception("Usuário inexistente");

            ValidateAdminUserUpdate(user.UserName, userDto.UserName);

            user.PhoneNumber = userDto.PhoneNumber;
            user.Email = userDto.Email;
            user.UserName = userDto.UserName;
            _userRepository.Update(user);
            _userRepository.SaveChanges();

            return Task.FromResult(new UserResponseDTO { Email = user.Email, Id = user.Id, PhoneNumber = user.PhoneNumber, UserName = user.UserName });
        }

        /// <summary>
        /// Método para trocar a senha do usuário
        /// </summary>
        /// <param name="changePasswordDTO">DTO com os dados necessários para a mudança de senha</param>
        /// <returns></returns>
        public Task<UserResponseDTO> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            ValidatePassword(changePasswordDTO.NewPassword);

            var user = _userRepository.SelectWhere(x => x.UserName == changePasswordDTO.UserName);

            if (user == null)
                throw new Exception("E-mail inválido ou inexistente");

            user.PasswordHash = changePasswordDTO.NewPassword.Encode();
            _userRepository.Update(user);
            _userRepository.SaveChanges();

            return Task.FromResult(new UserResponseDTO { Email = user.Email, Id = user.Id, PhoneNumber = user.PhoneNumber, UserName = user.UserName });
        }

        /// <summary>
        /// Método para validar a senha do usuário
        /// </summary>
        /// <param name="password">Senha a ser validada</param>
        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new Exception("A senha precisa ser informada");

            if (password.Length < 6 || password.Length > 20)
                throw new Exception("A senha deve ter entre 6 a 20 caracteres");
        }

        /// <summary>
        /// Método para remover um usuário
        /// </summary>
        /// <param name="userId">Is de identificação do usuárioa  ser removido</param>
        /// <returns></returns>
        public Task DeleteUser(Guid userId)
        {
            ValidateAdminUserDelete(userId);

            _userRepository.Delete(userId);
            _userRepository.SaveChanges();

            return Task.FromResult(userId);
        }

        /// <summary>
        /// Método para obter a lista de todos os usuários
        /// </summary>
        /// <returns>Lista de todos os usuários cadastrados no sistema</returns>
        public Task<List<UserResponseDTO>> GetAllUsers()
        {
            var users = (List<UserEntity>)_userRepository.Select();
            var usersConversion = users.ConvertAll(x => this._mapper.Map<UserResponseDTO>(x));
            
            return Task.FromResult(usersConversion);
        }

        /// <summary>
        /// Método para autenticar/logar no sistema
        /// </summary>
        /// <param name="userDto">Dados necessários para poder autenticar</param>
        /// <returns>Dados do usuário e token de autenticação</returns>
        public Task<UserAuthenticateResponseDTO> Authenticate(UserAuthenticationDTO userDto)
        {
            if (string.IsNullOrEmpty(userDto.UserName))
                throw new Exception("O nome do usuário precisa ser informado");

            if (string.IsNullOrEmpty(userDto.Password))
                throw new Exception("A senha precisa ser informada");

            var userEntity = this._mapper.Map<UserEntity>(userDto);
            userEntity.PasswordHash = userDto.Password.Encode();

            var existingUser = _userRepository.SelectWhere(x => x.UserName == userEntity.UserName && x.PasswordHash == userEntity.PasswordHash);
            
            if (existingUser == null)
                throw new Exception("Usuário ou senha inválido");

            var token = GenerateJwtToken(userEntity.Id.ToString());

            return Task.FromResult(new UserAuthenticateResponseDTO() { Id = userEntity.Id, Token = token, UserName = userEntity.UserName });
        }


        /// <summary>
        /// Método para verificar se está tentando modificar o usuário de admin
        /// </summary>
        /// <param name="currentUserName">nome de usuário atual</param>
        /// <param name="updatedUserName">nome de usuário a ser modificado</param>
        private void ValidateAdminUserUpdate(string currentUserName, string updatedUserName)
        {
            if (currentUserName == "4dmin21" && currentUserName != updatedUserName)
                throw new Exception("Não é possível mudar o nome do usuário padrão do sistema");
        }

        /// <summary>
        /// Método para validar se o usuário a ser excluído é o padrão
        /// </summary>
        /// <param name="userId">Id do usuário</param>
        private void ValidateAdminUserDelete(Guid userId)
        {
            var existingUser = _userRepository.SelectWhere(x => x.Id == userId);

            if (existingUser == null)
                throw new Exception("Usuário inexistente");
            if (existingUser.UserName == "4dmin21")
                throw new Exception("Não é permitido excluir o usuário padrão do sistema");
        }

        /// <summary>
        /// Método para verificar se o nome de usuário ou e-mail já existem
        /// </summary>
        /// <param name="userDto">Dados a serem verificados</param>
        private void VerifyExistingUser(UserDTO userDto)
        {
            var existingUser = _userRepository.SelectWhere(x => x.UserName == userDto.UserName);
            if (existingUser != null)
                throw new Exception("Nome de usuário existente. Por favor digite outro nome de usuário");
            
            existingUser = _userRepository.SelectWhere(x => x.Email == userDto.Email);
            if (existingUser != null)
                throw new Exception("E-mail em uso. Por favor logue com seu usuário ou digite outro e-mail");
        }

        /// <summary>
        /// Método para verificar se o nome de usuário ou e-mail a serem atualizados já existem
        /// </summary>
        /// <param name="userId">Id do usuário</param>
        /// <param name="userDto">Dados a serem verificados</param>
        private void VerifyExistingUserForUpdate(Guid userId, UserDTO userDto)
        {
            var existingUser = _userRepository.SelectWhere(x => x.UserName == userDto.UserName && x.Id != userId);
            if (existingUser != null)
                throw new Exception("Nome de usuário existente. Por favor digite outro nome de usuário");

            existingUser = _userRepository.SelectWhere(x => x.Email == userDto.Email && x.Id != userId);
            if (existingUser != null)
                throw new Exception("E-mail em uso. Por favor logue com seu usuário ou digite outro e-mail");
        }


        /// <summary>
        /// Método para validação dos dados do usuário
        /// </summary>
        /// <param name="userDto">Dados do usuário a serem validados</param>
        private void ValidateUserData(UserDTO userDto)
        {
            ValidatePassword(userDto.Password);

            if (string.IsNullOrEmpty(userDto.UserName))
                throw new Exception("O nome do usuário precisa ser informado");

            if(string.IsNullOrEmpty(userDto.Email))
                throw new Exception("O email precisa ser informado");

            if (string.IsNullOrEmpty(userDto.PhoneNumber))
                throw new Exception("O número de telefone precisa ser informado");

            string userNamePattern = @"^[a-zA-Z0-9]{5,50}$";
            Regex rg = new Regex(userNamePattern);
            MatchCollection matchedUserName = rg.Matches(userDto.UserName);

            if (matchedUserName.Count == 0)
                throw new Exception("O nome de usuário deve ter entre 5 a 50 caracteres alfanúmericos");

            var emailPattern = @"^(([^<>()[\]\\.,;:\s@\""]+(\.[^<> ()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";
            rg = new Regex(emailPattern);
            MatchCollection matchedEmail = rg.Matches(userDto.Email);

            if (matchedEmail.Count == 0)
                throw new Exception("O e-mail deve ser um e-mail válido");

            var phoneNumberPattern = @"(\(?\d{2}\)?\s?)?(\d{4,5}\-?\d{4})";
            rg = new Regex(phoneNumberPattern);
            MatchCollection matchedPhoneNumber = rg.Matches(userDto.PhoneNumber);

            if (matchedPhoneNumber.Count == 0)
                throw new Exception("O número de telefone deve ser um telefone válido");
        }

        /// <summary>
        /// Método de geração de token utilizando jwt
        /// </summary>
        /// <param name="userId">Id de identificação do usuário</param>
        /// <returns></returns>
        private string GenerateJwtToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", userId) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
