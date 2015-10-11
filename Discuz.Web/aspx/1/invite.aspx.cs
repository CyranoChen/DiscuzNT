using System;
using System.Data;
using System.Text;
using System.Web;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common.Generic;

namespace Discuz.Web
{
    /// <summary>
    /// 邀请功能页面
    /// </summary>
    public class invite : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 邀请码URL
        /// </summary>
        public string inviteurl = "";

        /// <summary>
        /// 当前用户信息
        /// </summary>
        private ShortUserInfo userinfo = null;

        /// <summary>
        /// 当前用户邀请码(开放式)
        /// </summary>
        public InviteCodeInfo invitecodeinfo;

        /// <summary>
        /// url中的邀请码
        /// </summary>
        public string invitecode = DNTRequest.GetQueryString("invitecode");

        /// <summary>
        /// 页面执行操作类型
        /// </summary>
        public string action = DNTRequest.GetQueryString("action");

        /// <summary>
        /// 当前用户邀请分数(开放式邀请码可兑换到的分数)
        /// </summary>
        public string userscore = "";

        /// <summary>
        /// 封闭式邀请码价格
        /// </summary>
        public string invitecodeprice = "";

        /// <summary>
        /// 当前用户邀请码列表(封闭式)
        /// </summary>
        public List<InviteCodeInfo> invitecodelist = new List<InviteCodeInfo>();

        /// <summary>
        /// 当前用户邀请码数量(封闭式)
        /// </summary>
        public int invitecodecount = 0;

        /// <summary>
        /// 当前邀请码页码(封闭式)
        /// </summary>
        public int pageindex = DNTRequest.GetQueryInt("page", 1);

        /// <summary>
        /// 分页显示样式
        /// </summary>
        public string pagenumber = "";

        /// <summary>
        /// 页面数量
        /// </summary>
        public int pagecount = 0;

        /// <summary>
        /// Email内容预览
        /// </summary>
        public string emailpreview = "";

        /// <summary>
        /// 用户邀请附言
        /// </summary>
        public string usersaid = Utils.HtmlEncode(Utils.RemoveHtml(DNTRequest.GetString("usersaid")));

        /// <summary>
        /// 邀请功能全局设置
        /// </summary>
        public InvitationConfigInfo invitationconfiginfo = InvitationConfigs.GetConfig();

        /// <summary>
        /// 控制是否显示邀请附言
        /// </summary>
        public bool isuseusersaid = false;

        /// <summary>
        /// 扩展积分名称
        /// </summary>
        public string[] extcreditnames = Scoresets.GetValidScoreName();

        /// <summary>
        /// 可以使用的扩展积分单位
        /// </summary>
        public string[] extcreditunits = Scoresets.GetValidScoreUnit();

        /// <summary>
        /// 预览头像小
        /// </summary>
        public string avatarSmall = "";

        /// <summary>
        /// 预览头像中
        /// </summary>
        public string avatarMedium = "";

        /// <summary>
        /// 预览头像大
        /// </summary>
        public string avatarLarge = "";

        /// <summary>
        /// 邀请链接是否过期
        /// </summary>
        public int datediff = 0;

        #endregion

        protected override void ShowPage()
        {
            pagetitle = "邀请注册";
            if (!Utils.InArray(config.Regstatus.ToString(), "2,3"))
            {
                AddErrLine("当前站点没有开启邀请功能！");
                return;
            }
            if (userid > 0)
            {
                if (action == "floatwinemail")
                {
                    return;
                }
                //提取预览头像
                avatarSmall = Avatars.GetAvatarUrl(userid, AvatarSize.Small);
                avatarMedium = Avatars.GetAvatarUrl(userid, AvatarSize.Medium);
                avatarLarge = Avatars.GetAvatarUrl(userid, AvatarSize.Large);

                userinfo = Users.GetUserInfo(userid);
                if (config.Regstatus == 2)
                {
                    invitecodeinfo = Invitation.GetInviteCodeByUid(userid);
                    if (invitecodeinfo != null)
                    {
                        inviteurl = GetUserInviteUrl(invitecodeinfo.Code, false);
                        userscore = GetUserInviteScore(invitecodeinfo.SuccessCount);
                        usersaid = string.Format("邀请附言:<div id=\"usersaidinemail\">{0}</div>", usersaid);
                        if (!ispost)
                            CreateEmailPreview();
                    }
                }
                else
                {
                    invitecodecount = Invitation.GetUserInviteCodeCount(userid);
                    invitecodelist = Invitation.GetUserInviteCodeList(userid, pageindex);
                    invitecodeprice = GetInviteCodePrice();
                    pagecount = ((invitecodecount - 1) / 10) + 1;
                    pagenumber = Utils.GetPageNumbers(pageindex, pagecount, "invite.aspx", 8);
                }
                if (ispost)
                {
                    switch (action)
                    {
                        case "createcode":
                            CreateInviteCode();//创建开放式邀请码
                            break;
                        case "convertcode":
                            ConvertInviteCode();//将开放邀请码兑换为用户积分
                            break;
                        case "buycode":
                            BuyInviteCode();//购买封闭式邀请码
                            break;
                        case "floatwinemailsend":
                            SendEmail();//弹窗邮件发送邀请码
                            break;
                        default:
                            return;
                    }
                }
            }
            if (userid == -1 && invitecode != "")
            {
                invitecodeinfo = Invitation.GetInviteCodeByCode(invitecode);
            }

        }

        public void CreateInviteCode()
        {
            if (config.Regstatus == 2 && invitationconfiginfo.InviteCodeUserCreatePerDay > 0 && Invitation.GetTodayUserCreatedInviteCode(userid) >= invitationconfiginfo.InviteCodeUserCreatePerDay)
            {
                AddErrLine("您今天申请邀请码的数量过多，请明天再试!");
                return;
            }
            if (invitecodeinfo == null)//如果当前用户未创建开放式邀请码
            {
                Invitation.CreateInviteCode(userinfo);
                SetUrl("invite.aspx");
                SetMetaRefresh();
                SetShowBackLink(false);
                AddMsgLine("创建邀请码成功");
                return;
            }
        }

        public void ConvertInviteCode()
        {

            if (config.Regstatus == 2)
            {
                if (invitecodeinfo != null)//防止可能性的恶意提交
                {
                    Invitation.ConvertInviteCodeToCredits(invitecodeinfo, invitationconfiginfo.InviteCodePayCount);
                    Invitation.DeleteInviteCode(invitecodeinfo.InviteId);
                    string msg = invitecodeinfo.SuccessCount - invitationconfiginfo.InviteCodePayCount > -1 ? "兑换成功" : "删除成功";
                    SetUrl("invite.aspx");
                    SetMetaRefresh();
                    SetShowBackLink(false);
                    AddMsgLine(msg);
                    return;
                }
            }
        }

        public void SendEmail()
        {
            List<string> emailAddress = new List<string>(Utils.SplitString(DNTRequest.GetString("email"), ","));

            if (invitecode != "")//针对弹窗发送邀请码的数据获取
                invitecodeinfo = Invitation.GetInviteCodeByCode(invitecode);
            else
            {
                AddErrLine("丢失参数导致邮件发送失败，请检查本地杀毒软件设置");
                return;
            }

            int sendSuccessCount = 0;
            foreach (string address in emailAddress)
            {
                
                if (string.IsNullOrEmpty(address) || !Utils.IsValidEmail(address))
                    continue;

                if (Emails.SendEmailNotify(address, "来自您的好友:" + invitecodeinfo.Creator + "的邀请!", 
                    string.Format(invitationconfiginfo.InvitationEmailTemplate, address, userid, invitecodeinfo.Creator, 
                    GetUserInviteUrl(invitecodeinfo.Code, true), config.Forumtitle, usersaid != "" ? usersaid : "", rooturl, avatarSmall, avatarMedium, avatarLarge)))
                    sendSuccessCount++;

                if (sendSuccessCount > 19)//如果邮件发送次数已经达到20个，则不在继续发送
                    break;
            }

            if (sendSuccessCount > 0)
            {
                AddMsgLine("成功发送" + sendSuccessCount.ToString() + "封Email");
                return;
            }
            else
            {
                AddErrLine("发送失败，请检查Email地址是否正确");
                return;
            }
        }

        public void BuyInviteCode()
        {
            if (invitecodecount >= invitationconfiginfo.InviteCodeMaxCountToBuy)
            {
                AddErrLine("您所拥有的邀请码数量超过了系统上限，无法再购买");
                return;
            }

            string[] strExtCredits = Utils.SplitString(invitationconfiginfo.InviteCodePrice, ",");
            float[] extCredits = new float[8];
            for (int i = 0; i < 8; i++)
                extCredits[i] = Utils.StrToFloat(strExtCredits[i], 0) * -1;

            if (UserCredits.UpdateUserExtCredits(userid, extCredits, false) > 0)
            {
                CreateInviteCode();
            }
            else
            {
                string addExtCreditsTip = "";
                if (EPayments.IsOpenEPayments())
                    addExtCreditsTip = "<br/><span><a href=\"usercpcreditspay.aspx\">点击充值积分</a></span>";
                AddErrLine("积分不足，无法购买邀请码" + addExtCreditsTip);
                return;
            }
        }

        /// <summary>
        /// 返回邀请链接
        /// </summary>
        /// <param name="code">邀请码</param>
        /// <returns></returns>
        public string GetUserInviteUrl(string code, bool isCreateLink)
        {
            if (isCreateLink)
                return string.Format("<a href=\"{0}\" target=\"_blank\">{0}</a>", rooturl + "invite.aspx?invitecode=" + code);

            return rooturl + "invite.aspx?invitecode=" + code;
        }

        /// <summary>
        /// 获取用户开放式邀请码的兑换价值
        /// </summary>
        /// <param name="mount"></param>
        /// <returns></returns>
        public string GetUserInviteScore(int mount)
        {
            float realMount = (float)(mount - invitationconfiginfo.InviteCodePayCount);
            if (realMount > 0)
            {
                float[] extCredits = Scoresets.GetUserExtCredits(CreditsOperationType.Invite);
                string userInviteScore = "";
                for (int i = 0; i < extCredits.Length; i++)
                {
                    if (extCredits[i] != 0.00)
                    {
                        userInviteScore += string.Format("{0}:{1}{2} ;", extcreditnames[i + 1], extCredits[i] * mount, extcreditunits[i + 1]);
                    }
                }
                return userInviteScore;
            }
            else
            {
                return "该邀请码尚未达到兑换条件!";
            }
        }

        /// <summary>
        /// 获取封闭式邀请码的定价
        /// </summary>
        /// <returns></returns>
        public string GetInviteCodePrice()
        {
            string priceStr = "";
            string[] extCredits = Utils.SplitString(invitationconfiginfo.InviteCodePrice, ",");
            for (int i = 0; i < 8; i++)
            {
                if (Utils.StrToFloat(extCredits[i], 0) != 0)
                    priceStr += string.Format("{0}:{1}{2} ,", extcreditnames[i + 1], extCredits[i], extcreditunits[i + 1]);
            }
            return priceStr != "" ? priceStr : "暂无定价;";
        }

        public void CreateEmailPreview()
        {
            isuseusersaid = invitationconfiginfo.InvitationEmailTemplate.IndexOf("{5}") > 0;
            emailpreview = string.Format(invitationconfiginfo.InvitationEmailTemplate, "[friend]", userid, invitecodeinfo.Creator, 
                GetUserInviteUrl(invitecodeinfo.Code, true), config.Forumtitle, usersaid, rooturl, avatarSmall, avatarMedium, avatarLarge);
        }

        public string InviteCodeExpireTip(string time)
        {
            datediff = Utils.StrDateDiffHours(time, 0);
            if (datediff < 0)
            {
                string script = "";
                switch (datediff / -24)
                {
                    case 0: script = "明天过期"; break;
                    case 1: script = "后天过期"; break;
                    default: script = ((datediff / -24) + 1).ToString() + "天后过期"; break;
                }
                return "您的邀请链接会在" + script;
            }
            else
            {
                return "<font color=red><b>您的邀请链接已过期</b></font>";
            }
        }

        public string CreateUserExtCreditsStr()
        {
            string result = "";
            string showTemplate = " {0}:{1}{2} ;";
            if (extcreditnames[1] != string.Empty)
                result += string.Format(showTemplate, extcreditnames[1], userinfo.Extcredits1, extcreditunits[1]);
            if (extcreditnames[2] != string.Empty)
                result += string.Format(showTemplate, extcreditnames[2], userinfo.Extcredits2, extcreditunits[2]);
            if (extcreditnames[3] != string.Empty)
                result += string.Format(showTemplate, extcreditnames[3], userinfo.Extcredits3, extcreditunits[3]);
            if (extcreditnames[4] != string.Empty)
                result += string.Format(showTemplate, extcreditnames[4], userinfo.Extcredits4, extcreditunits[4]);
            if (extcreditnames[5] != string.Empty)
                result += string.Format(showTemplate, extcreditnames[5], userinfo.Extcredits5, extcreditunits[5]);
            if (extcreditnames[6] != string.Empty)
                result += string.Format(showTemplate, extcreditnames[6], userinfo.Extcredits6, extcreditunits[6]);
            if (extcreditnames[7] != string.Empty)
                result += string.Format(showTemplate, extcreditnames[7], userinfo.Extcredits7, extcreditunits[7]);
            if (extcreditnames[8] != string.Empty)
                result += string.Format(showTemplate, extcreditnames[8], userinfo.Extcredits8, extcreditunits[8]);
            return result.TrimEnd(';');
        }
    }
}
