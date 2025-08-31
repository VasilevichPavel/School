using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using School.Application.Middlewares;
using School.Core.Configurations;

namespace School.Application
{
    internal class Module : IModule
    {
        public void Load(IServiceCollection services, IConfiguration configuration) { }

        public Task LoadAsync(IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionMiddleware>();

            return Task.CompletedTask;
        }
    }
}
