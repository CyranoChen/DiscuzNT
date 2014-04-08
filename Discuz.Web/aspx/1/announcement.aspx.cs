using System;
using System.Data;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;

namespace Discuz.Web
{
	/// <summary>
	/// 公告列表
	/// </summary>
	public class announcement : PageBase
    {
        #region 变量声明
        /// <summary>
        /// 公告列表
        /// </summary>
        public DataTable announcementlist = Announcements.GetAnnouncementList(Utils.GetDateTime(), "2099-12-31 23:59:59");
        #endregion 变量声明

        protected override void ShowPage()
		{
			pagetitle = "公告";

			if (announcementlist.Rows.Count == 0)
			{
				AddErrLine("当前没有任何公告");
				return;
			}
		}
	}
}
