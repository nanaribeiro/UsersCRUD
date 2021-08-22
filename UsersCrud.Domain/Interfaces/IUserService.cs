﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsersCrud.Domain.DTO;
using UsersCrud.Domain.Entities;

namespace UsersCrud.Domain.ServicesInterfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Método para adição de novo usuário
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        Task<UserEntity> AddNewUser(UserDTO userDto);

        /// <summary>
        /// Método para atualizar um usuário
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        Task<UserDTO> UpdateUser(Guid userId, UserDTO userDto);

        /// <summary>
        /// Método para deletar usuário pelo Id informado
        /// </summary>
        /// <param name="userId">Id de identificação do usuário a ser deletado</param>
        /// <returns></returns>
        Task DeleteUser(Guid userId);

        /// <summary>
        /// Método para autenticar no sistema
        /// </summary>
        /// <param name="userDto">Dados do usuário para autenticar</param>
        /// <returns></returns>
        Task<UserResponseDTO> Authenticate(UserDTO userDto);

        /// <summary>
        /// Método para listar todos os usuários cadastrados
        /// </summary>
        /// <returns>Lista com todos os usuários</returns>
        Task<IEnumerable<UserEntity>> GetAllUsers();
    }
}
