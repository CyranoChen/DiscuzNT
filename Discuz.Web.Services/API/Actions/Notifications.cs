using System;
using Discuz.Common;
using Discuz.Entity;
using Discuz.Forum;
using Newtonsoft.Json;

namespace Discuz.Web.Services.API.Actions
{
    public class Notifications : Actions.ActionBase
    {

        /// <summary>
        /// 发送通知
        /// </summary>
        /// <returns></returns>
        public string Send()
        {

            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }
                if (Discuz.Forum.UserGroups.GetUserGroupInfo(Discuz.Forum.Users.GetShortUserInfo(Uid).Groupid).Radminid != 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_PERMISSION_DENIED;
                    return "";
                }
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            if (!CheckRequiredParams("notification"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            if (GetParam("to_ids") == null)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            if (Uid < 1 && (!CheckRequiredParams("to_ids") || !Utils.IsNumericArray(GetParam("to_ids").ToString().Split(','))))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            string ids = GetParam("to_ids").ToString();

            string notification = GetParam("notification").ToString();

            string[] to_ids;
            if (ids == string.Empty)
            {
                to_ids = new string[1];
                to_ids[0] = Uid.ToString();
            }
            else
            {
                to_ids = GetParam("to_ids").ToString().Split(',');
            }

            string successfulIds = string.Empty;
            ShortUserInfo shortUserInfo = null;
            if (Uid > 0)
            {
                shortUserInfo = Discuz.Forum.Users.GetShortUserInfo(Uid);
            }
            foreach (string id in to_ids)
            {
                if (Utils.StrToInt(id, 0) < 1)
                {
                    continue;
                }
                NoticeInfo noticeinfo = new NoticeInfo();
                noticeinfo.Uid = Utils.StrToInt(id, 0);
                noticeinfo.New = 1;
                noticeinfo.Postdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (Uid > 0)
                {
                    //ShortUserInfo shortUserInfo = Discuz.Forum.Users.GetShortUserInfo(Uid);
                    noticeinfo.Poster = shortUserInfo == null ? "" : shortUserInfo.Username;
                    noticeinfo.Posterid = Uid;
                }
                else
                {
                    noticeinfo.Poster = "";
                    noticeinfo.Posterid = 0;
                }
                noticeinfo.Note = Utils.EncodeHtml(notification);//需要做ubb标签转换

                if (Notices.CreateNoticeInfo(noticeinfo) > 0)
                {
                    successfulIds += (id + ",");
                }

            }

            if (successfulIds.Length > 0)
                successfulIds = successfulIds.Remove(successfulIds.Length - 1);
            if (Format == FormatType.JSON)
                return string.Format("\"{0}\"", successfulIds);


            NotificationSendResponse nsr = new NotificationSendResponse();
            nsr.Result = successfulIds;
            return SerializationHelper.Serialize(nsr);
        }

        /// <summary>
        /// 发送email通知
        /// </summary>
        /// <returns></returns>
        public string SendEmail()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }
                if (Discuz.Forum.UserGroups.GetUserGroupInfo(Discuz.Forum.Users.GetShortUserInfo(Uid).Groupid).Radminid != 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_PERMISSION_DENIED;
                    return "";
                }
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            //	 recipients subject 
            if (!CheckRequiredParams("recipients,subject,text"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            if (!Utils.IsNumericArray(GetParam("recipients").ToString().Split(',')))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            //需要过滤部分html标签，待开发
            //得到了 用逗号分隔的ids 和 subject，先通过ids得到所有人的用户名
            string uids = Discuz.Forum.Emails.SendMailToUsers(GetParam("recipients").ToString(), GetParam("subject").ToString(), GetParam("text").ToString());

            if (Format == FormatType.JSON)
                return string.Format("\"{0}\"", uids);


            NotificationSendEmailResponse nser = new NotificationSendEmailResponse();
            nser.Recipients = uids;
            return SerializationHelper.Serialize(nser);
        }

        /// <summary>
        /// 获取用户通知
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            if (Uid < 1)
            {
                ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                return "";
            }

            //get unread and mostrecent message/notification count
            NotificationGetResponse notification = new NotificationGetResponse();
            notification.Message = new Notification();
            notification.Message.Unread = Discuz.Forum.PrivateMessages.GetPrivateMessageCount(Uid, 0, 1);
            notification.Message.MostRecent = Discuz.Forum.PrivateMessages.GetLatestPMID(Uid);


            notification.Notification = new Notification();
            notification.Notification.Unread = Discuz.Forum.Notices.GetNoticeCount(Uid, 1);
            notification.Notification.MostRecent = Discuz.Forum.Notices.GetLatestNoticeID(Uid);

            if (Format == FormatType.JSON)
                return JavaScriptConvert.SerializeObject(notification);

            return SerializationHelper.Serialize(notification);
        }
    }
}
