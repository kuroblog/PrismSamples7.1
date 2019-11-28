
namespace PEF.Modules.SGDE.Services.Dtos
{
    using Newtonsoft.Json;
    using PEF.Common.Models;
    using System.Linq;

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

    public abstract class ServiceItemsDto<TServiceData> : ServiceDto<TServiceData>
        where TServiceData : IServiceData
    {
        [JsonProperty("data")]
        public new TServiceData[] Data { get; set; }

        [JsonIgnore]
        public override bool HasData => Data != null && Data.Any();
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

    #region api/v1/Wardrobe/WardrobeReadCard
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
        /// 衣服尺寸（管理员为空）
        /// </summary>
        [JsonProperty("ClothesSize")]
        public string Size { get; set; }
    }

    public class CardQueryResponse : ServiceDto<CardQueryData> { }

    public class CardQueryRequest
    {
        /// <summary>
        /// 卡号
        /// </summary>
        [JsonProperty("CardID")]
        public string CardId { get; set; }

        /// <summary>
        /// 读取类型 0是刷卡 1是指纹 2是直接管理员登陆
        /// </summary>
        [JsonProperty("ReadStyle")]
        public int ReadStyle { get; set; }

        /// <summary>
        /// 发衣柜编号
        /// </summary>
        [JsonProperty("DeviceNumber")]
        public string DeviceId { get; set; }
    }
    #endregion

    #region api/v1/Wardrobe/GetTrayConfig
    public class ConfigQueryData : IServiceData
    {
        /// <summary>
        /// 尺码配置集合
        /// </summary>
        [JsonProperty("TrayConfigs")]
        public ConfigQueryItem[] Items { get; set; }
    }

    public class ConfigQueryItem
    {
        /// <summary>
        /// 托盘配置表Id
        /// </summary>
        [JsonProperty("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        [JsonProperty("Layer")]
        public int X { get; set; }

        /// <summary>
        /// 索引列
        /// </summary>
        [JsonProperty("IndexColumns")]
        public int Y { get; set; }

        /// <summary>
        /// 尺码
        /// </summary>
        [JsonProperty("Size")]
        public string Size { get; set; }

        /// <summary>
        /// 最大装衣数量
        /// </summary>
        [JsonProperty("MaxNumber")]
        public int Limit { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        [JsonProperty("Stock")]
        public int Stock { get; set; }
    }

    public class ConfigQueryResponse : ServiceDto<ConfigQueryData> { }

    public class ConfigQueryRequest
    {
        /// <summary>
        /// 发衣柜编号
        /// </summary>
        [JsonProperty("DeviceNumber")]
        public string DeviceId { get; set; }
    }
    #endregion

    #region api/v1/Wardrobe/AddStockTraConfig
    public class ConfigSubmitData : IServiceData
    {
        /// <summary>
        /// 成功或失败
        /// </summary>
        [JsonProperty("Status")]
        public bool IsSuccessful { get; set; }
    }

    public class ConfigSubmitResponse : ServiceDto<ConfigSubmitData> { }

    public class ConfigSubmitItem
    {
        /// <summary>
        /// 托盘配置表Id
        /// </summary>
        [JsonProperty("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        [JsonProperty("Stock")]
        public int Stock { get; set; }
    }

    public class ConfigSubmitRequest
    {
        /// <summary>
        /// 发衣柜编号
        /// </summary>
        [JsonProperty("DeviceNumber")]
        public string DeviceId { get; set; }

        /// <summary>
        /// 尺码配置集合
        /// </summary>
        [JsonProperty("trayParms")]
        public ConfigSubmitItem[] Items { get; set; }
    }
    #endregion

    #region api/v1/Wardrobe/ApplyResponse
    public class ApplySubmitData : IServiceData
    {
        /// <summary>
        /// 是否用户已有尺码 true有
        /// </summary>
        [JsonProperty("Status")]
        public bool HasSize { get; set; }

        /// <summary>
        /// 托盘配置表Id
        /// </summary>
        [JsonProperty("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        [JsonProperty("Layer")]
        public int X { get; set; }

        /// <summary>
        /// 索引列
        /// </summary>
        [JsonProperty("IndexColumns")]
        public int Y { get; set; }
    }

    public class ApplySubmitResponse : ServiceDto<ApplySubmitData> { }

    public class ApplySubmitV2Data : ApplySubmitData
    {
        /// <summary>
        /// 存衣柜编号
        /// </summary>
        [JsonProperty("Number")]
        public int SaveNo { get; set; }

        /// <summary>
        /// 手术排班
        /// </summary>
        [JsonProperty("schedulings")]
        public ApplyQuerySchedule[] Schedules { get; set; }
    }

    public class ApplySubmitV2Response : ServiceDto<ApplySubmitV2Data> { }

    public class ApplySubmitRequest
    {
        /// <summary>
        /// 发衣柜编号
        /// </summary>
        [JsonProperty("DeviceNumber")]
        public string DeviceId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [JsonProperty("UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// 用户是否已有尺码默认是1已由2是无（自己选择）
        /// </summary>
        /// <remarks>
        /// 1 向系统申请默认分配， 2 向系统申请手动选择的尺码
        /// </remarks>
        [JsonProperty("Type")]
        public int ApplyMode { get; set; }

        /// <summary>
        /// 尺码
        /// </summary>
        [JsonProperty("Size")]
        public string Size { get; set; }
    }
    #endregion

    #region api/v1/Wardrobe/UserSize
    public class SizeQueryData : IServiceData
    {
        /// <summary>
        /// 尺码配置集合
        /// </summary>
        [JsonProperty("systemSizes")]
        public SizeQueryItem[] Items { get; set; }
    }

    public class SizeQueryItem
    {
        /// <summary>
        /// 尺码 Id
        /// </summary>
        [JsonProperty("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 尺码
        /// </summary>
        [JsonProperty("Size")]
        public string Size { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [JsonProperty("Number")]
        public int Quantity { get; set; }
    }

    public class SizeQueryResponse : ServiceDto<SizeQueryData> { }

    public class SizeQueryRequest
    {
        /// <summary>
        /// 发衣柜编号
        /// </summary>
        [JsonProperty("DeviceNumber")]
        public string DeviceId { get; set; }
    }
    #endregion

    #region api/v1/Wardrobe/WardrobeOk
    public class ApplyQueryData : IServiceData
    {
        /// <summary>
        /// 存衣柜编号
        /// </summary>
        [JsonProperty("Number")]
        public int SaveNo { get; set; }

        /// <summary>
        /// 手术排班
        /// </summary>
        [JsonProperty("schedulings")]
        public ApplyQuerySchedule[] Schedules { get; set; }
    }

    public class ApplyQuerySchedule
    {
        /// <summary>
        /// 手术间
        /// </summary>
        [JsonProperty("Room")]
        public string Room { get; set; }

        /// <summary>
        /// 台序
        /// </summary>
        [JsonProperty("Sort")]
        public string Sequence { get; set; }

        /// <summary>
        /// 手术名称
        /// </summary>
        [JsonProperty("OperationName")]
        public string SurgeryName { get; set; }

        /// <summary>
        /// 医生
        /// </summary>
        [JsonProperty("Doctor")]
        public string SurgeryMember { get; set; }

        /// <summary>
        /// 病人
        /// </summary>
        [JsonProperty("Patient")]
        public string Patient { get; set; }
    }

    public class ApplyQueryResponse : ServiceDto<ApplyQueryData> { }

    public class ApplyQueryRequest
    {
        /// <summary>
        /// 用户卡号
        /// </summary>
        [JsonProperty("CardId")]
        public string CardId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [JsonProperty("UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [JsonProperty("UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// 配置表id
        /// </summary>
        [JsonProperty("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        [JsonProperty("Layer")]
        public int X { get; set; }

        /// <summary>
        /// 索引列
        /// </summary>
        [JsonProperty("IndexColumns")]
        public int Y { get; set; }
    }
    #endregion

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
