using System;
using System.Text;
using Discuz.Entity;
using Discuz.Data;
using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Config;

namespace Discuz.Forum
{
    public class ManyouApplications
    {
        /// <summary>
        /// 用户增加新应用
        /// </summary>
        /// <param name="userAppInfo"></param>
        /// <returns></returns>
        public static int AddUserApplication(UserApplicationInfo userAppInfo)
        {
            if (userAppInfo == null || userAppInfo.Uid < 1 || userAppInfo.AppId < 1)
                return -1;

            int result = Data.ManyouApplications.AddUserApplication(userAppInfo);

            DeleteApplicationInviteByAppId(userAppInfo.Uid, userAppInfo.AppId);

            int olId = OnlineUsers.GetOlidByUid(userAppInfo.Uid);

            if (olId > 0) //更新在线表信息
                OnlineUsers.UpdateNewApplicationRequest(olId, ManyouApplications.GetApplicationInviteCount(userAppInfo.Uid));

            return result;
        }
        /// <summary>
        /// 更新用户应用信息
        /// </summary>
        /// <param name="userAppInfo"></param>
        /// <returns></returns>
        public static int UpdateUserApplication(UserApplicationInfo userAppInfo)
        {
            if (userAppInfo == null || userAppInfo.Uid < 1 || userAppInfo.AppId < 1)
                return -1;

            return Data.ManyouApplications.UpdateUserApplication(userAppInfo);
        }
        /// <summary>
        /// 用户移除应用
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="appIds"></param>
        /// <returns></returns>
        public static int RemoveUserApplication(int uid, string appIds)
        {
            if (uid < 1 || appIds == "")
                return -1;
            return Data.ManyouApplications.RemoveUserApplication(uid, appIds);
        }
        /// <summary>
        /// 获取用户安装的应用信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static List<UserApplicationInfo> GetUserApplicationInfo(int uid, string appIdList)
        {
            return Data.ManyouApplications.GetUserApplicationInfo(uid, appIdList);
        }
        /// <summary>
        /// 对用户发送应用邀请
        /// </summary>
        /// <param name="userAppInviteInfo"></param>
        /// <returns></returns>
        public static int SendApplicationInvite(UserApplicationInviteInfo userAppInviteInfo)
        {
            int inviteId = Data.ManyouApplications.SendApplicationInvite(userAppInviteInfo);

            if (inviteId > 0)
            {
                int olId = OnlineUsers.GetOlidByUid(userAppInviteInfo.ToUid);
                if (olId > 0) //更新在线表信息
                    OnlineUsers.UpdateNewApplicationRequest(olId, ManyouApplications.GetApplicationInviteCount(userAppInviteInfo.ToUid));
            }
            return inviteId;
        }
        /// <summary>
        /// 发送应用通知
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="toUid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int SendApplicationNotice(int uid, int toUid, string message)
        {
            ShortUserInfo userInfo = Users.GetShortUserInfo(uid);

            if (userInfo == null || userInfo.Uid <= 0)
                return 0;

            NoticeInfo noticeinfo = new NoticeInfo();
            noticeinfo.Note = Utils.HtmlEncode(message);
            noticeinfo.Type = Noticetype.ApplicationNotice;
            noticeinfo.New = 1;
            noticeinfo.Posterid = userInfo.Uid;
            noticeinfo.Poster = userInfo.Username.Trim();
            noticeinfo.Postdatetime = Utils.GetDateTime();
            noticeinfo.Uid = toUid;

            return Notices.CreateNoticeInfo(noticeinfo);
        }
        /// <summary>
        /// 读取注册用户的动作日志
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<UserLogInfo> GetUserLog(int count)
        {
            if (count <= 0)
                return new List<UserLogInfo>();
            return Data.ManyouApplications.GetUserLog(count);
        }
        /// <summary>
        /// 记录用户动作日志
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static int AddUserLog(int uid, UserLogActionEnum action)
        {
            if (uid <= 0)
                return -1;

            return Data.ManyouApplications.AddUserLog(uid, action.ToString().ToLower());
        }
        /// <summary>
        /// 获取用户应用邀请列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<ApplicationInviteInfo> GetApplicationInviteList(int uid, int pageIndex, int pageSize)
        {
            if (uid <= 0 || pageIndex <= 0 || pageSize <= 0)
                return new List<ApplicationInviteInfo>();

            return Data.ManyouApplications.GetApplicationInviteList(uid, pageIndex, pageSize);
        }
        /// <summary>
        /// 获取用户应用邀请的个数
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int GetApplicationInviteCount(int uid)
        {
            if (uid <= 0)
                return 0;

            return Data.ManyouApplications.GetApplicationInviteCount(uid);
        }
        /// <summary>
        /// 忽略应用邀请
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public static int IgnoreApplicationInvite(string idList)
        {
            if (!Utils.IsNumericList(idList))
                return -1;

            return Data.ManyouApplications.IgnoreApplicationInvite(idList);
        }
        /// <summary>
        /// 删除用户指定应用的邀请
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        private static int DeleteApplicationInviteByAppId(int uid, int appId)
        {
            if (uid <= 0 || appId <= 0)
                return -1;
            return Data.ManyouApplications.DeleteApplicationInviteByAppId(uid, appId);
        }
        /// <summary>
        /// 获取用户已安装的应用列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<UserApplicationInfo> GetUserInstalledApplication(int uid, int pageIndex, int pageSize)
        {
            if (uid <= 0 || pageIndex <= 0 || pageSize <= 0)
                return new List<UserApplicationInfo>();

            return Data.ManyouApplications.GetUserInstalledApplication(uid, pageIndex, pageSize);
        }
        /// <summary>
        /// 更新应用配置信息
        /// </summary>
        /// <param name="appInfo"></param>
        /// <returns>返回影响数据行数</returns>
        public static int UpdateApplicationInfo(ManyouApplicationInfo appInfo)
        {
            if (appInfo.AppId <= 0)
                return 0;

            return Data.ManyouApplications.UpdateApplicationInfo(appInfo);
        }
        /// <summary>
        /// 设置应用程序状态
        /// </summary>
        /// <param name="appIdList"></param>
        /// <param name="appNameList"></param>
        /// <param name="flag"></param>
        /// <returns>返回影响数据行数</returns>
        public static int SetApplicationFlag(string appIdList, string appNameList, ApplicationFlag flag)
        {
            if (!Utils.IsNumericList(appIdList) || appIdList.Split(',').Length != appNameList.Split(',').Length)
                return 0;
            return Data.ManyouApplications.SetApplicationFlag(appIdList, appNameList, (int)flag);
        }
        /// <summary>
        /// 关闭应用
        /// </summary>
        /// <param name="appIdList"></param>
        /// <returns>返回影响数据行数</returns>
        public static int RemoveApplication(string appIdList)
        {
            if (!Utils.IsNumericList(appIdList))
                return 0;
            return Data.ManyouApplications.RemoveApplication(appIdList);
        }
        /// <summary>
        /// 获取用户已安装的应用id数组
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int[] GetUserInstalledApplicationId(int uid)
        {
            if (uid <= 0)
                return new int[0];
            return Data.ManyouApplications.GetUserInstalledApplicationId(uid);
        }
        /// <summary>
        /// 获取全局站点应用信息
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static ManyouApplicationInfo GetApplicationInfo(int appId)
        {
            if (appId <= 0)
                return null;

            return Data.ManyouApplications.GetApplicationInfo(appId);
        }

        /// <summary>
        /// 通知漫游平台同步指定用户的好友列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="manyouVersion">该参数在正式环境下请填写string.Empty</param>
        public static string GetSyncManyouFriendsUrl(int uid, string manyouVersion)
        {
            if (uid <= 0)
                return "";

            GeneralConfigInfo config = GeneralConfigs.GetConfig();

            string timeStamp = Utils.ConvertToUnixTimestamp(DateTime.Now).ToString();
            string syncUrl = string.Format("http://uchome" + manyouVersion + ".manyou.com/user/syncFriends?sId={0}&uUchId={1}&ts={2}&key={3}", config.Mysiteid, uid, timeStamp, Utils.MD5(config.Mysiteid + config.Mysitekey + uid + timeStamp));
            return syncUrl;
        }
    }
}
