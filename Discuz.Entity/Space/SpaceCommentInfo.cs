using System;

namespace Discuz.Entity
{
　　/// <summary>
	/// SpaceCommentsInfo 的摘要说明。
	/// </summary>
	public class SpaceCommentInfo
　　{
		public SpaceCommentInfo()
		{}
        /// <summary>
        /// 评论ID
        /// </summary>
		private int _commentID;
		public int CommentID
		{
			get { return _commentID; }
			set { _commentID = value; }
		}
        /// <summary>
        /// 日志ID
        /// </summary>
　　　　private int _postID;
		public int PostID
		{
			get { return _postID; }
			set { _postID = value; }
		}
        /// <summary>
        /// 评论人
        /// </summary>
　　　　private string _author;
		public string Author
		{
			get { return _author; }
			set { _author = value; }
		}
        /// <summary>
        /// 评论人EAMIL
        /// </summary>
　　　　private string _email;
		public string Email
		{
			get { return _email; }
			set { _email = value; }
		}

        /// <summary>
        /// 评论人网址
        /// </summary>
　　　　private string _url;
		public string Url
		{
			get { return _url; }
			set { _url = value; }
		}
        /// <summary>
        /// 评论人IP
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
		public DateTime PostDateTime
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
        /// 上级评论ID
        /// </summary>
　　　　private int _parentID;
		public int ParentID
		{
			get { return _parentID; }
			set { _parentID = value; }
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
        /// 评论标题
        /// </summary>
		private string _posttitle;
		public string PostTitle
		{
			get { return _posttitle; }
			set { _posttitle = value; }
		}
		
　　　　
	}
}
