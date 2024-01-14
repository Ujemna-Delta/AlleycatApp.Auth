namespace AlleycatApp.Auth.Infrastructure.Configuration
{
    public interface IApplicationConfigurationBuilder
    {
        IApplicationConfigurationBuilder BuildJwtConfiguration();
        IApplicationConfigurationBuilder BuildInitialManagerCredentials();
        IApplicationConfigurationBuilder BuildDataSeedingOptions();
        ApplicationConfiguration Build();
    }
}
