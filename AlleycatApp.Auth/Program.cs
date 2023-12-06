using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IRaceRepository, RaceDbRepository>();

var app = builder.Build();

app.MapDefaultControllerRoute();

app.Run();
