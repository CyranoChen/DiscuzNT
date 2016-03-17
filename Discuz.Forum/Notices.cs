using System.Data;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
    /// <summary>
    /// 通知信息操作类
    /// </summary>
    public class Notices
    {
        /// <summary>
        /// 添加指定的通知信息
        /// </summary>
        /// <param name="noticeinfo">要添加的通知信息</param>
        /// <returns></returns>
        public static int CreateNoticeInfo(NoticeInfo noticeinfo)
        {
#if !DEBUG
            if (noticeinfo.Posterid == noticeinfo.Uid)
                return 0;
#endif
            int noticeId = Discuz.Data.Notices.CreateNoticeInfo(noticeinfo);
            if(noticeId > 0)
            {
                int olid = OnlineUsers.GetOlidByUid(noticeinfo.Uid);
                if (olid > 0)
                {
                    OnlineUsers.UpdateNewNotices(olid);
                }
            }
            return noticeId;
        }

        /// <summary>
        /// 删除通知(计划任务调用)
        /// </summary>
        /// <returns></returns>
        public static void DeleteNotice()
        {
            Discuz.Data.Notices.DeleteNotice();//删除指定天数内的通知
        }

        /// <summary>
        /// 获取指定用户id及通知类型的通知数
        /// </summary>
        /// <param name="uid">指定用户id</param>
        /// <param name="noticetype">通知类型</param>
        /// <returns></returns>
        public static int GetNoticeCountByUid(int uid, NoticeType noticetype)
        {
            return uid > 0 ? Discuz.Data.Notices.GetNoticeCountByUid(uid, noticetype) : 0;
        }

    
        /// <summary>
        /// 获取指定用户和分页下的通知
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>通知集合</returns>
        public static NoticeinfoCollection GetNoticeinfoCollectionByUid(int uid, NoticeType noticetype, int pageid, int pagesize)
        {
            return (uid > 0 && pageid > 0) ? Discuz.Data.Notices.GetNoticeinfoCollectionByUid(uid, noticetype, pageid, pagesize) : null;
        }

        /// <summary>
        /// 发送回复通知
        /// </summary>
        /// <param name="postinfo">回复信息</param>
        /// <param name="topicinfo">所属主题信息</param>
        /// <param name="replyuserid">回复的某一楼的作者</param>
        public static void SendPostReplyNotice(PostInfo postinfo, TopicInfo topicinfo, int replyuserid)
        {
            NoticeInfo noticeinfo = new NoticeInfo();

            noticeinfo.Note = Utils.HtmlEncode(string.Format("<a href=\"userinfo.aspx?userid={0}\">{1}</a> 给您回帖, <a href =\"showtopic.aspx?topicid={2}&postid={3}#{3}\">{4}</a>.", postinfo.Posterid, postinfo.Poster, topicinfo.Tid, postinfo.Pid, topicinfo.Title));
            noticeinfo.Type = NoticeType.PostReplyNotice;
            noticeinfo.New = 1;
            noticeinfo.Posterid = postinfo.Posterid;
            noticeinfo.Poster = postinfo.Poster;
            noticeinfo.Postdatetime = Utils.GetDateTime();
            noticeinfo.Fromid = topicinfo.Tid;
            noticeinfo.Uid = replyuserid;

            //当回复人与帖子作者不是同一人时，则向帖子作者发送通知
            if (postinfo.Posterid != replyuserid && replyuserid > 0)
            {
                Notices.CreateNoticeInfo(noticeinfo);
            }

            //当上面通知的用户与该主题作者不同，则还要向主题作者发通知
            if (postinfo.Posterid != topicinfo.Posterid && topicinfo.Posterid != replyuserid && topicinfo.Posterid > 0)
            {
                noticeinfo.Uid = topicinfo.Posterid;
                Notices.CreateNoticeInfo(noticeinfo);
            }
        }

        /// <summary>
        /// 获取指定字滤字符串所对应的通知类型
        /// </summary>
        /// <param name="filter">字滤字符串</param>
        /// <returns></returns>
        public static NoticeType GetNoticetype(string filter)
        {
            switch (filter)
            {
                case "spacecomment": //日志评论
                        return NoticeType.SpaceCommentNotice;

                case "albumcomment": //相册图片评论
                        return NoticeType.AlbumCommentNotice;

                case "postreply": //主题回复
                        return NoticeType.PostReplyNotice;

                case "goodstrade": //商品交易
                        return NoticeType.GoodsTradeNotice;

                case "goodsleaveword": //商品留言通知
                        return NoticeType.GoodsLeaveWordNotice;
                default:
                        return NoticeType.All;
            }
        }

        /// <summary>
        /// 获取指定用户和分页下的通知
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>通知集合</returns>
        public static int GetNewNoticeCountByUid(int uid)
        {
            return uid > 0 ? Discuz.Data.Notices.GetNewNoticeCountByUid(uid) : 0;
        }

        /// <summary>
        /// 更新指定用户的通知新旧状态
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="newtype">通知新旧状态(1:新通知 0:旧通知)</param>
        public static void UpdateNoticeNewByUid(int uid, int newtype)
        {
            if(uid > 0)
                Discuz.Data.Notices.UpdateNoticeNewByUid(uid, newtype);
        }

        /// <summary>
        /// 得到通知数
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="state">通知状态(0为已读，1为未读)</param>
        /// <returns></returns>
        public static int GetNoticeCount(int userid, int state)
        {
            return userid > 0 ? Discuz.Data.Notices.GetNoticeCount(userid, state) : 0;
        }

        /// <summary>
        /// 获得最新的通知ID
        /// </summary>
        /// <param name="Uid"></param>
        /// <returns></returns>
        public static int GetLatestNoticeID(int userid)
        {
            return userid > 0 ? Discuz.Data.Notices.GetLatestNoticeID(userid) : 0;
        }

        /// <summary>
        /// 获得指定用户的新通知
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public static NoticeInfo[] GetNewNotices(int userid)
        {
            return userid > 0 ? Discuz.Data.Notices.GetNewNotices(userid) : null;
        }
    }
}
