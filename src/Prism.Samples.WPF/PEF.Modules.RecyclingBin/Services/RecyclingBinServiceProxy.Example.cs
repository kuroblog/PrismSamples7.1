
namespace PEF.Modules.RecyclingBin.Services
{
    using PEF.Modules.RecyclingBin.Services.Dtos;

    public class RecyclingBinServiceProxyExample : IRecyclingBinServiceProxy
    {
        private const string SuccessfulCode = "200";

        public CardQueryResponse CardQuery(CardQueryRequest request) => new CardQueryResponse
        {
            Code = SuccessfulCode,
            Data = new CardQueryData
            {
                IsRecovery = true,
                UserId = 12333,
                UserName = "guest",
                UserType = request.CardId == "808F5F40" ? "U" : request.CardId == "80B46340" ? "M" : "N"
            }
        };

        public RecyclingBinQueryResponse RecyclingBinQuery(RecyclingBinQueryRequest request) => new RecyclingBinQueryResponse
        {
            Code = SuccessfulCode,
            Data = new RecyclingBinQueryData
            {
                Quantity = 99,
                Weight = 990
            }
        };

        public RecyclingBinSubmitResponse RecyclingBinSubmit(RecyclingBinSubmitRequest request) => new RecyclingBinSubmitResponse
        {
            Code = SuccessfulCode,
            Data = new RecyclingBinSubmitData { IsSuccessful = true }
        };

        public RecyclingBinCleanResponse RecyclingBinClean(RecyclingBinCleanRequest request) => new RecyclingBinCleanResponse
        {
            Code = SuccessfulCode,
            Data = new RecyclingBinCleanData { IsSuccessful = true }
        };

        //public RecyclingBinAdminVerifyResponse RecyclingBinAdminVerify(RecyclingBinAdminVerifyRequest request) => new RecyclingBinAdminVerifyResponse
        //{
        //    Code = SuccessfulCode,
        //    Data = new RecyclingBinAdminVerifyData
        //    {
        //        UserId = 12333,
        //        UserName = "Admin",
        //        UserType = "M"
        //    }
        //};

        public SocketStatusReportResponse SocketStatusReport(SocketStatusReportRequest request) => new SocketStatusReportResponse
        {
            Code = SuccessfulCode,
            Data = new SocketStatusReportData { IsSuccessful = true }
        };

        public TokenResponse TokenRequest(TokenRequest request) => new TokenResponse
        {
            Code = SuccessfulCode,
            Data = new TokenData
            {
                Token = "1234567890"
            }
        };

        public void LogReport(LogReportRequest request) { }
    }
}
