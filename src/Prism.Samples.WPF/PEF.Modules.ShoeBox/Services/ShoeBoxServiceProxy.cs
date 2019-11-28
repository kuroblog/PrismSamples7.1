
namespace PEF.Modules.ShoeBox.Services
{
    using PEF.Common;
    using PEF.Common.Extensions;
    using PEF.Http.Utilities;
    using PEF.Logger.Infrastructures;
    using PEF.Modules.ShoeBox.Services.Dtos;
    using PEF.Modules.ShoeBox.Views;
    using Prism.Regions;
    using Prism.Ioc;
    using System;

    public interface IShoeBoxServiceProxy
    {
        /// <summary>
        /// 设备状态查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DeviceStateQueryResponse DeviceStateQuery(DeviceStateQueryRequest request);

        CardQueryResponse CardQuery(CardQueryRequest request);

        DeviceRegistrationResponse DeviceRegistration(DeviceRegistrationRequest request);

        DeviceConfigQueryResponse DeviceConfigQuery(DeviceConfigQueryRequest request);

        DeviceResetResponse DeviceReset(DeviceResetRequest request);

        TokenResponse TokenRequest(TokenRequest request);

        DeviceItemStateQueryResponse DeviceItemStateQuery(DeviceItemStateQueryRequest request);

        SocketStatusReportResponse SocketStatusReport(SocketStatusReportRequest request);

        void LogReport(LogReportRequest request);
    }

    public class ShoeBoxServiceProxy : IShoeBoxServiceProxy
    {
        private readonly ILogger logger = null;
        private readonly ShoeBoxConfig config = null;
        private readonly HttpClientProxy proxy = null;

        public ShoeBoxServiceProxy(ILogger logger, ShoeBoxConfig config, HttpClientProxy proxy)
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

        public DeviceStateQueryResponse DeviceStateQuery(DeviceStateQueryRequest request) => PostInvokeV2<DeviceStateQueryRequest, DeviceStateQueryResponse>(config.DeviceStateQueryUrl, request);

        public CardQueryResponse CardQuery(CardQueryRequest request) => PostInvokeV2<CardQueryRequest, CardQueryResponse>(config.CardQueryUrl, request);

        public DeviceRegistrationResponse DeviceRegistration(DeviceRegistrationRequest request) => PostInvokeV2<DeviceRegistrationRequest, DeviceRegistrationResponse>(config.DeviceRegistrationUrl, request);

        public DeviceConfigQueryResponse DeviceConfigQuery(DeviceConfigQueryRequest request) => PostInvokeV2<DeviceConfigQueryRequest, DeviceConfigQueryResponse>(config.DeviceConfigQueryUrl, request);

        public DeviceResetResponse DeviceReset(DeviceResetRequest request) => PostInvokeV2<DeviceResetRequest, DeviceResetResponse>(config.DeviceResetUrl, request);

        public TokenResponse TokenRequest(TokenRequest request) => PostInvokeV2<TokenRequest, TokenResponse>(config.TokenRequestUrl, request);

        public DeviceItemStateQueryResponse DeviceItemStateQuery(DeviceItemStateQueryRequest request) => PostInvokeV2<DeviceItemStateQueryRequest, DeviceItemStateQueryResponse>(config.DeviceItemStateQueryUrl, request);

        public SocketStatusReportResponse SocketStatusReport(SocketStatusReportRequest request) => PostInvokeV2<SocketStatusReportRequest, SocketStatusReportResponse>(config.SocketStatusReportUrl, request, false);

        public void LogReport(LogReportRequest request) => PostInvokeV2<LogReportRequest, dynamic>(config.LogReportUrl, request, false);
    }
}
