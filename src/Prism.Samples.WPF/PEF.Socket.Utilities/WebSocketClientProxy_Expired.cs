
namespace PEF.Socket.Utilities
{
    using PEF.Common.Extensions;
    using PEF.Logger;
    using PEF.Logger.Infrastructures;
    using PEF.Socket.Utilities.Infrastructures;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Net.WebSockets;
    using System.Runtime.ExceptionServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// WebSocket 代理类(废弃，在 Windows 7 系统上无法使用 ClientWebSocket 对象)
    /// </summary>
    /// <remarks>
    /// TODO
    /// 1) 重构，以实现 <see cref="ISocketProxy"/> 代理接口
    /// </remarks>
    public class WebSocketClientProxy_Expired : IDisposable
    {
        private readonly ILogger logger = null;
        private ClientWebSocket instance;
        private byte[] rBuffer;
        private Uri address;
        private CancellationTokenSource cts;

        public event ReceivedMessageHandler OnReceiveMessage;

        private void ReceiveMessage(string message) => OnReceiveMessage?.Invoke(this, new ReceivedMessageEventArg(message));

        public WebSocketClientProxy_Expired(ILogger logger)
        {
            this.logger = logger;
        }

        public void Initialize(string url, int bSize = 1024)
        {
            rBuffer = new byte[bSize];
            address = new Uri(url);
            cts = new CancellationTokenSource();
            instance = new ClientWebSocket();
        }

        private void Dump<TArg>(TArg arg, string message = null) => logger.Debug(arg);

        private void DumpException(Exception ex) => logger.Error(ex);

        private void DumpError(Exception error) => logger.Fatal(error);

        // TODO: 重连、超时处理
        public void Connect()
        {
            try
            {
                if (instance == null)
                {
                    instance = new ClientWebSocket();
                }

                instance.ConnectAsync(address, cts.Token).Wait();

                Dump($"连接到 {address}; 状态: {instance.State}");

                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        var rAsBuffer = new ArraySegment<byte>(rBuffer);
                        var result = instance.ReceiveAsync(rAsBuffer, cts.Token).Result;
                        var data = rAsBuffer.Skip(rAsBuffer.Offset).Take(result.Count).ToArray();

                        var message = Encoding.UTF8.GetString(data);

                        ReceiveMessage(message);
                    }
                }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
            catch (AggregateException aex)
            {
                if (aex.InnerException is WebSocketException wex)
                {
                    if (wex != null) { }
                }

                DumpError(aex);
            }
            catch (Exception ex)
            {
                DumpError(ex);

                // throw unknown error
                //ExceptionDispatchInfo.Capture(ex).Throw();
            }
        }

        public void Close()
        {
            try
            {
                if (instance == null)
                {
                    return;
                }

                Dump($"当前连接状态: {instance.State}");

                Dump($"关闭当前连接");

                Task.Delay(10).Wait();

                instance.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cts.Token);
            }
            catch (Exception ex)
            {
                DumpError(ex);

                // throw unknown error
                //ExceptionDispatchInfo.Capture(ex).Throw();
            }
        }

        public void Send(string message)
        {
            if (instance == null)
            {
                return;
            }

            if (instance.State != WebSocketState.Open)
            {
                return;
            }

            var buffer = Encoding.UTF8.GetBytes(message);
            var rAsBuffer = new ArraySegment<byte>(rBuffer);

            Dump(string.Join(",", buffer), $"发送数据流到 {address}: {rBuffer.GetJsonString()} => {message}");

            instance.SendAsync(rAsBuffer, WebSocketMessageType.Text, true, cts.Token).Wait();

            Dump($"发送成功");
        }

        public WebSocketState State => instance.State;

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

                    if (cts != null)
                    {
                        cts.Dispose();
                    }

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
        ~WebSocketClientProxy_Expired()
        {
            Dispose(false);
        }
        #endregion
    }
}