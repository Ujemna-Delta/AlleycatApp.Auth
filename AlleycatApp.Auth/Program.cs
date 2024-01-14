using System.Text;
using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Infrastructure;
using AlleycatApp.Auth.Infrastructure.Configuration;
using AlleycatApp.Auth.Services.Account;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), opts => opts.EnableRetryOnFailure()));

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAlleycatAppIdentity();
builder.Services.AddInfrastructuralServices();
builder.Services.AddProviders();
builder.Services.AddRepositories();
builder.Services.AddUserServices();

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

    if (args.Contains("--clear"))
        await DataFactory.ClearDatabase(context);

    if (args.Contains("--seed"))
        await DataFactory.SeedSampleData(serviceProvider);
}

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapDefaultControllerRoute();
app.MapGet("/secret", () => "This is a secret page, you have a valid token.").RequireAuthorization();

app.UseCors(options => options.AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod());

app.Run();
