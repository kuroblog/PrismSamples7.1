
namespace PEF.Common.Models
{
    using Newtonsoft.Json;

    public abstract class ServiceBizBaseDto
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }

        [JsonProperty("requestId")]
        public string RequestId { get; set; }
    }
}
