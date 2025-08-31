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
            services.AddDbContext<ISqlDbContext, SqlDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("ConnectionString"));
            });

            services.AddDbContext<ISqlDbContextQuery, SqlDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("ConnectionString"));
            });
        }
    }
}
