using System;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Discuz.Web.Services.API
{
    public class Arg
    {
        public Arg()
        { }
        public Arg(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        [JsonProperty("key")]
        [XmlElement("key")]
        public string Key;

        [JsonProperty("value")]
        [XmlElement("value")]
        public string Value;
    }
}
