
namespace PEF.Modules.AuthTerminal.ViewModels
{
    using PEF.Common;
    using PEF.Common.PubSubEvents;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.AuthTerminal.Models;
    using PEF.Modules.AuthTerminal.PubSubEvents;
    using PEF.Modules.AuthTerminal.Services;
    using PEF.Modules.AuthTerminal.Services.Dtos;
    //using PEF.Modules.AuthTerminal.Models;
    using PEF.Modules.AuthTerminal.Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    public partial class RoleChoiceViewModel : BindableBase, INavigationAware
    {
        private readonly AuthTerminalConfig config = null;
        //private readonly ISGDEDeviceProxy deviceProxy = null;
        private readonly IAuthTerminalServiceProxy serviceProxy = null;
        private readonly IRegionManager region = null;
        private readonly IEventAggregator eventAggregator;
        private readonly ILogger logger = null;

        public RoleChoiceViewModel(
            AuthTerminalConfig config,
            //ISGDEDeviceProxy deviceProxy,
            //ISGDEServiceProxy serviceProxy,
            IAuthTerminalServiceProxy serviceProxy,
            IRegionManager region,
            IEventAggregator eventAggregator,
            ILogger logger)
        {
            this.config = config;
            //this.deviceProxy = deviceProxy;
            //this.serviceProxy = serviceProxy;
            this.serviceProxy = serviceProxy;
            this.region = region;
            this.eventAggregator = eventAggregator;
            this.logger = logger;
        }

        private RoleModel roleInfo0 = new RoleModel();

        public RoleModel RoleInfo0
        {
            get => roleInfo0;
            set => SetProperty(ref roleInfo0, value);
        }

        private RoleModel roleInfo1 = new RoleModel();

        public RoleModel RoleInfo1
        {
            get => roleInfo1;
            set => SetProperty(ref roleInfo1, value);
        }

        private RoleModel roleInfo2 = new RoleModel();

        public RoleModel RoleInfo2
        {
            get => roleInfo2;
            set => SetProperty(ref roleInfo2, value);
        }

        private RoleModel roleInfo3 = new RoleModel();

        public RoleModel RoleInfo3
        {
            get => roleInfo3;
            set => SetProperty(ref roleInfo3, value);
        }

        private RoleModel roleInfo4 = new RoleModel();

        public RoleModel RoleInfo4
        {
            get => roleInfo4;
            set => SetProperty(ref roleInfo4, value);
        }

        private RoleModel roleInfo5 = new RoleModel();

        public RoleModel RoleInfo5
        {
            get => roleInfo5;
            set => SetProperty(ref roleInfo5, value);
        }

        private void SetRoleInfo(RoleModel target, RoleItem[] source, int index)
        {
            if (source.Length > index)
            {
                target.RoleId = source[index].RoleId;
                target.RoleName = source[index].RoleName;
            }
            else
            {
                target.RoleId = 0;
                target.RoleName = string.Empty;
            }

            roles.Add(target);
        }

        private List<RoleModel> roles = new List<RoleModel> { };

        private void SetAllRoleInfo(RoleItem[] source)
        {
            roles.Clear();

            SetRoleInfo(RoleInfo0, source, 0);
            SetRoleInfo(RoleInfo1, source, 1);
            SetRoleInfo(RoleInfo2, source, 2);
            SetRoleInfo(RoleInfo3, source, 3);
            SetRoleInfo(RoleInfo4, source, 4);
            SetRoleInfo(RoleInfo5, source, 5);
        }

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            eventAggregator.GetEvent<SetAuthUserCommandVisibility>().Publish(Visibility.Hidden);

            var result = serviceProxy?.RoleQuery();
            if (result != null && result.IsSuccessful && result.HasData)
            {
                SetAllRoleInfo(result.Data.Roles ?? (new RoleItem[] { }));
            }
        });

        public DelegateCommand<object> ChoiceCommand => new DelegateCommand<object>(arg =>
        {
            var role = roles.FirstOrDefault(p => p.RoleName == arg.ToString());
            if (role != null)
            {
                var result = serviceProxy?.UserRoleBinding(new UserRoleBindingRequest { RoleId = role.RoleId, CardId = cardId, ReadStyle = guestReadStyle });
                if (result != null && result.IsSuccessful && result.HasData)
                {
                    var callbackViewName = region?.Regions[RegionNames.Home].ActiveViews.FirstOrDefault().GetType().FullName;
                    var nParam = new NavigationParameters
                    {
                        { AuthTerminalKeys.CallbackViewName, callbackViewName },
                        { AuthTerminalKeys.UserRoleBindingResult, new string[]{ result.Data.UserName, role.RoleName } }
                    };

                    region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName, nParam);
                }
                else
                {
                    MainDispatcher.Value.ShowMessage(result.Message);
                }
            }
        });

        //private void PopupMainNotification(string message, string title = "温馨提示")
        //{
        //    eventAggregator.GetEvent<MainNotificationPopupEvent>().Publish(new PopupEventArg<INotification>
        //    {
        //        Title = title,
        //        Content = message,
        //        Callback = new Action<INotification>(res => { })
        //    });
        //}

        private string cardId;

        private int guestReadStyle;

        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            cardId = navigationContext.Parameters[AuthTerminalKeys.CardId] as string;
            guestReadStyle = int.TryParse(navigationContext.Parameters[AuthTerminalKeys.GuestReadStyle].ToString(), out int rs) ? rs : 1;
        }
        #endregion
    }
}
