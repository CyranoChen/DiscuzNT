using System;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Config;
using Discuz.Common.Generic;
using Discuz.Common;
using Discuz.Cache.Data;

namespace Discuz.Data
{
    public class Users
    {
        /// <summary>
        /// 是否启用TokyoTyrantCache缓存用户表
        /// </summary>
        public static bool appDBCache = (EntLibConfigs.GetConfig() != null && EntLibConfigs.GetConfig().Cacheusers.Enable);

        public static ICacheUsers IUserService = appDBCache ? DBCacheService.GetUsersService() : null;

        /// <summary>
        /// 返回指定用户的完整信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>用户信息</returns>
        public static UserInfo GetUserInfo(int uid)
        {
            if (uid < 0)
                return null;

            IDataReader reader;
            UserInfo userInfo = null;        

            if (appDBCache)
            {
                userInfo = IUserService.GetUserInfo(uid);
                if (userInfo == null)
                {
                    reader = DatabaseProvider.GetInstance().GetUserInfoToReader(uid);
                    if (reader.Read())
                    {
                        userInfo = LoadSingleUserInfo(reader);
                        reader.Close();
                    }
                    if (userInfo != null)//如数据库中有数据而cache中没有则强制添加
                        IUserService.CreateUser(userInfo);
                }
            }
            else
            {
                reader = DatabaseProvider.GetInstance().GetUserInfoToReader(uid);
                if (reader.Read())
                {
                    userInfo = LoadSingleUserInfo(reader);
                    reader.Close();
                }
            }
            return userInfo;
        }

        public static UserInfo LoadSingleUserInfo(IDataReader reader)
        {
            UserInfo userinfo = new UserInfo();
            userinfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
            userinfo.Username = reader["username"].ToString();
            userinfo.Nickname = reader["nickname"].ToString();
            userinfo.Password = reader["password"].ToString();
            userinfo.Spaceid = TypeConverter.ObjectToInt(reader["spaceid"]);
            userinfo.Secques = reader["secques"].ToString();
            userinfo.Gender = TypeConverter.ObjectToInt(reader["gender"]);
            userinfo.Adminid = TypeConverter.ObjectToInt(reader["adminid"]);
            userinfo.Groupid = TypeConverter.ObjectToInt(reader["groupid"]);
            userinfo.Groupexpiry = TypeConverter.ObjectToInt(reader["groupexpiry"]);
            userinfo.Extgroupids = reader["extgroupids"].ToString();
            userinfo.Regip = reader["regip"].ToString();
            userinfo.Joindate = Utils.GetStandardDateTime(reader["joindate"].ToString());
            userinfo.Lastip = reader["lastip"].ToString();
            userinfo.Lastvisit = Utils.GetStandardDateTime(reader["lastvisit"].ToString());
            userinfo.Lastactivity = Utils.GetStandardDateTime(reader["lastactivity"].ToString());
            userinfo.Lastpost = Utils.GetStandardDateTime(reader["lastpost"].ToString());
            userinfo.Lastpostid = TypeConverter.ObjectToInt(reader["lastpostid"]);
            userinfo.Lastposttitle = reader["lastposttitle"].ToString();
            userinfo.Posts = TypeConverter.ObjectToInt(reader["posts"]);
            userinfo.Digestposts = TypeConverter.ObjectToInt(reader["digestposts"]);
            userinfo.Oltime = TypeConverter.ObjectToInt(reader["oltime"]);
            userinfo.Pageviews = TypeConverter.ObjectToInt(reader["pageviews"]);
            userinfo.Credits = TypeConverter.ObjectToInt(reader["credits"]);
            userinfo.Extcredits1 = TypeConverter.StrToFloat(reader["extcredits1"].ToString());
            userinfo.Extcredits2 = TypeConverter.StrToFloat(reader["extcredits2"].ToString());
            userinfo.Extcredits3 = TypeConverter.StrToFloat(reader["extcredits3"].ToString());
            userinfo.Extcredits4 = TypeConverter.StrToFloat(reader["extcredits4"].ToString());
            userinfo.Extcredits5 = TypeConverter.StrToFloat(reader["extcredits5"].ToString());
            userinfo.Extcredits6 = TypeConverter.StrToFloat(reader["extcredits6"].ToString());
            userinfo.Extcredits7 = TypeConverter.StrToFloat(reader["extcredits7"].ToString());
            userinfo.Extcredits8 = TypeConverter.StrToFloat(reader["extcredits8"].ToString());
            userinfo.Medals = reader["medals"].ToString();
            userinfo.Email = reader["email"].ToString();
            userinfo.Bday = reader["bday"].ToString();
            userinfo.Sigstatus = TypeConverter.ObjectToInt(reader["sigstatus"]);
            userinfo.Tpp = TypeConverter.ObjectToInt(reader["tpp"]);
            userinfo.Ppp = TypeConverter.ObjectToInt(reader["ppp"]);
            userinfo.Templateid = TypeConverter.ObjectToInt(reader["templateid"]);
            userinfo.Pmsound = TypeConverter.ObjectToInt(reader["pmsound"]);
            userinfo.Showemail = TypeConverter.ObjectToInt(reader["showemail"]);
            userinfo.Newsletter = (ReceivePMSettingType)TypeConverter.ObjectToInt(reader["newsletter"]);
            userinfo.Invisible = TypeConverter.ObjectToInt(reader["invisible"]);
            userinfo.Newpm = TypeConverter.ObjectToInt(reader["newpm"]);
            userinfo.Newpmcount = TypeConverter.ObjectToInt(reader["newpmcount"]);
            userinfo.Accessmasks = TypeConverter.ObjectToInt(reader["accessmasks"]);
            userinfo.Onlinestate = TypeConverter.ObjectToInt(reader["onlinestate"]);
            userinfo.Website = reader["website"].ToString();
            userinfo.Icq = reader["icq"].ToString();
            userinfo.Qq = reader["qq"].ToString();
            userinfo.Yahoo = reader["yahoo"].ToString();
            userinfo.Msn = reader["msn"].ToString();
            userinfo.Skype = reader["skype"].ToString();
            userinfo.Location = reader["location"].ToString();
            userinfo.Customstatus = reader["customstatus"].ToString();
            userinfo.Bio = reader["bio"].ToString();
            userinfo.Signature = reader["signature"].ToString();
            userinfo.Sightml = reader["sightml"].ToString();
            userinfo.Authstr = reader["authstr"].ToString();
            userinfo.Authtime = reader["authtime"].ToString();
            userinfo.Authflag = Byte.Parse(TypeConverter.ObjectToInt(reader["authflag"]).ToString());
            userinfo.Realname = reader["realname"].ToString();
            userinfo.Idcard = reader["idcard"].ToString();
            userinfo.Mobile = reader["mobile"].ToString();
            userinfo.Phone = reader["phone"].ToString();
            userinfo.Ignorepm = reader["ignorepm"].ToString();
            userinfo.Salt = reader["salt"].ToString().Trim();
            return userinfo;
        }

        public static ShortUserInfo LoadSingleShortUserInfo(IDataReader reader)
        {
            ShortUserInfo userInfo = null;
            if (reader.Read())
            {
                userInfo = new ShortUserInfo();
                userInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
                userInfo.Username = reader["username"].ToString();
                userInfo.Nickname = reader["nickname"].ToString();
                userInfo.Password = reader["password"].ToString();
                userInfo.Spaceid = TypeConverter.ObjectToInt(reader["spaceid"]);
                userInfo.Secques = reader["secques"].ToString();
                userInfo.Gender = TypeConverter.ObjectToInt(reader["gender"]);
                userInfo.Adminid = TypeConverter.ObjectToInt(reader["adminid"]);
                userInfo.Groupid = TypeConverter.ObjectToInt(reader["groupid"]);
                userInfo.Groupexpiry = TypeConverter.ObjectToInt(reader["groupexpiry"]);
                userInfo.Extgroupids = reader["extgroupids"].ToString();
                userInfo.Regip = reader["regip"].ToString();
                userInfo.Joindate = reader["joindate"].ToString();
                userInfo.Lastip = reader["lastip"].ToString();
                userInfo.Lastvisit = reader["lastvisit"].ToString();
                userInfo.Lastactivity = reader["lastactivity"].ToString();
                userInfo.Lastpost = reader["lastpost"].ToString();
                userInfo.Lastpostid = TypeConverter.ObjectToInt(reader["lastpostid"]);
                userInfo.Lastposttitle = reader["lastposttitle"].ToString();
                userInfo.Posts = TypeConverter.ObjectToInt(reader["posts"]);
                userInfo.Digestposts = TypeConverter.StrToInt(reader["digestposts"].ToString());
                userInfo.Oltime = TypeConverter.ObjectToInt(reader["oltime"]);
                userInfo.Pageviews = TypeConverter.StrToInt(reader["pageviews"].ToString());
                userInfo.Credits = TypeConverter.ObjectToInt(reader["credits"]);
                userInfo.Extcredits1 = TypeConverter.StrToFloat(reader["extcredits1"].ToString());
                userInfo.Extcredits2 = TypeConverter.StrToFloat(reader["extcredits2"].ToString());
                userInfo.Extcredits3 = TypeConverter.StrToFloat(reader["extcredits3"].ToString());
                userInfo.Extcredits4 = TypeConverter.StrToFloat(reader["extcredits4"].ToString());
                userInfo.Extcredits5 = TypeConverter.StrToFloat(reader["extcredits5"].ToString());
                userInfo.Extcredits6 = TypeConverter.StrToFloat(reader["extcredits6"].ToString());
                userInfo.Extcredits7 = TypeConverter.StrToFloat(reader["extcredits7"].ToString());
                userInfo.Extcredits8 = TypeConverter.StrToFloat(reader["extcredits8"].ToString());
                userInfo.Email = reader["email"].ToString();
                userInfo.Bday = reader["bday"].ToString();
                userInfo.Sigstatus = TypeConverter.ObjectToInt(reader["sigstatus"]);
                userInfo.Tpp = TypeConverter.ObjectToInt(reader["tpp"]);
                userInfo.Ppp = TypeConverter.ObjectToInt(reader["ppp"]);
                userInfo.Templateid = TypeConverter.ObjectToInt(reader["templateid"]);
                userInfo.Pmsound = TypeConverter.ObjectToInt(reader["pmsound"]);
                userInfo.Showemail = TypeConverter.ObjectToInt(reader["showemail"]);
                userInfo.Newsletter = (ReceivePMSettingType)TypeConverter.ObjectToInt(reader["newsletter"]);
                userInfo.Invisible = TypeConverter.ObjectToInt(reader["invisible"]);
                userInfo.Newpm = TypeConverter.ObjectToInt(reader["newpm"]);
                userInfo.Newpmcount = TypeConverter.ObjectToInt(reader["newpmcount"]);
                userInfo.Accessmasks = TypeConverter.ObjectToInt(reader["accessmasks"]);
                userInfo.Onlinestate = TypeConverter.ObjectToInt(reader["onlinestate"]);
                userInfo.Salt = reader["salt"].ToString();//二次MD5所用的字段
            }
            reader.Close();
            return userInfo;
        }


        /// <summary>
        /// 返回指定用户的简短信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>用户信息</returns>
        public static ShortUserInfo GetShortUserInfo(int uid)
        {
            if (appDBCache)
                return GetUserInfo(uid);
            else
                return LoadSingleShortUserInfo(DatabaseProvider.GetInstance().GetShortUserInfoToReader(uid));
        }

        /// <summary>
        /// 购买主题
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="tid">主题ID</param>
        /// <param name="posterid">发帖人ID</param>
        /// <param name="price">售价</param>
        /// <param name="netamount">净收入</param>
        /// <param name="creditsTrans">要更新的扩展积分ID</param>
        public static void BuyTopic(int uid, int tid, int posterid, int price, float netamount, int creditsTrans)
        {
            DatabaseProvider.GetInstance().BuyTopic(uid, tid, posterid, price, netamount, creditsTrans);

            if (appDBCache)
                IUserService.BuyTopic(uid, tid, posterid, price, netamount, creditsTrans);
        }

        /// <summary>
        /// 更新用户的用户组信息
        /// </summary>
        /// <param name="uidList">用户ID列表</param>
        /// <param name="groupId">用户组ID</param>
        public static void UpdateUserGroup(string uidList, int groupId)
        {
            DatabaseProvider.GetInstance().ChangeUserGroupByUid(groupId, uidList);

            if (appDBCache)
                IUserService.UpdateUserGroup(uidList, groupId);
        }


        /// <summary>
        /// 根据IP查找用户
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>用户信息</returns>
        public static UserInfo GetShortUserInfoByIP(string ip)
        {
            IDataReader reader;
            UserInfo userInfo = null;

            if (appDBCache)
            {
                userInfo = IUserService.GetUserInfoByIP(ip);
                if (userInfo == null)
                {
                    reader = DatabaseProvider.GetInstance().GetUserInfoByIP(ip);
                    if (reader.Read())
                    {
                        userInfo = LoadSingleUserInfo(reader);
                        reader.Close();
                    }
                    //if (userInfo != null)
                    //    IUserService.CreateUser(userInfo);
                }
            }
            else
            {
                reader = DatabaseProvider.GetInstance().GetUserInfoByIP(ip);
                if (reader.Read())
                {
                    userInfo = LoadSingleUserInfo(reader);
                    reader.Close();
                }
            }
            return userInfo;
        }

        /// <summary>
        /// 根据用户名返回用户id
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>用户id</returns>
        public static ShortUserInfo GetShortUserInfoByName(string username)
        {
            if (appDBCache)
            {
                ShortUserInfo shortUserInfo = (ShortUserInfo)IUserService.GetUserInfoByName(username);
                if (shortUserInfo == null)
                {
                    shortUserInfo = LoadSingleShortUserInfo(DatabaseProvider.GetInstance().GetShortUserInfoByName(username));
                    //if (shortUserInfo != null)//如果有值，则通过该UID获取该用户全部个人信息并加入TTCache
                    //{
                    //    UserInfo userInfo = GetUserInfo(shortUserInfo.Uid);
                    //    IUserService.CreateUser(userInfo);
                    //}
                }
                return shortUserInfo;
            }
            else
                return LoadSingleShortUserInfo(DatabaseProvider.GetInstance().GetShortUserInfoByName(username));
        }

        /// <summary>
        /// 获得用户列表DataTable
        /// </summary>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="pageindex">当前页数</param>
        /// <returns>用户列表DataTable</returns>
        public static DataTable GetUserList(int pagesize, int pageindex, string column, string ordertype)
        {
            return DatabaseProvider.GetInstance().GetUserList(pagesize, pageindex, column, ordertype);
        }


        /// <summary>
        /// 检测Email和安全项
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="email">email</param>
        /// <param name="userSecques">用户安全问题答案的存储数据</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public static int CheckEmailAndSecques(string username, string email, string userSecques)
        {
            int userid = -1;
            if (appDBCache)
            {
                userid = IUserService.CheckEmailAndSecques(username, email, userSecques);
                if (userid > 0)
                    return userid;
            }

            IDataReader reader = DatabaseProvider.GetInstance().CheckEmailAndSecques(username, email, userSecques);
            if (reader.Read())
            {
                userid = Int32.Parse(reader[0].ToString());
            }
            reader.Close();
            return userid;
        }

        /// <summary>
        /// 检测密码和安全项
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <param name="userSecques">用户安全问题答案的存储数据</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public static int CheckPasswordAndSecques(string username, string password, bool originalpassword, string userSecques)
        {
            int userid = -1;
            if (appDBCache)
            {
                userid = IUserService.CheckPasswordAndSecques(username, password, originalpassword, userSecques);
                if (userid > 0)
                    return userid;
            }

            IDataReader reader = DatabaseProvider.GetInstance().CheckPasswordAndSecques(username, password, originalpassword, userSecques);
            if (reader.Read())
            {
                userid = Int32.Parse(reader[0].ToString());
            }
            reader.Close();

            if (appDBCache && userid > 0)
                GetUserInfo(userid);

            return userid;
        }


        /// <summary>
        /// 判断用户密码是否正确
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否为未MD5密码</param>
        /// <returns>如果正确则返回uid</returns>
        public static ShortUserInfo CheckPassword(string username, string password, bool originalpassword)
        {
            ShortUserInfo userInfo = null;
            if (appDBCache)
            {
                userInfo = IUserService.CheckPassword(username, password, originalpassword);
                if (userInfo != null)
                    return userInfo;
            }

            IDataReader reader = DatabaseProvider.GetInstance().CheckPassword(username, password, originalpassword);
            if (reader.Read())
            {
                userInfo = new ShortUserInfo();
                userInfo.Uid = Utils.StrToInt(reader[0].ToString(), -1);
                userInfo.Groupid = Utils.StrToInt(reader[1].ToString(), -1);
                userInfo.Adminid = Utils.StrToInt(reader[2].ToString(), -1);
            }
            reader.Close();

            if (appDBCache && userInfo.Uid > 0)
                GetUserInfo(userInfo.Uid);

            return userInfo;
        }

        /// <summary>
        /// 检测密码
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <returns>如果用户密码正确则返回uid, 否则返回-1</returns>
        public static ShortUserInfo CheckPassword(int uid, string password, bool originalpassword)
        {
            ShortUserInfo userInfo = null;
            if (appDBCache)
            {
                userInfo = IUserService.CheckPassword(uid, password, originalpassword);
                if (userInfo != null)
                    return userInfo;
            }

            IDataReader reader = DatabaseProvider.GetInstance().CheckPassword(uid, password, originalpassword);

            if (reader.Read())
            {
                userInfo = new ShortUserInfo();
                userInfo.Uid = Utils.StrToInt(reader[0].ToString(), -1);
                userInfo.Groupid = Utils.StrToInt(reader[1].ToString(), -1);
                userInfo.Adminid = Utils.StrToInt(reader[2].ToString(), -1);
            }
            reader.Close();

            if (appDBCache && userInfo != null && userInfo.Uid > 0)
                GetUserInfo(userInfo.Uid);

            return userInfo;
        }

        /// <summary>
        /// 根据指定的email查找用户并返回用户uid
        /// </summary>
        /// <param name="email">email地址</param>
        /// <returns>用户uid</returns>
        public static int FindUserEmail(string email)
        {
            int userid = -1;
            if (appDBCache)
            {
                userid = IUserService.FindUserEmail(email);
                if (userid > 0)
                    return userid;
            }

            IDataReader reader = DatabaseProvider.GetInstance().FindUserEmail(email);
            if (reader.Read())
            {
                userid = Utils.StrToInt(reader[0].ToString(), -1);
            }
            reader.Close();

            if (appDBCache && userid > 0)
                GetUserInfo(userid);

            return userid;
        }

        /// <summary>
        /// 得到论坛中用户总数
        /// </summary>
        /// <returns>用户总数</returns>
        public static int GetUserCount()
        {
            return DatabaseProvider.GetInstance().GetUserCount();
        }

        /// <summary>
        /// 得到论坛中用户总数
        /// </summary>
        /// <returns>用户总数</returns>
        public static int GetUserCountByAdmin()
        {
            return DatabaseProvider.GetInstance().GetUserCountByAdmin();
        }

        /// <summary>
        /// 创建新用户.
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>返回用户ID, 如果已存在该用户名则返回-1</returns>
        public static int CreateUser(UserInfo userinfo)
        {
            userinfo.Uid = DatabaseProvider.GetInstance().CreateUser(userinfo);
            if (appDBCache)
                IUserService.CreateUser(userinfo);
            return userinfo.Uid;
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="userinfo">用户信息</param>
        /// <returns>是否更新成功</returns>
        public static bool UpdateUser(UserInfo userinfo)
        {
            if (appDBCache)
                IUserService.UpdateUser(userinfo);
            return DatabaseProvider.GetInstance().UpdateUser(userinfo);
        }

        /// <summary>
        /// 更新权限验证字符串
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="authstr">验证串</param>
        /// <param name="authflag">验证标志</param>
        public static void UpdateAuthStr(int uid, string authstr, int authflag)
        {
            DatabaseProvider.GetInstance().UpdateAuthStr(uid, authstr, authflag);
            if (appDBCache)
                IUserService.UpdateAuthStr(uid, authstr, authflag);
        }


        /// <summary>
        /// 更新指定用户的个人资料
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>如果用户不存在则为false, 否则为true</returns>
        public static void UpdateUserProfile(UserInfo userinfo)
        {
            DatabaseProvider.GetInstance().UpdateUserProfile(userinfo);
            if (appDBCache)
                IUserService.UpdateUserProfile(userinfo);
        }

        /// <summary>
        /// 更新用户论坛设置
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>如果用户不存在则返回false, 否则返回true</returns>
        public static void UpdateUserForumSetting(UserInfo userinfo)
        {
            DatabaseProvider.GetInstance().UpdateUserForumSetting(userinfo);
            if (appDBCache)
                IUserService.UpdateUserForumSetting(userinfo);
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
            DatabaseProvider.GetInstance().UpdateUserExtCredits(uid, extid, pos);
            if (appDBCache)
                IUserService.UpdateUserExtCredits(uid, extid, pos);
        }

        /// <summary>
        /// 获得指定用户的指定积分扩展字段的值
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="extid">扩展字段序号(1-8)</param>
        /// <returns>值</returns>
        public static float GetUserExtCredits(int uid, int extid)
        {
            if (appDBCache)
                IUserService.GetUserExtCredits(uid, extid);
            return DatabaseProvider.GetInstance().GetUserExtCredits(uid, extid);
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
        public static void UpdateUserPreference(int uid, string avatar, int avatarwidth, int avatarheight, int templateid)
        {
            DatabaseProvider.GetInstance().UpdateUserPreference(uid, avatar, avatarwidth, avatarheight, templateid);
            if (appDBCache)
                IUserService.UpdateUserPreference(uid, avatar, avatarwidth, avatarheight, templateid);
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <returns>成功返回true否则false</returns>
        public static void UpdateUserPassword(int uid, string password, bool originalpassword)
        {
            DatabaseProvider.GetInstance().UpdateUserPassword(uid, password, originalpassword);
            if (appDBCache)
                IUserService.UpdateUserPassword(uid, password, originalpassword);
        }

        /// <summary>
        /// 更新用户安全问题
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="userSecques">用户安全问题答案的存储数据</param>
        /// <returns>成功返回true否则false</returns>
        public static void UpdateUserSecques(int uid, string userSecques)
        {
            DatabaseProvider.GetInstance().UpdateUserSecques(uid, userSecques);
            if (appDBCache)
                IUserService.UpdateUserSecques(uid, userSecques);
        }


        /// <summary>
        /// 更新用户最后登录时间
        /// </summary>
        /// <param name="uid">用户id</param>
        public static void UpdateUserLastvisit(int uid, string ip)
        {
            DatabaseProvider.GetInstance().UpdateUserLastvisit(uid, ip);
            if (appDBCache)
                IUserService.UpdateUserLastvisit(uid, ip);
        }

        /// <summary>
        /// 更新用户当前的在线状态
        /// </summary>
        /// <param name="uidlist">用户uid列表</param>
        /// <param name="state">当前在线状态(0:离线,1:在线)</param>
        public static void UpdateUserOnlineState(string uidlist, int state, string activitytime)
        {
            switch (state)
            {
                case 0:		//正常退出
                    DatabaseProvider.GetInstance().UpdateUserOnlineStateAndLastActivity(uidlist, 0, activitytime);
                    break;
                case 1:		//正常登录
                    DatabaseProvider.GetInstance().UpdateUserOnlineStateAndLastVisit(uidlist, 1, activitytime);
                    break;
                case 2:		//超时退出
                    DatabaseProvider.GetInstance().UpdateUserOnlineStateAndLastActivity(uidlist, 0, activitytime);
                    break;
                case 3:		//隐身登录
                    DatabaseProvider.GetInstance().UpdateUserOnlineStateAndLastVisit(uidlist, 0, activitytime);
                    break;
            }

            if (appDBCache)
                IUserService.UpdateUserOnlineState(uidlist, state, activitytime);
        }

        /// <summary>
        /// 更新用户当前的在线状态
        /// </summary>
        /// <param name="uid">用户uid列表</param>
        /// <param name="state">当前在线状态(0:离线,1:在线)</param>
        public static void UpdateUserOnlineState(int uid, int state, string activitytime)
        {
            switch (state)
            {
                case 0:		//正常退出
                    DatabaseProvider.GetInstance().UpdateUserOnlineStateAndLastActivity(uid, 0, activitytime);
                    break;
                case 1:		//正常登录
                    DatabaseProvider.GetInstance().UpdateUserOnlineStateAndLastVisit(uid, 1, activitytime);
                    break;
                case 2:		//超时退出
                    DatabaseProvider.GetInstance().UpdateUserOnlineStateAndLastActivity(uid, 0, activitytime);
                    break;
                case 3:		//隐身登录
                    DatabaseProvider.GetInstance().UpdateUserOnlineStateAndLastVisit(uid, 0, activitytime);
                    break;
            }

            if (appDBCache)
                IUserService.UpdateUserOnlineState(uid, state, activitytime);
        }

        /// <summary>
        /// 更新用户当前的在线时间和最后活动时间
        /// </summary>
        /// <param name="uid">用户uid</param>
        public static void UpdateUserOnlineTime(int uid, string activitytime)
        {
            DatabaseProvider.GetInstance().UpdateUserLastActivity(uid, activitytime);

            if (appDBCache)
                IUserService.UpdateUserOnlineTime(uid, activitytime);
        }

        /// <summary>
        /// 设置用户信息表中未读短消息的数量
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="pmnum">短消息数量</param>
        /// <returns>更新记录个数</returns>
        public static int SetUserNewPMCount(int uid, int pmnum)
        {
            if (appDBCache)
                IUserService.SetUserNewPMCount(uid, pmnum);

            return DatabaseProvider.GetInstance().SetUserNewPMCount(uid, pmnum);
        }

        /// <summary>
        /// 将用户的未读短信息数量减小一个指定的值
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="subval">短消息将要减小的值,负数为加</param>
        /// <returns>更新记录个数</returns>
        public static int DecreaseNewPMCount(int uid, int subval)
        {
            if (appDBCache)
                IUserService.DecreaseNewPMCount(uid, subval);

            return DatabaseProvider.GetInstance().DecreaseNewPMCount(uid, subval);
        }

        /// <summary>
        /// 更新用户精华数
        /// </summary>
        /// <param name="useridlist">uid列表</param>
        /// <returns></returns>
        public static int UpdateUserDigest(string useridlist)
        {
            if (appDBCache)
                IUserService.UpdateUserDigest(useridlist);

            return DatabaseProvider.GetInstance().UpdateUserDigest(useridlist);
        }

        /// <summary>
        /// 更新用户SpaceID
        /// </summary>
        /// <param name="spaceid">要更新的SpaceId</param>
        /// <param name="userid">要更新的UserId</param>
        /// <returns>是否更新成功</returns>
        public static void UpdateUserSpaceId(int spaceid, int userid)
        {
            if (appDBCache)
                IUserService.UpdateUserSpaceId(spaceid, userid);

            DatabaseProvider.GetInstance().UpdateUserSpaceId(spaceid, userid);
        }


        public static DataTable GetUserIdByAuthStr(string authstr)
        {
            return DatabaseProvider.GetInstance().GetUserIdByAuthStr(authstr);
        }


        public static DataTable GetUsers(string groupIdList)
        {
            return DatabaseProvider.GetInstance().GetUsers(groupIdList);
        }

        /// <summary>
        /// 通过RewriteName获取用户ID
        /// </summary>
        /// <param name="rewritename"></param>
        /// <returns></returns>
        public static int GetUserIdByRewriteName(string rewritename)
        {
            return DatabaseProvider.GetInstance().GetUserIdByRewriteName(rewritename);
        }

        /// <summary>
        /// 更新用户短消息设置
        /// </summary>
        /// <param name="user">用户信息</param>
        public static void UpdateUserPMSetting(UserInfo user)
        {
            DatabaseProvider.GetInstance().UpdateUserPMSetting(user);

            if (appDBCache)
                IUserService.UpdateUserPMSetting(user);
        }

        /// <summary>
        /// 更新被禁止的用户
        /// </summary>
        /// <param name="groupid">用户组id</param>
        /// <param name="groupexpiry">过期时间</param>
        /// <param name="uid">用户id</param>
        public static void UpdateBanUser(int groupid, string groupexpiry, int uid)
        {
            DatabaseProvider.GetInstance().UpdateBanUser(groupid, groupexpiry, uid);

            if (appDBCache)
                IUserService.UpdateBanUser(groupid, groupexpiry, uid);
        }

        /// <summary>
        /// 搜索特定板块特殊用户
        /// </summary>
        /// <param name="fid">板块id</param>
        /// <returns></returns>
        public static DataTable SearchSpecialUser(int fid)
        {
            return DatabaseProvider.GetInstance().SearchSpecialUser(fid);
        }

        /// <summary>
        /// 更新特定板块特殊用户
        /// </summary>
        /// <param name="permuserlist">特殊用户列表</param>
        /// <param name="fid">板块id</param>
        public static void UpdateSpecialUser(string permuserlist, int fid)
        {
            DatabaseProvider.GetInstance().UpdateSpecialUser(permuserlist, fid);
        }

        /// <summary>
        /// 得到指定用户的指定积分扩展字段的积分值
        /// </summary>
        /// <param name="uid">指定用户id</param>
        /// <param name="extnumber">指定扩展字段</param>
        /// <returns>扩展字展积分值</returns>
        public static int GetUserExtCreditsByUserid(int uid, int extnumber)
        {
            int score = -1;
            if (appDBCache)
            {
                score = IUserService.GetUserExtCreditsByUserid(uid, extnumber);
                if (score >= 0)
                    return score;
            }
            return DatabaseProvider.GetInstance().GetUserExtCreditsByUserid(uid, extnumber);
        }

        /// <summary>
        /// 更新用户勋章信息
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="medals">勋章信息</param>
        public static void UpdateMedals(int uid, string medals)
        {
            DatabaseProvider.GetInstance().UpdateMedals(uid, medals);

            if (appDBCache)
                IUserService.UpdateMedals(uid, medals);
        }

        /// <summary>
        /// 更改用户组用户的管理权限
        /// </summary>
        /// <param name="adminId">管理组Id</param>
        /// <param name="groupId">用户组Id</param>
        public static void UpdateUserAdminIdByGroupId(int adminId, int groupId)
        {
            DatabaseProvider.GetInstance().UpdateUserAdminIdByGroupId(adminId, groupId);

            if (appDBCache)
                IUserService.UpdateUserAdminIdByGroupId(adminId, groupId);
        }

        /// <summary>
        /// 更新用户到禁言组
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        public static void UpdateUserToStopTalkGroup(string uidList)
        {
            DatabaseProvider.GetInstance().SetStopTalkUser(uidList);

            if (appDBCache)
                IUserService.UpdateUserToStopTalkGroup(uidList);
        }

        /// <summary>
        /// 清除用户所发帖数以及精华数
        /// </summary>
        /// <param name="uid">用户Id</param>
        public static void ClearPosts(int uid)
        {
            DatabaseProvider.GetInstance().ClearPosts(uid);

            if (appDBCache)
                IUserService.ClearPosts(uid);
        }

        /// <summary>
        /// 更新Email验证信息
        /// </summary>
        /// <param name="authstr">验证字符串</param>
        /// <param name="authtime">验证时间</param>
        /// <param name="uid">用户Id</param>
        public static void UpdateEmailValidateInfo(string authstr, DateTime authTime, int uid)
        {
            DatabaseProvider.GetInstance().UpdateEmailValidateInfo(authstr, authTime, uid);

            if (appDBCache)
                IUserService.UpdateEmailValidateInfo(authstr, authTime, uid);
        }

        /// <summary>
        /// 更新用户积分
        /// </summary>
        /// <param name="credits">积分公式</param>
        /// <param name="startuid">更新的用户uid起始值</param>
        public static int UpdateUserCredits(string credits, int startuid)
        {
            return DatabaseProvider.GetInstance().UpdateUserCredits(credits, startuid);
        }

        /// <summary>
        /// 获取用户组列表中的所有用户
        /// </summary>
        /// <param name="groupIdList">用户组列表</param>
        /// <returns></returns>
        public static DataTable GetUserListByGroupid(string groupIdList)
        {
            return DatabaseProvider.GetInstance().GetUserListByGroupid(groupIdList);
        }

        /// <summary>
        /// 获取用户组列表中的所有用户
        /// </summary>
        /// <param name="groupIdList">用户组</param>
        /// <param name="topNumber">获取前N条记录</param>
        /// <param name="start_uid">大于该uid的用户记录</param>
        /// <returns></returns>
        public static DataTable GetUserListByGroupid(string groupIdList, int topNumber, int start_uid)
        {
            return DatabaseProvider.GetInstance().GetUserListByGroupid(groupIdList, topNumber, start_uid);
        }


        /// <summary>
        /// 获取当前页用户列表
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="currentPage">当前页数</param>
        /// <returns></returns>
        public static DataTable GetUserListByCurrentPage(int pageSize, int currentPage)
        {
            return DatabaseProvider.GetInstance().GetUserList(pageSize, currentPage);
        }

        /// <summary>
        /// 获取用户名列表指定的Email列表
        /// </summary>
        /// <param name="userNameList">用户名列表</param>
        /// <returns></returns>
        public static DataTable GetEmailListByUserNameList(string userNameList)
        {
            return DatabaseProvider.GetInstance().MailListTable(userNameList);
        }

        /// <summary>
        /// 获取用户组Id列表指定的Email列表
        /// </summary>
        /// <param name="userNameList">用户名列表</param>
        /// <returns></returns>
        public static DataTable GetEmailListByGroupidList(string groupidList)
        {
            return DatabaseProvider.GetInstance().GetUserEmailByGroupid(groupidList);
        }

        /// <summary>
        /// 将Uid列表中的用户更新到目标组中
        /// </summary>
        /// <param name="groupid">目标组</param>
        /// <param name="uidList">用户列表</param>
        public static void UpdateUserGroupByUidList(int groupid, string uidList)
        {
            DatabaseProvider.GetInstance().ChangeUserGroupByUid(groupid, uidList);
            if (appDBCache)
                IUserService.UpdateUserGroupByUidList(groupid, uidList);
        }

        /// <summary>
        /// 按用户Id列表删除用户
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        public static void DeleteUsers(string uidList)
        {
            //TODO:是否应该调用DeleteUser方法？
            DatabaseProvider.GetInstance().DeleteUserByUidlist(uidList);
            if (appDBCache)
                IUserService.DeleteUsers(uidList);
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="delPosts">是否删除帖子</param>
        /// <param name="delPms">是否删除短信</param>
        /// <returns></returns>
        public static bool DeleteUser(int uid, bool delPosts, bool delPms)
        {
            if (appDBCache)
                IUserService.DeleteUser(uid, delPosts, delPms);

            if (Topics.appDBCache && Topics.ITopicService != null)
                Topics.ITopicService.DeleteUserTopic(uid, delPosts);

            return DatabaseProvider.GetInstance().DelUserAllInf(uid, delPosts, delPms);
        }

        /// <summary>
        /// 清空用户Id列表中的验证码
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        public static void ClearUsersAuthstr(string uidList)
        {
            DatabaseProvider.GetInstance().ClearAuthstrByUidlist(uidList);

            if (appDBCache)
                IUserService.ClearUsersAuthstr(uidList);
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
            return DatabaseProvider.GetInstance().AuditNewUserClear(searchUserName, regBefore, regIp);
        }

        /// <summary>
        /// 获取用户Id列表中的用户
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        /// <returns></returns>
        public static DataTable GetUsersByUidlLst(string uidList)
        {
            return DatabaseProvider.GetInstance().GetUsersByUidlLst(uidList);
        }

        /// <summary>
        /// 获取版块版主
        /// </summary>
        /// <param name="fid">版块Id</param>
        /// <returns></returns>
        public static DataTable GetModerators(int fid)
        {
            return DatabaseProvider.GetInstance().GetModerators(fid);
        }

        /// <summary>
        /// 获取模糊匹配用户名的用户列表
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static IDataReader GetUserListByUserName(string userName)
        {
            return DatabaseProvider.GetInstance().GetUserInfoByName(userName);
        }

        /// <summary>
        /// 更新用户签名，来自，简介三个字段
        /// </summary>
        /// <param name="__userinfo"></param>
        /// <returns></returns>
        public static void UpdateUserShortInfo(string location, string bio, string signature, int uid)
        {
            DatabaseProvider.GetInstance().UpdateUserShortInfo(location, bio, signature, uid);

            if (appDBCache)
                IUserService.UpdateUserInfo(location, bio, signature, uid);
        }

        public static UserInfo GetUserInfo(string userName)
        {
            IDataReader reader;
            UserInfo userInfo = null;
            if (appDBCache)
            {
                userInfo = IUserService.GetUserInfoByName(userName);
                if (userInfo == null)
                {
                    reader = DatabaseProvider.GetInstance().GetUserInfoToReader(userName);
                    if (reader.Read())
                    {
                        userInfo = LoadSingleUserInfo(reader);
                        reader.Close();
                    }

                    //if (userInfo != null)
                    //    IUserService.CreateUser(userInfo);
                }
            }
            else
            {
                reader = DatabaseProvider.GetInstance().GetUserInfoToReader(userName);
                if (reader.Read())
                {
                    userInfo = LoadSingleUserInfo(reader);
                    reader.Close();
                }
            }
            return userInfo;
        }


        public static DataTable GetUserInfoByEmail(string email)
        {
            return DatabaseProvider.GetInstance().GetUserInfoByEmail(email);
        }

        /// <summary>
        /// 设置用户为版主
        /// </summary>
        /// <param name="userName">用户名</param>
        public static void SetUserToModerator(string userName)
        {
            DatabaseProvider.GetInstance().SetModerator(userName);

            if (appDBCache)
                IUserService.SetUserToModerator(userName);
        }

        /// <summary>
        /// 合并用户
        /// </summary>
        /// <param name="postTableName">分表名称</param>
        /// <param name="targetUserInfo">目标用户</param>
        /// <param name="srcUserInfo">要合并用户</param>
        public static void CombinationUser(string postTableName, UserInfo targetUserInfo, UserInfo srcUserInfo)
        {
            DatabaseProvider.GetInstance().CombinationUser(postTableName, targetUserInfo, srcUserInfo);

            if (appDBCache)
                IUserService.CombinationUser(postTableName, targetUserInfo, srcUserInfo);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="start_uid">起始UId</param>
        /// <param name="end_uid">终止UID</param>
        /// <returns></returns>
        public static IDataReader GetUsers(int start_uid, int end_uid)
        {
            return DatabaseProvider.GetInstance().GetUsers(start_uid, end_uid);
        }

        /// <summary>
        /// 更新用户帖数
        /// </summary>
        /// <param name="postcount">帖子数</param>
        /// <param name="userid">用户ID</param>
        public static void UpdateUserPostCount(int postcount, int userid)
        {
            DatabaseProvider.GetInstance().UpdateUserPostCount(postcount, userid);

            if (appDBCache)
                IUserService.UpdateUserPostCount(postcount, userid);
        }
        /// <summary>
        /// 更新所有用户的帖子数
        /// </summary>
        /// <param name="postTableId"></param>
        public static void UpdateAllUserPostCount(int postTableId)
        {
            DatabaseProvider.GetInstance().UpdateAllUserPostCount(postTableId);

            if (appDBCache)
                IUserService.UpdateAllUserPostCount(postTableId);
        }
        /// <summary>
        /// 重建用户精华帖数
        /// </summary>
        public static void ResetUserDigestPosts()
        {
            DatabaseProvider.GetInstance().ResetUserDigestPosts();

            if (appDBCache)
                IUserService.ResetUserDigestPosts();
        }

        /// <summary>
        /// 获取指定数量的用户
        /// </summary>
        /// <param name="statcount">获取数量</param>
        /// <param name="lastuid">最小用户ID</param>
        /// <returns></returns>
        public static IDataReader GetTopUsers(int statcount, int lastuid)
        {
            return DatabaseProvider.GetInstance().GetTopUsers(statcount, lastuid);
        }

        /// <summary>
        /// 得到论坛中最后注册的用户ID和用户名
        /// </summary>
        /// <param name="lastuserid">输出参数：最后注册的用户ID</param>
        /// <param name="lastusername">输出参数：最后注册的用户名</param>
        /// <returns>存在返回true,不存在返回false</returns>
        public static bool GetLastUserInfo(out string lastuserid, out string lastusername)
        {
            return DatabaseProvider.GetInstance().GetLastUserInfo(out lastuserid, out lastusername);
        }

        /// <summary>
        /// 更新普通用户用户组
        /// </summary>
        /// <param name="groupid">用户组id</param>
        /// <param name="userid">用户ID</param>
        public static void UpdateUserOtherInfo(int groupid, int userid)
        {
            DatabaseProvider.GetInstance().UpdateUserOtherInfo(groupid, userid);

            if (appDBCache)
                IUserService.UpdateUserOtherInfo(groupid, userid);
        }

        /// <summary>
        /// 更新在线表用户信息
        /// </summary>
        /// <param name="groupid">用户组id</param>
        /// <param name="userid">用户ID</param>
        public static void UpdateUserOnlineInfo(int groupid, int userid)
        {
            DatabaseProvider.GetInstance().UpdateUserOnlineInfo(groupid, userid);

            if (appDBCache)
                IUserService.UpdateUserOnlineInfo(groupid, userid);
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
            return DatabaseProvider.GetInstance().Global_UserGrid_SearchCondition(isLike, isPostDateTime, userName, nickName,
                userGroup, email, credits_Start, credits_End, lastIp, posts, digestPosts, uid, joindateStart, joindateEnd);
        }

        /// <summary>
        /// 获取按条件搜索得到的用户列表
        /// </summary>
        /// <param name="searchCondition">搜索条件</param>
        /// <returns></returns>
        public static DataTable GetUsersByCondition(string searchCondition)
        {
            return DatabaseProvider.GetInstance().Global_UserGrid(searchCondition);
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
            return DatabaseProvider.GetInstance().UserList(pagesize, currentpage, condition);
        }

        /// <summary>
        /// 获取符合条件的用户数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static int GetUserCount(string condition)
        {
            return DatabaseProvider.GetInstance().Global_UserGrid_RecordCount(condition);
        }

        /// <summary>
        /// 获取用户查询条件
        /// </summary>
        /// <param name="getstring"></param>
        /// <returns></returns>
        public static string GetUserListCondition(string getstring)
        {
            return DatabaseProvider.GetInstance().Global_UserGrid_GetCondition(getstring);
        }

        /// <summary>
        /// 更新我的帖子
        /// </summary>
        /// <param name="lasttableid">分表ID</param>
        public static void UpdateMyPost(int lasttableid)
        {
            DatabaseProvider.GetInstance().UpdateMyPost(lasttableid);
        }

        /// <summary>
        /// 通过email获取用户列表
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static List<UserInfo> GetUserListByEmail(string email)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUserListByEmail(email);

            List<UserInfo> userList = new List<UserInfo>();

            while (reader.Read())
            {
                userList.Add(LoadSingleUserInfo(reader));
            }
            reader.Close();

            return userList;
        }

        public static void UpdateTrendStat(TrendType trendType)
        {
            DatabaseProvider.GetInstance().UpdateTrendStat(trendType);
        }
    }
}
