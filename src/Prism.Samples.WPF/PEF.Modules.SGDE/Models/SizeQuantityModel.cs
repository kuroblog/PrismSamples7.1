
namespace PEF.Modules.SGDE.Models
{
    using Prism.Mvvm;

    public class SizeQuantityModel : BindableBase
    {
        private string size;

        public string Size
        {
            get => size;
            set => SetProperty(ref size, value);
        }

        private string quantity;

        public string Quantity
        {
            //get => quantity == "0" ? string.Empty : quantity;
            get => quantity;
            set => SetProperty(ref quantity, value);
        }

        private double width;

        public double Width
        {
            get => width;
            set => SetProperty(ref width, value);
        }

        //private double height;

        //public double Height
        //{
        //    get => height;
        //    set => SetProperty(ref height, value);
        //}
    }
}
