
namespace PEF.Logger
{
    using System;

    /// <summary>
    /// 基于 NLog 组件的 LogManager 类的封装
    /// </summary>
    public class NLogWrapper
    {
        private NLogWrapper() { }

        private static readonly Lazy<NLogWrapper> instance = new Lazy<NLogWrapper>(() => new NLogWrapper());

        public static NLogWrapper Instance => instance.Value;

        private NLog.Logger Logger => NLog.LogManager.GetCurrentClassLogger();

        //private NLog.Logger errorLogger => NLog.LogManager.GetLogger("ErrorLogger");

        public void Trace<TArg>(TArg arg) => Logger.Trace(arg);

        public void Debug<TArg>(TArg arg) => Logger.Debug(arg);

        public void Info<TArg>(TArg arg) => Logger.Info(arg);

        public void Error<TArg>(TArg arg) => Logger.Error(arg);

        public void Fatal<TArg>(TArg arg) => Logger.Fatal(arg);
    }
}
