namespace UsersCrud.Domain.DTO
{
    /// <summary>
    /// Representação dos dados necessários para autenticar no sistema
    /// </summary>
    public class UserAuthenticationDTO
    {
        /// <summary>
        /// Nome de usuário
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Senha do usuário.
        /// </summary>
        public string Password { get; set; }
    }
}
