using System;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using Zolibrary.Logging;

namespace Zolibrary.Config
{
    public class ConfigManager
    {
        private string configFileName = "Config.json";
        private string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public T LoadConfig<T>(T config)
        {
            try
            {
                string configFilePath = Path.Combine(assemblyPath, this.configFileName);
                bool exists = File.Exists(configFilePath);
                if (!exists)
                {
                    LogManager.Log("Config doesn't exist, therefore creating config.");
                    SaveConfig(config);
                    return config;
                }
                string value = File.ReadAllText(configFilePath);
                T readConfig = JsonConvert.DeserializeObject<T>(value);
                SaveConfig(readConfig);
                return readConfig;
            }
            catch (Exception e)
            {
                LogManager.LogError("Error when loading config file:" + e);
            }
            return config;
        }

        public void SaveConfig(object config)
        {
            try
            {
                string contents = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(Path.Combine(this.assemblyPath, this.configFileName), contents);
            }
            catch (Exception e)
            {
                LogManager.LogError("Error when saving config file:" + e);
            }
        }
    }
}
