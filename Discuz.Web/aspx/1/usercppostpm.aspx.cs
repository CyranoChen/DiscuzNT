using System;
using System.Data;
using System.Data.SqlClient;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;
using System.Text;


namespace Discuz.Web
{
    /// <summary>
    /// 撰写短消息页面
    /// </summary>
    public class usercppostpm : UserCpPage
    {
        #region 页面变量
        /// <summary>
        /// 短消息收件人
        /// </summary>
        public string msgto = Utils.HtmlEncode(DNTRequest.GetString("msgto"));
        /// <summary>
        /// 短消息标题
        /// </summary>
        public string subject = Utils.HtmlEncode(DNTRequest.GetString("subject"));
        /// <summary>
        /// 短消息内容
        /// </summary>
        public string message = Utils.HtmlEncode(DNTRequest.GetString("message"));
        /// <summary>
        /// 短消息收件人Id
        /// </summary>
        public int msgtoid = DNTRequest.GetInt("msgtoid", 0);

        /// <summary>
        /// 短消息
        /// </summary>
        PrivateMessageInfo pm = new PrivateMessageInfo();
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "撰写短消息";

            if (!IsLogin()) return;

            if (!CheckPermission())
                return;

            if (DNTRequest.IsPost() && !ForumUtils.IsCrossSitePost())
            {
                if (!CheckPermissionAfterPost())
                    return;

                SendPM();
                if (IsErr()) return;
            }

            ShortUserInfo shortUserInfo = Users.GetShortUserInfo(msgtoid);
            string msttoName = (shortUserInfo != null) ? shortUserInfo.Username : "";

            msgto = msgtoid > 0 ? msttoName : msgto;

            string action = DNTRequest.GetQueryString("action").ToLower();
            if (action.CompareTo("re") == 0 || action.CompareTo("fw") == 0) //回复或者转发
            {
                if (DNTRequest.GetQueryInt("pmid", -1) != -1)
                {
                    PrivateMessageInfo pm = PrivateMessages.GetPrivateMessageInfo(DNTRequest.GetQueryInt("pmid", -1));
                    if (pm != null && (pm.Msgtoid == userid || pm.Msgfromid == userid))
                    {
                        msgto = action.CompareTo("re") == 0 ? Utils.HtmlEncode(pm.Msgfrom) : "";
                        subject = Utils.HtmlEncode(action) + ":" + pm.Subject;
                        message = Utils.HtmlEncode("> ") + pm.Message.Replace("\n", "\n> ") + "\r\n\r\n";
                    }
                }
            }

            if (DNTRequest.GetString("operation") == "pmfriend")
                CreatePmFriendMessage();

            newnoticecount = Notices.GetNewNoticeCountByUid(userid);
        }

        /// <summary>
        /// 提交后的权限检查
        /// </summary>
        /// <returns></returns>
        private bool CheckPermissionAfterPost()
        {
            if (ForumUtils.IsCrossSitePost())
            {
                AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                return false;
            }
            if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("message")) || DNTRequest.GetString("message").Length > 3000)
            {
                AddErrLine("内容不能为空,且不能超过3000字");
                return false;
            }
            if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("msgto")))
            {
                AddErrLine("接收人不能为空");
                return false;
            }
            if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("subject")) || DNTRequest.GetString("subject").Trim().Length > 60)
            {
                AddErrLine("标题不能为空,且不能超过60字");
                return false;
            }
            // 不能给负责发送新用户注册欢迎信件的用户名称发送消息
            if (DNTRequest.GetString("msgto") == PrivateMessages.SystemUserName)
            {
                AddErrLine("不能给系统发送消息");
                return false;
            }
            msgtoid = Users.GetUserId(DNTRequest.GetString("msgto"));
            if (msgtoid <= 0)
            {
                AddErrLine("接收人不是注册用户");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 不论是否提交都有的权限检查
        /// </summary>
        /// <returns></returns>
        private bool CheckPermission()
        {
            // 如果是受灌水限制用户, 则判断是否是灌水
            AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
            if (admininfo == null || admininfo.Disablepostctrl != 1)
            {
                int Interval = Utils.StrDateDiffSeconds(lastpostpmtime, config.Postinterval * 2);
                if (Interval < 0)
                {
                    AddErrLine(string.Format("系统规定发帖或发短消息间隔为{0}秒, 您还需要等待 {1} 秒", (config.Postinterval * 2).ToString(), (Interval * -1).ToString()));
                    return false;
                }
            }

            if (!UserCredits.CheckUserCreditsIsEnough(userid, 1, CreditsOperationType.SendMessage, -1))
            {
                AddErrLine("您的积分不足, 不能发送短消息");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 发送邮件通知
        /// </summary>
        /// <param name="email">接收人邮箱</param>
        /// <param name="privatemessageinfo">短消息对象</param>
        public void SendNotifyEmail(string email, PrivateMessageInfo pm)
        {
            string jumpurl = string.Format("http://{0}/usercpshowpm.aspx?pmid={1}", DNTRequest.GetCurrentFullHost(), pm.Pmid);
            StringBuilder sb_body = new StringBuilder("# 论坛短消息: <a href=\"" + jumpurl + "\" target=\"_blank\">" + pm.Subject + "</a>");
            //发送人邮箱
            sb_body.AppendFormat("\r\n\r\n{0}\r\n<hr/>", pm.Message);
            sb_body.AppendFormat("作 者:{0}\r\n", pm.Msgfrom);
            sb_body.AppendFormat("Email:<a href=\"mailto:{0}\" target=\"_blank\">{0}</a>\r\n", Users.GetShortUserInfo(userid).Email.Trim());
            sb_body.AppendFormat("URL:<a href=\"{0}\" target=\"_blank\">{0}</a>\r\n", jumpurl);
            sb_body.AppendFormat("时 间:{0}", pm.Postdatetime);
            Emails.SendEmailNotify(email, "[" + config.Forumtitle + "短消息通知]" + pm.Subject, sb_body.ToString());
        }

        /// <summary>
        /// 创建并发送短消息
        /// </summary>
        public void SendPM()
        {
            #region 创建并发送短消息

            // 收件箱
            if (useradminid == 1)
            {
                pm.Message = Utils.HtmlEncode(DNTRequest.GetString("message"));
                pm.Subject = Utils.HtmlEncode(DNTRequest.GetString("subject"));
            }
            else
            {
                pm.Message = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("message")));
                pm.Subject = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("subject")));
            }

            if (useradminid != 1 && (ForumUtils.HasBannedWord(pm.Message) || ForumUtils.HasBannedWord(pm.Subject) || ForumUtils.HasAuditWord(pm.Message) || ForumUtils.HasAuditWord(pm.Subject)))
            {
                string bannedWord = ForumUtils.GetBannedWord(pm.Message) == string.Empty ? ForumUtils.GetBannedWord(pm.Subject) : ForumUtils.GetBannedWord(pm.Message);
                AddErrLine(string.Format("对不起, 您提交的内容包含不良信息 <font color=\"red\">{0}</font>, 因此无法提交, 请返回修改!", bannedWord));
                return;
            }
            string Ignorepm = "," + Users.GetUserInfo(msgtoid).Ignorepm + ",";
            //禁止所有用户或当前用户在忽略列表内时
            if (Ignorepm.IndexOf("{ALL}") >= 0 || Ignorepm.IndexOf("," + username + ",") >= 0)
            {
                AddErrLine("短消息发送失败!");
                return;
            }

            pm.Message = ForumUtils.BanWordFilter(pm.Message);
            pm.Subject = ForumUtils.BanWordFilter(pm.Subject);
            pm.Msgto = DNTRequest.GetString("msgto");
            pm.Msgtoid = msgtoid;
            pm.Msgfrom = username;
            pm.Msgfromid = userid;
            pm.New = 1;
            pm.Postdatetime = Utils.GetDateTime();

            // 只将消息保存到草稿箱
            if (!Utils.StrIsNullOrEmpty(DNTRequest.GetString("savetousercpdraftbox")))
            {
                CreatePM(2, 0, "usercpdraftbox.aspx", "已将消息保存到草稿箱");
                return;
            }
            else if (!Utils.StrIsNullOrEmpty(DNTRequest.GetString("savetosentbox")))// 发送消息且保存到发件箱
                CreatePM(0, 1, "usercpsentbox.aspx", "发送完毕, 且已将消息保存到发件箱");
            else// 发送消息但不保存到发件箱
                CreatePM(0, 0, "usercpinbox.aspx", "发送完毕");

            if (!IsErr())
            {
                // 更新在线表中的用户最后发帖时间
                OnlineUsers.UpdatePostPMTime(olid);

                //为在线用户更新短消息数
                int targetolid = OnlineUsers.GetOlidByUid(pm.Msgtoid);
                if (targetolid > 0)
                    Users.UpdateUserNewPMCount(pm.Msgtoid, targetolid);
            }
            #endregion
        }


        /// <summary>
        /// 创建短消息
        /// </summary>
        /// <param name="folder">所属文件夹(0:收件箱,1:发件箱,2:草稿箱)</param>
        /// <param name="saveToSendBox">是否保存到发件箱</param>
        /// <param name="url">跳转链接</param>
        /// <param name="msg">提示信息</param>
        private void CreatePM(int folder, int saveToSendBox, string url, string msg)
        {
            if (folder != 2)
            {
                UserInfo touser = Users.GetUserInfo(msgtoid);
                // 检查接收人的短消息是否已超过接收人用户组的上限,管理组不受接收人短消息上限限制
                int radminId = UserGroups.GetUserGroupInfo(usergroupid).Radminid;
                if (!(radminId > 0 && radminId <= 3) && PrivateMessages.GetPrivateMessageCount(msgtoid, -1) >= UserGroups.GetUserGroupInfo(touser.Groupid).Maxpmnum)
                {
                    AddErrLine("抱歉,接收人的短消息已达到上限,无法接收");
                    return;
                }
                if (!Utils.InArray(Convert.ToInt32(touser.Newsletter).ToString(), "2,3,6,7"))
                {
                    AddErrLine("抱歉,接收人拒绝接收短消息");
                    return;
                }
            }

            // 检查发送人的短消息是否已超过发送人用户组的上限
            if (url != "usercpinbox.aspx" && PrivateMessages.GetPrivateMessageCount(userid, -1) >= usergroupinfo.Maxpmnum)
            {
                AddErrLine("抱歉,您的短消息已达到上限,无法保存到发件箱");
                return;
            }
            pm.Folder = folder;
            if (UserCredits.UpdateUserCreditsBySendpms(base.userid) == -1)
            {
                AddErrLine("您的积分不足, 不能发送短消息");
                return;
            }
            pm.Pmid = PrivateMessages.CreatePrivateMessage(pm, saveToSendBox);
            //发送邮件通知
            if (DNTRequest.GetString("emailnotify") == "on")
                SendNotifyEmail(Users.GetUserInfo(msgtoid).Email.Trim(), pm);

            SetUrl(url);
            SetMetaRefresh();
            SetShowBackLink(true);
            MsgForward("usercppostpm_succeed");
            AddMsgLine(msg);
        }

        private void CreatePmFriendMessage()
        {
            int tid = DNTRequest.GetInt("tid", 0);
            if (tid == 0)
                return;
            message = string.Format("你好！我在 {0} 看到了这篇帖子，认为很有价值，特推荐给你。\r\n\r\n{1}\r\n地址 {2}\r\n\r\n希望你能喜欢。",
                config.Forumtitle, Topics.GetTopicInfo(tid).Title, DNTRequest.GetUrlReferrer());
        }
    } //class end
}