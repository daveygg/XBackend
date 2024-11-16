using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure;

public class SocialDbContext :IdentityDbContext<User>
{
    public SocialDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }
    public DbSet<Post> Posts { get; set; }

    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    base.OnModelCreating(builder);

    //    List<IdentityRole> roles = new List<IdentityRole>
    //    {
    //        new()
    //        {
    //            Name = "Admin",
    //            NormalizedName = "ADMIN"
    //        },
    //        new()
    //        {
    //            Name = "User",
    //            NormalizedName = "USER"
    //        }
    //    };

    //    builder.Entity<IdentityRole>().HasData(roles);
    //}
}

