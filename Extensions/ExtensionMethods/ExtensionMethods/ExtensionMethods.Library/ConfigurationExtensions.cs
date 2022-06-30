using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace ExtensionMethods.Library
{
    public static class ConfigurationExtensions
    {
        public static bool IsLoaded(this IConfiguration config)
        {
            return config != null && config.AsEnumerable().Any();
        }

        public static IConfigurationBuilder AddStandardProviders(this IConfigurationBuilder configurationBuilder)
        {
            return configurationBuilder.AddJsonFile("appsettings.json")
                                       .AddEnvironmentVariables()
                                       .AddJsonFile("config/config.json", optional: true)
                                       .AddJsonFile("secrets/secrets.json", optional: true);
        }
    } 
}

