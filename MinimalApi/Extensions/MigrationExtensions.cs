using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Infrastructure;

namespace MinimalApi.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using SocialDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<SocialDbContext>();

        dbContext.Database.Migrate();
    }
}
