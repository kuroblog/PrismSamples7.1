
namespace PEF.Socket.Utilities.Infrastructures
{
    /// <summary>
    /// Socket/WebSocket 代理接口
    /// </summary>
    /// <remarks>
    /// TODO
    /// 1) 统一 Socket 代理类和 WebSocket 代理类的方法接口
    /// </remarks>
    public interface ISocketProxy
    {
        //TcpClient Instance { get; }

        void Initialize(string ipAddress, int port, int bSize = 1024);

        void Connect();

        void Close();

        //void EndReadCallback(IAsyncResult ar);

        void Send(string message);

        event ReceivedMessageHandler OnReceiveMessage;
    }

    //public interface ISocketProxyArgument
    //{
    //    int BufferSize { get; set; }
    //}

    //public interface SocketProxyArgument : ISocketProxyArgument
    //{
    //    string IpAddress { get; set; }

    //    int Port { get; set; }
    //}

    //public interface WebSocketProxyArgument : ISocketProxyArgument
    //{
    //    string Url { get; set; }
    //}
}
