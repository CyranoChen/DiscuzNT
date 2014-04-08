using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Common;
using Newtonsoft.Json;

namespace Discuz.Web.Services.Manyou
{
    public class FullUserInfo : UserInfo
    {
        private string[] friendsArray;

        [JsonPropertyAttribute("friends")]
        public string[] FriendsArray
        {
            get { return friendsArray; }
            set { friendsArray = value; }
        }

        public string GetFriendsList()
        {
            return ConverteStrArrayToString(friendsArray);
        }

        private string ConverteStrArrayToString(string[] array)
        {
            string result = "";
            foreach (string s in array)
            {
                if (result != "")
                    result += ",";
                result += s;
            }
            return result;
        }
    }
}
