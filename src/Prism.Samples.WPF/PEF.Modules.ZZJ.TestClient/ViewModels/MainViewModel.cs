
namespace PEF.Modules.ZZJ.TestClient.ViewModels
{
    using Newtonsoft.Json;
    using PEF.Common;
    using PEF.Common.Extensions;
    using PEF.Common.PubSubEvents;
    using PEF.Logger.Infrastructures;
    //using PEF.Modules.Simulator.Devices;
    //using PEF.Modules.Simulator.Models;
    //using PEF.Modules.Simulator.PubSubEvents;
    //using PEF.Socket.Utilities.Infrastructures;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Unity;
    //using SuperSocket.SocketBase;
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;

    public partial class MainViewModel : BindableBase
    {
        private readonly ILogger logger = null;
        private readonly IRegionManager region = null;
        private readonly IEventAggregator eventAggregator = null;
        //private readonly IDeviceProxy deviceProxy = null;
        private readonly ZZJTestClientConfig config = null;

        public MainViewModel(
            ILogger logger,
            IRegionManager region,
            IEventAggregator eventAggregator,
            //IDeviceProxy deviceProxy,
            ZZJTestClientConfig config)
        {
            this.logger = logger;
            this.region = region;
            this.eventAggregator = eventAggregator;
            //this.deviceProxy = deviceProxy;
            this.config = config;

            //this.eventAggregator.GetEvent<SessionCloseEvent>().Subscribe(SessionCloseHandler);
            //this.eventAggregator.GetEvent<SessionConnectedEvent>().Subscribe(SessionConnectedHandler);
            //this.eventAggregator.GetEvent<SessionReceivedEvent>().Subscribe(SessionReceivedHandler);
            //this.eventAggregator.GetEvent<ReceivedDataEvent>().Subscribe(ReceivedDataHandler);
        }

    }
}
