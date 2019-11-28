
namespace PEF.Modules.SGDE.Models
{
    using Prism.Mvvm;

    public class PersonModel : BindableBase
    {
        private long userId;

        public long UserId
        {
            get => userId;
            set => SetProperty(ref userId, value);
        }

        private string userName;

        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        private CardModel card;

        public CardModel Card
        {
            get => card;
            set => SetProperty(ref card, value);
        }
    }
}
