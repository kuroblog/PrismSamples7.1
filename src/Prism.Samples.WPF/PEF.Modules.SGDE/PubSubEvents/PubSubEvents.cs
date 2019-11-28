
namespace PEF.Modules.SGDE.PubSubEvents
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
    /// 接收装衣完成事件参数类
    /// </summary>
    public class LoadCompletedEvent : PubSubEvent { }

    /// <summary>
    /// 接收发衣完成事件参数类
    /// </summary>
    public class DeliveryCompletedEvent : PubSubEvent<string> { }

    public class CardReceiveEventParameter
    {
        public string CardId { get; set; }

        public string ReadStyle { get; set; }
    }

    public class CardReceiveEvent : PubSubEvent<CardReceiveEventParameter> { }
}
