using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Cache;
using Discuz.Plugin.PasswordMode;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    /// <summary>
    /// 用户操作类
    /// </summary>
    public class Users
    {
        /// <summary>
        /// 返回相应用户扩展积分字段信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="extId"></param>
        /// <returns></returns>
        public static float GetUserExtCredit(UserInfo userInfo, int extId)
        {
            if (userInfo == null)
                return 0;
            switch (extId)
            {
                case 1: return userInfo.Extcredits1;
                case 2: return userInfo.Extcredits2;
                case 3: return userInfo.Extcredits3;
                case 4: return userInfo.Extcredits4;
                case 5: return userInfo.Extcredits5;
                case 6: return userInfo.Extcredits6;
                case 7: return userInfo.Extcredits7;
                case 8: return userInfo.Extcredits8;
                default: return 0;
            }
        }

        /// <summary>
        /// 返回指定用户的完整信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>用户信息</returns>
        public static UserInfo GetUserInfo(int uid)
        {
            return Discuz.Data.Users.GetUserInfo(uid);
        }

        /// <summary>
        /// 返回指定用户的简短信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>用户信息</returns>
        public static ShortUserInfo GetShortUserInfo(int uid)
        {
            return Discuz.Data.Users.GetShortUserInfo(uid);
        }

        /// <summary>
        /// 根据IP查找用户
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>用户信息</returns>
        public static string CheckRegisterDateDiff(string ip)
        {
            ShortUserInfo userinfo = Discuz.Data.Users.GetShortUserInfoByIP(ip);

            if (GeneralConfigs.GetConfig().Regctrl > 0 && userinfo != null)
            {
                int Interval = Utils.StrDateDiffHours(userinfo.Joindate, GeneralConfigs.GetConfig().Regctrl);
                if (Interval <= 0)
                    return "抱歉, 系统设置了IP注册间隔限制, 您必须在 " + (Interval * -1) + " 小时后才可以注册";
            }

            if (GeneralConfigs.GetConfig().Ipregctrl.Trim() != "" && Utils.InIPArray(DNTRequest.GetIP(), Utils.SplitString(GeneralConfigs.GetConfig().Ipregctrl, "\n")) && userinfo != null)
            {
                int Interval = Utils.StrDateDiffHours(userinfo.Joindate, 72);
                if (Interval < 0)
                    return "抱歉, 系统设置了特殊IP注册限制, 您必须在 " + (Interval * -1) + " 小时后才可以注册";
            }
            return null;
        }


        /// <summary>
        /// 根据用户名返回用户id
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>用户id</returns>
        public static int GetUserId(string username)
        {
            ShortUserInfo userInfo = Discuz.Data.Users.GetShortUserInfoByName(username);
            return (userInfo != null) ? userInfo.Uid : 0;
        }

        /// <summary>
        /// 获得用户列表DataTable
        /// </summary>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="pageindex">当前页数</param>
        /// <returns>用户列表DataTable</returns>
        public static DataTable GetUserList(int pagesize, int pageindex, string column, string ordertype)
        {
            DataTable dt = Discuz.Data.Users.GetUserList(pagesize, pageindex, column, ordertype);
            dt.Columns.Add("grouptitle");
            dt.Columns.Add("olimg");
            foreach (DataRow dataRow in dt.Rows)
            {
                UserGroupInfo group = UserGroups.GetUserGroupInfo(Utils.StrToInt(dataRow["groupid"], 0));

                if (Utils.StrIsNullOrEmpty(group.Color))
                    dataRow["grouptitle"] = group.Grouptitle;
                else
                    dataRow["grouptitle"] = string.Format("<font color='{1}'>{0}</font>", group.Grouptitle, group.Color);

                dataRow["olimg"] = OnlineUsers.GetGroupImg(Utils.StrToInt(dataRow["groupid"], 0));
            }
            return dt;
        }

        /// <summary>
        /// 获取专门用于WebService的用户列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUserListOnService(int pagesize, int pageindex, string column, string ordertype)
        {
            return Discuz.Data.Users.GetUserList(pagesize, pageindex, column, ordertype);
        }


        /// <summary>
        /// 检测Email和安全项
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="email">email</param>
        /// <param name="questionid">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public static bool CheckEmailAndSecques(string username, string email, int questionid, string answer, string forumPath)
        {
            int uid = Discuz.Data.Users.CheckEmailAndSecques(username, email, ForumUtils.GetUserSecques(questionid, answer));
            if (uid != -1)
            {
                string Authstr = ForumUtils.CreateAuthStr(20);
                Users.UpdateAuthStr(uid, Authstr, 2);

                StringBuilder body = new StringBuilder(username);
                body.AppendFormat("您好!<br />这封信是由 {0}", GeneralConfigs.GetConfig().Forumtitle);
                body.Append(" 发送的.<br /><br />您收到这封邮件,是因为在我们的论坛上这个邮箱地址被登记为用户邮箱,且该用户请求使用 Email 密码重置功能所致.");
                body.Append("<br /><br />----------------------------------------------------------------------");
                body.Append("<br />重要！");
                body.Append("<br /><br />----------------------------------------------------------------------");
                body.Append("<br /><br />如果您没有提交密码重置的请求或不是我们论坛的注册用户,请立即忽略并删除这封邮件.只在您确认需要重置密码的情况下,才继续阅读下面的内容.");
                body.Append("<br /><br />----------------------------------------------------------------------");
                body.Append("<br />密码重置说明");
                body.Append("<br /><br />----------------------------------------------------------------------");
                body.Append("<br /><br />您只需在提交请求后的三天之内,通过点击下面的链接重置您的密码:<br /><br />");
                body.AppendFormat("<a href={0}/setnewpassword.aspx?uid={1}&id={2} target=_blank>{0}", forumPath, uid, Authstr);
                body.AppendFormat("/setnewpassword.aspx?uid={0}&id={1}</a>", uid, Authstr);
                body.Append("<br /><br />(如果上面不是链接形式,请将地址手工粘贴到浏览器地址栏再访问)");
                body.Append("<br /><br />上面的页面打开后,输入新的密码后提交,之后您即可使用新的密码登录论坛了.您可以在用户控制面板中随时修改您的密码.");
                body.AppendFormat("<br /><br />本请求提交者的 IP 为 {0}<br /><br /><br /><br />", DNTRequest.GetIP());
                body.AppendFormat("<br />此致 <br /><br />{0} 管理团队.<br />{1}<br /><br />", GeneralConfigs.GetConfig().Forumtitle, forumPath);

                Emails.DiscuzSmtpMailToUser(DNTRequest.GetString("email"), GeneralConfigs.GetConfig().Forumtitle + " 取回密码说明", body.ToString());
                return true;
            }
            return false;
        }


        /// <summary>
        /// 检测密码和安全项
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <param name="questionid">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public static int CheckPasswordAndSecques(string username, string password, bool originalpassword, int questionid, string answer)
        {
            return Discuz.Data.Users.CheckPasswordAndSecques(username, password, originalpassword, ForumUtils.GetUserSecques(questionid, answer));
        }

        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public static int CheckPassword(string username, string password, bool originalpassword)
        {
            ShortUserInfo userInfo = Discuz.Data.Users.CheckPassword(username, password, originalpassword);

            return userInfo == null ? -1 : userInfo.Uid;
        }



        /// <summary>
        /// 检测DVBBS兼容模式的密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="questionid">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public static int CheckDvBbsPasswordAndSecques(string username, string password, int questionid, string answer)
        {
            ShortUserInfo userInfo = Discuz.Data.Users.GetShortUserInfoByName(username);
            int uid = -1;
            if (userInfo != null)
            {
                if (questionid != -1 && !Utils.StrIsNullOrEmpty(answer) &&
                    userInfo.Secques.Trim() != ForumUtils.GetUserSecques(questionid, answer))
                    return -1;

                string pw = userInfo.Password.Trim();

                if (pw.Length > 16 && Utils.MD5(password) == pw)
                {
                    uid = userInfo.Uid;
                }
                else if (Utils.MD5(password).Substring(8, 16) == pw)
                {
                    uid = userInfo.Uid;
                    UpdateUserPassword(uid, password, true);
                }
            }
            return uid;
        }

        /// <summary>
        /// 检测DVBBS兼容模式的密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public static int CheckDvBbsPassword(string username, string password)
        {
            return CheckDvBbsPasswordAndSecques(username, password, -1, null);
        }


        /// <summary>
        /// 判断用户密码是否正确
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否为未MD5密码</param>
        /// <param name="groupid">用户组ID</param>
        /// <param name="adminid">管理组ID</param>
        /// <returns>如果正确则返回uid</returns>
        public static int CheckPassword(string username, string password, bool originalpassword, out int groupid, out int adminid)
        {
            ShortUserInfo userInfo = Discuz.Data.Users.CheckPassword(username, password, originalpassword);

            int uid = -1;
            groupid = 7;
            adminid = 0;

            if (userInfo != null)
            {
                uid = userInfo.Uid;
                groupid = userInfo.Groupid;
                adminid = userInfo.Adminid;
            }
            return uid;
        }

        /// <summary>
        /// 检测密码
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <param name="groupid">用户组id</param>
        /// <param name="adminid">管理id</param>
        /// <returns>如果用户密码正确则返回uid, 否则返回-1</returns>
        public static int CheckPassword(int uid, string password, bool originalpassword, out int groupid, out int adminid)
        {
            ShortUserInfo userInfo = Discuz.Data.Users.CheckPassword(uid, password, originalpassword);

            uid = -1;
            groupid = 7;
            adminid = 0;

            if (userInfo != null)
            {
                uid = userInfo.Uid;
                groupid = userInfo.Groupid;
                adminid = userInfo.Adminid;
            }
            return uid;
        }


        /// <summary>
        /// 判断指定用户密码是否正确.
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="password">用户密码</param>
        /// <returns>如果用户密码正确则返回true, 否则返回false</returns>
        public static int CheckPassword(int uid, string password, bool originalpassword)
        {
            ShortUserInfo userInfo = Discuz.Data.Users.CheckPassword(uid, password, originalpassword);

            return userInfo == null ? -1 : userInfo.Uid;
        }

        /// <summary>
        /// 根据指定的email查找用户并返回用户uid
        /// </summary>
        /// <param name="email">email地址</param>
        /// <returns>用户uid</returns>
        public static bool ValidateEmail(string email)
        {
            if (GeneralConfigs.GetConfig().Doublee == 0 && Discuz.Data.Users.FindUserEmail(email) != -1)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据指定的email查找用户并返回用户uid
        /// </summary>
        /// <param name="email">email地址</param>
        /// <returns>用户uid</returns>
        public static bool ValidateEmail(string email, int uid)
        {
            int userid = Discuz.Data.Users.FindUserEmail(email);
            if (GeneralConfigs.GetConfig().Doublee == 0 && userid != -1 && uid != userid)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 得到论坛中用户总数
        /// </summary>
        /// <returns>用户总数</returns>
        public static int GetUserCount(string condition)
        {
            return (condition == "") ? Discuz.Data.Users.GetUserCount() : Data.Users.GetUserCount(condition);
        }

        /// <summary>
        /// 得到论坛中用户总数
        /// </summary>
        /// <returns>用户总数</returns>
        public static int GetUserCountByAdmin(string orderby)
        {
            if (orderby == "admin")
                return Discuz.Data.Users.GetUserCountByAdmin();

            return Users.GetUserCount("");
        }

        /// <summary>
        /// 创建新用户.
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>返回用户ID, 如果已存在该用户名则返回-1</returns>
        public static int CreateUser(UserInfo userinfo)
        {
            if (GetUserId(userinfo.Username) > 0)
                return -1;

            return Discuz.Data.Users.CreateUser(userinfo);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="userinfo">用户信息</param>
        /// <returns>是否更新成功</returns>
        public static bool UpdateUser(UserInfo userinfo)
        {
            if (userinfo == null)
                return false;

            return Discuz.Data.Users.UpdateUser(userinfo);
        }

        /// <summary>
        /// 更新权限验证字符串
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="authstr">验证串</param>
        /// <param name="authflag">验证标志</param>
        public static void UpdateAuthStr(int uid, string authstr, int authflag)
        {
            Discuz.Data.Users.UpdateAuthStr(uid, authstr, authflag);
        }


        /// <summary>
        /// 更新指定用户的个人资料
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>如果用户不存在则为false, 否则为true</returns>
        public static bool UpdateUserProfile(UserInfo userinfo)
        {
            if (Discuz.Data.Users.GetShortUserInfo(userinfo.Uid) == null)
                return false;

            Discuz.Data.Users.UpdateUserProfile(userinfo);
            return true;
        }

        /// <summary>
        /// 更新用户论坛设置
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>如果用户不存在则返回false, 否则返回true</returns>
        public static bool UpdateUserForumSetting(UserInfo userinfo)
        {
            if (Discuz.Data.Users.GetShortUserInfo(userinfo.Uid) == null)
                return false;

            Discuz.Data.Users.UpdateUserForumSetting(userinfo);
            return true;
        }

        /// <summary>
        /// 修改用户自定义积分字段的值
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="extid">扩展字段序号(1-8)</param>
        /// <param name="pos">增加的数值(可以是负数)</param>
        /// <returns>执行是否成功</returns>
        public static void UpdateUserExtCredits(int uid, int extid, float pos)
        {
            Discuz.Data.Users.UpdateUserExtCredits(uid, extid, pos);
        }

        /// <summary>
        /// 获得指定用户的指定积分扩展字段的值
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="extid">扩展字段序号(1-8)</param>
        /// <returns>值</returns>
        public static float GetUserExtCredits(int uid, int extid)
        {
            return Discuz.Data.Users.GetUserExtCredits(uid, extid);
        }

        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="avatar">头像</param>
        /// <param name="avatarwidth">头像宽度</param>
        /// <param name="avatarheight">头像高度</param>
        /// <param name="templateid">模板Id</param>
        /// <returns>如果用户不存在则返回false, 否则返回true</returns>
        public static bool UpdateUserPreference(int uid, string avatar, int avatarwidth, int avatarheight, int templateid)
        {
            if (Discuz.Data.Users.GetShortUserInfo(uid) == null)
                return false;

            Discuz.Data.Users.UpdateUserPreference(uid, avatar, avatarwidth, avatarheight, templateid);
            return true;
        }


        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <returns>成功返回true否则false</returns>
        public static bool UpdateUserPassword(int uid, string password, bool originalpassword)
        {
            if (Discuz.Data.Users.GetShortUserInfo(uid) == null)
                return false;

            Discuz.Data.Users.UpdateUserPassword(uid, password, originalpassword);
            return true;
        }

        /// <summary>
        /// 更新用户安全问题
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="questionid">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns>成功返回true否则false</returns>
        public static bool UpdateUserSecques(int uid, int questionid, string answer)
        {
            if (Discuz.Data.Users.GetShortUserInfo(uid) == null)
                return false;

            Discuz.Data.Users.UpdateUserSecques(uid, ForumUtils.GetUserSecques(questionid, answer));
            return true;
        }


        /// <summary>
        /// 更新用户积分和最后登录时间
        /// </summary>
        /// <param name="uid">用户id</param>
        public static void UpdateUserCreditsAndVisit(int uid, string ip)
        {
            UserCredits.UpdateUserCredits(uid);
            Discuz.Data.Users.UpdateUserLastvisit(uid, ip);
        }


        /// <summary>
        /// 更新指定用户的勋章信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="medals">勋章信息</param>
        /// <param name="adminUid">授予人Id</param>
        /// <param name="adminUserName">授予人用户名</param>
        /// <param name="ip">IP</param>
        /// <param name="reason">理由</param>
        public static void UpdateMedals(int uid, string medals, int adminUid, string adminUserName, string ip, string reason)
        {
            if (uid <= 0)
                return;
            Discuz.Data.Users.UpdateMedals(uid, medals);
            string givenusername = Users.GetUserInfo(uid).Username;
            foreach (string medalid in medals.Split(','))
            {
                if (medalid != "")
                {
                    if (!Data.Medals.IsExistMedalAwardRecord(int.Parse(medalid), uid))
                    {
                        Data.Medals.CreateMedalslog(adminUid, adminUserName, ip, givenusername, uid, "授予", int.Parse(medalid), reason);
                    }
                    else
                    {
                        Data.Medals.UpdateMedalslog("授予", DateTime.Now, reason, "收回", int.Parse(medalid), uid);
                    }
                }
            }
        }



        /// <summary>
        /// 将用户的未读短信息数量减小一个指定的值
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="olid">在线用户id</param>
        /// <returns>更新记录个数</returns>
        public static int UpdateUserNewPMCount(int uid, int olid)
        {
            int newPMs = Discuz.Data.PrivateMessages.GetNewPMCount(uid);
            OnlineUsers.UpdateNewPms(olid, newPMs);
            return Discuz.Data.Users.SetUserNewPMCount(uid, newPMs);
        }


        /// <summary>
        /// 更新用户SpaceID
        /// </summary>
        /// <param name="spaceid">要更新的SpaceId</param>
        /// <param name="userid">要更新的UserId</param>
        /// <returns>是否更新成功</returns>
        public static bool UpdateUserSpaceId(int spaceid, int userid)
        {
            if (Discuz.Data.Users.GetShortUserInfo(userid) == null)
                return false;

            Discuz.Data.Users.UpdateUserSpaceId(spaceid, userid);
            return true;
        }

        public static bool UpdateAuthStr(string authStr)
        {
            DataTable dt = Discuz.Data.Users.GetUserIdByAuthStr(authStr);
            if (dt.Rows.Count > 0)
            {
                int uid = TypeConverter.ObjectToInt(dt.Rows[0][0]);

                //将用户调整到相应的用户组
                UserGroupInfo tempGroupInfo = UserCredits.GetCreditsUserGroupId(0);
                if (tempGroupInfo != null)
                    Users.UpdateUserGroup(uid, tempGroupInfo.Groupid);   //添加注册用户审核机制后需要修改 

                //更新激活字段
                Users.UpdateAuthStr(uid, "", 0);
                ForumUtils.WriteUserCookie(uid, TypeConverter.StrToInt(DNTRequest.GetString("expires"), -1), GeneralConfigs.GetConfig().Passwordkey);

                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新用户组
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="groupID">用户组ID</param>
        public static void UpdateUserGroup(int uid, int groupId)
        {
            //Discuz.Data.Users.UpdateUserGroup(uid, groupID);
            UpdateUserGroup(uid.ToString(), groupId);
        }

        /// <summary>
        /// 更新用户的用户组信息
        /// </summary>
        /// <param name="uidList">用户ID列表</param>
        /// <param name="groupId">用户组ID</param>
        public static void UpdateUserGroup(string uidList, int groupId)
        {
            Discuz.Data.Users.UpdateUserGroup(uidList, groupId);
        }

        /// <summary>
        /// 举报信息
        /// </summary>
        /// <returns></returns>
        public static Hashtable GetReportUsers()
        {
            DNTCache cache = DNTCache.GetCacheService();
            Hashtable ht = cache.RetrieveObject("/Forum/ReportUsers") as Hashtable;

            if (ht == null)
            {
                ht = new Hashtable();
                string groupidlist = GeneralConfigs.GetConfig().Reportusergroup;

                if (!Utils.IsNumericList(groupidlist))
                    return ht;

                DataTable dt = Discuz.Data.Users.GetUsers(groupidlist);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //Edit By Cyrano, 修复举报信息重复发送的问题
                    if (!ht.ContainsKey(dt.Rows[i]["uid"]))
                        ht[dt.Rows[i]["uid"]] = dt.Rows[i]["username"];
                }
                cache.AddObject("/Forum/ReportUsers", ht);
            }
            return ht;
        }

        /// <summary>
        /// 通过RewriteName获取用户ID
        /// </summary>
        /// <param name="rewritename"></param>
        /// <returns></returns>
        public static int GetUserIdByRewriteName(string rewritename)
        {
            return Discuz.Data.Users.GetUserIdByRewriteName(rewritename);
        }

        /// <summary>
        /// 更新用户短消息设置
        /// </summary>
        /// <param name="user">用户信息</param>
        public static void UpdateUserPMSetting(UserInfo user)
        {
            Discuz.Data.Users.UpdateUserPMSetting(user);
        }

        /// <summary>
        /// 更新被禁止的用户
        /// </summary>
        /// <param name="groupid">用户组id</param>
        /// <param name="groupexpiry">过期时间</param>
        /// <param name="uid">用户id</param>
        public static void UpdateBanUser(int groupid, string groupexpiry, int uid)
        {
            Discuz.Data.Users.UpdateBanUser(groupid, groupexpiry, uid);

            //此处应该增加日志  把ban的原因记录下来
            NoticeInfo noticeinfo = new NoticeInfo();
            noticeinfo.New = 1;

            if (groupid == 4)
                noticeinfo.Type = NoticeType.BanPostNotice;

            if (groupid == 5)
                noticeinfo.Type = NoticeType.BanVisitNotice;

            noticeinfo.Postdatetime = DateTime.Now.ToShortDateString();
            noticeinfo.Poster = DNTRequest.GetFormString("uname");
            noticeinfo.Posterid = uid;
            noticeinfo.Uid = uid;
            noticeinfo.Note = DNTRequest.GetFormString("reason") + "截至到" + groupexpiry + "到期";

            Notices.CreateNoticeInfo(noticeinfo);
        }

        /// <summary>
        /// 搜索特定板块特殊用户
        /// </summary>
        /// <param name="fid">板块id</param>
        /// <returns></returns>
        public static string SearchSpecialUser(int fid)
        {
            DataTable forumdt = Discuz.Data.Users.SearchSpecialUser(fid);
            return forumdt.Rows.Count > 0 ? forumdt.Rows[0]["permuserlist"].ToString() : null;
        }

        /// <summary>
        /// 更新特定板块特殊用户
        /// </summary>
        /// <param name="permuserlist">特殊用户列表</param>
        /// <param name="fid">板块id</param>
        public static void UpdateSpecialUser(string permuserlist, int fid)
        {
            Discuz.Data.Users.UpdateSpecialUser(permuserlist, fid);
        }

        /// <summary>
        /// 得到指定用户的指定积分扩展字段的积分值
        /// </summary>
        /// <param name="uid">指定用户id</param>
        /// <param name="extnumber">指定扩展字段</param>
        /// <returns>扩展字展积分值</returns>
        public static int GetUserExtCreditsByUserid(int uid, int extnumber)
        {
            if (extnumber > 0 && extnumber <= 8)
                return Discuz.Data.Users.GetUserExtCreditsByUserid(uid, extnumber);
            else
                return 0;
        }


        /// <summary>
        /// 检测临时用户帐号信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="questionid">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        //public static int CheckTempUserInfo(string tempUserName, string tempPassWord, int question, string answer)
        //{
        //    int realUserId = -1;

        //    switch (GeneralConfigs.GetConfig().Passwordmode)
        //    {
        //        case 0://默认模式
        //            {
        //                if (GeneralConfigs.GetConfig().Secques == 1)
        //                    realUserId = Users.CheckPasswordAndSecques(tempUserName, tempPassWord, true, question, answer);
        //                else
        //                    realUserId = Users.CheckPassword(tempUserName, tempPassWord, true);
        //                break;
        //            }
        //        case 1://动网兼容模式
        //            {
        //                if (GeneralConfigs.GetConfig().Secques == 1)
        //                    realUserId = Users.CheckDvBbsPasswordAndSecques(tempUserName, tempPassWord, question, answer);
        //                else
        //                    realUserId = Users.CheckDvBbsPassword(tempUserName, tempPassWord);
        //                break;
        //            }
        //        default://第三方加密验证模式
        //            {
        //                UserInfo userInfo = CheckThirdPartPassword(tempUserName, tempPassWord, question, answer);
        //                realUserId = userInfo != null ? userInfo.Uid : -1;
        //                break;
        //            }  
        //    }
        //    return realUserId;           
        //}

        /// <summary>
        /// 第三方用户密码检查
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public static UserInfo CheckThirdPartPassword(string userName, string passWord, int question, string answer)
        {
            UserInfo userInfo = Users.GetUserInfo(userName);

            //当安全问题未通过时
            if (userInfo != null && GeneralConfigs.GetConfig().Secques == 1 &&
                userInfo.Secques.Trim() != ForumUtils.GetUserSecques(question, answer))
                return null;

            if (PasswordModeProvider.GetInstance() != null && PasswordModeProvider.GetInstance().CheckPassword(userInfo, passWord))
                return userInfo;

            return null;
        }

        /// <summary>
        /// 更改用户组用户的管理权限
        /// </summary>
        /// <param name="adminId">管理组Id</param>
        /// <param name="groupId">用户组Id</param>
        public static void UpdateUserAdminIdByGroupId(int adminId, int groupId)
        {
            Discuz.Data.Users.UpdateUserAdminIdByGroupId(adminId, groupId);
        }

        /// <summary>
        /// 更新用户到禁言组
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        public static void UpdateUserToStopTalkGroup(string uidList)
        {
            Discuz.Data.Users.UpdateUserToStopTalkGroup(uidList);
        }

        /// <summary>
        /// 更新Email验证信息
        /// </summary>
        /// <param name="authstr">验证字符串</param>
        /// <param name="authtime">验证时间</param>
        /// <param name="uid">用户Id</param>
        public static void UpdateEmailValidateInfo(string authstr, DateTime authTime, int uid)
        {
            Discuz.Data.Users.UpdateEmailValidateInfo(authstr, authTime, uid);
        }

        /// <summary>
        /// 根据积分公式更新用户积分
        /// </summary>
        /// <param name="credits">积分公式</param>
        /// <param name="startuid">更新的用户uid起始值</param>
        public static int UpdateUserCredits(string credits, int startuid)
        {
            return Data.Users.UpdateUserCredits(credits, startuid);
        }

        /// <summary>
        /// 获取用户组列表中的所有用户
        /// </summary>
        /// <param name="groupIdList">用户组Id列表</param>
        /// <returns></returns>
        public static DataTable GetUserListByGroupidList(string groupIdList)
        {
            return Data.Users.GetUserListByGroupid(groupIdList);
        }

        /// <summary>
        /// 获取指定用户组的用户并向其发送短信息
        /// </summary>
        /// <param name="groupidlist">用户组</param>
        /// <param name="topNumber">获取前N条记录</param>
        /// <param name="start_uid">大于该uid的用户记录</param>
        /// <param name="msgfrom">谁发的短消息</param>
        /// <param name="msguid">发短消息人的UID</param>
        /// <param name="folder">短消息文件夹</param>
        /// <param name="subject">主题</param>
        /// <param name="postdatetime">发送时间</param>
        /// <param name="message">短消息内容</param>
        /// <returns></returns>
        public static int SendPMByGroupidList(string groupidlist, int topnumber, ref int start_uid, string msgfrom, int msguid, int folder, string subject, string postdatetime, string message)
        {
            DataTable dt = Data.Users.GetUserListByGroupid(groupidlist, topnumber, start_uid);
            foreach (DataRow dr in dt.Rows)
            {
                PrivateMessageInfo pm = new PrivateMessageInfo();
                pm.Msgfrom = msgfrom.Replace("'", "''");
                pm.Msgfromid = msguid;
                pm.Msgto = dr["username"].ToString().Replace("'", "''");
                pm.Msgtoid = Convert.ToInt32(dr["uid"].ToString());
                pm.Folder = folder;
                pm.Subject = subject;
                pm.Postdatetime = postdatetime;
                pm.Message = message;
                pm.New = 1;//标记为未读
                PrivateMessages.CreatePrivateMessage(pm, 0);

                start_uid = pm.Msgtoid;
            }
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获取指定用户组的用户并向其发送邮件
        /// </summary>
        /// <param name="groupidlist">用户组</param>
        /// <param name="topnumber">获取前N条记录</param>
        /// <param name="start_uid">大于该uid的用户记录</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <returns></returns>
        public static int SendEmailByGroupidList(string groupidlist, int topnumber, ref int start_uid, string subject, string body)
        {
            DataTable dt = Data.Users.GetUserListByGroupid(groupidlist, topnumber, start_uid);
            foreach (DataRow dr in dt.Rows)
            {
                if (string.IsNullOrEmpty(dr["Email"].ToString().Trim()))
                {
                    EmailMultiThread emt = new EmailMultiThread(dr["UserName"].ToString().Trim(), dr["Email"].ToString().Trim(), subject, body);
                    new System.Threading.Thread(new System.Threading.ThreadStart(emt.Send)).Start();
                }
                start_uid = TypeConverter.ObjectToInt(dr["uid"]);
            }
            return dt.Rows.Count;
        }



        /// <summary>
        /// 获取用户组的所有用户
        /// </summary>
        /// <param name="groupId">用户组Id</param>
        /// <returns></returns>
        public static DataTable GetUserListByGroupid(int groupId)
        {
            return GetUserListByGroupidList(groupId.ToString());
        }

        /// <summary>
        /// 获取当前页用户列表
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="currentPage">当前页数</param>
        /// <returns></returns>
        public static DataTable GetUserListByCurrentPage(int pageSize, int currentPage)
        {
            return Data.Users.GetUserListByCurrentPage(pageSize, currentPage);
        }

        /// <summary>
        /// 获取用户名列表指定的Email列表
        /// </summary>
        /// <param name="userNameList">用户名列表</param>
        /// <returns></returns>
        public static DataTable GetEmailListByUserNameList(string userNameList)
        {
            return Data.Users.GetEmailListByUserNameList(userNameList);
        }

        /// <summary>
        /// 获取用户组Id列表指定的Email列表
        /// </summary>
        /// <param name="userNameList">用户名列表</param>
        /// <returns></returns>
        public static DataTable GetEmailListByGroupidList(string groupidList)
        {
            return Data.Users.GetEmailListByGroupidList(groupidList);
        }

        /// <summary>
        /// 将Uid列表中的用户更新到目标组中
        /// </summary>
        /// <param name="groupid">目标组</param>
        /// <param name="uidList">用户列表</param>
        public static void UpdateUserGroupByUidList(int groupid, string uidList)
        {
            Data.Users.UpdateUserGroupByUidList(groupid, uidList);
        }

        /// <summary>
        /// 按用户Id列表删除用户
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        public static void DeleteUsers(string uidList)
        {
            if (uidList == "")
                return;
            Data.Users.DeleteUsers(uidList);
        }

        /// <summary>
        /// 清空用户Id列表中的验证码
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        public static void ClearUsersAuthstr(string uidList)
        {
            if (uidList == "")
                return;
            Data.Users.ClearUsersAuthstr(uidList);
        }

        /// <summary>
        /// 清空审核用户组中用户的验证码
        /// </summary>
        public static void ClearUsersAuthstrByUncheckedUserGroup()
        {
            ClearUsersAuthstr(GetUidListByUserGroupId(8));
        }

        /// <summary>
        /// 获取指定用户组的用户Uid列表
        /// </summary>
        /// <param name="userGroupId">用户组Id</param>
        /// <returns></returns>
        private static string GetUidListByUserGroupId(int userGroupId)
        {
            string userIdList = "";
            foreach (DataRow dr in GetUserListByGroupid(userGroupId).Rows)
            {
                userIdList += dr["uid"].ToString() + ",";
            }
            return userIdList.TrimEnd(',');
        }

        public static void DeleteAuditUser()
        {
            DeleteUsers(GetUidListByUserGroupId(8));
        }

        /// <summary>
        /// 搜索未审核用户              
        /// </summary>
        /// <param name="searchUserName">用户名</param>
        /// <param name="regBefore">注册时间</param>
        /// <param name="regIp">注册IP</param>
        /// <returns></returns>
        public static DataTable AuditNewUserClear(string searchUserName, string regBefore, string regIp)
        {
            return Data.Users.AuditNewUserClear(searchUserName, regBefore, regIp);
        }

        /// <summary>
        /// 给指定用户Id列表发送帐户创建成功的Email 
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        /// <returns></returns>
        public static void SendEmailForAccountCreateSucceed(string uidList)
        {
            foreach (DataRow dr in Data.Users.GetUsersByUidlLst(uidList).Rows)
            {
                Emails.DiscuzSmtpMail(dr["username"].ToString().Trim(), dr["email"].ToString().Trim(), "");
            }
        }

        /// <summary>
        /// 获取版块版主
        /// </summary>
        /// <param name="fid">版块Id</param>
        /// <returns></returns>
        public static string GetModerators(int fid)
        {
            string moderatorList = "";
            foreach (DataRow dr in Data.Users.GetModerators(fid).Rows)
                moderatorList += dr["username"].ToString().Trim() + ",";
            return moderatorList.TrimEnd(',');
        }

        /// <summary>
        /// 获取模糊匹配用户名的用户列表
        /// </summary>
        /// <param name="userNameList">用户名列表</param>
        /// <returns>搜索结果表格字符串</returns>
        public static string GetSearchUserList(string userNameList)
        {
            StringBuilder sb = new StringBuilder();
            IDataReader idr = Data.Users.GetUserListByUserName(userNameList);
            int count = 0;
            bool isexist = false;

            sb.Append("<table width=\"100%\" style=\"align:center\"><tr>");
            while (idr.Read())
            {
                //先找出子菜单表中的相关菜单
                isexist = true;

                if (count >= 3)
                {
                    count = 0;
                    sb.Append("</tr><tr>");
                }
                count++;
                sb.Append("<td width=\"33%\" style=\"align:left\"><a href=\"#\" onclick=\"javascript:resetindexmenu('7','3','7,8','global/global_edituser.aspx?uid=" +
                    idr["uid"] + "');\">" + idr["username"] + "</a></td>");
            }
            idr.Close();
            if (!isexist)
            {
                sb.Append("没有找到相匹配的结果");
            }
            sb.Append("</tr></table>");
            return sb.ToString();
        }


        /// <summary>
        /// 通过用户名获取用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static UserInfo GetUserInfo(string userName)
        {
            return Data.Users.GetUserInfo(userName);
        }

        public static DataTable GetUserInfoByEmail(string email)
        {
            return Data.Users.GetUserInfoByEmail(email);
        }

        /// <summary>
        /// 获取用户查询条件
        /// </summary>
        /// <param name="isLike">模糊查询</param>
        /// <param name="isPostDateTime">发帖日期</param>
        /// <param name="userName">用户名</param>
        /// <param name="nickName">昵称</param>
        /// <param name="userGroup">用户组</param>
        /// <param name="email">Email</param>
        /// <param name="credits_Start">积分起始值</param>
        /// <param name="credits_End">积分结束值 </param>
        /// <param name="lastIp">最全登录IP</param>
        /// <param name="posts">帖数</param>
        /// <param name="digestPosts">精华帖数</param>
        /// <param name="uid">Uid</param>
        /// <param name="joindateStart">注册起始日期</param>
        /// <param name="joindateEnd">注册结束日期</param>
        /// <returns></returns>
        public static string GetUsersSearchCondition(bool isLike, bool isPostDateTime, string userName, string nickName,
            string userGroup, string email, string credits_Start, string credits_End, string lastIp, string posts, string digestPosts,
            string uid, string joindateStart, string joindateEnd)
        {
            return Data.Users.GetUsersSearchCondition(isLike, isPostDateTime, userName, nickName,
                userGroup, email, credits_Start, credits_End, lastIp, posts, digestPosts, uid, joindateStart, joindateEnd);
        }

        /// <summary>
        /// 获取按条件搜索得到的用户列表
        /// </summary>
        /// <param name="searchCondition">搜索条件</param>
        /// <returns></returns>
        public static DataTable GetUsersByCondition(string searchCondition)
        {
            return Data.Users.GetUsersByCondition(searchCondition);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pagesize">页面大小</param>
        /// <param name="currentpage">当前页</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable GetUserList(int pagesize, int currentpage, string condition)
        {
            return Data.Users.GetUserList(pagesize, currentpage, condition);
        }

        /// <summary>
        /// 获取用户查询条件
        /// </summary>
        /// <param name="getstring"></param>
        /// <returns></returns>
        public static string GetUserListCondition(string getstring)
        {
            return Discuz.Data.Users.GetUserListCondition(getstring);
        }

        /// <summary>
        /// 向等待验证用户组发送帐户创建成功的邮件
        /// </summary>
        public static void SendEmailForUncheckedUserGroup()
        {
            foreach (DataRow dr in Users.GetUserListByGroupid(8).Rows)
            {
                Emails.DiscuzSmtpMail(dr["username"].ToString().Trim(), dr["email"].ToString().Trim(), dr["password"].ToString().Trim());
            }
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="userInfo">用户信息(密码字段为明文)</param>
        /// <returns>是否成功</returns>
        public static bool ResetPassword(UserInfo userInfo)
        {
            //第三方加密验证模式
            if (GeneralConfigs.GetConfig().Passwordmode > 1 && PasswordModeProvider.GetInstance() != null)
            {
                PasswordModeProvider.GetInstance().SaveUserInfo(userInfo);
            }
            else
            {
                userInfo.Password = Utils.MD5(userInfo.Password);
                Users.UpdateUser(userInfo);
            }
            return true;
        }

        /// <summary>
        /// 通过email获取用户列表
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static List<UserInfo> GetUserListByEmail(string email)
        {
            if (!Utils.IsValidEmail(email))
                return new List<UserInfo>();

            return Data.Users.GetUserListByEmail(email);
        }

        public static void UpdateTrendStat(TrendType trendType)
        {
            Data.Users.UpdateTrendStat(trendType);
        }
    }//class end
}
