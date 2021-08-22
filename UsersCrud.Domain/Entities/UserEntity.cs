namespace UsersCrud.Domain.Entities
{
    public class UserEntity : BaseEntity
    {      
        /// <summary>
        /// Nome de usuário
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Email do usuário
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Hash da senha do usuário.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Telefone do usuário
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
