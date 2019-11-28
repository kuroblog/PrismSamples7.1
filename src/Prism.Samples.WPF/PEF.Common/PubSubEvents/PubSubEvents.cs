
namespace PEF.Common.PubSubEvents
{
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using System;

    /// <summary>
    /// 弹出框事件的参数类
    /// </summary>
    /// <typeparam name="ICallbackArg"></typeparam>
    public class PopupEventArg<ICallbackArg> where ICallbackArg : INotification
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public Action<ICallbackArg> Callback { get; set; }
    }

    /// <summary>
    /// 默认消息框的参数类
    /// </summary>
    public class MessagePopupEvent : PubSubEvent<PopupEventArg<INotification>> { }

    /// <summary>
    /// 默认确认框的参数类
    /// </summary>
    public class ConfirmPopupEvent : PubSubEvent<PopupEventArg<IConfirmation>> { }

    /// <summary>
    /// 自定义消息框的参数类
    /// </summary>
    public class CustomMessagePopupEvent : MessagePopupEvent { }

    /// <summary>
    /// 自定义弹出框的参数类
    /// </summary>
    public class CustomPopupEvent : PubSubEvent<PopupEventArg<ICustomPopupNotification<string>>> { }

    /// <summary>
    /// 主框架的透明度设置的参数类
    /// </summary>
    public class ShellWindowOpacityEvent : PubSubEvent<double> { }

    public class MainNotificationPopupEvent : PubSubEvent<PopupEventArg<INotification>> { }
}
