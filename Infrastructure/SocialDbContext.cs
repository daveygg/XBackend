using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure;

public class SocialDbContext : IdentityDbContext<User>
{
    public SocialDbContext(DbContextOptions opt) : base(opt)
    {        
    }
    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }
}

