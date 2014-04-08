using System;

namespace Discuz.Entity
{
	/// <summary>
	/// SpacePhotoInfo 的摘要说明。
	/// </summary>
	public class PhotoInfo
	{
		public PhotoInfo()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
        /// <summary>
        /// 照片ID
        /// </summary>
		private int _photoid;
		public int Photoid
		{
			get { return _photoid; }
			set { _photoid = value; }
		}

        /// <summary>
        /// 相册ID
        /// </summary>
		private int _albumid;
		public int Albumid
		{
			get { return _albumid; }
			set { _albumid = value; }
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
        /// 文件名
        /// </summary>
		private string _filename;
		public string Filename
		{
			get { return _filename; }
			set { _filename = value; }
		}

        /// <summary>
        /// 附件名
        /// </summary>
		private string _attachment;
		public string Attachment
		{
			get { return _attachment; }
			set { _attachment = value; }
		}
        /// <summary>
        /// 照片大小
        /// </summary>
		private int _filesize;
		public int Filesize
		{
			get { return _filesize; }
			set { _filesize = value; }
		}
        /// <summary>
        /// 照片介绍
        /// </summary>
		private string _description;
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}
        /// <summary>
        /// 照片上传时间
        /// </summary>
		private string _postdate;
		public string Postdate
		{
			get { return _postdate; }
			set { _postdate = value; }
		}

        /// <summary>
        /// 照片标题
        /// </summary>
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        /// <summary>
        /// 照片查看此俗
        /// </summary>
        private int _views;

        public int Views
        {
            get { return _views; }
            set { _views = value; }
        }

        /// <summary>
        /// 照片评论状态
        /// </summary>
        private PhotoStatus _commentstatus = PhotoStatus.RegisteredUser;

        public PhotoStatus Commentstatus
        {
            get { return _commentstatus; }
            set { _commentstatus = value; }
        }

        /// <summary>
        /// 照片标签
        /// </summary>
        private PhotoStatus _tagstatus = PhotoStatus.Buddy;

        public PhotoStatus Tagstatus
        {
            get { return _tagstatus; }
            set { _tagstatus = value; }
        }
        /// <summary>
        /// 评论次数
        /// </summary>
        private int _comments = 0;

        public int Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        private string _username;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        /// 是否作为附件
        /// </summary>
        private int _isattachment;
        public int IsAttachment
        {
            get { return _isattachment; }
            set { _isattachment = value; }
        }




	}
}
