namespace AlleycatApp.Auth.Infrastructure.Configuration
{
    public interface IApplicationConfigurationBuilder
    {
        IApplicationConfigurationBuilder BuildJwtConfiguration();
        ApplicationConfiguration Build();
    }
}
