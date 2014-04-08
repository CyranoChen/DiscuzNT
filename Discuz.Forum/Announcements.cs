using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// 论坛公告操作类
	/// </summary>
	public class Announcements
	{
		/// <summary>
		/// 获得全部指定时间段内的公告列表
		/// </summary>
		/// <param name="startDateTime">开始时间</param>
		/// <param name="endDateTime">结束时间</param>
		/// <returns>公告列表</returns>
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
		/// 获得全部指定时间段内的第一条公告列表
		/// </summary>
		/// <param name="startDateTime">开始时间</param>
		/// <param name="endDateTime">结束时间</param>
		/// <returns>公告列表</returns>
		public static DataTable GetSimplifiedAnnouncementList(string starttime, string endtime)
		{
			return GetSpecificAnnouncementList(starttime, endtime, -1);
		}

        /// <summary>
        /// 获得全部指定时间段内的第一条公告列表
        /// </summary>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        /// <returns>公告列表</returns>
        public static DataTable GetSimplifiedAnnouncementList(string starttime)
        {
            return GetSpecificAnnouncementList(starttime, "2999-01-01 00:00:00", -1);
        }

		/// <summary>
		/// 获得全部指定时间段内的前n条公告列表
		/// </summary>
		/// <param name="startDateTime">开始时间</param>
		/// <param name="endDateTime">结束时间</param>
		/// <param name="maxcount">最大记录数,小于0返回全部</param>
		/// <returns>公告列表</returns>
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
        /// 获取公告列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAnnouncementList()
        {
           return Data.Announcements.GetAnnouncementList();
        }

        /// <summary>
        /// 获取公告列表
        /// </summary>
        /// <param name="num">每页记录数</param>
        /// <param name="pageid">页号</param>
        /// <returns>该页公告列表</returns>
        public static DataTable GetAnnouncementList(int num,int pageid)
        {
            return (num > 0 && pageid > 0) ? Data.Announcements.GetAnnouncementList(num, pageid) : new DataTable();
        }

        /// <summary>
        /// 更新排序
        /// </summary>
        /// <param name="displayOrder">排序序号</param>
        /// <param name="aId">公告id</param>
        public static void UpdateAnnouncementDisplayOrder(string displayorder, string[] hid, int userid , int useradminid)
        {
            DataTable announcementlist = GetAnnouncementList();

            for (int i = 0; i < displayorder.Split(',').Length; i++)
            {
                AnnouncementInfo announcementInfo = Forum.Announcements.GetAnnouncement(TypeConverter.StrToInt(hid[i]));
                if ((announcementInfo.Posterid > 0 && announcementInfo.Posterid == userid) || useradminid == 1)
                {
                    if (announcementInfo.Displayorder.ToString() != displayorder[i].ToString())//更新显示顺序
                        Data.Announcements.UpdateAnnouncementDisplayOrder(TypeConverter.StrToInt(displayorder.Split(',')[i]), TypeConverter.ObjectToInt(announcementlist.Rows[i]["id"]));
                }
                else
                    continue;
            }
       }

        /// <summary>
        /// 添加公告
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="userId">用户id</param>
        /// <param name="subject">公告主题</param>
        /// <param name="displayOrder">排序序号</param>
        /// <param name="startDateTime">起始时间</param>
        /// <param name="endDateTime">结束时间</param>
        /// <param name="message">公告内容</param>
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
        /// 获取公告
        /// </summary>
        /// <param name="aId">公告id</param>
        /// <returns></returns>
        public static AnnouncementInfo GetAnnouncement(int aid)
        {
            return aid > 0 ? Data.Announcements.GetAnnouncement(aid) : null;
        }

        /// <summary>
        /// 更新公告
        /// </summary>
        /// <param name="aId">公告id</param>
        /// <param name="username">用户名</param>
        /// <param name="subject">公告主题</param>
        /// <param name="displayOrder">排序序号</param>
        /// <param name="startDateTime">起始时间</param>
        /// <param name="endDateTime">结束时间</param>
        /// <param name="message">公告内容</param>
        public static void UpdateAnnouncement(AnnouncementInfo announcementInfo)
        {
            if(announcementInfo.Id > 0)
                Data.Announcements.UpdateAnnouncement(announcementInfo);
        }

        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="aidlist">逗号分隔的id字符串</param>
        public static void DeleteAnnouncements(string aidlist)
        {
            Data.Announcements.DeleteAnnouncements(aidlist);
            //移除公告缓存
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AnnouncementList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/SimplifiedAnnouncementList");       
        }

        /// <summary>
        /// 按条件获取公告
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable GetAnnouncementsByCondition(string condition)
        {
            return Data.Announcements.GetAnnouncementsByCondition(condition);
        }
	}
}
