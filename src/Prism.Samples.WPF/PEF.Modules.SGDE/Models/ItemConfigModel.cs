
namespace PEF.Modules.SGDE.Models
{
    public class ItemSlotConfigModel : ItemSlotModel
    {
        private string size;

        public string Size
        {
            get => size;
            set => SetProperty(ref size, value);
        }

        public string Title => $"{X}-{Y}";

        private int stock;

        public int Stock
        {
            get => stock;
            set
            {
                SetProperty(ref stock, value);
                RaisePropertyChanged(nameof(DisplayValue));
            }
        }

        private int limit;

        public int Limit
        {
            get => limit;
            set
            {
                SetProperty(ref limit, value);
                RaisePropertyChanged(nameof(DisplayValue));
            }
        }

        public string DisplayValue => $"{Stock}/{Limit}";
    }
}
