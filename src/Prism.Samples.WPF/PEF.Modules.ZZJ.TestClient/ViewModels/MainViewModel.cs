
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

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            //Task.Factory.StartNew(() =>
            //{
            //    for (; ; )
            //    {
            //        MainDispatcherInvoke(() => Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            //        Task.Delay(TimeSpan.FromSeconds(1)).Wait();
            //    }
            //});

            //State = deviceProxy.State.ToString();
            //Sessions.Clear();

            //var serverJson = config.ReadAllText(Assembly.GetExecutingAssembly().Location.Replace("dll", "json"));

            //var items = serverJson.GetJsonObject<ServerItem[]>();
            //if (items != null && items.Any())
            //{
            //    ServerItems = new ObservableCollection<ServerItem>(items);
            //    if (SelectedServer == null)
            //    {
            //        SelectedServer = ServerItems?.FirstOrDefault();
            //    }

            //    SelectedServerChangedCommand.Execute(SelectedServer);
            //}
        });

        public DelegateCommand ResetCommand => new DelegateCommand(() => { });

        public DelegateCommand DebugCommand => new DelegateCommand(() => { });

        public DelegateCommand HelperCommand => new DelegateCommand(() => MainDispatcher.Value.ShowMessage<INotification>(config.Version));
    }
}
