
namespace PEF.Common.Infrastructures
{
    /// <summary>
    /// 获取配置文件的节点信息
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// 获取 AppSettings 的节点信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string ReadAppSetting(string key);

        /// <summary>
        /// 保存 AppSettings 的节点信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SaveAppSetting(string key, string value);

        /// <summary>
        /// 获取 ConnectionStrings 的节点信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string ReadConnectionString(string key);

        string ReadAllText(string path);
    }
}
