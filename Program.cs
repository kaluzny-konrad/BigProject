using BigProject.Data;
using BigProject.Data.Entities;
using BigProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

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
builder.Services.AddTransient<IMailService, NullMailService>();
builder.Services.AddIdentity<StoreUser, IdentityRole>(cfg =>
    {
        cfg.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<DutchContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication()
    .AddCookie()
    .AddJwtBearer(cfg =>
    {
        cfg.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = builder.Configuration.GetValue<string>("Token:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("Token:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetValue<string>("Token:Key")
            ))
        };
    });
builder.Services.AddTransient<DutchSeeder>();
builder.Services.AddScoped<IDutchRepository, DutchRepository>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var serviceProvider = builder.Services.BuildServiceProvider();
var scopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    if (DutchSeeder.IsSeeding(args))
    {
        var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
        seeder.SeedAsync().Wait();
    }
}

var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandler("/error");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();

    endpoints.MapControllerRoute("Default",
        "/{controller}/{action}/{id?}",
        new { controller = "App", action = "Index" });
});

if (!DutchSeeder.IsSeeding(args))
    app.Run();