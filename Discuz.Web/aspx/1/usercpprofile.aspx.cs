using System;
using System.Data;
using System.Data.SqlClient;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;
using System.Text.RegularExpressions;
using System.IO;

namespace Discuz.Web
{
    /// <summary>
    /// 更新用户档案页面
    /// </summary>
    public class usercpprofile : UserCpPage
    {
        public string avatarFlashParam = "";
        public string avatarImage = "";
        public string sig = string.Empty;

        public string action = DNTRequest.GetString("action");

        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";

            if (!IsLogin())
                return;

            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }

                ValidateInfo();

                if (IsErr())
                    return;

                if (page_err == 0)
                {
                    UserInfo oldUserInfo = Users.GetUserInfo(userid);
                    UserInfo userInfo = oldUserInfo.Clone();
                    //需要判断签名是否修改过
                    sig = oldUserInfo.Sightml;
                    userInfo.Uid = userid;
                    userInfo.Username = username;
                    userInfo.Nickname = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("nickname")));
                    userInfo.Gender = DNTRequest.GetInt("gender", 0);
                    userInfo.Realname = DNTRequest.GetString("realname");
                    userInfo.Idcard = DNTRequest.GetString("idcard");
                    userInfo.Mobile = DNTRequest.GetString("mobile");
                    userInfo.Phone = DNTRequest.GetString("phone");
                    userInfo.Email = DNTRequest.GetString("email").Trim().ToLower();
                    if (userInfo.Email != oldUserInfo.Email && !Users.ValidateEmail(userInfo.Email, userid))
                    {
                        AddErrLine("Email: \"" + userInfo.Email + "\" 已经被其它用户注册使用");
                        return;
                    }

                    userInfo.Bday = Utils.HtmlEncode(DNTRequest.GetString("bday"));
                    userInfo.Showemail = DNTRequest.GetInt("showemail", 1);

                    if (DNTRequest.GetString("website").IndexOf(".") > -1 && !DNTRequest.GetString("website").ToLower().StartsWith("http"))
                        userInfo.Website = Utils.HtmlEncode("http://" + DNTRequest.GetString("website"));
                    else
                        userInfo.Website = Utils.HtmlEncode(DNTRequest.GetString("website"));

                    userInfo.Icq = Utils.HtmlEncode(DNTRequest.GetString("icq"));
                    userInfo.Qq = Utils.HtmlEncode(DNTRequest.GetString("qq"));
                    userInfo.Yahoo = Utils.HtmlEncode(DNTRequest.GetString("yahoo"));
                    userInfo.Msn = Utils.HtmlEncode(DNTRequest.GetString("msn"));
                    userInfo.Skype = Utils.HtmlEncode(DNTRequest.GetString("skype"));
                    userInfo.Location = Utils.HtmlEncode(DNTRequest.GetString("location"));
                    userInfo.Bio = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("bio")));

                    PostpramsInfo postPramsInfo = new PostpramsInfo();
                    postPramsInfo.Usergroupid = usergroupid;
                    postPramsInfo.Attachimgpost = config.Attachimgpost;
                    postPramsInfo.Showattachmentpath = config.Showattachmentpath;
                    postPramsInfo.Hide = 0;
                    postPramsInfo.Price = 0;
                    //获取提交的内容并进行脏字和Html处理
                    postPramsInfo.Sdetail = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("signature"))); ;
                    postPramsInfo.Smileyoff = 1;
                    postPramsInfo.Bbcodeoff = 1 - usergroupinfo.Allowsigbbcode;
                    postPramsInfo.Parseurloff = 1;
                    postPramsInfo.Showimages = usergroupinfo.Allowsigimgcode;
                    postPramsInfo.Allowhtml = 0;
                    postPramsInfo.Signature = 1;
                    postPramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
                    postPramsInfo.Customeditorbuttoninfo = null;
                    postPramsInfo.Smiliesmax = config.Smiliesmax;
                    postPramsInfo.Signature = 1;

                    userInfo.Sightml = UBB.UBBToHTML(postPramsInfo);
                    if (sig != userInfo.Sightml)
                    {
                        Sync.UpdateSignature(userid, userInfo.Username, userInfo.Sightml, "");
                    }
                    if (userInfo.Sightml.Length >= 1000)
                    {
                        AddErrLine("您的签名转换后超出系统最大长度， 请返回修改");
                        return;
                    }

                    userInfo.Signature = postPramsInfo.Sdetail;
                    userInfo.Sigstatus = DNTRequest.GetInt("sigstatus", 0) != 0 ? 1 : 0;


                    if (CheckModified(oldUserInfo, userInfo))
                    {
                        Users.UpdateUserProfile(userInfo);
                        Sync.UpdateProfile(userInfo.Uid, userInfo.Username, "");
                    }
                    OnlineUsers.DeleteUserByUid(userid);    //删除在线表中的信息，使之重建该用户在线表信息
                    //ManyouApplications.AddUserLog(userid, UserLogActionEnum.Update);
                    
                    ForumUtils.WriteCookie("sigstatus", userInfo.Sigstatus.ToString());

                    SetUrl("usercpprofile.aspx");
                    SetMetaRefresh();
                    SetShowBackLink(true);
                    AddMsgLine("修改个人档案完毕");
                }
            }
            else
            {
                pagename += action == "" ? "" : "?action=" + action;

                UserInfo userInfo = Users.GetUserInfo(userid);//olid
                avatarFlashParam = Utils.GetRootUrl(BaseConfigs.GetForumPath) + "images/common/camera.swf?nt=1&inajax=1&appid=" +
                    Utils.MD5(userInfo.Username + userInfo.Password + userInfo.Uid + olid) + "&input=" +
                    DES.Encode(userid + "," + olid, config.Passwordkey) + "&ucapi=" + Utils.UrlEncode(Utils.GetRootUrl(BaseConfigs.GetForumPath) +
                    "tools/ajax.aspx");
                avatarImage = Avatars.GetAvatarUrl(userid);
            }
        }

        /// <summary>
        /// 检查除签名外的其他用户资料修改
        /// </summary>
        /// <param name="oldUserInfo"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private bool CheckModified(UserInfo oldUserInfo, UserInfo userInfo)
        {
            return
                userInfo.Uid != oldUserInfo.Uid ||
                userInfo.Nickname != oldUserInfo.Nickname ||
                userInfo.Gender != oldUserInfo.Gender ||
                userInfo.Realname != oldUserInfo.Realname ||
                userInfo.Idcard != oldUserInfo.Idcard ||
                userInfo.Mobile != oldUserInfo.Mobile ||
                userInfo.Phone != oldUserInfo.Phone ||
                userInfo.Email != oldUserInfo.Email ||
                userInfo.Bday != oldUserInfo.Bday ||
                userInfo.Showemail != oldUserInfo.Showemail ||
                userInfo.Website != oldUserInfo.Website ||

                userInfo.Icq != oldUserInfo.Icq ||
                userInfo.Qq != oldUserInfo.Qq ||
                userInfo.Yahoo != oldUserInfo.Yahoo ||
                userInfo.Msn != oldUserInfo.Msn ||
                userInfo.Skype != oldUserInfo.Skype ||
                userInfo.Location != oldUserInfo.Location ||
                userInfo.Bio != oldUserInfo.Bio ||
                userInfo.Signature != oldUserInfo.Signature ||
                userInfo.Sigstatus != oldUserInfo.Sigstatus;
        }

        /// <summary>
        /// 验证提交信息
        /// </summary>
        public void ValidateInfo()
        {
            //实名验证
            if (config.Realnamesystem == 1)
            {
                if (DNTRequest.GetString("realname").Trim() == "")
                    AddErrLine("真实姓名不能为空");
                else if (DNTRequest.GetString("realname").Trim().Length > 10)
                    AddErrLine("真实姓名不能大于10个字符");

                if (DNTRequest.GetString("idcard").Trim() == "")
                    AddErrLine("身份证号码不能为空");
                else if (DNTRequest.GetString("idcard").Trim().Length > 20)
                    AddErrLine("身份证号码不能大于20个字符");

                if (DNTRequest.GetString("mobile").Trim() == "" && DNTRequest.GetString("phone").Trim() == "")
                    AddErrLine("移动电话号码和是固定电话号码必须填写其中一项");

                if (DNTRequest.GetString("mobile").Trim().Length > 20)
                    AddErrLine("移动电话号码不能大于20个字符");

                if (DNTRequest.GetString("phone").Trim().Length > 20)
                    AddErrLine("固定电话号码不能大于20个字符");
            }

            if (DNTRequest.GetString("idcard").Trim() != "" && !Regex.IsMatch(DNTRequest.GetString("idcard").Trim(), @"^[\x20-\x80]+$"))
                AddErrLine("身份证号码中含有非法字符");

            if (DNTRequest.GetString("mobile").Trim() != "" && !Regex.IsMatch(DNTRequest.GetString("mobile").Trim(), @"^[\d|-]+$"))
                AddErrLine("移动电话号码中含有非法字符");

            if (DNTRequest.GetString("phone").Trim() != "" && !Regex.IsMatch(DNTRequest.GetString("phone").Trim(), @"^[\d|-]+$"))
                AddErrLine("固定电话号码中含有非法字符");

            string email = DNTRequest.GetString("email").Trim().ToLower();
            if (Utils.StrIsNullOrEmpty(email))
            {
                AddErrLine("Email不能为空");
                return;
            }
            else if (!Utils.IsValidEmail(email))
            {
                AddErrLine("Email格式不正确");
                return;
            }
            else
            {
                // 允许名单规则优先于禁止名单规则
                if (!Utils.StrIsNullOrEmpty(config.Accessemail) && !Utils.InArray(Utils.GetEmailHostName(email), config.Accessemail.Replace("\r\n", "\n"), "\n")) // 如果email后缀 不属于 允许名单
                {
                    AddErrLine("Email: \"" + email + "\" 不在本论坛允许范围之类, 本论坛只允许用户使用这些Email地址注册: " + config.Accessemail.Replace("\n", ",&nbsp;"));
                    return;
                }
                else if (!Utils.StrIsNullOrEmpty(config.Censoremail) && Utils.InArray(Utils.GetEmailHostName(email), config.Censoremail.Replace("\r\n", "\n"), "\n")) // 如果email后缀 属于 禁止名单
                {
                    AddErrLine("Email: \"" + email + "\" 不允许在本论坛使用, 本论坛不允许用户使用的Email地址包括: " + config.Censoremail.Replace("\n", ",&nbsp;"));
                    return;
                }
                if (DNTRequest.GetString("bio").Length > 500) //如果自我介绍超过500...
                {
                    AddErrLine("自我介绍不得超过500个字符");
                    return;
                }
                if (DNTRequest.GetString("signature").Length > 500) //如果签名超过500...
                {
                    AddErrLine("签名不得超过500个字符");
                    return;
                }
            }
            if (!Utils.StrIsNullOrEmpty(DNTRequest.GetString("nickname")) && ForumUtils.IsBanUsername(DNTRequest.GetString("nickname"), config.Censoruser))
            {
                AddErrLine("昵称 \"" + DNTRequest.GetString("nickname") + "\" 不允许在本论坛使用");
                return;
            }
            if (DNTRequest.GetString("signature").Length > usergroupinfo.Maxsigsize)
            {
                AddErrLine(string.Format("您的签名长度超过 {0} 字符的限制，请返回修改。", usergroupinfo.Maxsigsize));
                return;
            }
        }
    }
}