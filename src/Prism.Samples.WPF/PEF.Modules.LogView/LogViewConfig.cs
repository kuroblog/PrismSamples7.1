
namespace PEF.Modules.LogView
{
    using PEF.Common;

    public class LogViewConfig : ConfigManager
    {
        public string Version => ReadAppSetting(nameof(Version));
    }
}
