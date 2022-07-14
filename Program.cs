using BigProject.Data;
using BigProject.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.Sources.Clear();
    config.SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("config.json")
           .AddEnvironmentVariables();
});

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddNewtonsoftJson(cfg => 
        cfg.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );
builder.Services.AddRazorPages();

builder.Services.AddDbContext<DutchContext>();
builder.Services.AddTransient<DutchSeeder>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IDutchRepository, DutchRepository>();

builder.Services.AddTransient<IMailService, NullMailService>();

if (DutchSeeder.IsSeeding(args))
    DutchSeeder.SeedDb(builder.Services.BuildServiceProvider());

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

if (!DutchSeeder.IsSeeding(args))
    app.Run();