
namespace PEF.Modules.SGDE.Services
{
    using PEF.Common.Extensions;
    using PEF.Http.Utilities;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.SGDE.Services.Dtos;
    using System;

    /// <summary>
    /// 发衣柜服务代理接口
    /// </summary>
    public interface ISGDEServiceProxy
    {
        /// <summary>
        /// 卡查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CardQueryResponse CardQuery(CardQueryRequest request);

        /// <summary>
        /// 托盘配置查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ConfigQueryResponse ConfigQuery(ConfigQueryRequest request);

        /// <summary>
        /// 托盘配置上传
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ConfigSubmitResponse ConfigSubmit(ConfigSubmitRequest request);

        /// <summary>
        /// 申请发衣
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApplySubmitResponse ApplySubmit(ApplySubmitRequest request);

        /// <summary>
        /// 申请发衣 ver.2
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApplySubmitV2Response ApplySubmitV2(ApplySubmitRequest request);

        /// <summary>
        /// 尺码查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SizeQueryResponse SizeQuery(SizeQueryRequest request);

        /// <summary>
        /// 发衣申请结果查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApplyQueryResponse ApplyQuery(ApplyQueryRequest request);

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
    /// 发衣柜服务代理类
    /// </summary>
    public class SGDEServiceProxy : ISGDEServiceProxy
    {
        private readonly SGDEConfig config = null;
        private readonly HttpClientProxy proxy = null;
        private readonly ILogger logger = null;

        public SGDEServiceProxy(
            SGDEConfig config,
            HttpClientProxy proxy,
            ILogger logger)
        {
            this.config = config;
            this.proxy = proxy;
            this.logger = logger;
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

        public ConfigQueryResponse ConfigQuery(ConfigQueryRequest request) => PostInvokeV2<ConfigQueryRequest, ConfigQueryResponse>(config.ConfigQueryUrl, request);

        public ConfigSubmitResponse ConfigSubmit(ConfigSubmitRequest request) => PostInvokeV2<ConfigSubmitRequest, ConfigSubmitResponse>(config.ConfigSubmitUrl, request);

        public ApplySubmitResponse ApplySubmit(ApplySubmitRequest request) => PostInvokeV2<ApplySubmitRequest, ApplySubmitResponse>(config.ApplySubmitUrl, request);

        public SizeQueryResponse SizeQuery(SizeQueryRequest request) => PostInvokeV2<SizeQueryRequest, SizeQueryResponse>(config.SizeQueryUrl, request);

        public ApplyQueryResponse ApplyQuery(ApplyQueryRequest request) => PostInvokeV2<ApplyQueryRequest, ApplyQueryResponse>(config.ApplyQueryUrl, request);

        public SocketStatusReportResponse SocketStatusReport(SocketStatusReportRequest request) => PostInvokeV2<SocketStatusReportRequest, SocketStatusReportResponse>(config.SocketStatusReportUrl, request);

        public ApplySubmitV2Response ApplySubmitV2(ApplySubmitRequest request) => PostInvokeV2<ApplySubmitRequest, ApplySubmitV2Response>(config.ApplySubmitUrl, request);

        public TokenResponse TokenRequest(TokenRequest request) => PostInvokeV2<TokenRequest, TokenResponse>(config.TokenRequestUrl, request, false);

        public void LogReport(LogReportRequest request) => PostInvokeV2<LogReportRequest, dynamic>(config.LogReportUrl, request, false);
    }
}
