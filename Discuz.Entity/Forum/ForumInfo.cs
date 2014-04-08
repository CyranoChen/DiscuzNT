using System;


namespace Discuz.Entity
{
	/// <summary>
	/// 版块描述类
	/// </summary>
    [Serializable]
	public class ForumInfo
	{
		private int m_fid;	//论坛fid
		private int m_parentid;	//本论坛的上级论坛或分本论坛的上级论坛或分类的fid
		private int m_layer;	//论坛层次
		private string m_pathlist = ""; //论坛级别所处路径的html链接代码
		private string m_parentidlist = ""; //论坛级别所处路径id列表
		private int m_subforumcount; //论坛包括的子论坛个数
		private string m_name = "";	//论坛名称
		private int m_status;	//是否显示
		private int m_colcount;	//设置该论坛的子论坛在列表时分几列显示
		private int m_displayorder;	//显示顺序
		private int m_templateid;	//风格id,0为默认
		private int m_topics;	//主题数
		private int m_curtopics;	//主题数
		private int m_posts;	//帖子数
		private int m_todayposts;	//今日发帖
		private string m_lastpost;	//最后发表日期
		private string m_lastposter; //最后发表的用户名
		private int m_lastposterid; //最后发表的用户id
		private int m_lasttid; //最后发表帖子的主题id
		private string m_lasttitle; //最后发表的帖子标题
		private int m_allowsmilies;	//允许使用表情符
		private int m_allowrss;	//允许使用Rss
		private int m_allowhtml;	//允许Html代码
		private int m_allowbbcode;	//允许Discuz!NT代码
		private int m_allowimgcode;	//允许[img]代码
		private int m_allowblog;	//允许将文章添加为Blog
		private int m_istrade;	//是否是交易版块
        private int m_allowpostspecial;	//允许发表特殊主题
        private int m_allowspecialonly;	//仅允许发表特殊主题      
		private int m_alloweditrules;	//允许版主编辑论坛规则
		private int m_allowthumbnail;	//允许showforum页面输出缩略图
        private int m_allowtag;     //允许使用标签        
		private int m_recyclebin;	//打开回收站
		private int m_modnewposts;	//发帖需要审核
        private int m_modnewtopics; //发主题需要审核
		private int m_jammer;	//帖子中添加干扰码,防止恶意复制
		private int m_disablewatermark;	//禁止附件自动水印
		private int m_inheritedmod;	//继承上级论坛或分类的版主设定
		private int m_autoclose;	//定期自动关闭主题,单位为天
		//
		private string m_description = string.Empty;	//论坛描述
		private string m_password;	//访问本论坛的密码,留空为不需密码
		private string m_icon;	//论坛图标,显示于首页论坛列表等
		private string m_postcredits;	//发主题积分策略
		private string m_replycredits;	//发回复积分策略
		private string m_redirect;	//指向外部链接的地址
		private string m_attachextensions;	//允许在本论坛上传的附件类型,留空为默认
		private string m_moderators = "";	//版主列表(仅供显示使用,不记录实际权限)
		private string m_rules;	//本版规则
		private string m_topictypes;	//主题分类
		private string m_viewperm = "";	//浏览权限设定,格式为 groupid1,groupid2,...
		private string m_postperm;	//发主题权限设定,格式为 groupid1,groupid2,...
		private string m_replyperm;	//发回复权限设定,格式为 groupid1,groupid2,...
		private string m_getattachperm;	//下载附件权限设定,格式为 groupid1,groupid2,...
		private string m_postattachperm;	//上传附件权限设定,格式为 groupid1,groupid2,...
		private int m_applytopictype;	//启用主题分类
		private int m_postbytopictype;	//发帖必须归类
		private int m_viewbytopictype;	//允许按类别浏览
		private int m_topictypeprefix;	//类别前缀        

//		private string m_viewbyuser;  //用户身份浏览论坛权限设定,格式:username1,username2|uid1,uid2
//		private string m_postbyuser;   //用户身份发新话题权限设定,格式:username1,username2|uid1,uid2
//		private string m_replybyuser;	//用户身份发表回复权限设定,格式:username1,username2|uid1,uid2
//		private string m_getattachbyuser; //用户身份下载/查看附件权限设定,格式:username1,username2|uid1,uid2
//		private string m_postattachbyuser; //用户身份上传附件权限设定,格式:username1,username2|uid1,uid2
		private string m_permuserlist;
        private string m_seokeywords; //用于搜索引擎优化,放在 meta 的 keyword 标签中,多个关键字间请用半角逗号","隔开
        private string m_seodescription; //用于搜索引擎优化,放在 meta 的 description 标签中,多个关键字间请用半角逗号","隔开
        private string m_rewritename; //用于URL重写版块名称

        public ForumInfo Clone()
        {
            return (ForumInfo)this.MemberwiseClone();
        }

		///<summary>
		///论坛fid
		///</summary>
		public int Fid
		{
			get { return m_fid;}
			set { m_fid = value;}
		}
		///<summary>
		///本论坛的上级论坛或分本论坛的上级论坛或分类的fid
		///</summary>
		public int Parentid
		{
			get { return m_parentid;}
			set { m_parentid = value;}
		}
		///<summary>
		///论坛层次
		///</summary>
		public int Layer
		{
			get { return m_layer;}
			set { m_layer = value;}
		}

		///<summary>
		///论坛级别所处路径的html链接代码
		///</summary>
		public string Pathlist
		{
			get {return m_pathlist;}
			set {m_pathlist=value;}
		}
		///<summary>
		///论坛级别所处路径id列表
		///</summary>
		public string Parentidlist
		{
			get {return m_parentidlist.Trim();}
			set {m_parentidlist=value;}
		}

		///<summary>
		///论坛包括的子论坛个数
		///</summary>
		public int Subforumcount
		{
			get {return m_subforumcount;}
			set {m_subforumcount=value;}
		}
		///<summary>
		///论坛名称
		///</summary>
		public string Name
		{
			get { return m_name;}
			set { m_name = value.Trim();}
		}
		///<summary>
		///是否显示
		///</summary>
		public int Status
		{
			get { return m_status;}
			set { m_status = value;}
		}
		
		/// <summary>
		/// 设置该论坛的子论坛在列表时分几列显示
		/// </summary>
		public int Colcount
		{
			get { return m_colcount;}
			set { m_colcount = value;}
		}
		///<summary>
		///显示顺序
		///</summary>
		public int Displayorder
		{
			get { return m_displayorder;}
			set { m_displayorder = value;}
		}
		///<summary>
		///风格id,0为默认
		///</summary>
		public int Templateid
		{
			get { return m_templateid;}
			set { m_templateid = value;}
		}
		///<summary>
		///主题数
		///</summary>
		public int Topics
		{
			get { return m_topics;}
			set { m_topics = value;}
		}
		
		///<summary>
		///主题数,不包括子版
		///</summary>
		public int CurrentTopics
		{
			get { return m_curtopics;}
			set { m_curtopics = value;}
		}
		///<summary>
		///帖子数
		///</summary>
		public int Posts
		{
			get { return m_posts;}
			set { m_posts = value;}
		}
		///<summary>
		///今日发帖
		///</summary>
		public int Todayposts
		{
			get { return m_todayposts;}
			set { m_todayposts = value;}
		}
		///<summary>
		///最后发表时间
		///</summary>
		public string Lastpost
		{
			get { return m_lastpost;}
			set { m_lastpost = value;}
		}
		/// <summary>
		/// 最后发表的用户名
		/// </summary>
		public string Lastposter
		{
			get { return m_lastposter;}
			set { m_lastposter = value;}
		}

		/// <summary>
		/// 最后发表的用户id
		/// </summary>
		public int Lastposterid
		{
			get { return m_lastposterid;}
			set { m_lastposterid = value;}
		}

		/// <summary>
		/// 最后发表帖子的主题id
		/// </summary>
		public int Lasttid
		{
			get { return m_lasttid;}
			set { m_lasttid = value;}
		}


		/// <summary>
		/// 最后发表的帖子标题
		/// </summary>
		public string Lasttitle
		{
			get {return m_lasttitle;}
			set {m_lasttitle = value;}
		}
		///<summary>
		///允许使用Smilies
		///</summary>
		public int Allowsmilies
		{
			get { return m_allowsmilies;}
			set { m_allowsmilies = value;}
		}
		///<summary>
		///允许使用Rss
		///</summary>
		public int Allowrss
		{
			get { return m_allowrss;}
			set { m_allowrss = value;}
		}
		///<summary>
		///允许Html代码
		///</summary>
		public int Allowhtml
		{
			get { return m_allowhtml;}
			set { m_allowhtml = value;}
		}
		///<summary>
		///允许Discuz!NT代码
		///</summary>
		public int Allowbbcode
		{
			get { return m_allowbbcode;}
			set { m_allowbbcode = value;}
		}
		///<summary>
		///允许[img]代码
		///</summary>
		public int Allowimgcode
		{
			get { return m_allowimgcode;}
			set { m_allowimgcode = value;}
		}
		///<summary>
		///允许将文章添加为Blog
		///</summary>
		public int Allowblog
		{
			get { return m_allowblog;}
			set { m_allowblog = value;}
		}
		///<summary>
		///是否交易版块(只能发表交易)
		///</summary>
		public int Istrade
		{
            get { return m_istrade; }
            set { m_istrade = value; }
		}
        ///<summary>
        ///允许发表特殊主题
        ///</summary>  
		public int Allowpostspecial
		{
            get { return m_allowpostspecial; }
            set { m_allowpostspecial = value; }
		}
        ///<summary>
        ///仅允许发表特殊主题   
		///</summary>
		public int Allowspecialonly
		{
            get { return m_allowspecialonly; }
            set { m_allowspecialonly = value; }
		}
		///<summary>
		///允许版主编辑论坛规则
		///</summary>
		public int Alloweditrules
		{
			get { return m_alloweditrules;}
			set { m_alloweditrules = value;}
		}
		///<summary>
		///允许showforum页面输出缩略图
		///</summary>
		public int Allowthumbnail
		{
			get { return m_allowthumbnail;}
			set { m_allowthumbnail = value;}
		}
        /// <summary>
        /// 允许使用Tag
        /// </summary>
        public int Allowtag
        {
            get { return m_allowtag; }
            set { m_allowtag = value; }
        }
		///<summary>
		///打开回收站
		///</summary>
		public int Recyclebin
		{
			get { return m_recyclebin;}
			set { m_recyclebin = value;}
		}
		///<summary>
		///发帖需要审核
		///</summary>
		public int Modnewposts
		{
			get { return m_modnewposts;}
			set { m_modnewposts = value;}
		}
        ///<summary>
        ///发主题需要审核
        ///</summary>
        public int Modnewtopics
        {
            get { return m_modnewtopics; }
            set { m_modnewtopics = value; }
        }
		///<summary>
		///帖子中添加干扰码,防止恶意复制
		///</summary>
		public int Jammer
		{
			get { return m_jammer;}
			set { m_jammer = value;}
		}
		///<summary>
		///禁止附件自动水印
		///</summary>
		public int Disablewatermark
		{
			get { return m_disablewatermark;}
			set { m_disablewatermark = value;}
		}
		///<summary>
		///继承上级论坛或分类的版主设定
		///</summary>
		public int Inheritedmod
		{
			get { return m_inheritedmod;}
			set { m_inheritedmod = value;}
		}
		///<summary>
		///定期自动关闭主题,单位为天
		///</summary>
		public int Autoclose
		{
			get { return m_autoclose;}
			set { m_autoclose = value;}
		}

//

		///<summary>
		///论坛描述
		///</summary>
		public string Description
		{
			get { return m_description.Trim();}
			set { m_description = value;}
		}
		///<summary>
		///访问本论坛的密码,留空为不需密码
		///</summary>
		public string Password
		{
			get { return m_password;}
			set { m_password = value;}
		}
		///<summary>
		///论坛图标,显示于首页论坛列表等
		///</summary>
		public string Icon
		{
			get { return m_icon;}
			set { m_icon = value;}
		}
		///<summary>
		///发主题积分策略
		///</summary>
		public string Postcredits
		{
			get { return m_postcredits;}
			set { m_postcredits = value;}
		}
		///<summary>
		///发回复积分策略
		///</summary>
		public string Replycredits
		{
			get { return m_replycredits;}
			set { m_replycredits = value;}
		}
		///<summary>
		///指向外部链接的地址
		///</summary>
		public string Redirect
		{
			get { return m_redirect;}
			set { m_redirect = value;}
		}
		///<summary>
		///允许在本论坛上传的附件类型,留空为默认
		///</summary>
		public string Attachextensions
		{
			get { return m_attachextensions;}
			set { m_attachextensions = value;}
		}
		///<summary>
		///版主列表(仅供显示使用,不记录实际权限)
		///</summary>
		public string Moderators
		{
			get { return m_moderators;}
			set { m_moderators = value;}
		}
		///<summary>
		///本版规则
		///</summary>
		public string Rules
		{
			get { return m_rules;}
			set { m_rules = value;}
		}
		///<summary>
		///主题分类
		///</summary>
		public string Topictypes
		{
			get { return m_topictypes;}
			set { m_topictypes = value;}
		}
		///<summary>
		///浏览权限设定,格式为 groupid1,groupid2...
		///</summary>
		public string Viewperm
		{
			get { return m_viewperm;}
			set { m_viewperm = value;}
		}
		///<summary>
		///发主题权限设定,格式为 groupid1,groupid2...
		///</summary>
		public string Postperm
		{
			get { return m_postperm;}
			set { m_postperm = value;}
		}
		///<summary>
		///发回复权限设定,格式为 groupid1,groupid2...
		///</summary>
		public string Replyperm
		{
			get { return m_replyperm;}
			set { m_replyperm = value;}
		}
		///<summary>
		///下载附件权限设定,格式为 groupid1,groupid2...
		///</summary>
		public string Getattachperm
		{
			get { return m_getattachperm;}
			set { m_getattachperm = value;}
		}
		///<summary>
		///上传附件权限设定,格式为 groupid1,groupid2...
		///</summary>
		public string Postattachperm
		{
			get { return m_postattachperm;}
			set { m_postattachperm = value;}
		}

		///<summary>
		///启用主题分类
		///</summary>
		public int Applytopictype
		{
			get { return m_applytopictype;}
			set { m_applytopictype = value;}
		}

		///<summary>
		///发帖必须归类
		///</summary>
		public int Postbytopictype
		{
			get { return m_postbytopictype;}
			set { m_postbytopictype = value;}
		}


		///<summary>
		///允许按类别浏览
		///</summary>
		public int Viewbytopictype
		{
			get { return m_viewbytopictype;}
			set { m_viewbytopictype = value;}
		}


		///<summary>
		///类别前缀
		///</summary>
		public int Topictypeprefix
		{
			get { return m_topictypeprefix;}
			set { m_topictypeprefix = value;}
		}

		/*
		///<summary>
		///用户身份浏览论坛权限设定
		///</summary>
		public string Viewbyuser
		{
			get { return m_viewbyuser;}
			set { m_viewbyuser = value;}
		}

		///<summary>
		///用户身份发新话题权限设定
		///</summary>
		public string Postbyuser
		{
			get { return m_postbyuser;}
			set { m_postbyuser = value;}
		}

		///<summary>
		///用户身份发表回复权限设定
		///</summary>
		public string Replybyuser
		{
			get { return m_replybyuser;}
			set { m_replybyuser = value;}
		}

		///<summary>
		///用户身份下载/查看附件权限设定
		///</summary>
		public string Getattachbyuser
		{
			get { return m_getattachbyuser;}
			set { m_getattachbyuser = value;}
		}

		///<summary>
		///上用户身份上传附件权限设定
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
        /// 用于搜索引擎优化,放在 meta 的 keyword 标签中,多个关键字间请用半角逗号","隔开
        /// </summary>
        public string Seokeywords
        {
            get { return m_seokeywords; }
            set { m_seokeywords = value; }
        }

        /// <summary>
        /// 用于搜索引擎优化,放在 meta 的 description 标签中,多个关键字间请用半角逗号","隔开
        /// </summary>
        public string Seodescription
        {
            get { return m_seodescription; }
            set { m_seodescription = value; }
        }

        /// <summary>
        /// 用于URL重写版块名称
        /// </summary>
        public string Rewritename
        {
            get { return m_rewritename == null ? "" : m_rewritename; }
            set { m_rewritename = value; }
        }
        
	}
}
