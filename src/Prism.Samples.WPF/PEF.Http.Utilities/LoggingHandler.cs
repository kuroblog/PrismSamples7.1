
namespace PEF.Http.Utilities
{
    using PEF.Logger.Infrastructures;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// HTTP 日志处理类
    /// </summary>
    /// <remarks>
    /// TODO
    /// 1) 使用异步没有正常返回
    /// 2) 第一次访问失败，第二次HttpClientFactory.Create(loggingHandler)这里会出错
    /// </remarks>
    public class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger logger = null;

        public LoggingHandler(ILogger logger)
        {
            this.logger = logger;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        //protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                logger.Info(new { request.RequestUri, request.Headers, request.Content?.ReadAsStringAsync().Result });

                var response = base.SendAsync(request, cancellationToken).Result;
                //var response = await base.SendAsync(request, cancellationToken);

                logger.Info(response.ToString());

                if (response.IsSuccessStatusCode)
                {
                    logger.Info(response.Content?.ReadAsStringAsync().Result);
                }

                return Task.FromResult(response);
                //return await Task.FromResult(response);
            }
            catch (Exception)
            {

            }

            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode =
                HttpStatusCode.InternalServerError
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO: release unmanaged resources
            }

            base.Dispose(disposing);
        }
    }
}
