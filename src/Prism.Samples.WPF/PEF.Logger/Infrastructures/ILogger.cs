
namespace PEF.Logger.Infrastructures
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILogger
    {
        //void Dump<TObj>(TObj obj, bool isFormat = false);

        //void DumpError(Exception ex);

        /// <summary>
        /// Trace 级别的日志
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="arg"></param>
        void Trace<TArg>(TArg arg);

        /// <summary>
        /// Debug 级别的日志
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="arg"></param>
        void Debug<TArg>(TArg arg);

        /// <summary>
        /// Info 级别的日志
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="arg"></param>
        void Info<TArg>(TArg arg);

        /// <summary>
        /// Error 级别的日志
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="arg"></param>
        void Error<TArg>(TArg arg);

        /// <summary>
        /// Fatal 级别的日志
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="arg"></param>
        void Fatal<TArg>(TArg arg);
    }
}
