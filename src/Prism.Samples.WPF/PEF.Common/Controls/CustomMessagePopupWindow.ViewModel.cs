
namespace PEF.Common.Controls
{
    using Prism.Commands;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using System;

    public class CustomMessagePopupWindowViewModel : BindableBase, IInteractionRequestAware
    {
        private INotification notification;

        public INotification Notification
        {
            get => notification;
            set => SetProperty(ref notification, value);
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand ConfirmCommand => new DelegateCommand(() =>
        {
            FinishInteraction?.Invoke();
        });
    }
}
