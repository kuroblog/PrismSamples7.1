
namespace PEF.Common.Controls
{
    using Prism.Commands;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using System;

    public class CustomPopupWindowViewModel : BindableBase, IInteractionRequestAware
    {
        private ICustomPopupNotification<string> customNotification;

        public INotification Notification
        {
            get => customNotification;
            set => SetProperty(ref customNotification, (ICustomPopupNotification<string>)value);
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand ConfirmCommand => new DelegateCommand(() =>
        {
            customNotification.Result = nameof(ConfirmCommand);
            customNotification.Confirmed = true;
            FinishInteraction?.Invoke();
        });

        public DelegateCommand CancelCommand => new DelegateCommand(() =>
        {
            customNotification.Result = nameof(CancelCommand);
            customNotification.Confirmed = false;
            FinishInteraction?.Invoke();
        });
    }
}
