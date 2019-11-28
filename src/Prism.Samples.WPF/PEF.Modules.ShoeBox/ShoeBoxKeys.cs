
namespace PEF.Modules.ShoeBox
{
    using PEF.Common;

    /// <summary>
    /// 模块的窗体间跳转时，要传递的参数的 Key
    /// </summary>
    public class ShoeBoxKeys : NavigationKeys
    {
        /// <summary>
        /// 跳转前的 View 名称
        /// </summary>
        public const string CallbackViewName = nameof(CallbackViewName);

        public const string UserName = nameof(UserName);

        public const string DeviceItemCode = nameof(DeviceItemCode);
    }
}
