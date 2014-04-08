using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Discuz.Entity;
using Discuz.Forum;

namespace Discuz.Web.Services.Manyou.Actions
{
    public class Profile : ActionBase
    {
        public string SetMYML()
        {
            return UpdateUserAppFieldsInfo();
        }

        public string SetActionLink()
        {
            return UpdateUserAppFieldsInfo();
        }

        private string UpdateUserAppFieldsInfo()
        {
            SetUserAppFieldsInfoParams actionParams = JavaScriptConvert.DeserializeObject<SetUserAppFieldsInfoParams>(UnicodeToString(JsonParams));

            UserApplicationInfo userAppInfo = ManyouApplications.GetUserApplicationInfo(actionParams.UId, actionParams.AppId.ToString())[0];

            userAppInfo.MYML = actionParams.Markup != "" ? actionParams.Markup : userAppInfo.MYML;
            userAppInfo.ProfileLink = actionParams.ActionMarkup != "" ? actionParams.ActionMarkup : userAppInfo.ProfileLink;

            return GetResult(ManyouApplications.UpdateUserApplication(userAppInfo));
        }
    }
}
