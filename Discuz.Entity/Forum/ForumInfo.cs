using System;


namespace Discuz.Entity
{
	/// <summary>
	/// ���������
	/// </summary>
    [Serializable]
	public class ForumInfo
	{
		private int m_fid;	//��̳fid
		private int m_parentid;	//����̳���ϼ���̳��ֱ���̳���ϼ���̳������fid
		private int m_layer;	//��̳���
		private string m_pathlist = ""; //��̳��������·����html���Ӵ���
		private string m_parentidlist = ""; //��̳��������·��id�б�
		private int m_subforumcount; //��̳����������̳����
		private string m_name = "";	//��̳����
		private int m_status;	//�Ƿ���ʾ
		private int m_colcount;	//���ø���̳������̳���б�ʱ�ּ�����ʾ
		private int m_displayorder;	//��ʾ˳��
		private int m_templateid;	//���id,0ΪĬ��
		private int m_topics;	//������
		private int m_curtopics;	//������
		private int m_posts;	//������
		private int m_todayposts;	//���շ���
		private string m_lastpost;	//��󷢱�����
		private string m_lastposter; //��󷢱���û���
		private int m_lastposterid; //��󷢱���û�id
		private int m_lasttid; //��󷢱����ӵ�����id
		private string m_lasttitle; //��󷢱�����ӱ���
		private int m_allowsmilies;	//����ʹ�ñ����
		private int m_allowrss;	//����ʹ��Rss
		private int m_allowhtml;	//����Html����
		private int m_allowbbcode;	//����Discuz!NT����
		private int m_allowimgcode;	//����[img]����
		private int m_allowblog;	//�����������ΪBlog
		private int m_istrade;	//�Ƿ��ǽ��װ��
        private int m_allowpostspecial;	//��������������
        private int m_allowspecialonly;	//����������������      
		private int m_alloweditrules;	//��������༭��̳����
		private int m_allowthumbnail;	//����showforumҳ���������ͼ
        private int m_allowtag;     //����ʹ�ñ�ǩ        
		private int m_recyclebin;	//�򿪻���վ
		private int m_modnewposts;	//������Ҫ���
        private int m_modnewtopics; //��������Ҫ���
		private int m_jammer;	//��������Ӹ�����,��ֹ���⸴��
		private int m_disablewatermark;	//��ֹ�����Զ�ˮӡ
		private int m_inheritedmod;	//�̳��ϼ���̳�����İ����趨
		private int m_autoclose;	//�����Զ��ر�����,��λΪ��
		//
		private string m_description = string.Empty;	//��̳����
		private string m_password;	//���ʱ���̳������,����Ϊ��������
		private string m_icon;	//��̳ͼ��,��ʾ����ҳ��̳�б��
		private string m_postcredits;	//��������ֲ���
		private string m_replycredits;	//���ظ����ֲ���
		private string m_redirect;	//ָ���ⲿ���ӵĵ�ַ
		private string m_attachextensions;	//�����ڱ���̳�ϴ��ĸ�������,����ΪĬ��
		private string m_moderators = "";	//�����б�(������ʾʹ��,����¼ʵ��Ȩ��)
		private string m_rules;	//�������
		private string m_topictypes;	//�������
		private string m_viewperm = "";	//���Ȩ���趨,��ʽΪ groupid1,groupid2,...
		private string m_postperm;	//������Ȩ���趨,��ʽΪ groupid1,groupid2,...
		private string m_replyperm;	//���ظ�Ȩ���趨,��ʽΪ groupid1,groupid2,...
		private string m_getattachperm;	//���ظ���Ȩ���趨,��ʽΪ groupid1,groupid2,...
		private string m_postattachperm;	//�ϴ�����Ȩ���趨,��ʽΪ groupid1,groupid2,...
		private int m_applytopictype;	//�����������
		private int m_postbytopictype;	//�����������
		private int m_viewbytopictype;	//����������
		private int m_topictypeprefix;	//���ǰ׺        

//		private string m_viewbyuser;  //�û���������̳Ȩ���趨,��ʽ:username1,username2|uid1,uid2
//		private string m_postbyuser;   //�û���ݷ��»���Ȩ���趨,��ʽ:username1,username2|uid1,uid2
//		private string m_replybyuser;	//�û���ݷ���ظ�Ȩ���趨,��ʽ:username1,username2|uid1,uid2
//		private string m_getattachbyuser; //�û��������/�鿴����Ȩ���趨,��ʽ:username1,username2|uid1,uid2
//		private string m_postattachbyuser; //�û�����ϴ�����Ȩ���趨,��ʽ:username1,username2|uid1,uid2
		private string m_permuserlist;
        private string m_seokeywords; //�������������Ż�,���� meta �� keyword ��ǩ��,����ؼ��ּ����ð�Ƕ���","����
        private string m_seodescription; //�������������Ż�,���� meta �� description ��ǩ��,����ؼ��ּ����ð�Ƕ���","����
        private string m_rewritename; //����URL��д�������

        public ForumInfo Clone()
        {
            return (ForumInfo)this.MemberwiseClone();
        }

		///<summary>
		///��̳fid
		///</summary>
		public int Fid
		{
			get { return m_fid;}
			set { m_fid = value;}
		}
		///<summary>
		///����̳���ϼ���̳��ֱ���̳���ϼ���̳������fid
		///</summary>
		public int Parentid
		{
			get { return m_parentid;}
			set { m_parentid = value;}
		}
		///<summary>
		///��̳���
		///</summary>
		public int Layer
		{
			get { return m_layer;}
			set { m_layer = value;}
		}

		///<summary>
		///��̳��������·����html���Ӵ���
		///</summary>
		public string Pathlist
		{
			get {return m_pathlist;}
			set {m_pathlist=value;}
		}
		///<summary>
		///��̳��������·��id�б�
		///</summary>
		public string Parentidlist
		{
			get {return m_parentidlist.Trim();}
			set {m_parentidlist=value;}
		}

		///<summary>
		///��̳����������̳����
		///</summary>
		public int Subforumcount
		{
			get {return m_subforumcount;}
			set {m_subforumcount=value;}
		}
		///<summary>
		///��̳����
		///</summary>
		public string Name
		{
			get { return m_name;}
			set { m_name = value.Trim();}
		}
		///<summary>
		///�Ƿ���ʾ
		///</summary>
		public int Status
		{
			get { return m_status;}
			set { m_status = value;}
		}
		
		/// <summary>
		/// ���ø���̳������̳���б�ʱ�ּ�����ʾ
		/// </summary>
		public int Colcount
		{
			get { return m_colcount;}
			set { m_colcount = value;}
		}
		///<summary>
		///��ʾ˳��
		///</summary>
		public int Displayorder
		{
			get { return m_displayorder;}
			set { m_displayorder = value;}
		}
		///<summary>
		///���id,0ΪĬ��
		///</summary>
		public int Templateid
		{
			get { return m_templateid;}
			set { m_templateid = value;}
		}
		///<summary>
		///������
		///</summary>
		public int Topics
		{
			get { return m_topics;}
			set { m_topics = value;}
		}
		
		///<summary>
		///������,�������Ӱ�
		///</summary>
		public int CurrentTopics
		{
			get { return m_curtopics;}
			set { m_curtopics = value;}
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
		///���շ���
		///</summary>
		public int Todayposts
		{
			get { return m_todayposts;}
			set { m_todayposts = value;}
		}
		///<summary>
		///��󷢱�ʱ��
		///</summary>
		public string Lastpost
		{
			get { return m_lastpost;}
			set { m_lastpost = value;}
		}
		/// <summary>
		/// ��󷢱���û���
		/// </summary>
		public string Lastposter
		{
			get { return m_lastposter;}
			set { m_lastposter = value;}
		}

		/// <summary>
		/// ��󷢱���û�id
		/// </summary>
		public int Lastposterid
		{
			get { return m_lastposterid;}
			set { m_lastposterid = value;}
		}

		/// <summary>
		/// ��󷢱����ӵ�����id
		/// </summary>
		public int Lasttid
		{
			get { return m_lasttid;}
			set { m_lasttid = value;}
		}


		/// <summary>
		/// ��󷢱�����ӱ���
		/// </summary>
		public string Lasttitle
		{
			get {return m_lasttitle;}
			set {m_lasttitle = value;}
		}
		///<summary>
		///����ʹ��Smilies
		///</summary>
		public int Allowsmilies
		{
			get { return m_allowsmilies;}
			set { m_allowsmilies = value;}
		}
		///<summary>
		///����ʹ��Rss
		///</summary>
		public int Allowrss
		{
			get { return m_allowrss;}
			set { m_allowrss = value;}
		}
		///<summary>
		///����Html����
		///</summary>
		public int Allowhtml
		{
			get { return m_allowhtml;}
			set { m_allowhtml = value;}
		}
		///<summary>
		///����Discuz!NT����
		///</summary>
		public int Allowbbcode
		{
			get { return m_allowbbcode;}
			set { m_allowbbcode = value;}
		}
		///<summary>
		///����[img]����
		///</summary>
		public int Allowimgcode
		{
			get { return m_allowimgcode;}
			set { m_allowimgcode = value;}
		}
		///<summary>
		///�����������ΪBlog
		///</summary>
		public int Allowblog
		{
			get { return m_allowblog;}
			set { m_allowblog = value;}
		}
		///<summary>
		///�Ƿ��װ��(ֻ�ܷ�����)
		///</summary>
		public int Istrade
		{
            get { return m_istrade; }
            set { m_istrade = value; }
		}
        ///<summary>
        ///��������������
        ///</summary>  
		public int Allowpostspecial
		{
            get { return m_allowpostspecial; }
            set { m_allowpostspecial = value; }
		}
        ///<summary>
        ///����������������   
		///</summary>
		public int Allowspecialonly
		{
            get { return m_allowspecialonly; }
            set { m_allowspecialonly = value; }
		}
		///<summary>
		///��������༭��̳����
		///</summary>
		public int Alloweditrules
		{
			get { return m_alloweditrules;}
			set { m_alloweditrules = value;}
		}
		///<summary>
		///����showforumҳ���������ͼ
		///</summary>
		public int Allowthumbnail
		{
			get { return m_allowthumbnail;}
			set { m_allowthumbnail = value;}
		}
        /// <summary>
        /// ����ʹ��Tag
        /// </summary>
        public int Allowtag
        {
            get { return m_allowtag; }
            set { m_allowtag = value; }
        }
		///<summary>
		///�򿪻���վ
		///</summary>
		public int Recyclebin
		{
			get { return m_recyclebin;}
			set { m_recyclebin = value;}
		}
		///<summary>
		///������Ҫ���
		///</summary>
		public int Modnewposts
		{
			get { return m_modnewposts;}
			set { m_modnewposts = value;}
		}
        ///<summary>
        ///��������Ҫ���
        ///</summary>
        public int Modnewtopics
        {
            get { return m_modnewtopics; }
            set { m_modnewtopics = value; }
        }
		///<summary>
		///��������Ӹ�����,��ֹ���⸴��
		///</summary>
		public int Jammer
		{
			get { return m_jammer;}
			set { m_jammer = value;}
		}
		///<summary>
		///��ֹ�����Զ�ˮӡ
		///</summary>
		public int Disablewatermark
		{
			get { return m_disablewatermark;}
			set { m_disablewatermark = value;}
		}
		///<summary>
		///�̳��ϼ���̳�����İ����趨
		///</summary>
		public int Inheritedmod
		{
			get { return m_inheritedmod;}
			set { m_inheritedmod = value;}
		}
		///<summary>
		///�����Զ��ر�����,��λΪ��
		///</summary>
		public int Autoclose
		{
			get { return m_autoclose;}
			set { m_autoclose = value;}
		}

//

		///<summary>
		///��̳����
		///</summary>
		public string Description
		{
			get { return m_description.Trim();}
			set { m_description = value;}
		}
		///<summary>
		///���ʱ���̳������,����Ϊ��������
		///</summary>
		public string Password
		{
			get { return m_password;}
			set { m_password = value;}
		}
		///<summary>
		///��̳ͼ��,��ʾ����ҳ��̳�б��
		///</summary>
		public string Icon
		{
			get { return m_icon;}
			set { m_icon = value;}
		}
		///<summary>
		///��������ֲ���
		///</summary>
		public string Postcredits
		{
			get { return m_postcredits;}
			set { m_postcredits = value;}
		}
		///<summary>
		///���ظ����ֲ���
		///</summary>
		public string Replycredits
		{
			get { return m_replycredits;}
			set { m_replycredits = value;}
		}
		///<summary>
		///ָ���ⲿ���ӵĵ�ַ
		///</summary>
		public string Redirect
		{
			get { return m_redirect;}
			set { m_redirect = value;}
		}
		///<summary>
		///�����ڱ���̳�ϴ��ĸ�������,����ΪĬ��
		///</summary>
		public string Attachextensions
		{
			get { return m_attachextensions;}
			set { m_attachextensions = value;}
		}
		///<summary>
		///�����б�(������ʾʹ��,����¼ʵ��Ȩ��)
		///</summary>
		public string Moderators
		{
			get { return m_moderators;}
			set { m_moderators = value;}
		}
		///<summary>
		///�������
		///</summary>
		public string Rules
		{
			get { return m_rules;}
			set { m_rules = value;}
		}
		///<summary>
		///�������
		///</summary>
		public string Topictypes
		{
			get { return m_topictypes;}
			set { m_topictypes = value;}
		}
		///<summary>
		///���Ȩ���趨,��ʽΪ groupid1,groupid2...
		///</summary>
		public string Viewperm
		{
			get { return m_viewperm;}
			set { m_viewperm = value;}
		}
		///<summary>
		///������Ȩ���趨,��ʽΪ groupid1,groupid2...
		///</summary>
		public string Postperm
		{
			get { return m_postperm;}
			set { m_postperm = value;}
		}
		///<summary>
		///���ظ�Ȩ���趨,��ʽΪ groupid1,groupid2...
		///</summary>
		public string Replyperm
		{
			get { return m_replyperm;}
			set { m_replyperm = value;}
		}
		///<summary>
		///���ظ���Ȩ���趨,��ʽΪ groupid1,groupid2...
		///</summary>
		public string Getattachperm
		{
			get { return m_getattachperm;}
			set { m_getattachperm = value;}
		}
		///<summary>
		///�ϴ�����Ȩ���趨,��ʽΪ groupid1,groupid2...
		///</summary>
		public string Postattachperm
		{
			get { return m_postattachperm;}
			set { m_postattachperm = value;}
		}

		///<summary>
		///�����������
		///</summary>
		public int Applytopictype
		{
			get { return m_applytopictype;}
			set { m_applytopictype = value;}
		}

		///<summary>
		///�����������
		///</summary>
		public int Postbytopictype
		{
			get { return m_postbytopictype;}
			set { m_postbytopictype = value;}
		}


		///<summary>
		///����������
		///</summary>
		public int Viewbytopictype
		{
			get { return m_viewbytopictype;}
			set { m_viewbytopictype = value;}
		}


		///<summary>
		///���ǰ׺
		///</summary>
		public int Topictypeprefix
		{
			get { return m_topictypeprefix;}
			set { m_topictypeprefix = value;}
		}

		/*
		///<summary>
		///�û���������̳Ȩ���趨
		///</summary>
		public string Viewbyuser
		{
			get { return m_viewbyuser;}
			set { m_viewbyuser = value;}
		}

		///<summary>
		///�û���ݷ��»���Ȩ���趨
		///</summary>
		public string Postbyuser
		{
			get { return m_postbyuser;}
			set { m_postbyuser = value;}
		}

		///<summary>
		///�û���ݷ���ظ�Ȩ���趨
		///</summary>
		public string Replybyuser
		{
			get { return m_replybyuser;}
			set { m_replybyuser = value;}
		}

		///<summary>
		///�û��������/�鿴����Ȩ���趨
		///</summary>
		public string Getattachbyuser
		{
			get { return m_getattachbyuser;}
			set { m_getattachbyuser = value;}
		}

		///<summary>
		///���û�����ϴ�����Ȩ���趨
		///</summary>
		public string Postattachbyuser
		{
			get { return m_postattachbyuser;}
			set { m_postattachbyuser = value;}
		}
		*/

		public string Permuserlist
		{
			get { return m_permuserlist;}
			set { m_permuserlist = value;}
		}

        /// <summary>
        /// �������������Ż�,���� meta �� keyword ��ǩ��,����ؼ��ּ����ð�Ƕ���","����
        /// </summary>
        public string Seokeywords
        {
            get { return m_seokeywords; }
            set { m_seokeywords = value; }
        }

        /// <summary>
        /// �������������Ż�,���� meta �� description ��ǩ��,����ؼ��ּ����ð�Ƕ���","����
        /// </summary>
        public string Seodescription
        {
            get { return m_seodescription; }
            set { m_seodescription = value; }
        }

        /// <summary>
        /// ����URL��д�������
        /// </summary>
        public string Rewritename
        {
            get { return m_rewritename == null ? "" : m_rewritename; }
            set { m_rewritename = value; }
        }
        
	}
}
