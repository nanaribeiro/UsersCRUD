namespace UsersCrud.Domain.DTO
{
    public class ChangePasswordDTO
    {
        /// <summary>
        /// Email do usuário
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Nova Senha do usuário.
        /// </summary>
        public string NewPassword { get; set; }
    }
}
