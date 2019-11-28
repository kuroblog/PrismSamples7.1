
namespace PEF.Modules.ShoeBox
{
    using PEF.Common;

    public class ShoeBoxConfig : ConfigManager
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
        /// 设备状态查询路由
        /// </summary>
        public string DeviceStateQueryUrl => ReadAppSetting(nameof(DeviceStateQueryUrl));

        /// <summary>
        /// 刷卡查询路由
        /// </summary>
        public string CardQueryUrl => ReadAppSetting(nameof(CardQueryUrl));

        /// <summary>
        /// 设备申请路由
        /// </summary>
        public string DeviceRegistrationUrl => ReadAppSetting(nameof(DeviceRegistrationUrl));

        /// <summary>
        /// 设备配置查询路由
        /// </summary>
        public string DeviceConfigQueryUrl => ReadAppSetting(nameof(DeviceConfigQueryUrl));

        /// <summary>
        /// 设备重置路由
        /// </summary>
        public string DeviceResetUrl => ReadAppSetting(nameof(DeviceResetUrl));

        public string TokenRequestUrl => ReadAppSetting(nameof(TokenRequestUrl));

        public string DeviceItemStateQueryUrl => ReadAppSetting(nameof(DeviceItemStateQueryUrl));

        public string SocketStatusReportUrl => ReadAppSetting(nameof(SocketStatusReportUrl));

        public string LogReportUrl => ReadAppSetting(nameof(LogReportUrl));
    }
}
