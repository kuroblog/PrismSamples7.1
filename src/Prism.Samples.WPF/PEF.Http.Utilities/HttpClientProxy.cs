
namespace PEF.Http.Utilities
{
    using PEF.Common.Extensions;
    using PEF.Http.Utilities.Infrastructures;
    using PEF.Logger.Infrastructures;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.ExceptionServices;
    using System.Text;
    using System.Threading.Tasks;

    public interface IHttpRequestParameters
    {
        Dictionary<string, string> Headers { get; set; }
    }

    public class HttpRequestParameters : IHttpRequestParameters
    {
        public Guid Key { get; } = Guid.NewGuid();

        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    }

    /// <summary>
    /// HTTP 代理类
    /// </summary>
    public class HttpClientProxy : IHttpProxy
    {
        private readonly ILogger logger = null;
        private readonly LoggingHandler loggingHandler = null;
        private readonly HttpRequestParameters httpParameters = null;

        public HttpClientProxy(
            ILogger logger,
            LoggingHandler loggingHandler,
            HttpRequestParameters httpParameters)
        {
            this.logger = logger;
            this.loggingHandler = loggingHandler;
            this.httpParameters = httpParameters;
        }

        private readonly int retryCount = 3;
        private readonly TimeSpan delay = TimeSpan.FromSeconds(1);

        protected virtual HttpResponseMessage InvokeRetry(string host, string action, Func<HttpClient, HttpResponseMessage> method)
        {
            int currentRetry = 0;
            //HttpResponseMessage response = null;

            for (; ; )
            {
                try
                {
                    var proxy = HttpClientFactory.Create();
                    proxy.DefaultRequestHeaders.Clear();
                    if (httpParameters != null && httpParameters.Headers != null && httpParameters.Headers.Any())
                    {
                        httpParameters.Headers.ToList().ForEach(p => proxy.DefaultRequestHeaders.Add(p.Key, p.Value));
                    }

                    var response = method.Invoke(proxy);

                    logger.Info(response.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        logger.Info(response.Content?.ReadAsStringAsync().Result);
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    currentRetry++;

                    logger.Fatal(new
                    {
                        retry = currentRetry,
                        method = "doRetry",
                        error = ex
                    }.GetJsonString());

                    if (currentRetry > retryCount)
                    {
                        //ExceptionDispatchInfo.Capture(ex).Throw();

                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            Content = new StringContent(new { Message = "访问服务端发生错误请与管理员联系。" }.GetJsonString(), Encoding.UTF8, "application/json")
                        };
                    }
                }

                Task.Delay(delay).Wait();
            }
        }

        protected virtual HttpResponseMessage Invoke(string host, string action, Func<HttpClient, HttpResponseMessage> method)
        {
            //var proxy = HttpClientFactory.Create(loggingHandler);

            try
            {
                //logger.Info(new { host, action, method = method.Method.Name, httpParameters.Headers }.GetJsonString());

                var proxy = HttpClientFactory.Create();
                proxy.DefaultRequestHeaders.Clear();
                if (httpParameters != null && httpParameters.Headers != null && httpParameters.Headers.Any())
                {
                    httpParameters.Headers.ToList().ForEach(p => proxy.DefaultRequestHeaders.Add(p.Key, p.Value));
                }

                var response = method.Invoke(proxy);

                logger.Info(response.ToString());
                if (response.IsSuccessStatusCode)
                {
                    logger.Info(response.Content?.ReadAsStringAsync().Result);
                }

                //response.EnsureSuccessStatusCode();

                return response;
            }
            //catch (AggregateException eax)
            //{
            //    ExceptionDispatchInfo.Capture(eax.InnerException).Throw();
            //}
            catch (Exception ex)
            {
                logger.Fatal(ex.GetJsonString());
                //ExceptionDispatchInfo.Capture(ex).Throw();

                //var error = ex is AggregateException ? ex.InnerException != null ? (ex as AggregateException).InnerException.Message : ex.Message : ex.Message;

                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent($"服务不可用，请与管理员联系。{Environment.NewLine}{ex.GetMessages().LastOrDefault()}", Encoding.UTF8, "application/json")
                };
            }

            //return new HttpResponseMessage
            //{
            //    StatusCode = HttpStatusCode.InternalServerError
            //};
        }

        /// <summary>
        /// 以 Get 方式访问
        /// </summary>
        /// <param name="host"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(string host, string action) => Invoke(host, action, proxy =>
        {
            //logger.Info(new { host, action });

            logger.Info(new { path = Path.Combine(host, action), method = "Get", httpParameters.Headers }.GetJsonString());

            var response = proxy.GetAsync(Path.Combine(host, action)).Result;

            //logger.Info(response.ToString());

            //if (response.IsSuccessStatusCode)
            //{
            //    logger.Info(response.Content?.ReadAsStringAsync().Result);
            //}

            return response;
        });

        /// <summary>
        /// 以 Post 方式访问
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="host"></param>
        /// <param name="action"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public HttpResponseMessage Post<TData>(string host, string action, TData data) => Invoke(host, action, proxy =>
        {
            logger.Info(new { path = Path.Combine(host, action), method = "Post", httpParameters.Headers, body = data }.GetJsonString());

            var content = new StringContent(data.GetJsonString(), Encoding.UTF8, "application/json");

            var response = proxy.PostAsync(Path.Combine(host, action), content).Result;

            //logger.Info(response.ToString());

            //if (response.IsSuccessStatusCode)
            //{
            //    logger.Info(response.Content?.ReadAsStringAsync().Result);
            //}

            return response;
        });
    }
}
