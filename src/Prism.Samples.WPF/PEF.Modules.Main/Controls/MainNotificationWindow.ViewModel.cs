
namespace PEF.Modules.Main.Controls
{
    using PEF.Common;
    using PEF.Common.PubSubEvents;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Unity;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;

    public class MainNotificationWindowViewModel : BindableBase, IInteractionRequestAware
    {
        private readonly IEventAggregator eventAggregator;

        public MainNotificationWindowViewModel(IEventAggregator eventAggregator)
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
            //FinishInteraction?.Invoke();

            tokenSource?.Cancel();
        });

        private string confirmTitle;

        public string ConfirmTitle
        {
            get => confirmTitle;
            set => SetProperty(ref confirmTitle, value);
        }

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            ConfirmTitle = "确定";

            eventAggregator.GetEvent<ShellWindowOpacityEvent>().Publish(0.3);

            tokenSource = new CancellationTokenSource();

            AutoCloseWindows(tokenSource.Token, 9);
        });

        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        #region thread sync test
        //private void MainDispatcherInvoke(Action action)
        //{
        //    var app = Application.Current as PrismApplication;
        //    app.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(action));
        //}

        //private void AutoCloseWindows(CancellationToken ct, int counter)
        //{
        //    Task.Factory.StartNew(() =>
        //    {
        //        for (; ; )
        //        {
        //            if (ct.IsCancellationRequested || counter == 0)
        //            {
        //                MainDispatcherInvoke(() =>
        //                {
        //                    //ConfirmTitle = "确认";
        //                    //ConfirmCommand.Execute();

        //                    FinishInteraction?.Invoke();
        //                });

        //                break;
        //            }

        //            MainDispatcherInvoke(() => ConfirmTitle = $"确认 ({counter})");

        //            Task.Delay(1000).Wait();
        //            counter--;
        //        }
        //    });
        //}

        private void AutoCloseWindows(CancellationToken ct, int counter)
        {
            Task.Factory.StartNew(() =>
            {
                for (; ; )
                {
                    if (ct.IsCancellationRequested || counter == 0)
                    {
                        MainDispatcher.Value.Invoke(() =>
                       {
                           //ConfirmTitle = "确认";
                           //ConfirmCommand.Execute();

                           FinishInteraction?.Invoke();
                       });

                        break;
                    }

                    MainDispatcher.Value.Invoke(() => ConfirmTitle = $"确认 ({counter})");

                    Task.Delay(1000).Wait();
                    counter--;
                }
            });
        }
        #endregion

        public DelegateCommand<object> UnloadedCommand => new DelegateCommand<object>(arg => eventAggregator.GetEvent<ShellWindowOpacityEvent>().Publish(1));
    }
}
