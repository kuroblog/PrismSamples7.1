
namespace PEF.Shell
{
    using PEF.Common;

    /// <summary>
    /// 获取框架模块的配置信息
    /// </summary>
    public class ShellConfig : ConfigManager
    {
        /// <summary>
        /// 获取应用程序的宽度
        /// </summary>
        public int Width => int.TryParse(ReadAppSetting(nameof(Width)), out int result) ? result : 1280;

        /// <summary>
        /// 获取应用程序的高度
        /// </summary>
        public int Height => int.TryParse(ReadAppSetting(nameof(Height)), out int result) ? result : 1024;

        /// <summary>
        /// 获取要装配的模块名称
        /// </summary>
        public string ModuleNames => ReadAppSetting(nameof(ModuleNames));
    }
}
