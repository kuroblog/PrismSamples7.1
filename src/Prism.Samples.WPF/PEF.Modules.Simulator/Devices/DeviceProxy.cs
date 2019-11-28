
namespace PEF.Modules.Simulator.Devices
{
    using PEF.Logger.Infrastructures;
    using PEF.Modules.Simulator.PubSubEvents;
    using PEF.Socket.Utilities;
    using PEF.Socket.Utilities.Infrastructures;
    using Prism.Events;
    using SuperSocket.SocketBase;
    using SuperWebSocket;

    /// <summary>
    /// 硬件代理接口
    /// </summary>
    public interface IDeviceProxy
    {
        void Start(string serverName, int port = 8080);

        void Stop();

        void Send(string message, params WebSocketSession[] sessions);

        ServerState State { get; }

        ///// <summary>
        ///// 管理员开门装衣
        ///// </summary>
        ///// <param name="dto"></param>
        //void DeviceDoorControlDtoHandler(DeviceDoorControlDto dto);

        ///// <summary>
        ///// 管理员装衣完成
        ///// </summary>
        ///// <param name="dto"></param>
        //void DeviceLoadingSuccessDtoHandler(DeviceLoadingSuccessDto dto);

        ///// <summary>
        ///// 发衣
        ///// </summary>
        ///// <param name="dto"></param>
        //void DeviceDeliveryDtoHandler(DeviceDeliveryDto dto);
    }

    /// <summary>
    /// 发衣柜硬件代理类
    /// </summary>
    public class DeviceProxy : IDeviceProxy
    {
        private readonly ILogger logger = null;
        private readonly IEventAggregator eventAggregator;
        private readonly WebSocketServerProxy proxy = null;
        private readonly SimulatorConfig config = null;

        public ServerState State => proxy.State;

        public DeviceProxy(
            ILogger logger,
            IEventAggregator eventAggregator,
            WebSocketServerProxy proxy,
            SimulatorConfig config)
        {
            this.logger = logger;
            this.eventAggregator = eventAggregator;
            this.proxy = proxy;
            this.config = config;

            //this.proxy.Initialize(this.config.SocketUrl);
            //this.proxy.OnReceiveMessage += OnReceiveMessageHandler;

            this.proxy.OnReceivedData += OnReceivedDataHandler;
            this.proxy.OnSessionReceived += OnSessionReceivedHandler;
            this.proxy.OnSessionConnected += OnSessionConnectedHandler;
            this.proxy.OnSessionClose += OnSessionCloseHandler;
        }

        private void OnSessionCloseHandler(object sender, SessionCloseEventArg e) => eventAggregator?.GetEvent<SessionCloseEvent>().Publish(e);

        private void OnSessionConnectedHandler(object sender, SessionConnectedEventArg e) => eventAggregator?.GetEvent<SessionConnectedEvent>().Publish(e);

        private void OnSessionReceivedHandler(object sender, SessionReceivedEventArg e) => eventAggregator?.GetEvent<SessionReceivedEvent>().Publish(e);

        private void OnReceivedDataHandler(object sender, ReceivedDataEventArg e) => eventAggregator?.GetEvent<ReceivedDataEvent>().Publish(e);

        public void Start(string serverName, int port = 8080) => proxy?.Start(serverName, port);

        public void Stop() => proxy?.Stop();

        public void Send(string message, params WebSocketSession[] sessions) => proxy?.Send(message, sessions);

        //        protected virtual void OnReceiveMessageHandler(object sender, ReceivedMessageEventArg e)
        //        {
        //            //logger.Trace(e.Message);

        //            if (string.IsNullOrEmpty(e.Message))
        //            {
        //                return;
        //            }

        //            var baseDto = e.Message.GetJsonObject<DeviceBaseDto>();
        //            if (baseDto == null)
        //            {
        //                return;
        //            }

        //            switch (baseDto.MethodType)
        //            {
        //                case DeviceMethodTypes.FrontInfo:
        //                    DeviceFrontInfoDtoHandler(e.Message.GetJsonObject<DeviceFrontInfoDto>());
        //                    break;
        //                case DeviceMethodTypes.ReadCard:
        //                    DeviceReadCardDtoHandler(e.Message.GetJsonObject<DeviceReadCardDto>());
        //                    break;
        //                //case DeviceMethodTypes.LoadingSuccessReturn:
        //                //    DeviceLoadingSuccessReturnDtoHandler(e.Message.GetJsonObject<DeviceLoadingSuccessReturnDto>());
        //                //    break;
        //                //case DeviceMethodTypes.DeliveryReturn:
        //                //    DeviceDeliveryReturnDtoHandler(e.Message.GetJsonObject<DeviceDeliveryReturnDto>());
        //                //    break;
        //                //case DeviceMethodTypes.Unknown:
        //                default:
        //#if DEBUG
        //                    //DeviceReturnItemDtoHandler(e.Message.GetJsonObject<DeviceReturnItemDto>());
        //#endif
        //                    break;
        //            }
        //        }

        //protected virtual void DeviceFrontInfoDtoHandler(DeviceFrontInfoDto dto)
        //{
        //    if (dto == null)
        //    {
        //        return;
        //    }

        //    config.SaveAppSetting(nameof(config.DeviceId), dto.Parameter.DeviceId);

        //    eventAggregator.GetEvent<DeviceIdReceiveEvent>().Publish(dto.Parameter.DeviceId);
        //}

        //protected virtual void DeviceReadCardDtoHandler(DeviceReadCardDto dto)
        //{
        //    if (dto == null)
        //    {
        //        return;
        //    }

        //    eventAggregator.GetEvent<CardIdReceiveEvent>().Publish(dto.Parameter.CardId);
        //}

        //public virtual void DeviceDoorControlDtoHandler(DeviceDoorControlDto dto) => Invoke(dto);

        //public virtual void DeviceLoadingSuccessDtoHandler(DeviceLoadingSuccessDto dto) => Invoke(dto);

        //protected virtual void DeviceLoadingSuccessReturnDtoHandler(DeviceLoadingSuccessReturnDto dto)
        //{
        //    if (dto == null)
        //    {
        //        return;
        //    }

        //    eventAggregator.GetEvent<LoadCompletedEvent>().Publish();
        //}

        //public virtual void DeviceDeliveryDtoHandler(DeviceDeliveryDto dto) => Invoke(dto);

        //protected virtual void DeviceDeliveryReturnDtoHandler(DeviceDeliveryReturnDto dto)
        //{
        //    if (dto == null)
        //    {
        //        return;
        //    }

        //    eventAggregator.GetEvent<DeliveryCompletedEvent>().Publish(dto.Parameter.Code);
        //}

        //protected virtual void Invoke<TParameter>(DeviceDto<TParameter> dto)
        //    where TParameter : IDeviceParameter
        //{
        //    if (dto == null)
        //    {
        //        return;
        //    }

        //    proxy.Send(dto.GetJsonString());
        //}
    }
}
