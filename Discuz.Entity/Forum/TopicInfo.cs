using System;

namespace Discuz.Entity
{
	/// <summary>
	/// ������Ϣ������
	/// </summary>
    [Serializable]
	public class TopicInfo
    {
        #region ��Ա����
        private int m_tid;	//����tid
		private int m_fid;	//���fid
        private string m_forumname = ""; //�������
		private int m_iconid;	//����ͼ��id
		private int m_typeid;	//�������id
		private int m_readperm = 0;	//�Ķ�Ȩ��
		private int m_price;	//������ۼ۸����
		private string m_poster;	//����
		private int m_posterid;	//����uid
		private string m_title = "";	//����
		private string m_postdatetime;	//����ʱ��
		private string m_lastpost;	//���ظ�ʱ��
		private int m_lastpostid;	//���ظ�����ID
		private string m_lastposter;	//���ظ��û���
		private int m_lastposterid;	//���ظ��û���ID
		private int m_views = 0;	//�鿴��
		private int m_replies = 0;	//�ظ���
		private int m_displayorder;	//>0Ϊ�ö�,<0����ʾ,==0����   -1Ϊ����վ   -2����� -3Ϊ������
		private string m_highlight = "";	//�������ʶ���
		private int m_digest = 0;	//��������,1~3
		private int m_rate = 0;	//���ַ���
		private int m_hide = 0;	//�Ƿ�Ϊ�ظ��ɼ���
        //private int m_poll;	//�Ƿ���ͶƱ��
		private int m_attachment = 0;	//�Ƿ��и���,2ͼƬ��1�ļ�
		private int m_moderated = 0;	//�Ƿ�ִ�й������
		private int m_closed = 0;	//�Ƿ�ر�,�����ֵ>1,ֵ����ת��Ŀ�������tid
		private int m_magic;	//ħ��id
        private int m_identify; //����id
        private byte m_special;//��������, 0=��ͨ����, 1=ͶƱ��, 2=���ڽ��е�������, 3=������������, 4=������
        private int m_attention;
        #endregion ��Ա����

        #region ����

        public int Attention
        {
            get { return m_attention; }
            set { m_attention = value; }
        }


        ///<summary>
		///����tid
		///</summary>
		public int Tid
		{
			get { return m_tid;}
			set { m_tid = value;}
		}
        /// <summary>
        /// �������
        /// </summary>
        public string Forumname
        {
            get { return m_forumname; }
            set { m_forumname = value; }
        }
		///<summary>
		///���fid
		///</summary>
		public int Fid
		{
			get { return m_fid;}
			set { m_fid = value;}
		}
		///<summary>
		///����ͼ��id
		///</summary>
		public int Iconid
		{
			get { return m_iconid;}
			set { m_iconid = value;}
		}
		///<summary>
		///�������id
		///</summary>
		public int Typeid
		{
			get { return m_typeid;}
			set { m_typeid = value;}
		}
		///<summary>
		///�Ķ�Ȩ��
		///</summary>
		public int Readperm
		{
			get { return m_readperm;}
			set { m_readperm = value;}
		}
		///<summary>
		///������ۼ۸����
		///</summary>
		public int Price
		{
			get { return m_price;}
			set { m_price = value;}
		}
		///<summary>
		///����
		///</summary>
		public string Poster
		{
			get { return m_poster.Trim();}
			set { m_poster = value;}
		}
		///<summary>
		///����uid
		///</summary>
		public int Posterid
		{
			get { return m_posterid;}
			set { m_posterid = value;}
		}
		///<summary>
		///����
		///</summary>
		public string Title
		{
			get { return m_title.Trim();}
			set { m_title = value;}
		}
		///<summary>
		///����ʱ��
		///</summary>
		public string Postdatetime
		{
			get { return m_postdatetime;}
			set { m_postdatetime = value;}
		}
		///<summary>
		///���ظ�ʱ��
		///</summary>
		public string Lastpost
		{
			get { return m_lastpost;}
			set { m_lastpost = value;}
		}
		///<summary>
		///���ظ�����ID
		///</summary>
		public int Lastpostid
		{
			get { return m_lastpostid;}
			set { m_lastpostid = value;}
		}
		///<summary>
		///���ظ��û���
		///</summary>
		public string Lastposter
		{
			get { return m_lastposter;}
			set { m_lastposter = value;}
		}

		///<summary>
		///���ظ��û���ID
		///</summary>
		public int Lastposterid
		{
			get { return m_lastposterid;}
			set { m_lastposterid = value;}
		}
		///<summary>
		///�鿴��
		///</summary>
		public int Views
		{
			get { return m_views;}
			set { m_views = value;}
		}
		///<summary>
		///�ظ���
		///</summary>
		public int Replies
		{
			get { return m_replies;}
			set { m_replies = value;}
		}
		///<summary>
		///>0Ϊ�ö�,С��0����ʾ,==0����(-1,����վ��-2,δ���)
		///</summary>
		public int Displayorder
		{
			get { return m_displayorder;}
			set { m_displayorder = value;}
		}
		///<summary>
		///�������ʶ���
		///</summary>
		public string Highlight
		{
			get { return m_highlight;}
			set { m_highlight = value;}
		}
		///<summary>
		///��������,1~3
		///</summary>
		public int Digest
		{
			get { return m_digest;}
			set { m_digest = value;}
		}
		///<summary>
		///���ַ���
		///</summary>
		public int Rate
		{
			get { return m_rate;}
			set { m_rate = value;}
		}
		///<summary>
		///�Ƿ�Ϊ�ظ��ɼ���
		///</summary>
		public int Hide
		{
			get { return m_hide;}
			set { m_hide = value;}
		}
		///<summary>
		///�Ƿ���ͶƱ��
		///</summary>
        //public int Poll
        //{
        //    get { return m_poll;}
        //    set { m_poll = value;}
        //}
		///<summary>
		///�Ƿ��и���
		///</summary>
		public int Attachment
		{
			get { return m_attachment;}
			set { m_attachment = value;}
		}
		///<summary>
		///�Ƿ�ִ�й������
		///</summary>
		public int Moderated
		{
			get { return m_moderated;}
			set { m_moderated = value;}
		}

		///<summary>
		///�Ƿ�ر�,�����ֵ>1,ֵ����ת��Ŀ�������tid(1Ϊ�ر�,0�ǲ��ر�)
		///</summary>
		public int Closed
		{
			get { return m_closed;}
			set { m_closed = value;}
		}

		///<summary>
        ///ħ��id,���ո���λ/htmltitle(1λ)/magic(3λ)/tag(1λ)/�Ժ���չ��δ֪λ���� �ķ�ʽ���洢
		///</summary>
		public int Magic
		{
			get { return m_magic;}
			set { m_magic = value;}
		}

        /// <summary>
        /// ����Id
        /// </summary>
        public int Identify
        {
            get { return m_identify; }
            set { m_identify = value; }
        }

        /// <summary>
        /// 0=��ͨ����, 1=ͶƱ��, 2=���ڽ��е�������, 3=������������, 4=������
        /// </summary>
        public byte Special
        {
            get { return m_special; }
            set { m_special = value; }
        }
        #endregion ����

        #region ��������
        private string m_folder = string.Empty;

        private string m_topictypename = string.Empty;

        public string Folder
        {
            get { return m_folder; }
            set { m_folder = value; }
        }

        public string Topictypename
        {
            get { return m_topictypename; }
            set { m_topictypename = value; }
        }
        #endregion
    }

}
