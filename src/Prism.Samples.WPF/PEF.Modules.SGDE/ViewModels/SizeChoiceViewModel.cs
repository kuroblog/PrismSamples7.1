
namespace PEF.Modules.SGDE.ViewModels
{
    using PEF.Common;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.SGDE.Models;
    using PEF.Modules.SGDE.Views;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Regions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public partial class SizeChoiceViewModel : BindableBase, INavigationAware
    {
        private readonly SGDEConfig config = null;
        //private readonly ISGDEDeviceProxy deviceProxy = null;
        //private readonly ISGDEServiceProxy serviceProxy = null;
        private readonly IRegionManager region = null;
        private readonly ILogger logger = null;

        public SizeChoiceViewModel(
            SGDEConfig config,
            //ISGDEDeviceProxy deviceProxy,
            //ISGDEServiceProxy serviceProxy,
            IRegionManager region,
            ILogger logger)
        {
            this.config = config;
            //this.deviceProxy = deviceProxy;
            //this.serviceProxy = serviceProxy;
            this.region = region;
            this.logger = logger;
        }

        private SizeQuantityModel sqItemI;

        public SizeQuantityModel SqItemI
        {
            get => sqItemI;
            set => SetProperty(ref sqItemI, value);
        }

        private SizeQuantityModel sqItemII;

        public SizeQuantityModel SqItemII
        {
            get => sqItemII;
            set => SetProperty(ref sqItemII, value);
        }

        private SizeQuantityModel sqItemIII;

        public SizeQuantityModel SqItemIII
        {
            get => sqItemIII;
            set => SetProperty(ref sqItemIII, value);
        }

        private SizeQuantityModel sqItemIV;

        public SizeQuantityModel SqItemIV
        {
            get => sqItemIV;
            set => SetProperty(ref sqItemIV, value);
        }

        private SizeQuantityModel sqItemV;

        public SizeQuantityModel SqItemV
        {
            get => sqItemV;
            set => SetProperty(ref sqItemV, value);
        }

        private SizeQuantityModel sqItemVI;

        public SizeQuantityModel SqItemVI
        {
            get => sqItemVI;
            set => SetProperty(ref sqItemVI, value);
        }

        private void RefreshItems()
        {
            //var counter = sizeItems.Count() < 2 ? 2 : sizeItems.Count();
            //var width = (stepsImage.ActualWidth - counter * 2 * 2) / counter;

            //if (sizeItems.Count == 0)
            //{
            //    SqItemI = new SizeQuantityModel
            //    {
            //        Width = width
            //    };
            //}
            //else
            //{
            //    SqItemI = indexOfSizeQueryItems(sizeItems, 0, width);
            //}

            //SqItemII = indexOfSizeQueryItems(sizeItems, 1, width);
            //SqItemIII = indexOfSizeQueryItems(sizeItems, 2, width);
            //SqItemIV = indexOfSizeQueryItems(sizeItems, 3, width);
            //SqItemV = indexOfSizeQueryItems(sizeItems, 4, width);
            //SqItemVI = indexOfSizeQueryItems(sizeItems, 5, width);

            var width = (imageBorder.ActualWidth - 59 * 2 - 2 * 2 * 5) / 2;

            if (SqItemI == null)
            {
                SqItemI = new SizeQuantityModel();
            }
            SqItemI.Width = width;

            if (SqItemIV == null)
            {
                SqItemIV = new SizeQuantityModel();
            }
            SqItemIV.Width = width;

            if (SqItemII == null)
            {
                SqItemII = new SizeQuantityModel();
            }
            SqItemII.Width = width;

            if (SqItemV == null)
            {
                SqItemV = new SizeQuantityModel();
            }
            SqItemV.Width = width;

            if (SqItemIII == null)
            {
                SqItemIII = new SizeQuantityModel();
            }
            SqItemIII.Width = width;

            if (SqItemVI == null)
            {
                SqItemVI = new SizeQuantityModel();
            }
            SqItemVI.Width = width;

            if (sizeItems.Count(p => string.IsNullOrEmpty(p.Size) == false) == 4)
            {
                SqItemV.Size = SqItemIII.Size;
                SqItemV.Quantity = SqItemIII.Quantity;

                SqItemIII = new SizeQuantityModel();
            }
        }

        private Border imageBorder;
        private RowDefinition footRow;

        public DelegateCommand<RoutedEventArgs> LoadedCommand => new DelegateCommand<RoutedEventArgs>(arg =>
        {
            var view = arg.Source as SizeChoiceView;
            imageBorder = view.ImageBorder;
            footRow = view.FootRow;

            view.SizeChanged += (s, e) => RefreshItems();

            RefreshItems();
        });

        public DelegateCommand<object> SizeChoiceCommand => new DelegateCommand<object>(size =>
        {
            region?.RequestNavigate(
                RegionNames.Home,
                typeof(HomeView).FullName,
                new NavigationParameters {
                    { SGDEKeys.CallbackViewName, typeof(SizeChoiceView).FullName },
                    { SGDEKeys.SelectedSize, size } });
        });

        public DelegateCommand ReturnCommand => new DelegateCommand(() => region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName));

        private PersonModel person = new PersonModel();
        private List<SizeQuantityModel> sizeItems = new List<SizeQuantityModel>();

        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            person = new PersonModel { };
            var userIdParam = navigationContext.Parameters[SGDEKeys.UserId];
            if (userIdParam != null)
            {
                person.UserId = long.TryParse(userIdParam.ToString(), out long userId) ? userId : 0;
            }

            SqItemI = navigationContext.Parameters[SGDEKeys.SizeQuantityI] as SizeQuantityModel;
            SqItemII = navigationContext.Parameters[SGDEKeys.SizeQuantityII] as SizeQuantityModel;
            SqItemIII = navigationContext.Parameters[SGDEKeys.SizeQuantityIII] as SizeQuantityModel;
            SqItemIV = navigationContext.Parameters[SGDEKeys.SizeQuantityIV] as SizeQuantityModel;
            SqItemV = navigationContext.Parameters[SGDEKeys.SizeQuantityV] as SizeQuantityModel;
            SqItemVI = navigationContext.Parameters[SGDEKeys.SizeQuantityVI] as SizeQuantityModel;

            sizeItems.Add(SqItemI);
            sizeItems.Add(SqItemII);
            sizeItems.Add(SqItemIII);
            sizeItems.Add(SqItemIV);
            sizeItems.Add(SqItemV);
            sizeItems.Add(SqItemVI);
        }
        #endregion
    }
}
