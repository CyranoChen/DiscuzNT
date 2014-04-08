using System;

namespace Discuz.Entity
{
　　/// <summary>
	/// SpacePostsInfo 的摘要说明。
	/// </summary>
	public class SpacePostInfo
　　{
        /// <summary>
        /// 日志ID
        /// </summary>
		private int _postid;
		public int Postid
		{
			get { return _postid; }
			set { _postid = value; }
		}
		
        /// <summary>
        /// 发帖人
        /// </summary>
　　　　private string _author;
		public string Author
		{
			get { return _author; }
			set { _author = value; }
		}
		
        /// <summary>
        /// 用户ID
        /// </summary>
　　　　private int _uid;
		public int Uid
		{
			get { return _uid; }
			set { _uid = value; }
		}
		
        /// <summary>
        /// 发帖时间
        /// </summary>
　　　　private DateTime _postdatetime;
		public DateTime Postdatetime
		{
			get { return _postdatetime; }
			set { _postdatetime = value; }
		}
		/// <summary>
		/// 内容
		/// </summary>
　　　　private string _content;
		public string Content
		{
			get { return _content; }
			set { _content = value; }
		}

        /// <summary>
        /// 标题
        /// </summary>
　　　　private string _title;
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

        /// <summary>
        /// 日志分类
        /// </summary>
　　　　private string _category;
		public string Category
		{
			get { return _category; }
			set { _category = value; }
		}

        /// <summary>
        /// 日志状态
        /// </summary>
　　　　private int _postStatus;
		public int PostStatus
		{
			get { return _postStatus; }
			set { _postStatus = value; }
		}

        /// <summary>
        /// 评论状态
        /// </summary>
　　　　private int _commentStatus;
		public int CommentStatus
		{
			get { return _commentStatus; }
			set { _commentStatus = value; }
		}

        /// <summary>
        /// 内容更行时间
        /// </summary>
　　　　private DateTime _postUpdateTime;
		public DateTime PostUpDateTime
		{
			get { return _postUpdateTime; }
			set { _postUpdateTime = value; }
		}
		
　　　　private int _commentcount;
		public int Commentcount
		{
			get { return _commentcount; }
			set { _commentcount = value; }
		}
        /// <summary>
        /// 内容查看次数
        /// </summary>
		private int _views;
		public int Views
		{
			get { return _views; }
			set { _views = value; }
		}
　　　　
	}

    public class SpaceShortPostInfo
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        private int _postID;
        public int Postid
        {
            get { return _postID; }
            set { _postID = value; }
        }

        /// <summary>
        /// 发帖人
        /// </summary>
        private string _author;
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        private int _uid;
        public int Uid
        {
            get { return _uid; }
            set { _uid = value; }
        }

        /// <summary>
        /// 发帖时间
        /// </summary>
        private DateTime _postDateTime;
        public DateTime Postdatetime
        {
            get { return _postDateTime; }
            set { _postDateTime = value; }
        }
        /// <summary>
        /// 标题
        /// </summary>
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// 日志被评论次数
        /// </summary>
        private int _commentCount;
        public int Commentcount
        {
            get { return _commentCount; }
            set { _commentCount = value; }
        }
        /// <summary>
        /// 内容查看次数
        /// </summary>
        private int _views;
        public int Views
        {
            get { return _views; }
            set { _views = value; }
        }

    }
}
