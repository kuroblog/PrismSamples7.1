
namespace PEF.Modules.AuthTerminal.PubSubEvents
{
    using Prism.Events;
    using System.Windows;

    /// <summary>
    /// 接收设备Id事件参数类
    /// </summary>
    public class DeviceIdReceiveEvent : PubSubEvent<string> { }

    /// <summary>
    /// 接收卡Id事件参数类
    /// </summary>
    //public class CardIdReceiveEvent : PubSubEvent<string> { }

    public class CardReceiveEventParameter
    {
        public string CardId { get; set; }

        public string ReadStyle { get; set; }
    }

    public class CardReceiveEvent : PubSubEvent<CardReceiveEventParameter> { }

    public class SetAuthUserCommandVisibility : PubSubEvent<Visibility> { }

    ///// <summary>
    ///// 接收装衣完成事件参数类
    ///// </summary>
    //public class LoadCompletedEvent : PubSubEvent { }

    ///// <summary>
    ///// 接收发衣完成事件参数类
    ///// </summary>
    //public class DeliveryCompletedEvent : PubSubEvent<string> { }
}
