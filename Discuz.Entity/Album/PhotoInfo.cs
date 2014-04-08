using System;

namespace Discuz.Entity
{
	/// <summary>
	/// SpacePhotoInfo ��ժҪ˵����
	/// </summary>
	public class PhotoInfo
	{
		public PhotoInfo()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
        /// <summary>
        /// ��ƬID
        /// </summary>
		private int _photoid;
		public int Photoid
		{
			get { return _photoid; }
			set { _photoid = value; }
		}

        /// <summary>
        /// ���ID
        /// </summary>
		private int _albumid;
		public int Albumid
		{
			get { return _albumid; }
			set { _albumid = value; }
		}
        /// <summary>
        /// �û�ID
        /// </summary>
		private int _userid;
		public int Userid
		{
			get { return _userid; }
			set { _userid = value; }
		}
        /// <summary>
        /// �ļ���
        /// </summary>
		private string _filename;
		public string Filename
		{
			get { return _filename; }
			set { _filename = value; }
		}

        /// <summary>
        /// ������
        /// </summary>
		private string _attachment;
		public string Attachment
		{
			get { return _attachment; }
			set { _attachment = value; }
		}
        /// <summary>
        /// ��Ƭ��С
        /// </summary>
		private int _filesize;
		public int Filesize
		{
			get { return _filesize; }
			set { _filesize = value; }
		}
        /// <summary>
        /// ��Ƭ����
        /// </summary>
		private string _description;
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}
        /// <summary>
        /// ��Ƭ�ϴ�ʱ��
        /// </summary>
		private string _postdate;
		public string Postdate
		{
			get { return _postdate; }
			set { _postdate = value; }
		}

        /// <summary>
        /// ��Ƭ����
        /// </summary>
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        /// <summary>
        /// ��Ƭ�鿴����
        /// </summary>
        private int _views;

        public int Views
        {
            get { return _views; }
            set { _views = value; }
        }

        /// <summary>
        /// ��Ƭ����״̬
        /// </summary>
        private PhotoStatus _commentstatus = PhotoStatus.RegisteredUser;

        public PhotoStatus Commentstatus
        {
            get { return _commentstatus; }
            set { _commentstatus = value; }
        }

        /// <summary>
        /// ��Ƭ��ǩ
        /// </summary>
        private PhotoStatus _tagstatus = PhotoStatus.Buddy;

        public PhotoStatus Tagstatus
        {
            get { return _tagstatus; }
            set { _tagstatus = value; }
        }
        /// <summary>
        /// ���۴���
        /// </summary>
        private int _comments = 0;

        public int Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        /// <summary>
        /// �û���
        /// </summary>
        private string _username;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        /// �Ƿ���Ϊ����
        /// </summary>
        private int _isattachment;
        public int IsAttachment
        {
            get { return _isattachment; }
            set { _isattachment = value; }
        }




	}
}
