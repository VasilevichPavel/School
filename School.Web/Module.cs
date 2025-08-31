using School.Core.Configurations;
using School.Web.Components;
using School.Web.Endpoints;

namespace School.Web
{
    public class Module : IModule
    {
        public void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.AddRazorComponents()
                .AddInteractiveServerComponents();

            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public Task LoadAsync(IApplicationBuilder builder)
        {
            var app = builder as WebApplication;

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                app.UseHsts();
            }

            app.MapStudentEndpoints();
            app.MapTeacherEndpoints();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            return Task.CompletedTask;
        }
    }
}
