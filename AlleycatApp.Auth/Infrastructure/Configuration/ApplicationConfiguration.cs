namespace AlleycatApp.Auth.Infrastructure.Configuration
{
    public record ApplicationConfiguration
    {
        public JwtConfiguration? JwtConfiguration { get; set; }
        public string InitialManagerUserName { get; set; } = string.Empty;
        public string InitialManagerPassword { get; set; } = string.Empty;
    }
}
