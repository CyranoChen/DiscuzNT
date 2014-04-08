using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Discuz.Web.Services.API
{

    public class Notification
    {
        [JsonPropertyAttribute("unread")]
        [XmlElement("unread", IsNullable = false)]
        public int Unread;

        [JsonPropertyAttribute("most_recent")]
        [XmlElement("most_recent", IsNullable = false)]
        public int MostRecent;
    }   
}
