using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// ��̳���������
	/// </summary>
	public class Announcements
	{
		/// <summary>
		/// ���ȫ��ָ��ʱ����ڵĹ����б�
		/// </summary>
		/// <param name="startDateTime">��ʼʱ��</param>
		/// <param name="endDateTime">����ʱ��</param>
		/// <returns>�����б�</returns>
		public static DataTable GetAnnouncementList(string starttime, string endtime)
		{
			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
			DataTable dt = cache.RetrieveObject("/Forum/AnnouncementList") as DataTable;
			
			if(dt == null)
			{
                dt = Data.Announcements.GetAnnouncementList();
                cache.AddObject("/Forum/AnnouncementList", dt);
			}
			return dt;
		}

		/// <summary>
		/// ���ȫ��ָ��ʱ����ڵĵ�һ�������б�
		/// </summary>
		/// <param name="startDateTime">��ʼʱ��</param>
		/// <param name="endDateTime">����ʱ��</param>
		/// <returns>�����б�</returns>
		public static DataTable GetSimplifiedAnnouncementList(string starttime, string endtime)
		{
			return GetSpecificAnnouncementList(starttime, endtime, -1);
		}

        /// <summary>
        /// ���ȫ��ָ��ʱ����ڵĵ�һ�������б�
        /// </summary>
        /// <param name="startDateTime">��ʼʱ��</param>
        /// <param name="endDateTime">����ʱ��</param>
        /// <returns>�����б�</returns>
        public static DataTable GetSimplifiedAnnouncementList(string starttime)
        {
            return GetSpecificAnnouncementList(starttime, "2999-01-01 00:00:00", -1);
        }

		/// <summary>
		/// ���ȫ��ָ��ʱ����ڵ�ǰn�������б�
		/// </summary>
		/// <param name="startDateTime">��ʼʱ��</param>
		/// <param name="endDateTime">����ʱ��</param>
		/// <param name="maxcount">����¼��,С��0����ȫ��</param>
		/// <returns>�����б�</returns>
		public static DataTable GetSpecificAnnouncementList(string starttime, string endtime, int maxcount)
		{
			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
			DataTable dt = cache.RetrieveObject("/Forum/SimplifiedAnnouncementList") as DataTable;
			
			if(dt == null)
			{
                dt = Data.Announcements.GetAnnouncementList(maxcount);
				cache.AddObject("/Forum/SimplifiedAnnouncementList", dt);
			}
			return dt;
		}

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAnnouncementList()
        {
           return Data.Announcements.GetAnnouncementList();
        }

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <param name="num">ÿҳ��¼��</param>
        /// <param name="pageid">ҳ��</param>
        /// <returns>��ҳ�����б�</returns>
        public static DataTable GetAnnouncementList(int num,int pageid)
        {
            return (num > 0 && pageid > 0) ? Data.Announcements.GetAnnouncementList(num, pageid) : new DataTable();
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="displayOrder">�������</param>
        /// <param name="aId">����id</param>
        public static void UpdateAnnouncementDisplayOrder(string displayorder, string[] hid, int userid , int useradminid)
        {
            DataTable announcementlist = GetAnnouncementList();

            for (int i = 0; i < displayorder.Split(',').Length; i++)
            {
                AnnouncementInfo announcementInfo = Forum.Announcements.GetAnnouncement(TypeConverter.StrToInt(hid[i]));
                if ((announcementInfo.Posterid > 0 && announcementInfo.Posterid == userid) || useradminid == 1)
                {
                    if (announcementInfo.Displayorder.ToString() != displayorder[i].ToString())//������ʾ˳��
                        Data.Announcements.UpdateAnnouncementDisplayOrder(TypeConverter.StrToInt(displayorder.Split(',')[i]), TypeConverter.ObjectToInt(announcementlist.Rows[i]["id"]));
                }
                else
                    continue;
            }
       }

        /// <summary>
        /// ��ӹ���
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="userId">�û�id</param>
        /// <param name="subject">��������</param>
        /// <param name="displayOrder">�������</param>
        /// <param name="startDateTime">��ʼʱ��</param>
        /// <param name="endDateTime">����ʱ��</param>
        /// <param name="message">��������</param>
        public static void CreateAnnouncement(string username, int userid, string subject, int displayorder, string starttime, string endtime, string message)
        {
            AnnouncementInfo announcementInfo = new AnnouncementInfo();
            announcementInfo.Title = subject;
            announcementInfo.Poster = username;
            announcementInfo.Posterid = userid;
            announcementInfo.Displayorder = displayorder;
            DateTime dt;
            DateTime.TryParse(starttime, out dt);
            announcementInfo.Starttime = dt;
            DateTime.TryParse(endtime, out dt);
            announcementInfo.Endtime = dt;
            announcementInfo.Message = message;

            Data.Announcements.CreateAnnouncement(announcementInfo);
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="aId">����id</param>
        /// <returns></returns>
        public static AnnouncementInfo GetAnnouncement(int aid)
        {
            return aid > 0 ? Data.Announcements.GetAnnouncement(aid) : null;
        }

        /// <summary>
        /// ���¹���
        /// </summary>
        /// <param name="aId">����id</param>
        /// <param name="username">�û���</param>
        /// <param name="subject">��������</param>
        /// <param name="displayOrder">�������</param>
        /// <param name="startDateTime">��ʼʱ��</param>
        /// <param name="endDateTime">����ʱ��</param>
        /// <param name="message">��������</param>
        public static void UpdateAnnouncement(AnnouncementInfo announcementInfo)
        {
            if(announcementInfo.Id > 0)
                Data.Announcements.UpdateAnnouncement(announcementInfo);
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="aidlist">���ŷָ���id�ַ���</param>
        public static void DeleteAnnouncements(string aidlist)
        {
            Data.Announcements.DeleteAnnouncements(aidlist);
            //�Ƴ����滺��
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AnnouncementList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/SimplifiedAnnouncementList");       
        }

        /// <summary>
        /// ��������ȡ����
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable GetAnnouncementsByCondition(string condition)
        {
            return Data.Announcements.GetAnnouncementsByCondition(condition);
        }
	}
}
