
namespace PEF.Modules.RecyclingBin
{
    using PEF.Common;

    /// <summary>
    /// 获取回收桶模块的配置信息
    /// </summary>
    public class RecyclingBinConfig : ConfigManager
    {
        /// <summary>
        /// 模拟 api 返回数据
        /// </summary>
        public bool IsMockApiResponse => bool.TryParse(ReadAppSetting(nameof(IsMockApiResponse)), out bool result) ? result : false;

        /// <summary>
        /// 是否启用调式按钮(已废弃)
        /// </summary>
        public bool EnableDebugMenu => bool.Parse(ReadAppSetting(nameof(EnableDebugMenu)));

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version => ReadAppSetting(nameof(Version));

        /// <summary>
        /// 机器号
        /// </summary>
        public string DeviceNo => ReadAppSetting(nameof(DeviceNo));

        /// <summary>
        /// 故障维修
        /// </summary>
        public string ServiceCall => ReadAppSetting(nameof(ServiceCall));

        /// <summary>
        /// 技术支持
        /// </summary>
        public string Company => ReadAppSetting(nameof(Company));

        /// <summary>
        /// 产品(公司)
        /// </summary>
        public string Product => ReadAppSetting(nameof(Product));

        /// <summary>
        /// 产品名称
        /// </summary>
        public string Title => ReadAppSetting(nameof(Title));

        /// <summary>
        /// 设备 Socket 地址
        /// </summary>
        public string SocketUrl => ReadAppSetting(nameof(SocketUrl));

        //public int BufferSize => int.Parse(GetAppSetting(nameof(BufferSize)));

        /// <summary>
        /// Socket 数据缓冲长度
        /// </summary>
        public int BufferSize => int.TryParse(ReadAppSetting(nameof(BufferSize)), out int result) ? result : 1024;

        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceId => ReadAppSetting(nameof(DeviceId));

        /// <summary>
        /// 后台服务地址
        /// </summary>
        public string ServiceUrl => ReadAppSetting(nameof(ServiceUrl));

        /// <summary>
        /// 卡查询路由
        /// </summary>
        public string CardQueryUrl => ReadAppSetting(nameof(CardQueryUrl));

        /// <summary>
        /// 回收桶查询路由
        /// </summary>
        public string RecyclingBinQueryUrl => ReadAppSetting(nameof(RecyclingBinQueryUrl));

        /// <summary>
        /// 回收桶上传路由
        /// </summary>
        public string RecyclingBinSubmitUrl => ReadAppSetting(nameof(RecyclingBinSubmitUrl));

        /// <summary>
        /// 回收桶清空衣物路由
        /// </summary>
        public string RecyclingBinCleanUrl => ReadAppSetting(nameof(RecyclingBinCleanUrl));

        ///// <summary>
        ///// 管理员验证路由
        ///// </summary>
        //public string RecyclingBinAdminVerifyActionUrl => ReadAppSetting(nameof(RecyclingBinAdminVerifyActionUrl));

        /// <summary>
        /// Socket 状态报告路由
        /// </summary>
        public string SocketStatusReportUrl => ReadAppSetting(nameof(SocketStatusReportUrl));

        public string TokenRequestUrl => ReadAppSetting(nameof(TokenRequestUrl));

        public string LogReportUrl => ReadAppSetting(nameof(LogReportUrl));
    }
}
