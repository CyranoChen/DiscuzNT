using System;
using System.Data;
using System.Text.RegularExpressions;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Web.UI;
using Discuz.Plugin.PasswordMode;

namespace Discuz.Web
{
    /// <summary>
    /// 注册页
    /// </summary>
    public class register : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 可用的模板列表
        /// </summary>
        public DataTable templatelist = Templates.GetValidTemplateList();
        /// <summary>
        /// 此变量等于1时创建用户,否则显示填写用户信息界面
        /// </summary>
        public string createuser = DNTRequest.GetString("createuser");
        /// <summary>
        /// 是否同意注册协议
        /// </summary>
        public string agree = (GeneralConfigs.GetConfig().Rules == 0 ? "true" : DNTRequest.GetFormString("agree"));
        /// <summary>
        /// 邀请码
        /// </summary>
        public string invitecode = DNTRequest.GetString("invitecode");
        public bool allowinvite = false;
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "用户注册";

            if (userid != -1)
            {
                SetUrl(BaseConfigs.GetForumPath);
                SetMetaRefresh();
                SetShowBackLink(false);
                AddMsgLine("不能重复注册用户");
                ispost = true;
                createuser = "1";
                agree = "yes";
                return;
            }


            if (config.Regstatus < 1)
            {
                AddErrLine("论坛当前禁止新用户注册");
                return;
            }

            allowinvite = Utils.InArray(config.Regstatus.ToString(), "2,3");

            #region 用户IP判断
            string msg = Users.CheckRegisterDateDiff(DNTRequest.GetIP());
            if (msg != null)
            {
                AddErrLine(msg);
                return;
            }
            //if (config.Regctrl > 0)
            //{
            //    ShortUserInfo userinfo = Users.GetShortUserInfoByIP(DNTRequest.GetIP());
            //    if (userinfo != null)
            //    {
            //        int Interval = Utils.StrDateDiffHours(userinfo.Joindate, config.Regctrl);
            //        if (Interval <= 0)
            //        {
            //            AddErrLine("抱歉, 系统设置了IP注册间隔限制, 您必须在 " + (Interval * -1) + " 小时后才可以注册");
            //            return;
            //        }
            //    }
            //}

            //if (config.Ipregctrl.Trim() != "")
            //{
            //    string[] regctrl = Utils.SplitString(config.Ipregctrl, "\n");
            //    if (Utils.InIPArray(DNTRequest.GetIP(), regctrl))
            //    {
            //        ShortUserInfo userinfo = Users.GetShortUserInfoByIP(DNTRequest.GetIP());
            //        if (userinfo != null)
            //        {
            //            int Interval = Utils.StrDateDiffHours(userinfo.Joindate, 72);
            //            if (Interval < 0)
            //            {
            //                AddErrLine("抱歉, 系统设置了特殊IP注册限制, 您必须在 " + (Interval * -1) + " 小时后才可以注册");
            //                return;
            //            }
            //        }
            //    }
            //}
            #endregion

            //如果提交了用户注册信息...
            if (!Utils.StrIsNullOrEmpty(createuser) && ispost)
            {
                SetShowBackLink(true);
                InviteCodeInfo inviteCode = null;
                if (allowinvite)
                {
                    if (config.Regstatus == 3 && invitecode == "")
                    {
                        AddErrLine("邀请码不能为空！");
                        return;
                    }
                    if (invitecode != "")
                    {
                        inviteCode = Invitation.GetInviteCodeByCode(invitecode.ToUpper());
                        if (!Invitation.CheckInviteCode(inviteCode))
                        {
                            AddErrLine("邀请码不合法或已过期！");
                            return;
                        }
                    }
                }

                string tmpUserName = DNTRequest.GetString(config.Antispamregisterusername);
                string email = DNTRequest.GetString(config.Antispamregisteremail).Trim().ToLower();
                string tmpBday = DNTRequest.GetString("bday").Trim();

                if (tmpBday == "")
                {
                    tmpBday = string.Format("{0}-{1}-{2}", DNTRequest.GetString("bday_y").Trim(),
                           DNTRequest.GetString("bday_m").Trim(), DNTRequest.GetString("bday_d").Trim());
                }
                tmpBday = (tmpBday == "--" ? "" : tmpBday);

                ValidateUserInfo(tmpUserName, email, tmpBday);

                if (IsErr()) return;

                //如果用户名符合注册规则, 则判断是否已存在
                if (Users.GetUserId(tmpUserName) > 0)
                {
                    AddErrLine("请不要重复提交！");
                    return;
                }

                UserInfo userInfo = CreateUser(tmpUserName, email, tmpBday);

                #region 发送欢迎信息
                if (config.Welcomemsg == 1)
                {
                    // 收件箱
                    PrivateMessageInfo privatemessageinfo = new PrivateMessageInfo();
                    privatemessageinfo.Message = config.Welcomemsgtxt;
                    privatemessageinfo.Subject = "欢迎您的加入! (请勿回复本信息)";
                    privatemessageinfo.Msgto = userInfo.Username;
                    privatemessageinfo.Msgtoid = userInfo.Uid;
                    privatemessageinfo.Msgfrom = PrivateMessages.SystemUserName;
                    privatemessageinfo.Msgfromid = 0;
                    privatemessageinfo.New = 1;
                    privatemessageinfo.Postdatetime = Utils.GetDateTime();
                    privatemessageinfo.Folder = 0;
                    PrivateMessages.CreatePrivateMessage(privatemessageinfo, 0);
                }
                #endregion

                //发送同步数据给应用程序
                Sync.UserRegister(userInfo.Uid, userInfo.Username, userInfo.Password, "");



                SetUrl("index.aspx");
                SetShowBackLink(false);
                SetMetaRefresh(config.Regverify == 0 ? 2 : 5);
                Statistics.ReSetStatisticsCache();

                if (inviteCode != null)
                {
                    Invitation.UpdateInviteCodeSuccessCount(inviteCode.InviteId);
                    if (config.Regstatus == 3)
                    {
                        if (inviteCode.SuccessCount + 1 >= inviteCode.MaxCount)
                            Invitation.DeleteInviteCode(inviteCode.InviteId);
                    }
                }

                if (config.Regverify == 0)
                {
                    UserCredits.UpdateUserCredits(userInfo.Uid);
                    ForumUtils.WriteUserCookie(userInfo, -1, config.Passwordkey);
                    OnlineUsers.UpdateAction(olid, UserAction.Register.ActionID, 0, config.Onlinetimeout);
                    MsgForward("register_succeed");
                    AddMsgLine("注册成功, 返回登录页");
                }
                else
                {
                    if (config.Regverify == 1)
                        AddMsgLine("注册成功, 请您到您的邮箱中点击激活链接来激活您的帐号");
                    else if (config.Regverify == 2)
                        AddMsgLine("注册成功, 但需要系统管理员审核您的帐户后才可登录使用");
                }
                //ManyouApplications.AddUserLog(userInfo.Uid, UserLogActionEnum.Add);
                agree = "yes";
            }
        }

        /// <summary>
        /// 创建用户信息
        /// </summary>
        /// <param name="tmpUsername"></param>
        /// <param name="email"></param>
        /// <param name="tmpBday"></param>
        /// <returns></returns>
        private UserInfo CreateUser(string tmpUsername, string email, string tmpBday)
        {
            // 如果找不到0积分的用户组则用户自动成为待验证用户
            UserInfo userinfo = new UserInfo();
            userinfo.Username = tmpUsername;
            userinfo.Nickname = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("nickname")));
            userinfo.Password = DNTRequest.GetString("password");
            userinfo.Secques = ForumUtils.GetUserSecques(DNTRequest.GetInt("question", 0), DNTRequest.GetString("answer"));
            userinfo.Gender = DNTRequest.GetInt("gender", 0);
            userinfo.Adminid = 0;
            userinfo.Groupexpiry = 0;
            userinfo.Extgroupids = "";
            userinfo.Regip = DNTRequest.GetIP();
            userinfo.Joindate = Utils.GetDateTime();
            userinfo.Lastip = DNTRequest.GetIP();
            userinfo.Lastvisit = Utils.GetDateTime();
            userinfo.Lastactivity = Utils.GetDateTime();
            userinfo.Lastpost = Utils.GetDateTime();
            userinfo.Lastpostid = 0;
            userinfo.Lastposttitle = "";
            userinfo.Posts = 0;
            userinfo.Digestposts = 0;
            userinfo.Oltime = 0;
            userinfo.Pageviews = 0;
            userinfo.Credits = 0;
            userinfo.Extcredits1 = Scoresets.GetScoreSet(1).Init;
            userinfo.Extcredits2 = Scoresets.GetScoreSet(2).Init;
            userinfo.Extcredits3 = Scoresets.GetScoreSet(3).Init;
            userinfo.Extcredits4 = Scoresets.GetScoreSet(4).Init;
            userinfo.Extcredits5 = Scoresets.GetScoreSet(5).Init;
            userinfo.Extcredits6 = Scoresets.GetScoreSet(6).Init;
            userinfo.Extcredits7 = Scoresets.GetScoreSet(7).Init;
            userinfo.Extcredits8 = Scoresets.GetScoreSet(8).Init;
            //userinfo.Avatarshowid = 0;
            userinfo.Email = email;
            userinfo.Bday = tmpBday;
            userinfo.Sigstatus = DNTRequest.GetInt("sigstatus", 1) != 0 ? 1 : 0;
            userinfo.Tpp = DNTRequest.GetInt("tpp", 0);
            userinfo.Ppp = DNTRequest.GetInt("ppp", 0);
            userinfo.Templateid = DNTRequest.GetInt("templateid", 0);
            userinfo.Pmsound = DNTRequest.GetInt("pmsound", 0);
            userinfo.Showemail = DNTRequest.GetInt("showemail", 0);
            userinfo.Salt = "";

            int receivepmsetting = config.Regadvance == 0 ? 3 : DNTRequest.GetInt("receivesetting", 3);//关于短信息枚举值的设置看ReceivePMSettingType类型注释，此处不禁止用户接受系统短信息
            //foreach (string rpms in DNTRequest.GetString("receivesetting").Split(','))
            //{
            //    if (!Utils.StrIsNullOrEmpty(rpms))
            //        receivepmsetting = receivepmsetting | int.Parse(rpms);
            //}

            //if (config.Regadvance == 0)
            //    receivepmsetting = 7;

            userinfo.Newsletter = (ReceivePMSettingType)receivepmsetting;
            userinfo.Invisible = DNTRequest.GetInt("invisible", 0);
            userinfo.Newpm = config.Welcomemsg == 1 ? 1 : 0;
            userinfo.Medals = "";
            userinfo.Accessmasks = DNTRequest.GetInt("accessmasks", 0);
            userinfo.Website = Utils.HtmlEncode(DNTRequest.GetString("website"));
            userinfo.Icq = Utils.HtmlEncode(DNTRequest.GetString("icq"));
            userinfo.Qq = Utils.HtmlEncode(DNTRequest.GetString("qq"));
            userinfo.Yahoo = Utils.HtmlEncode(DNTRequest.GetString("yahoo"));
            userinfo.Msn = Utils.HtmlEncode(DNTRequest.GetString("msn"));
            userinfo.Skype = Utils.HtmlEncode(DNTRequest.GetString("skype"));
            userinfo.Location = Utils.HtmlEncode(DNTRequest.GetString("location"));
            userinfo.Customstatus = (usergroupinfo.Allowcstatus == 1) ? Utils.HtmlEncode(DNTRequest.GetString("customstatus")) : "";
            //userinfo.Avatar = @"avatars\common\0.gif";
            //userinfo.Avatarwidth = 0;
            //userinfo.Avatarheight = 0;
            userinfo.Bio = ForumUtils.BanWordFilter(DNTRequest.GetString("bio"));
            userinfo.Signature = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("signature")));

            PostpramsInfo postpramsinfo = new PostpramsInfo();
            postpramsinfo.Usergroupid = usergroupid;
            postpramsinfo.Attachimgpost = config.Attachimgpost;
            postpramsinfo.Showattachmentpath = config.Showattachmentpath;
            postpramsinfo.Hide = 0;
            postpramsinfo.Price = 0;
            postpramsinfo.Sdetail = userinfo.Signature;
            postpramsinfo.Smileyoff = 1;
            postpramsinfo.Bbcodeoff = 1 - usergroupinfo.Allowsigbbcode;
            postpramsinfo.Parseurloff = 1;
            postpramsinfo.Showimages = usergroupinfo.Allowsigimgcode;
            postpramsinfo.Allowhtml = 0;
            postpramsinfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            postpramsinfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            postpramsinfo.Smiliesmax = config.Smiliesmax;
            userinfo.Sightml = UBB.UBBToHTML(postpramsinfo);

            userinfo.Authtime = Utils.GetDateTime();

            //邮箱激活链接验证
            if (config.Regverify == 1)
            {
                userinfo.Authstr = ForumUtils.CreateAuthStr(20);
                userinfo.Authflag = 1;
                userinfo.Groupid = 8;
                SendEmail(tmpUsername, DNTRequest.GetString("password").Trim(), DNTRequest.GetString(config.Antispamregisteremail).Trim(), userinfo.Authstr);
            }
            //系统管理员进行后台验证
            else if (config.Regverify == 2)
            {
                userinfo.Authstr = DNTRequest.GetString("website");
                userinfo.Groupid = 8;
                userinfo.Authflag = 1;
            }
            else
            {
                userinfo.Authstr = "";
                userinfo.Authflag = 0;
                userinfo.Groupid = UserCredits.GetCreditsUserGroupId(0).Groupid;
            }
            userinfo.Realname = DNTRequest.GetString("realname");
            userinfo.Idcard = DNTRequest.GetString("idcard");
            userinfo.Mobile = DNTRequest.GetString("mobile");
            userinfo.Phone = DNTRequest.GetString("phone");

            //第三方加密验证模式
            if (config.Passwordmode > 1 && PasswordModeProvider.GetInstance() != null)
            {
                userinfo.Uid = PasswordModeProvider.GetInstance().CreateUserInfo(userinfo);
            }
            else
            {
                userinfo.Password = Utils.MD5(userinfo.Password);
                userinfo.Uid = Users.CreateUser(userinfo);
            }
            return userinfo;
        }

        /// <summary>
        /// 验证用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="birthday"></param>
        private void ValidateUserInfo(string username, string email, string birthday)
        {

            #region CheckUserName
            if (string.IsNullOrEmpty(username))
            {
                AddErrLine("用户名不能为空");
                return;
            }
            if (Utils.GetStringLength(username) > 20)
            {
                AddErrLine("用户名不得超过20个字符");
                return;
            }
            if (Utils.GetStringLength(username) < 3)
            {
                AddErrLine("用户名不得小于3个字符");
                return;
            }
            if (username.IndexOf("　") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1)
            {
                //如果用户名符合注册规则, 则判断是否已存在
                AddErrLine("用户名中不允许包含全角空格符");
                return;
            }
            if (username.IndexOf(" ") != -1)
            {
                //如果用户名符合注册规则, 则判断是否已存在
                AddErrLine("用户名中不允许包含空格");
                return;
            }
            if (username.IndexOf(":") != -1)
            {
                //如果用户名符合注册规则, 则判断是否已存在
                AddErrLine("用户名中不允许包含冒号");
                return;
            }
            if (Users.GetUserId(username) > 0)
            {
                //如果用户名符合注册规则, 则判断是否已存在
                AddErrLine("该用户名已存在");
                return;
            }
            if ((!Utils.IsSafeSqlString(username)) || (!Utils.IsSafeUserInfoString(username)))
            {
                AddErrLine("用户名中存在非法字符");
                return;
            }
            // 如果用户名属于禁止名单, 或者与负责发送新用户注册欢迎信件的用户名称相同...
            if (username.Trim() == PrivateMessages.SystemUserName ||
                     ForumUtils.IsBanUsername(username, config.Censoruser))
            {
                AddErrLine("用户名 \"" + username + "\" 不允许在本论坛使用");
                return;
            }
            #endregion

            #region CheckPassword
            // 检查密码
            if (DNTRequest.GetString("password").Equals(""))
            {
                AddErrLine("密码不能为空");
                return;
            }
            if (!DNTRequest.GetString("password").Equals(DNTRequest.GetString("password2")))
            {
                AddErrLine("两次密码输入必须相同");
                return;
            }
            if (DNTRequest.GetString("password").Length < 6)
            {
                AddErrLine("密码不得少于6个字符");
                return;
            }
            #endregion

            #region CheckMail
            if (string.IsNullOrEmpty(email))
            {
                AddErrLine("Email不能为空");
                return;
            }
            if (!Utils.IsValidEmail(email))
            {
                AddErrLine("Email格式不正确");
                return;
            }
            if (!Users.ValidateEmail(email))
            {
                AddErrLine("Email: \"" + email + "\" 已经被其它用户注册使用");
                return;
            }

            string emailhost = Utils.GetEmailHostName(email);
            // 允许名单规则优先于禁止名单规则
            if (config.Accessemail.Trim() != "")
            {
                // 如果email后缀 不属于 允许名单
                if (!Utils.InArray(emailhost, config.Accessemail.Replace("\r\n", "\n"), "\n"))
                {
                    AddErrLine("Email: \"" + email + "\" 不在本论坛允许范围之类, 本论坛只允许用户使用这些Email地址注册: " +
                               config.Accessemail.Replace("\n", ",").Replace("\r", ""));
                    return;
                }
            }
            if (config.Censoremail.Trim() != "")
            {
                // 如果email后缀 属于 禁止名单
                if (Utils.InArray(emailhost, config.Censoremail.Replace("\r\n", "\n"), "\n"))
                {
                    AddErrLine("Email: \"" + email + "\" 不允许在本论坛使用, 本论坛不允许用户使用的Email地址包括: " +
                               config.Censoremail.Replace("\n", ",").Replace("\r", ""));
                    return;
                }
            }
            #endregion
            //实名验证
            #region CheckRealInfo

            string realName = DNTRequest.GetString("realname").Trim();
            string idCard = DNTRequest.GetString("idcard").Trim();
            string mobile = DNTRequest.GetString("mobile").Trim();
            string phone = DNTRequest.GetString("phone").Trim();

            if (config.Realnamesystem == 1)
            {
                if (string.IsNullOrEmpty(realName)||Utils.GetStringLength(realName) > 10)
                {
                    AddErrLine("真实姓名不能为空且不能大于10个字符");
                    return;
                }
                if (string.IsNullOrEmpty(idCard) || idCard.Length > 20)
                {
                    AddErrLine("身份证号码不能为空且不能大于20个字符");
                    return;
                }
                if (string.IsNullOrEmpty(mobile) && string.IsNullOrEmpty(phone))
                {
                    AddErrLine("移动电话号码或固定电话号码必须填写其中一项");
                    return;
                }
                if (mobile.Length > 20)
                {
                    AddErrLine("移动电话号码不能大于20个字符");
                    return;
                }
                if (phone.Length > 20)
                {
                    AddErrLine("固定电话号码不能大于20个字符");
                    return;
                }
            }
            #endregion
            if (!string.IsNullOrEmpty(idCard) && !Regex.IsMatch(idCard, @"^[\x20-\x80]+$"))
            {
                AddErrLine("身份证号码中含有非法字符");
                return;
            }
            if (!string.IsNullOrEmpty(mobile) && !Regex.IsMatch(mobile, @"^[\d|-]+$"))
            {
                AddErrLine("移动电话号码中含有非法字符");
                return;
            }
            if (!string.IsNullOrEmpty(phone) && !Regex.IsMatch(phone, @"^[\d|-]+$"))
            {
                AddErrLine("固定电话号码中含有非法字符");
                return;
            }

            //用户注册模板中,生日可以单独用一个名为bday的文本框, 也可以分别用bday_y bday_m bday_d三个文本框, 用户可不填写
            if (!Utils.IsDateString(birthday) && !string.IsNullOrEmpty(birthday))
            {
                AddErrLine("生日格式错误, 如果不想填写生日请置空");
                return;
            }
            if (Utils.GetStringLength(DNTRequest.GetString("bio").Trim()) > 500)
            {
                //如果自我介绍超过500...
                AddErrLine("自我介绍不得超过500个字符");
                return;
            }
            if (Utils.GetStringLength(DNTRequest.GetString("signature").Trim()) > 500)
            {
                //如果签名超过500...
                AddErrLine("签名不得超过500个字符");
                return;
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="emailaddress"></param>
        /// <param name="authstr"></param>
        private void SendEmail(string username, string password, string emailaddress, string authstr)
        {
            Emails.DiscuzSmtpMail(username, emailaddress, password, authstr);
        }
    }
}