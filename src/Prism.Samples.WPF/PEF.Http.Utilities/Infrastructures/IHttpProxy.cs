
namespace PEF.Http.Utilities.Infrastructures
{
    using System.Collections.Generic;
    using System.Net.Http;

    /// <summary>
    /// HTTP 代理接口
    /// </summary>
    public interface IHttpProxy
    {
        /// <summary>
        /// 以 Get 方式访问
        /// </summary>
        /// <param name="host"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        HttpResponseMessage Get(string host, string action);

        /// <summary>
        /// 以 Post 方式访问
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="host"></param>
        /// <param name="action"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        HttpResponseMessage Post<TData>(string host, string action, TData data);
    }
}
