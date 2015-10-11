using System;
using System.Data;
using Discuz.Common.Generic;
using System.Text;
using Discuz.Entity;
using Discuz.Common;
using System.Collections;

namespace Discuz.Data
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
            return DatabaseProvider.GetInstance().AddUserApplication(userAppInfo);
        }

        /// <summary>
        /// 更新用户应用信息
        /// </summary>
        /// <param name="userAppInfo"></param>
        /// <returns></returns>
        public static int UpdateUserApplication(UserApplicationInfo userAppInfo)
        {
            return DatabaseProvider.GetInstance().UpdateUserApplication(userAppInfo);
        }

        /// <summary>
        /// 用户移除应用
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="appIds"></param>
        /// <returns></returns>
        public static int RemoveUserApplication(int uid, string appIds)
        {
            return DatabaseProvider.GetInstance().RemoveUserApplication(uid, appIds);
        }

        /// <summary>
        /// 获取用户安装的应用信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="appIdList">应用idlist</param>
        /// <returns></returns>
        public static List<UserApplicationInfo> GetUserApplicationInfo(int uid, string appIdList)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUserApplicationInfo(uid, appIdList);
            List<UserApplicationInfo> userAppList = new List<UserApplicationInfo>();

            while (reader.Read())
            {
                userAppList.Add(LoadUserApplicationInfo(reader));
            }
            reader.Close();
            return userAppList;
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
            IDataReader reader = DatabaseProvider.GetInstance().GetUserInstalledApplication(uid, pageIndex, pageSize);

            List<UserApplicationInfo> userAppList = new List<UserApplicationInfo>();

            while (reader.Read())
            {
                userAppList.Add(LoadShortUserApplicationInfo(reader));
            }
            reader.Close();

            return userAppList;
        }

        /// <summary>
        /// 对用户发送应用邀请
        /// </summary>
        /// <param name="userAppInviteInfo"></param>
        /// <returns></returns>
        public static int SendApplicationInvite(UserApplicationInviteInfo userAppInviteInfo)
        {
            return DatabaseProvider.GetInstance().SendApplicationInvite(userAppInviteInfo);
        }

        /// <summary>
        /// 读取注册用户的动作日志
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<UserLogInfo> GetUserLog(int count)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUserLog(count);

            List<UserLogInfo> userLogList = new List<UserLogInfo>();
            while (reader.Read())
            {
                UserLogInfo logInfo = new UserLogInfo();
                logInfo.UId = TypeConverter.ObjectToInt(reader["uid"]);
                logInfo.Action = Utils.GetEnum<UserLogActionEnum>(reader["action"].ToString(), UserLogActionEnum.Update);
                logInfo.DateTime = reader["datetime"].ToString();
                userLogList.Add(logInfo);
            }
            reader.Close();
            return userLogList;
        }

        /// <summary>
        /// 记录用户动作日志
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static int AddUserLog(int uid, string action)
        {
            return DatabaseProvider.GetInstance().AddUserLog(uid, action);
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
            IDataReader reader = DatabaseProvider.GetInstance().GetApplicationInviteList(uid, pageIndex, pageSize);

            List<ApplicationInviteInfo> applicationInviteList = new List<ApplicationInviteInfo>();
            while (reader.Read())
            {
                ApplicationInviteInfo appInviteInfo = new ApplicationInviteInfo();
                appInviteInfo.AppId = TypeConverter.ObjectToInt(reader["appid"]);
                appInviteInfo.DateTime = reader["datetime"].ToString();
                appInviteInfo.FromUid = TypeConverter.ObjectToInt(reader["fromuid"]);
                appInviteInfo.Hash = TypeConverter.ObjectToInt(reader["hash"]);
                appInviteInfo.Id = TypeConverter.ObjectToInt(reader["id"]);
                appInviteInfo.MYML = reader["myml"].ToString();
                appInviteInfo.ToUid = TypeConverter.ObjectToInt(reader["touid"]);
                appInviteInfo.Type = TypeConverter.ObjectToInt(reader["type"]);
                appInviteInfo.TypeName = reader["typename"].ToString();
                applicationInviteList.Add(appInviteInfo);
            }
            reader.Close();
            return applicationInviteList;

        }

        /// <summary>
        /// 获取用户应用邀请的个数
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int GetApplicationInviteCount(int uid)
        {
            return DatabaseProvider.GetInstance().GetApplicationInviteCount(uid);
        }

        /// <summary>
        /// 忽略应用邀请
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public static int IgnoreApplicationInvite(string idList)
        {
            return DatabaseProvider.GetInstance().IgnoreApplicationInvite(idList);
        }

        /// <summary>
        /// 删除用户指定应用的邀请
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static int DeleteApplicationInviteByAppId(int uid, int appId)
        {
            return DatabaseProvider.GetInstance().DeleteApplicationInviteByAppId(uid, appId);
        }

        public static int UpdateApplicationInfo(ManyouApplicationInfo appInfo)
        {
            return DatabaseProvider.GetInstance().UpdateApplicationInfo(appInfo);
        }

        public static int SetApplicationFlag(string appIdList, string appNameList, int flag)
        {
            return DatabaseProvider.GetInstance().SetApplicationFlag(appIdList, appNameList, flag);
        }

        public static int RemoveApplication(string appIdList)
        {
            return DatabaseProvider.GetInstance().RemoveApplication(appIdList);
        }

        public static int[] GetUserInstalledApplicationId(int uid)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUserInstalledApplicationId(uid);

            List<int> appIdList = new List<int>();
            while (reader.Read())
            {
                appIdList.Add(TypeConverter.ObjectToInt(reader["appid"]));
            }
            reader.Close();
            return appIdList.ToArray();
        }

        public static ManyouApplicationInfo GetApplicationInfo(int appId)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetApplicationInfo(appId);
            ManyouApplicationInfo appInfo = new ManyouApplicationInfo();

            if (reader.Read())
            {
                appInfo = LoadManyouApplicationInfo(reader);
            }
            reader.Close();
            return appInfo;
        }

        #region private method

        private static ManyouApplicationInfo LoadManyouApplicationInfo(IDataReader reader)
        {
            ManyouApplicationInfo appInfo = new ManyouApplicationInfo();

            appInfo.AppId = TypeConverter.ObjectToInt(reader["appid"]);
            appInfo.AppName = reader["appname"].ToString();
            appInfo.DisplayMethod = (DisplayMethod)TypeConverter.ObjectToInt(reader["displaymethod"]);
            appInfo.DisplayOrder = TypeConverter.ObjectToInt(reader["displayorder"]);
            appInfo.Flag = (ApplicationFlag)TypeConverter.ObjectToInt(reader["flag"]);
            appInfo.Narrow = TypeConverter.ObjectToInt(reader["narrow"]);
            appInfo.Version = TypeConverter.ObjectToInt(reader["version"]);

            return appInfo;
        }

        private static UserApplicationInfo LoadUserApplicationInfo(IDataReader reader)
        {
            UserApplicationInfo userAppInfo = new UserApplicationInfo();

            userAppInfo.AppId = TypeConverter.ObjectToInt(reader["appid"]);
            userAppInfo.AppName = reader["appname"].ToString();
            userAppInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
            userAppInfo.AllowSideNav = TypeConverter.ObjectToInt(reader["allowsidenav"]);
            userAppInfo.AllowFeed = TypeConverter.ObjectToInt(reader["allowfeed"]);
            userAppInfo.AllowProfileLink = TypeConverter.ObjectToInt(reader["allowprofilelink"]);
            userAppInfo.Narrow = TypeConverter.ObjectToInt(reader["narrow"]);
            userAppInfo.DisplayOrder = TypeConverter.ObjectToInt(reader["displayorder"]);
            userAppInfo.MenuOrder = TypeConverter.ObjectToInt(reader["menuorder"]);
            userAppInfo.ProfileLink = reader["profilelink"].ToString();
            userAppInfo.MYML = reader["myml"].ToString();

            return userAppInfo;
        }

        private static UserApplicationInfo LoadShortUserApplicationInfo(IDataReader reader)
        {
            UserApplicationInfo userAppInfo = new UserApplicationInfo();

            userAppInfo.AppId = TypeConverter.ObjectToInt(reader["appid"]);
            userAppInfo.AppName = reader["appname"].ToString();
            userAppInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
            userAppInfo.DisplayOrder = TypeConverter.ObjectToInt(reader["displayorder"]);
            userAppInfo.MenuOrder = TypeConverter.ObjectToInt(reader["menuorder"]);

            return userAppInfo;

        }
        #endregion
    }
}
