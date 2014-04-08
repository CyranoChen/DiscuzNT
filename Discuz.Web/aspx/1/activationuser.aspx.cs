using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Web.UI;
using Discuz.Entity;

namespace Discuz.Web
{
	/// <summary>
	/// 激活用户页面
	/// </summary>
	public class activationuser : PageBase
	{
		protected override void ShowPage()
		{
			pagetitle = "用户帐号激活";

			SetUrl("index.aspx");
			SetMetaRefresh();
			SetShowBackLink(false);

			string authStr = Utils.HtmlEncode(DNTRequest.GetString("authstr").Trim()).Replace("'","''");

			if(!Utils.StrIsNullOrEmpty(authStr))
			{
                if (Users.UpdateAuthStr(authStr))
				{
					                 
					AddMsgLine("您当前的帐号已经激活,稍后您将以相应身份返回首页");

					OnlineUsers.UpdateAction(olid, UserAction.ActivationUser.ActionID, 0, config.Onlinetimeout);
                    return;	
				}
            }
			AddMsgLine("您当前的激活链接无效,稍后您将以游客身份返回首页");
			OnlineUsers.DeleteRows(olid);
			ForumUtils.ClearUserCookie();
		}
	}
}
