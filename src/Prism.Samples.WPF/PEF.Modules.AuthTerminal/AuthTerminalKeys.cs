
namespace PEF.Modules.AuthTerminal
{
    using PEF.Common;

    /// <summary>
    /// 发衣柜模块的窗体间跳转时，要传递的参数的 Key
    /// </summary>
    public class AuthTerminalKeys : NavigationKeys
    {
        /// <summary>
        /// 跳转前的 View 名称
        /// </summary>
        public const string CallbackViewName = nameof(CallbackViewName);

        public const string CardId = nameof(CardId);

        public const string UserRoleBindingResult = nameof(UserRoleBindingResult);

        public const string GuestReadStyle = nameof(GuestReadStyle);

        ///// <summary>
        ///// 用户Id
        ///// </summary>
        //public const string UserId = nameof(UserId);

        ///// <summary>
        ///// 用户名
        ///// </summary>
        //public const string UserName = nameof(UserName);

        ///// <summary>
        ///// 存衣柜号
        ///// </summary>
        //public const string SaveBoxNo = nameof(SaveBoxNo);

        ///// <summary>
        ///// 用户排班
        ///// </summary>
        //public const string UserSchedules = nameof(UserSchedules);

        ///// <summary>
        ///// 尺码&数量1
        ///// </summary>
        //public const string SizeQuantityI = nameof(SizeQuantityI);

        ///// <summary>
        ///// 尺码&数量2
        ///// </summary>
        //public const string SizeQuantityII = nameof(SizeQuantityII);

        ///// <summary>
        ///// 用户选择的尺码
        ///// </summary>
        //public const string SelectedSize = nameof(SelectedSize);
    }
}
