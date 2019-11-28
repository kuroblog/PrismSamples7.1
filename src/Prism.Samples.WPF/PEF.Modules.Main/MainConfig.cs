
namespace PEF.Modules.Main
{
    using PEF.Common;

    /// <summary>
    /// 获取主模块的配置信息
    /// </summary>
    public class MainConfig : ConfigManager
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version => ReadAppSetting(nameof(Version));

        ///// <summary>
        ///// 默认加载的模块（废弃）
        ///// </summary>
        //public string ContentModule => ReadAppSetting(nameof(ContentModule));
    }
}
