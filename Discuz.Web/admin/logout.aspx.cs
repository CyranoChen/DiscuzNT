using System;
using System.Web;
using System.Web.UI;
using System.Web.Security;

using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// logout 的摘要说明. 
    /// </summary>
    public class logout : Page
    {
        protected internal GeneralConfigInfo config;

        protected void Page_Load(object sender, EventArgs e)
        {
            //更新在线表相关用户信息
            config = GeneralConfigs.GetConfig();
            OnlineUserInfo oluserinfo = OnlineUsers.UpdateInfo(config.Passwordkey, config.Onlinetimeout);
            if(AdminUserGroups.AdminGetUserGroupInfo(oluserinfo.Groupid).Radminid != 1)
            {
                HttpContext.Current.Response.Redirect("../");
                return;
            }
            int olid = oluserinfo.Olid;
            OnlineUsers.DeleteRows(olid);

            //清除Cookie
            ForumUtils.ClearUserCookie();
            HttpCookie cookie = new HttpCookie("dntadmin");
            HttpContext.Current.Response.AppendCookie(cookie);

            FormsAuthentication.SignOut();
        }
    }
}