using Discuz.Common;
using Discuz.Space.Entities;
using Discuz.Space.Pages;
using Discuz.Entity;
using Discuz.Space.Provider;
using Discuz.Data;
using Discuz.Config;
using System.Web;
using System;

namespace Discuz.Space
{
	/// <summary>
    /// 显示日志内容页面
	/// </summary>
	public class viewspacepost : SpaceBasePage
	{
		public int postid = 0;

        //当前日志的评论数
		public int commentcount = 0;

		protected SpacePostInfo spacepostinfo = new SpacePostInfo();

        protected string configspaceurl = null;

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
			postid = DNTRequest.GetInt("postid", 0);
			if (postid == 0)
			{
				return;
			}
			else
			{
				spacepostinfo = BlogProvider.GetSpacepostsInfo(Space.Data.DbProvider.GetInstance().GetSpacePost(postid));
                commentcount = spacepostinfo != null ? spacepostinfo.Commentcount : 0;
                if (spacepostinfo == null)
                {
                    spacepostinfo = new SpacePostInfo();                    
                }
                else
                {
                    //对用户空间浏览量加1
                    Space.Data.DbProvider.GetInstance().CountUserSpacePostByUserID(postid);

                    //对用户空间访问量加1
                    Space.Data.DbProvider.GetInstance().CountUserSpaceVisitedTimesByUserID(spaceconfiginfo.UserID);
                }
			
			}

            configspaceurl = GeneralConfigs.GetConfig().Spaceurl;
            if (configspaceurl.ToLower().IndexOf("http://") < 0)
            {
                configspaceurl = forumurlnopage + configspaceurl;
            }
         
		}
	
	}
}