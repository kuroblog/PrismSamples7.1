
namespace PEF.Modules.ShoeBox.Services
{
    using PEF.Modules.ShoeBox.Services.Dtos;

    public class ShoeBoxServiceProxyExample : IShoeBoxServiceProxy
    {
        private const string SuccessfulCode = "200";

        public SocketStatusReportResponse SocketStatusReport(SocketStatusReportRequest request) => new SocketStatusReportResponse
        {
            Code = SuccessfulCode,
            Data = new SocketStatusReportData { IsSuccessful = true }
        };

        public CardQueryResponse CardQuery(CardQueryRequest request) => new CardQueryResponse
        {
            Code = SuccessfulCode,
            Data = new CardQueryData
            {
                UserId = request.CardId == "808F5F40" ? 111 : request.CardId == "80B46340" ? 222 : 999,
                UserName = request.CardId == "808F5F40" ? "测试1" : request.CardId == "80B46340" ? "测试2" : "Guest",
                UserType = request.CardId == "808F5F40" ? "U" : request.CardId == "80B46340" ? "M" : "N",
                //DeviceItemNo = 11,
                //DeviceItemCode = "111"
                DeviceItemNo = 0,
                DeviceItemCode = ""
            }
        };

        public DeviceConfigQueryResponse DeviceConfigQuery(DeviceConfigQueryRequest request) => new DeviceConfigQueryResponse
        {
            Code = SuccessfulCode,
            Data = new DeviceConfigQueryData
            {
                Items = new DeviceConfigItem[] {
                    new DeviceConfigItem { DeviceItemId = 100, DeviceItemNo = 1, DeviceItemCode = "111", Status = 2, UserName = string.Empty },
                    new DeviceConfigItem { DeviceItemId = 101, DeviceItemNo = 2, DeviceItemCode = "112", Status = 2, UserName = string.Empty },
                    new DeviceConfigItem { DeviceItemId = 102, DeviceItemNo = 3, DeviceItemCode = "113", Status = 3, UserName = string.Empty },
                    new DeviceConfigItem { DeviceItemId = 103, DeviceItemNo = 4, DeviceItemCode = "114", Status = 2, UserName = string.Empty },
                    new DeviceConfigItem { DeviceItemId = 104, DeviceItemNo = 5, DeviceItemCode = "115", Status = 1, UserName = "Guest1" },
                    new DeviceConfigItem { DeviceItemId = 105, DeviceItemNo = 6, DeviceItemCode = "116", Status = 2, UserName = string.Empty },
                    new DeviceConfigItem { DeviceItemId = 106, DeviceItemNo = 7, DeviceItemCode = "117", Status = 1, UserName = "Guest2" },
                    new DeviceConfigItem { DeviceItemId = 107, DeviceItemNo = 8, DeviceItemCode = "118", Status = 1, UserName = "Guest3" },
                    new DeviceConfigItem { DeviceItemId = 108, DeviceItemNo = 9, DeviceItemCode = "119", Status = 2, UserName = string.Empty },
                    new DeviceConfigItem { DeviceItemId = 109, DeviceItemNo = 10, DeviceItemCode = "120", Status = 3, UserName = string.Empty },
                    new DeviceConfigItem { DeviceItemId = 110, DeviceItemNo = 11, DeviceItemCode = "121", Status = 3, UserName = string.Empty } }
            }
        };

        public DeviceItemStateQueryResponse DeviceItemStateQuery(DeviceItemStateQueryRequest request) => new DeviceItemStateQueryResponse
        {
            Code = SuccessfulCode,
            Data = new DeviceItemStateQueryData
            {
                Status = 2
            }
        };

        public DeviceRegistrationResponse DeviceRegistration(DeviceRegistrationRequest request) => new DeviceRegistrationResponse
        {
            Code = SuccessfulCode,
            Data = new DeviceRegistrationData
            {
                DeviceItemNo = 12,
                DeviceItemCode = "211"
            }
        };

        public DeviceResetResponse DeviceReset(DeviceResetRequest request) => new DeviceResetResponse
        {
            Code = SuccessfulCode,
            Data = new DeviceResetData
            {
                IsSuccessful = true
            }
        };

        public DeviceStateQueryResponse DeviceStateQuery(DeviceStateQueryRequest request) => new DeviceStateQueryResponse
        {
            Code = SuccessfulCode,
            Data = new DeviceStateQueryData
            {
                Quantity = 92
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
