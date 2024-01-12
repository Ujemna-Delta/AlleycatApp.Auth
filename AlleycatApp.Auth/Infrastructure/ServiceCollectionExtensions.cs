using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Infrastructure.Configuration;
using AlleycatApp.Auth.Models.Users;
using AlleycatApp.Auth.Repositories.Bikes;
using AlleycatApp.Auth.Repositories.Leagues;
using AlleycatApp.Auth.Repositories.Points;
using AlleycatApp.Auth.Repositories.Races;
using AlleycatApp.Auth.Repositories.Tasks;
using AlleycatApp.Auth.Repositories.Users;
using AlleycatApp.Auth.Services.Account;
using AlleycatApp.Auth.Services.Authentication;
using AlleycatApp.Auth.Services.Authentication.Jwt;
using AlleycatApp.Auth.Services.Providers;
using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAlleycatAppIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityCore<Manager>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityCore<Pointer>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityCore<Attendee>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }

        public static IServiceCollection AddInfrastructuralServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicationConfigurationBuilder, ApplicationConfigurationBuilder>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }

        public static IServiceCollection AddProviders(this IServiceCollection services)
        {
            services.AddScoped<IUserServicesProvider, UserServicesProvider>();
            services.AddScoped<IUserDataProvider, UserDataProvider>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ILeagueRepository, LeagueDbRepository>();
            services.AddScoped<IRaceRepository, RaceDbRepository>();
            services.AddScoped<IPointRepository, PointDbRepository>();
            services.AddScoped<ITaskRepository, TaskDbRepository>();
            services.AddScoped<IBikeRepository, BikeDbRepository>();

            services.AddScoped<IRaceCompletionRepository, RaceCompletionDbRepository>();
            services.AddScoped<IPointCompletionRepository, PointCompletionDbRepository>();
            services.AddScoped<ITaskCompletionRepository, TaskCompletionDbRepository>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRaceAttendanceRepository, RaceAttendanceDbRepository>();

            return services;
        }

        public static IServiceCollection AddUserServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, JwtAuthenticationService>();
            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}
