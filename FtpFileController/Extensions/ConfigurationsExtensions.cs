using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace FtpFileController.Extensions {
    public static class ConfigurationsExtensions {
        public static object GetConfiguration(Type configType, string directory = "") {
            var relativeFileName = configType.GetFileName();
            if (!string.IsNullOrEmpty(directory)) {
                relativeFileName = Path.Combine(directory, relativeFileName);
            }
            
            var configuration = new ConfigurationBuilder().AddJsonFile(relativeFileName).Build();
            var configurationInstance = Activator.CreateInstance(configType);
            configuration.Bind(configurationInstance);

            return configurationInstance;
        }
        
        public static TKey GetConfiguration<TKey>(string directory = "") where TKey : new() {
            return (TKey) GetConfiguration(typeof(TKey), directory);
        }

        public static void OverrideConfigurationFile<T>(this T config, string directory = "") {
            var configType = config.GetType<T>();
            
            var relativeFileName = configType.GetFileName();
            if (!string.IsNullOrEmpty(directory)) {
                relativeFileName = Path.Combine(directory, relativeFileName);
            }

            var jsonPayload = JsonSerializer.Serialize(config);
            File.WriteAllText(relativeFileName, jsonPayload, Encoding.Default);
        }

        private static string GetFileName(this Type configType) => $"{configType.Name}.json";
    }
}