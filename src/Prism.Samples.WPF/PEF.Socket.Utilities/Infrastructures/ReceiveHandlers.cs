
namespace PEF.Socket.Utilities.Infrastructures
{
    /// <summary>
    /// 接受到消息的事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ReceivedMessageHandler(object sender, ReceivedMessageEventArg e);

    public delegate void DeviceOfflineHandler(object sender, DeviceOfflineEventArg e);

    public delegate void DeviceOnlineHandler(object sender, DeviceOnlineEventArg e);

    public delegate void ReceivedDataHandler(object sender, ReceivedDataEventArg e);

    public delegate void SessionCloseHandler(object sender, SessionCloseEventArg e);

    public delegate void SessionConnectedHandler(object sender, SessionConnectedEventArg e);

    public delegate void SessionReceivedHandler(object sender, SessionReceivedEventArg e);

    //public delegate void WebSockerConnectionStateReportHandler(object sender, WebSockerConnectionStateReportEventArg e);
}
