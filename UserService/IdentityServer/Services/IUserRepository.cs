﻿using IdentityServer.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public interface IUserRepository
    {
        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetUsers();
        /// <summary>
        /// Get user by Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserDTO> GetUserByID(int userId);
        /// <summary>
        /// Get user by username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<UserDTO> GetUserByUsername(string username);
    }
}
