using System;
using System.Data;
using System.Web;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Web.UI;

namespace Discuz.Web
{
    /// <summary>
    /// 分栏框架页
    /// </summary>
    public class frame : PageBase
    {
        protected override void ShowPage()
        {
            pagetitle = "分栏";
            int toframe = DNTRequest.GetInt("f", 1);
            if (toframe == 1)
                ForumUtils.WriteCookie("isframe", "1");
            else
            {
                toframe = Utils.StrToInt(ForumUtils.GetCookie("isframe"), -1);
                toframe = toframe == -1 ? config.Isframeshow : toframe;
            }

            if (toframe == 0)
            {
                HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath);
                HttpContext.Current.Response.End();
            }
        }
    }
}