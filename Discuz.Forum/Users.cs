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
    /// �û�������
    /// </summary>
    public class Users
    {
        /// <summary>
        /// ������Ӧ�û���չ�����ֶ���Ϣ
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
        /// ����ָ���û���������Ϣ
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <returns>�û���Ϣ</returns>
        public static UserInfo GetUserInfo(int uid)
        {
            return Discuz.Data.Users.GetUserInfo(uid);
        }

        /// <summary>
        /// ����ָ���û��ļ����Ϣ
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <returns>�û���Ϣ</returns>
        public static ShortUserInfo GetShortUserInfo(int uid)
        {
            return Discuz.Data.Users.GetShortUserInfo(uid);
        }

        /// <summary>
        /// ����IP�����û�
        /// </summary>
        /// <param name="ip">ip��ַ</param>
        /// <returns>�û���Ϣ</returns>
        public static string CheckRegisterDateDiff(string ip)
        {
            ShortUserInfo userinfo = Discuz.Data.Users.GetShortUserInfoByIP(ip);

            if (GeneralConfigs.GetConfig().Regctrl > 0 && userinfo != null)
            {
                int Interval = Utils.StrDateDiffHours(userinfo.Joindate, GeneralConfigs.GetConfig().Regctrl);
                if (Interval <= 0)
                    return "��Ǹ, ϵͳ������IPע��������, �������� " + (Interval * -1) + " Сʱ��ſ���ע��";
            }

            if (GeneralConfigs.GetConfig().Ipregctrl.Trim() != "" && Utils.InIPArray(DNTRequest.GetIP(), Utils.SplitString(GeneralConfigs.GetConfig().Ipregctrl, "\n")) && userinfo != null)
            {
                int Interval = Utils.StrDateDiffHours(userinfo.Joindate, 72);
                if (Interval < 0)
                    return "��Ǹ, ϵͳ����������IPע������, �������� " + (Interval * -1) + " Сʱ��ſ���ע��";
            }
            return null;
        }


        /// <summary>
        /// �����û��������û�id
        /// </summary>
        /// <param name="username">�û���</param>
        /// <returns>�û�id</returns>
        public static int GetUserId(string username)
        {
            ShortUserInfo userInfo = Discuz.Data.Users.GetShortUserInfoByName(username);
            return (userInfo != null) ? userInfo.Uid : 0;
        }

        /// <summary>
        /// ����û��б�DataTable
        /// </summary>
        /// <param name="pagesize">ÿҳ��¼��</param>
        /// <param name="pageindex">��ǰҳ��</param>
        /// <returns>�û��б�DataTable</returns>
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
        /// ��ȡר������WebService���û��б�
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUserListOnService(int pagesize, int pageindex, string column, string ordertype)
        {
            return Discuz.Data.Users.GetUserList(pagesize, pageindex, column, ordertype);
        }


        /// <summary>
        /// ���Email�Ͱ�ȫ��
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="email">email</param>
        /// <param name="questionid">����id</param>
        /// <param name="answer">��</param>
        /// <returns>�����ȷ�򷵻��û�id, ���򷵻�-1</returns>
        public static bool CheckEmailAndSecques(string username, string email, int questionid, string answer, string forumPath)
        {
            int uid = Discuz.Data.Users.CheckEmailAndSecques(username, email, ForumUtils.GetUserSecques(questionid, answer));
            if (uid != -1)
            {
                string Authstr = ForumUtils.CreateAuthStr(20);
                Users.UpdateAuthStr(uid, Authstr, 2);

                StringBuilder body = new StringBuilder(username);
                body.AppendFormat("����!<br />��������� {0}", GeneralConfigs.GetConfig().Forumtitle);
                body.Append(" ���͵�.<br /><br />���յ�����ʼ�,����Ϊ�����ǵ���̳����������ַ���Ǽ�Ϊ�û�����,�Ҹ��û�����ʹ�� Email �������ù�������.");
                body.Append("<br /><br />----------------------------------------------------------------------");
                body.Append("<br />��Ҫ��");
                body.Append("<br /><br />----------------------------------------------------------------------");
                body.Append("<br /><br />�����û���ύ�������õ��������������̳��ע���û�,���������Բ�ɾ������ʼ�.ֻ����ȷ����Ҫ��������������,�ż����Ķ����������.");
                body.Append("<br /><br />----------------------------------------------------------------------");
                body.Append("<br />��������˵��");
                body.Append("<br /><br />----------------------------------------------------------------------");
                body.Append("<br /><br />��ֻ�����ύ����������֮��,ͨ��������������������������:<br /><br />");
                body.AppendFormat("<a href={0}/setnewpassword.aspx?uid={1}&id={2} target=_blank>{0}", forumPath, uid, Authstr);
                body.AppendFormat("/setnewpassword.aspx?uid={0}&id={1}</a>", uid, Authstr);
                body.Append("<br /><br />(������治��������ʽ,�뽫��ַ�ֹ�ճ�����������ַ���ٷ���)");
                body.Append("<br /><br />�����ҳ��򿪺�,�����µ�������ύ,֮��������ʹ���µ������¼��̳��.���������û������������ʱ�޸���������.");
                body.AppendFormat("<br /><br />�������ύ�ߵ� IP Ϊ {0}<br /><br /><br /><br />", DNTRequest.GetIP());
                body.AppendFormat("<br />���� <br /><br />{0} �����Ŷ�.<br />{1}<br /><br />", GeneralConfigs.GetConfig().Forumtitle, forumPath);

                Emails.DiscuzSmtpMailToUser(DNTRequest.GetString("email"), GeneralConfigs.GetConfig().Forumtitle + " ȡ������˵��", body.ToString());
                return true;
            }
            return false;
        }


        /// <summary>
        /// �������Ͱ�ȫ��
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="password">����</param>
        /// <param name="originalpassword">�Ƿ��MD5����</param>
        /// <param name="questionid">����id</param>
        /// <param name="answer">��</param>
        /// <returns>�����ȷ�򷵻��û�id, ���򷵻�-1</returns>
        public static int CheckPasswordAndSecques(string username, string password, bool originalpassword, int questionid, string answer)
        {
            return Discuz.Data.Users.CheckPasswordAndSecques(username, password, originalpassword, ForumUtils.GetUserSecques(questionid, answer));
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="password">����</param>
        /// <param name="originalpassword">�Ƿ��MD5����</param>
        /// <returns>�����ȷ�򷵻��û�id, ���򷵻�-1</returns>
        public static int CheckPassword(string username, string password, bool originalpassword)
        {
            ShortUserInfo userInfo = Discuz.Data.Users.CheckPassword(username, password, originalpassword);

            return userInfo == null ? -1 : userInfo.Uid;
        }



        /// <summary>
        /// ���DVBBS����ģʽ������
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="password">����</param>
        /// <param name="questionid">����id</param>
        /// <param name="answer">��</param>
        /// <returns>�����ȷ�򷵻��û�id, ���򷵻�-1</returns>
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
        /// ���DVBBS����ģʽ������
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="password">����</param>
        /// <returns>�����ȷ�򷵻��û�id, ���򷵻�-1</returns>
        public static int CheckDvBbsPassword(string username, string password)
        {
            return CheckDvBbsPasswordAndSecques(username, password, -1, null);
        }


        /// <summary>
        /// �ж��û������Ƿ���ȷ
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="password">����</param>
        /// <param name="originalpassword">�Ƿ�ΪδMD5����</param>
        /// <param name="groupid">�û���ID</param>
        /// <param name="adminid">������ID</param>
        /// <returns>�����ȷ�򷵻�uid</returns>
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
        /// �������
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="password">����</param>
        /// <param name="originalpassword">�Ƿ��MD5����</param>
        /// <param name="groupid">�û���id</param>
        /// <param name="adminid">����id</param>
        /// <returns>����û�������ȷ�򷵻�uid, ���򷵻�-1</returns>
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
        /// �ж�ָ���û������Ƿ���ȷ.
        /// </summary>
        /// <param name="uid">�û�ID</param>
        /// <param name="password">�û�����</param>
        /// <returns>����û�������ȷ�򷵻�true, ���򷵻�false</returns>
        public static int CheckPassword(int uid, string password, bool originalpassword)
        {
            ShortUserInfo userInfo = Discuz.Data.Users.CheckPassword(uid, password, originalpassword);

            return userInfo == null ? -1 : userInfo.Uid;
        }

        /// <summary>
        /// ����ָ����email�����û��������û�uid
        /// </summary>
        /// <param name="email">email��ַ</param>
        /// <returns>�û�uid</returns>
        public static bool ValidateEmail(string email)
        {
            if (GeneralConfigs.GetConfig().Doublee == 0 && Discuz.Data.Users.FindUserEmail(email) != -1)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// ����ָ����email�����û��������û�uid
        /// </summary>
        /// <param name="email">email��ַ</param>
        /// <returns>�û�uid</returns>
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
        /// �õ���̳���û�����
        /// </summary>
        /// <returns>�û�����</returns>
        public static int GetUserCount(string condition)
        {
            return (condition == "") ? Discuz.Data.Users.GetUserCount() : Data.Users.GetUserCount(condition);
        }

        /// <summary>
        /// �õ���̳���û�����
        /// </summary>
        /// <returns>�û�����</returns>
        public static int GetUserCountByAdmin(string orderby)
        {
            if (orderby == "admin")
                return Discuz.Data.Users.GetUserCountByAdmin();

            return Users.GetUserCount("");
        }

        /// <summary>
        /// �������û�.
        /// </summary>
        /// <param name="__userinfo">�û���Ϣ</param>
        /// <returns>�����û�ID, ����Ѵ��ڸ��û����򷵻�-1</returns>
        public static int CreateUser(UserInfo userinfo)
        {
            if (GetUserId(userinfo.Username) > 0)
                return -1;

            return Discuz.Data.Users.CreateUser(userinfo);
        }

        /// <summary>
        /// �����û�
        /// </summary>
        /// <param name="userinfo">�û���Ϣ</param>
        /// <returns>�Ƿ���³ɹ�</returns>
        public static bool UpdateUser(UserInfo userinfo)
        {
            if (userinfo == null)
                return false;

            return Discuz.Data.Users.UpdateUser(userinfo);
        }

        /// <summary>
        /// ����Ȩ����֤�ַ���
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="authstr">��֤��</param>
        /// <param name="authflag">��֤��־</param>
        public static void UpdateAuthStr(int uid, string authstr, int authflag)
        {
            Discuz.Data.Users.UpdateAuthStr(uid, authstr, authflag);
        }


        /// <summary>
        /// ����ָ���û��ĸ�������
        /// </summary>
        /// <param name="__userinfo">�û���Ϣ</param>
        /// <returns>����û���������Ϊfalse, ����Ϊtrue</returns>
        public static bool UpdateUserProfile(UserInfo userinfo)
        {
            if (Discuz.Data.Users.GetShortUserInfo(userinfo.Uid) == null)
                return false;

            Discuz.Data.Users.UpdateUserProfile(userinfo);
            return true;
        }

        /// <summary>
        /// �����û���̳����
        /// </summary>
        /// <param name="__userinfo">�û���Ϣ</param>
        /// <returns>����û��������򷵻�false, ���򷵻�true</returns>
        public static bool UpdateUserForumSetting(UserInfo userinfo)
        {
            if (Discuz.Data.Users.GetShortUserInfo(userinfo.Uid) == null)
                return false;

            Discuz.Data.Users.UpdateUserForumSetting(userinfo);
            return true;
        }

        /// <summary>
        /// �޸��û��Զ�������ֶε�ֵ
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="extid">��չ�ֶ����(1-8)</param>
        /// <param name="pos">���ӵ���ֵ(�����Ǹ���)</param>
        /// <returns>ִ���Ƿ�ɹ�</returns>
        public static void UpdateUserExtCredits(int uid, int extid, float pos)
        {
            Discuz.Data.Users.UpdateUserExtCredits(uid, extid, pos);
        }

        /// <summary>
        /// ���ָ���û���ָ��������չ�ֶε�ֵ
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="extid">��չ�ֶ����(1-8)</param>
        /// <returns>ֵ</returns>
        public static float GetUserExtCredits(int uid, int extid)
        {
            return Discuz.Data.Users.GetUserExtCredits(uid, extid);
        }

        /// <summary>
        /// �����û�ͷ��
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="avatar">ͷ��</param>
        /// <param name="avatarwidth">ͷ����</param>
        /// <param name="avatarheight">ͷ��߶�</param>
        /// <param name="templateid">ģ��Id</param>
        /// <returns>����û��������򷵻�false, ���򷵻�true</returns>
        public static bool UpdateUserPreference(int uid, string avatar, int avatarwidth, int avatarheight, int templateid)
        {
            if (Discuz.Data.Users.GetShortUserInfo(uid) == null)
                return false;

            Discuz.Data.Users.UpdateUserPreference(uid, avatar, avatarwidth, avatarheight, templateid);
            return true;
        }


        /// <summary>
        /// �����û�����
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="password">����</param>
        /// <param name="originalpassword">�Ƿ��MD5����</param>
        /// <returns>�ɹ�����true����false</returns>
        public static bool UpdateUserPassword(int uid, string password, bool originalpassword)
        {
            if (Discuz.Data.Users.GetShortUserInfo(uid) == null)
                return false;

            Discuz.Data.Users.UpdateUserPassword(uid, password, originalpassword);
            return true;
        }

        /// <summary>
        /// �����û���ȫ����
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="questionid">����id</param>
        /// <param name="answer">��</param>
        /// <returns>�ɹ�����true����false</returns>
        public static bool UpdateUserSecques(int uid, int questionid, string answer)
        {
            if (Discuz.Data.Users.GetShortUserInfo(uid) == null)
                return false;

            Discuz.Data.Users.UpdateUserSecques(uid, ForumUtils.GetUserSecques(questionid, answer));
            return true;
        }


        /// <summary>
        /// �����û����ֺ�����¼ʱ��
        /// </summary>
        /// <param name="uid">�û�id</param>
        public static void UpdateUserCreditsAndVisit(int uid, string ip)
        {
            UserCredits.UpdateUserCredits(uid);
            Discuz.Data.Users.UpdateUserLastvisit(uid, ip);
        }


        /// <summary>
        /// ����ָ���û���ѫ����Ϣ
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="medals">ѫ����Ϣ</param>
        /// <param name="adminUid">������Id</param>
        /// <param name="adminUserName">�������û���</param>
        /// <param name="ip">IP</param>
        /// <param name="reason">����</param>
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
                        Data.Medals.CreateMedalslog(adminUid, adminUserName, ip, givenusername, uid, "����", int.Parse(medalid), reason);
                    }
                    else
                    {
                        Data.Medals.UpdateMedalslog("����", DateTime.Now, reason, "�ջ�", int.Parse(medalid), uid);
                    }
                }
            }
        }



        /// <summary>
        /// ���û���δ������Ϣ������Сһ��ָ����ֵ
        /// </summary>
        /// <param name="uid">�û�ID</param>
        /// <param name="olid">�����û�id</param>
        /// <returns>���¼�¼����</returns>
        public static int UpdateUserNewPMCount(int uid, int olid)
        {
            int newPMs = Discuz.Data.PrivateMessages.GetNewPMCount(uid);
            OnlineUsers.UpdateNewPms(olid, newPMs);
            return Discuz.Data.Users.SetUserNewPMCount(uid, newPMs);
        }


        /// <summary>
        /// �����û�SpaceID
        /// </summary>
        /// <param name="spaceid">Ҫ���µ�SpaceId</param>
        /// <param name="userid">Ҫ���µ�UserId</param>
        /// <returns>�Ƿ���³ɹ�</returns>
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

                //���û���������Ӧ���û���
                UserGroupInfo tempGroupInfo = UserCredits.GetCreditsUserGroupId(0);
                if (tempGroupInfo != null)
                    Users.UpdateUserGroup(uid, tempGroupInfo.Groupid);   //���ע���û���˻��ƺ���Ҫ�޸� 

                //���¼����ֶ�
                Users.UpdateAuthStr(uid, "", 0);
                ForumUtils.WriteUserCookie(uid, TypeConverter.StrToInt(DNTRequest.GetString("expires"), -1), GeneralConfigs.GetConfig().Passwordkey);

                return true;
            }
            return false;
        }

        /// <summary>
        /// �����û���
        /// </summary>
        /// <param name="uid">�û�ID</param>
        /// <param name="groupID">�û���ID</param>
        public static void UpdateUserGroup(int uid, int groupId)
        {
            //Discuz.Data.Users.UpdateUserGroup(uid, groupID);
            UpdateUserGroup(uid.ToString(), groupId);
        }

        /// <summary>
        /// �����û����û�����Ϣ
        /// </summary>
        /// <param name="uidList">�û�ID�б�</param>
        /// <param name="groupId">�û���ID</param>
        public static void UpdateUserGroup(string uidList, int groupId)
        {
            Discuz.Data.Users.UpdateUserGroup(uidList, groupId);
        }

        /// <summary>
        /// �ٱ���Ϣ
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
                    //Edit By Cyrano, �޸��ٱ���Ϣ�ظ����͵�����
                    if (!ht.ContainsKey(dt.Rows[i]["uid"]))
                        ht[dt.Rows[i]["uid"]] = dt.Rows[i]["username"];
                }
                cache.AddObject("/Forum/ReportUsers", ht);
            }
            return ht;
        }

        /// <summary>
        /// ͨ��RewriteName��ȡ�û�ID
        /// </summary>
        /// <param name="rewritename"></param>
        /// <returns></returns>
        public static int GetUserIdByRewriteName(string rewritename)
        {
            return Discuz.Data.Users.GetUserIdByRewriteName(rewritename);
        }

        /// <summary>
        /// �����û�����Ϣ����
        /// </summary>
        /// <param name="user">�û���Ϣ</param>
        public static void UpdateUserPMSetting(UserInfo user)
        {
            Discuz.Data.Users.UpdateUserPMSetting(user);
        }

        /// <summary>
        /// ���±���ֹ���û�
        /// </summary>
        /// <param name="groupid">�û���id</param>
        /// <param name="groupexpiry">����ʱ��</param>
        /// <param name="uid">�û�id</param>
        public static void UpdateBanUser(int groupid, string groupexpiry, int uid)
        {
            Discuz.Data.Users.UpdateBanUser(groupid, groupexpiry, uid);

            //�˴�Ӧ��������־  ��ban��ԭ���¼����
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
            noticeinfo.Note = DNTRequest.GetFormString("reason") + "������" + groupexpiry + "����";

            Notices.CreateNoticeInfo(noticeinfo);
        }

        /// <summary>
        /// �����ض���������û�
        /// </summary>
        /// <param name="fid">���id</param>
        /// <returns></returns>
        public static string SearchSpecialUser(int fid)
        {
            DataTable forumdt = Discuz.Data.Users.SearchSpecialUser(fid);
            return forumdt.Rows.Count > 0 ? forumdt.Rows[0]["permuserlist"].ToString() : null;
        }

        /// <summary>
        /// �����ض���������û�
        /// </summary>
        /// <param name="permuserlist">�����û��б�</param>
        /// <param name="fid">���id</param>
        public static void UpdateSpecialUser(string permuserlist, int fid)
        {
            Discuz.Data.Users.UpdateSpecialUser(permuserlist, fid);
        }

        /// <summary>
        /// �õ�ָ���û���ָ��������չ�ֶεĻ���ֵ
        /// </summary>
        /// <param name="uid">ָ���û�id</param>
        /// <param name="extnumber">ָ����չ�ֶ�</param>
        /// <returns>��չ��չ����ֵ</returns>
        public static int GetUserExtCreditsByUserid(int uid, int extnumber)
        {
            if (extnumber > 0 && extnumber <= 8)
                return Discuz.Data.Users.GetUserExtCreditsByUserid(uid, extnumber);
            else
                return 0;
        }


        /// <summary>
        /// �����ʱ�û��ʺ���Ϣ
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="password">����</param>
        /// <param name="questionid">����id</param>
        /// <param name="answer">��</param>
        /// <returns>�����ȷ�򷵻��û�id, ���򷵻�-1</returns>
        //public static int CheckTempUserInfo(string tempUserName, string tempPassWord, int question, string answer)
        //{
        //    int realUserId = -1;

        //    switch (GeneralConfigs.GetConfig().Passwordmode)
        //    {
        //        case 0://Ĭ��ģʽ
        //            {
        //                if (GeneralConfigs.GetConfig().Secques == 1)
        //                    realUserId = Users.CheckPasswordAndSecques(tempUserName, tempPassWord, true, question, answer);
        //                else
        //                    realUserId = Users.CheckPassword(tempUserName, tempPassWord, true);
        //                break;
        //            }
        //        case 1://��������ģʽ
        //            {
        //                if (GeneralConfigs.GetConfig().Secques == 1)
        //                    realUserId = Users.CheckDvBbsPasswordAndSecques(tempUserName, tempPassWord, question, answer);
        //                else
        //                    realUserId = Users.CheckDvBbsPassword(tempUserName, tempPassWord);
        //                break;
        //            }
        //        default://������������֤ģʽ
        //            {
        //                UserInfo userInfo = CheckThirdPartPassword(tempUserName, tempPassWord, question, answer);
        //                realUserId = userInfo != null ? userInfo.Uid : -1;
        //                break;
        //            }  
        //    }
        //    return realUserId;           
        //}

        /// <summary>
        /// �������û�������
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public static UserInfo CheckThirdPartPassword(string userName, string passWord, int question, string answer)
        {
            UserInfo userInfo = Users.GetUserInfo(userName);

            //����ȫ����δͨ��ʱ
            if (userInfo != null && GeneralConfigs.GetConfig().Secques == 1 &&
                userInfo.Secques.Trim() != ForumUtils.GetUserSecques(question, answer))
                return null;

            if (PasswordModeProvider.GetInstance() != null && PasswordModeProvider.GetInstance().CheckPassword(userInfo, passWord))
                return userInfo;

            return null;
        }

        /// <summary>
        /// �����û����û��Ĺ���Ȩ��
        /// </summary>
        /// <param name="adminId">������Id</param>
        /// <param name="groupId">�û���Id</param>
        public static void UpdateUserAdminIdByGroupId(int adminId, int groupId)
        {
            Discuz.Data.Users.UpdateUserAdminIdByGroupId(adminId, groupId);
        }

        /// <summary>
        /// �����û���������
        /// </summary>
        /// <param name="uidList">�û�Id�б�</param>
        public static void UpdateUserToStopTalkGroup(string uidList)
        {
            Discuz.Data.Users.UpdateUserToStopTalkGroup(uidList);
        }

        /// <summary>
        /// ����Email��֤��Ϣ
        /// </summary>
        /// <param name="authstr">��֤�ַ���</param>
        /// <param name="authtime">��֤ʱ��</param>
        /// <param name="uid">�û�Id</param>
        public static void UpdateEmailValidateInfo(string authstr, DateTime authTime, int uid)
        {
            Discuz.Data.Users.UpdateEmailValidateInfo(authstr, authTime, uid);
        }

        /// <summary>
        /// ���ݻ��ֹ�ʽ�����û�����
        /// </summary>
        /// <param name="credits">���ֹ�ʽ</param>
        /// <param name="startuid">���µ��û�uid��ʼֵ</param>
        public static int UpdateUserCredits(string credits, int startuid)
        {
            return Data.Users.UpdateUserCredits(credits, startuid);
        }

        /// <summary>
        /// ��ȡ�û����б��е������û�
        /// </summary>
        /// <param name="groupIdList">�û���Id�б�</param>
        /// <returns></returns>
        public static DataTable GetUserListByGroupidList(string groupIdList)
        {
            return Data.Users.GetUserListByGroupid(groupIdList);
        }

        /// <summary>
        /// ��ȡָ���û�����û������䷢�Ͷ���Ϣ
        /// </summary>
        /// <param name="groupidlist">�û���</param>
        /// <param name="topNumber">��ȡǰN����¼</param>
        /// <param name="start_uid">���ڸ�uid���û���¼</param>
        /// <param name="msgfrom">˭���Ķ���Ϣ</param>
        /// <param name="msguid">������Ϣ�˵�UID</param>
        /// <param name="folder">����Ϣ�ļ���</param>
        /// <param name="subject">����</param>
        /// <param name="postdatetime">����ʱ��</param>
        /// <param name="message">����Ϣ����</param>
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
                pm.New = 1;//���Ϊδ��
                PrivateMessages.CreatePrivateMessage(pm, 0);

                start_uid = pm.Msgtoid;
            }
            return dt.Rows.Count;
        }

        /// <summary>
        /// ��ȡָ���û�����û������䷢���ʼ�
        /// </summary>
        /// <param name="groupidlist">�û���</param>
        /// <param name="topnumber">��ȡǰN����¼</param>
        /// <param name="start_uid">���ڸ�uid���û���¼</param>
        /// <param name="subject">����</param>
        /// <param name="body">����</param>
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
        /// ��ȡ�û���������û�
        /// </summary>
        /// <param name="groupId">�û���Id</param>
        /// <returns></returns>
        public static DataTable GetUserListByGroupid(int groupId)
        {
            return GetUserListByGroupidList(groupId.ToString());
        }

        /// <summary>
        /// ��ȡ��ǰҳ�û��б�
        /// </summary>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="currentPage">��ǰҳ��</param>
        /// <returns></returns>
        public static DataTable GetUserListByCurrentPage(int pageSize, int currentPage)
        {
            return Data.Users.GetUserListByCurrentPage(pageSize, currentPage);
        }

        /// <summary>
        /// ��ȡ�û����б�ָ����Email�б�
        /// </summary>
        /// <param name="userNameList">�û����б�</param>
        /// <returns></returns>
        public static DataTable GetEmailListByUserNameList(string userNameList)
        {
            return Data.Users.GetEmailListByUserNameList(userNameList);
        }

        /// <summary>
        /// ��ȡ�û���Id�б�ָ����Email�б�
        /// </summary>
        /// <param name="userNameList">�û����б�</param>
        /// <returns></returns>
        public static DataTable GetEmailListByGroupidList(string groupidList)
        {
            return Data.Users.GetEmailListByGroupidList(groupidList);
        }

        /// <summary>
        /// ��Uid�б��е��û����µ�Ŀ������
        /// </summary>
        /// <param name="groupid">Ŀ����</param>
        /// <param name="uidList">�û��б�</param>
        public static void UpdateUserGroupByUidList(int groupid, string uidList)
        {
            Data.Users.UpdateUserGroupByUidList(groupid, uidList);
        }

        /// <summary>
        /// ���û�Id�б�ɾ���û�
        /// </summary>
        /// <param name="uidList">�û�Id�б�</param>
        public static void DeleteUsers(string uidList)
        {
            if (uidList == "")
                return;
            Data.Users.DeleteUsers(uidList);
        }

        /// <summary>
        /// ����û�Id�б��е���֤��
        /// </summary>
        /// <param name="uidList">�û�Id�б�</param>
        public static void ClearUsersAuthstr(string uidList)
        {
            if (uidList == "")
                return;
            Data.Users.ClearUsersAuthstr(uidList);
        }

        /// <summary>
        /// �������û������û�����֤��
        /// </summary>
        public static void ClearUsersAuthstrByUncheckedUserGroup()
        {
            ClearUsersAuthstr(GetUidListByUserGroupId(8));
        }

        /// <summary>
        /// ��ȡָ���û�����û�Uid�б�
        /// </summary>
        /// <param name="userGroupId">�û���Id</param>
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
        /// ����δ����û�              
        /// </summary>
        /// <param name="searchUserName">�û���</param>
        /// <param name="regBefore">ע��ʱ��</param>
        /// <param name="regIp">ע��IP</param>
        /// <returns></returns>
        public static DataTable AuditNewUserClear(string searchUserName, string regBefore, string regIp)
        {
            return Data.Users.AuditNewUserClear(searchUserName, regBefore, regIp);
        }

        /// <summary>
        /// ��ָ���û�Id�б����ʻ������ɹ���Email 
        /// </summary>
        /// <param name="uidList">�û�Id�б�</param>
        /// <returns></returns>
        public static void SendEmailForAccountCreateSucceed(string uidList)
        {
            foreach (DataRow dr in Data.Users.GetUsersByUidlLst(uidList).Rows)
            {
                Emails.DiscuzSmtpMail(dr["username"].ToString().Trim(), dr["email"].ToString().Trim(), "");
            }
        }

        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <param name="fid">���Id</param>
        /// <returns></returns>
        public static string GetModerators(int fid)
        {
            string moderatorList = "";
            foreach (DataRow dr in Data.Users.GetModerators(fid).Rows)
                moderatorList += dr["username"].ToString().Trim() + ",";
            return moderatorList.TrimEnd(',');
        }

        /// <summary>
        /// ��ȡģ��ƥ���û������û��б�
        /// </summary>
        /// <param name="userNameList">�û����б�</param>
        /// <returns>�����������ַ���</returns>
        public static string GetSearchUserList(string userNameList)
        {
            StringBuilder sb = new StringBuilder();
            IDataReader idr = Data.Users.GetUserListByUserName(userNameList);
            int count = 0;
            bool isexist = false;

            sb.Append("<table width=\"100%\" style=\"align:center\"><tr>");
            while (idr.Read())
            {
                //���ҳ��Ӳ˵����е���ز˵�
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
                sb.Append("û���ҵ���ƥ��Ľ��");
            }
            sb.Append("</tr></table>");
            return sb.ToString();
        }


        /// <summary>
        /// ͨ���û�����ȡ�û���Ϣ
        /// </summary>
        /// <param name="userName">�û���</param>
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
        /// ��ȡ�û���ѯ����
        /// </summary>
        /// <param name="isLike">ģ����ѯ</param>
        /// <param name="isPostDateTime">��������</param>
        /// <param name="userName">�û���</param>
        /// <param name="nickName">�ǳ�</param>
        /// <param name="userGroup">�û���</param>
        /// <param name="email">Email</param>
        /// <param name="credits_Start">������ʼֵ</param>
        /// <param name="credits_End">���ֽ���ֵ </param>
        /// <param name="lastIp">��ȫ��¼IP</param>
        /// <param name="posts">����</param>
        /// <param name="digestPosts">��������</param>
        /// <param name="uid">Uid</param>
        /// <param name="joindateStart">ע����ʼ����</param>
        /// <param name="joindateEnd">ע���������</param>
        /// <returns></returns>
        public static string GetUsersSearchCondition(bool isLike, bool isPostDateTime, string userName, string nickName,
            string userGroup, string email, string credits_Start, string credits_End, string lastIp, string posts, string digestPosts,
            string uid, string joindateStart, string joindateEnd)
        {
            return Data.Users.GetUsersSearchCondition(isLike, isPostDateTime, userName, nickName,
                userGroup, email, credits_Start, credits_End, lastIp, posts, digestPosts, uid, joindateStart, joindateEnd);
        }

        /// <summary>
        /// ��ȡ�����������õ����û��б�
        /// </summary>
        /// <param name="searchCondition">��������</param>
        /// <returns></returns>
        public static DataTable GetUsersByCondition(string searchCondition)
        {
            return Data.Users.GetUsersByCondition(searchCondition);
        }

        /// <summary>
        /// ��ȡ�û��б�
        /// </summary>
        /// <param name="pagesize">ҳ���С</param>
        /// <param name="currentpage">��ǰҳ</param>
        /// <param name="condition">����</param>
        /// <returns></returns>
        public static DataTable GetUserList(int pagesize, int currentpage, string condition)
        {
            return Data.Users.GetUserList(pagesize, currentpage, condition);
        }

        /// <summary>
        /// ��ȡ�û���ѯ����
        /// </summary>
        /// <param name="getstring"></param>
        /// <returns></returns>
        public static string GetUserListCondition(string getstring)
        {
            return Discuz.Data.Users.GetUserListCondition(getstring);
        }

        /// <summary>
        /// ��ȴ���֤�û��鷢���ʻ������ɹ����ʼ�
        /// </summary>
        public static void SendEmailForUncheckedUserGroup()
        {
            foreach (DataRow dr in Users.GetUserListByGroupid(8).Rows)
            {
                Emails.DiscuzSmtpMail(dr["username"].ToString().Trim(), dr["email"].ToString().Trim(), dr["password"].ToString().Trim());
            }
        }

        /// <summary>
        /// �����û�����
        /// </summary>
        /// <param name="userInfo">�û���Ϣ(�����ֶ�Ϊ����)</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public static bool ResetPassword(UserInfo userInfo)
        {
            //������������֤ģʽ
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
        /// ͨ��email��ȡ�û��б�
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
