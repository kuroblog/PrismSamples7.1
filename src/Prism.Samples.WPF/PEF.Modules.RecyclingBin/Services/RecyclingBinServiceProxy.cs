
namespace PEF.Modules.RecyclingBin.Services
{
    using PEF.Common.Extensions;
    using PEF.Http.Utilities;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.RecyclingBin.Services.Dtos;
    using System;

    /// <summary>
    /// 回收桶服务代理接口
    /// </summary>
    public interface IRecyclingBinServiceProxy
    {
        /// <summary>
        /// 卡查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CardQueryResponse CardQuery(CardQueryRequest request);

        /// <summary>
        /// 回收桶查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        RecyclingBinQueryResponse RecyclingBinQuery(RecyclingBinQueryRequest request);

        /// <summary>
        /// 回收桶提交
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        RecyclingBinSubmitResponse RecyclingBinSubmit(RecyclingBinSubmitRequest request);

        /// <summary>
        /// 回收桶清空
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        RecyclingBinCleanResponse RecyclingBinClean(RecyclingBinCleanRequest request);

        ///// <summary>
        ///// 管理员验证
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //RecyclingBinAdminVerifyResponse RecyclingBinAdminVerify(RecyclingBinAdminVerifyRequest request);

        /// <summary>
        /// Socket 状态报告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SocketStatusReportResponse SocketStatusReport(SocketStatusReportRequest request);

        TokenResponse TokenRequest(TokenRequest request);

        void LogReport(LogReportRequest request);
    }

    /// <summary>
    /// 回收桶服务代理类
    /// </summary>
    public class RecyclingBinServiceProxy : IRecyclingBinServiceProxy
    {
        private readonly ILogger logger = null;
        private readonly RecyclingBinConfig config = null;
        private readonly HttpClientProxy proxy = null;

        public RecyclingBinServiceProxy(ILogger logger, RecyclingBinConfig config, HttpClientProxy proxy)
        {
            this.logger = logger;
            this.config = config;
            this.proxy = proxy;
        }

        protected virtual TResponse PostInvoke<TRequest, TResponse>(string actionUrl, TRequest request)
        {
            var response = proxy.Post(config.ServiceUrl, actionUrl, request);
            if (!response.IsSuccessStatusCode)
            {
                return default(TResponse);
            }

            var json = response.Content.ReadAsStringAsync().Result;

            return json.GetJsonObject<TResponse>();
        }

        protected virtual TResponse PostInvokeV2<TRequest, TResponse>(string actionUrl, TRequest request, bool errorReport = true)
        {
            TResponse result;

            var response = proxy.Post(config.ServiceUrl, actionUrl, request);
            if (!response.IsSuccessStatusCode)
            {
                result = Activator.CreateInstance<TResponse>();
                (result as ServiceBaseDto).Code = response.StatusCode.ToString();
                (result as ServiceBaseDto).Message = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                var json = response.Content.ReadAsStringAsync().Result;
                result = json.GetJsonObject<TResponse>();
            }

            return result;
        }

        public CardQueryResponse CardQuery(CardQueryRequest request) => PostInvokeV2<CardQueryRequest, CardQueryResponse>(config.CardQueryUrl, request);

        public RecyclingBinQueryResponse RecyclingBinQuery(RecyclingBinQueryRequest request) =>
            PostInvokeV2<RecyclingBinQueryRequest, RecyclingBinQueryResponse>(config.RecyclingBinQueryUrl, request);

        public RecyclingBinSubmitResponse RecyclingBinSubmit(RecyclingBinSubmitRequest request) =>
            PostInvokeV2<RecyclingBinSubmitRequest, RecyclingBinSubmitResponse>(config.RecyclingBinSubmitUrl, request);

        public RecyclingBinCleanResponse RecyclingBinClean(RecyclingBinCleanRequest request) =>
            PostInvokeV2<RecyclingBinCleanRequest, RecyclingBinCleanResponse>(config.RecyclingBinCleanUrl, request);

        //public RecyclingBinAdminVerifyResponse RecyclingBinAdminVerify(RecyclingBinAdminVerifyRequest request) =>
        //    Invoke<RecyclingBinAdminVerifyRequest, RecyclingBinAdminVerifyResponse>(config.CardQueryActionUrlRecyclingBinAdminVerifyActionUrl, request);

        public SocketStatusReportResponse SocketStatusReport(SocketStatusReportRequest request) => PostInvokeV2<SocketStatusReportRequest, SocketStatusReportResponse>(config.SocketStatusReportUrl, request);

        public TokenResponse TokenRequest(TokenRequest request) => PostInvokeV2<TokenRequest, TokenResponse>(config.TokenRequestUrl, request, false);

        public void LogReport(LogReportRequest request) => PostInvokeV2<LogReportRequest, dynamic>(config.LogReportUrl, request, false);
    }
}
