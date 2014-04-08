using System;

namespace Discuz.Entity
{

	/// <summary>
	/// ��̵��û���Ϣ������
	/// </summary>    
	public class ShortUserInfo
	{
		
		private int m_uid;	//�û�uid
		private string m_username;	//�û���
		private string m_nickname;	//�ǳ�
		private string m_password;	//�û�����
		private int m_spaceid; //���˿ռ�ID,0Ϊ�û���δ����ռ�;�������û��Ѿ�����,�ȴ�����Ա��ͨ,����ֵΪ��ͨ�Ժ����ʵSpaceid;�������û��Ѿ���ͨ��Spaceid
		private string m_secques;	//�û���ȫ������
		private int m_gender;	//�Ա�
        private int m_adminid;	//�û�����(1Ϊ����Ա��2Ϊ���棬3Ϊ������0Ϊ��ͨ�û�)
		private int m_groupid;	//�û���ID
		private int m_groupexpiry;	//�����ʱ��
		private string m_extgroupids;	//��չ�û���
		private string m_regip;	//ע��IP
		private string m_joindate;	//ע��ʱ��
		private string m_lastip;	//�ϴε�¼IP
		private string m_lastvisit;	//�ϴη���ʱ��
		private string m_lastactivity;	//���ʱ��
		private string m_lastpost;	//�����ʱ��
		private int m_lastpostid;	//�����id
		private string m_lastposttitle;	//���������
		private int m_posts;	//������
		private int m_digestposts;	//��������
		private int m_oltime;	//����ʱ��
		private int m_pageviews;	//ҳ�������
		private int m_credits;	//������
		private float m_extcredits1;	//��չ����1
		private float m_extcredits2;	//��չ����2
		private float m_extcredits3;	//��չ����3
		private float m_extcredits4;	//��չ����4
		private float m_extcredits5;	//��չ����5
		private float m_extcredits6;	//��չ����6
		private float m_extcredits7;	//��չ����7
		private float m_extcredits8;	//��չ����8
		//private int m_avatarshowid;	//ͷ��ID
		private string m_email = "";	//�ʼ���ַ
		private string m_bday;	//����
		private int m_sigstatus;	//�Ƿ�����ǩ��
		private int m_tpp;	//ÿҳ������
		private int m_ppp;	//ÿҳ����
		private int m_templateid;	//���ID
		private int m_pmsound;	//����Ϣ����
		private int m_showemail;	//�Ƿ���ʾ����
		private ReceivePMSettingType m_newsletter;	//�Ƿ������̳֪ͨ
		private int m_invisible;	//�Ƿ�����
		//private string m_timeoffset;	//ʱ��
		private int m_newpm;	//�Ƿ�������Ϣ
		private int m_newpmcount;	//�¶���Ϣ����
		private int m_accessmasks;	//�Ƿ�ʹ������Ȩ��
		private int m_onlinestate;	//����״̬, 1Ϊ����, 0Ϊ������       
        private string m_salt;//��������MD5���ֶ�



		///<summary>
		///�û�UID
		///</summary>
		public int Uid
		{
			get { return m_uid;}
			set { m_uid = value;}
		}
		///<summary>
		///�û���
		///</summary>
		public string Username
		{
			get { return m_username.Trim();}
			set { m_username = value;}
		}
		///<summary>
		///�û�����
		///</summary>
		public string Password
		{
            get { return string.IsNullOrEmpty(m_password) ? "" : m_password.Trim(); }
			set { m_password = value;}
		}

		///<summary>
		///�û��ռ�ID
		///</summary>
		public int Spaceid
		{
			get { return m_spaceid; }
			set { m_spaceid = value; }
		}

		///<summary>
		///�û���ȫ������
		///</summary>
		public string Secques
		{
            get { return string.IsNullOrEmpty(m_secques) ? "" : m_secques.Trim(); }
			set { m_secques = value;}
		}
		///<summary>
		///�Ա�
		///</summary>
		public int Gender
		{
			get { return m_gender;}
			set { m_gender = value;}
		}
		///<summary>
        ///�û�����(1Ϊ����Ա��2Ϊ���棬3Ϊ������0Ϊ��ͨ�û�)
		///</summary>
		public int Adminid
		{
			get { return m_adminid;}
			set { m_adminid = value;}
		}
		///<summary>
		///�û���ID
		///</summary>
		public int Groupid
		{
			get { return m_groupid;}
			set { m_groupid = value;}
		}
		///<summary>
		///�����ʱ��
		///</summary>
		public int Groupexpiry
		{
			get { return m_groupexpiry;}
			set { m_groupexpiry = value;}
		}
		///<summary>
		///��չ�û���
		///</summary>
		public string Extgroupids
		{
			get { return m_extgroupids;}
			set { m_extgroupids = value;}
		}
		///<summary>
		///ע��IP
		///</summary>
		public string Regip
		{
			get { return m_regip;}
			set { m_regip = value;}
		}
		///<summary>
		///ע��ʱ��
		///</summary>
		public string Joindate
		{
			get { return m_joindate;}
			set { m_joindate = value;}
		}
		///<summary>
		///�ϴε�¼IP
		///</summary>
		public string Lastip
		{
			get { return m_lastip;}
			set { m_lastip = value;}
		}
		///<summary>
		///�ϴη���ʱ��
		///</summary>
		public string Lastvisit
		{
			get { return m_lastvisit;}
			set { m_lastvisit = value;}
		}
		///<summary>
		///���ʱ��
		///</summary>
		public string Lastactivity
		{
			get { return m_lastactivity;}
			set { m_lastactivity = value;}
		}
		///<summary>
		///�����ʱ��
		///</summary>
		public string Lastpost
		{
			get { return m_lastpost;}
			set { m_lastpost = value;}
		}
		/// <summary>
		/// �����id
		/// </summary>
		public int Lastpostid
		{
			get { return m_lastpostid;}
			set { m_lastpostid = value;}
		}		
		/// <summary>
		/// ���������
		/// </summary>
		public string Lastposttitle
		{
			get { return m_lastposttitle;}
			set { m_lastposttitle = value;}
		}

		///<summary>
		///������
		///</summary>
		public int Posts
		{
			get { return m_posts;}
			set { m_posts = value;}
		}
		///<summary>
		///��������
		///</summary>
		public int Digestposts
		{
			get { return m_digestposts;}
			set { m_digestposts = value;}
		}
		///<summary>
		///����ʱ��
		///</summary>
		public int Oltime
		{
			get { return m_oltime;}
			set { m_oltime = value;}
		}
		///<summary>
		///ҳ�������
		///</summary>
		public int Pageviews
		{
			get { return m_pageviews;}
			set { m_pageviews = value;}
		}
		///<summary>
		///������
		///</summary>
		public int Credits
		{
			get { return m_credits;}
			set { m_credits = value;}
		}
		///<summary>
		///��չ����1
		///</summary>
		public float Extcredits1
		{
			get { return m_extcredits1;}
			set { m_extcredits1 = value;}
		}
		///<summary>
		///��չ����2
		///</summary>
		public float Extcredits2
		{
			get { return m_extcredits2;}
			set { m_extcredits2 = value;}
		}
		///<summary>
		///��չ����3
		///</summary>
		public float Extcredits3
		{
			get { return m_extcredits3;}
			set { m_extcredits3 = value;}
		}
		///<summary>
		///��չ����4
		///</summary>
		public float Extcredits4
		{
			get { return m_extcredits4;}
			set { m_extcredits4 = value;}
		}
		///<summary>
		///��չ����5
		///</summary>
		public float Extcredits5
		{
			get { return m_extcredits5;}
			set { m_extcredits5 = value;}
		}
		///<summary>
		///��չ����6
		///</summary>
		public float Extcredits6
		{
			get { return m_extcredits6;}
			set { m_extcredits6 = value;}
		}
		///<summary>
		///��չ����7
		///</summary>
		public float Extcredits7
		{
			get { return m_extcredits7;}
			set { m_extcredits7 = value;}
		}
		///<summary>
		///��չ����8
		///</summary>
		public float Extcredits8
		{
			get { return m_extcredits8;}
			set { m_extcredits8 = value;}
		}
		///<summary>
		///ͷ��ID
		///</summary>
        //public int Avatarshowid
        //{
        //    get { return m_avatarshowid;}
        //    set { m_avatarshowid = value;}
        //}
		///<summary>
		///�ʼ���ַ
		///</summary>
		public string Email
		{
			get { return m_email.Trim();}
			set { m_email = value;}
		}
		///<summary>
		///����
		///</summary>
		public string Bday
		{
			get { return m_bday.Trim();}
			set { m_bday = value;}
		}
		///<summary>
        ///�Ƿ�����ǩ��
		///</summary>
		public int Sigstatus
		{
			get { return m_sigstatus;}
			set { m_sigstatus = value;}
		}
		///<summary>
		///ÿҳ������
		///</summary>
		public int Tpp
		{
			get { return m_tpp;}
			set { m_tpp = value;}
		}
		///<summary>
		///ÿҳ����
		///</summary>
		public int Ppp
		{
			get { return m_ppp;}
			set { m_ppp = value;}
		}
		///<summary>
		///���ID
		///</summary>
		public int Templateid
		{
			get { return m_templateid;}
			set { m_templateid = value;}
		}
		///<summary>
		///����Ϣ����
		///</summary>
		public int Pmsound
		{
			get { return m_pmsound;}
			set { m_pmsound = value;}
		}
		///<summary>
		///�Ƿ���ʾ����
		///</summary>
		public int Showemail
		{
			get { return m_showemail;}
			set { m_showemail = value;}
		}
		///<summary>
		///�Ƿ������̳֪ͨ
		///</summary>
        public ReceivePMSettingType Newsletter
		{
			get { return m_newsletter;}
			set { m_newsletter = value;}
		}
		///<summary>
		///�Ƿ�����
		///</summary>
		public int Invisible
		{
			get { return m_invisible;}
			set { m_invisible = value;}
		}
		/*
		///<summary>
		///ʱ��
		///</summary>
		public string Timeoffset
		{
			get { return m_timeoffset;}
			set { m_timeoffset = value;}
		}*/
		///<summary>
		///�Ƿ�������Ϣ
		///</summary>
		public int Newpm
		{
			get { return m_newpm;}
			set { m_newpm = value;}
		}
		///<summary>
		///�¶���Ϣ����
		///</summary>
		public int Newpmcount
		{
			get { return m_newpmcount;}
			set { m_newpmcount = value;}
		}
		///<summary>
		///�Ƿ�ʹ������Ȩ��
		///</summary>
		public int Accessmasks
		{
			get { return m_accessmasks;}
			set { m_accessmasks = value;}
		}
		/// <summary>
		/// ����״̬, 1Ϊ����, 0Ϊ������
		/// </summary>
		public int Onlinestate
		{
			get { return m_onlinestate;}
			set { m_onlinestate = value;}
		}

		///<summary>
		///�ǳ�
		///</summary>
		public string Nickname
		{
			get { return m_nickname.Trim();}
			set { m_nickname = value;}
		}

        public string Salt
        {
            get { return m_salt; }
            set { m_salt = value; }
        }
	
	}
	
	/// <summary>
	/// �����û���Ϣ������
	/// </summary>
	public class UserInfo : ShortUserInfo
	{
		
		//����Ϊ��չ��Ϣ
		private string m_website;	//��վ
		private string m_icq;	//icq����
		private string m_qq;	//qq����
		private string m_yahoo;	//yahoo messenger�ʺ�
		private string m_msn;	//msn messenger�ʺ�
		private string m_skype;	//skype�ʺ�
		private string m_location;	//����
		private string m_customstatus;	//�Զ���ͷ��
        //private string m_avatar;	//ͷ��
        //private int m_avatarwidth;	//ͷ����
        //private int m_avatarheight;	//ͷ��߶�
		private string m_medals; //ѫ���б�
		private string m_bio;	//���ҽ���
		private string m_signature;	//ǩ��
		private string m_sightml;	//ǩ��Html(�Զ�ת���õ�)
		private string m_authstr;	//��֤��
		private string m_authtime;	//��֤����������
		private byte m_authflag;	//��֤��ʹ�ñ�־(0 δʹ��,1 �û�������֤���û���Ϣ����, 2 �û������һ�)
        private string m_realname;  //�û�ʵ��
        private string m_idcard;    //�û����֤����
        private string m_mobile;    //�û��ƶ��绰
        private string m_phone;     //�û��̶��绰
        private string m_ignorepm;  //����Ϣ�����б�

		///<summary>
		///��վ
		///</summary>
		public string Website
		{
			get { return m_website.Trim();}
			set { m_website = value;}
		}
		///<summary>
		///icq����
		///</summary>
		public string Icq
		{
			get { return m_icq.Trim();}
			set { m_icq = value;}
		}
		///<summary>
		///qq����
		///</summary>
		public string Qq
		{
			get { return m_qq.Trim();}
			set { m_qq = value;}
		}
		///<summary>
		///yahoo messenger�ʺ�
		///</summary>
		public string Yahoo
		{
			get { return m_yahoo.Trim();}
			set { m_yahoo = value;}
		}
		///<summary>
		///msn messenger�ʺ�
		///</summary>
		public string Msn
		{
			get { return m_msn.Trim();}
			set { m_msn = value;}
		}
		///<summary>
		///skype�ʺ�
		///</summary>
		public string Skype
		{
			get { return m_skype;}
			set { m_skype = value;}
		}
		///<summary>
		///����
		///</summary>
		public string Location
		{
			get { return m_location.Trim();}
			set { m_location = value;}
		}
		///<summary>
		///�Զ���ͷ��
		///</summary>
		public string Customstatus
		{
			get { return m_customstatus;}
			set { m_customstatus = value;}
		}
		///<summary>
		///ͷ��
		///</summary>
        //public string Avatar
        //{
        //    get { return m_avatar.Trim();}
        //    set { m_avatar = value;}
        //}
		///<summary>
		///ͷ����
		///</summary>
        //public int Avatarwidth
        //{
        //    get { return m_avatarwidth;}
        //    set { m_avatarwidth = value;}
        //}
		///<summary>
		///ͷ��߶�
		///</summary>
        //public int Avatarheight
        //{
        //    get { return m_avatarheight;}
        //    set { m_avatarheight = value;}
        //}
		/// <summary>
		/// ѫ���б�
		/// </summary>
		public string  Medals
		{
			get { return m_medals;}
			set { m_medals = value;}
		}
		///<summary>
		///���ҽ���
		///</summary>
		public string Bio
		{
			get { return m_bio.Trim();}
			set { m_bio = value;}
		}
		///<summary>
		///ǩ��
		///</summary>
		public string Signature
		{
			get { return m_signature.Trim();}
			set { m_signature = value;}
		}
		///<summary>
		///ǩ��Html(�Զ�ת���õ�)
		///</summary>
		public string Sightml
		{
			get { return m_sightml.Trim();}
			set { m_sightml = value;}
		}
		
		
		///<summary>
		///��֤��
		///</summary>
		public string Authstr
		{
			get { return m_authstr;}
			set { m_authstr = value;}
		}
		
		///<summary>
		///��֤��
		///</summary>
		public string Authtime
		{
			get { return m_authtime;}
			set { m_authtime = value;}
		}
		
		///<summary>
		///��֤��
		///</summary>
		public byte Authflag
		{
			get { return m_authflag;}
			set { m_authflag = value;}
		}

        /// <summary>
        /// �û�ʵ��
        /// </summary>
        public string Realname
        {
            get { return m_realname; }
            set { m_realname = value; }
        }

        /// <summary>
        /// �û����֤����
        /// </summary>
        public string Idcard
        {
            get { return m_idcard; }
            set { m_idcard = value; }
        }

        /// <summary>
        /// �û��ƶ��绰
        /// </summary>
        public string Mobile
        {
            get { return m_mobile; }
            set { m_mobile = value; }
        }

        /// <summary>
        /// �û��̶��绰
        /// </summary>
        public string Phone
        {
            get { return m_phone; }
            set { m_phone = value; }
        }

        /// <summary>
        /// ����Ϣ�����б�
        /// </summary>
        public string Ignorepm
        {
            get { return m_ignorepm; }
            set { m_ignorepm = value; }
        }

        public UserInfo Clone()
        {
            return (UserInfo)this.MemberwiseClone();
        }
	}
}
