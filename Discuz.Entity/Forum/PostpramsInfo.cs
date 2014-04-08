using Discuz.Common;


namespace Discuz.Entity
{
	/// <summary>
	/// PostpramsInfo ��ժҪ˵����
	/// </summary>
	public class PostpramsInfo
	{
		private int fid; //���id
		private int tid; //����id
		private int pid; //����id
		private int pagesize; //��ҳ��ʾ��������
		private int pageindex; //��ҳ��ǰҳ��
		private string getattachperm; //���ظ���Ȩ���趨,��ʽΪ groupid1,groupid2...
		private bool ubbmode; //�Ƿ��Discuz������н���(true:����,false:������)
		private int usergroupid; //�û�������ID
		private int usergroupreadaccess; //�û�������/����Ȩ��
		private int attachimgpost; //�Ƿ���ʾͼƬ����
		private int showattachmentpath; //�Ƿ���ʾ������ֱʵ·��
        private int hide; //�ж��Ƿ�Ϊ�ظ��ɼ���, hide=0Ϊ�ǻظ��ɼ�(����), hide>0Ϊ�ظ��ɼ�, hide=-1Ϊ�ظ��ɼ�����ǰ�û��ѻظ�, hide = -2��ʾ��ǰ�û�Ϊ��������
		private int price; //�ж��Ƿ�Ϊ����ɼ���, price=0Ϊ�ǹ���ɼ�(����), price>0 Ϊ����ɼ�
		private string condition; //��������
		private int jammer; //�����Ƿ����Ӹ�����
		private int onlinetimeout;
		private int currentuserid;//��ǰ�����û�id
        private int usercredits; //��ǰ�û����� 
		private UserGroupInfo currentusergroup; //��ǰ�û���

		/// <summary>
		/// ����Ϊubbת��ר������
		/// </summary>
		private string sdetail;		//��������
		private int smileyoff;		//��ֹЦ����ʾ.
		private int bbcodeoff;		//��ֹDiscuz!NT����ת��
		private int parseurloff;	//��ֹ��ַ�Զ�ת��
		private int showimages;		//�Ƿ�������е�ͼƬ��ǩ���н���.
		private int allowhtml;		//�Ƿ��������html��ǩ.
		private SmiliesInfo[] smiliesinfo;	//�����
		private CustomEditorButtonInfo[] customeditorbuttoninfo; ///�Զ��尴ťͼ��
		private int smiliesmax;		//�����н����ĵ�������������.
		private int bbcodemode;		//Discuz!NT�������ģʽ(0:������,1:��������)
		private int signature;		//�Ƿ�Ϊǩ��������ǩ��ubbת��
        private int isforspace = 0;     //��Ϊ���˿ռ�����еĽ���  
        private TopicInfo topicinfo; // ������Ϣ
        private int templatewidth = 600; //�û���ѡģ����ʽ���

		/// <summary>
		/// ���id
		/// </summary>
		public int Fid
		{
			get { return fid; }
			set { fid = value; }
		}

		/// <summary>
		/// ����id
		/// </summary>
		public int Tid
		{
			get { return tid; }
			set { tid = value; }
		}


		/// <summary>
		/// ����id
		/// </summary>
		public int Pid
		{
			get { return pid; }
			set { pid = value; }
		}

		/// <summary>
		/// ��ҳ��ʾ��������
		/// </summary>
		public int Pagesize
		{
			get { return pagesize; }
			set { pagesize = value; }
		}

		/// <summary>
		/// ��ҳ��ǰҳ��
		/// </summary>
		public int Pageindex
		{
			get { return pageindex; }
			set { pageindex = value; }
		}

		/// <summary>
		/// ���ظ���Ȩ���趨,��ʽΪ groupid1,groupid2...
		/// </summary>
		public string Getattachperm
		{
			get { return getattachperm; }
			set { getattachperm = value; }
		}

		/// <summary>
		/// �Ƿ��Discuz������н���(true:����,false:������)
		/// </summary>
		public bool Ubbmode
		{
			get { return ubbmode; }
			set { ubbmode = value; }
		}

		/// <summary>
		/// �û�������ID
		/// </summary>
		public int Usergroupid
		{
			get { return usergroupid; }
			set { usergroupid = value; }
		}


		/// <summary>
		/// �û�������/����Ȩ��
		/// </summary>
		public int Usergroupreadaccess
		{
			get { return usergroupreadaccess; }
			set { usergroupreadaccess = value; }
		}

		/// <summary>
		/// �Ƿ���ʾͼƬ����
		/// </summary>
		public int Attachimgpost
		{
			get { return attachimgpost; }
			set { attachimgpost = value; }
		}

		/// <summary>
		/// �Ƿ���ʾ������ֱʵ·��
		/// </summary>
		public int Showattachmentpath
		{
			get { return showattachmentpath; }
			set { showattachmentpath = value; }
		}

		/// <summary>
        /// �ж��Ƿ�Ϊ�ظ��ɼ���, hide=0Ϊ�ǻظ��ɼ�(����), hide>0Ϊ�ظ��ɼ�, hide=-1Ϊ�ظ��ɼ�����ǰ�û��ѻظ�, hide = -2��ʾ��ǰ�û�Ϊ��������
		/// </summary>
		public int Hide
		{
			get { return hide; }
			set { hide = value; }
		}
      
		/// <summary>
		/// �ж��Ƿ�Ϊ����ɼ���, price=0Ϊ�ǹ���ɼ�(����), price>0 Ϊ����ɼ�
		/// </summary>
		public int Price
		{
			get { return price; }
			set { price = value; }
		}

		/// <summary>
		/// ��������
		/// </summary>
		public string Condition
		{
			get { return condition == null ? "" : condition ; }
			set { condition = value; }
		}

		/// <summary>
		/// ����Ϊubbת��ר������
		/// </summary>
		
		/// <summary>
		/// ��������
		/// </summary>
		public string Sdetail
		{
			get { return sdetail == null ? "" : sdetail; }
			set { sdetail = value; }
		}

		/// <summary>
		/// ��ֹЦ����ʾ.
		/// </summary>
		public int Smileyoff
		{
			get { return smileyoff; }
			set { smileyoff = value; }
		}

		/// <summary>
		/// ��ֹubbת��
		/// </summary>
		public int Bbcodeoff
		{
			get { return bbcodeoff; }
			set { bbcodeoff = value; }
		}

		/// <summary>
		/// ��ֹ��ַ�Զ�ת��
		/// </summary>
		public int Parseurloff
		{
			get { return parseurloff; }
			set { parseurloff = value; }
		}

		/// <summary>
		/// �Ƿ�������е�ͼƬ��ǩ���н���.
		/// </summary>
		public int Showimages
		{
			get { return showimages; }
			set { showimages = value; }
		}

		/// <summary>
		/// �Ƿ��������html��ǩ.
		/// </summary>
		public int Allowhtml
		{
			get { return allowhtml; }
			set { allowhtml = value; }
		}

		/// <summary>
		/// �����
		/// </summary>
		public SmiliesInfo[] Smiliesinfo
		{
			get { return smiliesinfo; }
			set { smiliesinfo = value; }
		}

		/// <summary>
		/// �Զ��尴ťͼ��
		/// </summary>
		public CustomEditorButtonInfo[] Customeditorbuttoninfo
		{
			get { return customeditorbuttoninfo; }
			set { customeditorbuttoninfo = value; }
		}

		/// <summary>
		/// �����н����ĵ�������������.
		/// </summary>
		public int Smiliesmax
		{
			get { return smiliesmax; }
			set { smiliesmax = value; }
		}

		/// <summary>
		/// Discuz�������ģʽ(0:������,1:��������)
		/// </summary>
		public int Bbcodemode
		{
			get { return bbcodemode; }
			set { bbcodemode = value; }
		}

		/// <summary>
		/// �����Ƿ����Ӹ�����
		/// </summary>
		public int Jammer
		{
			get { return jammer; }
			set { jammer = value; }
		}

		/// <summary>
		/// �û����߳�ʱʱ��
		/// </summary>
		public int Onlinetimeout
		{
			get { return onlinetimeout; }
			set { onlinetimeout = value; }
		}
		
		/// <summary>
		/// ��ǰ���û�id
		/// </summary>
		public int CurrentUserid
		{
			get { return currentuserid; }
			set { currentuserid = value; }
		}

        /// <summary>
        /// �û�����
        /// </summary>
        public int Usercredits
        {
            get { return usercredits; }
            set { usercredits = value; }
        }
        		
		/// <summary>
		/// ��ǰ���û�����Ϣ
		/// </summary>
		public UserGroupInfo CurrentUserGroup
		{
			get { return currentusergroup; }
			set { currentusergroup = value; }
		}

		/// <summary>
		/// �Ƿ�Ϊǩ��
		/// </summary>
		public int Signature
		{
			get { return signature; }
			set { signature = value; }
		}

        /// <summary>
        /// �Ƿ���Ϊ���˿ռ�����е�
        /// </summary>
        public int Isforspace
        {
            get { return isforspace; }
            set { isforspace = value; }
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public TopicInfo Topicinfo
        {
            get { return topicinfo; }
            set { topicinfo = value; }
        }

        /// <summary>
        /// �û���ѡģ����ʽ���
        /// </summary>
        public int TemplateWidth
        {
            get { return templatewidth; }
            set { templatewidth = value; }
        }
        
	}
}