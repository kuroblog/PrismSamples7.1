
namespace PEF.Modules.LogView.Models
{
    using Prism.Mvvm;
    using System;

    public class LogModel : BindableBase
    {
        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { SetProperty(ref date, value); }
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string level;

        public string Level
        {
            get { return level; }
            set { SetProperty(ref level, value); }
        }

        private string trace;

        public string Trace
        {
            get { return trace; }
            set { SetProperty(ref trace, value); }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }
    }
}
