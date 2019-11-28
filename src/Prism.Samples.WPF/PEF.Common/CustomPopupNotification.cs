
namespace PEF.Common
{
    using Prism.Interactivity.InteractionRequest;

    public interface ICustomPopupNotification<TData> : IConfirmation
    {
        TData Result { get; set; }
    }

    public class CustomPopupNotification : Confirmation, ICustomPopupNotification<string>
    {
        public string Result { get; set; }
    }
}
