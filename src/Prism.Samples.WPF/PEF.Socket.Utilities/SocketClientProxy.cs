
namespace PEF.Socket.Utilities
{
    using PEF.Common.Extensions;
    using PEF.Logger.Infrastructures;
    using PEF.Socket.Utilities.Infrastructures;
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.ExceptionServices;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Socket 代理类
    /// </summary>
    public class SocketClientProxy : ISocketProxy, IDisposable
    {
        private readonly ILogger logger = null;

        private TcpClient instance;

        private byte[] rBuffer;
        private IPAddress ipAddress;
        private int port;
        private IPEndPoint ip;
        private NetworkStream stream;

        public event ReceivedMessageHandler OnReceiveMessage;

        private void ReceiveMessage(string message) => OnReceiveMessage?.Invoke(this, new ReceivedMessageEventArg(message));

        public SocketClientProxy(ILogger logger)
        {
            this.logger = logger;
        }

        public void Initialize(string ipAddress, int port, int bSize = 1024)
        {
            rBuffer = new byte[bSize];
            this.ipAddress = IPAddress.Parse(ipAddress);
            this.port = port;
            ip = new IPEndPoint(this.ipAddress, port);

            instance = new TcpClient();
        }

        private void Dump<TArg>(TArg arg, string message = null)
        {
            logger.Debug(arg);

            if (!string.IsNullOrEmpty(message))
            {
                ReceiveMessage(message);
            }
            else if (arg is string)
            {
                ReceiveMessage(arg.ToString());
            }
        }

        private void DumpException(Exception ex)
        {
            logger.Error(ex);

            ReceiveMessage(ex.GetFullMessage());
        }

        private void DumpError(Exception error)
        {
            logger.Fatal(error);

            ReceiveMessage(error.GetFullMessage());
        }

        public void Connect()
        {
            try
            {
                if (instance == null || instance.Client == null)
                {
                    instance = new TcpClient();
                }

                instance.Connect(ip);

                Dump($"连接到 {instance.Client.RemoteEndPoint}; 状态: {instance.Client.Connected}");

                stream = new NetworkStream(instance.Client, true);

                Dump($"开始在 {instance.Client.LocalEndPoint} 接收 {instance.Client.RemoteEndPoint} 的数据流");

                stream.BeginRead(rBuffer, 0, rBuffer.Length, new AsyncCallback(EndReadCallback), instance);
            }
            catch (SocketException exSocket)
            {
                DumpException(exSocket);
            }
            catch (InvalidOperationException exIo)
            {
                DumpException(exIo);
            }
            catch (Exception ex)
            {
                DumpError(ex);

                // throw unknown error
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
        }

        public void Close()
        {
            try
            {
                if (instance == null || instance.Client == null)
                {
                    return;
                }

                Dump($"当前连接状态: {instance.Client.Connected}");

                instance.Client.Shutdown(SocketShutdown.Both);
                instance.Client.Close();

                Dump($"关闭当前连接");

                Task.Delay(10).Wait();

                instance.Close();
            }
            catch (Exception ex)
            {
                DumpError(ex);

                // throw unknown error
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
        }

        private void CloseStream()
        {
            stream.Close();
            stream.Dispose();
        }

        protected virtual void EndReadCallback(IAsyncResult ar)
        {
            try
            {
                if (!(ar.AsyncState is TcpClient instance))
                {
                    return;
                }

                if (instance.Client == null)
                {
                    CloseStream();

                    return;
                }

                var offset = stream.EndRead(ar);

                var msg = Encoding.UTF8.GetString(rBuffer, 0, offset);

                var data = new byte[offset];
                Array.Copy(rBuffer, data, offset);

                Dump(string.Join(",", data), $"从 {instance.Client.RemoteEndPoint} 接收到数据流: {rBuffer.GetJsonString()} => {msg}");

                stream.BeginRead(rBuffer, 0, rBuffer.Length, new AsyncCallback(EndReadCallback), instance);
            }
            catch (IOException exIo)
            {
                DumpException(exIo);

                Close();
            }
            catch (Exception ex)
            {
                DumpError(ex);

                // throw unknown error
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
        }

        public void Send(string message)
        {
            if (instance == null || instance.Client == null)
            {
                return;
            }

            if (!instance.Connected)
            {
                return;
            }

            if (stream == null)
            {
                stream = instance.GetStream();
            }

            var buffer = Encoding.UTF8.GetBytes(message);

            Dump(string.Join(",", buffer), $"发送数据流到 {instance.Client.RemoteEndPoint}: {rBuffer.GetJsonString()} => {message}");

            stream.Write(buffer, 0, buffer.Length);

            Dump($"发送成功");
        }

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
                    CloseStream();

                    if (instance != null)
                    {
                        instance.Dispose();
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
        ~SocketClientProxy()
        {
            Dispose(false);
        }
        #endregion
    }
}
