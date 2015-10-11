using System;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// 通知类型
    /// </summary>
    public enum NoticeType
    {
        /// <summary>
        /// (特殊值)用于(查询/删除)全部通知标识
        /// </summary>
        All = -1,
        /// <summary>
        /// 回帖通知
        /// </summary>
        PostReplyNotice = 1,
        /// <summary>
        /// 相册图片评论
        /// </summary>
        AlbumCommentNotice = 2,
        /// <summary>
        /// 空间日志评论
        /// </summary>
        SpaceCommentNotice = 3,
        /// <summary>
        /// 商品交易通知
        /// </summary>
        GoodsTradeNotice = 4,
        /// <summary>
        /// 商品留言通知
        /// </summary>
        GoodsLeaveWordNotice = 5,
        /// <summary>
        /// 禁止访问
        /// </summary>
        BanVisitNotice = 6,
        /// <summary>
        /// 禁止发言
        /// </summary>
        BanPostNotice = 7,
        /// <summary>
        /// 主题关注
        /// </summary>
        AttentionNotice = 8,
        /// <summary>
        /// 被执行管理操作
        /// </summary>
        TopicAdmin = 9,
        /// <summary>
        /// 应用通知
        /// </summary>
        //ApplicationNotice=10
    }


    /// <summary>
    /// 通知信息类
    /// </summary>
    public class NoticeInfo
    {
       
        private int m_nid;
        private int m_uid;
        private NoticeType m_type;
        private int m_new;
        private int m_posterid;
        private string m_poster;
        private string m_note;
        private string m_postdatetime;
        private int m_fromid;


        /// <summary>
        /// 通知ID
        /// </summary>
        public int Nid
        {
            set { m_nid = value; }
            get { return m_nid; }
        }
     
        /// <summary>
        /// 通知用户ID
        /// </summary>
        public int Uid
        {
            set { m_uid = value; }
            get { return m_uid; }
        }
      
        /// <summary>
        /// 通知类型
        /// </summary>
        public NoticeType Type
        {
            set { m_type = value; }
            get { return m_type; }
        }


        /// <summary>
        /// 标识id(和NoticeType绑定,如被回复帖子pid,被回复空间日志id)
        /// </summary>
        public int Fromid
        {
            get { return m_fromid; }
            set { m_fromid = value; }
        }
      
        ///<summary>
        ///通知是否为新:'1'为新通知,'0'为通知已读
        ///</summary>
        public int New
        {
            get { return m_new; }
            set { m_new = value; }
        }
       
        ///<summary>
        ///作者用户ID
        ///</summary>
        public int Posterid
        {
            get { return m_posterid; }
            set { m_posterid = value; }
        }
      
        ///<summary>
        ///发送通知的用户名
        ///</summary>
        public string Poster
        {
            get { return m_poster; }
            set { m_poster = value; }
        }
       
        ///<summary>
        ///通知内容
        ///</summary>
        public string Note
        {
            get { return m_note; }
            set { m_note = value; }
        }
        
        ///<summary>
        ///通知发送时间
        ///</summary>
        public string Postdatetime
        {
            get { return m_postdatetime; }
            set { m_postdatetime = value; }
        }
    }
}
