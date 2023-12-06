using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Infrastructure;
using AlleycatApp.Auth.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IRaceRepository, RaceDbRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var provider = scope.ServiceProvider;
    var context = provider.GetRequiredService<ApplicationDbContext>();

    if(context.Database.GetPendingMigrations().Any())
        context.Database.Migrate();

    var repository = provider.GetRequiredService<IRaceRepository>();
    await DataFactory.InsertRacesAsync(repository, DataFactory.GetSampleRaces());
}

app.MapDefaultControllerRoute();

app.Run();
