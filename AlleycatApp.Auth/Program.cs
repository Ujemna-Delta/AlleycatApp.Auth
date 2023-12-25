using System.Text;
using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Infrastructure.Configuration;
using AlleycatApp.Auth.Repositories;
using AlleycatApp.Auth.Services.Authentication;
using AlleycatApp.Auth.Services.Authentication.Jwt;
using AlleycatApp.Auth.Services.Registration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAutoMapper(typeof(Program));

// Add infrastructural services

builder.Services.AddScoped<IApplicationConfigurationBuilder, ApplicationConfigurationBuilder>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

// Add repositories

builder.Services.AddScoped<IRaceRepository, RaceDbRepository>();

// Add services

builder.Services.AddScoped<IAuthenticationService, JwtAuthenticationService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

var jwtConfig = new ApplicationConfigurationBuilder(builder.Configuration)
    .BuildJwtConfiguration()
    .Build()
    .JwtConfiguration;

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
        ValidAudience = jwtConfig!.Audience,
        ValidIssuer = jwtConfig.Issuer,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey))
    };
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapGet("/secret", () => "This is a secret page, you have a valid token.").RequireAuthorization();

app.Run();
