using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Forum;
using Newtonsoft.Json;

namespace Discuz.Web.Services.Manyou.Actions
{
    public class Request : ActionBase
    {
        public string Send()
        {
            SendApplicationInviteParams actionParams = JavaScriptConvert.DeserializeObject<SendApplicationInviteParams>(UnicodeToString(JsonParams));

            Dictionary<string,int> inviteTable = new Dictionary<string,int>();

            foreach (string toUid in actionParams.RecipientIds)
            {
                UserApplicationInviteInfo userAppInviteInfo = new UserApplicationInviteInfo();
                userAppInviteInfo.AppId = int.Parse(actionParams.AppId);
                userAppInviteInfo.FromUid = actionParams.UId;
                userAppInviteInfo.Hash = 1;
                userAppInviteInfo.MYML = actionParams.MYML;
                userAppInviteInfo.ToUid = int.Parse(toUid);
                userAppInviteInfo.Type = actionParams.Type == "invite" ? 0 : 1;
                userAppInviteInfo.TypeName = actionParams.RequestName;
                inviteTable.Add(toUid, ManyouApplications.SendApplicationInvite(userAppInviteInfo));
            }

            return GetResult(inviteTable);
        }
    }
}
