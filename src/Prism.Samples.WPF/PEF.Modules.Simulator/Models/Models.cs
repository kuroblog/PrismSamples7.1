
namespace PEF.Modules.Simulator.Models
{
    using Newtonsoft.Json;
    using Prism.Mvvm;
    using SuperWebSocket;
    using System;

    public class SessionItem : BindableBase
    {
        private string remote;

        public string Remote
        {
            get => remote;
            set => SetProperty(ref remote, value);
        }

        private WebSocketSession session;

        public WebSocketSession Session
        {
            get => session;
            set => SetProperty(ref session, value);
        }
    }

    public enum ScriptType
    {
        Unknown = 0x00,
        //Send = 0x01
    }

    //public class SendScriptItemData : IScriptItemData
    //{
    //    [JsonProperty("test")]
    //    public string Test { get; set; }

    //    [JsonProperty("test2")]
    //    public string Test1 { get; set; }
    //}

    public abstract class ScriptItemBase : BindableBase
    {
        private string key;

        [JsonProperty("key")]
        public virtual string Key
        {
            get => key;
            set => SetProperty(ref key, value);
        }

        private string name;

        [JsonProperty("name")]
        public virtual string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string method;

        [JsonProperty("action")]
        public virtual string Action
        {
            get => method;
            set => SetProperty(ref method, value);
        }
    }

    public interface IScriptItemData { }

    public class ScriptItemParser : ScriptItemBase
    {
        private dynamic data;

        [JsonProperty("data")]
        public virtual dynamic Data
        {
            get => data;
            set
            {
                SetProperty(ref data, value);
                Context = JsonConvert.SerializeObject(value, Formatting.Indented);
            }
        }

        private string context;

        [JsonIgnore]
        public string Context
        {
            get => context;
            set => SetProperty(ref context, value);
        }

        public virtual ScriptType ScriptType => Enum.TryParse(Action, out ScriptType result) ? result : ScriptType.Unknown;

        public virtual TScriptItemData ConvertTo<TScriptItemData>() where TScriptItemData : IScriptItemData
        {
            var json = JsonConvert.SerializeObject(Data);
            return JsonConvert.DeserializeObject<TScriptItemData>(json);
        }
    }

    public class ServerItem : BindableBase
    {
        private string itemKey;

        [JsonProperty("key")]
        public string ItemKey
        {
            get => itemKey;
            set => SetProperty(ref itemKey, value);
        }

        private string itemName;

        [JsonProperty("name")]
        public string ItemName
        {
            get => itemName;
            set => SetProperty(ref itemName, value);
        }

        private int? port;

        [JsonProperty("port")]
        public int? Port
        {
            get => port;
            set => SetProperty(ref port, value);
        }

        private string serverName;

        [JsonProperty("url")]
        public string ServerName
        {
            get => serverName;
            set => SetProperty(ref serverName, value);
        }
    }
}
