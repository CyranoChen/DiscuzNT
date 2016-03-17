using System;
using System.Web;
using System.Data;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Common.Generic;

namespace Discuz.Web
{
    /// <summary>
    /// 帖子管理页面
    /// </summary>
    public class misc : PageBase
    {
        #region 页面变量
        protected int tid = DNTRequest.GetInt("tid", 0);
        protected string emailcontent = "";
        protected string topictitle = "";
        protected string title = "";
        protected string action = DNTRequest.GetString("action");
        protected string voters = "";
        protected string polloptionid = DNTRequest.GetString("polloptionid");
        protected DataTable pollOptionList;
        #endregion
        protected override void ShowPage()
        {
            if (!ispost)
            {
                if (tid <= 0)
                {
                    AddErrLine("不存在的主题ID"); return;
                }
                TopicInfo topic = Topics.GetTopicInfo(tid);
                if (topic == null)
                {
                    AddErrLine("不存在的主题"); return;
                }
                switch (action)
                {
                    case "emailfriend":
                        title = "分享";
                        emailcontent = "你好！我在 {0} 看到了这篇帖子，认为很有价值，特推荐给你。\r\n{1}\r\n地址 {2}\r\n希望你能喜欢。";
                        if (topic != null)
                        {
                            topictitle = topic.Title;
                            emailcontent = string.Format(emailcontent, config.Forumtitle, topictitle, DNTRequest.GetUrlReferrer());
                        }
                        break;
                    case "viewvote":
                        title = "参与投票的会员";
                        PollInfo pollinfo = Polls.GetPollInfo(tid);
                        if (pollinfo == null)
                        {
                            AddErrLine("不存在的调查"); return;
                        }
                        if (!(pollinfo.Allowview == 1 || pollinfo.Uid == userid || Moderators.IsModer(useradminid, userid, topic.Fid)))
                        {
                            AddErrLine("您没有查看投票人的权限"); return;
                        }
                        pollOptionList = Polls.GetPollOptionList(tid);
                        bool allowvote;
                        if (polloptionid == "0" || polloptionid == "")
                        {
                            voters = Polls.GetVoters(tid, userid, username, out allowvote);
                        }
                        else
                        {
                            foreach (DataRow dr in pollOptionList.Rows)
                            {
                                if (dr["polloptionid"].ToString() == polloptionid.ToString())
                                {
                                    string[] votername = Utils.SplitString(dr["votername"].ToString().Trim(), " <");
                                    foreach (string user in votername)
                                    {
                                        voters += "<li>" + (user.StartsWith("<") ? user : "<" + user) + "</li>";
                                    }
                                }
                            }
                            if(voters == "")
                                voters = "<li>暂无人投票</li>";
                        }
                        break;
                }
            }
            else
            {
                SendEmail();
            }
        }

        private void SendEmail()
        {
            string sendtoemail = DNTRequest.GetString("sendtoemail");
            if (sendtoemail == "")
            {
                AddErrLine("接收者的Email不能为空");
                return;
            }
            if (!Utils.IsValidEmail(sendtoemail))
            {
                AddErrLine("接收者的Email不正确");
                return;
            }
            string message = string.Format("这封信是由 {0} 的 {1} 发送的。\r\n\r\n您收到这封邮件，是因为在 {1} 通过 {0} 的“推荐给朋友”\r\n"
                + "功能推荐了如下的内容给您，如果您对此不感兴趣，请忽略这封邮件。您不\r\n需要退订或进行其他进一步的操作。\r\n\r\n"
                + "----------------------------------------------------------------------\r\n"
                + "信件原文开始\r\n----------------------------------------------------------------------\r\n\r\n{2}"
                + "\r\n\r\n----------------------------------------------------------------------\r\n"
                + "信件原文结束\r\n----------------------------------------------------------------------\r\n\r\n"
                + "请注意这封信仅仅是由用户使用 “推荐给朋友”发送的，不是论坛官方邮件，\r\n论坛管理团队不会对这类邮件负责。\r\n\r\n"
                + "欢迎您访问 {0}\r\n{3}", config.Forumtitle, username, DNTRequest.GetString("message"), Utils.GetRootUrl(forumpath));
            Emails.DiscuzSmtpMailToUser(sendtoemail, string.Format("[{0}] {1} 推荐给您: {2} ", config.Forumtitle, username, Topics.GetTopicInfo(tid).Title), message);
        }
    }

}
