
namespace PEF.Shell
{
    using PEF.Common;
    using PEF.Common.Extensions;
    using PEF.Http.Utilities;
    using PEF.Logger;
    using PEF.Logger.Infrastructures;
    using PEF.Socket.Utilities;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Mvvm;
    using Prism.Unity;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// 使用 Prism 框架的 PrismApplication 类
    /// </summary>
    public partial class App : PrismApplication
    {
        #region PrismApplication
        /// <summary>
        /// 创建容器窗体
        /// </summary>
        /// <returns></returns>
        protected override Window CreateShell()
        {
            return Container.Resolve<ShellWindow>();
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ShellConfig>();
            containerRegistry.RegisterSingleton<ILogger, NLogger>();

            //containerRegistry.RegisterSingleton<SocketClientProxy>();
            // socket 改用 web-socket
            containerRegistry.RegisterSingleton<WebSocketClientProxy>();

            var logger = Container.Resolve<ILogger>();
            containerRegistry.RegisterInstance(new LoggingHandler(logger));

            var loggingHandler = Container.Resolve<LoggingHandler>();

            containerRegistry.RegisterInstance(new HttpRequestParameters());
            var httpParameters = Container.Resolve<HttpRequestParameters>();

            containerRegistry.RegisterInstance(new HttpClientProxy(logger, loggingHandler, httpParameters));

            //containerRegistry.RegisterSingleton<ConfigManager>();
        }

        ///// <summary>
        ///// 重写 View & ViewMmodel 的映射规则
        ///// or
        ///// 在 View 的 Xaml 视图中启用 prism:ViewModelLocator.AutoWireViewModel="True"
        ///// or
        ///// 在 IModule 的实现类里的 OnInitialized 方法中直接注册 View 和 ViewModel
        ///// </summary>
        //protected override void ConfigureViewModelLocator()
        //{
        //    base.ConfigureViewModelLocator();

        //    // 默认: Xxx => XxxViewModel
        //    ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
        //    {
        //        var vName = viewType.FullName;
        //        var aName = viewType.GetTypeInfo().Assembly.FullName;
        //        var vmName = $"{vName}ViewModel, {aName}";
        //        return Type.GetType(vmName);
        //    });
        //}

        /// <summary>
        /// 重写模块的加载方式，此处使用单个文件夹
        /// </summary>
        /// <remarks>
        /// TODO
        /// 1) 通过继承 DirectoryModuleCatalog 类和 IModuleCatalog 接口来实现从多个文件夹加载模块
        /// </remarks>
        /// <returns></returns>
        protected override IModuleCatalog CreateModuleCatalog()
        {
            //return base.CreateModuleCatalog();

            //return new DirectoryModuleCatalog { ModulePath = @".\Modules" };

            return new CustomModuleCatalog { ModulePath = @".\Modules" };
        }
        #endregion

        private ILogger Logger => Container.Resolve<ILogger>();

        /// <summary>
        /// 程序初始化设置
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                ShutdownMode = ShutdownMode.OnMainWindowClose;

                //DispatcherUnhandledException += (obj, error) => Logger.Fatal(error);
                DispatcherUnhandledException += DispatcherUnhandledExceptionHandler;

                //AppDomain.CurrentDomain.UnhandledException += (obj, error) => Logger.Fatal(error);
                AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

                //TaskScheduler.UnobservedTaskException += (obj, error) => Logger.Fatal(error);
                TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionHandler;

                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
            }
        }

        private void UnobservedTaskExceptionHandler(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Logger.Fatal(e.Exception.GetJsonString());
            //MessageBox.Show(e.Exception.GetJsonString());
        }

        private void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Fatal(e.ExceptionObject.GetJsonString());
            //MessageBox.Show(e.ExceptionObject.GetJsonString());
        }

        private void DispatcherUnhandledExceptionHandler(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Fatal(e.GetJsonString());
            //MessageBox.Show(e.Exception.GetJsonString());
        }
    }
}
