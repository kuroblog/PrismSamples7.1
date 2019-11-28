
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

    public partial class TestViewModel : BindableBase
    {
    }
}

namespace PEF.Modules.LogView
{
    public class TestParameters
    {
        public static string TestValue { get; set; }

        //public 
    }
}
