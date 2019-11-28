
namespace PEF.Modules.SGDE.ViewModels
{
    using PEF.Common;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.SGDE.Controls;
    using PEF.Modules.SGDE.Devices;
    using PEF.Modules.SGDE.Devices.Dtos;
    using PEF.Modules.SGDE.Models;
    using PEF.Modules.SGDE.PubSubEvents;
    using PEF.Modules.SGDE.Services;
    using PEF.Modules.SGDE.Services.Dtos;
    using PEF.Modules.SGDE.Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Unity;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;

    public partial class SizeConfigViewModel : BindableBase
    {
        private readonly SGDEConfig config = null;
        private readonly ISGDEDeviceProxy deviceProxy = null;
        private readonly ISGDEServiceProxy serviceProxy = null;
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager region = null;
        private readonly ILogger logger = null;

        public SizeConfigViewModel(
            SGDEConfig config,
            ISGDEDeviceProxy deviceProxy,
            ISGDEServiceProxy serviceProxy,
            IEventAggregator eventAggregator,
            IRegionManager region,
            ILogger logger)
        {
            this.config = config;
            this.deviceProxy = deviceProxy;
            this.serviceProxy = serviceProxy;
            this.eventAggregator = eventAggregator;
            this.region = region;
            this.logger = logger;

            eventAggregator.GetEvent<LoadCompletedEvent>().Subscribe(LoadCompletedHandler);
        }

        #region thread sync test
        //private void MainDispatcherInvoke(Action action)
        //{
        //    var app = Application.Current as PrismApplication;
        //    app.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(action));
        //}

        //private void LoadCompletedHandler()
        //{
        //    MainDispatcherInvoke(() => region?.RequestNavigate(
        //        RegionNames.Home,
        //        typeof(HomeView).FullName));
        //}

        private void LoadCompletedHandler() => MainDispatcher.Value.Invoke(() => region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName));
        #endregion

        private void DoSizeQuantity()
        {
            var query = slotConfigs
                .Where(p => p.SlotId != 0)
                .GroupBy(p => p.Size)
                ?.Select(p => new SizeQuantityModel
                {
                    Size = p.Key,
                    Quantity = p.Sum(x => x.Stock).ToString()
                })
                ?.ToArray();

            SqItemI = indexOfSizeQueryItems(query, 0);
            SqItemII = indexOfSizeQueryItems(query, 1);
            SqItemIII = indexOfSizeQueryItems(query, 2);
            SqItemIV = indexOfSizeQueryItems(query, 3);
            SqItemV = indexOfSizeQueryItems(query, 4);
            SqItemVI = indexOfSizeQueryItems(query, 5);
        }

        private Func<SizeQuantityModel[], int, SizeQuantityModel> indexOfSizeQueryItems = (source, index) =>
        {
            if (source.Length >= index + 1)
            {
                return new SizeQuantityModel
                {
                    Quantity = source[index].Quantity,
                    Size = source[index].Size
                };
            }

            return new SizeQuantityModel();
        };

        private Func<ConfigQueryItem[], int, ItemSlotConfigModel> indexOfConfigQueryItems = (source, index) =>
        {
            if (source.Length >= index + 1)
            {
                return new ItemSlotConfigModel
                {
                    Stock = source[index].Stock,
                    Limit = source[index].Limit,
                    Size = source[index].Size,
                    X = source[index].X,
                    Y = source[index].Y,
                    SlotId = source[index].Id
                };
            }

            return new ItemSlotConfigModel();
        };

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            slotConfigs.Clear();

            var result = serviceProxy?.ConfigQuery(new ConfigQueryRequest { DeviceId = config.DeviceId });
            if (result != null && result.IsSuccessful && result.HasData)
            {
                Slot00Config = indexOfConfigQueryItems(result.Data.Items, 0);
                Slot01Config = indexOfConfigQueryItems(result.Data.Items, 1);
                Slot02Config = indexOfConfigQueryItems(result.Data.Items, 2);

                Slot10Config = indexOfConfigQueryItems(result.Data.Items, 3);
                Slot11Config = indexOfConfigQueryItems(result.Data.Items, 4);
                Slot12Config = indexOfConfigQueryItems(result.Data.Items, 5);

                Slot20Config = indexOfConfigQueryItems(result.Data.Items, 6);
                Slot21Config = indexOfConfigQueryItems(result.Data.Items, 7);
                Slot22Config = indexOfConfigQueryItems(result.Data.Items, 8);

                Slot30Config = indexOfConfigQueryItems(result.Data.Items, 9);
                Slot31Config = indexOfConfigQueryItems(result.Data.Items, 10);
                Slot32Config = indexOfConfigQueryItems(result.Data.Items, 11);

                Slot40Config = indexOfConfigQueryItems(result.Data.Items, 12);
                Slot41Config = indexOfConfigQueryItems(result.Data.Items, 13);
                Slot42Config = indexOfConfigQueryItems(result.Data.Items, 14);

                Slot50Config = indexOfConfigQueryItems(result.Data.Items, 15);
                Slot51Config = indexOfConfigQueryItems(result.Data.Items, 16);
                Slot52Config = indexOfConfigQueryItems(result.Data.Items, 17);
            }
            else
            {
                Slot00Config = new ItemSlotConfigModel();
                Slot01Config = new ItemSlotConfigModel();
                Slot02Config = new ItemSlotConfigModel();

                Slot10Config = new ItemSlotConfigModel();
                Slot11Config = new ItemSlotConfigModel();
                Slot12Config = new ItemSlotConfigModel();

                Slot20Config = new ItemSlotConfigModel();
                Slot21Config = new ItemSlotConfigModel();
                Slot22Config = new ItemSlotConfigModel();

                Slot30Config = new ItemSlotConfigModel();
                Slot31Config = new ItemSlotConfigModel();
                Slot32Config = new ItemSlotConfigModel();

                Slot40Config = new ItemSlotConfigModel();
                Slot41Config = new ItemSlotConfigModel();
                Slot42Config = new ItemSlotConfigModel();

                Slot50Config = new ItemSlotConfigModel();
                Slot51Config = new ItemSlotConfigModel();
                Slot52Config = new ItemSlotConfigModel();
            }

            slotConfigs.AddRange(new[] {
                Slot00Config,
                Slot01Config,
                Slot02Config,
                Slot10Config,
                Slot11Config,
                Slot12Config,
                Slot20Config,
                Slot21Config,
                Slot22Config,
                Slot30Config,
                Slot31Config,
                Slot32Config,
                Slot40Config,
                Slot41Config,
                Slot42Config,
                Slot50Config,
                Slot51Config,
                Slot52Config });

            DoSizeQuantity();

            deviceProxy.DeviceDoorControlDtoHandler(new DeviceDoorControlDto());

            serviceProxy.LogReport(new LogReportRequest { DeviceId = config.DeviceId, LogType = 2 });
        });

        public DelegateCommand AllCleanCommand => new DelegateCommand(() =>
        {
            slotConfigs?.ToList().ForEach(p => p.Stock = 0);

            DoSizeQuantity();
        });

        public DelegateCommand AllFillCommand => new DelegateCommand(() =>
        {
            slotConfigs?.ToList().ForEach(p => p.Stock = p.Limit);

            DoSizeQuantity();
        });

        public DelegateCommand DoneCommand => new DelegateCommand(() =>
        {
            var result = serviceProxy.ConfigSubmit(new ConfigSubmitRequest
            {
                DeviceId = config.DeviceId,
                Items = slotConfigs?.Where(p => p.SlotId != 0).Select(p => new ConfigSubmitItem
                {
                    Id = p.SlotId,
                    Stock = p.Stock
                })?.ToArray()
            });
            if (result != null && result.IsSuccessful && result.HasData)
            {
                deviceProxy.DeviceLoadingSuccessDtoHandler(new DeviceLoadingSuccessDto());
            }
            else
            {
                MainDispatcher.Value.ShowMessage(result.Message);
            }
        });

        public DelegateCommand<ItemSlotConfigModel> ItemConfigCommand => new DelegateCommand<ItemSlotConfigModel>(arg =>
        {
            if (arg == null)
            {
                return;
            }

            //logger.Trace($"command:{nameof(ItemConfigCommand)}; arg:{arg.GetJsonString()} invoke");

            CustomPopupRequest.Raise(new ItemConfigPopupNotification
            {
                Title = "",
                Content = "",
                SelectedItem = new ItemSlotConfigModel
                {
                    Limit = arg.Limit,
                    Size = arg.Size,
                    SlotId = arg.SlotId,
                    Stock = arg.Stock,
                    X = arg.X,
                    Y = arg.Y
                }
            }, result =>
            {
                if (result.Confirmed)
                {
                    arg.Stock = result.Result;

                    DoSizeQuantity();
                }
            });
        });

        private SizeQuantityModel sqItemI;

        public SizeQuantityModel SqItemI
        {
            get => sqItemI;
            set => SetProperty(ref sqItemI, value);
        }

        private SizeQuantityModel sqItemII;

        public SizeQuantityModel SqItemII
        {
            get => sqItemII;
            set => SetProperty(ref sqItemII, value);
        }

        private SizeQuantityModel sqItemIII;

        public SizeQuantityModel SqItemIII
        {
            get => sqItemIII;
            set => SetProperty(ref sqItemIII, value);
        }

        private SizeQuantityModel sqItemIV;

        public SizeQuantityModel SqItemIV
        {
            get => sqItemIV;
            set => SetProperty(ref sqItemIV, value);
        }

        private SizeQuantityModel sqItemV;

        public SizeQuantityModel SqItemV
        {
            get => sqItemV;
            set => SetProperty(ref sqItemV, value);
        }

        private SizeQuantityModel sqItemVI;

        public SizeQuantityModel SqItemVI
        {
            get => sqItemVI;
            set => SetProperty(ref sqItemVI, value);
        }

        private ObservableCollection<ItemSlotConfigModel> slotConfigs = new ObservableCollection<ItemSlotConfigModel>();

        private ItemSlotConfigModel slot00Config;

        public ItemSlotConfigModel Slot00Config
        {
            get => slot00Config;
            set => SetProperty(ref slot00Config, value);
        }

        private ItemSlotConfigModel slot01Config;

        public ItemSlotConfigModel Slot01Config
        {
            get => slot01Config;
            set => SetProperty(ref slot01Config, value);
        }

        private ItemSlotConfigModel slot02Config;

        public ItemSlotConfigModel Slot02Config
        {
            get => slot02Config;
            set => SetProperty(ref slot02Config, value);
        }

        private ItemSlotConfigModel slot10Config;

        public ItemSlotConfigModel Slot10Config
        {
            get => slot10Config;
            set => SetProperty(ref slot10Config, value);
        }

        private ItemSlotConfigModel slot11Config;

        public ItemSlotConfigModel Slot11Config
        {
            get => slot11Config;
            set => SetProperty(ref slot11Config, value);
        }

        private ItemSlotConfigModel slot12Config;

        public ItemSlotConfigModel Slot12Config
        {
            get => slot12Config;
            set => SetProperty(ref slot12Config, value);
        }

        private ItemSlotConfigModel slot20Config;

        public ItemSlotConfigModel Slot20Config
        {
            get => slot20Config;
            set => SetProperty(ref slot20Config, value);
        }

        private ItemSlotConfigModel slot21Config;

        public ItemSlotConfigModel Slot21Config
        {
            get => slot21Config;
            set => SetProperty(ref slot21Config, value);
        }

        private ItemSlotConfigModel slot22Config;

        public ItemSlotConfigModel Slot22Config
        {
            get => slot22Config;
            set => SetProperty(ref slot22Config, value);
        }

        private ItemSlotConfigModel slot30Config;

        public ItemSlotConfigModel Slot30Config
        {
            get => slot30Config;
            set => SetProperty(ref slot30Config, value);
        }

        private ItemSlotConfigModel slot31Config;

        public ItemSlotConfigModel Slot31Config
        {
            get => slot31Config;
            set => SetProperty(ref slot31Config, value);
        }

        private ItemSlotConfigModel slot32Config;

        public ItemSlotConfigModel Slot32Config
        {
            get => slot32Config;
            set => SetProperty(ref slot32Config, value);
        }

        private ItemSlotConfigModel slot40Config;

        public ItemSlotConfigModel Slot40Config
        {
            get => slot40Config;
            set => SetProperty(ref slot40Config, value);
        }

        private ItemSlotConfigModel slot41Config;

        public ItemSlotConfigModel Slot41Config
        {
            get => slot41Config;
            set => SetProperty(ref slot41Config, value);
        }

        private ItemSlotConfigModel slot42Config;

        public ItemSlotConfigModel Slot42Config
        {
            get => slot42Config;
            set => SetProperty(ref slot42Config, value);
        }

        private ItemSlotConfigModel slot50Config;

        public ItemSlotConfigModel Slot50Config
        {
            get => slot50Config;
            set => SetProperty(ref slot50Config, value);
        }

        private ItemSlotConfigModel slot51Config;

        public ItemSlotConfigModel Slot51Config
        {
            get => slot51Config;
            set => SetProperty(ref slot51Config, value);
        }

        private ItemSlotConfigModel slot52Config;

        public ItemSlotConfigModel Slot52Config
        {
            get => slot52Config;
            set => SetProperty(ref slot52Config, value);
        }

        public InteractionRequest<ICustomPopupNotification<int>> CustomPopupRequest { get; } = new InteractionRequest<ICustomPopupNotification<int>>();
    }
}
