using System;

namespace Discuz.Entity
{
  

    /// <summary>
    /// ShowtopicPagePostInfo 的摘要说明。
    /// </summary>
    [Serializable]
    public class ShowtopicPagePostInfo : ICloneable
    {
        private string m_ubbmessage;
        private int m_id; //序号(楼层)
        private string m_postnocustom = "";
        private int m_pid; //帖子PID
        private int m_fid; //归属版块ID
        private string m_title; //标题
        private int m_layer; //帖子所处层次
        private string m_message; //内容
        private string m_ip; //IP地址
        private string m_lastedit; //最后编辑
        private string m_postdatetime; //发表时间
        private int m_attachment; //是否含有附件
        private string m_poster; //帖子作者
        private int m_posterid; //作者UID
        private int m_invisible; //是否隐藏, 如果未通过审核则为隐藏
        private int m_usesig; //是否启用签名
        private int m_htmlon; //是否支持html
        private int m_smileyoff; //是否关闭smaile表情
        private int m_parseurloff; //是否关闭url自动解析
        private int m_bbcodeoff; //是否支持html
        private int m_rate; //评分分数
        private int m_ratetimes; //评分次数
        private string m_nickname; //昵称
        private string m_username; //用户名
        private int m_groupid; //用户组ID
        private string m_email; //邮件地址
        private int m_showemail; //是否显示邮箱
        private int m_digestposts; //精华帖数
        private int m_credits; //积分数
        private float m_extcredits1; //扩展积分1
        private float m_extcredits2; //扩展积分2
        private float m_extcredits3; //扩展积分3
        private float m_extcredits4; //扩展积分4
        private float m_extcredits5; //扩展积分5
        private float m_extcredits6; //扩展积分6
        private float m_extcredits7; //扩展积分7
        private float m_extcredits8; //扩展积分8
        private int m_posts; //发帖数
        private string m_joindate; //注册时间
        private int m_onlinestate; //在线状态, 1为在线, 0为不在线
        private string m_lastactivity; //最后活动时间
        private int m_userinvisible; //是否隐身
        private string m_avatar; //头像
        private int m_avatarwidth; //头像宽度
        private int m_avatarheight; //头像高度
        private string m_medals; //勋章列表
        private string m_signature; //签名Html
        private string m_location; //来自
        private string m_customstatus; //自定义头衔
        private string m_website; //网站
        private string m_icq; //ICQ帐号
        private string m_qq; //QQ帐号
        private string m_msn; //MSN messenger帐号
        private string m_yahoo; //Yahoo messenger帐号
        private string m_skype; //skype帐号
        private string m_oltime;
        private string m_lastvisit;
        //扩展属性
        private string m_status; //头衔
        private int m_stars; //星星
        private int m_adindex; //广告序号
        private int m_spaceid;//空间Id
        private int m_gender;//性别
        private string m_bday;//生日
        private int m_debateopinion;//辩论观点,1,正方观点，2反方观点
        private int m_diggs;//作为辩论观点的支持数
        private bool m_digged = true;


        /// <summary>
        /// 最后登录时间
        /// </summary>
        public string Lastvisit
        {
            get { return m_lastvisit; }
            set { m_lastvisit = value; }
        }

        /// <summary>
        /// 在线时间
        /// </summary>
        public string Oltime
        {
            get { return m_oltime; }
            set { m_oltime = value; }
        }
        /// <summary>
        /// UBB内容
        /// </summary>
        public string Ubbmessage
        {
            get { return m_ubbmessage; }
            set { m_ubbmessage = value; }
        }

        /// <summary>
        /// 是否被顶
        /// </summary>
        public bool Digged
        {
            get { return m_digged; }
            set { m_digged = value; }
        }

        /// <summary>
        /// 序号(楼层)
        /// </summary>
        public int Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        /// <summary>
        /// 自定义楼层名称
        /// </summary>
        public string Postnocustom
        {
            get { return m_postnocustom; }
            set { m_postnocustom = value; }
        }
        /// <summary>
        /// 帖子PID
        /// </summary>
        public int Pid
        {
            get { return m_pid; }
            set { m_pid = value; }
        }

        /// <summary>
        /// 归属版块ID
        /// </summary>
        public int Fid
        {
            get { return m_fid; }
            set { m_fid = value; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        /// <summary>
        /// 帖子所处层次
        /// </summary>
        public int Layer
        {
            get { return m_layer; }
            set { m_layer = value; }
        }

        /// <summary>
        /// 内容
        /// </summary>
        public string Message
        {
            get { return m_message; }
            set { m_message = value; }
        }

        /// <summary>
        /// IP地址
        /// </summary>
        public string Ip
        {
            get { return m_ip; }
            set { m_ip = value; }
        }

        /// <summary>
        /// 最后编辑
        /// </summary>
        public string Lastedit
        {
            get { return m_lastedit; }
            set { m_lastedit = value; }
        }

        /// <summary>
        /// 发表时间
        /// </summary>
        public string Postdatetime
        {
            get { return m_postdatetime; }
            set { m_postdatetime = value; }
        }

        /// <summary>
        /// 是否含有附件
        /// </summary>
        public int Attachment
        {
            get { return m_attachment; }
            set { m_attachment = value; }
        }

        /// <summary>
        /// 帖子作者
        /// </summary>
        public string Poster
        {
            get { return m_poster; }
            set { m_poster = value; }
        }

        /// <summary>
        /// 作者UID
        /// </summary>
        public int Posterid
        {
            get { return m_posterid; }
            set { m_posterid = value; }
        }

        /// <summary>
        /// 是否隐藏, 如果未通过审核则为隐藏 1:不显示   0：显示  -1：待审核  -2：屏蔽
        /// </summary>
        public int Invisible
        {
            get { return m_invisible; }
            set { m_invisible = value; }
        }

        /// <summary>
        /// 是否启用签名
        /// </summary>
        public int Usesig
        {
            get { return m_usesig; }
            set { m_usesig = value; }
        }

        /// <summary>
        /// 是否支持html
        /// </summary>
        public int Htmlon
        {
            get { return m_htmlon; }
            set { m_htmlon = value; }
        }

        /// <summary>
        /// 是否关闭smaile表情
        /// </summary>
        public int Smileyoff
        {
            get { return m_smileyoff; }
            set { m_smileyoff = value; }
        }

        /// <summary>
        /// 是否关闭url自动解析
        /// </summary>
        public int Parseurloff
        {
            get { return m_parseurloff; }
            set { m_parseurloff = value; }
        }

        /// <summary>
        /// 是否支持html
        /// </summary>
        public int Bbcodeoff
        {
            get { return m_bbcodeoff; }
            set { m_bbcodeoff = value; }
        }

        /// <summary>
        /// 评分分数
        /// </summary>
        public int Rate
        {
            get { return m_rate; }
            set { m_rate = value; }
        }

        /// <summary>
        /// 评分次数
        /// </summary>
        public int Ratetimes
        {
            get { return m_ratetimes; }
            set { m_ratetimes = value; }
        }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname
        {
            get { return m_nickname; }
            set { m_nickname = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }

        /// <summary>
        /// 用户组ID
        /// </summary>
        public int Groupid
        {
            get { return m_groupid; }
            set { m_groupid = value; }
        }

        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email
        {
            get { return m_email; }
            set { m_email = value; }
        }

        /// <summary>
        /// 是否显示邮箱
        /// </summary>
        public int Showemail
        {
            get { return m_showemail; }
            set { m_showemail = value; }
        }

        /// <summary>
        /// 精华帖数
        /// </summary>
        public int Digestposts
        {
            get { return m_digestposts; }
            set { m_digestposts = value; }
        }

        /// <summary>
        /// 积分数
        /// </summary>
        public int Credits
        {
            get { return m_credits; }
            set { m_credits = value; }
        }

        /// <summary>
        /// 扩展积分1
        /// </summary>
        public float Extcredits1
        {
            get { return m_extcredits1; }
            set { m_extcredits1 = value; }
        }

        /// <summary>
        /// 扩展积分2
        /// </summary>
        public float Extcredits2
        {
            get { return m_extcredits2; }
            set { m_extcredits2 = value; }
        }

        /// <summary>
        /// 扩展积分3
        /// </summary>
        public float Extcredits3
        {
            get { return m_extcredits3; }
            set { m_extcredits3 = value; }
        }

        /// <summary>
        /// 扩展积分4
        /// </summary>
        public float Extcredits4
        {
            get { return m_extcredits4; }
            set { m_extcredits4 = value; }
        }

        /// <summary>
        /// 扩展积分5
        /// </summary>
        public float Extcredits5
        {
            get { return m_extcredits5; }
            set { m_extcredits5 = value; }
        }

        /// <summary>
        /// 扩展积分6
        /// </summary>
        public float Extcredits6
        {
            get { return m_extcredits6; }
            set { m_extcredits6 = value; }
        }

        /// <summary>
        /// 扩展积分7
        /// </summary>
        public float Extcredits7
        {
            get { return m_extcredits7; }
            set { m_extcredits7 = value; }
        }

        /// <summary>
        /// 扩展积分8
        /// </summary>
        public float Extcredits8
        {
            get { return m_extcredits8; }
            set { m_extcredits8 = value; }
        }

        /// <summary>
        /// 发帖数
        /// </summary>
        public int Posts
        {
            get { return m_posts; }
            set { m_posts = value; }
        }

        /// <summary>
        /// 注册时间
        /// </summary>
        public string Joindate
        {
            get { return m_joindate; }
            set { m_joindate = value; }
        }

        /// <summary>
        /// 在线状态, 1为在线, 0为不在线
        /// </summary>
        public int Onlinestate
        {
            get { return m_onlinestate; }
            set { m_onlinestate = value; }
        }

        /// <summary>
        /// 最后活动时间
        /// </summary>
        public string Lastactivity
        {
            get { return m_lastactivity; }
            set { m_lastactivity = value; }
        }

        /// <summary>
        /// 是否隐身
        /// </summary>
        public int Userinvisible
        {
            get { return m_userinvisible; }
            set { m_userinvisible = value; }
        }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar
        {
            get { return m_avatar; }
            set { m_avatar = value; }
        }

        /// <summary>
        /// 头像宽度
        /// </summary>
        public int Avatarwidth
        {
            get { return m_avatarwidth; }
            set { m_avatarwidth = value; }
        }

        /// <summary>
        /// 头像高度
        /// </summary>
        public int Avatarheight
        {
            get { return m_avatarheight; }
            set { m_avatarheight = value; }
        }

        /// <summary>
        /// 勋章列表
        /// </summary>
        public string Medals
        {
            get { return m_medals; }
            set { m_medals = value; }
        }

        /// <summary>
        /// 签名Html
        /// </summary>
        public string Signature
        {
            get { return m_signature; }
            set { m_signature = value; }
        }

        /// <summary>
        /// 来自
        /// </summary>
        public string Location
        {
            get { return m_location; }
            set { m_location = value; }
        }

        /// <summary>
        /// 自定义头衔
        /// </summary>
        public string Customstatus
        {
            get { return m_customstatus; }
            set { m_customstatus = value; }
        }

        /// <summary>
        /// 网站
        /// </summary>
        public string Website
        {
            get { return m_website; }
            set { m_website = value; }
        }

        /// <summary>
        /// ICQ帐号
        /// </summary>
        public string Icq
        {
            get { return m_icq; }
            set { m_icq = value; }
        }

        /// <summary>
        /// QQ帐号
        /// </summary>
        public string Qq
        {
            get { return m_qq; }
            set { m_qq = value; }
        }

        /// <summary>
        /// MSN messenger帐号
        /// </summary>
        public string Msn
        {
            get { return m_msn; }
            set { m_msn = value; }
        }

        /// <summary>
        /// Yahoo messenger帐号
        /// </summary>
        public string Yahoo
        {
            get { return m_yahoo; }
            set { m_yahoo = value; }
        }

        /// <summary>
        /// skype帐号
        /// </summary>
        public string Skype
        {
            get { return m_skype; }
            set { m_skype = value; }
        }

        /// <summary>
        /// 头衔
        /// </summary>
        public string Status
        {
            get { return m_status; }
            set { m_status = value; }
        }

        /// <summary>
        /// 星星
        /// </summary>
        public int Stars
        {
            get { return m_stars; }
            set { m_stars = value; }
        }

        /// <summary>
        /// 广告序号
        /// </summary>
        public int Adindex
        {
            get { return m_adindex; }
            set { m_adindex = value; }
        }

        /// <summary>
        /// 空间Id
        /// </summary>
        public int Spaceid
        {
            get { return m_spaceid; }
            set { m_spaceid = value; }
        }

        /// <summary>
        /// 性别
        /// </summary>
        public int Gender
        {
            get { return m_gender; }
            set { m_gender = value; }
        }

        /// <summary>
        /// 生日
        /// </summary>
        public string Bday
        {
            get { return m_bday; }
            set { m_bday = value; }
        }

        /// <summary>
        /// 辩论观点,1,正方观点，2反方观点
        /// </summary>
        public int Debateopinion
        {
            get { return m_debateopinion; }
            set { m_debateopinion = value; }
        }

        /// <summary>
        /// 作为辩论观点的支持数
        /// </summary>
        public int Diggs
        {
            get { return m_diggs; }
            set { m_diggs = value; }
        }

        public object Clone()
        {
           return this.MemberwiseClone(); //浅复制      
        }
    }
}
