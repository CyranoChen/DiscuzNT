using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using System.Text;
using Discuz.Entity;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    /// <summary>
    /// �û��������
    /// </summary>
    public class UserGroups
    {
        /// <summary>
        /// ����û�������
        /// </summary>
        /// <returns>�û�������</returns>
        public static List<UserGroupInfo> GetUserGroupList()
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            List<UserGroupInfo> userGruopInfoList = cache.RetrieveObject("/Forum/UserGroupList") as List<UserGroupInfo>;

            if (userGruopInfoList == null)
            {
                userGruopInfoList = Discuz.Data.UserGroups.GetUserGroupList();
                cache.AddObject("/Forum/UserGroupList", userGruopInfoList);
            }
            return userGruopInfoList;
        }


        /// <summary>
        /// ��ȡָ�������Ϣ
        /// </summary>
        /// <param name="groupid">��id</param>
        /// <returns>����Ϣ</returns>
        public static UserGroupInfo GetUserGroupInfo(int groupid)
        {
            List<UserGroupInfo> userGroupInfoList = GetUserGroupList();
            UserGroupInfo info = userGroupInfoList.Find(delegate(UserGroupInfo i) { return i.Groupid == groupid; });
            if (info != null)
            {
                return info;
            }
            else
            {
                // ������Ҳ�����Ϊ�ο�
                return userGroupInfoList.Find(delegate(UserGroupInfo i) { return i.Groupid == 7; });
            }
        }


        /// <summary>
        /// ������ʱ�û�����ֱ�
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateGroupScoreTable()
        {
            DataTable templateDT = new DataTable("templateDT");

            templateDT.Columns.Add("id", Type.GetType("System.Int32"));
            templateDT.Columns.Add("available", Type.GetType("System.Boolean")); //�Ƿ��������ֶ�
            templateDT.Columns.Add("ScoreCode", Type.GetType("System.Int32"));//���ִ���
            templateDT.Columns.Add("ScoreName", Type.GetType("System.String"));//��������
            templateDT.Columns.Add("Min", Type.GetType("System.String"));//������Сֵ
            templateDT.Columns.Add("Max", Type.GetType("System.String"));//�������ֵ
            templateDT.Columns.Add("MaxInDay", Type.GetType("System.String"));//24Сʱ���������
            templateDT.Columns.Add("Options", Type.GetType("System.String"));//options HTML���� 

            //����м���Ĭ������
            for (int rowcount = 0; rowcount < 8; rowcount++)
            {
                DataRow dr = templateDT.NewRow();
                dr["id"] = rowcount + 1;
                dr["available"] = false;
                dr["ScoreCode"] = rowcount + 1;
                dr["ScoreName"] = "";
                dr["Min"] = "";
                dr["Max"] = "";
                dr["MaxInDay"] = "";
                templateDT.Rows.Add(dr);
            }

            //ͨ��CONFIG�ļ��õ���ص�ScoreName��������
            DataRow scoresetname = Scoresets.GetScoreSet().Rows[0];
            for (int count = 0; count < 8; count++)
            {
                if ((!Utils.StrIsNullOrEmpty(scoresetname[count + 2].ToString())) && (scoresetname[count + 2].ToString().Trim() != "0"))
                    templateDT.Rows[count]["ScoreName"] = scoresetname[count + 2].ToString().Trim();
            }
            return templateDT;
        }

        /// <summary>
        /// ͨ����ID�õ���������ַ�Χ,����������򷵻ؿձ�
        /// </summary>
        /// <param name="groupid">��ID</param>
        /// <returns>���ַ�Χ</returns>
        public static DataTable GroupParticipateScore(int groupid)
        {
            DataTable dt = Discuz.Data.UserGroups.GetUserGroupRateRange(groupid);
            //���û���δ������������ַ�Χʱ���ؿձ�
            if (dt.Rows.Count == 0)
                return null;
            //��������ʼ����ṹ
            DataTable templateDT = CreateGroupScoreTable();
            if (Utils.StrIsNullOrEmpty(dt.Rows[0][0].ToString()))
                return templateDT;

            //�����ݿ��еļ�¼������װ���Ĭ������
            int i = 0;
            foreach (string raterangestr in dt.Rows[0][0].ToString().Trim().Split('|'))
            {
                if (raterangestr.Trim() != "")
                {
                    string[] scoredata = raterangestr.Split(',');
                    //�ж��Ƿ��������ֶε������ж�
                    if (scoredata[1].Trim() == "True")
                        templateDT.Rows[i]["available"] = true;

                    //���������ֶ�
                    templateDT.Rows[i]["Min"] = scoredata[4].Trim();
                    templateDT.Rows[i]["Max"] = scoredata[5].Trim();
                    templateDT.Rows[i]["MaxInDay"] = scoredata[6].Trim();
                }
                i++;
            }
            return templateDT;
        }

        /// <summary>
        /// ͨ����ID��UID�õ���������ַ�Χ,����������򷵻ؿձ�
        /// </summary>
        /// <param name="uid">�û�ID</param>
        /// <param name="gid">�û����</param>
        /// <returns>ID��UID��������ַ�Χ</returns>
        public static DataTable GroupParticipateScore(int uid, int gid)
        {
            DataTable dt = GroupParticipateScore(gid);
            int[] extcredits = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            StringBuilder sb = new StringBuilder();
            int offset = 1;
            if (dt != null)
            {
                extcredits = Discuz.Data.UserGroups.GroupParticipateScore(uid);

                int max = 0;
                int min = 0;
                int maxInDay = 0;
                int maxSelect = 0;

                DataRow dr = null;
                for (int count = dt.Rows.Count - 1; count >= 0; count--)
                {
                    dr = dt.Rows[count];

                    max = TypeConverter.ObjectToInt(dr["Max"]);
                    min = TypeConverter.ObjectToInt(dr["Min"]);
                    maxInDay = TypeConverter.ObjectToInt(dr["MaxInDay"]);
                    maxInDay = maxInDay - extcredits[TypeConverter.ObjectToInt(dr["ScoreCode"])];
                    dr["MaxInDay"] = maxInDay;
                    maxSelect = max > maxInDay ? maxInDay : max;

                    if (!Convert.ToBoolean(dr["available"]) || maxInDay <= 0)
                    {
                        dr.Delete();
                        continue;
                    }

                    offset = TypeConverter.ObjectToInt(Math.Abs(Math.Ceiling((maxSelect - min) / 10.0)));
                    offset = offset <= 0 ? 1 : offset;
                    sb.Remove(0, sb.Length);
                    for (int i = maxSelect; i >= TypeConverter.ObjectToFloat(dr["Min"]); i -= offset)
                    {
                        if (i == 0)
                            continue;
                        if (Math.Abs(i) <= maxInDay)
                            sb.AppendFormat("\n<li>{0}{1}</li>", i > 0 ? "+" : "", i);
                    }
                    dr["Options"] = sb.ToString();
                }
                dt.AcceptChanges();
            }

            if (dt == null)
                dt = new DataTable();

            return dt;
        }

        /// <summary>
        /// ��������û��������\��������ۼ�
        /// </summary>
        /// <param name="usergroupinfo"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static int CheckUserGroupMaxPrice(UserGroupInfo usergroupinfo, int price)
        {
            if (price <= 0)
                return 0;

            if (price > 0 && price <= usergroupinfo.Maxprice)
                return price;
            else
                return usergroupinfo.Maxprice;
        }

        /// <summary>
        /// ��ȡ���groupid
        /// </summary>
        /// <returns></returns>
        public static int GetMaxUserGroupId()
        {
            List<UserGroupInfo> list = GetUserGroupList();
            int maxUserGroupId = 0;
            foreach (UserGroupInfo userGroupInfo in list)
            {
                if (userGroupInfo.Groupid > maxUserGroupId)
                    maxUserGroupId = userGroupInfo.Groupid;
            }
            return maxUserGroupId;
        }

        /// <summary>
        /// ��ȡ�������б�
        /// </summary>
        /// <returns></returns>
        public static List<UserGroupInfo> GetAdminUserGroup()
        {
            //List<UserGroupInfo> list = GetUserGroupList();
            //List<UserGroupInfo> adminList = new List<UserGroupInfo>();
            //foreach (UserGroupInfo userGroupInfo in list)
            //{
            //    if (userGroupInfo.Radminid > 0)
            //        adminList.Add(userGroupInfo);
            //}
            //return adminList;
            return GetUserGroupByAdminIdList("1,2,3");
        }

        /// <summary>
        /// ��ȡ��������������б�
        /// </summary>
        /// <returns></returns>
        public static List<UserGroupInfo> GetAdminAndSpecialGroup()
        {
            return GetUserGroupByAdminIdList("-1,1,2,3");
        }

        /// <summary>
        /// ��ȡָ�������û����б�
        /// </summary>
        /// <param name="adminIdList"></param>
        /// <returns></returns>
        private static List<UserGroupInfo> GetUserGroupByAdminIdList(string adminIdList)
        {
            List<UserGroupInfo> list = GetUserGroupList();
            List<UserGroupInfo> adminList = new List<UserGroupInfo>();
            foreach (UserGroupInfo userGroupInfo in list)
            {
                if (Utils.InArray(userGroupInfo.Radminid.ToString(), adminIdList))
                    adminList.Add(userGroupInfo);
            }
            return adminList;
        }

        /// <summary>
        /// �����û������������Ϣ
        /// </summary>
        /// <param name="raterange">����������Ϣ</param>
        /// <param name="groupid">�û���Id</param>
        public static void UpdateUserGroupRaterange(string raterange, int groupid)
        {
            Data.UserGroups.UpdateUserGroupRaterange(raterange, groupid);
        }

        /// <summary>
        /// �����û�����Ϣ
        /// </summary>
        /// <param name="info">�û�����Ϣ</param>
        public static void UpdateUserGroup(UserGroupInfo info)
        {
            Data.UserGroups.UpdateUserGroup(info);
        }

        /// <summary>
        /// ɾ���û���
        /// </summary>
        /// <param name="groupid">�û���Id</param>
        public static void DeleteUserGroupInfo(int groupid)
        {
            Data.UserGroups.DeleteUserGroupInfo(groupid);
        }

        /// <summary>
        /// ����ȫ���û�����Id
        /// </summary>
        /// <param name="sourceGroupId">Դ��Id</param>
        /// <param name="targetGroupId">Ŀ����Id</param>
        public static void ChangeAllUserGroupId(int sourceGroupId, int targetGroupId)
        {
            Data.UserGroups.ChangeAllUserGroupId(sourceGroupId, targetGroupId);
        }

        /// <summary>
        /// ��ȡ��ָ��id���û��������ͨ�û���
        /// </summary>
        /// <param name="groupid">�û���</param>
        /// <returns></returns>
        public static DataTable GetUserGroupExceptGroupid(int groupid)
        {
            return Data.UserGroups.GetUserGroupExceptGroupid(groupid);
        }

        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAdminGroups()
        {
            return Data.UserGroups.GetAdminGroups();
        }

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <returns></returns>
        public static DataTable GetOnlineList()
        {
            return Data.UserGroups.GetOnlineList();
        }

        /// <summary>
        /// �������߱�
        /// </summary>
        /// <param name="groupid">�û���ID</param>
        /// <param name="displayOrder">���</param>
        /// <param name="img">ͼƬ</param>
        /// <param name="title">����</param>
        /// <returns></returns>
        public static int UpdateOnlineList(int groupid, int displayorder, string img, string title)
        {
            return Data.UserGroups.UpdateOnlineList(groupid, displayorder, img, title);
        }

        /// <summary>
        /// ��ȡ���ַ�Χ
        /// </summary>
        /// <param name="scoreid"></param>
        /// <returns></returns>
        public static DataTable GetRateRange(int scoreid)
        {
            return Data.UserGroups.GetRateRange(scoreid);
        }

        /// <summary>
        /// �������ַ�Χ
        /// </summary>
        /// <param name="raterange"></param>
        /// <param name="groupid"></param>
        public static void UpdateRateRange(string raterange, int groupid)
        {
            Data.UserGroups.UpdateRateRange(raterange, groupid);
        }

        /// <summary>
        /// ��ȡ���ο���֮����û�������
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUserGroupWithOutGuestTitle()
        {
            return Data.UserGroups.GetUserGroupWithOutGuestTitle();
        }

        /// <summary>
        /// ��ȡ�û����б�
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUserGroupForDataTable()
        {
            return Data.UserGroups.GetUserGroupForDataTable();
        }

        /// <summary>
        /// ��ȡ�����û���
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCreditUserGroup()
        {
            return Data.UserGroups.GetCreditUserGroup();
        }

        /// <summary>
        /// �ж��û����Ƿ�Ϊ�����û���
        /// </summary>
        /// <param name="groupid">�û���id</param>
        /// <returns>��/��</returns>
        public static bool IsCreditUserGroup(int groupid)
        {
            //UserGroupInfo usergroupInfo = GetUserGroupList().Find(
            //    delegate(UserGroupInfo u)
            //    {
            //        return u.Radminid == 0 && u.System == 0 && u.Groupid == groupid;
            //    }
            //    );
            ////�����Ϊ�գ�˵�������ڻ����û����
            //return usergroupInfo != null;
            return IsCreditUserGroup(GetUserGroupInfo(groupid));
        }
        /// <summary>
        /// �ж��û����Ƿ�Ϊ�����û���
        /// </summary>
        /// <param name="usergroup">�û�����Ϣ</param>
        /// <returns>��/��</returns>
        public static bool IsCreditUserGroup(UserGroupInfo usergroup)
        {
            if (usergroup.Radminid == 0 && usergroup.System == 0)
                return true;
            return false;
        }
        /// <summary>
        /// ��ȡ�����û���
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSpecialUserGroup()
        {
            return Data.UserGroups.GetSpecialUserGroup();
        }
    }
}
