
namespace PEF.Modules.SGDE
{
    using PEF.Common;

    /// <summary>
    /// 获取发衣柜模块的配置信息
    /// </summary>
    public class SGDEConfig : ConfigManager
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
        /// 托盘配置查询路由
        /// </summary>
        public string ConfigQueryUrl => ReadAppSetting(nameof(ConfigQueryUrl));

        /// <summary>
        /// 托盘配置上传路由
        /// </summary>
        public string ConfigSubmitUrl => ReadAppSetting(nameof(ConfigSubmitUrl));

        /// <summary>
        /// 发衣申请路由
        /// </summary>
        public string ApplySubmitUrl => ReadAppSetting(nameof(ApplySubmitUrl));

        /// <summary>
        /// 尺码查询路由
        /// </summary>
        public string SizeQueryUrl => ReadAppSetting(nameof(SizeQueryUrl));

        /// <summary>
        /// 发衣结果查询路由
        /// </summary>
        public string ApplyQueryUrl => ReadAppSetting(nameof(ApplyQueryUrl));

        /// <summary>
        /// Socket 状态报告路由
        /// </summary>
        public string SocketStatusReportUrl => ReadAppSetting(nameof(SocketStatusReportUrl));

        public string TokenRequestUrl => ReadAppSetting(nameof(TokenRequestUrl));

        public string LogReportUrl => ReadAppSetting(nameof(LogReportUrl));
    }
}
