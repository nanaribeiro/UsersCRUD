using System;

namespace UsersCrud.Domain.DTO
{
    public class UserResponseDTO
    {
        /// <summary>
        /// Id único de identificação do usuário
        /// </summary>
        public Guid Id { get; set; }

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
