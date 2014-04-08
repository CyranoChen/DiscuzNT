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
    /// logout ��ժҪ˵��. 
    /// </summary>
    public class logout : Page
    {
        protected internal GeneralConfigInfo config;

        protected void Page_Load(object sender, EventArgs e)
        {
            //�������߱�����û���Ϣ
            config = GeneralConfigs.GetConfig();
            OnlineUserInfo oluserinfo = OnlineUsers.UpdateInfo(config.Passwordkey, config.Onlinetimeout);
            if(AdminUserGroups.AdminGetUserGroupInfo(oluserinfo.Groupid).Radminid != 1)
            {
                HttpContext.Current.Response.Redirect("../");
                return;
            }
            int olid = oluserinfo.Olid;
            OnlineUsers.DeleteRows(olid);

            //���Cookie
            ForumUtils.ClearUserCookie();
            HttpCookie cookie = new HttpCookie("dntadmin");
            HttpContext.Current.Response.AppendCookie(cookie);

            FormsAuthentication.SignOut();
        }
    }
}