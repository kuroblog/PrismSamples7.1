
namespace PEF.Modules.ShoeBox.ViewModels
{
    using PEF.Common;
    using PEF.Common.PubSubEvents;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.ShoeBox.Controls;
    using PEF.Modules.ShoeBox.Devices;
    using PEF.Modules.ShoeBox.Models;
    using PEF.Modules.ShoeBox.PubSubEvents;
    using PEF.Modules.ShoeBox.Devices.Dtos;
    using PEF.Modules.ShoeBox.Services;
    using PEF.Modules.ShoeBox.Services.Dtos;
    using PEF.Modules.ShoeBox.Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Prism.Unity;
    using System.Windows.Threading;

    public partial class AdminConfigViewModel : BindableBase
    {
        private readonly ILogger logger = null;
        private readonly IRegionManager region = null;
        private readonly IEventAggregator eventAggregator = null;
        private readonly ShoeBoxConfig config = null;
        private readonly IShoeBoxDeviceProxy deviceProxy = null;
        private readonly IShoeBoxServiceProxy serviceProxy = null;

        public AdminConfigViewModel(
            ILogger logger,
            IRegionManager region,
            IEventAggregator eventAggregator,
            ShoeBoxConfig config,
            IShoeBoxDeviceProxy deviceProxy,
            IShoeBoxServiceProxy serviceProxy)
        {
            this.logger = logger;
            this.region = region;
            this.eventAggregator = eventAggregator;
            this.config = config;
            this.deviceProxy = deviceProxy;
            this.serviceProxy = serviceProxy;

            this.eventAggregator?.GetEvent<AdminOpenDoorTransferEvent>().Subscribe(AdminOpenDoorTransferHandler);
            this.eventAggregator?.GetEvent<AdminCloseDoorTransferEvent>().Subscribe(AdminCloseDoorTransferHandler);
        }

        //private void MainDispatcherInvoke(Action action)
        //{
        //    var app = Application.Current as PrismApplication;
        //    app.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(action));
        //}

        private void AdminOpenDoorTransferHandler(string doorId)
        {
            //MainDispatcherInvoke(() =>
            //{
            var item = configItems?.FirstOrDefault(p => p.ItemCode == doorId);
            if (item != null)
            {
                item.State = ConfigState.Opened;
            }

            SetAllConfigItems();
            //});
        }

        private void AdminCloseDoorTransferHandler(string doorId)
        {
            //MainDispatcherInvoke(() =>
            //{
            var item = configItems?.FirstOrDefault(p => p.ItemCode == doorId);
            if (item != null && item.State == ConfigState.Opened)
            {
                var dcqResult = serviceProxy.DeviceItemStateQuery(new DeviceItemStateQueryRequest { DeviceItemId = item.ItemId });
                if (dcqResult != null && dcqResult.IsSuccessful && dcqResult.HasData)
                {
                    item.State = dcqResult.Data.Status == 1 ? ConfigState.Locked : dcqResult.Data.Status == 2 ? ConfigState.Unused : dcqResult.Data.Status == 3 ? ConfigState.ToBeRecycled : ConfigState.Unknown;
                }
            }

            SetAllConfigItems();
            //});
        }

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            configItems.Clear();

            var dcqResult = serviceProxy.DeviceConfigQuery(new DeviceConfigQueryRequest { DeviceId = config.DeviceId });
            if (dcqResult != null && dcqResult.IsSuccessful && dcqResult.HasData)
            {
                configItems = dcqResult.Data.Items?.Select(p => new ConfigItem
                {
                    ItemId = p.DeviceItemId,
                    ItemNo = p.DeviceItemNo,
                    ItemCode = p.DeviceItemCode,
                    UserName = p.UserName,
                    State = p.Status == 1 ? ConfigState.Locked : p.Status == 2 ? ConfigState.Unused : p.Status == 3 ? ConfigState.ToBeRecycled : ConfigState.Unknown
                })?.ToList();
            }
            else
            {

            }

            TotalPage = configItems.Count % 40 == 0 ? configItems.Count / 40 : configItems.Count / 40 + 1;
            CurrentPage = TotalPage > 0 ? 1 : 0;

            SetAllConfigItems();
        });

        public DelegateCommand<ConfigItem> ItemConfigCommand => new DelegateCommand<ConfigItem>(arg =>
        {
            if (arg != null && arg.State == ConfigState.Locked)
            {
                eventAggregator?.GetEvent<ShoeBoxConfirmPopupEvent>().Publish(new PopupEventArg<IConfirmation>
                {
                    Title = "温馨提示",
                    Content = $"{arg.ItemNo}号已经被{arg.UserName}占用{Environment.NewLine}确认是否打开？",
                    Callback = new Action<IConfirmation>(res =>
                    {
                        if (res.Confirmed)
                        {
                            deviceProxy.OpenDoor(new DeviceOpenDoorDto(new[] { arg.ItemCode }));
                        }
                    })
                });
            }
        });

        private List<ConfigItem> configItems = new List<ConfigItem>();

        private ConfigItem[] currentItems = new ConfigItem[] { };

        private void SetAllConfigItems()
        {
            currentItems = configItems.Skip((CurrentPage - 1) * 40).Take(40)?.ToArray();

            SetConfigItem(ConfigItem0, currentItems, 0);
            SetConfigItem(ConfigItem1, currentItems, 1);
            SetConfigItem(ConfigItem2, currentItems, 2);
            SetConfigItem(ConfigItem3, currentItems, 3);
            SetConfigItem(ConfigItem4, currentItems, 4);
            SetConfigItem(ConfigItem5, currentItems, 5);
            SetConfigItem(ConfigItem6, currentItems, 6);
            SetConfigItem(ConfigItem7, currentItems, 7);
            SetConfigItem(ConfigItem8, currentItems, 8);
            SetConfigItem(ConfigItem9, currentItems, 9);
            SetConfigItem(ConfigItem10, currentItems, 10);
            SetConfigItem(ConfigItem11, currentItems, 11);
            SetConfigItem(ConfigItem12, currentItems, 12);
            SetConfigItem(ConfigItem13, currentItems, 13);
            SetConfigItem(ConfigItem14, currentItems, 14);
            SetConfigItem(ConfigItem15, currentItems, 15);
            SetConfigItem(ConfigItem16, currentItems, 16);
            SetConfigItem(ConfigItem17, currentItems, 17);
            SetConfigItem(ConfigItem18, currentItems, 18);
            SetConfigItem(ConfigItem19, currentItems, 19);
            SetConfigItem(ConfigItem20, currentItems, 20);
            SetConfigItem(ConfigItem21, currentItems, 21);
            SetConfigItem(ConfigItem22, currentItems, 22);
            SetConfigItem(ConfigItem23, currentItems, 23);
            SetConfigItem(ConfigItem24, currentItems, 24);
            SetConfigItem(ConfigItem25, currentItems, 25);
            SetConfigItem(ConfigItem26, currentItems, 26);
            SetConfigItem(ConfigItem27, currentItems, 27);
            SetConfigItem(ConfigItem28, currentItems, 28);
            SetConfigItem(ConfigItem29, currentItems, 29);
            SetConfigItem(ConfigItem30, currentItems, 30);
            SetConfigItem(ConfigItem31, currentItems, 31);
            SetConfigItem(ConfigItem32, currentItems, 32);
            SetConfigItem(ConfigItem33, currentItems, 33);
            SetConfigItem(ConfigItem34, currentItems, 34);
            SetConfigItem(ConfigItem35, currentItems, 35);
            SetConfigItem(ConfigItem36, currentItems, 36);
            SetConfigItem(ConfigItem37, currentItems, 37);
            SetConfigItem(ConfigItem38, currentItems, 38);
            SetConfigItem(ConfigItem39, currentItems, 39);
        }

        private void SetConfigItem(ConfigItem target, ConfigItem[] source, int sourceIndex)
        {
            if (sourceIndex < source.Count())
            {
                target.ItemCode = source[sourceIndex].ItemCode;
                target.UserName = source[sourceIndex].UserName;
                target.State = source[sourceIndex].State;
                target.ItemId = source[sourceIndex].ItemId;
                target.ItemNo = source[sourceIndex].ItemNo;
            }
            else
            {
                target.ItemCode = string.Empty;
                target.UserName = string.Empty;
            }
        }

        private int currentPage;

        public int CurrentPage
        {
            get => currentPage;
            set => SetProperty(ref currentPage, value);
        }

        private int totalPage;

        public int TotalPage
        {
            get => totalPage;
            set => SetProperty(ref totalPage, value);
        }

        public DelegateCommand PreviousPageCommand => new DelegateCommand(() =>
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }

            SetAllConfigItems();
        });

        public DelegateCommand NextPageCommand => new DelegateCommand(() =>
        {
            if (CurrentPage < TotalPage && CurrentPage > 0)
            {
                CurrentPage++;
            }

            SetAllConfigItems();
        });

        public DelegateCommand ResetCommand => new DelegateCommand(() =>
        {
            //var randomGenerator = new Random();
            //ConfigItem0.Code = randomGenerator.Next(0, 10).ToString();
            //ConfigItem0.State = (Controls.ConfigState)randomGenerator.Next(0, 4);

            //ConfigItem1.Code = "2";
            //ConfigItem1.UserName = "333";
            //ConfigItem1.State = ConfigItem0.State;

            serviceProxy.DeviceReset(new DeviceResetRequest { DeviceId = config.DeviceId });

            LoadedCommand.Execute(null);
        });

        public DelegateCommand AllOpenCommand => new DelegateCommand(() =>
        {
            var codes = configItems?.Where(p => p.State == ConfigState.ToBeRecycled)?.Select(p => p.ItemCode)?.ToArray();

            if (codes != null && codes.Any())
            {
                deviceProxy.OpenDoor(new DeviceOpenDoorDto(codes));
            }

            //SetAllConfigItems();
        });

        public DelegateCommand ExitCommand => new DelegateCommand(() => region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName));

        #region config items
        private ConfigItem configItem0 = new ConfigItem();

        public ConfigItem ConfigItem0
        {
            get => configItem0;
            set => SetProperty(ref configItem0, value);
        }

        private ConfigItem configItem1 = new ConfigItem();

        public ConfigItem ConfigItem1
        {
            get => configItem1;
            set => SetProperty(ref configItem1, value);
        }

        private ConfigItem configItem2 = new ConfigItem();

        public ConfigItem ConfigItem2
        {
            get => configItem2;
            set => SetProperty(ref configItem2, value);
        }

        private ConfigItem configItem3 = new ConfigItem();

        public ConfigItem ConfigItem3
        {
            get => configItem3;
            set => SetProperty(ref configItem3, value);
        }

        private ConfigItem configItem4 = new ConfigItem();

        public ConfigItem ConfigItem4
        {
            get => configItem4;
            set => SetProperty(ref configItem4, value);
        }

        private ConfigItem configItem5 = new ConfigItem();

        public ConfigItem ConfigItem5
        {
            get => configItem5;
            set => SetProperty(ref configItem5, value);
        }

        private ConfigItem configItem6 = new ConfigItem();

        public ConfigItem ConfigItem6
        {
            get => configItem6;
            set => SetProperty(ref configItem6, value);
        }

        private ConfigItem configItem7 = new ConfigItem();

        public ConfigItem ConfigItem7
        {
            get => configItem7;
            set => SetProperty(ref configItem7, value);
        }

        private ConfigItem configItem8 = new ConfigItem();

        public ConfigItem ConfigItem8
        {
            get => configItem8;
            set => SetProperty(ref configItem8, value);
        }

        private ConfigItem configItem9 = new ConfigItem();

        public ConfigItem ConfigItem9
        {
            get => configItem9;
            set => SetProperty(ref configItem9, value);
        }

        private ConfigItem configItem10 = new ConfigItem();

        public ConfigItem ConfigItem10
        {
            get => configItem10;
            set => SetProperty(ref configItem10, value);
        }

        private ConfigItem configItem11 = new ConfigItem();

        public ConfigItem ConfigItem11
        {
            get => configItem11;
            set => SetProperty(ref configItem11, value);
        }

        private ConfigItem configItem12 = new ConfigItem();

        public ConfigItem ConfigItem12
        {
            get => configItem12;
            set => SetProperty(ref configItem12, value);
        }

        private ConfigItem configItem13 = new ConfigItem();

        public ConfigItem ConfigItem13
        {
            get => configItem13;
            set => SetProperty(ref configItem13, value);
        }

        private ConfigItem configItem14 = new ConfigItem();

        public ConfigItem ConfigItem14
        {
            get => configItem14;
            set => SetProperty(ref configItem14, value);
        }

        private ConfigItem configItem15 = new ConfigItem();

        public ConfigItem ConfigItem15
        {
            get => configItem15;
            set => SetProperty(ref configItem15, value);
        }

        private ConfigItem configItem16 = new ConfigItem();

        public ConfigItem ConfigItem16
        {
            get => configItem16;
            set => SetProperty(ref configItem16, value);
        }

        private ConfigItem configItem17 = new ConfigItem();

        public ConfigItem ConfigItem17
        {
            get => configItem17;
            set => SetProperty(ref configItem17, value);
        }

        private ConfigItem configItem18 = new ConfigItem();

        public ConfigItem ConfigItem18
        {
            get => configItem18;
            set => SetProperty(ref configItem18, value);
        }

        private ConfigItem configItem19 = new ConfigItem();

        public ConfigItem ConfigItem19
        {
            get => configItem19;
            set => SetProperty(ref configItem19, value);
        }

        private ConfigItem configItem20 = new ConfigItem();

        public ConfigItem ConfigItem20
        {
            get => configItem20;
            set => SetProperty(ref configItem20, value);
        }

        private ConfigItem configItem21 = new ConfigItem();

        public ConfigItem ConfigItem21
        {
            get => configItem21;
            set => SetProperty(ref configItem21, value);
        }

        private ConfigItem configItem22 = new ConfigItem();

        public ConfigItem ConfigItem22
        {
            get => configItem22;
            set => SetProperty(ref configItem22, value);
        }

        private ConfigItem configItem23 = new ConfigItem();

        public ConfigItem ConfigItem23
        {
            get => configItem23;
            set => SetProperty(ref configItem23, value);
        }

        private ConfigItem configItem24 = new ConfigItem();

        public ConfigItem ConfigItem24
        {
            get => configItem24;
            set => SetProperty(ref configItem24, value);
        }

        private ConfigItem configItem25 = new ConfigItem();

        public ConfigItem ConfigItem25
        {
            get => configItem25;
            set => SetProperty(ref configItem25, value);
        }

        private ConfigItem configItem26 = new ConfigItem();

        public ConfigItem ConfigItem26
        {
            get => configItem26;
            set => SetProperty(ref configItem26, value);
        }

        private ConfigItem configItem27 = new ConfigItem();

        public ConfigItem ConfigItem27
        {
            get => configItem27;
            set => SetProperty(ref configItem27, value);
        }

        private ConfigItem configItem28 = new ConfigItem();

        public ConfigItem ConfigItem28
        {
            get => configItem28;
            set => SetProperty(ref configItem28, value);
        }

        private ConfigItem configItem29 = new ConfigItem();

        public ConfigItem ConfigItem29
        {
            get => configItem29;
            set => SetProperty(ref configItem29, value);
        }

        private ConfigItem configItem30 = new ConfigItem();

        public ConfigItem ConfigItem30
        {
            get => configItem30;
            set => SetProperty(ref configItem30, value);
        }

        private ConfigItem configItem31 = new ConfigItem();

        public ConfigItem ConfigItem31
        {
            get => configItem31;
            set => SetProperty(ref configItem31, value);
        }

        private ConfigItem configItem32 = new ConfigItem();

        public ConfigItem ConfigItem32
        {
            get => configItem32;
            set => SetProperty(ref configItem32, value);
        }

        private ConfigItem configItem33 = new ConfigItem();

        public ConfigItem ConfigItem33
        {
            get => configItem33;
            set => SetProperty(ref configItem33, value);
        }

        private ConfigItem configItem34 = new ConfigItem();

        public ConfigItem ConfigItem34
        {
            get => configItem34;
            set => SetProperty(ref configItem34, value);
        }

        private ConfigItem configItem35 = new ConfigItem();

        public ConfigItem ConfigItem35
        {
            get => configItem35;
            set => SetProperty(ref configItem35, value);
        }

        private ConfigItem configItem36 = new ConfigItem();

        public ConfigItem ConfigItem36
        {
            get => configItem36;
            set => SetProperty(ref configItem36, value);
        }

        private ConfigItem configItem37 = new ConfigItem();

        public ConfigItem ConfigItem37
        {
            get => configItem37;
            set => SetProperty(ref configItem37, value);
        }

        private ConfigItem configItem38 = new ConfigItem();

        public ConfigItem ConfigItem38
        {
            get => configItem38;
            set => SetProperty(ref configItem38, value);
        }

        private ConfigItem configItem39 = new ConfigItem();

        public ConfigItem ConfigItem39
        {
            get => configItem39;
            set => SetProperty(ref configItem39, value);
        }
        #endregion
    }
}
