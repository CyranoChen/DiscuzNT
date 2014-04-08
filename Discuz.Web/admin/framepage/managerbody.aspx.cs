using System;
using System.Web;
using System.Threading;
using System.Web.UI;
using System.Xml;

using Discuz.Forum;
using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    public partial class managerbody : AdminPage
    {
        public int olid;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                config = GeneralConfigs.GetConfig();

                // 如果IP访问列表有设置则进行判断
                if (config.Adminipaccess.Trim() != "")
                {
                    string[] regctrl = Utils.SplitString(config.Adminipaccess, "\n");
                    if (!Utils.InIPArray(DNTRequest.GetIP(), regctrl))
                    {
                        Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                        return;
                    }
                }

                //获取当前用户的在线信息
                OnlineUserInfo oluserinfo = new OnlineUserInfo();
                try
                {
                    oluserinfo = OnlineUsers.UpdateInfo(config.Passwordkey, config.Onlinetimeout);
                }
                catch
                {
                    Thread.Sleep(2000);
                    oluserinfo = OnlineUsers.UpdateInfo(config.Passwordkey, config.Onlinetimeout);
                }


                #region 进行权限判断

                UserGroupInfo usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(oluserinfo.Groupid);
                if (oluserinfo.Userid <= 0 || usergroupinfo.Radminid != 1)
                {
                    Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                    return;
                }

                string secques = Users.GetUserInfo(oluserinfo.Userid).Secques;
                // 管理员身份验证
                if (Context.Request.Cookies["dntadmin"] == null || Context.Request.Cookies["dntadmin"]["key"] == null || ForumUtils.GetCookiePassword(Context.Request.Cookies["dntadmin"]["key"].ToString(), config.Passwordkey) != (oluserinfo.Password + secques + oluserinfo.Userid.ToString()))
                {
                    Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                    return;
                }
                else
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["dntadmin"];
                    cookie.Values["key"] = ForumUtils.SetCookiePassword(oluserinfo.Password + secques + oluserinfo.Userid.ToString(), config.Passwordkey);
                    cookie.Expires = DateTime.Now.AddMinutes(30);
                    HttpContext.Current.Response.AppendCookie(cookie);
                }

                #endregion
                
            }
        }
    }
}
