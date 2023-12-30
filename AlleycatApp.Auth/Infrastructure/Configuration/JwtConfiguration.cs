namespace AlleycatApp.Auth.Infrastructure.Configuration
{
    public record JwtConfiguration(string Audience, string Issuer, int ExpirationTimeMinutes, string SecretKey);
}
