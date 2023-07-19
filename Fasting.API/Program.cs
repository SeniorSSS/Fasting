using Fasting.Host;
using Fasting.Identity.Database;
using Fasting.Identity.Models;
using Fasting.Repository.Databases;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", true, true);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddCors();

// Add your DbContexts
builder.Services.AddDbContext<FastingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings:FastingDb").Value));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings:IdentityDb").Value));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add IdentityServer
builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryPersistedGrants()
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddInMemoryApiScopes(Config.GetApiScopes())
    .AddInMemoryClients(Config.GetClients())
    .AddAspNetIdentity<ApplicationUser>();

var app = builder.Build();

// Create roles
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var RoleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
    string[] roleNames = { "Admin", "User" };
    foreach (var roleName in roleNames)
    {
        bool roleExist = await RoleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await RoleManager.CreateAsync(new ApplicationRole { Name = roleName });
        }
    }
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while creating roles");
}

// Configure middleware
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.Run();