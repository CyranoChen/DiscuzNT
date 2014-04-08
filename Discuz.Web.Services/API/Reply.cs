using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Discuz.Web.Services.API
{
    public class Reply
    {
        [JsonPropertyAttribute("tid")]
        public int Tid;

        [JsonPropertyAttribute("fid")]
        public int Fid;

        [JsonPropertyAttribute("message")]
        public string Message;


        [JsonPropertyAttribute("title")]
        public string Title;


    }
}
