
namespace PEF.Modules.ShoeBox.Controls
{
    using PEF.Common;
    using PEF.Common.PubSubEvents;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Unity;
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;

    public class CustomNotificationWindowViewModel : BindableBase, IInteractionRequestAware
    {
        private readonly IEventAggregator eventAggregator;

        public CustomNotificationWindowViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

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

        private string confirmTitle;

        public string ConfirmTitle
        {
            get => confirmTitle;
            set => SetProperty(ref confirmTitle, value);
        }

        #region thread sync test
        //private void MainDispatcherInvoke(Action action)
        //{
        //    var app = Application.Current as PrismApplication;
        //    app.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(action));
        //}

        //public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        //{
        //    ConfirmTitle = "确定";

        //    eventAggregator.GetEvent<ShellWindowOpacityEvent>().Publish(0.3);

        //    Task.Factory.StartNew(() =>
        //    {
        //        var i = 9;
        //        for (; ; )
        //        {
        //            if (i == 0)
        //            {
        //                MainDispatcherInvoke(() =>
        //                {
        //                    ConfirmCommand.Execute();

        //                    ConfirmTitle = "确认";
        //                });

        //                break;
        //            }

        //            MainDispatcherInvoke(() => ConfirmTitle = $"确认 ({i})");

        //            Task.Delay(1000).Wait();
        //            i--;
        //        }
        //    });
        //});

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            ConfirmTitle = "确定";

            eventAggregator.GetEvent<ShellWindowOpacityEvent>().Publish(0.3);

            Task.Factory.StartNew(() =>
            {
                var i = 9;
                for (; ; )
                {
                    if (i == 0)
                    {
                        MainDispatcher.Value.Invoke(() =>
                       {
                           ConfirmCommand.Execute();

                           ConfirmTitle = "确认";
                       });

                        break;
                    }

                    MainDispatcher.Value.Invoke(() => ConfirmTitle = $"确认 ({i})");

                    Task.Delay(1000).Wait();
                    i--;
                }
            });
        });
        #endregion

        public DelegateCommand<object> UnloadedCommand => new DelegateCommand<object>(arg => eventAggregator.GetEvent<ShellWindowOpacityEvent>().Publish(1));
    }
}
