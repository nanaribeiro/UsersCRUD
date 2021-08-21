namespace UsersCrud.Domain.DTO
{
    public abstract class UserDTO
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
        /// Senha do usuário.
        /// </summary>
        public string Password{ get; set; }

        /// <summary>
        /// Telefone do usuário
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
