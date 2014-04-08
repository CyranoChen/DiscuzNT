using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Discuz.Web.Services.Manyou
{
    public class UserInfoWithAction : UserInfo
    {
        private string action = "";

        [JsonPropertyAttribute("action")]
        public string Action
        {
            get { return action; }
            set { action = value; }
        }

        //private string updateType = "";

        //public string UpdateType
        //{
        //    get { return updateType; }
        //    set { updateType = value; }
        //}
    }
}
