using System;
#if NET1
#else
using System.Collections.Generic;
#endif
using System.Text;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using System.Xml.Serialization;
using System.IO;
using System.Security.Cryptography;
using Discuz.Plugin.PasswordMode;
using Discuz.Config;

namespace Discuz.Web.Services.API.Actions
{
    public class Auth : ActionBase
    {
        /// <summary>
        /// 为客户端创建令牌
        /// </summary>
        /// <returns></returns>
        public string CreateToken()
        {
            string returnStr = "";
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return returnStr;
            }

            //应用程序类型为Web的时候应用程序没有调用此方法的权限
            if (this.App.ApplicationType == (int)ApplicationType.WEB)
            {
                ErrorCode = (int)ErrorType.API_EC_PERMISSION_DENIED;
                return returnStr;
            }

            OnlineUserInfo oluserinfo = OnlineUsers.UpdateInfo(Config.Passwordkey, Config.Onlinetimeout);
            int olid = oluserinfo.Olid;

            string expires = string.Empty;
            DateTime expireUTCTime;
            TokenInfo token = new TokenInfo();

            if (System.Web.HttpContext.Current.Request.Cookies["dnt"] == null || System.Web.HttpContext.Current.Request.Cookies["dnt"]["expires"] == null)
            {
                token.Token = "";
                if (Format == FormatType.JSON)
                    returnStr = "";
                else
                    returnStr = SerializationHelper.Serialize(token);
                return returnStr;
            }
            expires = System.Web.HttpContext.Current.Request.Cookies["dnt"]["expires"].ToString();
            ShortUserInfo userinfo = Discuz.Forum.Users.GetShortUserInfo(oluserinfo.Userid);
            expireUTCTime = DateTime.Parse(userinfo.Lastvisit).ToUniversalTime().AddSeconds(Convert.ToDouble(expires));
            expires = Utils.ConvertToUnixTimestamp(expireUTCTime).ToString();

            string time = string.Empty;
            if (oluserinfo == null)
                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            else
                time = DateTime.Parse(oluserinfo.Lastupdatetime).ToString("yyyy-MM-dd HH:mm:ss");

            string authToken = Common.DES.Encode(string.Format("{0},{1},{2}", olid.ToString(), time, expires), this.Secret.Substring(0, 10)).Replace("+", "[");
            token.Token = authToken;
            if (Format == FormatType.JSON)
                returnStr = authToken;
            else
                returnStr = SerializationHelper.Serialize(token);
            return returnStr;

        }

        /// <summary>
        /// 获得会话
        /// </summary>
        /// <returns></returns>
        public string GetSession()
        {
            string returnStr = "";
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return returnStr;
            }

            if (GetParam("auth_token") == null)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return returnStr;
            }

            string auth_token = GetParam("auth_token").ToString().Replace("[", "+");
            string a = Discuz.Common.DES.Decode(auth_token, Secret.Substring(0, 10));

            string[] userstr = a.Split(',');
            if (userstr.Length != 3)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return returnStr;
            }

            int olid = Utils.StrToInt(userstr[0], -1);
            OnlineUserInfo oluser = OnlineUsers.GetOnlineUser(olid);
            if (oluser == null)
            {
                ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                return returnStr;
            }
            string time = DateTime.Parse(oluser.Lastupdatetime).ToString("yyyy-MM-dd HH:mm:ss");
            if (time != userstr[1])
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return returnStr;
            }
            byte[] md5_result = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(olid.ToString() + Secret));

            StringBuilder sessionkey_builder = new StringBuilder();

            foreach (byte b in md5_result)
                sessionkey_builder.Append(b.ToString("x2"));

            string sessionkey = string.Format("{0}-{1}", sessionkey_builder.ToString(), oluser.Userid.ToString());
            SessionInfo session = new SessionInfo();
            session.SessionKey = sessionkey;
            session.UId = oluser.Userid;
            session.UserName = oluser.Username;
            session.Expires = Utils.StrToInt(userstr[2], 0);

            if (Format == FormatType.JSON)
                returnStr = string.Format(@"{{""session_key"":""{0}"",""uid"":{1},""user_name"":""{2}"",""expires"":{3}}}", sessionkey, Uid, session.UserName, session.Expires);
            else
                returnStr = SerializationHelper.Serialize(session);

            OnlineUsers.UpdateAction(olid, UserAction.Login.ActionID, 0, GeneralConfigs.GetConfig().Onlinetimeout);

            return returnStr;
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <returns></returns>
        public string Register()
        {
            string returnStr = string.Empty;

            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return returnStr;
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return returnStr;
            }

            if (!CheckRequiredParams("user_name,password,email"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return returnStr;
            }

            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)//如果是桌面程序则不允许此方法
            {
                if (Uid < 1 || Discuz.Forum.UserGroups.GetUserGroupInfo(Discuz.Forum.Users.GetShortUserInfo(Uid).Groupid).Radminid != 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_PERMISSION_DENIED;
                    return "";
                }
            }
            else if (Uid > 0)//已经登录的用户不能再注册
            {
                ErrorCode = (int)ErrorType.API_EC_USER_ONLINE;
                return returnStr;
            }

            string username = GetParam("user_name").ToString();
            string password = GetParam("password").ToString();
            string email = GetParam("email").ToString();

            bool isMD5Passwd = GetParam("password_format") != null && GetParam("password_format").ToString() == "md5" ? true : false;

            //用户名不符合规范
            if (!CheckUsername(username))
            {
                ErrorCode = (int)ErrorType.API_EC_USERNAME_ILLEGAL;
                return returnStr;
            }

            if (Discuz.Forum.Users.GetUserId(username) != 0)//如果用户名符合注册规则, 则判断是否已存在
            {
                ErrorCode = (int)ErrorType.API_EC_USER_ALREADY_EXIST;
                return returnStr;
            }

            if (!isMD5Passwd && password.Length < 6)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return returnStr;
            }

            if (!CheckEmail(email))
            {
                ErrorCode = (int)ErrorType.API_EC_EMAIL;
                return returnStr;
            }

            UserInfo userInfo = new UserInfo();
            userInfo.Username = username;
            userInfo.Nickname = string.Empty;
            userInfo.Password = isMD5Passwd ? password : Utils.MD5(password);
            userInfo.Secques = string.Empty;
            userInfo.Gender = 0;
            userInfo.Adminid = 0;
            userInfo.Groupexpiry = 0;
            userInfo.Extgroupids = "";
            userInfo.Regip = DNTRequest.GetIP();
            userInfo.Joindate = Utils.GetDateTime();
            userInfo.Lastip = DNTRequest.GetIP();
            userInfo.Lastvisit = Utils.GetDateTime();
            userInfo.Lastactivity = Utils.GetDateTime();
            userInfo.Lastpost = Utils.GetDateTime();
            userInfo.Lastpostid = 0;
            userInfo.Lastposttitle = "";
            userInfo.Posts = 0;
            userInfo.Digestposts = 0;
            userInfo.Oltime = 0;
            userInfo.Pageviews = 0;
            userInfo.Credits = 0;
            userInfo.Extcredits1 = Scoresets.GetScoreSet(1).Init;
            userInfo.Extcredits2 = Scoresets.GetScoreSet(2).Init;
            userInfo.Extcredits3 = Scoresets.GetScoreSet(3).Init;
            userInfo.Extcredits4 = Scoresets.GetScoreSet(4).Init;
            userInfo.Extcredits5 = Scoresets.GetScoreSet(5).Init;
            userInfo.Extcredits6 = Scoresets.GetScoreSet(6).Init;
            userInfo.Extcredits7 = Scoresets.GetScoreSet(7).Init;
            userInfo.Extcredits8 = Scoresets.GetScoreSet(8).Init;
            userInfo.Email = email;
            userInfo.Bday = string.Empty;
            userInfo.Sigstatus = 0;

            userInfo.Tpp = 0;
            userInfo.Ppp = 0;
            userInfo.Templateid = 0;
            userInfo.Pmsound = 0;
            userInfo.Showemail = 0;
            userInfo.Salt = "0";
            int receivepmsetting = Config.Regadvance == 0 ? 7 : 1;
            userInfo.Newsletter = (ReceivePMSettingType)receivepmsetting;
            userInfo.Invisible = 0;
            userInfo.Newpm = Config.Welcomemsg == 1 ? 1 : 0;
            userInfo.Medals = "";
            userInfo.Accessmasks = 0;
            userInfo.Website = string.Empty;
            userInfo.Icq = string.Empty;
            userInfo.Qq = string.Empty;
            userInfo.Yahoo = string.Empty;
            userInfo.Msn = string.Empty;
            userInfo.Skype = string.Empty;
            userInfo.Location = string.Empty;
            userInfo.Customstatus = string.Empty;
            userInfo.Bio = string.Empty;
            userInfo.Signature = string.Empty;
            userInfo.Sightml = string.Empty;
            userInfo.Authtime = Utils.GetDateTime();

            //邮箱激活链接验证
            if (Config.Regverify == 1)
            {
                userInfo.Authstr = ForumUtils.CreateAuthStr(20);
                userInfo.Authflag = 1;
                userInfo.Groupid = 8;
                Emails.DiscuzSmtpMail(username, email, string.Empty, userInfo.Authstr);
            }
            //系统管理员进行后台验证
            else if (Config.Regverify == 2)
            {
                userInfo.Authstr = string.Empty;
                userInfo.Groupid = 8;
                userInfo.Authflag = 1;
            }
            else
            {
                userInfo.Authstr = "";
                userInfo.Authflag = 0;
                userInfo.Groupid = UserCredits.GetCreditsUserGroupId(0).Groupid;
            }
            userInfo.Realname = string.Empty;
            userInfo.Idcard = string.Empty;
            userInfo.Mobile = string.Empty;
            userInfo.Phone = string.Empty;

            if (Config.Passwordmode > 1 && PasswordModeProvider.GetInstance() != null)
            {
                userInfo.Uid = PasswordModeProvider.GetInstance().CreateUserInfo(userInfo);
            }
            else
            {
                userInfo.Uid = Discuz.Forum.Users.CreateUser(userInfo);
            }

            if (Config.Welcomemsg == 1)
            {
                PrivateMessageInfo privatemessageinfo = new PrivateMessageInfo();
                // 收件箱
                privatemessageinfo.Message = Config.Welcomemsgtxt;
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

            Statistics.ReSetStatisticsCache();

            //信息同步通知不会发向当前请求接口的应用程序，所以此处应保留，以支持论坛向其他关联应用程序发送通知
            Sync.UserRegister(userInfo.Uid, userInfo.Username, userInfo.Password, ApiKey);

            UserCredits.UpdateUserCredits(userInfo.Uid);

            if (Format == FormatType.JSON)
                return string.Format("\"{0}\"", userInfo.Uid);

            RegisterResponse rr = new RegisterResponse();
            rr.Uid = userInfo.Uid;

            return SerializationHelper.Serialize(rr);
        }

        /// <summary>
        /// 加密密码
        /// </summary>
        /// <returns></returns>
        public string EncodePassword()
        {
            string returnStr = string.Empty;

            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return returnStr;
            }

            //桌面程序不允许使用此方法
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                ErrorCode = (int)ErrorType.API_EC_PERMISSION_DENIED;
                return returnStr;
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return returnStr;
            }

            if (!CheckRequiredParams("password"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return returnStr;
            }

            string password = GetParam("password").ToString();
            bool isMD5Passwd = GetParam("password_format") != null && GetParam("password_format").ToString() == "md5" ? true : false;

            EncodePasswordResponse epr = new EncodePasswordResponse();
            epr.Password = Utils.UrlEncode(ForumUtils.SetCookiePassword(isMD5Passwd ? password : Utils.MD5(password), Config.Passwordkey));

            if (Format == FormatType.JSON)
                return string.Format("\"{0}\"", epr.Password);

            return SerializationHelper.Serialize(epr);
        }

        /// <summary>
        /// 验证用户 // Edit By Cyrano
        /// </summary>
        /// <returns></returns>
        public string Validate()
        {
            string returnStr = string.Empty;

            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return returnStr;
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return returnStr;
            }

            if (!CheckRequiredParams("user_name,password"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return returnStr;
            }

            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)//如果是桌面程序则不允许此方法
            {
                if (Uid < 1 || Discuz.Forum.UserGroups.GetUserGroupInfo(Discuz.Forum.Users.GetShortUserInfo(Uid).Groupid).Radminid != 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_PERMISSION_DENIED;
                    return "";
                }
            }
            else if (Uid > 0)//已经登录的用户不能再验证
            {
                ErrorCode = (int)ErrorType.API_EC_USER_ONLINE;
                return returnStr;
            }

            string username = GetParam("user_name").ToString();
            string password = GetParam("password").ToString();

            bool isMD5Passwd = GetParam("password_format") != null && GetParam("password_format").ToString() == "md5" ? true : false;

            //用户名不符合规范
            //if (!CheckUsername(username))
            //{
            //    ErrorCode = (int)ErrorType.API_EC_USERNAME_ILLEGAL;
            //    return returnStr;
            //}

            if (Discuz.Forum.Users.GetUserId(username) == 0)//如果用户名符合注册规则, 则判断是否已存在
            {
                ErrorCode = (int)ErrorType.API_EC_USER_NONEXIST;
                return returnStr;
            }

            if (!isMD5Passwd && password.Length < 6)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return returnStr;
            }

            ShortUserInfo userInfo = new ShortUserInfo();
            var postpassword =  password;
            var postusername = username;
            int uid = -1;

            uid = Discuz.Forum.Users.CheckPassword(postusername, postpassword, !isMD5Passwd);
            userInfo = uid > 0 ? Discuz.Forum.Users.GetShortUserInfo(uid) : null;

            if (userInfo != null)
            {
                #region 当前用户所在用户组为"禁止访问"或"等待激活"时

                if (userInfo.Groupid == 5 || userInfo.Groupid == 8)// 5-禁止访问, 8-等待激活
                {
                    ErrorCode = (int)ErrorType.API_EC_USERNAME_ILLEGAL;
                    return returnStr;
                }

                #endregion
            }
            else
            {
                int errcount = LoginLogs.UpdateLoginLog(DNTRequest.GetIP(), true);

                if (errcount > 5)
                {
                    //AddErrLine("您已经输入密码5次错误, 请15分钟后再试");
                }
                else
                {
                    //AddErrLine(string.Format("密码或安全提问第{0}次错误, 您最多有5次机会重试", errcount));
                }
            }

            //ForumUtils.WriteUserCreditsCookie(userInfo, usergroupinfo.Grouptitle);

            if (Format == FormatType.JSON)
                return string.Format("\"{0}\"", userInfo.Uid);

            ValidateResponse vr = new ValidateResponse();
            vr.Uid = userInfo.Uid;

            return SerializationHelper.Serialize(vr);
        }

        #region Helper
        private bool CheckUsername(string username)
        {
            if (username.Equals(""))
            {
                return false;
            }
            if (username.Length > 20)
            {
                //如果用户名超过20...
                return false;
            }
            if (Utils.GetStringLength(username) < 3)
            {
                return false;
            }
            if (username.IndexOf(" ") != -1)
            {
                //如果用户名符合注册规则, 则判断是否已存在
                return false;
            }
            if (username.IndexOf("　") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1)
            {
                //如果用户名符合注册规则, 则判断是否已存在                
                return false;
            }
            if (username.IndexOf(":") != -1)
            {
                //如果用户名符合注册规则, 则判断是否已存在
                return false;
            }
            if ((!Utils.IsSafeSqlString(username)) || (!Utils.IsSafeUserInfoString(username)))
            {
                return false;
            }
            // 如果用户名属于禁止名单, 或者与负责发送新用户注册欢迎信件的用户名称相同...
            if (username.Trim() == PrivateMessages.SystemUserName || ForumUtils.IsBanUsername(username, Config.Censoruser))
            {
                return false;
            }
            return true;

        }

        private bool CheckEmail(string email)
        {
            if (!Utils.IsValidEmail(email))
            {
                //AddErrLine("Email格式不正确");
                return false;
            }
            if (!Discuz.Forum.Users.ValidateEmail(email))
            {
                //AddErrLine("Email: \"" + email + "\" 已经被其它用户注册使用");
                return false;
            }

            string emailhost = Utils.GetEmailHostName(email);
            // 允许名单规则优先于禁止名单规则
            if (Config.Accessemail.Trim() != "" && !Utils.InArray(emailhost, Config.Accessemail.Replace("\r\n", "\n"), "\n"))
            {
                //AddErrLine("Email: \"" + email + "\" 不在本论坛允许范围之类, 本论坛只允许用户使用这些Email地址注册: " + config.Accessemail.Replace("\n", ",").Replace("\r", ""));
                return false;
            }
            if (Config.Censoremail.Trim() != "" && Utils.InArray(email, Config.Censoremail.Replace("\r\n", "\n"), "\n"))
            {
                //AddErrLine("Email: \"" + email + "\" 不允许在本论坛使用, 本论坛不允许用户使用的Email地址包括: " + config.Censoremail.Replace("\n", ",").Replace("\r", ""));
                return false;
            }
            return true;
        }

        #endregion
    }
}
