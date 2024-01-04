using System.Text;
using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Infrastructure;
using AlleycatApp.Auth.Infrastructure.Configuration;
using AlleycatApp.Auth.Models.Users;
using AlleycatApp.Auth.Repositories.Bikes;
using AlleycatApp.Auth.Services.Authentication;
using AlleycatApp.Auth.Services.Authentication.Jwt;
using AlleycatApp.Auth.Services.Account;
using AlleycatApp.Auth.Services.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AlleycatApp.Auth.Repositories.Leagues;
using AlleycatApp.Auth.Repositories.Races;
using AlleycatApp.Auth.Repositories.Points;
using AlleycatApp.Auth.Repositories.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddCors();

// Add identity

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityCore<Manager>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityCore<Pointer>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityCore<Attendee>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add infrastructural services

builder.Services.AddScoped<IApplicationConfigurationBuilder, ApplicationConfigurationBuilder>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

// Add repositories

builder.Services.AddScoped<ILeagueRepository, LeagueDbRepository>();
builder.Services.AddScoped<IRaceRepository, RaceDbRepository>();
builder.Services.AddScoped<IPointRepository, PointDbRepository>();
builder.Services.AddScoped<ITaskRepository, TaskDbRepository>();
builder.Services.AddScoped<IBikeRepository, BikeDbRepository>();

// Add providers

builder.Services.AddScoped<IUserServicesProvider, UserServicesProvider>();
builder.Services.AddScoped<IUserDataProvider, UserDataProvider>();

// Add services

builder.Services.AddScoped<IAuthenticationService, JwtAuthenticationService>();
builder.Services.AddScoped<IAccountService, AccountService>();

var appConfig = new ApplicationConfigurationBuilder(builder.Configuration)
    .BuildJwtConfiguration()
    .BuildInitialManagerCredentials()
    .Build();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = appConfig.JwtConfiguration!.Audience,
        ValidIssuer = appConfig.JwtConfiguration.Issuer,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfig.JwtConfiguration.SecretKey))
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var accountService = serviceProvider.GetRequiredService<IAccountService>();

    if(context.Database.GetPendingMigrations().Any())
        context.Database.Migrate();

    await DataFactory.EnsureRolesAsync(roleManager);
    await DataFactory.CreateInitialManager(accountService, appConfig.InitialManagerUserName, appConfig.InitialManagerPassword);
}

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapGet("/secret", () => "This is a secret page, you have a valid token.").RequireAuthorization();

app.UseCors(options => options.AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.Run();
