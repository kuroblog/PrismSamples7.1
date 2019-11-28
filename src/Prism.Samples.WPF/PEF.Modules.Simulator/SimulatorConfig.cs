
namespace PEF.Modules.Simulator
{
    using PEF.Common;

    public class SimulatorConfig : ConfigManager
    {
        ///// <summary>
        ///// 模拟 api 返回数据
        ///// </summary>
        //public bool IsMockApiResponse => bool.TryParse(ReadAppSetting(nameof(IsMockApiResponse)), out bool result) ? result : false;

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version => ReadAppSetting(nameof(Version));
    }
}
