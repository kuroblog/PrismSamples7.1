
namespace PEF.Modules.Simulator.ViewModels
{
    using Newtonsoft.Json;
    using PEF.Common;
    using PEF.Common.Extensions;
    using PEF.Common.PubSubEvents;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.Simulator.Devices;
    using PEF.Modules.Simulator.Models;
    using PEF.Modules.Simulator.PubSubEvents;
    using PEF.Socket.Utilities.Infrastructures;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Unity;
    using SuperSocket.SocketBase;
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
        private readonly IDeviceProxy deviceProxy = null;
        private readonly SimulatorConfig config = null;

        public MainViewModel(
            ILogger logger,
            IRegionManager region,
            IEventAggregator eventAggregator,
            IDeviceProxy deviceProxy,
            SimulatorConfig config)
        {
            this.logger = logger;
            this.region = region;
            this.eventAggregator = eventAggregator;
            this.deviceProxy = deviceProxy;
            this.config = config;

            this.eventAggregator.GetEvent<SessionCloseEvent>().Subscribe(SessionCloseHandler);
            this.eventAggregator.GetEvent<SessionConnectedEvent>().Subscribe(SessionConnectedHandler);
            this.eventAggregator.GetEvent<SessionReceivedEvent>().Subscribe(SessionReceivedHandler);
            this.eventAggregator.GetEvent<ReceivedDataEvent>().Subscribe(ReceivedDataHandler);
        }

        private string state;

        public string State
        {
            get => state;
            set => SetProperty(ref state, value);
        }

        private string logs;

        public string Logs
        {
            get => logs;
            set => SetProperty(ref logs, value);
        }

        private ObservableCollection<SessionItem> sessions = new ObservableCollection<SessionItem>();

        public ObservableCollection<SessionItem> Sessions
        {
            get => sessions;
            set => SetProperty(ref sessions, value);
        }

        private SessionItem selectedSession;

        public SessionItem SelectedSession
        {
            get => selectedSession;
            set => SetProperty(ref selectedSession, value);
        }

        private string logHeader = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}: {Environment.NewLine}  -> ";

        #region thread sync test
        //private void SessionCloseHandler(SessionCloseEventArg arg)
        //{
        //    MainDispatcherInvoke(() =>
        //    {
        //        var session = Sessions?.FirstOrDefault(p => p.Remote == arg.Session.RemoteEndPoint.ToString());
        //        if (session != null)
        //        {
        //            Sessions.Remove(session);
        //        }

        //        Logs += $"{logHeader}device offline => {arg.Session.RemoteEndPoint}{Environment.NewLine}";
        //    });
        //}

        //private void SessionConnectedHandler(SessionConnectedEventArg arg)
        //{
        //    MainDispatcherInvoke(() =>
        //    {
        //        Sessions.Add(new SessionItem { Remote = arg.Session.RemoteEndPoint.ToString(), Session = arg.Session });

        //        if (SelectedSession == null)
        //        {
        //            SelectedSession = Sessions.FirstOrDefault();
        //        }

        //        Logs += $"{logHeader}device online => {arg.Session.RemoteEndPoint}{Environment.NewLine}";
        //    });
        //}

        //private void SessionReceivedHandler(SessionReceivedEventArg arg)
        //{
        //    MainDispatcherInvoke(() =>
        //    {
        //        Logs += $"{logHeader}receive from ({arg.Session.RemoteEndPoint}) => {arg.Message}{Environment.NewLine}";
        //    });
        //}

        //private void ReceivedDataHandler(ReceivedDataEventArg arg)
        //{
        //    MainDispatcherInvoke(() =>
        //    {
        //        Logs += $"{logHeader}receive from ({arg.Session.RemoteEndPoint}) => {arg.Data}{Environment.NewLine}";
        //    });
        //}

        //private void MainDispatcherInvoke(Action action)
        //{
        //    var app = Application.Current as PrismApplication;
        //    app.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(action));
        //}

        private void SessionCloseHandler(SessionCloseEventArg arg)
        {
            MainDispatcher.Value.Invoke(() =>
            {
                var session = Sessions?.FirstOrDefault(p => p.Remote == arg.Session.RemoteEndPoint.ToString());
                if (session != null)
                {
                    Sessions.Remove(session);
                }

                Logs += $"{logHeader}device offline => {arg.Session.RemoteEndPoint}{Environment.NewLine}";
            });
        }

        private void SessionConnectedHandler(SessionConnectedEventArg arg)
        {
            MainDispatcher.Value.Invoke(() =>
            {
                Sessions.Add(new SessionItem { Remote = arg.Session.RemoteEndPoint.ToString(), Session = arg.Session });

                if (SelectedSession == null)
                {
                    SelectedSession = Sessions.FirstOrDefault();
                }

                Logs += $"{logHeader}device online => {arg.Session.RemoteEndPoint}{Environment.NewLine}";
            });
        }

        private void SessionReceivedHandler(SessionReceivedEventArg arg)
        {
            MainDispatcher.Value.Invoke(() =>
            {
                Logs += $"{logHeader}receive from ({arg.Session.RemoteEndPoint}) => {arg.Message}{Environment.NewLine}";
            });
        }

        private void ReceivedDataHandler(ReceivedDataEventArg arg)
        {
            MainDispatcher.Value.Invoke(() =>
            {
                Logs += $"{logHeader}receive from ({arg.Session.RemoteEndPoint}) => {arg.Data}{Environment.NewLine}";
            });
        }
        #endregion

        private void PopupNotification(string message, string title = "Message")
        {
            eventAggregator.GetEvent<MessagePopupEvent>().Publish(new PopupEventArg<INotification>
            {
                Title = title,
                Content = message,
                Callback = new Action<INotification>(res => { })
            });
        }

        public DelegateCommand HelperCommand => new DelegateCommand(() => PopupNotification("Helper Us."));

        public DelegateCommand DebugCommand => new DelegateCommand(() =>
        {
            var j1 = SelectedServer.GetJsonString();

            PopupNotification($"{nameof(SelectedServer)}: {j1}");
        });

        private bool isRunning;

        public bool IsRunning
        {
            get => isRunning;
            set => SetProperty(ref isRunning, value);
        }

        public DelegateCommand StartCommand => new DelegateCommand(() =>
        {
            if (SelectedServer == null)
            {
                return;
            }

            deviceProxy.Start(SelectedServer.ServerName, SelectedServer.Port ?? 8080);

            State = deviceProxy.State.ToString();
            IsRunning = true;
        });

        public DelegateCommand StopCommand => new DelegateCommand(() =>
        {
            deviceProxy.Stop();

            Sessions.Clear();

            State = deviceProxy.State.ToString();
            IsRunning = false;
        });

        public DelegateCommand SendToCommand => new DelegateCommand(() =>
        {
            if (SelectedScript == null || SelectedSession == null)
            {
                return;
            }

            deviceProxy.Send(SelectedScript.Context, SelectedSession.Session);
        });

        public DelegateCommand SendToAllCommand => new DelegateCommand(() =>
        {
            if (SelectedScript == null)
            {
                return;
            }

            if (Sessions != null && Sessions.Any())
            {
                deviceProxy.Send(SelectedScript.Context, Sessions.Select(p => p.Session).ToArray());
            }
        });

        public DelegateCommand ReloadScriptsCommand => new DelegateCommand(() => SelectedServerChangedCommand.Execute(SelectedServer));

        public DelegateCommand CleanLogsCommand => new DelegateCommand(() => Logs = string.Empty);

        public string Version => config.Version;

        private string time = DateTime.UtcNow.ToString("yyyy-MM-dd");

        public string Time
        {
            get => time;
            set => SetProperty(ref time, value);
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

            State = deviceProxy.State.ToString();
            Sessions.Clear();

            var serverJson = config.ReadAllText(Assembly.GetExecutingAssembly().Location.Replace("dll", "json"));

            var items = serverJson.GetJsonObject<ServerItem[]>();
            if (items != null && items.Any())
            {
                ServerItems = new ObservableCollection<ServerItem>(items);
                if (SelectedServer == null)
                {
                    SelectedServer = ServerItems?.FirstOrDefault();
                }

                SelectedServerChangedCommand.Execute(SelectedServer);
            }
        });

        private ObservableCollection<ServerItem> serverItems = new ObservableCollection<ServerItem>();

        public ObservableCollection<ServerItem> ServerItems
        {
            get => serverItems;
            set => SetProperty(ref serverItems, value);
        }

        private ServerItem selectedServer;

        public ServerItem SelectedServer
        {
            get => selectedServer;
            set => SetProperty(ref selectedServer, value);
        }

        private ObservableCollection<ScriptItemParser> scriptItems = new ObservableCollection<ScriptItemParser>();

        public ObservableCollection<ScriptItemParser> ScriptItems
        {
            get => scriptItems;
            set => SetProperty(ref scriptItems, value);
        }

        private ScriptItemParser selectedScript;

        public ScriptItemParser SelectedScript
        {
            get => selectedScript;
            set => SetProperty(ref selectedScript, value);
        }

        public DelegateCommand<ServerItem> SelectedServerChangedCommand => new DelegateCommand<ServerItem>(server =>
        {
            if (server == null)
            {
                return;
            }

            if (deviceProxy.State == ServerState.Running)
            {
                StopCommand.Execute();
            }

            ScriptItems.Clear();

            var scriptPath = Path.Combine(Assembly.GetExecutingAssembly().Location.Replace("dll", "Scripts"), $"{server.ItemKey}.json");
            if (File.Exists(scriptPath))
            {
                var scriptJson = config.ReadAllText(scriptPath);
                if (!string.IsNullOrEmpty(scriptJson))
                {
                    var items = JsonConvert.DeserializeObject<ScriptItemParser[]>(scriptJson);
                    if (items != null && items.Any())
                    {
                        ScriptItems = new ObservableCollection<ScriptItemParser>(items);
                        if (SelectedScript == null)
                        {
                            SelectedScript = items?.FirstOrDefault();
                        }
                    }

                    //scriptKeyInfos?.ToList()?.ForEach(p =>
                    //{
                    //    switch (p.MethodType)
                    //    {
                    //        case ScriptType.Send:
                    //            p.ConvertTo<SendScriptItemData>();
                    //            break;
                    //        case ScriptType.Unknown:
                    //        default: break;
                    //    }

                    //    //var instance =;
                    //});
                }
            }
        });
    }
}
