
namespace PEF.Common
{
    using PEF.Common.Infrastructures;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// 获取配置文件的节点信息
    /// </summary>
    public class ConfigManager : IConfigManager
    {
        /// <summary>
        /// 获取 AppSettings 的节点信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReadAppSetting(string key) => ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
        {
            ExeConfigFilename = string.Concat(Assembly.GetCallingAssembly().Location, ".config")
        }, ConfigurationUserLevel.None).AppSettings.Settings[key].Value;

        /// <summary>
        /// 保存 AppSettings 的节点信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SaveAppSetting(string key, string value)
        {
            var config = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
            {
                ExeConfigFilename = string.Concat(Assembly.GetCallingAssembly().Location, ".config")
            }, ConfigurationUserLevel.None);

            if (config.AppSettings.Settings.AllKeys.Contains(key))
            {
                var result = config.AppSettings.Settings[key].Value;
                if (result != value)
                {
                    config.AppSettings.Settings[key].Value = value;
                    config.Save(ConfigurationSaveMode.Modified);
                }
            }
            else
            {
                config.AppSettings.Settings.Add(key, value);
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        /// <summary>
        /// 获取 ConnectionStrings 的节点信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReadConnectionString(string key) => ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
        {
            ExeConfigFilename = string.Concat(Assembly.GetCallingAssembly().Location, ".config")
        }, ConfigurationUserLevel.None).ConnectionStrings.ConnectionStrings[key].ConnectionString;

        public string ReadAllText(string path) => File.ReadAllText(path);
    }
}
