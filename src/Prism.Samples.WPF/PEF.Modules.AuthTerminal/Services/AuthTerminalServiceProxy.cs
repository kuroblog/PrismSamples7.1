
namespace PEF.Modules.AuthTerminal.Services
{
    using PEF.Common.Extensions;
    using PEF.Http.Utilities;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.AuthTerminal.Services.Dtos;
    using System;

    /// <summary>
    /// 服务代理接口
    /// </summary>
    public interface IAuthTerminalServiceProxy
    {
        /// <summary>
        /// 卡查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CardQueryResponse CardQuery(CardQueryRequest request);

        /// <summary>
        /// 卡查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        RoleQueryResponse RoleQuery();

        UserRoleBindingResponse UserRoleBinding(UserRoleBindingRequest request);

        TokenResponse TokenRequest(TokenRequest request);

        SocketStatusReportResponse SocketStatusReport(SocketStatusReportRequest request);

        ///// <summary>
        ///// 托盘配置查询
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //ConfigQueryResponse ConfigQuery(ConfigQueryRequest request);

        ///// <summary>
        ///// 托盘配置上传
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //ConfigSubmitResponse ConfigSubmit(ConfigSubmitRequest request);

        ///// <summary>
        ///// 申请发衣
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //ApplySubmitResponse ApplySubmit(ApplySubmitRequest request);

        ///// <summary>
        ///// 申请发衣 ver.2
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //ApplySubmitV2Response ApplySubmitV2(ApplySubmitRequest request);

        ///// <summary>
        ///// 尺码查询
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //SizeQueryResponse SizeQuery(SizeQueryRequest request);

        ///// <summary>
        ///// 发衣申请结果查询
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //ApplyQueryResponse ApplyQuery(ApplyQueryRequest request);

        ///// <summary>
        ///// Socket 状态报告
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //SocketStatusReportResponse SocketStatusReport(SocketStatusReportRequest request);
    }

    /// <summary>
    /// 服务代理类
    /// </summary>
    public class AuthTerminalServiceProxy : IAuthTerminalServiceProxy
    {
        private readonly AuthTerminalConfig config = null;
        private readonly HttpClientProxy proxy = null;
        private readonly ILogger logger = null;

        public AuthTerminalServiceProxy(
            AuthTerminalConfig config,
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

        protected virtual TResponse GetInvoke<TResponse>(string actionUrl)
        {
            var response = proxy.Get(config.ServiceUrl, actionUrl);
            if (!response.IsSuccessStatusCode)
            {
                return default(TResponse);
            }

            var json = response.Content.ReadAsStringAsync().Result;

            return json.GetJsonObject<TResponse>();
        }

        protected virtual TResponse GetInvokeV2<TResponse>(string actionUrl, bool errorReport = true)
        {
            TResponse result;

            var response = proxy.Get(config.ServiceUrl, actionUrl);
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

        public RoleQueryResponse RoleQuery() => GetInvokeV2<RoleQueryResponse>(config.RoleQueryUrl);

        public UserRoleBindingResponse UserRoleBinding(UserRoleBindingRequest request) => PostInvokeV2<UserRoleBindingRequest, UserRoleBindingResponse>(config.UserRoleUrl, request);

        public TokenResponse TokenRequest(TokenRequest request) => PostInvokeV2<TokenRequest, TokenResponse>(config.TokenRequestUrl, request);

        //public ConfigQueryResponse ConfigQuery(ConfigQueryRequest request) => Invoke<ConfigQueryRequest, ConfigQueryResponse>(config.ConfigQueryUrl, request);

        //public ConfigSubmitResponse ConfigSubmit(ConfigSubmitRequest request) => Invoke<ConfigSubmitRequest, ConfigSubmitResponse>(config.ConfigSubmitUrl, request);

        //public ApplySubmitResponse ApplySubmit(ApplySubmitRequest request) => Invoke<ApplySubmitRequest, ApplySubmitResponse>(config.ApplySubmitUrl, request);

        //public SizeQueryResponse SizeQuery(SizeQueryRequest request) => Invoke<SizeQueryRequest, SizeQueryResponse>(config.SizeQueryUrl, request);

        //public ApplyQueryResponse ApplyQuery(ApplyQueryRequest request) => Invoke<ApplyQueryRequest, ApplyQueryResponse>(config.ApplyQueryUrl, request);

        //public SocketStatusReportResponse SocketStatusReport(SocketStatusReportRequest request) => Invoke<SocketStatusReportRequest, SocketStatusReportResponse>(config.SocketStatusReportUrl, request);

        //public ApplySubmitV2Response ApplySubmitV2(ApplySubmitRequest request) => Invoke<ApplySubmitRequest, ApplySubmitV2Response>(config.ApplySubmitUrl, request);

        public SocketStatusReportResponse SocketStatusReport(SocketStatusReportRequest request) => PostInvokeV2<SocketStatusReportRequest, SocketStatusReportResponse>(config.SocketStatusReportUrl, request, false);
    }
}
