using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using School.Core.Configurations;
using School.Web.Components;
using School.Web.Endpoints;
using School.Web.Profiles;

namespace School.Web
{
    public class Module : IModule
    {
        public void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(cfg => cfg.AddProfile<AddressProfile>());
            services.AddAutoMapper(cfg => cfg.AddProfile<TeacherProfile>());
            services.AddAutoMapper(cfg => cfg.AddProfile<StudentProfile>());
            services.AddAutoMapper(cfg => cfg.AddProfile<ClassProfile>());

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
            app.MapClassEndpoints();

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
