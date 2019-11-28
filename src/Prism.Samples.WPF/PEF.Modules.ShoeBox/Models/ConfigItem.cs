
namespace PEF.Modules.ShoeBox.Models
{
    using PEF.Modules.ShoeBox.Controls;
    using Prism.Mvvm;

    public class ConfigItem : BindableBase
    {
        private long itemId;

        public long ItemId
        {
            get => itemId;
            set => SetProperty(ref itemId, value);
        }

        private int itemNo;

        public int ItemNo
        {
            get => itemNo;
            set => SetProperty(ref itemNo, value);
        }

        private string itemCode;

        public string ItemCode
        {
            get => itemCode;
            set => SetProperty(ref itemCode, value);
        }

        private string userName;

        public string UserName
        {
            get => string.IsNullOrEmpty(userName) ? "--" : userName;
            set => SetProperty(ref userName, value);
        }

        private ConfigState state;

        public ConfigState State
        {
            get => state;
            set => SetProperty(ref state, value);
        }
    }
}
