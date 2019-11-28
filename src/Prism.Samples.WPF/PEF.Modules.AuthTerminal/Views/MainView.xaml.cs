
namespace PEF.Modules.AuthTerminal.Views
{
    using System.Windows.Controls;

    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //(DataContext as PEF.Modules.ShoeBox.MainViewModel).CustomNotificationRequest.Raise(new Prism.Interactivity.InteractionRequest.Notification
            //{
            //    Title = "error",
            //    Content = "123333"
            //}, result =>
            //{
            //});

            //(DataContext as PEF.Modules.ShoeBox.MainViewModel).eventAggregator.GetEvent<PubSubEvents.ShoeBoxMessagePopupEvent>().Publish(new Common.PubSubEvents.PopupEventArg<Prism.Interactivity.InteractionRequest.INotification>
            //{
            //    Title = "温馨提示",
            //    Content = "对不起，您没有权限！",
            //    Callback = new System.Action<Prism.Interactivity.InteractionRequest.INotification>(res =>
            //    {

            //    })
            //});

            //(DataContext as PEF.Modules.ShoeBox.MainViewModel).eventAggregator.GetEvent<PubSubEvents.ShoeBoxConfirmPopupEvent>().Publish(new Common.PubSubEvents.PopupEventArg<Prism.Interactivity.InteractionRequest.IConfirmation>
            //{
            //    Title = "温馨提示",
            //    Content = $"{25}号已经被陈豪占用{System.Environment.NewLine}确认是否打开？",
            //    Callback = new System.Action<Prism.Interactivity.InteractionRequest.IConfirmation>(res =>
            //    {

            //    })
            //});
        }
    }
}
