using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Discuz.Web.Services.Manyou
{
    public class Friend
    {
        private int uid;
        private string userName;

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

        public Friend(int uid, string userName)
        {
            this.uid = uid;
            this.userName = userName;
        }
    }

    public class FriendsAction
    {
        private int uid1;
        private int uid2;
        private string action;

        [JsonPropertyAttribute("uId1")]
        public int Uid1
        {
            get { return uid1; }
            set { uid1 = value; }
        }

        [JsonPropertyAttribute("uId2")]
        public int Uid2
        {
            get { return uid2; }
            set { uid2 = value; }
        }

        [JsonPropertyAttribute("action")]
        public string Action
        {
            get { return action; }
            set { action = value; }
        }
    }
}
