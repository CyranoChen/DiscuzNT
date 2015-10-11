using System;
using System.Data;
using System.Text;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Config;

namespace Discuz.Web
{
    /// <summary>
    /// 退出页面
    /// </summary>
    public class logout : PageBase
    {
        private string reurl = DNTRequest.GetQueryString("reurl");

        protected override void ShowPage()
        {
            int logoutUid = userid;
            pagetitle = "用户退出";
            username = "游客";
            userid = -1;

            base.AddScript("if (top.document.getElementById('leftmenu')){ top.frames['leftmenu'].location.reload(); }");

            if (!DNTRequest.IsPost() || reurl != "")
            {
                string r = (!Utils.StrIsNullOrEmpty(reurl)) ? reurl : "";

                if (reurl == "")
                    r = (DNTRequest.GetUrlReferrer() == "" || DNTRequest.GetUrlReferrer().IndexOf("login") > -1 || DNTRequest.GetUrlReferrer().IndexOf("logout") > -1) ?
                            "index.aspx" : DNTRequest.GetUrlReferrer();

                Utils.WriteCookie("reurl", (reurl == "" || reurl.IndexOf("login.aspx") > -1) ? r : reurl);
            }

            if (DNTRequest.GetString("userkey") == userkey || IsApplicationLogout())
            {
               
                AddMsgLine("已经清除了您的登录信息, 稍后您将以游客身份返回首页");

                OnlineUsers.DeleteRows(olid);
                ForumUtils.ClearUserCookie();
                Utils.WriteCookie(Utils.GetTemplateCookieName(), "", -999999);

                System.Web.HttpContext.Current.Response.AppendCookie(new System.Web.HttpCookie("dntadmin"));

                //同步登录到第三方应用
                if (APIConfigs.GetConfig().Enable)
                    AddMsgLine(Sync.GetLogoutScript(logoutUid));

                if (!APIConfigs.GetConfig().Enable || !Sync.NeedAsyncLogout()) 
                    MsgForward("logout_succeed");
            }
            else
                AddMsgLine("无法确定您的身份, 稍后返回首页");

            SetUrl(Utils.UrlDecode(ForumUtils.GetReUrl()));
            SetMetaRefresh();
            SetShowBackLink(false);
        }

        /// <summary>
        /// 是否是来自应用程序的登出
        /// </summary>
        /// <returns></returns>
        private bool IsApplicationLogout()
        {
            if(!APIConfigs.GetConfig().Enable)
                return false;

            if (DNTRequest.GetFormInt("confirm", -1) != 1)
                return false;        

            return true;
        }
    }
}