using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Discuz.Web.Services.Manyou
{
    public class UserInfo
    {
        private int uid;
        private string userName = "";

        [JsonPropertyAttribute("uId")]
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        [JsonPropertyAttribute("handle")]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
    }
}
