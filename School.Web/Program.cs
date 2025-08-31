using School.Core.Configurations;

var builder = WebApplication.CreateBuilder(args);

AppDomain.CurrentDomain.Load("School.Infrastructure");

var modules = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(a => a.GetTypes())
    .Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
    .Select(Activator.CreateInstance)
    .Cast<IModule>();

foreach (var module in modules)
{
    module.Load(builder.Services, builder.Configuration);
}

var app = builder.Build();

foreach (var module in modules)
{
    await module.LoadAsync(app);
}

app.Run();
