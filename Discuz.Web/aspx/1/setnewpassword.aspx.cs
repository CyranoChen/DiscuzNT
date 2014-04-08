using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Web.UI;

namespace Discuz.Web
{
    /// <summary>
    /// 设置新密码
    /// </summary>
    public class setnewpassword : PageBase
    {
        /// <summary>
        /// 用户id
        /// </summary>
        private int uid = DNTRequest.GetInt("uid", 0);
        /// <summary>
        /// 认证字符串
        /// </summary>
        private string Authstr = DNTRequest.GetQueryString("id") != "" ? DNTRequest.GetQueryString("id") : DNTRequest.GetString("authstr");

        protected override void ShowPage()
        {
            pagetitle = "密码找回";
            username = DNTRequest.GetString("username");

            base.SetBackLink("/");

            //更新激活字段
            UserInfo __userinfo = Users.GetUserInfo(uid);
            if (__userinfo == null)
            {
                AddErrLine("用户名不存在,你无法重设密码");
                return;
            }

            if ((!__userinfo.Authstr.Equals(Authstr)) || Convert.ToDateTime(__userinfo.Authtime) <  DateTime.Now.AddDays(-3))
            {
                ReSendMail(__userinfo.Uid, __userinfo.Username, __userinfo.Email.Trim());
                SetUrl("/");
                SetMetaRefresh(5);
                SetShowBackLink(false);
                AddErrLine("验证码已失效,新的验证码已经通过 Email 发送到您的信箱中,<BR />请在 3 天之内到论坛修改您的密码.");
                return;
            }

            //如果提交了用户注册信息...
            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }
                base.SetBackLink("setnewpassword.aspx?uid=" + uid + "&id=" + Authstr);

                if (DNTRequest.GetString("newpassword").Equals(""))
                    AddErrLine("新密码不能为空");

                if (!DNTRequest.GetString("newpassword").Equals(DNTRequest.GetString("confirmpassword")))
                    AddErrLine("两次密码输入不一致");

                if (Utils.StrIsNullOrEmpty(Authstr) || !__userinfo.Authstr.Equals(Authstr))
                    AddErrLine("您所提供的验证码与注册信息不符.");

                if (IsErr()) return;
                   
                if (Utils.IsSafeSqlString(DNTRequest.GetString("newpassword")) && Users.UpdateUserPassword(uid, DNTRequest.GetString("newpassword"), true))
                {
                    Users.UpdateAuthStr(uid, "", 0); //将验证码清空,并设置验证标志为无效

                    SetUrl("login.aspx");
                    SetMetaRefresh(5);
                    SetShowBackLink(false);
                    MsgForward("setnewpassword_succeed");
                    AddMsgLine("你的密码已被重新设置,请用新密码登录.");
                }
                else
                    AddErrLine("用户名,Email 地址或安全提问不匹配,请返回修改.");
            }
        }


        private void ReSendMail(int uid, string username, string email)
        {
            string forumPath = this.Context.Request.Url.ToString().ToLower().Substring(0, this.Context.Request.Url.ToString().ToLower().IndexOf("/aspx/"));
            string Authstr = ForumUtils.CreateAuthStr(20);
            string title = config.Forumtitle + " 取回密码说明";

            Users.UpdateAuthStr(uid, Authstr, 2);

            StringBuilder body = new StringBuilder();
            body.Append(username);
            body.Append("您好!<BR />这封信是由 ");
            body.Append(config.Forumtitle);
            body.Append(" 发送的.<BR /><BR />您收到这封邮件,是因为在我们的论坛上这个邮箱地址被登记为用户邮箱,且该用户请求使用 Email 密码重置功能所致.");
            body.Append("<BR /><BR />----------------------------------------------------------------------");
            body.Append("<BR />重要！");
            body.Append("<BR /><BR />----------------------------------------------------------------------");
            body.Append("<BR /><BR />如果您没有提交密码重置的请求或不是我们论坛的注册用户,请立即忽略并删除这封邮件.只在您确认需要重置密码的情况下,才继续阅读下面的内容.");
            body.Append("<BR /><BR />----------------------------------------------------------------------");
            body.Append("<BR />密码重置说明");
            body.Append("<BR /><BR />----------------------------------------------------------------------");
            body.Append("<BR /><BR />您只需在提交请求后的三天之内,通过点击下面的链接重置您的密码:");
            body.Append("<BR /><BR /><a href=" + forumPath + "/setnewpassword.aspx?uid=" + uid + "&id=" + Authstr + " target=_blank>");
            body.Append(forumPath);
            body.Append("/setnewpassword.aspx?uid=");
            body.Append(uid);
            body.Append("&id=");
            body.Append(Authstr);
            body.Append("</a>");
            body.Append("<BR /><BR />(如果上面不是链接形式,请将地址手工粘贴到浏览器地址栏再访问)");
            body.Append("<BR /><BR />上面的页面打开后,输入新的密码后提交,之后您即可使用新的密码登录论坛了.您可以在用户控制面板中随时修改您的密码.");
            body.Append("<BR /><BR />本请求提交者的 IP 为 ");
            body.Append(DNTRequest.GetIP());
            body.Append("<BR /><BR /><BR /><BR />");
            body.Append("<BR />此致 <BR /><BR />");
            body.Append(config.Forumtitle);
            body.Append(" 管理团队.");
            body.Append("<BR />");
            body.Append(forumPath);
            body.Append("<BR /><BR />");

            Emails.DiscuzSmtpMailToUser(email, title, body.ToString());
        }
    }
}