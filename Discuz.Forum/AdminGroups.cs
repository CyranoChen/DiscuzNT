using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Entity;

namespace Discuz.Forum
{
    /// <summary>
    /// �����������
    /// </summary>
    public class AdminGroups
    {
        /// <summary>
        /// ��õ�ָ����������Ϣ
        /// </summary>
        /// <returns>��������Ϣ</returns>
        public static AdminGroupInfo[] GetAdminGroupList()
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            AdminGroupInfo[] admingroupArray = cache.RetrieveObject("/Forum/AdminGroupList") as AdminGroupInfo[];
            if (admingroupArray == null)
            {
                admingroupArray = Discuz.Data.AdminGroups.GetAdminGroupList();
                cache.AddObject("/Forum/AdminGroupList", admingroupArray);                
            }
            return admingroupArray;
        }

        /// <summary>
        /// ��õ�ָ����������Ϣ
        /// </summary>
        /// <param name="admingid">������ID</param>
        /// <returns>����Ϣ</returns>
        public static AdminGroupInfo GetAdminGroupInfo(int admingid)
        {
            // ���������id����0
            if (admingid > 0)
            {
                AdminGroupInfo[] admingroupArray = GetAdminGroupList();
                foreach (AdminGroupInfo admingroup in admingroupArray)
                {
                    // ������ڸù������򷵻ظ�����Ϣ
                    if (admingroup.Admingid == admingid)
                        return admingroup;
                }
            }
            // ��������ڸ����򷵻�null
            return null;
        }


        /// <summary>
        /// ���ù�������Ϣ
        /// </summary>
        /// <param name="__admingroupsInfo">��������Ϣ</param>
        /// <returns>���ļ�¼��</returns>
        public static int SetAdminGroupInfo(AdminGroupInfo admingroupsInfo, int userGroupId)
        {
            //�����м�¼ʱ
            if (AdminGroups.GetAdminGroupInfo(userGroupId) != null)
            {
                //������Ӧ�Ĺ�����
                return Discuz.Data.AdminGroups.SetAdminGroupInfo(admingroupsInfo);
            }
            else
            {
                //������Ӧ���û���
                return CreateAdminGroupInfo(admingroupsInfo);
            }
        }

        /// <summary>
        /// ����һ���µĹ�������Ϣ
        /// </summary>
        /// <param name="__admingroupsInfo">Ҫ��ӵĹ�������Ϣ</param>
        /// <returns>���ļ�¼��</returns>
        public static int CreateAdminGroupInfo(AdminGroupInfo admingroupsInfo)
        {
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AdminGroupList");

            return Discuz.Data.AdminGroups.CreateAdminGroupInfo(admingroupsInfo);
        }

        /// <summary>
        /// ɾ��ָ���Ĺ�������Ϣ
        /// </summary>
        /// <param name="admingid">������ID</param>
        /// <returns>���ļ�¼��</returns>
        public static int DeleteAdminGroupInfo(short admingid)
        {
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AdminGroupList");
            return Discuz.Data.AdminGroups.DeleteAdminGroupInfo(admingid);
        }

        public static void ChangeUserAdminidByGroupid(int radminId, int groupId)
        {
            if (radminId > 0 && groupId > 0)
                Discuz.Data.AdminGroups.ChangeUserAdminidByGroupid(radminId, groupId);
        }
    }//class end
}
