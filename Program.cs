using BigProject.Data;
using BigProject.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.Sources.Clear();
    config.SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("config.json")
           .AddEnvironmentVariables();
});

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<DutchContext>();
builder.Services.AddTransient<DutchSeeder>();
builder.Services.AddScoped<IDutchRepository, DutchRepository>();
builder.Services.AddTransient<IMailService, NullMailService>();

var serviceProvider = builder.Services.BuildServiceProvider();

if (args.Length == 1 && args[0].ToLower() == "/seed")
{
    var scopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
    using (var scope = scopeFactory?.CreateScope())
    {
        var seeder = scope?.ServiceProvider.GetService<DutchSeeder>();
        seeder?.Seed();
    }
}

var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandler("/error");

app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();

    endpoints.MapControllerRoute("Default",
        "/{controller}/{action}/{id?}",
        new { controller = "App", action = "Index" });
});

if (!(args.Length == 1 && args[0].ToLower() == "/seed"))
    app.Run();