using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;

namespace Discuz.Web
{
    /// <summary>
    /// 找回密码页
    /// </summary>
    public class getpassword : PageBase
    {
        protected override void ShowPage()
        {
            pagetitle = "密码找回";
            username = Utils.RemoveHtml(DNTRequest.GetString("username"));

            //如果提交...
            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }

                base.SetBackLink("getpassword.aspx?username=" + username);

                if (Users.GetUserId(username) == 0)
                {
                    AddErrLine("用户不存在");
                    return;
                }
                if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("email")))
                {
                    AddErrLine("电子邮件不能为空");
                    return;
                }

                if (IsErr()) return;
  
                if (Users.CheckEmailAndSecques(username, DNTRequest.GetString("email"), DNTRequest.GetInt("question", 0), DNTRequest.GetString("answer"), GetForumPath()))
                {
                    SetUrl(forumpath);
                    SetMetaRefresh(5);
                    SetShowBackLink(false);
                    MsgForward("getpassword_succeed");
                    AddMsgLine("取回密码的方法已经通过 Email 发送到您的信箱中,<br />请在 3 天之内到论坛修改您的密码.");
                }
                else
                    AddErrLine("用户名,Email 地址或安全提问不匹配,请返回修改.");
            }
        }

        private string GetForumPath()
        {
            return this.Context.Request.Url.ToString().ToLower().Substring(0, this.Context.Request.Url.ToString().ToLower().IndexOf("/aspx/"));
        }
    }
}