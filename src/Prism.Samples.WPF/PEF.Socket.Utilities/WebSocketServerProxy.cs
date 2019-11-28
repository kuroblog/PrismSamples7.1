
namespace PEF.Socket.Utilities
{
    using PEF.Common.Extensions;
    using PEF.Logger.Infrastructures;
    using PEF.Socket.Utilities.Infrastructures;
    using SuperSocket.SocketBase;
    using SuperSocket.SocketBase.Config;
    using SuperSocket.SocketEngine;
    using SuperWebSocket;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class WebSocketServerProxy : IDisposable
    {
        #region IDisposable
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                // Dispose of resources held by this instance.

                // Violates rule: DisposableFieldsShouldBeDisposed.
                // Should call aFieldOfADisposableType.Dispose();

                disposed = true;
                // Suppress finalization of this disposed instance.
                if (disposing)
                {
                    //CloseStream();

                    if (instance != null)
                    {
                        instance.Dispose();
                    }

                    //if (cts != null)
                    //{
                    //    cts.Dispose();
                    //}

                    GC.SuppressFinalize(this);
                }
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                // Dispose of resources held by this instance.
                Dispose(true);
            }
        }

        // Disposable types implement a finalizer.
        ~WebSocketServerProxy()
        {
            Dispose(false);
        }
        #endregion

        private readonly ILogger logger = null;
        private WebSocketServer instance;
        //private byte[] rBuffer;
        //private Uri address;
        //private CancellationTokenSource cts;

        //private string serverName;
        //private int port;

        public event ReceivedDataHandler OnReceivedData;

        private void ReceivedData(WebSocketSession session, byte[] data) => OnReceivedData?.Invoke(this, new ReceivedDataEventArg(session, data));

        public event SessionConnectedHandler OnSessionConnected;

        private void SessionConnected(WebSocketSession session) => OnSessionConnected?.Invoke(this, new SessionConnectedEventArg(session));

        public event SessionCloseHandler OnSessionClose;

        private void SessionClose(WebSocketSession session, CloseReason reason) => OnSessionClose?.Invoke(this, new SessionCloseEventArg(session, reason));

        public event SessionReceivedHandler OnSessionReceived;

        private void SessionReceived(WebSocketSession session, string message) => OnSessionReceived?.Invoke(this, new SessionReceivedEventArg(session, message));

        //public event ReceivedMessageHandler OnReceiveMessage;

        //private void ReceiveMessage(string message) => OnReceiveMessage?.Invoke(this, new ReceivedMessageEventArg(message));

        public WebSocketServerProxy(ILogger logger)
        {
            this.logger = logger;
        }

        //public void Initialize(string serverName, int port = 8080)
        //{
        //    this.serverName = serverName;
        //    this.port = port;
        //    //rBuffer = new byte[bSize];
        //    //address = new Uri(url);
        //    //cts = new CancellationTokenSource();
        //    //instance = new WebSocket(address.AbsoluteUri);
        //}

        //private void Dump<TArg>(TArg arg, string message = null) => logger.Debug(arg);

        //private void DumpException(Exception ex) => logger.Error(ex);

        //private void DumpError(Exception error) => logger.Fatal(error.GetJsonString());

        #region retry
        //private bool IsRetryException(Exception ex) => ex is RetryException;

        //private int retryCount = 3;
        //private readonly TimeSpan delay = TimeSpan.FromSeconds(1);

        //private void DoRetry(Action action)
        //{
        //    int currentRetry = 0;

        //    for (; ; )
        //    {
        //        try
        //        {
        //            action.Invoke();

        //            break;
        //        }
        //        catch (Exception ex)
        //        {
        //            DumpError(ex);

        //            currentRetry++;

        //            if (currentRetry > retryCount || !IsRetryException(ex))
        //            {
        //                ExceptionDispatchInfo.Capture(ex).Throw();
        //            }
        //        }

        //        Task.Delay(delay).Wait();
        //    }
        //}

        //private void DoConnect()
        //{
        //    if (instance == null)
        //    {
        //        instance = new WebSocket(address.AbsoluteUri);

        //        instance.Opened += WebSocketOpenedHandler;
        //        instance.Error += WebSocketErrorHandler;
        //        instance.Closed += WebSocketClosedHandler;
        //        instance.MessageReceived += WebSocketMessageReceivedHandler;
        //    }

        //    instance.Open();
        //}
        #endregion

        // TODO: 重连、超时处理
        public void Start(string serverName, int port = 8080)
        {
            //DoRetry(DoConnect);

            try
            {
                if (instance == null)
                {
                    instance = new WebSocketServer();

                    var config = new ServerConfig
                    {
                        Name = serverName,
                        MaxConnectionNumber = 10000,    //最大允许的客户端连接数目，默认为100。
                        Mode = SocketMode.Tcp,
                        Port = port,                    //服务器监听的端口。
                        ClearIdleSession = false,       //true或者false， 是否清除空闲会话，默认为false。
                        ClearIdleSessionInterval = 120, //清除空闲会话的时间间隔，默认为120，单位为秒。
                        ListenBacklog = 10,
                        ReceiveBufferSize = 64 * 1024,  //用于接收数据的缓冲区大小，默认为2048。
                        SendBufferSize = 64 * 1024,     //用户发送数据的缓冲区大小，默认为2048。
                        KeepAliveInterval = 1,          //keep alive消息发送时间间隔。单位为秒。
                        KeepAliveTime = 60,             //keep alive失败重试的时间间隔。单位为秒。
                        SyncSend = false
                    };

                    SocketServerFactory socketServerFactory = null;

                    instance.Setup(new RootConfig(), config, socketServerFactory);

                    instance.NewMessageReceived += NewMessageReceivedHandler;
                    instance.NewSessionConnected += NewSessionConnectedHandler;
                    instance.SessionClosed += SessionClosedHandler;
                    instance.NewDataReceived += NewDataReceivedHandler;
                }

                instance.Start();
            }
            catch (Exception ex)
            {
                //DumpError(ex);
                logger.Error(ex.GetJsonString());
            }
        }

        private void NewDataReceivedHandler(WebSocketSession session, byte[] value)
        {
            logger.Info(new
            {
                method = MethodBase.GetCurrentMethod().Name,
                session = session?.RemoteEndPoint?.ToString(),
                state = session.AppServer.State,
                data = value
            }.GetJsonString());

            ReceivedData(session, value);
        }

        private void SessionClosedHandler(WebSocketSession session, CloseReason value)
        {
            logger.Info(new
            {
                method = MethodBase.GetCurrentMethod().Name,
                session = session?.RemoteEndPoint?.ToString(),
                state = session.AppServer.State,
                remark = value.ToString()
            }.GetJsonString());

            SessionClose(session, value);
        }

        private void NewSessionConnectedHandler(WebSocketSession session)
        {
            logger.Info(new
            {
                method = MethodBase.GetCurrentMethod().Name,
                session = session?.RemoteEndPoint?.ToString(),
                state = session.AppServer.State
            }.GetJsonString());

            SessionConnected(session);
        }

        private void NewMessageReceivedHandler(WebSocketSession session, string value)
        {
            logger.Info(new
            {
                method = MethodBase.GetCurrentMethod().Name,
                session = session?.RemoteEndPoint?.ToString(),
                state = session.AppServer.State,
                message = value
            }.GetJsonString());

            SessionReceived(session, value);
        }

        public void Stop()
        {
            try
            {
                if (instance == null)
                {
                    return;
                }

                //Dump($"当前连接状态: {instance.State}");
                //Dump($"关闭当前连接");

                logger.Info(new { method = MethodBase.GetCurrentMethod().Name, todo = "server path", state = instance.State }.GetJsonString());

                Task.Delay(10).Wait();

                instance.Stop();
            }
            catch (Exception ex)
            {
                //DumpError(ex);
                logger.Error(ex.GetJsonString());

                // throw unknown error
                //ExceptionDispatchInfo.Capture(ex).Throw();
            }
        }

        public void Send(string message, params WebSocketSession[] sessions)
        {
            if (instance == null)
            {
                return;
            }

            if (instance.State != ServerState.Running)
            {
                return;
            }

            //var buffer = Encoding.UTF8.GetBytes(message);
            //var rAsBuffer = new ArraySegment<byte>(rBuffer);
            //logger.Info(new { method = MethodBase.GetCurrentMethod().Name, address, state = instance.State, message }.GetJsonString());

            //instance.Send(message);

            sessions?.ToList()?.ForEach(p =>
            {
                if (p.AppServer != null)
                {
                    if (p.AppServer.State == ServerState.Running)
                    {
                        p.Send(message);

                        logger.Info(new { method = MethodBase.GetCurrentMethod().Name, message, state = p.AppServer.State, target = p.RemoteEndPoint.ToString() }.GetJsonString());
                    }
                }
            });

            //DoRetry(() => instance.Send(message));

            //Dump($"发送成功");
        }

        public ServerState State => instance == null ? ServerState.NotStarted : instance.State;
    }
}
