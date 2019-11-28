
namespace PEF.Modules.ZZJ.TestClient.Models
{
    using Newtonsoft.Json;
    using Prism.Mvvm;
    //using SuperWebSocket;
    using System;
    using System.IO;
    using System.Reflection;

    public class ScriptItem : BindableBase
    {
        private string key;

        [JsonProperty("key")]
        public string Key
        {
            get => key;
            set => SetProperty(ref key, value);
        }

        private string name;

        [JsonProperty("name")]
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private int? port;

        [JsonProperty("port")]
        public int? Port
        {
            get => port;
            set => SetProperty(ref port, value);
        }

        private string uri;

        [JsonProperty("uri")]
        public string Uri
        {
            get => uri;
            set => SetProperty(ref uri, value);
        }

        [JsonIgnore]
        public string ScriptPath => Path.Combine(Assembly.GetExecutingAssembly().Location.Replace("dll", "Scripts"), $"{Key}.json");
    }
}
