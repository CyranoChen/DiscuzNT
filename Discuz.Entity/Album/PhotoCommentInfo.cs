using System;
using System.Text;

namespace Discuz.Entity
{
    public class PhotoCommentInfo
    {
        private int _commentID;
        /// <summary>
        /// 照片评论ID
        /// </summary>
        public int Commentid
        {
            get { return _commentID; }
            set { _commentID = value; }
        }

        /// <summary>
        /// 帖子ID
        /// </summary>
        private int _postID;
        public int Photoid
        {
            get { return _postID; }
            set { _postID = value; }
        }
        /// <summary>
        /// 用户名ID
        /// </summary>
        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        private int _userid;
        public int Userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// 用户IP
        /// </summary>
        private string _ip;
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

        /// <summary>
        /// 评论时间
        /// </summary>
        private DateTime _postDateTime;
        public DateTime Postdatetime
        {
            get { return _postDateTime; }
            set { _postDateTime = value; }
        }
        /// <summary>
        /// 评论内容
        /// </summary>
        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
        /// <summary>
        /// 上级评论
        /// </summary>
        private int _parentID;
        public int Parentid
        {
            get { return _parentID; }
            set { _parentID = value; }
        }　　　　
    }
}
