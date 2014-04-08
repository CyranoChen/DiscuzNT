using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Discuz.Common;

namespace Discuz.Web.Services.Manyou
{
    #region Base

    public class GetResponse<T>
    {
        private string version = Utils.ASSEMBLY_VERSION;
        private int timeZone = 8;
        private T result;
        private string charset = "UTF-8";
        private string language = "zh_CN";
        private int myVersion = 0;

        [JsonPropertyAttribute("charset")]
        public string Charset
        {
            get { return charset; }
            set { charset = value; }
        }

        [JsonPropertyAttribute("language")]
        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        [JsonPropertyAttribute("version")]
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        [JsonPropertyAttribute("result")]
        public T Result
        {
            get { return result; }
            set { result = value; }
        }

        [JsonPropertyAttribute("timezone")]
        public int TimeZone
        {
            get { return timeZone; }
            set { timeZone = value; }
        }

        [JsonPropertyAttribute("my_version")]
        public int MyVersion
        {
            get { return myVersion; }
            set { myVersion = value; }
        }
    }

    public class GetErrorResponse
    {
        private string version = Utils.ASSEMBLY_VERSION;
        private int timeZone = 8;
        private int errNo;
        private string errMsg;
        private string charset = "UTF-8";
        private string language = "zh_CN";
        private int myVersion = 0;

        [JsonPropertyAttribute("charset")]
        public string Charset
        {
            get { return charset; }
            set { charset = value; }
        }

        [JsonPropertyAttribute("language")]
        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        [JsonPropertyAttribute("version")]
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        [JsonPropertyAttribute("errno")]
        public int ErrorNumber
        {
            get { return errNo; }
            set { errNo = value; }
        }

        [JsonPropertyAttribute("errmsg")]
        public string ErrorMessage
        {
            get { return errMsg; }
            set { errMsg = value; }
        }

        [JsonPropertyAttribute("timezone")]
        public int TimeZone
        {
            get { return timeZone; }
            set { timeZone = value; }
        }

        [JsonPropertyAttribute("my_version")]
        public int MyVersion
        {
            get { return myVersion; }
            set { myVersion = value; }
        }
    }
    #endregion

    #region Users

    public class GetFriendInfoResponse
    {
        private int totalNum;
        private UserInfo me;
        private List<Friend> friends;

        [JsonPropertyAttribute("totalNum")]
        public int TotalNum
        {
            get { return totalNum; }
            set { totalNum = value; }
        }

        [JsonPropertyAttribute("friends")]
        public List<Friend> Friends
        {
            get { return friends; }
            set { friends = value; }
        }

        [JsonPropertyAttribute("me")]
        public UserInfo Me
        {
            get { return me; }
            set { me = value; }
        }
    }

    #endregion

    #region Site

    public class GetAllUsersResponse
    {
        private int totalNum;
        private List<FullUserInfo> fullUserList = new List<FullUserInfo>();

        [JsonPropertyAttribute("totalNum")]
        public int TotalNum
        {
            get { return totalNum; }
            set { totalNum = value; }
        }

        [JsonPropertyAttribute("users")]
        public List<FullUserInfo> FullUserList
        {
            get { return fullUserList; }
            set { fullUserList = value; }
        }
    }

    public class GetUpdateUsersResponse
    {
        private int totalNum;
        private List<UserInfoWithAction> userInfoWithActionList;

        [JsonPropertyAttribute("users")]
        public List<UserInfoWithAction> UserInfoWithActionList
        {
            get { return userInfoWithActionList; }
            set { userInfoWithActionList = value; }
        }

        [JsonPropertyAttribute("totalNum")]
        public int TotalNum
        {
            get { return totalNum; }
            set { totalNum = value; }
        }
    }

    public class GetUpdatedFriendsResponse
    {
        private int totalNum;
        private List<FriendsAction> actionList = new List<FriendsAction>();

        [JsonPropertyAttribute("totalNum")]
        public int TotalNum
        {
            get { return totalNum; }
            set { totalNum = value; }
        }

        [JsonPropertyAttribute("friends")]
        public List<FriendsAction> ActionList
        {
            get { return actionList; }
            set { actionList = value; }
        }
    }

    #endregion

    #region Notifications

    public class GetNoticeResponse
    {
        private MessageInfo message = new MessageInfo();

        [JsonPropertyAttribute("message")]
        public MessageInfo Message
        {
            get { return message; }
            set { message = value; }
        }

        private NoticeInfo notification = new NoticeInfo();

        [JsonPropertyAttribute("notification")]
        public NoticeInfo Notification
        {
            get { return notification; }
            set { notification = value; }
        }

        private FriendRequestInfo friendRequest = new FriendRequestInfo();

        [JsonPropertyAttribute("friendRequest")]
        public FriendRequestInfo FriendRequest
        {
            get { return friendRequest; }
            set { friendRequest = value; }
        }
    }

    #endregion
}
