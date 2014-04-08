using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common.Generic;
using System.Text;
using System.Web;

namespace Discuz.Forum
{
    /// <summary>
    /// UserCreditsFactory ��ժҪ˵����
    /// </summary>
    public class UserCredits
    {
        /// <summary>
        /// ���ݻ��ֹ�ʽ�����û�����,�����ܷ����䶯Ӱ���п��ܻ�����û��������û���
        /// <param name="uid">�û�ID</param>
        /// </summary>
        public static int UpdateUserCredits(int uid)
        {
            ShortUserInfo userInfo = uid > 0 ? Users.GetShortUserInfo(uid) : null;
            if (userInfo != null)
            {
                Discuz.Data.UserCredits.UpdateUserCredits(uid);
                UserGroupInfo tmpUserGroupInfo = UserGroups.GetUserGroupInfo(userInfo.Groupid);

                if (tmpUserGroupInfo != null && (UserGroups.IsCreditUserGroup(tmpUserGroupInfo) || tmpUserGroupInfo.Groupid == 7))//���û���Ϊ�����û��������IDΪ�ο�(ID=7)
                {
                    tmpUserGroupInfo = GetCreditsUserGroupId(userInfo.Credits);
                    if (tmpUserGroupInfo.Groupid != userInfo.Groupid)//���û������鷢���仯ʱ
                    {
                        Discuz.Data.Users.UpdateUserGroup(userInfo.Uid.ToString(), tmpUserGroupInfo.Groupid);
                        Discuz.Data.OnlineUsers.UpdateGroupid(userInfo.Uid, tmpUserGroupInfo.Groupid);
                    }
                }
                //�жϲ����û��Ƿ��ǵ�ǰ�û�������������dntusertips��cookie
                HttpCookie cookie = HttpContext.Current.Request.Cookies["dnt"];
                if (cookie != null)
                {
                    if (cookie["userid"] == uid.ToString())
                        ForumUtils.WriteUserCreditsCookie(userInfo, tmpUserGroupInfo.Grouptitle);
                }
                return 1;

            }
            else
                return 0;
        }


        /// <summary>
        /// �������ֶ�����COOKIE
        /// </summary>
        /// <param name="values">�����б�</param>
        public static void WriteUpdateUserExtCreditsCookies(float[] values)
        {
            StringBuilder creditsValue = new StringBuilder("");
            creditsValue.Append("0,");
            foreach (float s in values)
            {
                creditsValue.Append(s.ToString());
                creditsValue.Append(",");
            }

            HttpCookie cookie = HttpContext.Current.Request.Cookies["discuz_creditnotice"];
            if (cookie == null)
            {
                cookie = new HttpCookie("discuz_creditnotice");
            }
            cookie.Value = creditsValue.ToString().TrimEnd(',');
            cookie.Expires = DateTime.Now.AddMinutes(36000);
            cookie.Path = BaseConfigs.GetForumPath;
            HttpContext.Current.Response.AppendCookie(cookie);
            //Utils.WriteCookie("discuz_creditnotice", creditsValue.ToString().TrimEnd(','), 36000);
        }


        /// <summary>
        /// ͨ��ָ��ֵ�����û�����
        /// </summary>
        /// <param name="uid">�û�ID</param>
        /// <param name="values">���ֱ䶯ֵ,Ӧ��֤��һ������Ϊ8������,��Ӧ8����չ���ֵı䶯ֵ</param>
        /// <param name="allowMinus">�Ƿ������۳ɸ���,true����,false�������Ҳ����п۷ַ���-1</param>
        /// <returns></returns>
        public static int UpdateUserExtCredits(int uid, float[] values, bool allowMinus)
        {
            if (uid < 1 || Discuz.Data.Users.GetUserInfo(uid) == null)
                return 0;

            if (values.Length < 8)
                return -1;

            if (!allowMinus)//������۳ɸ���ʱҪ�����жϻ����Ƿ��㹻����
            {
                // ���Ҫ����չ����, �����ж���չ�����Ƿ��㹻����
                if (!Discuz.Data.UserCredits.CheckUserCreditsIsEnough(uid, values))
                    return -1;
            }

            Discuz.Data.UserCredits.UpdateUserExtCredits(uid, values);

            UpdateUserCredits(uid);

            //��Ӧ��ͬ����չ����
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != 0.0)
                {
                    Sync.UpdateCredits(uid, i + 1, values[i].ToString(), "");
                }
            }
            ///�����û�����
            return 1;
        }


        /// <summary>
        /// ����û������Ƿ��㹻����(�����ڵ��û�, ������������)
        /// </summary>
        /// <param name="uid">�û�ID</param>
        /// <param name="mount">��������,�������ϴ�2�����������˲���,��ô�˲���ֵӦΪ2</param>
        /// <param name="creditsOperationType">���ֲ�������,�緢����</param>
        /// <param name="pos">�ӻ����־(����Ϊ��,����Ϊ��,ͨ��������1����-1)</param>
        /// <returns></returns>
        public static bool CheckUserCreditsIsEnough(int uid, int mount, CreditsOperationType creditsOperationType, int pos)
        {
            DataTable dt = Scoresets.GetScoreSet();

            dt.PrimaryKey = new DataColumn[1] { dt.Columns["id"] };

            float[] extCredits = new float[8];
            for (int i = 0; i < 8; i++)
            {
                extCredits[i] = TypeConverter.ObjectToFloat(dt.Rows[(int)creditsOperationType]["extcredits" + (i + 1)]);
            }

            if (pos < 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (Utils.StrToFloat(extCredits[i], 0) < 0)//ֻҪ�κ�һ��Ҫ�����,��ȥ���ݿ���
                        return Discuz.Data.UserCredits.CheckUserCreditsIsEnough(uid, extCredits, pos, mount);
                }
            }
            return true;
        }


        /// <summary>
        /// �����û�����(�����ڵ��û�,������������)
        /// </summary>
        /// <param name="uid">�û�ID</param>
        /// <param name="extCredits">ʹ�õĻ��ֹ���</param>
        /// <param name="mount">��������,�������ϴ�2�����������˲���,��ô�˲���ֵӦΪ2</param>
        /// <param name="creditsOperationType">���ֲ�������,�緢����</param>
        /// <param name="pos">�ӻ����־(����Ϊ��,����Ϊ��,ͨ��������1����-1)</param>
        /// <param name="allowMinus">�Ƿ������۳ɸ���,true����,false�������Ҳ����п۷ַ���-1</param>
        /// <returns></returns>
        public static int UpdateUserExtCredits(int uid, float[] extCredits, int mount, CreditsOperationType creditsOperationType, int pos, bool allowMinus)
        {
            //float[] extCredits = Scoresets.GetUserExtCredits(creditsOperationType);
            float extCredit = 0;

            foreach (float e in extCredits)//��ѭ������У�鵱ǰ���ֲ����Ƿ���Ҫ�����û�����
            {
                if (e != 0)
                {
                    extCredit = e;
                    break;
                }
            }

            if (extCredit == 0)//�����������������ȫ��Ϊ0�������������֣���ֱ�ӷ���1
                return 1;

            if (uid == -1)//�����ǰ�û�Ϊ�οͣ���ֱ�ӷ���-1
                return -1;

            // ���Ҫ����չ����, �����ж���չ�����Ƿ��㹻����
            if (pos < 0)
            {
                //������ɾ�������ظ�ʱ
                if (creditsOperationType != CreditsOperationType.PostTopic && creditsOperationType != CreditsOperationType.PostReply)
                {
                    if (!allowMinus && !Discuz.Data.UserCredits.CheckUserCreditsIsEnough(uid, extCredits, pos, mount))
                        return -1;
                }
            }
            else
            {
                if (creditsOperationType == CreditsOperationType.DownloadAttachment || creditsOperationType == CreditsOperationType.Search)//��ʱ�Խ���û������۷ֿ���Ϊ���������⣬������ϵͳ�����¿���ʱ�����жϴ���ɸ���ʵ������õ�
                {
                    if (!allowMinus && !Discuz.Data.UserCredits.CheckUserCreditsIsEnough(uid, extCredits, -1, mount))
                        return -1;
                }
            }

            Discuz.Data.UserCredits.UpdateUserExtCredits(uid, extCredits, pos, mount);

            for (int i = 0; i < extCredits.Length; i++)
            {
                if (extCredits[i] != 0.0)
                {
                    Sync.UpdateCredits(uid, i + 1, extCredits[i].ToString(), "");
                }
            }

            ///�����û�����
            return 1;
        }

        /// <summary>
        /// �����û�����(�����ڵ��û�,������������)
        /// </summary>
        /// <param name="uid">�û�ID</param>
        /// <param name="mount">��������,�������ϴ�2�����������˲���,��ô�˲���ֵӦΪ2</param>
        /// <param name="creditsOperationType">���ֲ�������,�緢����</param>
        /// <param name="pos">�ӻ����־(����Ϊ��,����Ϊ��,ͨ��������1����-1)</param>
        /// <param name="allowMinus">�Ƿ������۳ɸ���,true����,false�������Ҳ����п۷ַ���-1</param>
        /// <returns></returns>
        public static int UpdateUserExtCredits(int uid, int mount, CreditsOperationType creditsOperationType, int pos, bool allowMinus)
        {
            return UpdateUserExtCredits(uid, Scoresets.GetUserExtCredits(creditsOperationType), mount, creditsOperationType, pos, allowMinus);
        }

        /// <summary>
        /// �����û��б�,һ�θ��¶���û��Ļ���
        /// </summary>
        /// <param name="uidlist">�û�ID�б�</param>
        /// <param name="values">��չ����ֵ</param>
        public static int UpdateUserExtCredits(string uidlist, float[] values)
        {
            int reval = -1;
            if (Utils.IsNumericList(uidlist))
            {
                reval = 0;
                ///���ݹ�ʽ�����û����ܻ���,������	
                foreach (string uid in Utils.SplitString(uidlist, ","))
                {
                    if (TypeConverter.StrToInt(uid, 0) > 0)
                        reval = reval + UpdateUserExtCredits(TypeConverter.StrToInt(uid, 0), values, true);
                }
            }
            return reval;
        }



        /// <summary>
        /// �����û��б�,һ�θ��¶���û��Ļ���
        /// </summary>
        /// <param name="uidlist">�û�ID�б�</param>
        /// <param name="creditsOperationType">���ֲ�������,�緢����</param>
        /// <param name="pos">�ӻ����־(����Ϊ��,����Ϊ��,ͨ��������1����-1)</param>
        public static int UpdateUserCreditsAndExtCredits(string uidlist, CreditsOperationType creditsOperationType, int pos)
        {
            int reval = -1;
            if (Utils.IsNumericList(uidlist))
            {
                reval = 0;
                ///���ݹ�ʽ�����û����ܻ���,������	
                foreach (string uid in Utils.SplitString(uidlist, ","))
                {
                    if (TypeConverter.StrToInt(uid, 0) > 0)
                        reval = reval + UpdateUserExtCredits(TypeConverter.StrToInt(uid), 1, creditsOperationType, pos, false);

                    UserCredits.UpdateUserCredits(TypeConverter.StrToInt(uid));
                }
            }
            return reval;
        }


        /// <summary>
        /// �����û��б�,һ�θ��¶���û��Ļ���(�˷���ֻ��ɾ������ʱʹ�ù�)
        /// </summary>
        /// <param name="uidlist">�û�ID�б�</param>
        /// <param name="creditsOperationType">���ֲ�������,�緢����</param>
        /// <param name="pos">�ӻ����־(����Ϊ��,����Ϊ��,ͨ��������1����-1)</param>
        private static int UpdateUserCredits(int[] uidlist, CreditsOperationType creditsOperationType, int pos)
        {
            ///���ݹ�ʽ�����û����ܻ���,������
            int[] mountlist = new int[uidlist.Length];
            for (int i = 0; i < mountlist.Length; i++)
            {
                mountlist[i] = 1;
            }
            return UpdateUserCredits(uidlist, mountlist, creditsOperationType, pos);
        }

        /// <summary>
        /// �����û��б�,һ�θ��¶���û��Ļ���(�˷���ֻ��ɾ������ʱʹ�ù�)
        /// </summary>
        /// <param name="uidlist">�û�ID�б�</param>
        /// <param name="mountlist">�����Ĵ�����������¼�������ֵĴ���������ɾ����ɾһ�����ӣ�Ҳ����һ����������mountlistΪ1;ɾ���������ӣ��������ӷֱ�������������Ҳ�������β�����mountlistֵ��Ϊ2</param>
        /// <param name="creditsOperationType">���ֲ�������,�緢����</param>
        /// <param name="pos">�ӻ����־(����Ϊ��,����Ϊ��,ͨ��������1����-1)</param>
        public static int UpdateUserCredits(int[] uidlist, int[] mountlist, CreditsOperationType creditsOperationType, int pos)
        {
            ///���ݹ�ʽ�����û����ܻ���,������
            int reval = 0;
            for (int i = 0; i < uidlist.Length; i++)
            {
                if (uidlist[i] > 0)
                    reval = reval + UpdateUserExtCredits(uidlist[i], mountlist[i], creditsOperationType, pos, true);

                UserCredits.UpdateUserCredits(uidlist[i]);
            }
            return reval;
        }


        /// <summary>
        /// ���ݻ��ֻ�û����û�����Ӧ��ƥ����û������� (���û��ƥ������û��ǻ����û����򷵻�null)
        /// </summary>
        /// <param name="Credits">����</param>
        /// <returns>�û�������</returns>
        public static UserGroupInfo GetCreditsUserGroupId(float Credits)
        {
            List<UserGroupInfo> usergroupinfo = UserGroups.GetUserGroupList();
            UserGroupInfo tmpitem = null;

            UserGroupInfo maxCreditGroup = null;
            foreach (UserGroupInfo infoitem in usergroupinfo)
            {
                // �����û����������radminid����0
                if (infoitem.Radminid == 0 && infoitem.System == 0 && (Credits >= infoitem.Creditshigher && Credits <= infoitem.Creditslower))
                {
                    if (tmpitem == null || infoitem.Creditshigher > tmpitem.Creditshigher)
                        tmpitem = infoitem;
                }
                //���»���������ߵ��û���
                if (maxCreditGroup == null || maxCreditGroup.Creditshigher < infoitem.Creditshigher)
                    maxCreditGroup = infoitem;
            }

            if (maxCreditGroup != null && maxCreditGroup.Creditshigher < Credits)
                tmpitem = maxCreditGroup;

            return tmpitem == null ? new UserGroupInfo() : tmpitem;
        }



        /// <summary>
        /// �û���������ʱ�����û��Ļ���
        /// </summary>
        /// <param name="uid">�û�id</param>
        public static void UpdateUserCreditsByPostTopic(int uid)
        {
            UpdateUserExtCredits(uid, 1, CreditsOperationType.PostTopic, 1, false);
            UserCredits.UpdateUserCredits(uid);
        }

        /// <summary>
        /// �û���������ʱ�����û��Ļ���
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="values">���ֱ䶯ֵ,Ӧ��֤��һ������Ϊ8������,��Ӧ8����չ���ֵı䶯ֵ</param>
        public static void UpdateUserCreditsByPostTopic(int uid, float[] values)
        {
            UpdateUserExtCredits(uid, values, false);
            UserCredits.UpdateUserCredits(uid);
        }

        /// <summary>
        /// �û�����ظ�ʱ�����û��Ļ���
        /// </summary>
        /// <param name="uid">�û�id</param>
        public static void UpdateUserCreditsByPosts(int uid)
        {
            UpdateUserExtCredits(uid, 1, CreditsOperationType.PostReply, 1, false);
            UserCredits.UpdateUserCredits(uid);
        }

        /// <summary>
        /// �û�����ظ�ʱ�����û��Ļ���
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="values">���ֱ䶯ֵ,Ӧ��֤��һ������Ϊ8������,��Ӧ8����չ���ֵı䶯ֵ</param>
        public static void UpdateUserCreditsByPosts(int uid, float[] values)
        {
            UpdateUserExtCredits(uid, values, false);
            UserCredits.UpdateUserCredits(uid);
        }

        /// <summary>
        /// �û�����ظ�ʱ�����û��Ļ���
        /// </summary>
        /// <param name="uid">�û�id</param>
        public static void UpdateUserCreditsByDeletePosts(int uid)
        {
            UpdateUserExtCredits(uid, 1, CreditsOperationType.DeletePost, -1, false);
            UserCredits.UpdateUserCredits(uid);
        }


        /// <summary>
        /// �û��ϴ�����ʱ�����û��Ļ���
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="mount">�ϴ���������</param>
        public static int UpdateUserExtCreditsByUploadAttachment(int uid, int mount)
        {
            if (uid > 0 && mount > 0)
                return UpdateUserExtCredits(uid, mount, CreditsOperationType.UploadAttachment, 1, false);
            else
                return 0;
        }

        /// <summary>
        /// �û����ظ���ʱ�����û��Ļ���
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="mount">���ظ�������,��������2�����������˲���,��ô�˲���ֵӦΪ2</param>
        public static int UpdateUserExtCreditsByDownloadAttachment(int uid, int mount)
        {
            if (uid > 0 && mount > 0)
                return UpdateUserExtCredits(uid, mount, CreditsOperationType.DownloadAttachment, 1, false);
            else
                return -1;
        }

        /// <summary>
        /// �û����Ͷ���Ϣʱ�����û��Ļ���
        /// </summary>
        /// <param name="uid">�û�id</param>
        public static int UpdateUserCreditsBySendpms(int uid)
        {
            if (uid > 0)
            {
                int result = UpdateUserExtCredits(uid, 1, CreditsOperationType.SendMessage, 1, false);
                UserCredits.UpdateUserCredits(uid);
                return result;
            }
            else
                return -1;
        }


        /// <summary>
        /// �û�����ʱ�����û��Ļ���
        /// </summary>
        /// <param name="uid">�û�id</param>
        public static int UpdateUserCreditsBySearch(int uid)
        {
            int result = UpdateUserExtCredits(uid, 1, CreditsOperationType.Search, 1, false);
            UserCredits.UpdateUserCredits(uid);
            return result;
        }



        /// <summary>
        /// �û����׳ɹ�ʱ�����û��Ļ���
        /// </summary>
        /// <param name="uid">�û�id</param>
        public static int UpdateUserCreditsByTradefinished(int userid)
        {
            if (userid > 0)
            {
                UpdateUserExtCredits(userid, 1, CreditsOperationType.TradeSucceed, 1, false);
                return UserCredits.UpdateUserCredits(userid);
            }
            else
                return 0;
        }

        /// <summary>
        /// �û�����ͶƱʱ�����û��Ļ���
        /// </summary>
        /// <param name="uid">�û�id</param>
        public static void UpdateUserCreditsByVotepoll(int userid)
        {
            if (userid > 0)
            {
                UpdateUserExtCredits(userid, 1, CreditsOperationType.Vote, 1, false);
                UpdateUserCredits(userid);
            }
        }

        /// <summary>
        /// �û�����ע������û��Ļ���
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="mount"></param>
        public static void UpdateUserCreditsByInvite(int userid, int mount)
        {
            if (userid > 0)
            {
                UpdateUserExtCredits(userid, mount, CreditsOperationType.Invite, 1, false);
                UpdateUserCredits(userid);
            }
        }

        /// <summary>
        /// �����û�Id���¼����û�����
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <returns>�û�����</returns>
        public static int GetUserCreditsByUid(int uid)
        {
            ShortUserInfo shortUserInfo = Discuz.Data.Users.GetShortUserInfo(uid);
            if (shortUserInfo != null)
            {
                return GetUserCreditsByUserInfo(shortUserInfo);
            }
            return 0;
        }

        /// <summary>
        /// �����û���Ϣ���¼����û�����
        /// </summary>
        /// <param name="shortUserInfo">�û���Ϣ</param>
        /// <returns>�û�����</returns>
        public static int GetUserCreditsByUserInfo(ShortUserInfo shortUserInfo)
        {
            string ArithmeticStr = Scoresets.GetScoreCalFormula();

            if (Utils.StrIsNullOrEmpty(ArithmeticStr))
                return 0;

            ArithmeticStr = ArithmeticStr.Replace("digestposts", shortUserInfo.Digestposts.ToString());
            ArithmeticStr = ArithmeticStr.Replace("posts", shortUserInfo.Posts.ToString());
            ArithmeticStr = ArithmeticStr.Replace("oltime", shortUserInfo.Oltime.ToString());
            ArithmeticStr = ArithmeticStr.Replace("pageviews", shortUserInfo.Pageviews.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits1", shortUserInfo.Extcredits1.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits2", shortUserInfo.Extcredits2.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits3", shortUserInfo.Extcredits3.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits4", shortUserInfo.Extcredits4.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits5", shortUserInfo.Extcredits5.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits6", shortUserInfo.Extcredits6.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits7", shortUserInfo.Extcredits7.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits8", shortUserInfo.Extcredits8.ToString());

            object expression = Arithmetic.ComputeExpression(ArithmeticStr);
            return Utils.StrToInt(Math.Floor(Utils.StrToFloat(expression, 0)), 0);
        }
    } // end class
}
