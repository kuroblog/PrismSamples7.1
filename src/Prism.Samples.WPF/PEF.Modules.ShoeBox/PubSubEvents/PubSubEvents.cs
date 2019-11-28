
namespace PEF.Modules.ShoeBox.PubSubEvents
{
    using PEF.Common.PubSubEvents;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;

    /// <summary>
    /// 接收设备Id事件参数类
    /// </summary>
    public class DeviceIdReceiveEvent : PubSubEvent<string> { }

    /// <summary>
    /// 接收卡Id事件参数类
    /// </summary>
    public class CardIdReceiveEvent : PubSubEvent<string> { }

    public class CardReceiveEventParameter
    {
        public string CardId { get; set; }

        public string ReadStyle { get; set; }
    }

    public class CardReceiveEvent : PubSubEvent<CardReceiveEventParameter> { }

    public class DoorOpenReceiveEvent : PubSubEvent<string> { }

    public class DoorCloseReceiveEvent : PubSubEvent<string> { }

    public class AdminOpenDoorTransferEvent : PubSubEvent<string> { }

    public class AdminCloseDoorTransferEvent : PubSubEvent<string> { }

    public class ShoeBoxMessagePopupEvent : PubSubEvent<PopupEventArg<INotification>> { }

    public class ShoeBoxConfirmPopupEvent : PubSubEvent<PopupEventArg<IConfirmation>> { }
}
