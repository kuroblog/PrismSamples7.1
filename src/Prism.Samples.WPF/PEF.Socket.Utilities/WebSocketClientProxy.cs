
namespace PEF.Socket.Utilities
{
    using PEF.Common.Extensions;
    using PEF.Logger;
    using PEF.Logger.Infrastructures;
    using PEF.Socket.Utilities.Infrastructures;
    using SuperSocket.ClientEngine;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Text;
    using System.Threading.Tasks;
    using WebSocket4Net;

    public class WebSocketClientProxy : IDisposable
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
        ~WebSocketClientProxy()
        {
            Dispose(false);
        }
        #endregion

        private readonly ILogger logger = null;
        private WebSocket instance;
        private byte[] rBuffer;
        private Uri address;
        //private CancellationTokenSource cts;

        public event ReceivedMessageHandler OnReceiveMessage;

        private void ReceiveMessage(string message) => OnReceiveMessage?.Invoke(this, new ReceivedMessageEventArg(message));

        public event DeviceOfflineHandler OnDeviceOffline;

        private void DeviceOffline() => OnDeviceOffline?.Invoke(this, new DeviceOfflineEventArg());

        public event DeviceOnlineHandler OnDeviceOnline;

        private void DeviceOnline() => OnDeviceOnline?.Invoke(this, new DeviceOnlineEventArg());

        //public event WebSockerConnectionStateReportHandler OnWebSockerConnectionStateReport;

        //private void WebSockerConnectionStateReport(bool isConnected) => OnWebSockerConnectionStateReport?.Invoke(this, new WebSockerConnectionStateReportEventArg(isConnected));

        public WebSocketClientProxy(ILogger logger)
        {
            this.logger = logger;
        }

        public void Initialize(string url, int bSize = 1024)
        {
            rBuffer = new byte[bSize];
            address = new Uri(url);
            //cts = new CancellationTokenSource();
            //instance = new WebSocket(address.AbsoluteUri);
        }

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
        public void Connect()
        {
            //DoRetry(DoConnect);

            try
            {
                if (instance == null)
                {
                    instance = new WebSocket(address.AbsoluteUri);

                    instance.Opened += WebSocketOpenedHandler;
                    instance.Error += WebSocketErrorHandler;
                    instance.Closed += WebSocketClosedHandler;
                    instance.MessageReceived += WebSocketMessageReceivedHandler;
                }

                instance.Open();
            }
            catch (Exception ex)
            {
                //DumpError(ex);
                logger.Error(ex.GetJsonString());
            }
        }

        private ConcurrentDictionary<int, int> errorCodes = new ConcurrentDictionary<int, int>();

        private void WebSocketOpenedHandler(object sender, EventArgs e)
        {
            //Dump($"连接到 {address}; 状态: {instance.State}");
            logger.Info(new { method = MethodBase.GetCurrentMethod().Name, address, state = instance.State }.GetJsonString());

            //WebSockerConnectionStateReport(true);

            DeviceOnline();
        }

        private void WebSocketErrorHandler(object sender, ErrorEventArgs e)
        {
            //Dump($"接收到 {address}; 错误: {e.Exception.GetJsonString()}");
            logger.Error(new { method = MethodBase.GetCurrentMethod().Name, address, state = instance.State, error = e.Exception }.GetJsonString());

            if (e.Exception != null)
            {
                if (e.Exception is SocketException ex)
                {
                    if (errorCodes.TryGetValue(ex.NativeErrorCode, out int counter))
                    {
                        errorCodes.TryUpdate(ex.NativeErrorCode, counter + 1, counter);
                    }
                    else
                    {
                        errorCodes.TryAdd(ex.NativeErrorCode, 0);
                    }
                }
            }
        }

        private void WebSocketClosedHandler(object sender, EventArgs e)
        {
            //Dump($"关闭到 {address}; 状态: {instance.State}");
            logger.Info(new { method = MethodBase.GetCurrentMethod().Name, address, state = instance.State }.GetJsonString());

            errorCodes.TryGetValue(10061, out int counter);

            if (counter == 0 || counter < 3)
            {
                if (counter == 0)
                {
                    //WebSockerConnectionStateReport(false);
                    DeviceOffline();
                }

                //DeviceOffline();
            }

            Task.Delay(counter == 0 ? 1000 : counter * counter * 1000).Wait();

            Connect();
        }

        private void WebSocketMessageReceivedHandler(object sender, MessageReceivedEventArgs e)
        {
            logger.Info(e.Message);

            ReceiveMessage(e.Message);
        }

        public void Close()
        {
            try
            {
                if (instance == null)
                {
                    return;
                }

                //Dump($"当前连接状态: {instance.State}");
                //Dump($"关闭当前连接");

                logger.Info(new { method = MethodBase.GetCurrentMethod().Name, address, state = instance.State }.GetJsonString());

                Task.Delay(10).Wait();

                instance.Close();
            }
            catch (Exception ex)
            {
                //DumpError(ex);
                logger.Error(ex.GetJsonString());

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

            //Dump(string.Join(",", buffer), $"发送数据流到 {address}: {rBuffer.GetJsonString()} => {message}");
            logger.Info(new { method = MethodBase.GetCurrentMethod().Name, address, state = instance.State, message }.GetJsonString());

            instance.Send(message);

            //DoRetry(() => instance.Send(message));

            //Dump($"发送成功");
        }

        public WebSocketState State => instance.State;
    }
}
