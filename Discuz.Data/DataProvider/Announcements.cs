using System;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Common;

namespace Discuz.Data
{
    /// <summary>
    /// 论坛公告操作类
    /// </summary>
    public class Announcements
    {
        /// <summary>
        /// 添加公告
        /// </summary>
        /// <param name="announcementInfo">公告对象</param>
        public static void CreateAnnouncement(AnnouncementInfo announcementInfo)
        {
            DatabaseProvider.GetInstance().CreateAnnouncement(announcementInfo);
        }

        /// <summary>
        /// 批量删除公告
        /// </summary>
        /// <param name="aidlist">逗号分隔的id列表字符串</param>
        public static void DeleteAnnouncements(string aidlist)
        {
            DatabaseProvider.GetInstance().DeleteAnnouncements(aidlist);
        }

        /// <summary>
        /// 更新公告排序
        /// </summary>
        /// <param name="displayOrder">排序序号</param>
        /// <param name="aId">公告id</param>
        public static void UpdateAnnouncementDisplayOrder(int displayOrder, int aId)
        {
            DatabaseProvider.GetInstance().UpdateAnnouncementDisplayOrder(displayOrder, aId);
        }

        /// <summary>
        /// 更新公告
        /// </summary>
        /// <param name="announcementInfo">公告对象</param>
        public static void UpdateAnnouncement(AnnouncementInfo announcementInfo)
        {
            DatabaseProvider.GetInstance().UpdateAnnouncement(announcementInfo);
        }

        /// <summary>
        /// 获取公告
        /// </summary>
        /// <param name="aId">公告id</param>
        /// <returns></returns>
        public static AnnouncementInfo GetAnnouncement(int aid)
        {
            AnnouncementInfo announcementInfo = null; 
            IDataReader reader = DatabaseProvider.GetInstance().GetAnnouncement(aid);
            if (reader.Read())
            { 
                announcementInfo = LoadSingleAnnouncementInfo(reader);
            }
            reader.Close();
            return announcementInfo;
        }

        /// <summary>
        /// 获得全部指定时间段内的前n条公告列表
        /// </summary>
        /// <param name="maxCount">最大记录数,小于0返回全部</param>
        /// <returns>公告列表</returns>
        public static DataTable GetAnnouncementList(int maxCount)
        {
            return DatabaseProvider.GetInstance().GetAnnouncementList(maxCount);
        }

        /// <summary>
        /// 获取公告列表
        /// </summary>
        /// <returns>公告列表</returns>
        public static DataTable GetAnnouncementList()
        {
            return DatabaseProvider.GetInstance().GetAnnouncements();
        }

        /// <summary>
        /// 获取公告列表
        /// </summary>
        /// <param name="num"></param>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public static DataTable GetAnnouncementList(int num, int pageId)
        {
            return DatabaseProvider.GetInstance().GetAnnouncements(num, pageId);
        }


        /// <summary>
        /// 更新公告的创建者用户名
        /// </summary>
        /// <param name="uid">uid</param>
        /// <param name="newUserName">新用户名</param>
        public static void UpdateAnnouncementPoster(int uid, string newUserName)
        {
            DatabaseProvider.GetInstance().UpdateAnnouncementPoster(uid, newUserName);
        }

        /// <summary>
        /// 按条件获取公告
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable GetAnnouncementsByCondition(string condition)
        {
            return DatabaseProvider.GetInstance().GetAnnouncementsByCondition(condition);
        }

        #region Private Methods
        /// <summary>
        /// 装载实体对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static AnnouncementInfo LoadSingleAnnouncementInfo(IDataReader reader)
        {
            AnnouncementInfo announcementInfo = new AnnouncementInfo();
            announcementInfo.Id = TypeConverter.ObjectToInt(reader["id"]);
            announcementInfo.Poster = reader["poster"].ToString();
            announcementInfo.Posterid = TypeConverter.ObjectToInt(reader["posterid"]);
            announcementInfo.Title = reader["title"].ToString();
            announcementInfo.Displayorder = TypeConverter.ObjectToInt(reader["displayorder"]);
            announcementInfo.Starttime = Convert.ToDateTime(reader["starttime"].ToString());
            announcementInfo.Endtime = Convert.ToDateTime(reader["endtime"].ToString());
            announcementInfo.Message = reader["message"].ToString();
            return announcementInfo;
        }
        #endregion


    }
}
