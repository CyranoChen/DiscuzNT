using System;
using System.Text;
using System.Data;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Forum;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Discuz.Web.Services.Manyou.Actions
{
    public class UserApplication : ActionBase
    {
        public string Add()
        {
            AddUserApplicationParams actionParam = JavaScriptConvert.DeserializeObject<AddUserApplicationParams>(UnicodeToString(JsonParams));//当传递过来的参数值可能存在unicode编码的字符串时需要使用UnicodeToString方法

            UserApplicationInfo userAppInfo = new UserApplicationInfo();
            userAppInfo.AppId = actionParam.AppId;
            userAppInfo.AppName = actionParam.AppName;
            userAppInfo.Uid = actionParam.Uid;
            userAppInfo.AllowFeed = actionParam.AllowFeed == null || (bool)actionParam.AllowFeed ? 1 : 0;//可不传递型属性处理实例
            userAppInfo.AllowProfileLink = actionParam.AllowProfileLink == null || (bool)actionParam.AllowProfileLink ? 1 : 0;
            userAppInfo.AllowSideNav = actionParam.AllowSideNav == null || (bool)actionParam.AllowSideNav ? 1 : 0;
            userAppInfo.DisplayOrder = actionParam.DisplayOrder == null ? 0 : (int)actionParam.DisplayOrder;
            userAppInfo.Privacy = Utils.GetEnum<PrivacyEnum>(actionParam.Privacy, PrivacyEnum.Public);

            //不确定代码
            userAppInfo.ProfileLink = actionParam.DefaultProfileLink;
            userAppInfo.MYML = actionParam.DefaultMYML;

            //当用户添加应用的时候，会在全局应用表中也增加或者更新原有记录，保证数据同步
            ManyouApplicationInfo appInfo = new ManyouApplicationInfo();
            appInfo.AppId = actionParam.AppId;
            appInfo.AppName = actionParam.AppName;
            appInfo.DisplayMethod = Utils.GetEnum<DisplayMethod>(actionParam.DisplayMethod, DisplayMethod.IFrame);
            appInfo.DisplayOrder = actionParam.DisplayOrder ?? 0;
            appInfo.Version = TypeConverter.StrToInt(actionParam.Version);

            ManyouApplications.UpdateApplicationInfo(appInfo);

            return GetResult(ManyouApplications.AddUserApplication(userAppInfo) > -1);
        }

        public string Update()
        {
            UpdateUserApplicationParams actionParam = JavaScriptConvert.DeserializeObject<UpdateUserApplicationParams>(UnicodeToString(JsonParams));

            UserApplicationInfo userAppInfo = new UserApplicationInfo();
            userAppInfo.AppId = actionParam.AppIds[0];
            userAppInfo.AppName = actionParam.AppName;
            userAppInfo.Uid = actionParam.Uid;
            userAppInfo.AllowFeed = actionParam.AllowFeed == null || (bool)actionParam.AllowFeed ? 1 : 0;//可不传递型属性处理实例
            userAppInfo.AllowProfileLink = actionParam.AllowProfileLink == null || (bool)actionParam.AllowProfileLink ? 1 : 0;
            userAppInfo.AllowSideNav = actionParam.AllowSideNav == null || (bool)actionParam.AllowSideNav ? 1 : 0;
            userAppInfo.DisplayOrder = actionParam.DisplayOrder == null ? 0 : (int)actionParam.DisplayOrder;
            userAppInfo.Privacy = Utils.GetEnum<PrivacyEnum>(actionParam.Privacy, PrivacyEnum.Public);
            //不确定代码
            userAppInfo.ProfileLink = actionParam.DefaultProfileLink;
            userAppInfo.MYML = actionParam.DefaultMYML;

            return GetResult(ManyouApplications.UpdateUserApplication(userAppInfo) > -1);
        }

        public string Remove()
        {
            RemoveUserApplicationParams actionParams = JavaScriptConvert.DeserializeObject<RemoveUserApplicationParams>(JsonParams);

            return GetResult(ManyouApplications.RemoveUserApplication(actionParams.Uid, actionParams.AppIdList) > -1);
        }

        public string GetInstalled()
        {
            GetUserInstalledApplicationIdParams actionParams = JavaScriptConvert.DeserializeObject<GetUserInstalledApplicationIdParams>(JsonParams);
            return GetResult(ManyouApplications.GetUserInstalledApplicationId(actionParams.Uid));
        }

        public string Get()
        {
            GetUserApplicationMessageParams actionParams = JavaScriptConvert.DeserializeObject<GetUserApplicationMessageParams>(JsonParams);

            List<UserApplicationInfo> userAppList = ManyouApplications.GetUserApplicationInfo(actionParams.Uid, actionParams.AppIdList);

            List<UserApplicationMessage> userAppMsgList = new List<UserApplicationMessage>();

            foreach (UserApplicationInfo appInfo in userAppList)
            {
                userAppMsgList.Add(LoadSingleUserApplicationMessage(appInfo));
            }

            return GetResult(userAppMsgList);
        }

        #region private method

        private string GetPrivacyString(PrivacyEnum privacy)
        {
            switch (privacy)
            {
                case PrivacyEnum.Friends: return "friends";
                case PrivacyEnum.Me: return "me";
                case PrivacyEnum.None: return "none";
                case PrivacyEnum.Public: return "public";
                default: return "public";
            }
        }

        private UserApplicationMessage LoadSingleUserApplicationMessage(UserApplicationInfo userAppInfo)
        {
            UserApplicationMessage appMsg = new UserApplicationMessage();
            appMsg.AllowFeed = userAppInfo.AllowFeed == 1;
            appMsg.AllowProfileLink = userAppInfo.AllowProfileLink == 1;
            appMsg.AllowSideNav = userAppInfo.AllowSideNav == 1;
            appMsg.AppId = userAppInfo.AppId;
            appMsg.DisplayOrder = userAppInfo.DisplayOrder;
            appMsg.Privacy = GetPrivacyString(userAppInfo.Privacy);
            return appMsg;
        }

        #endregion
    }
}
