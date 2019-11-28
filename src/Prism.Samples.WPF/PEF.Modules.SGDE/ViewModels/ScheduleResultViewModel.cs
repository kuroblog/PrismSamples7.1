
namespace PEF.Modules.SGDE.ViewModels
{
    using PEF.Common;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.SGDE.Models;
    using PEF.Modules.SGDE.Services.Dtos;
    using PEF.Modules.SGDE.Views;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Regions;
    using Prism.Unity;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;

    public partial class ScheduleResultViewModel : BindableBase, INavigationAware
    {
        private readonly SGDEConfig config = null;
        private readonly IRegionManager region = null;
        private readonly ILogger logger = null;

        private string userName;

        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        private string boxId;

        public string BoxId
        {
            get => boxId;
            set => SetProperty(ref boxId, value);
        }

        private ObservableCollection<ScheduleModel> schedules = new ObservableCollection<ScheduleModel>();

        public ObservableCollection<ScheduleModel> Schedules
        {
            get => schedules;
            set => SetProperty(ref schedules, value);
        }

        public ScheduleResultViewModel(
            SGDEConfig config,
            IRegionManager region,
            ILogger logger)
        {
            this.config = config;
            this.region = region;
            this.logger = logger;

            //UserName = "富强民主";
            //BoxId = "99";

            //Schedules.Add(new ScheduleModel { RoomId = "A11", Sequence = 1, SurgeryName = "测试1", SurgeryMember = "测试", Patient = "测试" });
            //Schedules.Add(new ScheduleModel { RoomId = "A07", Sequence = 2, SurgeryName = "测试1测试2", SurgeryMember = "测试测试", Patient = "测试测试" });
            //Schedules.Add(new ScheduleModel { RoomId = "B01", Sequence = 3, SurgeryName = "测试", SurgeryMember = "测试", Patient = "测试" });
            //Schedules.Add(new ScheduleModel { RoomId = "A10", Sequence = 9, SurgeryName = "测试1测试2测试3", SurgeryMember = "测试", Patient = "测试" });
            //Schedules.Add(new ScheduleModel { RoomId = "C01", Sequence = 11, SurgeryName = "测试", SurgeryMember = "测试", Patient = "测试" });
            //Schedules.Add(new ScheduleModel { RoomId = "C99", Sequence = 20, SurgeryName = "测试1测试2测试3测试4", SurgeryMember = "测试", Patient = "测试" });
            //Schedules.Add(new ScheduleModel { RoomId = "A11", Sequence = 31, SurgeryName = "测试", SurgeryMember = "测试", Patient = "测试" });
            //Schedules.Add(new ScheduleModel { RoomId = "A11", Sequence = 99, SurgeryName = "测试1测试2测试3测试4测试5测试6测试7测试8测试9", SurgeryMember = "测试", Patient = "测试" });
        }

        #region thread sync test
        //private void MainDispatcherInvoke(Action action)
        //{
        //    var app = Application.Current as PrismApplication;
        //    app.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(action));
        //}

        //private void AutoReturnHomeView(int counter = 6)
        //{
        //    Task.Factory.StartNew(() =>
        //    {
        //        for (; ; )
        //        {
        //            //if (ct.IsCancellationRequested || counter == 0)
        //            if (counter == 0)
        //            {
        //                MainDispatcherInvoke(() =>
        //                {
        //                    region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName);
        //                });

        //                break;
        //            };

        //            Task.Delay(1000).Wait();
        //            counter--;
        //        }
        //    });
        //}

        private void AutoReturnHomeView(int counter = 6)
        {
            Task.Factory.StartNew(() =>
            {
                for (; ; )
                {
                    //if (ct.IsCancellationRequested || counter == 0)
                    if (counter == 0)
                    {
                        MainDispatcher.Value.Invoke(() =>
                        {
                            region?.RequestNavigate(RegionNames.Home, typeof(HomeView).FullName);
                        });

                        break;
                    };

                    Task.Delay(1000).Wait();
                    counter--;
                }
            });
        }
        #endregion

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg => AutoReturnHomeView());

        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            UserName = navigationContext.Parameters[SGDEKeys.UserName] as string;
            BoxId = navigationContext.Parameters[SGDEKeys.SaveBoxNo] as string;

            Schedules.Clear();
            if (navigationContext.Parameters[SGDEKeys.UserSchedules] is ApplyQuerySchedule[] userSchedules && userSchedules.Any())
            {
                Schedules = new ObservableCollection<ScheduleModel>(userSchedules.Select(p => new ScheduleModel
                {
                    Patient = p.Patient,
                    RoomId = p.Room,
                    Sequence = p.Sequence,
                    SurgeryMember = p.SurgeryMember,
                    SurgeryName = p.SurgeryName
                }).ToArray());
            }
        }
        #endregion
    }
}
