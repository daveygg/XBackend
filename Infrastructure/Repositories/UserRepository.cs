using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly SocialDbContext _ctx;

    public UserRepository(SocialDbContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<User> GetUserByEmail(string email)
    {
        return await _ctx.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> GetUserById(string id)
    {
        return await _ctx.Users.FirstOrDefaultAsync(p => p.Id == id);
    }
}
