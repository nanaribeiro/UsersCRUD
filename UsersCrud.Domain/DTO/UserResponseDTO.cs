using System;

namespace UsersCrud.Domain.DTO
{
    public class UserResponseDTO
    {
        /// <summary>
        /// Nome de usuário
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Token de autenticação do usuário
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Id de identificação do usuário
        /// </summary>
        public Guid Id { get; set; }
    }
}
