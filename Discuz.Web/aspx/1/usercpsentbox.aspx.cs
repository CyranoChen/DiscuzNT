using System;
using System.Data;
using System.Data.SqlClient;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;

namespace Discuz.Web
{
	/// <summary>
	/// 发件箱页面
	/// </summary>
	public class usercpsentbox : UserCpPage
    {
        protected override void ShowPage()
		{
			pagetitle = "短消息发件箱";

            if (!IsLogin()) return;

            if (DNTRequest.IsPost())
			{
				if (ForumUtils.IsCrossSitePost())
				{
					AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
					return;
				}

				if (PrivateMessages.DeletePrivateMessage(userid, Utils.SplitString(DNTRequest.GetFormString("pmitemid"), ",")) == -1)
				{
					AddErrLine("参数无效<br />");
					return;
				}

				SetShowBackLink(false);
                AddMsgLine("删除完毕");
			}
			else
                BindPrivateMessage(1);

            newnoticecount = Notices.GetNewNoticeCountByUid(userid);
		}
	}
}

