
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
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;

    public class CustomConfirmationWindowViewModel : BindableBase, IInteractionRequestAware
    {
        private readonly IEventAggregator eventAggregator;
        private IConfirmation confirmation;

        public CustomConfirmationWindowViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public INotification Notification
        {
            get => confirmation;
            set => SetProperty(ref confirmation, (IConfirmation)value);
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand ConfirmCommand => new DelegateCommand(() =>
        {
            //confirmation.Result = nameof(ConfirmCommand);
            confirmation.Confirmed = true;
            //FinishInteraction?.Invoke();
            tokenSource.Cancel();
        });

        public DelegateCommand CancelCommand => new DelegateCommand(() =>
        {
            //confirmation.Result = nameof(CancelCommand);
            confirmation.Confirmed = false;
            tokenSource.Cancel();
            //FinishInteraction?.Invoke();
        });

        private string confirmTitle;

        public string ConfirmTitle
        {
            get => confirmTitle;
            set => SetProperty(ref confirmTitle, value);
        }

        private string cancelTitle;

        public string CancelTitle
        {
            get => cancelTitle;
            set => SetProperty(ref cancelTitle, value);
        }

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            ConfirmTitle = "确定";
            CancelTitle = "取消";

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
        //                    //CancelTitle = "取消";
        //                    //CancelCommand.Execute();

        //                    FinishInteraction?.Invoke();
        //                });

        //                break;
        //            };

        //            MainDispatcherInvoke(() => CancelTitle = $"取消 ({counter})");

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
                            //CancelTitle = "取消";
                            //CancelCommand.Execute();

                            FinishInteraction?.Invoke();
                        });

                        break;
                    };

                    MainDispatcher.Value.Invoke(() => CancelTitle = $"取消 ({counter})");

                    Task.Delay(1000).Wait();
                    counter--;
                }
            });
        }
        #endregion

        public DelegateCommand<object> UnloadedCommand => new DelegateCommand<object>(arg => eventAggregator.GetEvent<ShellWindowOpacityEvent>().Publish(1));
    }
}
