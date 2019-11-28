
namespace PEF.Socket.Utilities.Infrastructures
{
    using SuperSocket.SocketBase;
    using SuperWebSocket;
    using System;

    /// <summary>
    /// 接受到消息的事件参数类
    /// </summary>
    public class ReceivedMessageEventArg : EventArgs
    {
        public string Message { get; private set; }

        public ReceivedMessageEventArg(string message)
        {
            Message = message;
        }
    }

    public class DeviceOfflineEventArg : EventArgs { }

    public class DeviceOnlineEventArg : DeviceOfflineEventArg { }

    public class ReceivedDataEventArg : EventArgs
    {
        public WebSocketSession Session { get; private set; }

        public byte[] Data { get; private set; }

        public ReceivedDataEventArg(WebSocketSession session, byte[] data)
        {
            Session = session;
            Data = data;
        }
    }

    public class SessionCloseEventArg : EventArgs
    {
        public WebSocketSession Session { get; private set; }

        public CloseReason Reason { get; private set; }

        public SessionCloseEventArg(WebSocketSession session, CloseReason reason)
        {
            Session = session;
            Reason = reason;
        }
    }

    public class SessionConnectedEventArg : EventArgs
    {
        public WebSocketSession Session { get; private set; }

        public SessionConnectedEventArg(WebSocketSession session)
        {
            Session = session;
        }
    }

    public class SessionReceivedEventArg : EventArgs
    {
        public WebSocketSession Session { get; private set; }

        public string Message { get; private set; }

        public SessionReceivedEventArg(WebSocketSession session, string message)
        {
            Session = session;
            Message = message;
        }
    }

    //public class WebSockerConnectionStateReportEventArg : EventArgs
    //{
    //    public bool IsConnected { get; private set; }

    //    public WebSockerConnectionStateReportEventArg(bool isConnected)
    //    {
    //        IsConnected = isConnected;
    //    }
    //}
}
