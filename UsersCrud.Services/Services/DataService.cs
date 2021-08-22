using UsersCrud.CrossCutting.Helpers;
using UsersCrud.Domain.Entities;
using UsersCrud.Domain.Interfaces;

namespace UsersCrud.Services.Services
{
    public class DataService: IDataService
    {
        /// <summary>
        /// Instância do repositório para persistir dados
        /// </summary>
        private readonly IBaseRepository<UserEntity> _userRepository;

        public DataService(IBaseRepository<UserEntity> userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Cria o usuário padrão do sistema caso ele não exista
        /// </summary>
        public void CreateAdminUser()
        {
            var adminUser = new UserEntity { Email = "admin@admin.com", UserName = "4dmin21", PasswordHash = "Junt0Segur0s@2021".Encode(), PhoneNumber = "21 9875-9854"};
            var existingUser = _userRepository.SelectWhere(x => x.UserName == adminUser.UserName && x.PasswordHash == adminUser.PasswordHash);
           
            if(existingUser == null)
            {
                _userRepository.Insert(adminUser);
                _userRepository.SaveChanges();
            }            
        }
    }
}
