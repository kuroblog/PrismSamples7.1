
namespace PEF.Modules.SGDE.Services
{
    using PEF.Modules.SGDE.Services.Dtos;

    public class SGDEServiceProxyExample : ISGDEServiceProxy
    {
        private const string SuccessfulCode = "200";

        public CardQueryResponse CardQuery(CardQueryRequest request) => new CardQueryResponse
        {
            Code = SuccessfulCode,
            Data = new CardQueryData
            {
                Size = "L",
                UserId = request.CardId == "808F5F40" ? 111 : request.CardId == "80B46340" ? 222 : 999,
                UserName = request.CardId == "808F5F40" ? "测试1" : request.CardId == "80B46340" ? "测试2" : "Guest",
                UserType = request.CardId == "808F5F40" ? "U" : request.CardId == "80B46340" ? "M" : "N"
            }
        };

        public ConfigQueryResponse ConfigQuery(ConfigQueryRequest request) => new ConfigQueryResponse
        {
            Code = SuccessfulCode,
            Data = new ConfigQueryData
            {
                Items = new ConfigQueryItem[] {
                    new ConfigQueryItem { Id = 111, Limit = 99, Size = "L", Stock = 2, X = 1, Y = 1 } }
            }
        };

        public ConfigSubmitResponse ConfigSubmit(ConfigSubmitRequest request) => new ConfigSubmitResponse
        {
            Code = SuccessfulCode,
            Data = new ConfigSubmitData { IsSuccessful = true }
        };

        public ApplySubmitResponse ApplySubmit(ApplySubmitRequest request) => new ApplySubmitResponse
        {
            Code = SuccessfulCode,
            Data = new ApplySubmitData
            {
                HasSize = request.ApplyMode == 2,
                Id = 1,
                X = 1,
                Y = 1
            }
        };

        public SizeQueryResponse SizeQuery(SizeQueryRequest request) => new SizeQueryResponse
        {
            Code = SuccessfulCode,
            Data = new SizeQueryData
            {
                Items = new SizeQueryItem[] {
                    new SizeQueryItem { Id = 1, Size = "XS", Quantity = 10 },
                    new SizeQueryItem { Id = 2, Size = "S", Quantity = 9 },
                    new SizeQueryItem { Id = 3, Size = "M", Quantity = 12 },
                    //new SizeQueryItem { Id = 4, Size = "L", Quantity = 20 },
                    //new SizeQueryItem { Id = 5, Size = "XL", Quantity = 39 },
                    new SizeQueryItem { Id = 6, Size = "XXL", Quantity = 9 }
                }
            }
        };

        public ApplyQueryResponse ApplyQuery(ApplyQueryRequest request) => new ApplyQueryResponse
        {
            Code = SuccessfulCode,
            Data = new ApplyQueryData
            {
                SaveNo = 99,
                Schedules = new ApplyQuerySchedule[] {
                    new ApplyQuerySchedule { Room = "A1", Sequence = "1", SurgeryName = "Test", SurgeryMember = "1", Patient = "2" } }
            }
        };

        public SocketStatusReportResponse SocketStatusReport(SocketStatusReportRequest request) => new SocketStatusReportResponse
        {
            Code = SuccessfulCode,
            Data = new SocketStatusReportData { IsSuccessful = true }
        };

        public ApplySubmitV2Response ApplySubmitV2(ApplySubmitRequest request) => new ApplySubmitV2Response
        {
            Code = SuccessfulCode,
            Data = new ApplySubmitV2Data
            {
                HasSize = request.ApplyMode == 2,
                Id = 1,
                X = 1,
                Y = 1,
                SaveNo = 99,
                Schedules = new ApplyQuerySchedule[] { }
            }
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
