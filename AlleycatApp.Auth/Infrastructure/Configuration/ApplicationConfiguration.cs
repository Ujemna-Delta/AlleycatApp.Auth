namespace AlleycatApp.Auth.Infrastructure.Configuration
{
    public record ApplicationConfiguration
    {
        public JwtConfiguration? JwtConfiguration { get; set; }
    }
}
