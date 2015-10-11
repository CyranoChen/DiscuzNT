using System;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Common;
using Discuz.Config;

namespace Discuz.Data
{
    public class Notices
    {
        /// <summary>
        /// 添加指定的通知信息
        /// </summary>
        /// <param name="noticeinfo">要添加的通知信息</param>
        /// <returns></returns>
        public static int CreateNoticeInfo(NoticeInfo noticeinfo)
        {
            return DatabaseProvider.GetInstance().CreateNoticeInfo(noticeinfo);
        }

        /// <summary>
        /// 更新指定的通知信息(注释无用方法 2011-04-12)
        /// </summary>
        /// <param name="noticeinfo">要更新的通知信息</param>
        /// <returns></returns>
        //public static bool UpdateNoticeInfo(NoticeInfo noticeinfo)
        //{
        //    return DatabaseProvider.GetInstance().UpdateNoticeInfo(noticeinfo);
        //}

        /// <summary>
        /// 删除指定通知id的信息
        /// </summary>
        /// <param name="nid">指定的通知id</param>
        /// <returns></returns>
        public static bool DeleteNoticeByNid(int nid)
        {
            return DatabaseProvider.GetInstance().DeleteNoticeByNid(nid);
        }

        /// <summary>
        /// 删除指定用户id的通知信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static bool DeleteNoticeByUid(int uid)
        {
            return DatabaseProvider.GetInstance().DeleteNoticeByUid(uid);
        }

        /// <summary>
        /// 删除通知(计划任务调用)
        /// </summary>
        /// <returns></returns>
        public static void DeleteNotice()
        {
            DatabaseProvider.GetInstance().DeleteNotice(NoticeType.All, GeneralConfigs.GetConfig().Notificationreserveddays);//删除指定天数内的通知
        }


        /// <summary>
        /// 获取指定通知id的信息
        /// </summary>
        /// <param name="nid">通知id</param>
        /// <returns>通知信息</returns>
        public static NoticeInfo GetNoticeInfo(int nid)
        {
            NoticeInfo noticeinfo = new NoticeInfo();
            IDataReader idatareader = DatabaseProvider.GetInstance().GetNoticeByNid(nid);
            if (idatareader.Read())
            {
                LoadSingleNoticeInfo(idatareader);
            }
            idatareader.Close();
            return noticeinfo;
        }

        #region Helper

        private static NoticeInfo LoadSingleNoticeInfo(IDataReader reader)
        {
            NoticeInfo noticeinfo = new NoticeInfo();
            noticeinfo.Nid = TypeConverter.ObjectToInt(reader["nid"], 0);
            noticeinfo.Uid = TypeConverter.ObjectToInt(reader["uid"], 0);
            noticeinfo.Type = (NoticeType)TypeConverter.ObjectToInt(reader["type"], 0);
            noticeinfo.New = TypeConverter.ObjectToInt(reader["new"], 0);
            noticeinfo.Posterid = TypeConverter.ObjectToInt(reader["posterid"], 0);
            noticeinfo.Poster = reader["poster"].ToString().Trim();
            noticeinfo.Note = Utils.HtmlDecode(reader["note"].ToString());
            noticeinfo.Postdatetime = reader["postdatetime"].ToString();
            return noticeinfo;
        }
        #endregion

        /// <summary>
        /// 获得最新的通知ID
        /// </summary>
        /// <param name="Uid"></param>
        /// <returns></returns>
        public static int GetLatestNoticeID(int userid)
        {
            int latestnid = 0;
            IDataReader reader = DatabaseProvider.GetInstance().GetNoticeByUid(userid, NoticeType.All, 1, 1);
            if (reader.Read())
            {
                latestnid = TypeConverter.ObjectToInt(reader["nid"], 0);
            }
            reader.Close();
            return latestnid;
        }

        /// <summary>
        /// 将某一类通知更改为未读状态
        /// </summary>
        /// <param name="type"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int ReNewNotice(int type, int uid)
        {
            return DatabaseProvider.GetInstance().ReNewNotice(type, uid);
        }

        /// <summary>
        /// 获得指定用户的新通知
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static NoticeInfo[] GetNewNotices(int userid)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetNewNotices(userid);
            System.Collections.Generic.List<NoticeInfo> list = new System.Collections.Generic.List<NoticeInfo>();
            while (reader.Read())
            {
                list.Add(LoadSingleNoticeInfo(reader));
            }
            reader.Close();
            return list.ToArray();

        }

        /// <summary>
        /// 获取指定用户的所有通知
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>通知集合</returns>
        public static NoticeinfoCollection GetNoticeInfoCollectionByUid(int uid)
        {
            return NoticeinfoCollectionDTO(DatabaseProvider.GetInstance().GetNoticeByUid(uid,0));
        }


        /// <summary>
        /// 获取指定用户的所有通知
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>通知集合</returns>
        public static NoticeinfoCollection GetNoticeinfoCollectionByUid(int uid, NoticeType noticetype)
        {
            return NoticeinfoCollectionDTO(DatabaseProvider.GetInstance().GetNoticeByUid(uid, noticetype));
        }

        /// <summary>
        /// 获取指定用户id及通知类型的通知数
        /// </summary>
        /// <param name="uid">指定用户id</param>
        /// <param name="noticetype">通知类型</param>
        /// <returns></returns>
        public static int GetNoticeCountByUid(int uid, NoticeType noticetype)
        {
            return DatabaseProvider.GetInstance().GetNoticeCountByUid(uid, noticetype);
        }

        /// <summary>
        /// 获取指定用户和分页下的通知
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>通知集合</returns>
        public static NoticeinfoCollection GetNoticeinfoCollectionByUid(int uid, NoticeType noticetype, int pageid, int pagesize)
        {
            return NoticeinfoCollectionDTO(DatabaseProvider.GetInstance().GetNoticeByUid(uid, noticetype, pageid, pagesize));
        }

        /// <summary>
        /// 获取指定用户的所有通知
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>通知集合</returns>
        private static NoticeinfoCollection NoticeinfoCollectionDTO(IDataReader idatareader)
        {
            NoticeinfoCollection noticeinfocoll = new NoticeinfoCollection();
            while (idatareader.Read())
            {
                noticeinfocoll.Add(LoadSingleNoticeInfo(idatareader));
            }
            idatareader.Close();
            return noticeinfocoll;
        }

        /// <summary>
        /// 获取指定用户和分页下的通知
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>通知集合</returns>
        public static int GetNewNoticeCountByUid(int uid)
        {
            return DatabaseProvider.GetInstance().GetNewNoticeCountByUid(uid);
        }

        /// <summary>
        /// 更新指定用户的通知新旧状态
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="newtype">通知新旧状态(1:新通知 0:旧通知)</param>
        public static void UpdateNoticeNewByUid(int uid, int newtype)
        {
            DatabaseProvider.GetInstance().UpdateNoticeNewByUid(uid, newtype);
        }

        /// <summary>
        /// 得到通知数
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="state">通知状态(0为已读，1为未读)</param>
        /// <returns></returns>
        public static int GetNoticeCount(int userid, int state)
        {
            return DatabaseProvider.GetInstance().GetNoticeCount(userid, state);
        }
    }
}
