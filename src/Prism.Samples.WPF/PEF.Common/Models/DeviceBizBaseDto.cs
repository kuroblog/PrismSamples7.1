
namespace PEF.Common.Models
{
    using Newtonsoft.Json;

    public abstract class DeviceBizBaseDto
    {
        [JsonProperty("Err_Code", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorCode { get; set; }

        [JsonProperty("Err_Msg", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorMessage { get; set; }

        [JsonProperty("Method")]
        public string Method { get; set; }
    }
}
