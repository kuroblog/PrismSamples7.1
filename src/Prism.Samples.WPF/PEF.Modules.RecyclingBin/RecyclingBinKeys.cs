
namespace PEF.Modules.RecyclingBin
{
    using PEF.Common;

    /// <summary>
    /// 回收桶模块的窗体间跳转时，要传递的参数的 Key
    /// </summary>
    public class RecyclingBinKeys : NavigationKeys
    {
        /// <summary>
        /// 跳转前的 View 名称
        /// </summary>
        public const string CallbackViewName = nameof(CallbackViewName);

        /// <summary>
        /// 用户名
        /// </summary>
        public const string UserName = nameof(UserName);

        /// <summary>
        /// 用户的卡 Id
        /// </summary>
        public const string UserCardId = nameof(UserCardId);
    }
}
