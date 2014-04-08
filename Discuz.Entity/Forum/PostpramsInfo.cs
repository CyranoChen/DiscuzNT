using Discuz.Common;


namespace Discuz.Entity
{
	/// <summary>
	/// PostpramsInfo 的摘要说明。
	/// </summary>
	public class PostpramsInfo
	{
		private int fid; //版块id
		private int tid; //主题id
		private int pid; //帖子id
		private int pagesize; //分页显示帖子数量
		private int pageindex; //分页当前页数
		private string getattachperm; //下载附件权限设定,格式为 groupid1,groupid2...
		private bool ubbmode; //是否对Discuz代码进行解析(true:解析,false:不解析)
		private int usergroupid; //用户所在组ID
		private int usergroupreadaccess; //用户的下载/访问权限
		private int attachimgpost; //是否显示图片附件
		private int showattachmentpath; //是否显示附件的直实路径
        private int hide; //判断是否为回复可见帖, hide=0为非回复可见(正常), hide>0为回复可见, hide=-1为回复可见但当前用户已回复, hide = -2表示当前用户为该帖作者
		private int price; //判断是否为购买可见帖, price=0为非购买可见(正常), price>0 为购买可见
		private string condition; //附加条件
		private int jammer; //帖子是否增加干扰码
		private int onlinetimeout;
		private int currentuserid;//当前在线用户id
        private int usercredits; //当前用户积分 
		private UserGroupInfo currentusergroup; //当前用户组

		/// <summary>
		/// 以下为ubb转换专用属性
		/// </summary>
		private string sdetail;		//帖子内容
		private int smileyoff;		//禁止笑脸显示.
		private int bbcodeoff;		//禁止Discuz!NT代码转换
		private int parseurloff;	//禁止网址自动转换
		private int showimages;		//是否对帖子中的图片标签进行解析.
		private int allowhtml;		//是否允许解析html标签.
		private SmiliesInfo[] smiliesinfo;	//表情库
		private CustomEditorButtonInfo[] customeditorbuttoninfo; ///自定义按钮图库
		private int smiliesmax;		//帖子中解析的单个表情最大个数.
		private int bbcodemode;		//Discuz!NT代码兼容模式(0:不兼容,1:动网兼容)
		private int signature;		//是否为签名，用于签名ubb转换
        private int isforspace = 0;     //是为个人空间而进行的解析  
        private TopicInfo topicinfo; // 主题信息
        private int templatewidth = 600; //用户所选模版样式宽度

		/// <summary>
		/// 版块id
		/// </summary>
		public int Fid
		{
			get { return fid; }
			set { fid = value; }
		}

		/// <summary>
		/// 主题id
		/// </summary>
		public int Tid
		{
			get { return tid; }
			set { tid = value; }
		}


		/// <summary>
		/// 帖子id
		/// </summary>
		public int Pid
		{
			get { return pid; }
			set { pid = value; }
		}

		/// <summary>
		/// 分页显示帖子数量
		/// </summary>
		public int Pagesize
		{
			get { return pagesize; }
			set { pagesize = value; }
		}

		/// <summary>
		/// 分页当前页数
		/// </summary>
		public int Pageindex
		{
			get { return pageindex; }
			set { pageindex = value; }
		}

		/// <summary>
		/// 下载附件权限设定,格式为 groupid1,groupid2...
		/// </summary>
		public string Getattachperm
		{
			get { return getattachperm; }
			set { getattachperm = value; }
		}

		/// <summary>
		/// 是否对Discuz代码进行解析(true:解析,false:不解析)
		/// </summary>
		public bool Ubbmode
		{
			get { return ubbmode; }
			set { ubbmode = value; }
		}

		/// <summary>
		/// 用户所在组ID
		/// </summary>
		public int Usergroupid
		{
			get { return usergroupid; }
			set { usergroupid = value; }
		}


		/// <summary>
		/// 用户的下载/访问权限
		/// </summary>
		public int Usergroupreadaccess
		{
			get { return usergroupreadaccess; }
			set { usergroupreadaccess = value; }
		}

		/// <summary>
		/// 是否显示图片附件
		/// </summary>
		public int Attachimgpost
		{
			get { return attachimgpost; }
			set { attachimgpost = value; }
		}

		/// <summary>
		/// 是否显示附件的直实路径
		/// </summary>
		public int Showattachmentpath
		{
			get { return showattachmentpath; }
			set { showattachmentpath = value; }
		}

		/// <summary>
        /// 判断是否为回复可见帖, hide=0为非回复可见(正常), hide>0为回复可见, hide=-1为回复可见但当前用户已回复, hide = -2表示当前用户为该帖作者
		/// </summary>
		public int Hide
		{
			get { return hide; }
			set { hide = value; }
		}
      
		/// <summary>
		/// 判断是否为购买可见帖, price=0为非购买可见(正常), price>0 为购买可见
		/// </summary>
		public int Price
		{
			get { return price; }
			set { price = value; }
		}

		/// <summary>
		/// 附加条件
		/// </summary>
		public string Condition
		{
			get { return condition == null ? "" : condition ; }
			set { condition = value; }
		}

		/// <summary>
		/// 以下为ubb转换专用属性
		/// </summary>
		
		/// <summary>
		/// 帖子内容
		/// </summary>
		public string Sdetail
		{
			get { return sdetail == null ? "" : sdetail; }
			set { sdetail = value; }
		}

		/// <summary>
		/// 禁止笑脸显示.
		/// </summary>
		public int Smileyoff
		{
			get { return smileyoff; }
			set { smileyoff = value; }
		}

		/// <summary>
		/// 禁止ubb转换
		/// </summary>
		public int Bbcodeoff
		{
			get { return bbcodeoff; }
			set { bbcodeoff = value; }
		}

		/// <summary>
		/// 禁止网址自动转换
		/// </summary>
		public int Parseurloff
		{
			get { return parseurloff; }
			set { parseurloff = value; }
		}

		/// <summary>
		/// 是否对帖子中的图片标签进行解析.
		/// </summary>
		public int Showimages
		{
			get { return showimages; }
			set { showimages = value; }
		}

		/// <summary>
		/// 是否允许解析html标签.
		/// </summary>
		public int Allowhtml
		{
			get { return allowhtml; }
			set { allowhtml = value; }
		}

		/// <summary>
		/// 表情库
		/// </summary>
		public SmiliesInfo[] Smiliesinfo
		{
			get { return smiliesinfo; }
			set { smiliesinfo = value; }
		}

		/// <summary>
		/// 自定义按钮图库
		/// </summary>
		public CustomEditorButtonInfo[] Customeditorbuttoninfo
		{
			get { return customeditorbuttoninfo; }
			set { customeditorbuttoninfo = value; }
		}

		/// <summary>
		/// 帖子中解析的单个表情最大个数.
		/// </summary>
		public int Smiliesmax
		{
			get { return smiliesmax; }
			set { smiliesmax = value; }
		}

		/// <summary>
		/// Discuz代码兼容模式(0:不兼容,1:动网兼容)
		/// </summary>
		public int Bbcodemode
		{
			get { return bbcodemode; }
			set { bbcodemode = value; }
		}

		/// <summary>
		/// 帖子是否增加干扰码
		/// </summary>
		public int Jammer
		{
			get { return jammer; }
			set { jammer = value; }
		}

		/// <summary>
		/// 用户在线超时时间
		/// </summary>
		public int Onlinetimeout
		{
			get { return onlinetimeout; }
			set { onlinetimeout = value; }
		}
		
		/// <summary>
		/// 当前的用户id
		/// </summary>
		public int CurrentUserid
		{
			get { return currentuserid; }
			set { currentuserid = value; }
		}

        /// <summary>
        /// 用户积分
        /// </summary>
        public int Usercredits
        {
            get { return usercredits; }
            set { usercredits = value; }
        }
        		
		/// <summary>
		/// 当前的用户组信息
		/// </summary>
		public UserGroupInfo CurrentUserGroup
		{
			get { return currentusergroup; }
			set { currentusergroup = value; }
		}

		/// <summary>
		/// 是否为签名
		/// </summary>
		public int Signature
		{
			get { return signature; }
			set { signature = value; }
		}

        /// <summary>
        /// 是否是为个人空间而进行的
        /// </summary>
        public int Isforspace
        {
            get { return isforspace; }
            set { isforspace = value; }
        }

        /// <summary>
        /// 主题信息
        /// </summary>
        public TopicInfo Topicinfo
        {
            get { return topicinfo; }
            set { topicinfo = value; }
        }

        /// <summary>
        /// 用户所选模版样式宽度
        /// </summary>
        public int TemplateWidth
        {
            get { return templatewidth; }
            set { templatewidth = value; }
        }
        
	}
}