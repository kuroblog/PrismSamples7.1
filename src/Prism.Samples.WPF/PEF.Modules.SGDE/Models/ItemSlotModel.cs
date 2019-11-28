
namespace PEF.Modules.SGDE.Models
{
    using Prism.Mvvm;

    public class ItemSlotModel : BindableBase
    {
        private long slotId;

        public long SlotId
        {
            get => slotId;
            set => SetProperty(ref slotId, value);
        }

        private int x;

        public int X
        {
            get => x;
            set => SetProperty(ref x, value);
        }

        private int y;

        public int Y
        {
            get => y;
            set => SetProperty(ref y, value);
        }
    }
}
