namespace AlleycatApp.Auth.Infrastructure.Exceptions
{
    public class ConfigurationException(string message, string keyName) : Exception(message)
    {
        public string KeyName { get; private set; } = keyName;
    }
}
