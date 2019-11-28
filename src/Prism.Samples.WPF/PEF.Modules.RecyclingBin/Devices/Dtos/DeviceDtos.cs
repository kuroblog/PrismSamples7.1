
namespace PEF.Modules.RecyclingBin.Devices.Dtos
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
        ReadCard,
        OpenDoor,
        ReturnItem,
        EndRecycle,
        OpenLock
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

    #region ReadCard/FC5-R0-0
    public class DeviceReadCardParameter : IDeviceParameter
    {
        [JsonProperty("CardID")]
        public string CardId { get; set; }

        [JsonProperty("ReadStyle")]
        public string ReadStyle { get; set; }
    }

    public class DeviceReadCardDto : DeviceDto<DeviceReadCardParameter> { }
    #endregion

    #region OpenDoor/TC5-R1-0
    public class DeviceOpenDoorParameter : IDeviceParameter
    {
        [JsonProperty("GroupID")]
        public string GroupId { get; set; }
    }

    public class DeviceOpenDoorDto : DeviceDto<DeviceOpenDoorParameter>
    {
        public DeviceOpenDoorDto(string groupId = "")
        {
            Method = DeviceMethodTypes.OpenDoor.ToString();
            Parameter = new DeviceOpenDoorParameter
            {
                GroupId = string.IsNullOrEmpty(groupId) ? Guid.NewGuid().ToString() : groupId
            };
        }
    }
    #endregion

    #region ReturnItem/FC5-R1-0
    public class DeviceReturnItemParameter : DeviceOpenDoorParameter { }

    public class DeviceReturnItemDto : DeviceDto<DeviceReturnItemParameter>
    {
        public DeviceReturnItemDto(string groupId)
        {
            Method = DeviceMethodTypes.ReturnItem.ToString();
            Parameter = new DeviceReturnItemParameter
            {
                GroupId = groupId
            };
        }
    }
    #endregion

    #region EndRecycle/FC5-R2-0
    public class DeviceEndRecycleParameter : DeviceOpenDoorParameter { }

    public class DeviceEndRecycleDto : DeviceDto<DeviceEndRecycleParameter>
    {
        public DeviceEndRecycleDto(string groupId)
        {
            Method = DeviceMethodTypes.EndRecycle.ToString();
            Parameter = new DeviceEndRecycleParameter
            {
                GroupId = groupId
            };
        }
    }
    #endregion

    #region OpenLock/TC5-R0-0
    public class DeviceOpenLockParameter : IDeviceParameter { }

    public class DeviceOpenLockDto : DeviceDto<DeviceOpenLockParameter>
    {
        public DeviceOpenLockDto()
        {
            Method = DeviceMethodTypes.OpenLock.ToString();
            Parameter = new DeviceOpenLockParameter { };
        }
    }
    #endregion
}
