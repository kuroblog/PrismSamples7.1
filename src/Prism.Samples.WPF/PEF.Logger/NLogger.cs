
namespace PEF.Logger
{
    using PEF.Logger.Infrastructures;

    /// <summary>
    /// 日志类
    /// </summary>
    public class NLogger : ILogger
    {
        /// <summary>
        /// Trace 级别的日志
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="arg"></param>
        public void Trace<TArg>(TArg arg) => NLogWrapper.Instance.Trace(arg);

        /// <summary>
        /// Debug 级别的日志
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="arg"></param>
        public void Debug<TArg>(TArg arg) => NLogWrapper.Instance.Trace(arg);

        /// <summary>
        /// Info 级别的日志
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="arg"></param>
        public void Info<TArg>(TArg arg) => NLogWrapper.Instance.Info(arg);

        /// <summary>
        /// Error 级别的日志
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="arg"></param>
        public void Error<TArg>(TArg arg) => NLogWrapper.Instance.Error(arg);

        /// <summary>
        /// Fatal 级别的日志
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="arg"></param>
        public void Fatal<TArg>(TArg arg) => NLogWrapper.Instance.Fatal(arg);
    }
}
