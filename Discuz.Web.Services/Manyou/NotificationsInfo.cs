using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Discuz.Web.Services.Manyou
{
    public class MessageInfo
    {
        private int unRead;

        [JsonPropertyAttribute("unRead")]
        public int UnRead
        {
            get { return unRead; }
            set { unRead = value; }
        }

        private double mostRecent;

        [JsonPropertyAttribute("mostRecent")]
        public double MostRecent
        {
            get { return mostRecent; }
            set { mostRecent = value; }
        }
    }

    public class NoticeInfo
    {
        private int unRead;

        [JsonPropertyAttribute("unRead")]
        public int UnRead
        {
            get { return unRead; }
            set { unRead = value; }
        }

        private double mostRecent;

        [JsonPropertyAttribute("mostRecent")]
        public double MostRecent
        {
            get { return mostRecent; }
            set { mostRecent = value; }
        }
    }

    public class FriendRequestInfo
    {
        private int[] uIds;

        [JsonPropertyAttribute("uIds")]
        public int[] UIds
        {
            get { return uIds; }
            set { uIds = value; }
        }

        private double mostRecent;

        [JsonPropertyAttribute("mostRecent")]
        public double MostRecent
        {
            get { return mostRecent; }
            set { mostRecent = value; }
        }
    }
}
