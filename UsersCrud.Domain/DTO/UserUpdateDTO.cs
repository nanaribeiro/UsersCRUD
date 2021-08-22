namespace UsersCrud.Domain.DTO
{
    public class UserUpdateDTO
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
        /// Telefone do usuário
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
