using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace School.Core.Configurations
{
    public interface IModule
    {
        void Load(IServiceCollection services, IConfiguration configuration);
    }
}
