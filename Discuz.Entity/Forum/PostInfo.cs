using System;

namespace Discuz.Entity
{
    /// <summary>
    /// 帖子信息描述类
    /// </summary>
    public class PostInfo
    {
        private int m_pid;	//帖子PID
        private int m_fid;	//归属版块ID
        private string m_forumname = string.Empty; //板块名称
        private int m_tid;	//归属主题ID
        private int m_parentid = 0;	//父帖ID
        private int m_layer = 0;	//帖子所处层次
        private string m_poster = string.Empty;	//帖子作者
        private int m_posterid;	//作者UID
        private string m_title = string.Empty;	//标题
        private string m_topictitle = string.Empty; //主题标题
        private string m_postdatetime = string.Empty;	//发表时间
        private string m_message = string.Empty;	//内容
        private string m_ip = string.Empty;	//IP地址
        private string m_lastedit = string.Empty; //最后编辑
        private int m_invisible;	//是否隐藏, 如果未通过审核则为隐藏  -3为被忽略
        private int m_usesig;	//是否启用签名
        private int m_htmlon;	//是否支持html
        private int m_bbcodeoff;	//是否支持html
        private int m_smileyoff;	//是否关闭smaile表情
        private int m_parseurloff;	//是否关闭url自动解析
        private int m_attachment = 0;	//是否含有附件
        private int m_rate = 0;	//评分分数
        private int m_ratetimes = 0;	//评分次数
        private int m_debateopinion; //辩论所持观点
        private string m_forumrewritename = string.Empty; //帖子所在版块重写名

        ///<summary>
        ///帖子PID
        ///</summary>
        public int Pid
        {
            get { return m_pid; }
            set { m_pid = value; }
        }
        ///<summary>
        ///归属版块ID
        ///</summary>
        public int Fid
        {
            get { return m_fid; }
            set { m_fid = value; }
        }

        /// <summary>
        /// 板块名称
        /// </summary>
        public string Forumname
        {
            get { return m_forumname; }
            set { m_forumname = value; }
        }

        ///<summary>
        ///归属主题ID
        ///</summary>
        public int Tid
        {
            get { return m_tid; }
            set { m_tid = value; }
        }
        ///<summary>
        ///父帖ID
        ///</summary>
        public int Parentid
        {
            get { return m_parentid; }
            set { m_parentid = value; }
        }
        ///<summary>
        ///帖子所处层次
        ///</summary>
        public int Layer
        {
            get { return m_layer; }
            set { m_layer = value; }
        }
        ///<summary>
        ///帖子作者
        ///</summary>
        public string Poster
        {
            get { return m_poster; }
            set { m_poster = value; }
        }
        ///<summary>
        ///作者UID
        ///</summary>
        public int Posterid
        {
            get { return m_posterid; }
            set { m_posterid = value; }
        }
        ///<summary>
        ///标题
        ///</summary>
        public string Title
        {
            get { return m_title.Trim(); }
            set { m_title = value; }
        }
        ///<summary>
        ///标题
        ///</summary>
        public string Topictitle
        {
            get { return m_topictitle.Trim(); }
            set { m_topictitle = value; }
        }
        ///<summary>
        ///发表时间
        ///</summary>
        public string Postdatetime
        {
            get { return m_postdatetime; }
            set { m_postdatetime = value; }
        }
        ///<summary>
        ///内容
        ///</summary>
        public string Message
        {
            get { return m_message; }
            set { m_message = value; }
        }
        ///<summary>
        ///IP地址
        ///</summary>
        public string Ip
        {
            get { return m_ip.Trim(); }
            set { m_ip = value; }
        }
        ///<summary>
        ///最后编辑
        ///</summary>
        public string Lastedit
        {
            get { return m_lastedit; }
            set { m_lastedit = value; }
        }
        ///<summary>
        ///是否隐藏, 如果未通过审核则为隐藏 0:显示 1:不显示 -1:待审核  -2:屏蔽
        ///</summary>
        public int Invisible
        {
            get { return m_invisible; }
            set { m_invisible = value; }
        }
        ///<summary>
        ///是否启用签名
        ///</summary>
        public int Usesig
        {
            get { return m_usesig; }
            set { m_usesig = value; }
        }
        ///<summary>
        ///是否支持html
        ///</summary>
        public int Htmlon
        {
            get { return m_htmlon; }
            set { m_htmlon = value; }
        }
        ///<summary>
        ///是否关闭smile表情
        ///</summary>
        public int Smileyoff
        {
            get { return m_smileyoff; }
            set { m_smileyoff = value; }
        }
        ///<summary>
        ///是否关闭Discuz!NT代码
        ///</summary>
        public int Bbcodeoff
        {
            get { return m_bbcodeoff; }
            set { m_bbcodeoff = value; }
        }
        ///<summary>
        ///是否关闭url自动解析
        ///</summary>
        public int Parseurloff
        {
            get { return m_parseurloff; }
            set { m_parseurloff = value; }
        }
        ///<summary>
        ///是否含有附件
        ///</summary>
        public int Attachment
        {
            get { return m_attachment; }
            set { m_attachment = value; }
        }
        ///<summary>
        ///评分分数
        ///</summary>
        public int Rate
        {
            get { return m_rate; }
            set { m_rate = value; }
        }
        ///<summary>
        ///评分次数
        ///</summary>
        public int Ratetimes
        {
            get { return m_ratetimes; }
            set { m_ratetimes = value; }
        }

        /// <summary>
        /// 辩论所持观点
        /// </summary>
        public int Debateopinion
        {
            get { return m_debateopinion; }
            set { m_debateopinion = value; }
        }

        /// <summary>
        /// 版块url重写名
        /// </summary>
        public string ForumRewriteName
        {
            get { return m_forumrewritename; }
            set { m_forumrewritename = value; }
        }
    }
}
