using System;
using System.Data;
using Discuz.Common;
//using System.Data.SqlClient;

namespace Discuz.Entity
{
    /// <summary>
    /// 用户组操作类
    /// </summary>
    [Serializable]
    public class UserGroupInfo
    {

        private int m_groupid;	//用户组ID
        private int m_radminid;	//关联管理组ID
        private int m_type;	//默认是私有,公用可以自己加入
        private int m_system;	//用户组类型
        private string m_grouptitle;	//用户组名称
        private int m_creditshigher;	//积分下限
        private int m_creditslower;	//积分上限
        private int m_stars;	//星星数目
        private string m_color;	//名称颜色
        private string m_groupavatar;	//用户组头像
        private int m_readaccess;	//阅读权限
        private int m_allowvisit;	//是否允许访问论坛
        private int m_allowpost;	//是否允许发新主题
        private int m_allowreply;	//是否允许回复
        private int m_allowpostpoll;	//是否允许发起投票
        private int m_allowdirectpost;	//允许使用 html 代码
        private int m_allowgetattach;	//是否允许下载附件
        private int m_allowpostattach;	//是否允许发布附件
        private int m_allowvote;	//是否允许参与投票
        private int m_allowmultigroups;	//是否允许加入多个用户组
        private int m_allowsearch;	//是否允许搜索
        //		private int m_allowavatar;	//是否允许使用头像, 0=不允许, 1=允许使用系统自带头像, 2=允许使用Url地址头像(且包括1), 3允许使用上传头像(且包括1和2)
        private int m_allowcstatus;	//是否允许自定义头衔
        private int m_allowuseblog;	//是否允许使用blog
        private int m_allowinvisible;	//是否允许隐身
        private int m_allowtransfer;	//是否允许积分转账
        private int m_allowsetreadperm;	//是否允许设置阅读积分权限
        private int m_allowsetattachperm;	//是否允许设置下载积分限制
        private int m_allowhidecode;	//是否允许使用hide代码
        private int m_allowhtmltitle;   //是否允许使用html标题
        private int m_allowhtml;	//是否允许发布html帖
        private int m_allowcusbbcode;	//是否允许使用Discuz!NT代码
        private int m_allownickname;	//是否允许使用昵称
        private int m_allowsigbbcode;	//签名是否支持Discuz!NT代码
        private int m_allowsigimgcode;	//签名是否支持图片代码
        private int m_allowviewpro;	//是否允许查看用户资料
        private int m_allowviewstats;	//是否允许查看统计
        private int m_disableperiodctrl;	//是否不受时间段限制
        private int m_reasonpm;	//是否将操作理由短消息通知作者
        private int m_maxprice;	//主题最高售价
        private int m_maxpmnum;	//短消息最多条数
        private int m_maxsigsize;	//签名最多字节
        private int m_maxattachsize;	//附件最大尺寸
        private int m_maxsizeperday;	//每天最大附件总尺寸
        private string m_attachextensions;	//允许附件类型
        private string m_raterange;	//允许的评分范围
        private int m_allowspace = 0;	//是否允许申请个人空间
        private int m_maxspaceattachsize = 0;	//个人空间附件最大尺寸
        private int m_maxspacephotosize = 0;	//个人空间照片最大尺寸
        private int m_allowdebate = 0;//是否允许发起辩论        
        private int m_allowbonus = 0; //是否允许发起悬赏        
        private int m_minbonusprice = 0;//最低悬赏价格        
        private int m_maxbonusprice = 0;//最高悬赏价格
        private int m_allowtrade = 0;//是否允许发表交易
        private int m_allowdiggs = 0;//是否允顶
        //private int m_maxfriendscount = 0;//好友人数上限
        private int m_modnewtopics = 0;//发主题是否需要审核 0不审核；1审核
        private int m_modnewposts = 0;//发回复是否需要审核 0不审核；1审核
        private int m_ignoreseccode = 0;//是否忽略页面的验证码检测   0为不忽略;1为忽略



        ///<summary>
        ///用户组ID
        ///</summary>
        public int Groupid
        {
            get { return m_groupid; }
            set { m_groupid = value; }
        }
        ///<summary>
        ///关联管理组ID
        ///</summary>
        public int Radminid
        {
            get { return m_radminid; }
            set { m_radminid = value; }
        }
        ///<summary>
        ///默认是私有,公用可以自己加入
        ///</summary>
        public int Type
        {
            get { return m_type; }
            set { m_type = value; }
        }
        ///<summary>
        ///用户组类型
        ///</summary>
        public int System
        {
            get { return m_system; }
            set { m_system = value; }
        }
        ///<summary>
        ///用户组名称
        ///</summary>
        public string Grouptitle
        {
            get { return m_grouptitle; }
            set
            {
                if ((m_color != null) && (m_color != string.Empty))
                    m_grouptitle = string.Format("<font color=\"{0}\">{1}</font>", m_color, value);
                else
                    m_grouptitle = value;
            }
        }
        ///<summary>
        ///积分下限
        ///</summary>
        public int Creditshigher
        {
            get { return m_creditshigher; }
            set { m_creditshigher = value; }
        }
        ///<summary>
        ///积分上限
        ///</summary>
        public int Creditslower
        {
            get { return m_creditslower; }
            set { m_creditslower = value; }
        }
        ///<summary>
        ///星星数目
        ///</summary>
        public int Stars
        {
            get { return m_stars; }
            set { m_stars = value; }
        }
        ///<summary>
        ///名称颜色
        ///</summary>
        public string Color
        {
            get { return m_color; }
            set { m_color = value; }
        }
        ///<summary>
        ///用户组头像
        ///</summary>
        public string Groupavatar
        {
            get { return m_groupavatar; }
            set { m_groupavatar = value; }
        }
        ///<summary>
        ///阅读权限
        ///</summary>
        public int Readaccess
        {
            get { return m_readaccess; }
            set { m_readaccess = value; }
        }
        ///<summary>
        ///是否允许访问论坛
        ///</summary>
        public int Allowvisit
        {
            get { return m_allowvisit; }
            set { m_allowvisit = value; }
        }
        ///<summary>
        ///是否允许发帖
        ///</summary>
        public int Allowpost
        {
            get { return m_allowpost; }
            set { m_allowpost = value; }
        }
        ///<summary>
        ///是否允许回复
        ///</summary>
        public int Allowreply
        {
            get { return m_allowreply; }
            set { m_allowreply = value; }
        }
        ///<summary>
        ///是否允许发起投票
        ///</summary>
        public int Allowpostpoll
        {
            get { return m_allowpostpoll; }
            set { m_allowpostpoll = value; }
        }
        ///<summary>
        ///允许使用 html 代码
        ///</summary>
        public int Allowdirectpost
        {
            get { return m_allowdirectpost; }
            set { m_allowdirectpost = value; }
        }
        ///<summary>
        ///是否允许下载附件
        ///</summary>
        public int Allowgetattach
        {
            get { return m_allowgetattach; }
            set { m_allowgetattach = value; }
        }
        ///<summary>
        ///是否发布附件
        ///</summary>
        public int Allowpostattach
        {
            get { return m_allowpostattach; }
            set { m_allowpostattach = value; }
        }
        ///<summary>
        ///是否允许参与投票
        ///</summary>
        public int Allowvote
        {
            get { return m_allowvote; }
            set { m_allowvote = value; }
        }
        ///<summary>
        ///是否允许加入多个用户组
        ///</summary>
        public int Allowmultigroups
        {
            get { return m_allowmultigroups; }
            set { m_allowmultigroups = value; }
        }
        ///<summary>
        ///是否允许搜索（0：不允许，1：允许搜索标题或全文，2：仅允许搜索标题）
        ///</summary>
        public int Allowsearch
        {
            get { return m_allowsearch; }
            set { m_allowsearch = value; }
        }
        ///<summary>
        ///是否允许使用头像, 0=不允许, 1=允许使用系统自带头像, 2=允许使用Url地址头像(且包括1), 3允许使用上传头像(且包括1和2)
        ///</summary>
        public int Allowavatar
        {
            get { return 3; }
            set { /*m_allowavatar = 3;*/ }
        }
        ///<summary>
        ///是否允许自定义头衔
        ///</summary>
        public int Allowcstatus
        {
            get { return m_allowcstatus; }
            set { m_allowcstatus = value; }
        }
        ///<summary>
        ///是否允许使用blog
        ///</summary>
        public int Allowuseblog
        {
            get { return m_allowuseblog; }
            set { m_allowuseblog = value; }
        }
        ///<summary>
        ///是否允许隐身
        ///</summary>
        public int Allowinvisible
        {
            get { return m_allowinvisible; }
            set { m_allowinvisible = value; }
        }
        ///<summary>
        ///是否允许积分转账
        ///</summary>
        public int Allowtransfer
        {
            get { return m_allowtransfer; }
            set { m_allowtransfer = value; }
        }
        ///<summary>
        ///是否允许设置阅读积分权限
        ///</summary>
        public int Allowsetreadperm
        {
            get { return m_allowsetreadperm; }
            set { m_allowsetreadperm = value; }
        }
        ///<summary>
        ///是否允许设置下载积分限制
        ///</summary>
        public int Allowsetattachperm
        {
            get { return m_allowsetattachperm; }
            set { m_allowsetattachperm = value; }
        }
        ///<summary>
        ///是否允许使用hide代码
        ///</summary>
        public int Allowhidecode
        {
            get { return m_allowhidecode; }
            set { m_allowhidecode = value; }
        }
        /// <summary>
        /// 是否允许使用html标题
        /// </summary>
        public int Allowhtmltitle
        {
            get { return m_allowhtmltitle; }
            set { m_allowhtmltitle = value; }
        }
        ///<summary>
        ///是否允许发布html帖
        ///</summary>
        public int Allowhtml
        {
            get { return m_allowhtml; }
            set { m_allowhtml = value; }
        }
        ///<summary>
        ///是否允许使用Discuz!NT代码
        ///</summary>
        public int Allowcusbbcode
        {
            get { return m_allowcusbbcode; }
            set { m_allowcusbbcode = value; }
        }
        ///<summary>
        ///是否允许使用昵称
        ///</summary>
        public int Allownickname
        {
            get { return m_allownickname; }
            set { m_allownickname = value; }
        }
        ///<summary>
        ///签名是否支持Discuz!NT代码
        ///</summary>
        public int Allowsigbbcode
        {
            get { return m_allowsigbbcode; }
            set { m_allowsigbbcode = value; }
        }
        ///<summary>
        ///签名是否支持图片代码
        ///</summary>
        public int Allowsigimgcode
        {
            get { return m_allowsigimgcode; }
            set { m_allowsigimgcode = value; }
        }
        ///<summary>
        ///是否允许查看用户资料
        ///</summary>
        public int Allowviewpro
        {
            get { return m_allowviewpro; }
            set { m_allowviewpro = value; }
        }
        ///<summary>
        ///是否允许查看统计
        ///</summary>
        public int Allowviewstats
        {
            get { return m_allowviewstats; }
            set { m_allowviewstats = value; }
        }
        ///<summary>
        ///是否不受时间段限制
        ///</summary>
        public int Disableperiodctrl
        {
            get { return m_disableperiodctrl; }
            set { m_disableperiodctrl = value; }
        }
        ///<summary>
        ///是否操作理由短消息通知作者
        ///</summary>
        public int Reasonpm
        {
            get { return m_reasonpm; }
            set { m_reasonpm = value; }
        }
        ///<summary>
        ///主题\附件最高售价
        ///</summary>
        public int Maxprice
        {
            get { return m_maxprice; }
            set { m_maxprice = value; }
        }
        ///<summary>
        ///短消息最多条数
        ///</summary>
        public int Maxpmnum
        {
            get { return m_maxpmnum; }
            set { m_maxpmnum = value; }
        }
        ///<summary>
        ///签名最多字节
        ///</summary>
        public int Maxsigsize
        {
            get { return m_maxsigsize; }
            set { m_maxsigsize = value; }
        }
        ///<summary>
        ///附件最大尺寸
        ///</summary>
        public int Maxattachsize
        {
            get { return m_maxattachsize; }
            set { m_maxattachsize = value; }
        }
        ///<summary>
        ///每天最大附件总尺寸
        ///</summary>
        public int Maxsizeperday
        {
            get { return m_maxsizeperday; }
            set { m_maxsizeperday = value; }
        }
        ///<summary>
        ///允许附件类型
        ///</summary>
        public string Attachextensions
        {
            get { return m_attachextensions; }
            set { m_attachextensions = value; }
        }
        ///<summary>
        ///允许的评分范围
        ///</summary>
        public string Raterange
        {
            get { return m_raterange.Trim(); }
            set { m_raterange = value; }
        }
        /// <summary>
        /// 是否允许申请个人空间
        /// </summary>
        public int Allowspace
        {
            get { return m_allowspace; }
            set { m_allowspace = value; }
        }
        /// <summary>
        /// 个人空间附件最大尺寸
        /// </summary>
        public int Maxspaceattachsize
        {
            get { return m_maxspaceattachsize; }
            set { m_maxspaceattachsize = value; }
        }

        /// <summary>
        /// 个人空间照片最大尺寸
        /// </summary>
        public int Maxspacephotosize
        {
            get { return m_maxspacephotosize; }
            set { m_maxspacephotosize = value; }
        }
        /// <summary>
        /// 是否允许发起辩论
        /// </summary>
        public int Allowdebate
        {
            get { return m_allowdebate; }
            set { m_allowdebate = value; }
        }
        /// <summary>
        /// 是否允许发起悬赏
        /// </summary>
        public int Allowbonus
        {
            get { return m_allowbonus; }
            set { m_allowbonus = value; }
        }
        /// <summary>
        /// 最低悬赏价格
        /// </summary>
        public int Minbonusprice
        {
            get { return m_minbonusprice; }
            set { m_minbonusprice = value; }
        }
        /// <summary>
        /// 最高悬赏价格
        /// </summary>
        public int Maxbonusprice
        {
            get { return m_maxbonusprice; }
            set { m_maxbonusprice = value; }
        }

        /// <summary>
        /// 是否允许发表交易
        /// </summary>
        public int Allowtrade
        {
            get { return m_allowtrade; }
            set { m_allowtrade = value; }
        }

        public int Allowdiggs
        {
            get { return m_allowdiggs; }
            set { m_allowdiggs = value; }
        }

        /// <summary>
        /// 好友人数上限
        /// </summary>
        //public int MaxFriendsCount
        //{
        //    get { return m_maxfriendscount; }
        //    set { m_maxfriendscount = value; }
        //}

        /// <summary>
        /// 发主题是否需要审核 0不审核；1审核
        /// </summary>
        public int ModNewTopics
        {
            get { return m_modnewtopics; }
            set { m_modnewtopics = value; }
        }

        /// <summary>
        /// 发回复是否需要审核 0不审核；1审核
        /// </summary>
        public int ModNewPosts
        {
            get { return m_modnewposts; }
            set { m_modnewposts = value; }
        }

        /// <summary>
        /// 是否忽略页面的验证码检测   0为不忽略;1为忽略
        /// </summary>
        public int Ignoreseccode
        {
            get { return m_ignoreseccode; }
            set { m_ignoreseccode = value; }
        }
    }
}
