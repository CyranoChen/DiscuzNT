using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Discuz.Common;
using System.Collections.Specialized;

namespace Discuz.Web.Services.Manyou
{
    #region Users

    public class GetUsersInfoParams
    {
        private string[] uids;
        private string fields;
        private bool isExtra;

        [JsonPropertyAttribute("uIds")]
        public string[] UidArray
        {
            get { return uids; }
            set { uids = value; }
        }

        public string Uids
        {
            get
            {
                return ConverteStrArrayToString(uids);
            }
            set
            {
                uids = Utils.SplitString(value, ",");
            }
        }

        [JsonPropertyAttribute("fields")]
        public string Fields
        {
            get { return fields; }
            set { fields = value; }
        }

        [JsonPropertyAttribute("isExtra")]
        public bool IsExtra
        {
            get { return isExtra; }
            set { isExtra = value; }
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

    public class GetFriendInfoParams
    {
        private int uid;
        private int showFriendsNum;
        private bool isExtra;


        [JsonPropertyAttribute("uId")]
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        [JsonPropertyAttribute("num")]
        public int ShowFriendsNum
        {
            get { return showFriendsNum; }
            set { showFriendsNum = value; }
        }

        [JsonPropertyAttribute("isExtra")]
        public bool IsExtra
        {
            get { return isExtra; }
            set { isExtra = value; }
        }
    }

    #endregion

    #region Site

    public class GetAllUsersParams
    {
        private int from;
        private int readCount;
        private bool isExtra;
        private int friendNum;

        [JsonPropertyAttribute("friendNum")]
        public int FriendNum
        {
            get { return friendNum; }
            set { friendNum = value; }
        }

        [JsonPropertyAttribute("from")]
        public int From
        {
            get { return from; }
            set { from = value; }
        }

        [JsonPropertyAttribute("num")]
        public int ReadCount
        {
            get { return readCount; }
            set { readCount = value; }
        }

        [JsonPropertyAttribute("isExtra")]
        public bool IsExtra
        {
            get { return isExtra; }
            set { isExtra = value; }
        }
    }

    public class GetUpdatedUsers
    {
        private int count;

        [JsonPropertyAttribute("num")]
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
    }

    public class GetUpdatedFriendsParams
    {
        private int count;

        [JsonPropertyAttribute("num")]
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
    }

    #endregion

    #region UserApplication

    public class AddUserApplicationParams
    {
        private int appId;

        [JsonPropertyAttribute("appId")]
        public int AppId
        {
            get { return appId; }
            set { appId = value; }
        }

        private string appName;

        [JsonPropertyAttribute("appName")]
        public string AppName
        {
            get { return appName; }
            set { appName = value; }
        }

        private int uid;

        [JsonPropertyAttribute("uId")]
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        #region 可选参数
        private string version = "";

        [JsonPropertyAttribute("version")]
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        private string displayMethod = "";

        [JsonPropertyAttribute("displayMethod")]
        public string DisplayMethod
        {
            get { return displayMethod; }
            set { displayMethod = value; }
        }

        private int? displayOrder = 0;

        [JsonPropertyAttribute("displayOrder")]
        public int? DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        private string privacy = "";

        [JsonPropertyAttribute("privacy")]
        public string Privacy
        {
            get { return privacy; }
            set { privacy = value; }
        }

        private bool? allowSideNav = false;

        [JsonPropertyAttribute("allowSideNav")]
        public bool? AllowSideNav
        {
            get { return allowSideNav; }
            set { allowSideNav = value; }
        }

        private bool? allowFeed = false;

        [JsonPropertyAttribute("allowFeed")]
        public bool? AllowFeed
        {
            get { return allowFeed; }
            set { allowFeed = value; }
        }

        private bool? allowProfileLink = false;

        [JsonPropertyAttribute("allowProfileLink")]
        public bool? AllowProfileLink
        {
            get { return allowProfileLink; }
            set { allowProfileLink = value; }
        }

        private string defaultBoxType = "";

        [JsonPropertyAttribute("defaultBoxType")]
        public string DefaultBoxType
        {
            get { return defaultBoxType; }
            set { defaultBoxType = value; }
        }

        private string defaultMYML = "";

        [JsonPropertyAttribute("defaultMYML")]
        public string DefaultMYML
        {
            get { return defaultMYML; }
            set { defaultMYML = value; }
        }

        private string defaultProfileLink = "";

        [JsonPropertyAttribute("defaultProfileLink")]
        public string DefaultProfileLink
        {
            get { return defaultProfileLink; }
            set { defaultProfileLink = value; }
        }

        private int? userPanelArea = 0;

        [JsonPropertyAttribute("userPanelArea")]
        public int? UserPanelArea
        {
            get { return userPanelArea; }
            set { userPanelArea = value; }
        }

        private string canvasTitle = "";

        [JsonPropertyAttribute("canvasTitle")]
        public string CanvasTitle
        {
            get { return canvasTitle; }
            set { canvasTitle = value; }
        }

        private bool? isFullScreen = false;

        [JsonPropertyAttribute("isFullscreen")]
        public bool? IsFullScreen
        {
            get { return isFullScreen; }
            set { isFullScreen = value; }
        }

        private bool? displayUserPanel = false;

        [JsonPropertyAttribute("displayUserPanel")]
        public bool? DisplayUserPanel
        {
            get { return displayUserPanel; }
            set { displayUserPanel = value; }
        }

        #endregion
    }

    public class UpdateUserApplicationParams
    {
        private int[] appIds;

        [JsonPropertyAttribute("appIds")]
        public int[] AppIds
        {
            get { return appIds; }
            set { appIds = value; }
        }

        private string appName;

        [JsonPropertyAttribute("appName")]
        public string AppName
        {
            get { return appName; }
            set { appName = value; }
        }

        private int uid;

        [JsonPropertyAttribute("uId")]
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        #region 可选参数
        private string version = "";

        [JsonPropertyAttribute("version")]
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        private string displayMethod = "";

        [JsonPropertyAttribute("displayMethod")]
        public string DisplayMethod
        {
            get { return displayMethod; }
            set { displayMethod = value; }
        }

        private int? displayOrder = 0;

        [JsonPropertyAttribute("displayOrder")]
        public int? DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        private string privacy = "";

        [JsonPropertyAttribute("privacy")]
        public string Privacy
        {
            get { return privacy; }
            set { privacy = value; }
        }

        private bool? allowSideNav = false;

        [JsonPropertyAttribute("allowSideNav")]
        public bool? AllowSideNav
        {
            get { return allowSideNav; }
            set { allowSideNav = value; }
        }

        private bool? allowFeed = false;

        [JsonPropertyAttribute("allowFeed")]
        public bool? AllowFeed
        {
            get { return allowFeed; }
            set { allowFeed = value; }
        }

        private bool? allowProfileLink = false;

        [JsonPropertyAttribute("allowProfileLink")]
        public bool? AllowProfileLink
        {
            get { return allowProfileLink; }
            set { allowProfileLink = value; }
        }

        private string defaultBoxType = "";

        [JsonPropertyAttribute("defaultBoxType")]
        public string DefaultBoxType
        {
            get { return defaultBoxType; }
            set { defaultBoxType = value; }
        }

        private string defaultMYML = "";

        [JsonPropertyAttribute("defaultMYML")]
        public string DefaultMYML
        {
            get { return defaultMYML; }
            set { defaultMYML = value; }
        }

        private string defaultProfileLink = "";

        [JsonPropertyAttribute("defaultProfileLink")]
        public string DefaultProfileLink
        {
            get { return defaultProfileLink; }
            set { defaultProfileLink = value; }
        }

        private int? userPanelArea = 0;

        [JsonPropertyAttribute("userPanelArea")]
        public int? UserPanelArea
        {
            get { return userPanelArea; }
            set { userPanelArea = value; }
        }

        private string canvasTitle = "";

        [JsonPropertyAttribute("canvasTitle")]
        public string CanvasTitle
        {
            get { return canvasTitle; }
            set { canvasTitle = value; }
        }

        private bool? isFullScreen = false;

        [JsonPropertyAttribute("isFullscreen")]
        public bool? IsFullScreen
        {
            get { return isFullScreen; }
            set { isFullScreen = value; }
        }

        private bool? displayUserPanel = false;

        [JsonPropertyAttribute("displayUserPanel")]
        public bool? DisplayUserPanel
        {
            get { return displayUserPanel; }
            set { displayUserPanel = value; }
        }

        #endregion
    }

    public class RemoveUserApplicationParams
    {
        private int uid;

        [JsonPropertyAttribute("uId")]
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        private string[] appIds;

        [JsonPropertyAttribute("appIds")]
        public string[] AppIds
        {
            get { return appIds; }
            set { appIds = value; }
        }

        public string AppIdList
        {
            get { return ConverteStrArrayToString(appIds); }
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

    public class GetUserInstalledApplicationIdParams
    {
        private int uid;

        [JsonPropertyAttribute("uId")]
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }
    }

    public class GetUserApplicationMessageParams
    {
        private int uid;

        [JsonPropertyAttribute("uId")]
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        private int[] appIds;

        [JsonPropertyAttribute("appIds")]
        public int[] AppIds
        {
            get { return appIds; }
            set { appIds = value; }
        }

        public string AppIdList
        {
            get { return ConverteStrArrayToString(appIds); }
        }

        private string ConverteStrArrayToString(int[] array)
        {
            string result = "";
            foreach (int s in array)
            {
                if (result != "")
                    result += ",";
                result += s;
            }
            return result;
        }
    }

    #endregion

    #region Request

    public class SendApplicationInviteParams
    {
        private int uId;

        [JsonPropertyAttribute("uId")]
        public int UId
        {
            get { return uId; }
            set { uId = value; }
        }

        private string[] recipientIds;

        [JsonPropertyAttribute("recipientIds")]
        public string[] RecipientIds
        {
            get { return recipientIds; }
            set { recipientIds = value; }
        }

        private string appId;

        [JsonPropertyAttribute("appId")]
        public string AppId
        {
            get { return appId; }
            set { appId = value; }
        }

        private string requestName;

        [JsonPropertyAttribute("requestName")]
        public string RequestName
        {
            get { return requestName; }
            set { requestName = value; }
        }

        private string myml;

        [JsonPropertyAttribute("myml")]
        public string MYML
        {
            get { return myml; }
            set { myml = value; }
        }

        private string type;

        [JsonPropertyAttribute("type")]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
    }

    #endregion

    #region Friends

    public class GetFriendsParams
    {
        private int[] uIds;

        [JsonPropertyAttribute("uIds")]
        public int[] UIds
        {
            get { return uIds; }
            set { uIds = value; }
        }

        private int friendNum;

        [JsonPropertyAttribute("friendNum")]
        public int FriendNum
        {
            get { return friendNum; }
            set { friendNum = value; }
        }
    }

    public class AreFriendsParams
    {
        private int uId1;

        [JsonPropertyAttribute("uId1")]
        public int UId1
        {
            get { return uId1; }
            set { uId1 = value; }
        }

        private int uId2;

        [JsonPropertyAttribute("uId2")]
        public int UId2
        {
            get { return uId2; }
            set { uId2 = value; }
        }


    }

    #endregion

    #region notification

    public class SendNoticeParams
    {

        private int uId;

        [JsonPropertyAttribute("uId")]
        public int UId
        {
            get { return uId; }
            set { uId = value; }
        }

        private int[] recipientIds;

        [JsonPropertyAttribute("recipientIds")]
        public int[] RecipientIds
        {
            get { return recipientIds; }
            set { recipientIds = value; }
        }

        private int appId;

        [JsonPropertyAttribute("appId")]
        public int AppId
        {
            get { return appId; }
            set { appId = value; }
        }

        private string notification;

        [JsonPropertyAttribute("notification")]
        public string Notification
        {
            get { return notification; }
            set { notification = value; }
        }
    }

    public class GetNoticeParams
    {
        private int uId;

        [JsonPropertyAttribute("uId")]
        public int UId
        {
            get { return uId; }
            set { uId = value; }
        }
    }

    #endregion

    #region application

    public class UpdateApplicationParams
    {
        private int appId;

        [JsonPropertyAttribute("appId")]
        public int AppId
        {
            get { return appId; }
            set { appId = value; }
        }

        private string appName;

        [JsonPropertyAttribute("appName")]
        public string AppName
        {
            get { return appName; }
            set { appName = value; }
        }

        private int version;

        [JsonPropertyAttribute("version")]
        public int Version
        {
            get { return version; }
            set { version = value; }
        }

        private string displayMethod;

        [JsonPropertyAttribute("displayMethod")]
        public string DisplayMethod
        {
            get { return displayMethod; }
            set { displayMethod = value; }
        }

        private int displayOrder;

        [JsonPropertyAttribute("displayOrder")]
        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        private int? userPanelArea = 0;

        [JsonPropertyAttribute("userPanelArea")]
        public int? UserPanelArea
        {
            get { return userPanelArea; }
            set { userPanelArea = value; }
        }

        private string canvasTitle = "";

        [JsonPropertyAttribute("canvasTitle")]
        public string CanvasTitle
        {
            get { return canvasTitle; }
            set { canvasTitle = value; }
        }

        private bool? isFullScreen = false;

        [JsonPropertyAttribute("isFullscreen")]
        public bool? IsFullScreen
        {
            get { return isFullScreen; }
            set { isFullScreen = value; }
        }

        private bool? displayUserPanel = false;

        [JsonPropertyAttribute("displayUserPanel")]
        public bool? DisplayUserPanel
        {
            get { return displayUserPanel; }
            set { displayUserPanel = value; }
        }



    }

    public class RemoveApplicationParams
    {
        private int[] appIds;

        [JsonPropertyAttribute("appIds")]
        public int[] AppIds
        {
            get { return appIds; }
            set { appIds = value; }
        }

        public string AppIdList
        {
            get
            {
                string str = "";
                foreach (int i in appIds)
                    str += i + ",";
                return str.TrimEnd(',');
            }
        }
    }

    public class SetApplicationFlagParams
    {
        private ApplicationInfo[] applications;

        [JsonPropertyAttribute("applications")]
        public ApplicationInfo[] Applications
        {
            get { return applications; }
            set { applications = value; }
        }

        private string flag;

        [JsonPropertyAttribute("flag")]
        public string Flag
        {
            get { return flag; }
            set { flag = value; }
        }
    }

    public class ApplicationInfo
    {
        private int appId;

        [JsonPropertyAttribute("appId")]
        public int AppId
        {
            get { return appId; }
            set { appId = value; }
        }

        private string appName;

        [JsonPropertyAttribute("appName")]
        public string AppName
        {
            get { return appName; }
            set { appName = value; }
        }
    }

    #endregion

    #region profile

    public class SetUserAppFieldsInfoParams
    {
        private int uId;
        [JsonPropertyAttribute("uId")]
        public int UId
        {
            get { return uId; }
            set { uId = value; }
        }

        private int appId;
        [JsonPropertyAttribute("appId")]
        public int AppId
        {
            get { return appId; }
            set { appId = value; }
        }

        private string markup = "";
        [JsonPropertyAttribute("markup")]
        public string Markup
        {
            get { return markup; }
            set { markup = value; }
        }

        private string actionMarkup = "";
        [JsonPropertyAttribute("actionMarkup")]
        public string ActionMarkup
        {
            get { return actionMarkup; }
            set { actionMarkup = value; }
        }
    }

    #endregion

    #region feed

    public class PublishTemplatizedActionParams
    {
        private int uId;

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("uId")]
        public int Uid
        {
            get { return uId; }
            set { uId = value; }
        }

        private int appId;

        /// <summary>
        /// 应用id，如果FeedType不是应用，则为0
        /// </summary>
        [JsonPropertyAttribute("appId")]
        public int AppId
        {
            get { return appId; }
            set { appId = value; }
        }

        private string titleTemplate = "";

        /// <summary>
        /// 标题模板
        /// </summary>
        [JsonPropertyAttribute("titleTemplate")]
        public string TitleTemplate
        {
            get { return titleTemplate; }
            set { titleTemplate = value; }
        }

        private Dictionary<string, string> titleData = new Dictionary<string, string>();

        /// <summary>
        /// 标题参数和值
        /// </summary>
        [JsonPropertyAttribute("titleData")]
        public Dictionary<string,string> TitleData
        {
            get { return titleData; }
            set { titleData = value; }
        }

        private string bodyTemplate = "";

        /// <summary>
        /// 正文模板
        /// </summary>
        [JsonPropertyAttribute("bodyTemplate")]
        public string BodyTemplate
        {
            get { return bodyTemplate; }
            set { bodyTemplate = value; }
        }

        //public struct KeyValue
        //{
        //    public string Key;
        //    public string Value;
        //}

        private ArrayList bodyData = new ArrayList();

        /// <summary>
        /// 正文参数和值
        /// </summary>
        [JsonPropertyAttribute("bodyData")]
        public ArrayList BodyData
        {
            get { return bodyData; }
            set { bodyData = value; }
        }

        private string bodyGeneral = "";

        /// <summary>
        /// 用户附加内容
        /// </summary>
        [JsonPropertyAttribute("bodyGeneral")]
        public string BodyGeneral
        {
            get { return bodyGeneral; }
            set { bodyGeneral = value; }
        }

        private string image1Url = "";

        /// <summary>
        /// 图片1地址
        /// </summary>
        [JsonPropertyAttribute("image1")]
        public string Image1Url
        {
            get { return image1Url; }
            set { image1Url = value; }
        }

        private string image1Link = "";

        /// <summary>
        /// 图片1链接地址
        /// </summary>
        [JsonPropertyAttribute("image1Link")]
        public string Image1Link
        {
            get { return image1Link; }
            set { image1Link = value; }
        }

        private string image2Url = "";

        /// <summary>
        /// 图片2地址
        /// </summary>
        [JsonPropertyAttribute("image2")]
        public string Image2Url
        {
            get { return image2Url; }
            set { image2Url = value; }
        }

        private string image2Link = "";

        /// <summary>
        /// 图片2链接地址
        /// </summary>
        [JsonPropertyAttribute("image2Link")]
        public string Image2Link
        {
            get { return image2Link; }
            set { image2Link = value; }
        }

        private string image3Url = "";

        /// <summary>
        /// 图片3地址
        /// </summary>
        [JsonPropertyAttribute("image3")]
        public string Image3Url
        {
            get { return image3Url; }
            set { image3Url = value; }
        }

        private string image3Link = "";

        /// <summary>
        /// 图片3链接地址
        /// </summary>
        [JsonPropertyAttribute("image3Link")]
        public string Image3Link
        {
            get { return image3Link; }
            set { image3Link = value; }
        }

        private string image4Url = "";

        /// <summary>
        /// 图片4地址
        /// </summary>
        [JsonPropertyAttribute("image4")]
        public string Image4Url
        {
            get { return image4Url; }
            set { image4Url = value; }
        }

        private string image4Link = "";

        /// <summary>
        /// 图片4链接地址
        /// </summary>
        [JsonPropertyAttribute("image4Link")]
        public string Image4Link
        {
            get { return image4Link; }
            set { image4Link = value; }
        }

        private string targetIds = "";
        [JsonPropertyAttribute("targetIds")]
        public string TargetIds
        {
            get { return targetIds; }
            set { targetIds = value; }
        }

        private string privacy = "";
        [JsonPropertyAttribute("privacy")]
        public string Privacy
        {
            get { return privacy; }
            set { privacy = value; }
        }

        private string hashTemplate = "";
        [JsonPropertyAttribute("hashTemplate")]
        public string HashTemplate
        {
            get { return hashTemplate; }
            set { hashTemplate = value; }
        }

        private Dictionary<string, string> hashData = new Dictionary<string, string>();
        [JsonPropertyAttribute("hashData")]
        public Dictionary<string, string> HashData
        {
            get { return hashData; }
            set { hashData = value; }
        }
    }

    #endregion

}
