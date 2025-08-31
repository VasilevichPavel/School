using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using School.Core.Configurations;
using School.Infrastructure.Contexts;

namespace School.Infrastructure
{
    internal class Module : IModule
    {
        public void Load(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnectionString");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Database connection string is missing!");
            }

            services.AddDbContext<SqlDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            services.AddScoped<ISqlDbContext>(sp => sp.GetRequiredService<SqlDbContext>());
            services.AddScoped<ISqlDbContextQuery>(sp => sp.GetRequiredService<SqlDbContext>());

            SQLitePCL.Batteries.Init();
        }

        public async Task LoadAsync(IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ISqlDbContext>();
            await db.Database.EnsureCreatedAsync();
        }
    }
}
