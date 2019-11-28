
namespace PEF.Modules.SGDE.Models
{
    using Prism.Mvvm;

    public class ScheduleModel : BindableBase
    {
        private string roomId;

        public string RoomId
        {
            get => roomId;
            set => SetProperty(ref roomId, value);
        }

        private string sequence;

        public string Sequence
        {
            get => sequence;
            set => SetProperty(ref sequence, value);
        }

        private string surgeryName;

        public string SurgeryName
        {
            get => surgeryName;
            set => SetProperty(ref surgeryName, value);
        }

        private string surgeryMember;

        public string SurgeryMember
        {
            get => surgeryMember;
            set => SetProperty(ref surgeryMember, value);
        }

        private string patient;

        public string Patient
        {
            get => patient;
            set => SetProperty(ref patient, value);
        }
    }
}
