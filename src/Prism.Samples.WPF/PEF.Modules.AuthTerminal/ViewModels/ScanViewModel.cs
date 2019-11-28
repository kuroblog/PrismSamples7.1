
namespace PEF.Modules.AuthTerminal.ViewModels
{
    using PEF.Common;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.AuthTerminal.Devices;
    using PEF.Modules.AuthTerminal.Devices.Dtos;
    //using PEF.Modules.AuthTerminal.Models;
    using PEF.Modules.AuthTerminal.PubSubEvents;
    using PEF.Modules.AuthTerminal.Services;
    using PEF.Modules.AuthTerminal.Services.Dtos;
    //using PEF.Modules.AuthTerminal.Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Unity;
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;

    public partial class ScanViewModel : BindableBase, INavigationAware
    {
        private readonly AuthTerminalConfig config = null;
        private readonly IAuthTerminalDeviceProxy deviceProxy = null;
        private readonly IAuthTerminalServiceProxy serviceProxy = null;
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager region = null;
        private readonly ILogger logger = null;

        public ScanViewModel(
            AuthTerminalConfig config,
            IAuthTerminalDeviceProxy deviceProxy,
            IAuthTerminalServiceProxy serviceProxy,
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

            //eventAggregator.GetEvent<DeviceIdReceiveEvent>().Subscribe(DeviceIdReceiveHandler);
            //eventAggregator.GetEvent<CardIdReceiveEvent>().Subscribe(CardIdReceiveHandler);
            ////eventAggregator.GetEvent<DeliveryCompletedEvent>().Subscribe(DeliveryCompletedHandler);
            //eventAggregator.GetEvent<DeliveryCompletedEvent>().Subscribe(DeliveryCompletedV2Handler);
        }

        //private void MainDispatcherInvoke(Action action)
        //{
        //    var app = Application.Current as PrismApplication;
        //    app.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(action));
        //}

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            //RefreshSizeQuantityItems();

            eventAggregator.GetEvent<SetAuthUserCommandVisibility>().Publish(Visibility.Hidden);
        });

        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var viewName = navigationContext.Parameters[AuthTerminalKeys.CallbackViewName] as string;
            //if (viewName == typeof(SizeChoiceView).FullName)
            //{
            //    var size = navigationContext.Parameters[SGDEKeys.SelectedSize] as string;
            //    //DoApplySubmit(person.UserId, 2, size);
            //    DoApplySubmitV2(person.UserId, 2, size);
            //}
        }
        #endregion
    }
}
