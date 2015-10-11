using System;
using System.Data;
using System.Text;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Web.UI;
using Discuz.Plugin.PasswordMode;

namespace Discuz.Web
{
    /// <summary>
    /// 登录
    /// </summary>
    public class login : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 登录所使用的用户名
        /// </summary>
        public string postusername = Utils.HtmlEncode(DNTRequest.GetString("postusername")).Trim();
        /// <summary>
        /// 登陆时的密码验证信息
        /// </summary>
        public string loginauth = DNTRequest.GetString("loginauth");
        /// <summary>
        /// 登陆时提交的密码
        /// </summary>
        public string postpassword = "";
        /// <summary>
        /// 登陆成功后跳转的链接
        /// </summary>
        public string referer = Utils.HtmlEncode(DNTRequest.GetString("referer"));
        /// <summary>
        /// 是否跨页面提交
        /// </summary>
        public bool loginsubmit = DNTRequest.GetString("loginsubmit") == "true" ? true : false;
        /// <summary>
        /// 重设Email的加密校验，确保是该用户在当前页面操作的
        /// </summary>
        public string authstr = "";
        /// <summary>
        /// 需要激活的用户id
        /// </summary>
        public int needactiveuid = -1;
        /// <summary>
        /// 重置的Email信息的有效时间
        /// </summary>
        public string timestamp = "";
        /// <summary>
        /// 需要激活的用户Email
        /// </summary>
        public string email = "";

        public int inapi = 0;

        #endregion

        protected override void ShowPage()
        {
            pagetitle = "用户登录";
            inapi = DNTRequest.GetInt("inapi", 0);
            if (userid != -1)
            {
                SetUrl(BaseConfigs.GetForumPath);
                AddMsgLine("您已经登录，无须重复登录");
                ispost = true;
                SetLeftMenuRefresh();

                if (APIConfigs.GetConfig().Enable)
                    APILogin(APIConfigs.GetConfig());
            }

            if (LoginLogs.UpdateLoginLog(DNTRequest.GetIP(), false) >= 5)
            {
                AddErrLine("您已经多次输入密码错误, 请15分钟后再登录");
                loginsubmit = false;
                return;
            }

            SetReUrl();

            //如果提交...
            if (DNTRequest.IsPost())
            {
                SetBackLink();

                //如果没输入验证码就要求用户填写
                if (isseccode && DNTRequest.GetString("vcode") == "")
                {
                    postusername = DNTRequest.GetString("username");
                    loginauth = DES.Encode(DNTRequest.GetString("password"), config.Passwordkey).Replace("+", "[");
                    loginsubmit = true;
                    return;
                }

                if (config.Emaillogin == 1 && Utils.IsValidEmail(DNTRequest.GetString("username")))
                {
                    DataTable dt = Users.GetUserInfoByEmail(DNTRequest.GetString("username"));
                    if (dt.Rows.Count == 0)
                    {
                        AddErrLine("用户不存在");
                        return;
                    }
                    if (dt.Rows.Count > 1)
                    {
                        AddErrLine("您所使用Email不唯一，请使用用户名登陆");
                        return;
                    }
                    if (dt.Rows.Count == 1)
                    {
                        postusername = dt.Rows[0]["username"].ToString();
                    }
                }

                if (config.Emaillogin == 0)
                {
                    if ((Users.GetUserId(DNTRequest.GetString("username")) == 0))
                        AddErrLine("用户不存在");
                }

                if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("password")) && Utils.StrIsNullOrEmpty(DNTRequest.GetString("loginauth")))
                    AddErrLine("密码不能为空");

                if (IsErr()) return;

                ShortUserInfo userInfo = GetShortUserInfo();

                if (userInfo != null)
                {
                    #region 当前用户所在用户组为"禁止访问"或"等待激活"时

                    if ((userInfo.Groupid == 4 || userInfo.Groupid == 5) && userInfo.Groupexpiry != 0 && userInfo.Groupexpiry <= Utils.StrToInt(DateTime.Now.ToString("yyyyMMdd"), 0))
                    {
                        //根据当前用户的积分获取对应积分用户组
                        UserGroupInfo groupInfo = UserCredits.GetCreditsUserGroupId(userInfo.Credits);
                        usergroupid = groupInfo.Groupid != 0 ? groupInfo.Groupid : usergroupid;
                        userInfo.Groupid = usergroupid;
                        Users.UpdateUserGroup(userInfo.Uid, usergroupid);
                    }

                    if (userInfo.Groupid == 5)// 5-禁止访问
                    {
                        AddErrLine("您所在的用户组，已经被禁止访问");
                        return;
                    }

                    if (userInfo.Groupid == 8)
                    {
                        if (config.Regverify == 1)
                        {
                            needactiveuid = userInfo.Uid;
                            email = userInfo.Email;
                            timestamp = DateTime.Now.Ticks.ToString();
                            authstr = Utils.MD5(string.Concat(userInfo.Password, config.Passwordkey, timestamp));
                            AddMsgLine("请您到您的邮箱中点击激活链接来激活您的帐号");
                        }
                        else if (config.Regverify == 2)
                            AddMsgLine("您需要等待一些时间, 待系统管理员审核您的帐户后才可登录使用");
                        else
                            AddErrLine("抱歉, 您的用户身份尚未得到验证");

                        loginsubmit = false;
                        return;
                    }
                    #endregion

                    if (!Utils.StrIsNullOrEmpty(userInfo.Secques) && loginsubmit && Utils.StrIsNullOrEmpty(DNTRequest.GetString("loginauth")))
                    {
                        loginauth = DES.Encode(DNTRequest.GetString("password"), config.Passwordkey).Replace("+", "[");
                    }
                    else
                    {
                        //通过api整合的程序登录
                        if (APIConfigs.GetConfig().Enable)
                            APILogin(APIConfigs.GetConfig());


                        AddMsgLine("登录成功, 返回登录前页面");

                        #region 无延迟更新在线信息和相关用户信息
                        ForumUtils.WriteUserCookie(userInfo.Uid, TypeConverter.StrToInt(DNTRequest.GetString("expires"), -1),
        config.Passwordkey, DNTRequest.GetInt("templateid", 0), DNTRequest.GetInt("loginmode", -1));
                        //oluserinfo = OnlineUsers.UpdateInfo(config.Passwordkey, config.Onlinetimeout);
                        oluserinfo = OnlineUsers.UpdateInfo(config.Passwordkey, config.Onlinetimeout, userInfo.Uid, "");
                        olid = oluserinfo.Olid;
                        username = DNTRequest.GetString("username");
                        userid = userInfo.Uid;
                        usergroupinfo = UserGroups.GetUserGroupInfo(userInfo.Groupid);
                        useradminid = usergroupinfo.Radminid; // 根据用户组得到相关联的管理组id


                        OnlineUsers.UpdateAction(olid, UserAction.Login.ActionID, 0);
                        LoginLogs.DeleteLoginLog(DNTRequest.GetIP());
                        Users.UpdateUserCreditsAndVisit(userInfo.Uid, DNTRequest.GetIP());
                        #endregion

                        loginsubmit = false;
                        string reurl = Utils.UrlDecode(ForumUtils.GetReUrl());
                        SetUrl(reurl.IndexOf("register.aspx") < 0 ? reurl : forumpath + "index.aspx");

                        SetLeftMenuRefresh();

                        //同步登录到第三方应用
                        if (APIConfigs.GetConfig().Enable)
                            AddMsgLine(Sync.GetLoginScript(userid, username));

                        if (!APIConfigs.GetConfig().Enable || !Sync.NeedAsyncLogin())
                            MsgForward("login_succeed", true);
                    }
                }
                else
                {
                    int errcount = LoginLogs.UpdateLoginLog(DNTRequest.GetIP(), true);
                    if (errcount > 5)
                        AddErrLine("您已经输入密码5次错误, 请15分钟后再试");
                    else
                        AddErrLine(string.Format("密码或安全提问第{0}次错误, 您最多有5次机会重试", errcount));
                }
                if (IsErr()) return;

                ForumUtils.WriteUserCreditsCookie(userInfo, usergroupinfo.Grouptitle);
            }
        }

        /// <summary>
        /// 设置BackLink
        /// </summary>
        private void SetBackLink()
        {
            StringBuilder builder = new StringBuilder();
            foreach (string key in System.Web.HttpContext.Current.Request.QueryString.AllKeys)
            {
                //if (key != "postusername")
                if (!string.IsNullOrEmpty(key) && !Utils.InArray(key, "postusername"))
                    builder.AppendFormat("&{0}={1}", key, DNTRequest.GetQueryString(key));
            }
            question = DNTRequest.GetFormInt("question", 0);
            if (question > 0)
                builder.AppendFormat("&question={0}", question);
            base.SetBackLink("login.aspx?postusername=" + Utils.UrlEncode(DNTRequest.GetString("username")) + builder);
        }

        /// <summary>
        /// 获取用户id
        /// </summary>
        /// <returns></returns>
        private ShortUserInfo GetShortUserInfo()
        {
            postpassword = !Utils.StrIsNullOrEmpty(loginauth) ?
                    DES.Decode(loginauth.Replace("[", "+"), config.Passwordkey) :
                    DNTRequest.GetString("password");

            postusername = Utils.StrIsNullOrEmpty(postusername) ? DNTRequest.GetString("username") : postusername;

            int uid = -1;
            switch (config.Passwordmode)
            {
                case 1://动网兼容模式
                    {
                        if (config.Secques == 1 && (!Utils.StrIsNullOrEmpty(loginauth) || !loginsubmit))
                            uid = Users.CheckDvBbsPasswordAndSecques(postusername, postpassword, DNTRequest.GetFormInt("question", 0), DNTRequest.GetString("answer"));
                        else
                            uid = Users.CheckDvBbsPassword(postusername, postpassword);
                        break;
                    }
                case 0://默认模式
                    {
                        if (config.Secques == 1 && (!Utils.StrIsNullOrEmpty(loginauth) || !loginsubmit))
                            uid = Users.CheckPasswordAndSecques(postusername, postpassword, true, DNTRequest.GetFormInt("question", 0), DNTRequest.GetString("answer"));
                        else
                            uid = Users.CheckPassword(postusername, postpassword, true);
                        break;
                    }
                default: //第三方加密验证模式
                    {
                        return (ShortUserInfo)Users.CheckThirdPartPassword(postusername, postpassword, DNTRequest.GetFormInt("question", 0), DNTRequest.GetString("answer"));
                    }
            }
            if (uid != -1)
                Users.UpdateTrendStat(TrendType.Login);
            return uid > 0 ? Users.GetShortUserInfo(uid) : null;
        }



        /// <summary>
        /// 设置reurl
        /// </summary>
        private void SetReUrl()
        {
            //未提交或跨页提交时
            if (!DNTRequest.IsPost() || referer != "")
            {
                string r = "";
                if (referer != "")
                    r = DNTRequest.GetUrlReferrer();
                else
                {
                    if ((DNTRequest.GetUrlReferrer() == "") || (DNTRequest.GetUrlReferrer().IndexOf("login") > -1) || DNTRequest.GetUrlReferrer().IndexOf("logout") > -1)
                        r = "index.aspx";
                    else
                        r = DNTRequest.GetUrlReferrer();
                }
                Utils.WriteCookie("reurl", (DNTRequest.GetQueryString("reurl") == "" || DNTRequest.GetQueryString("reurl").IndexOf("login.aspx") > -1) ? r : DNTRequest.GetQueryString("reurl"));
            }
        }

        private void APILogin(APIConfigInfo apiInfo)
        {
            ApplicationInfo appInfo = null;
            ApplicationInfoCollection appcollection = apiInfo.AppCollection;
            foreach (ApplicationInfo newapp in appcollection)
            {
                if (newapp.APIKey == DNTRequest.GetString("api_key"))
                    appInfo = newapp;
            }

            if (appInfo == null)
                return;

            this.Load += delegate
            {
                RedirectAPILogin(appInfo);
                this.Load += delegate { };
            };
        }


        private void RedirectAPILogin(ApplicationInfo appInfo)
        {
            string expires = DNTRequest.GetFormString("expires");
            DateTime expireUTCTime;
            if (Utils.StrIsNullOrEmpty(expires))
                expireUTCTime = DateTime.Parse(Users.GetShortUserInfo(userid).Lastvisit).ToUniversalTime().AddSeconds(
                    Convert.ToDouble(Request.Cookies["dnt"]["expires"].ToString()));
            else
                expireUTCTime = DateTime.UtcNow.AddSeconds(Convert.ToDouble(expires));

            expires = Utils.ConvertToUnixTimestamp(expireUTCTime).ToString();

            //CreateToken
            OnlineUsers.UpdateAction(olid, UserAction.Login.ActionID, 0);
            string next = DNTRequest.GetString("next");
            string time = "";
            OnlineUserInfo oui = OnlineUsers.GetOnlineUser(olid);
            if (oui == null)
                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            else
                time = DateTime.Parse(oui.Lastupdatetime).ToString("yyyy-MM-dd HH:mm:ss");

            string authToken = DES.Encode(string.Format("{0},{1},{2}", olid, time, expires), appInfo.Secret.Substring(0, 10)).Replace("+", "[");
            Response.Redirect(string.Format("{0}{1}auth_token={2}{3}", appInfo.CallbackUrl, appInfo.CallbackUrl.IndexOf("?") > 0 ? "&" : "?", authToken, next == "" ? next : "&next=" + next));
        }

        private void SetLeftMenuRefresh()
        {
            SetMetaRefresh();
            SetShowBackLink(false);
            AddScript("if (top.document.getElementById('leftmenu')){top.frames['leftmenu'].location.reload();}");
        }
    }
}