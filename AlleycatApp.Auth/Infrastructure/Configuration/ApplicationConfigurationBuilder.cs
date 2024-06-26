﻿using AlleycatApp.Auth.Infrastructure.Exceptions;

namespace AlleycatApp.Auth.Infrastructure.Configuration
{
    public class ApplicationConfigurationBuilder(IConfiguration configuration) : IApplicationConfigurationBuilder
    {
        private readonly ApplicationConfiguration _appConfig = new();

        public IApplicationConfigurationBuilder BuildJwtConfiguration()
        {
            var audience = TryGetStringValue("Jwt:Audience");
            var issuer = TryGetStringValue("Jwt:Issuer");
            var expirationTimeMinutes = TryGetValue<int>("Jwt:ExpirationTimeMinutes");
            var secretKey = TryGetStringValue("Jwt:SecretKey");

            if (secretKey.Length < 32)
                throw new ConfigurationException("Jwt:SecretKey must be at least 32 characters long.", "Jwt:SecretKey");

            if (expirationTimeMinutes <= 0)
                throw new ConfigurationException("Jwt:ExpirationTimeMinutes must be set to at least 1.", "Jwt:ExpirationTimeMinutes");

            _appConfig.JwtConfiguration = new JwtConfiguration(audience, issuer, expirationTimeMinutes, secretKey);
            return this;
        }

        public IApplicationConfigurationBuilder BuildInitialManagerCredentials()
        {
            var userName = TryGetStringValue("InitialManagerUserName");
            var password = TryGetStringValue("InitialManagerPassword");

            _appConfig.InitialManagerUserName = userName;
            _appConfig.InitialManagerPassword = password;

            return this;
        }

        public IApplicationConfigurationBuilder BuildDataSeedingOptions()
        {
            _appConfig.ClearOnInit = TryGetValue<bool>("ClearOnInit");
            _appConfig.SeedData = TryGetValue<bool>("SeedData");

            return this;
        }

        public ApplicationConfiguration Build() => _appConfig;

        private T TryGetValue<T>(string key) => configuration.GetValue<T>(key) ??
                                                        throw new ConfigurationException($"{key} configuration value not set.", key);

        private string TryGetStringValue(string key)
        {
            var value = TryGetValue<string>(key);
            if (string.IsNullOrEmpty(value))
                throw new ConfigurationException($"{key} configuration value cannot be set to empty string.", key);

            return value;
        }
    }
}
