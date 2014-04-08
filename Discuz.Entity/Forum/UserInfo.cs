using System;

namespace Discuz.Entity
{

	/// <summary>
	/// 简短的用户信息描述类
	/// </summary>    
	public class ShortUserInfo
	{
		
		private int m_uid;	//用户uid
		private string m_username;	//用户名
		private string m_nickname;	//昵称
		private string m_password;	//用户密码
		private int m_spaceid; //个人空间ID,0为用户还未申请空间;负数是用户已经申请,等待管理员开通,绝对值为开通以后的真实Spaceid;正数是用户已经开通的Spaceid
		private string m_secques;	//用户安全提问码
		private int m_gender;	//性别
        private int m_adminid;	//用户级别(1为管理员，2为超版，3为版主，0为普通用户)
		private int m_groupid;	//用户组ID
		private int m_groupexpiry;	//组过期时间
		private string m_extgroupids;	//扩展用户组
		private string m_regip;	//注册IP
		private string m_joindate;	//注册时间
		private string m_lastip;	//上次登录IP
		private string m_lastvisit;	//上次访问时间
		private string m_lastactivity;	//最后活动时间
		private string m_lastpost;	//最后发帖时间
		private int m_lastpostid;	//最后发帖id
		private string m_lastposttitle;	//最后发帖标题
		private int m_posts;	//发帖数
		private int m_digestposts;	//精华帖数
		private int m_oltime;	//在线时间
		private int m_pageviews;	//页面浏览量
		private int m_credits;	//积分数
		private float m_extcredits1;	//扩展积分1
		private float m_extcredits2;	//扩展积分2
		private float m_extcredits3;	//扩展积分3
		private float m_extcredits4;	//扩展积分4
		private float m_extcredits5;	//扩展积分5
		private float m_extcredits6;	//扩展积分6
		private float m_extcredits7;	//扩展积分7
		private float m_extcredits8;	//扩展积分8
		//private int m_avatarshowid;	//头像ID
		private string m_email = "";	//邮件地址
		private string m_bday;	//生日
		private int m_sigstatus;	//是否启用签名
		private int m_tpp;	//每页主题数
		private int m_ppp;	//每页帖数
		private int m_templateid;	//风格ID
		private int m_pmsound;	//短消息铃声
		private int m_showemail;	//是否显示邮箱
		private ReceivePMSettingType m_newsletter;	//是否接收论坛通知
		private int m_invisible;	//是否隐身
		//private string m_timeoffset;	//时差
		private int m_newpm;	//是否有新消息
		private int m_newpmcount;	//新短消息数量
		private int m_accessmasks;	//是否使用特殊权限
		private int m_onlinestate;	//在线状态, 1为在线, 0为不在线       
        private string m_salt;//用来二次MD5的字段



		///<summary>
		///用户UID
		///</summary>
		public int Uid
		{
			get { return m_uid;}
			set { m_uid = value;}
		}
		///<summary>
		///用户名
		///</summary>
		public string Username
		{
			get { return m_username.Trim();}
			set { m_username = value;}
		}
		///<summary>
		///用户密码
		///</summary>
		public string Password
		{
            get { return string.IsNullOrEmpty(m_password) ? "" : m_password.Trim(); }
			set { m_password = value;}
		}

		///<summary>
		///用户空间ID
		///</summary>
		public int Spaceid
		{
			get { return m_spaceid; }
			set { m_spaceid = value; }
		}

		///<summary>
		///用户安全提问码
		///</summary>
		public string Secques
		{
            get { return string.IsNullOrEmpty(m_secques) ? "" : m_secques.Trim(); }
			set { m_secques = value;}
		}
		///<summary>
		///性别
		///</summary>
		public int Gender
		{
			get { return m_gender;}
			set { m_gender = value;}
		}
		///<summary>
        ///用户级别(1为管理员，2为超版，3为版主，0为普通用户)
		///</summary>
		public int Adminid
		{
			get { return m_adminid;}
			set { m_adminid = value;}
		}
		///<summary>
		///用户组ID
		///</summary>
		public int Groupid
		{
			get { return m_groupid;}
			set { m_groupid = value;}
		}
		///<summary>
		///组过期时间
		///</summary>
		public int Groupexpiry
		{
			get { return m_groupexpiry;}
			set { m_groupexpiry = value;}
		}
		///<summary>
		///扩展用户组
		///</summary>
		public string Extgroupids
		{
			get { return m_extgroupids;}
			set { m_extgroupids = value;}
		}
		///<summary>
		///注册IP
		///</summary>
		public string Regip
		{
			get { return m_regip;}
			set { m_regip = value;}
		}
		///<summary>
		///注册时间
		///</summary>
		public string Joindate
		{
			get { return m_joindate;}
			set { m_joindate = value;}
		}
		///<summary>
		///上次登录IP
		///</summary>
		public string Lastip
		{
			get { return m_lastip;}
			set { m_lastip = value;}
		}
		///<summary>
		///上次访问时间
		///</summary>
		public string Lastvisit
		{
			get { return m_lastvisit;}
			set { m_lastvisit = value;}
		}
		///<summary>
		///最后活动时间
		///</summary>
		public string Lastactivity
		{
			get { return m_lastactivity;}
			set { m_lastactivity = value;}
		}
		///<summary>
		///最后发帖时间
		///</summary>
		public string Lastpost
		{
			get { return m_lastpost;}
			set { m_lastpost = value;}
		}
		/// <summary>
		/// 最后发帖id
		/// </summary>
		public int Lastpostid
		{
			get { return m_lastpostid;}
			set { m_lastpostid = value;}
		}		
		/// <summary>
		/// 最后发帖标题
		/// </summary>
		public string Lastposttitle
		{
			get { return m_lastposttitle;}
			set { m_lastposttitle = value;}
		}

		///<summary>
		///发帖数
		///</summary>
		public int Posts
		{
			get { return m_posts;}
			set { m_posts = value;}
		}
		///<summary>
		///精华帖数
		///</summary>
		public int Digestposts
		{
			get { return m_digestposts;}
			set { m_digestposts = value;}
		}
		///<summary>
		///在线时间
		///</summary>
		public int Oltime
		{
			get { return m_oltime;}
			set { m_oltime = value;}
		}
		///<summary>
		///页面浏览量
		///</summary>
		public int Pageviews
		{
			get { return m_pageviews;}
			set { m_pageviews = value;}
		}
		///<summary>
		///积分数
		///</summary>
		public int Credits
		{
			get { return m_credits;}
			set { m_credits = value;}
		}
		///<summary>
		///扩展积分1
		///</summary>
		public float Extcredits1
		{
			get { return m_extcredits1;}
			set { m_extcredits1 = value;}
		}
		///<summary>
		///扩展积分2
		///</summary>
		public float Extcredits2
		{
			get { return m_extcredits2;}
			set { m_extcredits2 = value;}
		}
		///<summary>
		///扩展积分3
		///</summary>
		public float Extcredits3
		{
			get { return m_extcredits3;}
			set { m_extcredits3 = value;}
		}
		///<summary>
		///扩展积分4
		///</summary>
		public float Extcredits4
		{
			get { return m_extcredits4;}
			set { m_extcredits4 = value;}
		}
		///<summary>
		///扩展积分5
		///</summary>
		public float Extcredits5
		{
			get { return m_extcredits5;}
			set { m_extcredits5 = value;}
		}
		///<summary>
		///扩展积分6
		///</summary>
		public float Extcredits6
		{
			get { return m_extcredits6;}
			set { m_extcredits6 = value;}
		}
		///<summary>
		///扩展积分7
		///</summary>
		public float Extcredits7
		{
			get { return m_extcredits7;}
			set { m_extcredits7 = value;}
		}
		///<summary>
		///扩展积分8
		///</summary>
		public float Extcredits8
		{
			get { return m_extcredits8;}
			set { m_extcredits8 = value;}
		}
		///<summary>
		///头像ID
		///</summary>
        //public int Avatarshowid
        //{
        //    get { return m_avatarshowid;}
        //    set { m_avatarshowid = value;}
        //}
		///<summary>
		///邮件地址
		///</summary>
		public string Email
		{
			get { return m_email.Trim();}
			set { m_email = value;}
		}
		///<summary>
		///生日
		///</summary>
		public string Bday
		{
			get { return m_bday.Trim();}
			set { m_bday = value;}
		}
		///<summary>
        ///是否启用签名
		///</summary>
		public int Sigstatus
		{
			get { return m_sigstatus;}
			set { m_sigstatus = value;}
		}
		///<summary>
		///每页主题数
		///</summary>
		public int Tpp
		{
			get { return m_tpp;}
			set { m_tpp = value;}
		}
		///<summary>
		///每页帖数
		///</summary>
		public int Ppp
		{
			get { return m_ppp;}
			set { m_ppp = value;}
		}
		///<summary>
		///风格ID
		///</summary>
		public int Templateid
		{
			get { return m_templateid;}
			set { m_templateid = value;}
		}
		///<summary>
		///短消息铃声
		///</summary>
		public int Pmsound
		{
			get { return m_pmsound;}
			set { m_pmsound = value;}
		}
		///<summary>
		///是否显示邮箱
		///</summary>
		public int Showemail
		{
			get { return m_showemail;}
			set { m_showemail = value;}
		}
		///<summary>
		///是否接收论坛通知
		///</summary>
        public ReceivePMSettingType Newsletter
		{
			get { return m_newsletter;}
			set { m_newsletter = value;}
		}
		///<summary>
		///是否隐身
		///</summary>
		public int Invisible
		{
			get { return m_invisible;}
			set { m_invisible = value;}
		}
		/*
		///<summary>
		///时差
		///</summary>
		public string Timeoffset
		{
			get { return m_timeoffset;}
			set { m_timeoffset = value;}
		}*/
		///<summary>
		///是否有新消息
		///</summary>
		public int Newpm
		{
			get { return m_newpm;}
			set { m_newpm = value;}
		}
		///<summary>
		///新短消息数量
		///</summary>
		public int Newpmcount
		{
			get { return m_newpmcount;}
			set { m_newpmcount = value;}
		}
		///<summary>
		///是否使用特殊权限
		///</summary>
		public int Accessmasks
		{
			get { return m_accessmasks;}
			set { m_accessmasks = value;}
		}
		/// <summary>
		/// 在线状态, 1为在线, 0为不在线
		/// </summary>
		public int Onlinestate
		{
			get { return m_onlinestate;}
			set { m_onlinestate = value;}
		}

		///<summary>
		///昵称
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
	/// 完整用户信息描述类
	/// </summary>
	public class UserInfo : ShortUserInfo
	{
		
		//以下为扩展信息
		private string m_website;	//网站
		private string m_icq;	//icq号码
		private string m_qq;	//qq号码
		private string m_yahoo;	//yahoo messenger帐号
		private string m_msn;	//msn messenger帐号
		private string m_skype;	//skype帐号
		private string m_location;	//来自
		private string m_customstatus;	//自定义头衔
        //private string m_avatar;	//头像
        //private int m_avatarwidth;	//头像宽度
        //private int m_avatarheight;	//头像高度
		private string m_medals; //勋章列表
		private string m_bio;	//自我介绍
		private string m_signature;	//签名
		private string m_sightml;	//签名Html(自动转换得到)
		private string m_authstr;	//验证码
		private string m_authtime;	//验证码生成日期
		private byte m_authflag;	//验证码使用标志(0 未使用,1 用户邮箱验证及用户信息激活, 2 用户密码找回)
        private string m_realname;  //用户实名
        private string m_idcard;    //用户身份证件号
        private string m_mobile;    //用户移动电话
        private string m_phone;     //用户固定电话
        private string m_ignorepm;  //短消息忽略列表

		///<summary>
		///网站
		///</summary>
		public string Website
		{
			get { return m_website.Trim();}
			set { m_website = value;}
		}
		///<summary>
		///icq号码
		///</summary>
		public string Icq
		{
			get { return m_icq.Trim();}
			set { m_icq = value;}
		}
		///<summary>
		///qq号码
		///</summary>
		public string Qq
		{
			get { return m_qq.Trim();}
			set { m_qq = value;}
		}
		///<summary>
		///yahoo messenger帐号
		///</summary>
		public string Yahoo
		{
			get { return m_yahoo.Trim();}
			set { m_yahoo = value;}
		}
		///<summary>
		///msn messenger帐号
		///</summary>
		public string Msn
		{
			get { return m_msn.Trim();}
			set { m_msn = value;}
		}
		///<summary>
		///skype帐号
		///</summary>
		public string Skype
		{
			get { return m_skype;}
			set { m_skype = value;}
		}
		///<summary>
		///来自
		///</summary>
		public string Location
		{
			get { return m_location.Trim();}
			set { m_location = value;}
		}
		///<summary>
		///自定义头衔
		///</summary>
		public string Customstatus
		{
			get { return m_customstatus;}
			set { m_customstatus = value;}
		}
		///<summary>
		///头像
		///</summary>
        //public string Avatar
        //{
        //    get { return m_avatar.Trim();}
        //    set { m_avatar = value;}
        //}
		///<summary>
		///头像宽度
		///</summary>
        //public int Avatarwidth
        //{
        //    get { return m_avatarwidth;}
        //    set { m_avatarwidth = value;}
        //}
		///<summary>
		///头像高度
		///</summary>
        //public int Avatarheight
        //{
        //    get { return m_avatarheight;}
        //    set { m_avatarheight = value;}
        //}
		/// <summary>
		/// 勋章列表
		/// </summary>
		public string  Medals
		{
			get { return m_medals;}
			set { m_medals = value;}
		}
		///<summary>
		///自我介绍
		///</summary>
		public string Bio
		{
			get { return m_bio.Trim();}
			set { m_bio = value;}
		}
		///<summary>
		///签名
		///</summary>
		public string Signature
		{
			get { return m_signature.Trim();}
			set { m_signature = value;}
		}
		///<summary>
		///签名Html(自动转换得到)
		///</summary>
		public string Sightml
		{
			get { return m_sightml.Trim();}
			set { m_sightml = value;}
		}
		
		
		///<summary>
		///验证码
		///</summary>
		public string Authstr
		{
			get { return m_authstr;}
			set { m_authstr = value;}
		}
		
		///<summary>
		///验证码
		///</summary>
		public string Authtime
		{
			get { return m_authtime;}
			set { m_authtime = value;}
		}
		
		///<summary>
		///验证码
		///</summary>
		public byte Authflag
		{
			get { return m_authflag;}
			set { m_authflag = value;}
		}

        /// <summary>
        /// 用户实名
        /// </summary>
        public string Realname
        {
            get { return m_realname; }
            set { m_realname = value; }
        }

        /// <summary>
        /// 用户身份证件号
        /// </summary>
        public string Idcard
        {
            get { return m_idcard; }
            set { m_idcard = value; }
        }

        /// <summary>
        /// 用户移动电话
        /// </summary>
        public string Mobile
        {
            get { return m_mobile; }
            set { m_mobile = value; }
        }

        /// <summary>
        /// 用户固定电话
        /// </summary>
        public string Phone
        {
            get { return m_phone; }
            set { m_phone = value; }
        }

        /// <summary>
        /// 短消息忽略列表
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
