
namespace PEF.Modules.LogView.ViewModels
{
    using PEF.Common.Extensions;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.LogView.Models;
    using Prism.Commands;
    using Prism.Mvvm;
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows;

    public partial class MainViewModel : BindableBase
    {
        private readonly ILogger logger = null;
        private readonly LogViewConfig config = null;

        public MainViewModel(ILogger logger, LogViewConfig config)
        {
            this.logger = logger;
            this.config = config;
        }

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(arg =>
        {
            //Logs.Add(new LogModel
            //{
            //    Date = DateTime.Now,
            //    Id = 999,
            //    Level = "debug",
            //    Message = "12333",
            //    Trace = "no trace"
            //});
        });

        public string Version => config.Version;

        public ObservableCollection<LogModel> Logs { get; set; } = new ObservableCollection<LogModel>();

        public DelegateCommand<DragEventArgs> LogsDragOverCommand => new DelegateCommand<DragEventArgs>(arg =>
        {
            //if (!(arg is DragEventArgs e))
            //{
            //    return;
            //}

            //if (!(e is  files))
            //{

            //}

            //var e = arg as DragEventArgs;
            //if (e == null)
            //{
            //    return;
            //}
            //var s = e.Data.GetData(DataFormats.FileDrop);

            if (arg == null)
            {
                return;
            }

            if (!(arg.Data.GetData(DataFormats.FileDrop) is string[] file) || file.Length == 0)
            {
                return;
            }

            var path = file[0];
            var txt = File.ReadAllText(path);
            var txts = txt.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            txts.ToList().ForEach(p => Logs.Add(p.GetJsonObject<LogModel>()));

            Logs.OrderByDescending(p => p.Date);
        });


        public DelegateCommand CleanCommand => new DelegateCommand(() =>
        {
            Logs.Clear();
        });
    }
}
