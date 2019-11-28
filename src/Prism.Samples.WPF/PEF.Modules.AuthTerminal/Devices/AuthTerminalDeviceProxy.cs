
namespace PEF.Modules.AuthTerminal.Devices
{
    using PEF.Common;
    using PEF.Common.Extensions;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.AuthTerminal.Devices.Dtos;
    using PEF.Modules.AuthTerminal.PubSubEvents;
    using PEF.Modules.AuthTerminal.Services;
    using PEF.Modules.AuthTerminal.Services.Dtos;
    using PEF.Modules.AuthTerminal.Views;
    using PEF.Socket.Utilities;
    using PEF.Socket.Utilities.Infrastructures;
    using Prism.Events;
    using Prism.Regions;
    using System;

    /// <summary>
    /// 发衣柜硬件代理接口
    /// </summary>
    public interface IAuthTerminalDeviceProxy
    {
        /// <summary>
        /// 连接
        /// </summary>
        void Connect();

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
    public class AuthTerminalDeviceProxy : IAuthTerminalDeviceProxy
    {
        private readonly ILogger logger = null;
        private readonly IEventAggregator eventAggregator;
        private readonly WebSocketClientProxy proxy = null;
        private readonly IAuthTerminalServiceProxy service = null;
        private readonly AuthTerminalConfig config = null;
        private readonly IRegionManager region = null;

        public AuthTerminalDeviceProxy(
            ILogger logger,
            IEventAggregator eventAggregator,
            WebSocketClientProxy proxy,
            IAuthTerminalServiceProxy service,
            AuthTerminalConfig config,
            IRegionManager region)
        {
            this.logger = logger;
            this.eventAggregator = eventAggregator;
            this.proxy = proxy;
            this.service = service;
            this.config = config;
            this.region = region;

            this.proxy.Initialize(this.config.SocketUrl);
            this.proxy.OnReceiveMessage += OnReceiveMessageHandler;
            this.proxy.OnDeviceOffline += OnDeviceOfflineHandler;
            this.proxy.OnDeviceOnline += OnDeviceOnlineHandler;
        }

        private void OnDeviceOnlineHandler(object sender, DeviceOnlineEventArg e)
        {
            service.SocketStatusReport(new SocketStatusReportRequest { DeviceId = config.DeviceId, Status = 1 });
        }

        private void OnDeviceOfflineHandler(object sender, DeviceOfflineEventArg e)
        {
            service.SocketStatusReport(new SocketStatusReportRequest { DeviceId = config.DeviceId, Status = 2 });

            MainDispatcher.Value.ShowMessage($"设备离线中……{Environment.NewLine}请重启应用程序或与管理员联系。", action: n => region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName));
        }

        public void Connect() => proxy?.Connect();

        protected virtual void OnReceiveMessageHandler(object sender, ReceivedMessageEventArg e)
        {
            //logger.Trace(e.Message);

            if (string.IsNullOrEmpty(e.Message))
            {
                return;
            }

            var baseDto = e.Message.GetJsonObject<DeviceBaseDto>();
            if (baseDto == null)
            {
                return;
            }

            switch (baseDto.MethodType)
            {
                case DeviceMethodTypes.FrontInfo:
                    DeviceFrontInfoDtoHandler(e.Message.GetJsonObject<DeviceFrontInfoDto>());
                    break;
                case DeviceMethodTypes.ReadCard:
                    DeviceReadCardDtoHandler(e.Message.GetJsonObject<DeviceReadCardDto>());
                    break;
                //case DeviceMethodTypes.LoadingSuccessReturn:
                //    DeviceLoadingSuccessReturnDtoHandler(e.Message.GetJsonObject<DeviceLoadingSuccessReturnDto>());
                //    break;
                //case DeviceMethodTypes.DeliveryReturn:
                //    DeviceDeliveryReturnDtoHandler(e.Message.GetJsonObject<DeviceDeliveryReturnDto>());
                //    break;
                //case DeviceMethodTypes.Unknown:
                default:
#if DEBUG
                    //DeviceReturnItemDtoHandler(e.Message.GetJsonObject<DeviceReturnItemDto>());
#endif
                    break;
            }
        }

        protected virtual void DeviceFrontInfoDtoHandler(DeviceFrontInfoDto dto)
        {
            if (dto == null)
            {
                return;
            }

            config.SaveAppSetting(nameof(config.DeviceId), dto.Parameter.DeviceId);

            eventAggregator.GetEvent<DeviceIdReceiveEvent>().Publish(dto.Parameter.DeviceId);
        }

        protected virtual void DeviceReadCardDtoHandler(DeviceReadCardDto dto)
        {
            if (dto == null)
            {
                return;
            }

            eventAggregator.GetEvent<CardReceiveEvent>().Publish(new CardReceiveEventParameter
            {
                CardId = dto.Parameter.CardId,
                ReadStyle = dto.Parameter.ReadStyle
            });
        }

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
