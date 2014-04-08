using Discuz.Common;
using Discuz.Space.Pages;
using Discuz.Data;
using Discuz.Config;
using System.Web;
using System;

namespace Discuz.Space
{
	/// <summary>
	/// 显示文件列表页面
	/// </summary>
	public class viewpostlist : SpaceBasePage
	{
		public int postid = 0;

        override protected void OnInit(EventArgs e)
		{
            if (GeneralConfigs.GetConfig().Enablespace != 1)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("<script>alert('" + config.Spacename + "功能已被关闭！');document.location = '" + forumurlnopage + "/index.aspx';</script>");
                HttpContext.Current.Response.End();
                return;
            }
            if (DNTRequest.GetString("act") == "del")
            {
                Discuz.Space.Spaces.DeleteSpacePost(DNTRequest.GetString("postid"), spaceconfiginfo.UserID);
                HttpContext.Current.Response.Write("<script>alert('日志已经被删除！');document.location = 'viewspacepostlist.aspx?spaceid=" + DNTRequest.GetString("spaceid") + "';</script>");
                return;
            }
			postid = DNTRequest.GetInt("postid",0);
			if (postid == 0)
			{
				return ;
			}
			else
			{
                //对用户空间日志浏览量加1
				Space.Data.DbProvider.GetInstance().CountUserSpacePostByUserID(postid);

				//对用户空间访问量加1
				Space.Data.DbProvider.GetInstance().CountUserSpaceVisitedTimesByUserID(spaceconfiginfo.UserID);
			}
		}
	}
}
