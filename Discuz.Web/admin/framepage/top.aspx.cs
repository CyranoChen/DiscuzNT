using System;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Threading;
using System.Xml;

using Discuz.Forum;
using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common.Xml;

namespace Discuz.Web.Admin
{
    public class top : AdminPage
    {
        public StringBuilder sb = new StringBuilder();

        public int menucount = 0;

        public int olid;
        public string showmenuid;
        public string toptabmenuid;
        public string mainmenulist;
        public string defaulturl;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                config = GeneralConfigs.GetConfig();
                // ���IP�����б�������������ж�
                if (config.Adminipaccess.Trim() != "")
                {
                    string[] regctrl = Utils.SplitString(config.Adminipaccess, "\n");
                    if (!Utils.InIPArray(DNTRequest.GetIP(), regctrl))
                    {
                        Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                        return;
                    }
                }

                //��ȡ��ǰ�û������߷�?
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


                #region ����Ȩ���ж�

                UserGroupInfo usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(oluserinfo.Groupid);
                if (oluserinfo.Userid <= 0 || usergroupinfo.Radminid != 1)
                {
                    Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                    return;
                }

                string secques = Users.GetUserInfo(oluserinfo.Userid).Secques;
                // ����Ա�����?
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
