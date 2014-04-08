using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Forum;

namespace Discuz.Web.Services.Manyou.Actions
{
    public class Application : ActionBase
    {
        public string Update()
        {
            UpdateApplicationParams actionParams = JavaScriptConvert.DeserializeObject<UpdateApplicationParams>(UnicodeToString(JsonParams));
            ManyouApplicationInfo appInfo = new ManyouApplicationInfo();
            appInfo.AppId = actionParams.AppId;
            appInfo.AppName = actionParams.AppName;
            appInfo.DisplayMethod = Utils.GetEnum<DisplayMethod>(actionParams.DisplayMethod, DisplayMethod.IFrame);
            appInfo.DisplayOrder = actionParams.DisplayOrder;
            appInfo.Version = actionParams.Version;

            return GetResult(ManyouApplications.UpdateApplicationInfo(appInfo));
        }

        public string Remove()
        {
            RemoveApplicationParams actionParams = JavaScriptConvert.DeserializeObject<RemoveApplicationParams>(JsonParams);

            int result = ManyouApplications.RemoveApplication(actionParams.AppIdList);

            return GetResult(result > 0 ? result : 1);
        }

        public string SetFlag()
        {
            SetApplicationFlagParams actionParams = JavaScriptConvert.DeserializeObject<SetApplicationFlagParams>(UnicodeToString(JsonParams));
            string appIdList = "";
            string appNameList = "";

            foreach (ApplicationInfo appInfo in actionParams.Applications)
            {
                appIdList += appInfo.AppId + ",";
                appNameList += appInfo.AppName + ",";
            }

            return GetResult(0 <= ManyouApplications.SetApplicationFlag(appIdList.TrimEnd(','), appNameList.TrimEnd(','), Utils.GetEnum<ApplicationFlag>(actionParams.Flag, ApplicationFlag.Normal)));
        }
    }
}
