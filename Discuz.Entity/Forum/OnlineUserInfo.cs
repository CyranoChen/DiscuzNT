using System;

namespace Discuz.Entity
{
	/// <summary>
	/// 在线用户信息描述类
	/// </summary>
	public class OnlineUserInfo
	{
		
		private int m_olid;	//唯一ID
		private int m_userid;	//用户ID
		private string m_username;	//用户登录名
		private string m_nickname;	//用户昵称
		private string m_password;	//登录密码
		//private int m_tickcount;	//最后一次修改时间(秒级)
		private string m_ip;        //IP地址
		private short m_groupid;	//用户组ID
		private string m_olimg;		//对应的在线图例
		private short m_adminid;	//管理组ID
		private short m_invisible;	//用户是否隐身
		private short m_action;	//当前所在位置或所做的动作(1:首页 2:主题列表 3:查看主题 4:登录 5:发表主题 6:发表回复)
        private string m_actionname;	//当前所在位置或所做的动作(1:首页 2:主题列表 3:查看主题 4:登录 5:发表主题 6:发表回复)
        
		private short m_lastactivity;	//上一次所在的位置或所做的动作
		private string m_lastposttime;	//最后一次发帖时间(日期型)
		private string m_lastpostpmtime;	//最后一次发短消息时间(日期型)
		private string m_lastsearchtime;	//最后一次搜索时间(日期型)
		private string m_lastupdatetime;	//最后一次修改时间(日期型)
		private int m_forumid;	//最后一次所在的论坛版块ID
		private string m_forumname; //最后一次所在的论坛版块名称
		private int m_titleid;	//最后一次所在的论坛帖子ID
		private string m_title; //最后一次所在的论坛帖子名称
		private string m_verifycode;	//验证码
        private short m_newpms;  //新短消息数
        private short m_newnotices;  //新通知数
        //private short m_newfriendrequest;  //好友关系请求数
        //private short m_newapprequest;  //新应用请求数





		///<summary>
		///唯一ID
		///</summary>
		public int Olid
		{
			get { return m_olid;}
			set { m_olid = value;}
		}
		///<summary>
		///用户ID
		///</summary>
		public int Userid
		{
			get { return m_userid;}
			set { m_userid = value;}
		}
		///<summary>
		///用户登录名
		///</summary>
		public string Username
		{
			get { return m_username;}
			set { m_username = value;}
		}
		///<summary>
		///用户昵称
		///</summary>
		public string Nickname
		{
			get { return m_nickname;}
			set { m_nickname = value;}
		}
		///<summary>
		///登录密码
		///</summary>
		public string Password
		{
			get { return m_password;}
			set { m_password = value;}
		}
        /////<summary>
        /////最后一次修改时间(秒级)
        /////</summary>
        //public int Tickcount
        //{
        //    get { return m_tickcount;}
        //    set { m_tickcount = value;}
        //}
		///<summary>
		///IP地址
		///</summary>
		public string Ip
		{
			get { return m_ip;}
			set { m_ip = value;}
		}
		///<summary>
		///用户组ID
		///</summary>
		public short Groupid
		{
			get { return m_groupid;}
			set { m_groupid = value;}
		}
		///<summary>
		///对应的在线图例
		///</summary>
		public string Olimg
		{
			get { return m_olimg;}
			set { m_olimg = value;}
		}
		///<summary>
		///管理组ID
		///</summary>
		public short Adminid
		{
			get { return m_adminid;}
			set { m_adminid = value;}
		}
		///<summary>
		///用户是否隐身
		///</summary>
		public short Invisible
		{
			get { return m_invisible;}
			set { m_invisible = value;}
		}
        ///<summary>
        ///当前所在位置或所做的动作
        ///</summary>
        public short Action
        {
            get { return m_action; }
            set { m_action = value; }
        }

        ///<summary>
        ///当前所在位置或所做的动作
        ///</summary>
        public string Actionname
        {
            get { return m_actionname; }
            set { m_actionname = value; }
        }
        ///<summary>
		///上一次所在的位置或所做的动作
		///</summary>
		public short Lastactivity
		{
			get { return m_lastactivity;}
			set { m_lastactivity = value;}
		}
		///<summary>
		///最后一次发帖时间(日期型)
		///</summary>
		public string Lastposttime
		{
			get { return m_lastposttime;}
			set { m_lastposttime = value;}
		}
		///<summary>
		///最后一次发短消息时间(日期型)
		///</summary>
		public string Lastpostpmtime
		{
			get { return m_lastpostpmtime;}
			set { m_lastpostpmtime = value;}
		}
		///<summary>
		///最后一次搜索时间(日期型)
		///</summary>
		public string Lastsearchtime
		{
			get { return m_lastsearchtime;}
			set { m_lastsearchtime = value;}
		}

		///<summary>
		///最后一次修改时间(日期型)
		///</summary>
		public string Lastupdatetime
		{
			get { return m_lastupdatetime;}
			set { m_lastupdatetime = value;}
		}
		///<summary>
		///最后一次所在的论坛主题ID
		///</summary>
		public int Forumid
		{
			get { return m_forumid;}
			set { m_forumid = value;}
		}
		/// <summary>
		/// 最后一次所在的论坛版块名称
		/// </summary>
		public string Forumname
		{
			get { return m_forumname;}
			set { m_forumname = value;}
		}
		///<summary>
		///最后一次所在的论坛帖子ID
		///</summary>
		public int Titleid
		{
			get { return m_titleid;}
			set { m_titleid = value;}
		}
		/// <summary>
		/// 最后一次所在的论坛帖子标题
		/// </summary>
		public string Title
		{
			get { return m_title;}
			set { m_title = value;}
		}
		///<summary>
		///校验码
		///</summary>
		public string Verifycode
		{
			get { return m_verifycode;}
			set { m_verifycode = value;}
		}

        ///<summary>
        ///新短消息数
        ///</summary>
        public short Newpms
        {
            get { return m_newpms; }
            set { m_newpms = value; }
        }
        ///<summary>
        ///新通知数
        ///</summary>
        public short Newnotices
        {
            get { return m_newnotices; }
            set { m_newnotices = value; }
        }
        ///// <summary>
        ///// 好友关系请求数
        ///// </summary>
        //public short Newfriendrequest
        //{
        //    get { return m_newfriendrequest; }
        //    set { m_newfriendrequest = value; }
        //}
        ///// <summary>
        ///// 应用请求/通知数
        ///// </summary>
        //public short Newapprequest
        //{
        //    get { return m_newapprequest; }
        //    set { m_newapprequest = value; }
        //}

	}
}
