﻿using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions;
public interface IUserRepository
{
    Task<User?> GetUserById(string id);
    Task<User?> GetUserByEmail(string email);
}
