using System.Text;
using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Infrastructure.Configuration;
using AlleycatApp.Auth.Repositories;
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

// Add repositories

builder.Services.AddScoped<IRaceRepository, RaceDbRepository>();

// Add infrastructural services

builder.Services.AddScoped<IApplicationConfigurationBuilder, ApplicationConfigurationBuilder>();

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
        ValidAudience = jwtConfig?.Audience,
        ValidIssuer = jwtConfig?.Issuer,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig?.SecretKey!))
    };
});

var app = builder.Build();

app.MapDefaultControllerRoute();

app.Run();
