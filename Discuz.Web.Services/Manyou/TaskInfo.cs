using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Discuz.Web.Services
{
    public class TaskInfo
    {
        private string className;

        [JsonPropertyAttribute("module")]
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        private string method;

        [JsonPropertyAttribute("method")]
        public string Method
        {
            get { return method; }
            set { method = value; }
        }
    }
}
