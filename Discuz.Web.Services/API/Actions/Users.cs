using Discuz.Common;
using Discuz.Entity;
using Discuz.Forum;
using Newtonsoft.Json;
using Discuz.Common.Generic;

namespace Discuz.Web.Services.API.Actions
{
    public class Users : ActionBase
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public string GetInfo()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            if (!CheckRequiredParams("uids,fields"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            if (!Utils.IsNumericArray(GetParam("uids").ToString().Split(',')))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            string[] uids = GetParam("uids").ToString().Split(',');

            if (Utils.StrToInt(uids[0], -1) < 1)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            List<string> fieldlist = new List<string>(GetParam("fields").ToString().Split(','));

            List<User> userlist = new List<User>();
            UserInfo userInfo;
            for (int i = 0; i < uids.Length; i++)
            {
                int userid = Utils.StrToInt(uids[i], -1);
                if (userid < 1)
                    continue;
                userInfo = Discuz.Forum.Users.GetUserInfo(userid);
                if (userInfo == null)
                    continue;

                User user = new User();

                user = LoadSingleUser(userInfo);

                userlist.Add(user);
            }

            UserInfoResponse uir = new UserInfoResponse();
            uir.user_array = userlist.ToArray();
            uir.List = true;

            if (Format == FormatType.JSON)
            {
                return Util.RemoveJsonNull(JavaScriptConvert.SerializeObject(userlist.ToArray()));
            }
            if (userlist.Count < 1)
            {
                return SerializationHelper.Serialize(uir);
            }
            return Util.RemoveEmptyNodes(SerializationHelper.Serialize(uir), GetParam("fields").ToString());
        }

        /// <summary>
        /// 获得当前登录用户
        /// </summary>
        /// <returns></returns>
        public string GetLoggedInUser()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }
            }

            //if (Uid < 1)
            //{
            //    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
            //    return "";
            //}
            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }


            if (Format == FormatType.JSON)
                return string.Format("\"{0}\"", Uid);

            LoggedInUserResponse loggeduser = new LoggedInUserResponse();
            //loggeduser.List = true;
            loggeduser.Uid = Uid;

            return SerializationHelper.Serialize(loggeduser);
        }

        /// <summary>
        /// 设置用户资料
        /// </summary>
        /// <returns></returns>
        public string SetInfo()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            if (!this.CheckRequiredParams("uid,user_info"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            int uid = Utils.StrToInt(GetParam("uid").ToString(), 0);

            if (uid <= 0)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            UserForEditing ufe;

            try
            {
                ufe = JavaScriptConvert.DeserializeObject<UserForEditing>(GetParam("user_info").ToString());
            }
            catch
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }


            UserInfo userinfo = Discuz.Forum.Users.GetUserInfo(uid);

            if (!string.IsNullOrEmpty(ufe.Password))
            {
                userinfo.Password = ufe.Password;
            }

            if (ufe.Bio != null)
            {
                userinfo.Bio = ufe.Bio;
            }

            if (ufe.Birthday != null)
            {
                userinfo.Bday = ufe.Birthday;
            }

            if (!string.IsNullOrEmpty(ufe.Email) && userinfo.Email != ufe.Email && CheckEmail(ufe.Email))
            {
                userinfo.Email = ufe.Email;
            }

            if (!string.IsNullOrEmpty(ufe.ExtCredits1))
            {
                userinfo.Extcredits1 = Utils.StrToFloat(ufe.ExtCredits1, 0);
            }

            if (!string.IsNullOrEmpty(ufe.ExtCredits2))
            {
                userinfo.Extcredits2 = Utils.StrToFloat(ufe.ExtCredits2, 0);
            }

            if (!string.IsNullOrEmpty(ufe.ExtCredits3))
            {
                userinfo.Extcredits3 = Utils.StrToFloat(ufe.ExtCredits3, 0);
            }

            if (!string.IsNullOrEmpty(ufe.ExtCredits4))
            {
                userinfo.Extcredits4 = Utils.StrToFloat(ufe.ExtCredits4, 0);
            }

            if (!string.IsNullOrEmpty(ufe.ExtCredits5))
            {
                userinfo.Extcredits5 = Utils.StrToFloat(ufe.ExtCredits5, 0);
            }

            if (!string.IsNullOrEmpty(ufe.ExtCredits6))
            {
                userinfo.Extcredits6 = Utils.StrToFloat(ufe.ExtCredits6, 0);
            }

            if (!string.IsNullOrEmpty(ufe.ExtCredits7))
            {
                userinfo.Extcredits7 = Utils.StrToFloat(ufe.ExtCredits7, 0);
            }

            if (!string.IsNullOrEmpty(ufe.ExtCredits8))
            {
                userinfo.Extcredits8 = Utils.StrToFloat(ufe.ExtCredits8, 0);
            }

            if (ufe.Gender != null)
            {
                userinfo.Gender = Utils.StrToInt(ufe.Gender, 0);
            }

            if (ufe.Icq != null)
            {
                userinfo.Icq = ufe.Icq;
            }

            if (ufe.IdCard != null)
            {
                userinfo.Idcard = ufe.IdCard;
            }

            if (ufe.Location != null)
            {
                userinfo.Location = ufe.Location;
            }

            if (ufe.Mobile != null)
            {
                userinfo.Mobile = ufe.Mobile;
            }

            if (ufe.Msn != null)
            {
                userinfo.Msn = ufe.Msn;
            }

            if (ufe.NickName != null)
            {
                userinfo.Nickname = ufe.NickName;
            }

            if (ufe.Phone != null)
            {
                userinfo.Phone = ufe.Phone;
            }

            if (ufe.Qq != null)
            {
                userinfo.Qq = ufe.Qq;
            }

            if (ufe.RealName != null)
            {
                userinfo.Realname = ufe.RealName;
            }

            if (ufe.Skype != null)
            {
                userinfo.Skype = ufe.Skype;
            }

            if (!string.IsNullOrEmpty(ufe.SpaceId))
            {
                userinfo.Spaceid = Utils.StrToInt(ufe.SpaceId, 0);
            }

            if (ufe.WebSite != null)
            {
                userinfo.Website = ufe.WebSite;
            }

            if (ufe.Yahoo != null)
            {
                userinfo.Yahoo = ufe.Yahoo;
            }

            try
            {
                Discuz.Forum.Users.UpdateUser(userinfo);
            }
            catch
            {
                ErrorCode = (int)ErrorType.API_EC_UNKNOWN;
                return "0";
            }

            if (Format == FormatType.JSON)
            {
                return "true";
            }

            SetInfoResponse sir = new SetInfoResponse();

            sir.Successfull = 1;

            return SerializationHelper.Serialize(sir);
        }

        public string SetExtCredits()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }

                if (Discuz.Forum.Users.GetShortUserInfo(Uid).Adminid != 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_PERMISSION_DENIED;
                    return "";
                }
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            if (!this.CheckRequiredParams("uids,additional_values"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            if (!Utils.IsNumericArray(GetParam("additional_values").ToString().Split(',')) || !Utils.IsNumericArray(GetParam("uids").ToString().Split(',')))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            string[] values = GetParam("additional_values").ToString().Split(',');
            if (values.Length != 8)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            List<float> list = new List<float>();
            for (int i = 0; i < values.Length; i++)
            {
                list.Add(Utils.StrToFloat(values[i], 0));
            }

            foreach (string uId in GetParam("uids").ToString().Split(','))
            {
                int id = TypeConverter.StrToInt(uId);
                if (id == 0)
                    continue;
                UserCredits.UpdateUserExtCredits(id, list.ToArray(), true);
                UserCredits.UpdateUserCredits(id);

                //向第三方应用同步积分
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] != 0.0)
                    {
                        Sync.UpdateCredits(TypeConverter.StrToInt(uId), i + 1, list[i].ToString(), ApiKey);
                    }
                }
            }

            int successful = 1;

            if (Format == FormatType.JSON)
            {
                return successful == 1 ? "true" : "false";
            }

            SetExtCreditsResponse secr = new SetExtCreditsResponse();

            secr.Successfull = successful;

            return SerializationHelper.Serialize(secr);
        }

        /// <summary>
        /// 根据用户名获得用户ID
        /// </summary>
        /// <returns></returns>
        public string GetID()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            if (!this.CheckRequiredParams("user_name"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            int uid = Discuz.Forum.Users.GetUserId(GetParam("user_name").ToString());
            if (Format == FormatType.JSON)
                return string.Format("\"{0}\"", uid);

            GetIDResponse gir = new GetIDResponse();
            gir.UId = uid;

            return SerializationHelper.Serialize(gir);
        }

        public string ChangePassword()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            int uid = TypeConverter.ObjectToInt(GetParam("uid"));

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }

                if (Uid != uid)
                {
                    ErrorCode = (int)ErrorType.API_EC_PERMISSION_DENIED;
                    return "";
                }
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            if (!this.CheckRequiredParams("uid,original_password,new_password,confirm_new_password"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            string originalPassword = GetParam("original_password").ToString();
            string newPassword = GetParam("new_password").ToString();
            string confirmNewPassword = GetParam("confirm_new_password").ToString();

            if (newPassword != confirmNewPassword)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return string.Empty;
            }

            bool isMD5Passwd = GetParam("password_format") != null && GetParam("password_format").ToString() == "md5";

            ShortUserInfo user = Discuz.Forum.Users.GetShortUserInfo(uid);

            if (!isMD5Passwd)
            {
                originalPassword = Utils.MD5(originalPassword);
            }
            if (user.Password != originalPassword)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return string.Empty;
            }

            bool result = Discuz.Forum.Users.UpdateUserPassword(uid, newPassword, !isMD5Passwd);
            ChangePasswordResponse cpr = new ChangePasswordResponse();
            cpr.Successfull = result ? 1 : 0;
            if (Format == FormatType.JSON)
                return string.Format("\"{0}\"", result.ToString().ToLower());

            return SerializationHelper.Serialize(cpr);
        }

        public string GetInfoByEmail()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            if (!this.CheckRequiredParams("email"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            List<UserInfo> userList = new List<UserInfo>();
            List<User> userListResult = new List<User>();

            userList = Discuz.Forum.Users.GetUserListByEmail(GetParam("email").ToString().Trim());

            foreach (UserInfo userInfo in userList)
            {
                userListResult.Add(LoadSingleUser(userInfo));
            }

            UserInfoResponse uir = new UserInfoResponse();
            uir.user_array = userListResult.ToArray();
            uir.List = true;

            if (Format == FormatType.JSON)
            {
                return Util.RemoveJsonNull(JavaScriptConvert.SerializeObject(userListResult.ToArray()));
            }

            if (userListResult.Count < 1)
            {
                return SerializationHelper.Serialize(uir);
            }
            return Util.RemoveEmptyNodes(SerializationHelper.Serialize(uir), GetParam("fields").ToString());
        }

        #region private methods

        private User LoadSingleUser(UserInfo userInfo)
        {
            List<string> fieldlist = new List<string>(GetParam("fields").ToString().Split(','));

            User user = new User();

            user.AccessMasks = fieldlist.Contains("access_masks") ? (int?)userInfo.Accessmasks : null;

            user.AdminId = fieldlist.Contains("admin_id") ? (int?)userInfo.Adminid : null;

            user.Birthday = fieldlist.Contains("birthday") ? userInfo.Bday.Trim() : null;

            user.Credits = fieldlist.Contains("credits") ? (int?)userInfo.Credits : null;

            user.DigestPosts = fieldlist.Contains("digest_post_count") ? (int?)userInfo.Digestposts : null;

            user.Email = fieldlist.Contains("email") ? userInfo.Email.Trim() : null;

            user.ExtCredits1 = fieldlist.Contains("ext_credits_1") ? (int?)userInfo.Extcredits1 : null;

            user.ExtCredits2 = fieldlist.Contains("ext_credits_2") ? (int?)userInfo.Extcredits2 : null;

            user.ExtCredits3 = fieldlist.Contains("ext_credits_3") ? (int?)userInfo.Extcredits3 : null;

            user.ExtCredits4 = fieldlist.Contains("ext_credits_4") ? (int?)userInfo.Extcredits4 : null;

            user.ExtCredits5 = fieldlist.Contains("ext_credits_5") ? (int?)userInfo.Extcredits5 : null;

            user.ExtCredits6 = fieldlist.Contains("ext_credits_6") ? (int?)userInfo.Extcredits6 : null;

            user.ExtCredits7 = fieldlist.Contains("ext_credits_7") ? (int?)userInfo.Extcredits7 : null;

            user.ExtCredits8 = fieldlist.Contains("ext_credits_8") ? (int?)userInfo.Extcredits8 : null;

            user.ExtGroupids = fieldlist.Contains("ext_groupids") ? userInfo.Extgroupids.Trim() : null;

            user.Gender = fieldlist.Contains("gender") ? (int?)userInfo.Gender : null;

            user.GroupExpiry = fieldlist.Contains("group_expiry") ? (int?)userInfo.Groupexpiry : null;

            user.GroupId = fieldlist.Contains("group_id") ? (int?)userInfo.Groupid : null;

            user.Invisible = fieldlist.Contains("invisible") ? (int?)userInfo.Invisible : null;

            user.JoinDate = fieldlist.Contains("join_date") ? userInfo.Joindate : null;

            user.LastActivity = fieldlist.Contains("last_activity") ? userInfo.Lastactivity : null;

            user.LastIp = fieldlist.Contains("last_ip") ? userInfo.Lastip.Trim() : null;

            user.LastPost = fieldlist.Contains("last_post") ? userInfo.Lastpost : null;

            user.LastPostid = fieldlist.Contains("last_post_id") ? (int?)userInfo.Lastpostid : null;

            user.LastPostTitle = fieldlist.Contains("last_post_title") ? userInfo.Lastposttitle : null;

            user.LastVisit = fieldlist.Contains("last_visit") ? userInfo.Lastvisit : null;

            user.NewPm = fieldlist.Contains("has_new_pm") ? (int?)userInfo.Newpm : null;

            user.NewPmCount = fieldlist.Contains("new_pm_count") ? (int?)userInfo.Newpmcount : null;

            user.NickName = fieldlist.Contains("nick_name") ? userInfo.Nickname : null;

            user.OnlineState = fieldlist.Contains("online_state") ? (int?)userInfo.Onlinestate : null;

            user.OnlineTime = fieldlist.Contains("online_time") ? (int?)userInfo.Oltime : null;

            user.PageViews = fieldlist.Contains("page_view_count") ? (int?)userInfo.Pageviews : null;

            user.Password = fieldlist.Contains("password") ? userInfo.Password : null;

            user.PmSound = fieldlist.Contains("pm_sound") ? (int?)userInfo.Pmsound : null;

            user.Posts = fieldlist.Contains("post_count") ? (int?)userInfo.Posts : null;

            user.Ppp = fieldlist.Contains("ppp") ? (int?)userInfo.Ppp : null;

            user.RegIp = fieldlist.Contains("reg_ip") ? userInfo.Regip : null;

            user.Secques = fieldlist.Contains("secques") ? userInfo.Secques : null;

            user.ShowEmail = fieldlist.Contains("show_email") ? (int?)userInfo.Showemail : null;

            user.SpaceId = fieldlist.Contains("space_id") ? (int?)userInfo.Spaceid : null;

            user.Templateid = fieldlist.Contains("template_id") ? (int?)userInfo.Templateid : null;

            user.Tpp = fieldlist.Contains("tpp") ? (int?)userInfo.Tpp : null;

            user.Uid = fieldlist.Contains("uid") ? (int?)userInfo.Uid : null;

            user.UserName = fieldlist.Contains("user_name") ? userInfo.Username : null;

            user.CustomStatus = fieldlist.Contains("custom_status") ? userInfo.Customstatus : null;	//自定义头衔

            user.Avatar = fieldlist.Contains("avatar") ? Avatars.GetAvatarUrl(userInfo.Uid).TrimStart('/') : null;

            user.Medals = fieldlist.Contains("medals") ? userInfo.Medals : null; //勋章列表

            user.WebSite = fieldlist.Contains("web_site") ? userInfo.Website : null;	//网站

            user.Icq = fieldlist.Contains("icq") ? userInfo.Icq : null;	//icq号码

            user.Qq = fieldlist.Contains("qq") ? userInfo.Qq : null;	//qq号码

            user.Yahoo = fieldlist.Contains("yahoo") ? userInfo.Yahoo : null;//yahoo messenger帐号

            user.Msn = fieldlist.Contains("msn") ? userInfo.Msn : null;	//msn messenger帐号

            user.Skype = fieldlist.Contains("skype") ? userInfo.Skype : null;	//skype帐号

            user.Location = fieldlist.Contains("location") ? userInfo.Location : null;	//来自

            user.Bio = fieldlist.Contains("about_me") ? userInfo.Bio : null;	//自我介绍

            user.Sightml = fieldlist.Contains("signhtml") ? userInfo.Sightml : null;	//签名Html(自动转换得到)

            user.RealName = fieldlist.Contains("real_name") ? userInfo.Realname : null;  //用户实名

            user.IdCard = fieldlist.Contains("id_card") ? userInfo.Idcard : null;    //用户身份证件号

            user.Mobile = fieldlist.Contains("mobile") ? userInfo.Mobile : null;    //用户移动电话

            user.Phone = fieldlist.Contains("telephone") ? userInfo.Phone : null;     //用户固定电话

            return user;
        }

        /// <summary>
        /// 检测用户邮箱是否合法
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool CheckEmail(string email)
        {
            if (!Utils.IsValidEmail(email))
            {
                return false;
            }
            if (!Discuz.Forum.Users.ValidateEmail(email))
            {
                return false;
            }

            string emailhost = Utils.GetEmailHostName(email);
            // 允许名单规则优先于禁止名单规则
            if (Config.Accessemail.Trim() != "" && !Utils.InArray(emailhost, Config.Accessemail.Replace("\r\n", "\n"), "\n"))
            {
                return false;
            }
            if (Config.Censoremail.Trim() != "" && Utils.InArray(email, Config.Censoremail.Replace("\r\n", "\n"), "\n"))
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
