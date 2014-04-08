using System;
using System.Web;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Space;
using Discuz.Space.Entities;
using Discuz.Space.Pages;
 
using Discuz.Space.Utilities;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Space.Provider;

namespace Discuz.Space
{
	/// <summary>
	/// 更多模块
	/// </summary>
	public class moregadgets : SpaceBasePage 
	{
		private SpaceConfigInfo currentUserSpaceConfig = new SpaceConfigInfo();
		private string templatePath = Utils.GetMapPath(BaseConfigs.GetForumPath + "/space/skins/");
		private TimeSpan begints = new TimeSpan(DateTime.Now.Ticks);
		private int oluserid = 0;
		private string olusername = string.Empty;
		private string oluserkey = string.Empty;

		public moregadgets()
		{
			if (this.userid < 1)
			{
				HttpContext.Current.Response.Redirect(forumurlnopage + "/login.aspx?reurl=space");
				return;
			}
			GetOnlineUserInfo();

#if NET1			
            TabInfoCollection tc = Spaces.GetTabInfoCollectionByUserID(this.userid);
#else
            Discuz.Common.Generic.List<TabInfo> tc = Spaces.GetTabInfoCollectionByUserID(this.userid);
#endif
			
            currentUserSpaceConfig = Spaces.GetSpaceConfigByUserId(this.userid);
			int defaultTabId = Spaces.GetDefaultTabId(currentUserSpaceConfig, tc);
			string html = StaticFileProvider.GetContent(templatePath + "moregadgets.htm");
			TemplateEngine te = new TemplateEngine();
			te.Init(html, templatePath);
			te.Put("tabid", defaultTabId);
			te.Put("forumpath", BaseConfigs.GetForumPath);
			te.Put("config", currentUserSpaceConfig);
			te.Put("username", this.olusername);
			te.Put("userid", this.oluserid);
			te.Put("userkey", oluserkey);
            te.Put("forumurlnopage", forumurlnopage);
			double processtime = new TimeSpan(DateTime.Now.Ticks).Subtract(begints).Duration().TotalSeconds;
			te.Put("processtime", processtime);
			html = te.MergeTemplate();
			HttpContext.Current.Response.Clear();
			HttpContext.Current.Response.Write(html);
			HttpContext.Current.Response.End();
		}

		private void GetOnlineUserInfo()
		{
			oluserid = Utils.StrToInt(ForumUtils.GetCookie("userid"), -1);
			OnlineUserInfo oluserinfo = OnlineUsers.UpdateInfo(config.Passwordkey, config.Onlinetimeout);
			olid = oluserinfo.Olid;
			oluserid = oluserinfo.Userid;
			olusername = oluserinfo.Username;
			string password = oluserinfo.Password;
			if (password.Length > 16)
			{
				oluserkey = password.Substring(4, 8).Trim();
			}
			else
			{
				oluserkey = "";
			}
		}
	}
}