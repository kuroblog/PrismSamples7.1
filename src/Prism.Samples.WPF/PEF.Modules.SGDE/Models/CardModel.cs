
namespace PEF.Modules.SGDE.Models
{
    using Prism.Mvvm;

    public class CardModel : BindableBase
    {
        private string cardId;

        public string CardId
        {
            get => cardId;
            set => SetProperty(ref cardId, value);
        }
    }
}
