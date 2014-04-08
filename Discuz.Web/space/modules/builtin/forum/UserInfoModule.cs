using System;
using System.Text;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Entities;
 
using Discuz.Space.Utilities;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Space.Provider;
using System.Text.RegularExpressions;

namespace Discuz.Space.Modules.Forum
{
	/// <summary>
	/// UserInfoModule 的摘要说明。
	/// </summary>
	public class UserInfoModule : ModuleBase
	{
		private string contentTemplate = Utils.GetMapPath(BaseConfigs.GetForumPath + "/space/Modules/builtin/forum/userinfomodule.config");

		public UserInfoModule()
		{			
		}

		public override string GetModulePost(System.Web.HttpContext httpContext)
		{
			return base.GetModulePost (httpContext);
		}

		public override string OnEditBoxLoad(string editbox)
		{
			this.Editable = true;
			UserPrefsSaved userprefs = new UserPrefsSaved(this.Module.UserPref);
			string value = userprefs.GetValueByName("showaddons");
			value = value == "" ? "0" : value;
			string checkedvalue = "";
			if (value == "1")
				checkedvalue = "checked";
			editbox = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"95%\" align=\"center\">";
			editbox += string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\"><nobr>{0}{4}</nobr></td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<input id=\"m___MODULE_ID___0\" name=\"m___MODULE_ID___up_{3}\" value=\"{1}\" type=\"hidden\" /><input type=\"checkbox\" {2} onclick=\"_gel('m___MODULE_ID___0').value = this.checked ? '1' : '0';\" /></td></tr>", "显示附加信息", value, checkedvalue, "showaddons", "");
            value = userprefs.GetValueByName("isvertical");
            value = value == "" ? "0" : value;
            if (value == "1")
                checkedvalue = "checked";
            else
                checkedvalue = "";

            editbox += string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\"><nobr>{0}{4}</nobr></td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<input id=\"m___MODULE_ID___1\" name=\"m___MODULE_ID___up_{3}\" value=\"{1}\" type=\"hidden\" /><input type=\"checkbox\" {2} onclick=\"_gel('m___MODULE_ID___1').value = this.checked ? '1' : '0';\" /></td></tr>", "竖排显示", value, checkedvalue, "isvertical", "");
			editbox += "</table>";
			return base.OnEditBoxLoad (editbox);
		}

		public override string OnMouduleLoad(string content)
		{
			UserPrefsSaved userprefs = new UserPrefsSaved(this.Module.UserPref);
			string value = userprefs.GetValueByName("showaddons");
            string vertical = userprefs.GetValueByName("isvertical");
			string[] score = Scoresets.GetValidScoreName();
			UserInfo ui = Users.GetUserInfo(this.Module.Uid);
			UserGroupInfo group = UserGroups.GetUserGroupInfo(ui.Groupid);
			content = StaticFileProvider.GetContent(contentTemplate);

            if (vertical == "1")
            {
                content = Regex.Replace(content, @"<div id=""UserInfo""([\s\S]+?)</div>\r\n</div>", "");
            }
            else
            {
                content = Regex.Replace(content, @"<div id=""UserInfo2""([\s\S]+?)</div>\r\n</div>", "");
            }

			content = content.Replace("${username}", ui.Username);
			string avatar = string.Empty;
            //if (ui.Avatar != string.Empty)
            //{
            //    if (Regex.IsMatch(ui.Avatar, @"^(http://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"))
            //        avatar = string.Format("<img src='{0}'", ui.Avatar);
            //    else if (ui.Avatar.StartsWith("avatars"))                
            //        avatar = string.Format("<img src='{0}'", BaseConfigs.GetForumPath + ui.Avatar.Replace('\\','/'));
            //    else
            //        avatar = string.Format("<img src='{0}'", ui.Avatar.Replace('\\', '/'));
            //    if (ui.Avatarwidth > 0)
            //        avatar += string.Format(" width='{0}' height='{1}' ", ui.Avatarwidth, ui.Avatarheight);
            //    avatar += "/>";
            //}
			content = content.Replace("${useravatar}", avatar);
			content = content.Replace("${userid}", ui.Uid.ToString());
			content = content.Replace("${nickname}", ui.Nickname);
			content = content.Replace("${usergroup}", group.Grouptitle);
			content = content.Replace("${usercredits}", ui.Credits.ToString());
            content = content.Replace("${forumpath}", BaseConfigs.GetForumPath);

			string addoninfo = string.Empty;
			if (value == "1")
			{
				if (score[1] != "")
					addoninfo += string.Format("<li><span>{0}: </span>{1}</li>", score[1], ui.Extcredits1);
				if (score[2] != "")
					addoninfo += string.Format("<li><span>{0}: </span>{1}</li>", score[2], ui.Extcredits2);
				if (score[3] != "")
					addoninfo += string.Format("<li><span>{0}: </span>{1}</li>", score[3], ui.Extcredits3);
				if (score[4] != "")
					addoninfo += string.Format("<li><span>{0}: </span>{1}</li>", score[4], ui.Extcredits4);
				if (score[5] != "")
					addoninfo += string.Format("<li><span>{0}: </span>{1}</li>", score[5], ui.Extcredits5);
				if (score[6] != "")
					addoninfo += string.Format("<li><span>{0}: </span>{1}</li>", score[6], ui.Extcredits6);
				if (score[7] != "")
					addoninfo += string.Format("<li><span>{0}: </span>{1}</li>", score[7], ui.Extcredits7);
				if (score[8] != "")
					addoninfo += string.Format("<li><span>{0}: </span>{1}</li>", score[8], ui.Extcredits8);
			}
			content = content.Replace("${addoninfo}", addoninfo);

			return base.OnMouduleLoad (content);
		}
	}
}
