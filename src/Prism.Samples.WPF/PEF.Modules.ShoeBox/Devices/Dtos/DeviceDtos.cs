
namespace PEF.Modules.ShoeBox.Devices.Dtos
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using PEF.Common.Models;
    using System;

    public class DeviceBaseDto : DeviceBizBaseDto
    {
        [JsonIgnore]
        public virtual DeviceMethodTypes MethodType => Enum.TryParse(Method, out DeviceMethodTypes result) ? result : DeviceMethodTypes.Unknown;
    }

    public interface IDeviceParameter { }

    public class DeviceDto<TDeviceParameter> : DeviceBaseDto
        where TDeviceParameter : IDeviceParameter
    {
        [JsonProperty("Parameter")]
        public TDeviceParameter Parameter { get; set; }
    }

    public enum DeviceMethodTypes
    {
        Unknown,
        FrontInfo,
        SHReadCard,
        SBReadCard,
        OpenDoor,
        DoorState
        //DoorControl,
        //LoadingSuccess,
        //LoadingSuccessReturn,
        //Delivery,
        //DeliveryReturn
    }

    #region FrontInfo/FC0-R0-0
    public class DeviceFrontInfoParameter : IDeviceParameter
    {
        [JsonProperty("DeviceType")]
        public string DeviceType { get; set; }

        [JsonProperty("DeviceVersion")]
        public string DeviceVersion { get; set; }

        [JsonProperty("DeviceID")]
        public string DeviceId { get; set; }
    }

    public class DeviceFrontInfoDto : DeviceDto<DeviceFrontInfoParameter> { }
    #endregion

    #region ReadCard/FC3-R0-0
    public class DeviceReadCardParameter : IDeviceParameter
    {
        [JsonProperty("CardID")]
        public string CardId { get; set; }

        [JsonProperty("ReadStyle")]
        public string ReadStyle { get; set; }
    }

    public class DeviceReadCardDto : DeviceDto<DeviceReadCardParameter> { }
    #endregion

    #region OpenDoor/TC2-S0-0
    public class DeviceOpenDoorParameter : IDeviceParameter
    {
        [JsonProperty("OperationType")]
        public string OpenMode { get; set; }

        [JsonProperty("Operation")]
        public string[] DoorIds { get; set; }
    }

    public class DeviceOpenDoorDto : DeviceDto<DeviceOpenDoorParameter>
    {
        public DeviceOpenDoorDto(string[] doorIds, string openMode = "0")
        {
            Method = DeviceMethodTypes.OpenDoor.ToString();
            Parameter = new DeviceOpenDoorParameter
            {
                OpenMode = openMode,
                DoorIds = doorIds
            };
        }
    }
    #endregion

    #region DoorState/FS-R1-0
    public class DeviceDoorStateParameter : IDeviceParameter
    {
        [JsonProperty("DoorId")]
        public string DoorId { get; set; }

        /// <summary>
        /// state门状态0:关门1:开门
        /// </summary>
        [JsonProperty("State")]
        public string State { get; set; }
    }

    public class DeviceDoorStateDto : DeviceDto<DeviceDoorStateParameter> { }
    #endregion

    //#region DoorControl/TC3-S1-0
    //public class DeviceDoorControlParameter : IDeviceParameter
    //{
    //    [JsonProperty("Type")]
    //    public string TypeId { get; set; }
    //}

    //public class DeviceDoorControlDto : DeviceDto<DeviceDoorControlParameter>
    //{
    //    public DeviceDoorControlDto(string typeId = "1")
    //    {
    //        Method = DeviceMethodTypes.DoorControl.ToString();
    //        Parameter = new DeviceDoorControlParameter
    //        {
    //            TypeId = typeId
    //        };
    //    }
    //}
    //#endregion

    //#region LoadingSuccess/TC3-S2-0
    //public class DeviceLoadingSuccessParameter : IDeviceParameter { }

    //public class DeviceLoadingSuccessDto : DeviceDto<DeviceLoadingSuccessParameter>
    //{
    //    public DeviceLoadingSuccessDto()
    //    {
    //        Method = DeviceMethodTypes.LoadingSuccess.ToString();
    //        Parameter = null;
    //    }
    //}
    //#endregion

    //#region LoadingSuccessReturn/FC3-R2-0
    //public class DeviceLoadingSuccessReturnParameter : IDeviceParameter { }

    //public class DeviceLoadingSuccessReturnDto : DeviceDto<DeviceLoadingSuccessReturnParameter> { }
    //#endregion

    //#region Delivery/TC3-S0-0
    //public class DeviceDeliveryParameter : IDeviceParameter
    //{
    //    [JsonProperty("Type")]
    //    public string TypeId { get; set; }
    //}

    //public class DeviceDeliveryDto : DeviceDto<DeviceDeliveryParameter>
    //{
    //    public DeviceDeliveryDto(int x, int y)
    //    {
    //        Method = DeviceMethodTypes.Delivery.ToString();
    //        Parameter = new DeviceDeliveryParameter
    //        {
    //            TypeId = $"{x}-{y}"
    //        };
    //    }
    //}
    //#endregion

    //#region DeliveryReturn/FC3-R1-0
    //public class DeviceDeliveryReturnParameter : IDeviceParameter
    //{
    //    /// <summary>
    //    /// Code存放前置机发过来的异常信息类型,为"0"表示出货成功。 "1"表示空货; ”2”表示出货失败
    //    /// </summary>
    //    [JsonProperty("Code")]
    //    public string Code { get; set; }

    //    [JsonProperty("RFIDs")]
    //    public string[] RFIDs { get; set; }
    //}

    //public class DeviceDeliveryReturnDto : DeviceDto<DeviceDeliveryReturnParameter> { }
    //#endregion
}
