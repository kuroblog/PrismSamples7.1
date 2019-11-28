
namespace PEF.Modules.SGDE.Controls
{
    using PEF.Common;
    using PEF.Common.PubSubEvents;
    using PEF.Modules.SGDE.Models;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using System;

    public class ItemConfigWindowViewModel : BindableBase, IInteractionRequestAware
    {
        private readonly IEventAggregator eventAggregator;

        public ItemConfigWindowViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        private ICustomPopupNotification<int> customNotification;

        public INotification Notification
        {
            get => customNotification;
            set => SetProperty(ref customNotification, (ICustomPopupNotification<int>)value);
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand ConfirmCommand => new DelegateCommand(() =>
        {
            var item = Notification as ItemConfigPopupNotification;

            customNotification.Result = item != null ? item.SelectedItem.Stock : 0;
            customNotification.Confirmed = true;
            FinishInteraction?.Invoke();
        });

        public DelegateCommand CancelCommand => new DelegateCommand(() =>
        {
            customNotification.Confirmed = false;
            FinishInteraction?.Invoke();
        });

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg => eventAggregator.GetEvent<ShellWindowOpacityEvent>().Publish(0.3));

        public DelegateCommand<object> UnloadedCommand => new DelegateCommand<object>(arg => eventAggregator.GetEvent<ShellWindowOpacityEvent>().Publish(1));

        public DelegateCommand PlusCommand => new DelegateCommand(() =>
        {
            if (Notification is ItemConfigPopupNotification item)
            {
                if (item != null)
                {
                    if (item.SelectedItem.Stock < item.SelectedItem.Limit)
                    {
                        item.SelectedItem.Stock += 1;
                    }
                }
            }
        });

        public DelegateCommand LessCommand => new DelegateCommand(() =>
        {
            if (Notification is ItemConfigPopupNotification item)
            {
                if (item != null)
                {
                    if (item.SelectedItem.Stock > 0)
                    {
                        item.SelectedItem.Stock -= 1;
                    }
                }
            }
        });
    }

    public class ItemConfigPopupNotification : Confirmation, ICustomPopupNotification<int>
    {
        public ItemSlotConfigModel SelectedItem { get; set; }

        public int Result { get; set; }
    }
}
