
namespace PEF.Modules.AuthTerminal.Models
{
    using Prism.Mvvm;

    public class RoleModel : BindableBase
    {
        private long roleId;

        public long RoleId
        {
            get => roleId;
            set => SetProperty(ref roleId, value);
        }

        private string roleName;

        public string RoleName
        {
            get => roleName;
            set
            {
                SetProperty(ref roleName, value);
            }
        }
    }
}
