
namespace PEF.Modules.RecyclingBin.Services.Dtos
{
    using Newtonsoft.Json;
    using PEF.Common.Models;

    public abstract class ServiceBaseDto : ServiceBizBaseDto
    {
        [JsonIgnore]
        public virtual bool IsSuccessful => Code?.ToLower() == "200";
    }

    public interface IServiceData { }

    public abstract class ServiceDto<TServiceData> : ServiceBaseDto
        where TServiceData : IServiceData
    {
        [JsonProperty("data")]
        public TServiceData Data { get; set; }

        [JsonIgnore]
        public virtual bool HasData => Data != null;
    }

    #region api/v1/Token/Login
    public class TokenRequest
    {
        /// <summary>
        /// 卡号
        /// </summary>
        [JsonProperty("CardId")]
        public string CardId { get; set; }

        /// <summary>
        /// 读取类型 0是刷卡 1是指纹 2是直接管理员登陆
        /// </summary>
        [JsonProperty("ReadStyle")]
        public int ReadStyle { get; set; }
    }

    public class TokenData : IServiceData
    {
        [JsonProperty("Token")]
        public string Token { get; set; }
    }

    public class TokenResponse : ServiceDto<TokenData> { }
    #endregion

    #region api/v1/Recovery/RecoveryQuery
    public class CardQueryData : IServiceData
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [JsonProperty("UserID")]
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty("UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户类型 M超级管理员 U普通员工 N无权限
        /// </summary>
        [JsonProperty("UserType")]
        public string UserType { get; set; }

        /// <summary>
        /// 是否有回收的衣服
        /// </summary>
        [JsonProperty("IsRecovery")]
        public bool IsRecovery { get; set; }
    }

    public class CardQueryResponse : ServiceDto<CardQueryData> { }

    public class CardQueryRequest
    {
        /// <summary>
        /// 卡号
        /// </summary>
        [JsonProperty("CardId")]
        public string CardId { get; set; }

        /// <summary>
        /// 读取类型 0是刷卡 1是指纹 2是直接管理员登陆
        /// </summary>
        [JsonProperty("ReadStyle")]
        public int ReadStyle { get; set; }
    }
    #endregion

    #region api/v1/Recovery/RecoveryDefalt
    public class RecyclingBinQueryData : IServiceData
    {
        /// <summary>
        /// 数量 默认单位 件
        /// </summary>
        [JsonProperty("Num")]
        public int Quantity { get; set; }

        /// <summary>
        /// 重量 默认单位 千克
        /// </summary>
        [JsonProperty("Weight")]
        public int Weight { get; set; }
    }

    public class RecyclingBinQueryResponse : ServiceDto<RecyclingBinQueryData> { }

    public class RecyclingBinQueryRequest
    {
        /// <summary>
        /// 回收桶编号
        /// </summary>
        [JsonProperty("DeviceNumber")]
        public string DeviceId { get; set; }
    }
    #endregion

    #region api/v1/Recovery/RecoveryReturn
    public class RecyclingBinSubmitData : IServiceData
    {
        [JsonProperty("Flag")]
        public bool IsSuccessful { get; set; }
    }

    public class RecyclingBinSubmitResponse : ServiceDto<RecyclingBinSubmitData> { }

    public class RecyclingBinSubmitRequest
    {
        /// <summary>
        /// 用户卡号
        /// </summary>
        [JsonProperty("CardId")]
        public string CardId { get; set; }

        /// <summary>
        /// 回收桶编号
        /// </summary>
        [JsonProperty("DeviceNumber")]
        public string DeviceId { get; set; }
    }
    #endregion

    #region api/v1/Recovery/RecoveryEmpty
    public class RecyclingBinCleanData : RecyclingBinSubmitData { }

    public class RecyclingBinCleanResponse : ServiceDto<RecyclingBinCleanData> { }

    public class RecyclingBinCleanRequest
    {
        /// <summary>
        /// 回收桶编号
        /// </summary>
        [JsonProperty("DeviceNumber")]
        public string DeviceId { get; set; }
    }
    #endregion

    //#region api/v1/Login/CustemLogin
    //public class RecyclingBinAdminVerifyData : IServiceData
    //{
    //    /// <summary>
    //    /// 用户类型 M超级管理员 U普通员工 N无权限
    //    /// </summary>
    //    [JsonProperty("UserType")]
    //    public string UserType { get; set; }

    //    /// <summary>
    //    /// 用户Id
    //    /// </summary>
    //    [JsonProperty("UserID")]
    //    public long UserId { get; set; }

    //    /// <summary>
    //    /// 用户名
    //    /// </summary>
    //    [JsonProperty("UserName")]
    //    public string UserName { get; set; }
    //}

    //public class RecyclingBinAdminVerifyResponse : ServiceDto<RecyclingBinAdminVerifyData> { }

    //public class RecyclingBinAdminVerifyRequest
    //{
    //    /// <summary>
    //    /// 用户卡号
    //    /// </summary>
    //    [JsonProperty("CardId")]
    //    public string CardId { get; set; }

    //    /// <summary>
    //    /// 用户密码
    //    /// </summary>
    //    [JsonProperty("Password")]
    //    public string Password { get; set; }
    //}
    //#endregion

    #region api/v1/Socket/Socket
    public class SocketStatusReportData : IServiceData
    {
        /// <summary>
        /// 成功或失败
        /// </summary>
        [JsonProperty("Status")]
        public bool IsSuccessful { get; set; }
    }

    public class SocketStatusReportResponse : ServiceDto<SocketStatusReportData> { }

    public class SocketStatusReportRequest
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [JsonProperty("Code")]
        public string DeviceId { get; set; }

        /// <summary>
        /// socket状态 1是上线 2是下线
        /// </summary>
        [JsonProperty("Status")]
        public int Status { get; set; }
    }
    #endregion

    public class LogReportRequest
    {
        [JsonProperty("Code")]
        public string DeviceId { get; set; }

        [JsonProperty("Action")]
        public int LogType { get; set; }

        [JsonProperty("Message")]
        public string JsonLogs { get; set; }

        [JsonProperty("UserId")]
        public long UserId { get; set; }
    }
}
