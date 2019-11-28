
namespace PEF.Modules.ZZJ.TestClient.ViewModels
{
    using Newtonsoft.Json;
    using PEF.Common;
    using PEF.Common.Extensions;
    using PEF.Common.PubSubEvents;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.ZZJ.TestClient.Models;
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
            var scriptItemsJson = config.ReadAllText(Assembly.GetExecutingAssembly().Location.Replace("dll", "json"));

            var items = scriptItemsJson.GetJsonObject<ScriptItem[]>();
            if (items != null && items.Any())
            {
                ScriptItems = new ObservableCollection<ScriptItem>(items);
                if (SelectedScriptItem == null)
                {
                    SelectedScriptItem = ScriptItems?.FirstOrDefault();
                }

                SelectedServerChangedCommand.Execute(SelectedScriptItem);
            }
        });

        public DelegateCommand ReloadCommand => new DelegateCommand(() => { });

        public DelegateCommand DebugCommand => new DelegateCommand(() => { });

        public DelegateCommand HelperCommand => new DelegateCommand(() => MainDispatcher.Value.ShowMessage<MessagePopupEvent>(config.Version));

        private ObservableCollection<ScriptItem> scriptItems = new ObservableCollection<ScriptItem>();

        public ObservableCollection<ScriptItem> ScriptItems
        {
            get => scriptItems;
            set => SetProperty(ref scriptItems, value);
        }

        private ScriptItem selectedScriptItem;

        public ScriptItem SelectedScriptItem
        {
            get => selectedScriptItem;
            set => SetProperty(ref selectedScriptItem, value);
        }

        private string scriptItemPath;

        public string ScriptItemPath
        {
            get => scriptItemPath;
            set => SetProperty(ref scriptItemPath, value);
        }

        public DelegateCommand<ScriptItem> SelectedServerChangedCommand => new DelegateCommand<ScriptItem>(item =>
        {
            if (item == null)
            {
                return;
            }

            ScriptItemPath = item.ScriptPath;

            //if (deviceProxy.State == ServerState.Running)
            //{
            //    StopCommand.Execute();
            //}

            //ScriptItems.Clear();

            //var scriptItemPath = Path.Combine(Assembly.GetExecutingAssembly().Location.Replace("dll", "Scripts"), $"{item.Key}.json");
            //if (File.Exists(scriptPath))
            //{
            //    var scriptJson = config.ReadAllText(scriptPath);
            //    if (!string.IsNullOrEmpty(scriptJson))
            //    {
            //        var items = JsonConvert.DeserializeObject<ScriptItemParser[]>(scriptJson);
            //        if (items != null && items.Any())
            //        {
            //            ScriptItems = new ObservableCollection<ScriptItemParser>(items);
            //            if (SelectedScript == null)
            //            {
            //                SelectedScript = items?.FirstOrDefault();
            //            }
            //        }

            //        //scriptKeyInfos?.ToList()?.ForEach(p =>
            //        //{
            //        //    switch (p.MethodType)
            //        //    {
            //        //        case ScriptType.Send:
            //        //            p.ConvertTo<SendScriptItemData>();
            //        //            break;
            //        //        case ScriptType.Unknown:
            //        //        default: break;
            //        //    }

            //        //    //var instance =;
            //        //});
            //    }
            //}
        });
    }
}
