using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Discuz.Web.Services.API
{
    public class Message
    {
        [JsonPropertyAttribute("message_id")]
        [XmlElement("message_id", IsNullable = false)]
        public int MessageId;

        [JsonPropertyAttribute("from")]
        [XmlElement("from", IsNullable = false)]
        public string From;

        [JsonPropertyAttribute("from_id")]
        [XmlElement("from_id", IsNullable = false)]
        public int FromId;

        [JsonPropertyAttribute("subject")]
        [XmlElement("subject", IsNullable = false)]
        public string Subject;

        [JsonPropertyAttribute("post_date_time")]
        [XmlElement("post_date_time", IsNullable = false)]
        public string PostDateTime;

        [JsonPropertyAttribute("message")]
        [XmlElement("message", IsNullable = false)]
        public string MessageContent;
        
    }
}
