
namespace PEF.Modules.RecyclingBin.PubSubEvents
{
    using Prism.Events;

    /// <summary>
    /// 接收设备Id事件参数类
    /// </summary>
    public class DeviceIdReceiveEvent : PubSubEvent<string> { }

    /// <summary>
    /// 接收卡Id事件参数类
    /// </summary>
    public class CardIdReceiveEvent : PubSubEvent<string> { }

    /// <summary>
    /// 接收物品回收事件参数类
    /// </summary>
    public class ItemReveiveEvent : PubSubEvent<string> { }

    /// <summary>
    /// 接收关桶事件参数类
    /// </summary>
    public class CloseDoorEvent : PubSubEvent<string> { }

    public class CardReceiveEventParameter
    {
        public string CardId { get; set; }

        public string ReadStyle { get; set; }
    }

    public class CardReceiveEvent : PubSubEvent<CardReceiveEventParameter> { }
}
