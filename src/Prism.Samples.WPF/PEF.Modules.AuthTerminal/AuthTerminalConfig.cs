
namespace PEF.Modules.AuthTerminal
{
    using PEF.Common;

    /// <summary>
    /// 获取发衣柜模块的配置信息
    /// </summary>
    public class AuthTerminalConfig : ConfigManager
    {
        /// <summary>
        /// 模拟 api 返回数据
        /// </summary>
        public bool IsMockApiResponse => bool.TryParse(ReadAppSetting(nameof(IsMockApiResponse)), out bool result) ? result : false;

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version => ReadAppSetting(nameof(Version));

        /// <summary>
        /// 设备 Socket 地址
        /// </summary>
        public string SocketUrl => ReadAppSetting(nameof(SocketUrl));

        /// <summary>
        /// 后台服务地址
        /// </summary>
        public string ServiceUrl => ReadAppSetting(nameof(ServiceUrl));

        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceId => ReadAppSetting(nameof(DeviceId));

        /// <summary>
        /// 卡查询路由
        /// </summary>
        public string CardQueryUrl => ReadAppSetting(nameof(CardQueryUrl));

        /// <summary>
        /// 卡查询路由
        /// </summary>
        public string RoleQueryUrl => ReadAppSetting(nameof(RoleQueryUrl));

        public string UserRoleUrl => ReadAppSetting(nameof(UserRoleUrl));

        public string TokenRequestUrl => ReadAppSetting(nameof(TokenRequestUrl));

        public string SocketStatusReportUrl => ReadAppSetting(nameof(SocketStatusReportUrl));
    }
}
