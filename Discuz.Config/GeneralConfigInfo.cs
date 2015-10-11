using System;

namespace Discuz.Config
{
    /// <summary>
    /// 论坛基本设置描述类, 加[Serializable]标记为可序列化
    /// </summary>
    [Serializable]
    public class GeneralConfigInfo : IConfigInfo
    {
        #region 私有字段
        private string m_forumtitle = "Discuz!NT"; //论坛名称
        private string m_forumurl = "forumindex.aspx"; //论坛url地址
        private string m_webtitle = "Discuz!NT"; //网站名称
        private string m_weburl = ""; //论坛网站url地址
        private int m_licensed = 1; //是否显示商业授权链接
        private string m_icp = ""; //网站备案信息
        private int m_closed = 0; //论坛关闭
        private string m_closedreason = ""; //论坛关闭提示信息
        private int m_isframeshow = 0;   //是否以框架方式显示  1是　0不是
        private int m_admintools = 0; //是否使用管理员客户端工具,0=不使用,1=仅论坛创始人可用,2=管理员可用
        private int m_indexpage = 0; //首页类型, 0=论坛首页, 1=聚合首页
        private string m_linktext = "<a href=\"http://nt.discuz.net\" title=\"The Official Discuz!NT Site\" target=\"_blank\">Discuz!NT</a>"; //外部链接html
        private string m_statcode = ""; //统计代码

        private string m_passwordkey = "1234567890"; //用户密码Key

        private int m_regstatus = 1; //是否允许新用户注册,0不允许,1允许,2允许常规注册+开放式邀请,4允许封闭式邀请注册
        private int m_regadvance = 1; //注册时候是否显示高级选项
        private int m_realnamesystem = 0; //注册时是否启用实名制
        private string m_censoruser = "admin"; //用户信息保留关键字
        private int m_doublee = 0; //允许同一 Email 注册不同用户
        private int m_regverify = 0; //新用户注册验证
        private string m_accessemail = ""; //Email允许地址
        private string m_censoremail = ""; //Email禁止地址
        private int m_hideprivate = 1; //隐藏无权访问的论坛
        private int m_regctrl = 0; //IP 注册间隔限制(小时)
        private string m_ipregctrl = ""; //特殊 IP 注册限制
        private string m_ipdenyaccess = ""; //IP禁止访问列表
        private string m_ipaccess = ""; //IP访问列表
        private string m_adminipaccess = ""; //管理员后台IP访问列表
        private int m_newbiespan = 0; //新手见习期限(单位:分钟)
        private int m_welcomemsg = 1; //发送欢迎短消息
        private string m_welcomemsgtxt = "Welcome to visit this forum!"; //欢迎短消息内容
        private int m_rules = 1; //是否显示注册许可协议
        private string m_rulestxt = ""; //许可协议内容
        private int m_secques = 0; //是否启用用户登录安全提问

        private int m_templateid = 1; //默认论坛风格
        private int m_hottopic = 20; //热门话题最低帖数
        private int m_starthreshold = 2; //星星升级阀值
        private int m_visitedforums = 20; //显示最近访问论坛数量
        private int m_maxsigrows = 0; //最大签名高度(行)
        private int m_moddisplay = 0; //版主显示方式 0=平面显示 1=下拉菜单
        private int m_subforumsindex = 1; //首页是否显示论坛的下级子论坛
        private int m_stylejump = 1; //显示风格下拉菜单
        private int m_fastpost = 1; //快速发帖
        private int m_showsignatures = 1; //是否显示签名
        private int m_showavatars = 1; //是否显示头像
        private int m_showimages = 1; //是否在帖子中显示图片
        private int m_smiliesmax = 30; //帖子中最大允许的表情符数量

        private int m_archiverstatus = 1; //启用 Archiver
        private string m_seotitle = ""; //标题附加字
        private string m_seokeywords = ""; //Meta Keywords
        private string m_seodescription = ""; //Meta Description
        private string m_seohead = "<meta name=\"generator\" content=\"Discuz!NT\" />"; //其他头部信息

        private int m_rssstatus = 1; //rssstatus
        private int m_rssttl = 60; //RSS TTL(分钟)
        private int m_sitemapstatus = 1; //Sitemap是否开启
        private int m_sitemapttl = 12; //Sitemap TTL(小时)
        private int m_nocacheheaders = 0; //禁止浏览器缓冲
        private int m_fullmytopics = 1; //我的话题全文搜索 0=只搜索用户是主题发表者的主题 1=搜索用户是主题发表者或回复者的主题
        private int m_debug = 1; //显示程序运行信息
        private string m_rewriteurl = ""; //伪静态url的替换规则
        private string m_extname = ".aspx"; //伪静态url的扩展名

        private int m_whosonlinestatus = 3; //显示在线用户 0=不显示 1=仅在首页显示 2=仅在分论坛显示 3=在首页和分论坛显示
        private int m_maxonlinelist = 300; //最多显示在线人数
        private int m_userstatusby = 1; //衡量并显示用户头衔
        private int m_forumjump = 1; //显示论坛跳转菜单
        private int m_modworkstatus = 1; //论坛管理工作统计
        private int m_maxmodworksmonths = 3; //管理记录保留时间(月)

        private string m_seccodestatus = "register.aspx,login.aspx"; //使用验证码的页面列表,用","分隔 例如:register.aspx,login.aspx
        private int m_guestcachepagetimeout = 0; // 缓存游客页面的失效时间, 为0则不缓存, 大于0则缓存该值的时间(单位:分钟)
        private int m_topiccachemark = 0; //缓存游客查看主题页面的权重, 为0则不缓存, 范围0 - 100
        private int m_maxonlines = 5000; //最大在线人数
        private int m_postinterval = 0; //发帖灌水预防(秒)
        private int m_searchctrl = 0; //搜索时间限制(秒)
        private int m_maxspm = 5; //60 秒最大搜索次数

        private string m_visitbanperiods = ""; //禁止访问时间段
        private string m_postbanperiods = ""; //禁止发帖时间段
        private string m_postmodperiods = ""; //发帖审核时间段
        private string m_attachbanperiods = ""; //禁止下载附件时间段
        private string m_searchbanperiods = ""; //禁止全文搜索时间段

        private int m_memliststatus = 1; //允许查看会员列表
        private int m_dupkarmarate = 0; //允许重复评分
        private int m_minpostsize = 1; //帖子最小字数(字节)
        private int m_maxpostsize = 10000; //帖子最大字数(字节)
        private int m_tpp = 26; //每页主题数
        private int m_ppp = 16; //每页帖子数
        private int m_maxfavorites = 30; //收藏夹容量
        //private int m_maxavatarsize = 20480; //头像最大尺寸(字节)
        //private int m_maxavatarwidth = 120; //头像最大宽度(像素)
        //private int m_maxavatarheight = 120; //头像最大高度(像素)
        private int m_maxpolloptions = 10; //投票最大选项数
        private int m_maxattachments = 10; //最大允许的上传附件数

        private int m_attachimgpost = 1; //帖子中显示图片附件
        private int m_attachrefcheck = 1; //下载附件来路检查
        private int m_attachsave = 0; //附件保存方式  0=按年/月/日存入不同目录 1=按年/月/日/论坛存入不同目录 2=按论坛存入不同目录 3=按文件类型存入不同目录
        private int m_watermarkstatus = 3; //图片附件添加水印 0=不使用 1=左上 2=中上 3=右上 4=左中 ... 9=右下
        private int m_watermarktype = 0; //图片附件添加何种水印 0=文字 1=图片
        private int m_watermarktransparency = 5; //图片水印透明度 取值范围1--10 (10为不透明)
        private string m_watermarktext = "DiscuzNT";  //图片附件添加文字水印的内容 {1}表示论坛标题 {2}表示论坛地址 {3}表示当前日期 {4}表示当前时间, 例如: {3} {4}上传于{1} {2}
        private string m_watermarkpic = "watermark.gif";   //使用的水印图片的名称
        private string m_watermarkfontname = "Tahoma"; //图片附件添加文字水印的字体
        private int m_watermarkfontsize = 12; //图片附件添加文字水印的大小(像素)
        private int m_showattachmentpath = 0; //图片附件如果直接显示, 地址是否直接使用图片真实路径
        private int m_showimgattachmode = 0; //设置图片附件自动加载还是手工点击加载   0为自动
        private int m_attachimgquality = 80; //是否是高质量图片 取值范围0--100
        private int m_attachimgmaxheight = 0; //附件图片最大高度
        private int m_attachimgmaxwidth = 0; //附件图片最大宽度

        private int m_reasonpm = 0; //是否将管理操作的理由短消息通知作者
        private int m_moderactions = 1; //是否在主题查看页面显示管理操作
        private int m_karmaratelimit = 4; //评分时间限制(小时)
        private int m_losslessdel = 200; //删帖不减积分时间期限(天)
        private int m_edittimelimit = 10; //编辑帖子时间限制(分钟)
        private int m_deletetimelimit = 10;//删除帖子时间限制(分钟)
        private int m_editedby = 1; //编辑帖子附加编辑记录
        private int m_defaulteditormode = 1; //默认的编辑器模式 0=ubb代码编辑器 1=可视化编辑器
        private int m_allowswitcheditor = 1; //是否允许切换编辑器模式
        private int m_smileyinsert = 1; //显示可点击表情
        private string m_cookiedomain = "";//身份验证Cookie域

        private int m_passwordmode = 0; //密码模式, 0为默认(32位md5), 1为动网兼容模式(16位md5)
        private int m_bbcodemode = 0; //UBB模式, 0为默认(标准Discuz!NT代码), 1为动网UBB代码兼容模式
        private int m_fulltextsearch = 0; //是否启用SQLServer全文检索, 0为不使用, 1为使用
        private int m_cachelog = 0; //是否使用缓存日志, 0为不使用, 1为使用
        private int m_onlinetimeout = 10; //多久无动作视为离线

        private int m_topicqueuestats = 0; //是否开启主题统计队列功能
        private int m_topicqueuestatscount = 20; //主题统计队列长度(浏览量)

        private int m_displayratecount = 100; //评分记录现实的最大数量


        private string m_reportusergroup = "1"; //举报报告用户组
        private string m_photomangegroups = ""; //图片管理用户组

        private int m_silverlight = 0; //是否开启Silverlight功能  
        private int m_enablespace = 1; //是否启用个人空间服务，默认是启用
        private int m_enablealbum = 1; //是否启用相册服务，默认是启用

        private string m_spacename = "空间"; //空间名称
        private string m_albumname = "相册"; //相册名称
        private string m_spaceurl = "spaceindex.aspx"; //空间URL地址
        private string m_albumurl = "albumindex.aspx"; //相册URL地址
        private int m_browsecreatetemplate = 0; //浏览时如果模板不存在自动生成否，0为不使用，1为使用

        private string m_ratevalveset = "1,50,200,600,800"; //评分阀值;
        private int m_topictoblog = 1;//开通空间是转移过去的主题数

        private int m_aspxrewrite = 1; //是否使用伪aspx, 如:showforum-1.aspx等.
        private int m_viewnewtopicminute = 120; //设置前台"查看新帖"的分钟数

        //private int m_htmltitle = 0; //是否使用html标题
      //  private string m_htmltitleusergroup = "";  //可以使用html标题的用户组
        private int m_specifytemplate = 0;  //版块是否指定模板 (0:为未指定)
        private string m_verifyimageassemly = "";//验证码生成所使用的程序集
        private int m_mytopicsavetime = 30;//我的主题保留时间
        private int m_mypostsavetime = 30;//我的帖子保留时间        
        private int m_myattachmentsavetime = 30;//我的附件保留时间
        private int m_enabletag = 0;//是否启用Tag功能        
        private int m_enablemall = 1;//是否开启商场 (0:不开启 1:开启 2:开启高级模式)
        private int m_statscachelife = 120;//统计缓存时间(分钟)
        private int m_statstatus = 0;//是否开启浏览统计
        private int m_pvfrequence = 60; //页面访问量更新频率(页面数)
        private int m_oltimespan = 20;//用户在线时间更新时长(分钟):

        //private string m_defaultpositivecolor = "FF0000";//默认正方颜色        
        //private string m_defaultnegativecolor = "00FF00";//默认反方颜色
        //private string m_defaultpositivebordercolor = "336699";//默认正方边框颜色        
        //private string m_defaultnegativebordercolor = "A9CD62";//默认反方边框颜色        
        private string m_recommenddebates = "";

        private int m_gpp = 16;//每页商品数
        //private int m_allowdiggs = 0;//是否允顶

        private int m_debatepagesize = 5;
        private int m_hottagcount = 10; //首页热门标签数量
        private int m_disablepostad = 0;  //新用户广告强力屏蔽(0:不开启 1:开启)
        private int m_disablepostadregminute = 60;  //用户注册N分钟内进行新用户广告强力屏蔽功能检查,默认60分钟
        private int m_disablepostadpostcount = 5;   //用户发帖N帖内进行新用户广告强力屏蔽功能检查,默认5帖
        private string m_disablepostadregular = "((\\d{4}|\\d{4}-)?(\\d(?:\\s*)){7})|((\\d{3}|\\d{3}-)?\\d{8})|(1(?:\\s*)[35](?:\\s*)[0123456789](?:\\s*)(\\d(?:\\s*)){8})\r\n[qQ](.+?)(\\d(?:\\s*)){7}"; //新用户广告强力屏蔽功能正则表达式
        private int m_whosonlinecontract = 0;  //在线列表是否隐藏游客: 1 是 0 否
        private string m_postnocustom = "";
        private int m_iisurlrewrite = 0; //是否启用IIS的URL重写

        private int m_notificationreserveddays = 7;//通知在系统中保留天数
        private int m_maxindexsubforumcount = 0;//首页显示的子版最大数,0为不限制
        private int m_deletingexpireduserfrequency = 5;//删除过期用户频率(单位:时间)
        private int m_replynotificationstatus = 1;//是否设置默认回帖通知用户，0为不通知
        private int m_replyemailstatus = 1;//是否设置默认回帖短信息通知用户，0为不通知
        private string m_disallowfloatwin = "";//不开启弹窗效果的窗口ID列表
        private int m_allwoforumindexpost = 1;//是否开启首页发帖功能
        private int m_onlineoptimization = 0;//用户在线表性能优化开关
        private int m_onlineusercountcacheminute = 0;//在线用户数统计缓存时间，0为实时统计
        private int m_avatarmethod = 1;//头像显示方式，0使用动态地址调用头像，1使用静态地址调用头像
        private string m_msgforwardlist = "posttopic_succeed,editpost_succeed,postreply_succeed";//不需要提示信息的页面
        private int m_quickforward = 1;//是否开启跳转
        private int m_posttimestoragemedia = 0;//论坛通过读取用户最后发帖时间的存储介质  0：database  1：cookie
        private int m_disableshare = 1;//是否开启分享功能
        private string m_sharelist = "0|kaixin001|开心|1,1|sina|新浪微博|1,2|renren|人人|1,3|douban|豆瓣|1,4|sohu|白社会|1,5|qq|qq书签|1,6|google|google书签|1,7|vivi|爱问收藏|1,8|live|live收藏|1,9|favorite|收藏夹|1,10|baidu|百度收藏|1";//被分享的网站

        private string m_alipayaccout = "";//支付宝卖家帐号
        private int m_usealipaycustompartnerid = 0;//是否使用支付宝签约接口
        private string m_alipaypartnercheckkey = "";//交易安全校验码 (key)
        private string m_alipaypartnerid = "";//合作者身份 (partnerID)
        private int m_usealipayinstantpay = 0;//是否使用即时到帐接口

        private string m_tenpayaccout = "";//财付通卖家帐号
        private string m_tenpaysecretkey = "";//财付通卖家密码

        private int m_cashtocreditrate = 1;//现金/积分兑换比率
        private int m_mincreditstobuy = 0;//单次购买最小积分数额
        private int m_maxcreditstobuy = 1000;//单次购买最大积分数额
        private int m_userbuycreditscountperday = 15;//每日最多购买积分的次数

        private int m_shownewposticon = 1;//首页是否显示版块前有无新帖图标
        private string m_customauthorinfo = "gender,bday,credits,posts,joindate|uid,digestposts";//自定义用户显示项目
        private string m_jqueryurl = BaseConfigs.GetForumPath + "javascript/jquery.js";   //Jquery库地址
        private string m_verifycode = "";

        //private int m_manyouenabled = 0;//是否开启Manyou应用
        //private string m_mysiteid = "";//Manyou的站点id
        //private string m_mysitekey = "";//漫游的站点key
        //private string m_sitekey = "";//站点key

        private int m_installation = 1;//是否已经安装，类型中默认已安装，安装包中配置文件该项目应为未安装0

        private int m_emaillogin = 1;//是否允许Email 登陆

        private int m_ratelisttype = 0;//帖子评分列表展示类型 0，头像 1，列表 2，头像和列表

        //private int m_allowsearchfriendbyusername = 1;//是否允许通过用户名查找好友
        //private int m_friendgroupmaxcount = 10;//用户可创建好友分组的最多个数
        private int m_swfupload = 1;//1为开启flash批量上传，0为开启silverlight批量上传
        private int m_webgarden = 1;//当前站点web园进程数       

        private int m_allowchangewidth = 1;//是否允许用户切换页面的宽窄显示 1：允许 0：不允许
        private int m_showwidthmode = 0;//页面默认显示的宽窄模式 0：宽屏 1：窄屏

        private int m_datediff = 1;//是否开启人性化时间1:开启 0:关闭

        #endregion

        #region 属性
        /// <summary>
        /// 自定义用户显示项目
        /// </summary>
        public string Customauthorinfo
        {
            get { return m_customauthorinfo; }
            set { m_customauthorinfo = value; }

        }

        public int Debatepagesize
        {
            get { return m_debatepagesize; }
            set { m_debatepagesize = value; }

        }
        /// <summary>
        /// 论坛版权文字 (只读)
        /// </summary>
        public string Forumcopyright
        {
            get { return "&copy; 2001-" + DateTime.Now.Year.ToString() + " <a href=\"http://www.comsenz.com\" target=\"_blank\">Comsenz Inc</a>."; }
        }

        /// <summary>
        /// 论坛名称
        /// </summary>
        public string Forumtitle
        {
            get { return m_forumtitle; }
            set { m_forumtitle = value; }
        }

        /// <summary>
        /// 论坛url地址
        /// </summary>
        public string Forumurl
        {
            get { return m_forumurl; }
            set { m_forumurl = value; }
        }

        /// <summary>
        /// 网站名称
        /// </summary>
        public string Webtitle
        {
            get { return m_webtitle; }
            set { m_webtitle = value; }
        }

        /// <summary>
        /// 论坛网站url地址
        /// </summary>
        public string Weburl
        {
            get { return m_weburl; }
            set { m_weburl = value; }
        }

        /// <summary>
        /// 是否显示商业授权链接
        /// </summary>
        public int Licensed
        {
            get { return m_licensed; }
            set { m_licensed = value; }
        }

        /// <summary>
        /// 网站备案信息
        /// </summary>
        public string Icp
        {
            get { return m_icp; }
            set { m_icp = value; }
        }

        /// <summary>
        /// 论坛关闭
        /// </summary>
        public int Closed
        {
            get { return m_closed; }
            set { m_closed = value; }
        }

        /// <summary>
        /// 论坛关闭提示信息
        /// </summary>
        public string Closedreason
        {
            get { return m_closedreason; }
            set { m_closedreason = value; }
        }

        /// <summary>
        /// 是否以框架方式显示  1是　　 0不是
        /// </summary>

        public int Isframeshow
        {
            get { return m_isframeshow; }
            set { m_isframeshow = value; }
        }

        /// <summary>
        /// 是否使用管理员客户端工具,0=不使用,1=仅论坛创始人可用,2=管理员可用
        /// </summary>
        public int Admintools
        {
            get { return m_admintools; }
            set { m_admintools = value; }
        }

        /// <summary>
        /// 首页类型, 0=论坛首页, 1=聚合首页
        /// </summary>
        public int Indexpage
        {
            get { return m_indexpage; }
            set { m_indexpage = value; }
        }


        /// <summary>
        /// 外部链接html
        /// </summary>
        public string Linktext
        {
            get { return m_linktext; }
            set { m_linktext = value; }
        }

        /// <summary>
        /// 统计代码
        /// </summary>
        public string Statcode
        {
            get { return m_statcode; }
            set { m_statcode = value; }
        }

        /// <summary>
        /// 用户密码Key
        /// </summary>
        public string Passwordkey
        {
            get { return m_passwordkey; }
            set { m_passwordkey = value; }
        }

        /// <summary>
        /// 是否允许新用户注册
        /// </summary>
        public int Regstatus
        {
            get { return m_regstatus; }
            set { m_regstatus = value; }
        }

        /// <summary>
        /// 注册时候是否显示高级选项
        /// </summary>
        public int Regadvance
        {
            get { return m_regadvance; }
            set { m_regadvance = value; }
        }

        /// <summary>
        /// 注册时是否启用实名制
        /// </summary>
        public int Realnamesystem
        {
            get { return m_realnamesystem; }
            set { m_realnamesystem = value; }
        }



        /// <summary>
        /// 用户信息保留关键字
        /// </summary>
        public string Censoruser
        {
            get { return m_censoruser; }
            set { m_censoruser = value; }
        }

        /// <summary>
        /// 允许同一 Email 注册不同用户
        /// </summary>
        public int Doublee
        {
            get { return m_doublee; }
            set { m_doublee = value; }
        }

        /// <summary>
        /// 新用户注册验证
        /// </summary>
        public int Regverify
        {
            get { return m_regverify; }
            set { m_regverify = value; }
        }

        /// <summary>
        /// Email允许地址
        /// </summary>
        public string Accessemail
        {
            get { return m_accessemail; }
            set { m_accessemail = value; }
        }

        /// <summary>
        /// Email禁止地址
        /// </summary>
        public string Censoremail
        {
            get { return m_censoremail; }
            set { m_censoremail = value; }
        }

        /// <summary>
        /// 隐藏无权访问的论坛
        /// </summary>
        public int Hideprivate
        {
            get { return m_hideprivate; }
            set { m_hideprivate = value; }
        }

        /// <summary>
        /// IP 注册间隔限制(小时)
        /// </summary>
        public int Regctrl
        {
            get { return m_regctrl; }
            set { m_regctrl = value; }
        }

        /// <summary>
        /// 特殊 IP 注册限制
        /// </summary>
        public string Ipregctrl
        {
            get { return m_ipregctrl; }
            set { m_ipregctrl = value; }
        }

        /// <summary>
        /// IP禁止访问列表
        /// </summary>
        public string Ipdenyaccess
        {
            get { return m_ipdenyaccess; }
            set { m_ipdenyaccess = value; }
        }

        /// <summary>
        /// IP访问列表
        /// </summary>
        public string Ipaccess
        {
            get { return m_ipaccess; }
            set { m_ipaccess = value; }
        }

        /// <summary>
        /// 管理员后台IP访问列表
        /// </summary>
        public string Adminipaccess
        {
            get { return m_adminipaccess; }
            set { m_adminipaccess = value; }
        }

        /// <summary>
        /// 新手见习期限(单位:小时)
        /// </summary>
        public int Newbiespan
        {
            get { return m_newbiespan; }
            set { m_newbiespan = value; }
        }

        /// <summary>
        /// 发送欢迎短消息
        /// </summary>
        public int Welcomemsg
        {
            get { return m_welcomemsg; }
            set { m_welcomemsg = value; }
        }

        /// <summary>
        /// 欢迎短消息内容
        /// </summary>
        public string Welcomemsgtxt
        {
            get { return m_welcomemsgtxt; }
            set { m_welcomemsgtxt = value; }
        }

        /// <summary>
        /// 是否显示注册许可协议
        /// </summary>
        public int Rules
        {
            get { return m_rules; }
            set { m_rules = value; }
        }

        /// <summary>
        /// 许可协议内容
        /// </summary>
        public string Rulestxt
        {
            get { return m_rulestxt; }
            set { m_rulestxt = value; }
        }

        /// <summary>
        /// 是否启用用户登录安全提问
        /// </summary>
        public int Secques
        {
            get { return m_secques; }
            set { m_secques = value; }
        }

        /// <summary>
        /// 默认论坛风格
        /// </summary>
        public int Templateid
        {
            get { return m_templateid; }
            set { m_templateid = value; }
        }

        /// <summary>
        /// 热门话题最低帖数
        /// </summary>
        public int Hottopic
        {
            get { return m_hottopic; }
            set { m_hottopic = value; }
        }

        /// <summary>
        /// 星星升级阀值
        /// </summary>
        public int Starthreshold
        {
            get { return m_starthreshold; }
            set { m_starthreshold = value; }
        }

        /// <summary>
        /// 显示最近访问论坛数量
        /// </summary>
        public int Visitedforums
        {
            get { return m_visitedforums; }
            set { m_visitedforums = value; }
        }

        /// <summary>
        /// 最大签名高度(行)
        /// </summary>
        public int Maxsigrows
        {
            get { return m_maxsigrows; }
            set { m_maxsigrows = value; }
        }

        /// <summary>
        /// 版主显示方式 0=平面显示 1=下拉菜单
        /// </summary>
        public int Moddisplay
        {
            get { return m_moddisplay; }
            set { m_moddisplay = value; }
        }

        /// <summary>
        /// 首页是否显示论坛的下级子论坛
        /// </summary>
        public int Subforumsindex
        {
            get { return m_subforumsindex; }
            set { m_subforumsindex = value; }
        }

        /// <summary>
        /// 显示风格下拉菜单
        /// </summary>
        public int Stylejump
        {
            get { return m_stylejump; }
            set { m_stylejump = value; }
        }

        /// <summary>
        /// 快速发帖 0:不显示 1:只显示快速发表主题 2:只显示快速发表回复 3:同时显示快速发表主题和回复
        /// </summary>
        public int Fastpost
        {
            get { return m_fastpost; }
            set { m_fastpost = value; }
        }

        /// <summary>
        /// 是否显示签名
        /// </summary>
        public int Showsignatures
        {
            get { return m_showsignatures; }
            set { m_showsignatures = value; }
        }

        /// <summary>
        /// 是否显示头像
        /// </summary>
        public int Showavatars
        {
            get { return m_showavatars; }
            set { m_showavatars = value; }
        }

        /// <summary>
        /// 是否在帖子中显示图片
        /// </summary>
        public int Showimages
        {
            get { return m_showimages; }
            set { m_showimages = value; }
        }

        /// <summary>
        /// 帖子中最大允许的表情符数量
        /// </summary>
        public int Smiliesmax
        {
            get { return m_smiliesmax; }
            set { m_smiliesmax = value; }
        }

        /// <summary>
        /// 启用 Archiver
        /// </summary>
        public int Archiverstatus
        {
            get { return m_archiverstatus; }
            set { m_archiverstatus = value; }
        }

        /// <summary>
        /// 标题附加字
        /// </summary>
        public string Seotitle
        {
            get { return m_seotitle; }
            set { m_seotitle = value; }
        }

        /// <summary>
        /// Meta Keywords
        /// </summary>
        public string Seokeywords
        {
            get { return m_seokeywords; }
            set { m_seokeywords = value; }
        }

        /// <summary>
        /// Meta Description
        /// </summary>
        public string Seodescription
        {
            get { return m_seodescription; }
            set { m_seodescription = value; }
        }

        /// <summary>
        /// 其他头部信息
        /// </summary>
        public string Seohead
        {
            get { return m_seohead; }
            set { m_seohead = value; }
        }

        /// <summary>
        /// 是否启用RSS
        /// </summary>
        public int Rssstatus
        {
            get { return m_rssstatus; }
            set { m_rssstatus = value; }
        }

        /// <summary>
        /// RSS TTL(分钟)
        /// </summary>
        public int Rssttl
        {
            get { return m_rssttl; }
            set { m_rssttl = value; }
        }

        /// <summary>
        /// 是否启用baidu sitemap
        /// </summary>
        public int Sitemapstatus
        {
            get { return m_sitemapstatus; }
            set { m_sitemapstatus = value; }
        }

        /// <summary>
        /// baidu sitemap TTL(分钟)
        /// </summary>
        public int Sitemapttl
        {
            get { return m_sitemapttl; }
            set { m_sitemapttl = value; }
        }

        /// <summary>
        /// 禁止浏览器缓冲
        /// </summary>
        public int Nocacheheaders
        {
            get { return m_nocacheheaders; }
            set { m_nocacheheaders = value; }
        }

        /// <summary>
        /// 我的话题全文搜索 0=只搜索用户是主题发表者的主题 1=搜索用户是主题发表者或回复者的主题
        /// </summary>
        public int Fullmytopics
        {
            get { return m_fullmytopics; }
            set { m_fullmytopics = value; }
        }

        /// <summary>
        /// 显示程序运行信息
        /// </summary>
        public int Debug
        {
            get { return m_debug; }
            set { m_debug = value; }
        }

        /// <summary>
        /// 伪静态url的替换规则
        /// </summary>
        public string Rewriteurl
        {
            get { return m_rewriteurl; }
            set { m_rewriteurl = value; }
        }

        /// <summary>
        /// 伪静态url的扩展名
        /// </summary>
        public string Extname
        {
            get { return m_extname; }
            set { m_extname = value; }
        }


        /// <summary>
        /// 显示在线用户 0=不显示 1=仅在首页显示 2=仅在分论坛显示 3=在首页和分论坛显示
        /// </summary>
        public int Whosonlinestatus
        {
            get { return m_whosonlinestatus; }
            set { m_whosonlinestatus = value; }
        }

        /// <summary>
        /// 最多显示在线人数
        /// </summary>
        public int Maxonlinelist
        {
            get { return m_maxonlinelist; }
            set { m_maxonlinelist = value; }
        }

        /// <summary>
        /// 衡量并显示用户头衔
        /// </summary>
        public int Userstatusby
        {
            get { return m_userstatusby; }
            set { m_userstatusby = value; }
        }

        /// <summary>
        /// 显示论坛跳转菜单
        /// </summary>
        public int Forumjump
        {
            get { return m_forumjump; }
            set { m_forumjump = value; }
        }

        /// <summary>
        /// 论坛管理工作统计
        /// </summary>
        public int Modworkstatus
        {
            get { return m_modworkstatus; }
            set { m_modworkstatus = value; }
        }

        /// <summary>
        /// 管理记录保留时间(月)
        /// </summary>
        public int Maxmodworksmonths
        {
            get { return m_maxmodworksmonths; }
            set { m_maxmodworksmonths = value; }
        }

        /// <summary>
        /// 使用验证码的页面列表,用","分隔 例如:register.aspx,login.aspx
        /// </summary>
        public string Seccodestatus
        {
            get { return m_seccodestatus; }
            set { m_seccodestatus = value; }
        }

        /// <summary>
        /// 缓存游客页面的失效时间
        /// </summary>
        public int Guestcachepagetimeout
        {
            get { return m_guestcachepagetimeout; }
            set { m_guestcachepagetimeout = value; }
        }

        /// <summary>
        /// 缓存游客查看主题页面的权重, 为0则不缓存, 范围0 - 100
        /// </summary>
        public int Topiccachemark
        {
            get { return m_topiccachemark; }
            set { m_topiccachemark = value; }
        }

        /// <summary>
        /// 最大在线人数
        /// </summary>
        public int Maxonlines
        {
            get { return m_maxonlines; }
            set { m_maxonlines = value; }
        }

        /// <summary>
        /// 发帖灌水预防(秒)
        /// </summary>
        public int Postinterval
        {
            get { return m_postinterval; }
            set { m_postinterval = value; }
        }

        /// <summary>
        /// 搜索时间限制(秒)
        /// </summary>
        public int Searchctrl
        {
            get { return m_searchctrl; }
            set { m_searchctrl = value; }
        }

        /// <summary>
        /// 60 秒最大搜索次数
        /// </summary>
        public int Maxspm
        {
            get { return m_maxspm; }
            set { m_maxspm = value; }
        }

        /// <summary>
        /// 禁止访问时间段
        /// </summary>
        public string Visitbanperiods
        {
            get { return m_visitbanperiods; }
            set { m_visitbanperiods = value; }
        }

        /// <summary>
        /// 禁止发帖时间段
        /// </summary>
        public string Postbanperiods
        {
            get { return m_postbanperiods; }
            set { m_postbanperiods = value; }
        }

        /// <summary>
        /// 发帖审核时间段
        /// </summary>
        public string Postmodperiods
        {
            get { return m_postmodperiods; }
            set { m_postmodperiods = value; }
        }

        /// <summary>
        /// 禁止下载附件时间段
        /// </summary>
        public string Attachbanperiods
        {
            get { return m_attachbanperiods; }
            set { m_attachbanperiods = value; }
        }

        /// <summary>
        /// 禁止全文搜索时间段
        /// </summary>
        public string Searchbanperiods
        {
            get { return m_searchbanperiods; }
            set { m_searchbanperiods = value; }
        }

        /// <summary>
        /// 允许查看会员列表
        /// </summary>
        public int Memliststatus
        {
            get { return m_memliststatus; }
            set { m_memliststatus = value; }
        }

        /// <summary>
        /// 允许重复评分
        /// </summary>
        public int Dupkarmarate
        {
            get { return m_dupkarmarate; }
            set { m_dupkarmarate = value; }
        }

        /// <summary>
        /// 帖子最小字数(字节)
        /// </summary>
        public int Minpostsize
        {
            get { return m_minpostsize; }
            set { m_minpostsize = value; }
        }

        /// <summary>
        /// 帖子最大字数(字节)
        /// </summary>
        public int Maxpostsize
        {
            get { return m_maxpostsize; }
            set { m_maxpostsize = value; }
        }

        /// <summary>
        /// 每页主题数
        /// </summary>
        public int Tpp
        {
            get { return m_tpp; }
            set { m_tpp = value; }
        }

        /// <summary>
        /// 每页帖子数
        /// </summary>
        public int Ppp
        {
            get { return m_ppp; }
            set { m_ppp = value; }
        }


        /// <summary>
        /// 收藏夹容量
        /// </summary>
        public int Maxfavorites
        {
            get { return m_maxfavorites; }
            set { m_maxfavorites = value; }
        }

        ///// <summary>
        ///// 头像最大尺寸(字节)
        ///// </summary>
        //public int Maxavatarsize
        //{
        //    get { return m_maxavatarsize; }
        //    set { m_maxavatarsize = value; }
        //}

        ///// <summary>
        ///// 头像最大宽度(像素)
        ///// </summary>
        //public int Maxavatarwidth
        //{
        //    //get { return m_maxavatarwidth<130 ? 130: m_maxavatarwidth;} //当宽度小于165时,返回165
        //    get { return m_maxavatarwidth; } //当宽度小于165时,返回165
        //    set { m_maxavatarwidth = value; }
        //}

        ///// <summary>
        ///// 头像最大高度(像素)
        ///// </summary>
        //public int Maxavatarheight
        //{
        //    get { return m_maxavatarheight; }
        //    set { m_maxavatarheight = value; }
        //}

        /// <summary>
        /// 投票最大选项数
        /// </summary>
        public int Maxpolloptions
        {
            get { return m_maxpolloptions; }
            set { m_maxpolloptions = value; }
        }

        /// <summary>
        /// 最大允许的上传附件数
        /// </summary>
        public int Maxattachments
        {
            get { return m_maxattachments; }
            set { m_maxattachments = value; }
        }

        /// <summary>
        /// 帖子中显示图片附件
        /// </summary>
        public int Attachimgpost
        {
            get { return m_attachimgpost; }
            set { m_attachimgpost = value; }
        }

        /// <summary>
        /// 下载附件来路检查
        /// </summary>
        public int Attachrefcheck
        {
            get { return m_attachrefcheck; }
            set { m_attachrefcheck = value; }
        }

        /// <summary>
        /// 附件保存方式 0=全部存入同一目录 1=按论坛存入不同目录 2=按文件类型存入不同目录 3=按年月日存入不同目录
        /// </summary>
        public int Attachsave
        {
            get { return m_attachsave; }
            set { m_attachsave = value; }
        }

        /// <summary>
        /// 图片附件添加水印 0=不使用 1=左上 2=中上 3=右上 4=左中 ... 9=右下
        /// </summary>
        public int Watermarkstatus
        {
            get { return m_watermarkstatus; }
            set { m_watermarkstatus = value; }
        }

        /// <summary>
        /// 图片附件添加何种水印 0=文字 1=图片
        /// </summary>
        public int Watermarktype
        {
            get { return m_watermarktype; }
            set { m_watermarktype = value; }
        }

        /// <summary>
        /// 图片水印透明度 取值范围1--10 (10为不透明)
        /// </summary>
        public int Watermarktransparency
        {
            get { return m_watermarktransparency; }
            set { m_watermarktransparency = value; }
        }

        /// <summary>
        /// 图片附件添加文字水印的内容 {1}表示论坛标题 {2}表示论坛地址 {3}表示当前日期 {4}表示当前时间, 例如: {3} {4}上传于{1} {2}
        /// </summary>
        public string Watermarktext
        {
            get { return m_watermarktext; }
            set { m_watermarktext = value; }
        }

        /// <summary>
        /// 使用的水印图片的名称
        /// </summary>
        public string Watermarkpic
        {
            get { return m_watermarkpic; }
            set { m_watermarkpic = value; }
        }

        /// <summary>
        /// 图片附件添加文字水印的字体
        /// </summary>
        public string Watermarkfontname
        {
            get { return m_watermarkfontname; }
            set { m_watermarkfontname = value; }
        }

        /// <summary>
        /// 图片附件添加文字水印的大小(像素)
        /// </summary>
        public int Watermarkfontsize
        {
            get { return m_watermarkfontsize; }
            set { m_watermarkfontsize = value; }
        }

        /// <summary>
        /// 图片附件如果直接显示, 地址是否直接使用图片真实路径
        /// </summary>
        public int Showattachmentpath
        {
            get { return m_showattachmentpath; }
            set { m_showattachmentpath = value; }
        }

        /// <summary>
        /// 附件图片质量　取值范围 1是　0不是
        /// </summary>
        public int Attachimgquality
        {
            get { return m_attachimgquality; }
            set { m_attachimgquality = value; }
        }

        /// <summary>
        /// 附件图片最大高度 0为不受限制
        /// </summary>
        public int Attachimgmaxheight
        {
            get { return m_attachimgmaxheight; }
            set { m_attachimgmaxheight = value; }
        }

        /// <summary>
        /// 附件图片最大宽度 0为不受限制
        /// </summary>
        public int Attachimgmaxwidth
        {
            get { return m_attachimgmaxwidth; }
            set { m_attachimgmaxwidth = value; }
        }

        /// <summary>
        /// 是否将管理操作的理由短消息通知作者
        /// </summary>
        public int Reasonpm
        {
            get { return m_reasonpm; }
            set { m_reasonpm = value; }
        }

        /// <summary>
        /// 是否在主题查看页面显示管理操作
        /// </summary>
        public int Moderactions
        {
            get { return m_moderactions; }
            set { m_moderactions = value; }
        }

        /// <summary>
        /// 评分时间限制(小时)
        /// </summary>
        public int Karmaratelimit
        {
            get { return m_karmaratelimit; }
            set { m_karmaratelimit = value; }
        }

        /// <summary>
        /// 删帖不减积分时间期限(天)
        /// </summary>
        public int Losslessdel
        {
            get { return m_losslessdel; }
            set { m_losslessdel = value; }
        }

        /// <summary>
        /// 编辑帖子时间限制(分钟)
        /// </summary>
        public int Edittimelimit
        {
            get { return m_edittimelimit; }
            set { m_edittimelimit = value; }
        }

        /// <summary>
        /// 编辑帖子附加编辑记录
        /// </summary>
        public int Editedby
        {
            get { return m_editedby; }
            set { m_editedby = value; }
        }

        /// <summary>
        /// 默认的编辑器模式 0=ubb代码编辑器 1=可视化编辑器
        /// </summary>
        public int Defaulteditormode
        {
            get { return m_defaulteditormode; }
            set { m_defaulteditormode = value; }
        }

        /// <summary>
        /// 是否允许切换编辑器模式
        /// </summary>
        public int Allowswitcheditor
        {
            get { return m_allowswitcheditor; }
            set { m_allowswitcheditor = value; }
        }

        /// <summary>
        /// 显示可点击表情
        /// </summary>
        public int Smileyinsert
        {
            get { return m_smileyinsert; }
            set { m_smileyinsert = value; }
        }

        /// <summary>
        /// 身份验证Cookie域
        /// </summary>
        public string CookieDomain
        {
            get { return m_cookiedomain; }
            set { m_cookiedomain = value; }
        }




        /// <summary>
        /// 密码模式, 0为默认(32位md5), 1为动网兼容模式(16位md5)
        /// </summary>
        public int Passwordmode
        {
            get { return m_passwordmode; }
            set { m_passwordmode = value; }
        }

        /// <summary>
        /// UBB模式, 0为默认(标准Discuz!代码), 1为动网UBB代码兼容模式
        /// </summary>
        public int Bbcodemode
        {
            get { return m_bbcodemode; }
            set { m_bbcodemode = value; }
        }

        /// <summary>
        /// 是否启用SQLServer全文检索, 0为不使用, 1为使用
        /// </summary>
        public int Fulltextsearch
        {
            get { return m_fulltextsearch; }
            set { m_fulltextsearch = value; }
        }

        /// <summary>
        /// 是否使用缓存日志, 0为不使用, 1为使用
        /// </summary>
        public int Cachelog
        {
            get { return m_cachelog; }
            set { m_cachelog = value; }
        }

        /// <summary>
        /// 多久无动作视为离线
        /// </summary>
        public int Onlinetimeout
        {
            get { return m_onlinetimeout; }
            set { m_onlinetimeout = value; }
        }

        /// <summary>
        /// 主题浏览统计队列
        /// </summary>
        public int TopicQueueStats
        {
            get { return m_topicqueuestats; }
            set { m_topicqueuestats = value; }
        }

        /// <summary>
        /// 主题浏览统计队列数值
        /// </summary>
        public int TopicQueueStatsCount
        {
            get { return m_topicqueuestatscount; }
            set { m_topicqueuestatscount = value; }
        }

        /// <summary>
        /// 展现的评分记录最大条数
        /// </summary>
        public int DisplayRateCount
        {
            get { return m_displayratecount; }
            set { m_displayratecount = value; }
        }

        /// <summary>
        /// 举报报告用户组
        /// </summary>
        public string Reportusergroup
        {
            get { return m_reportusergroup; }
            set { m_reportusergroup = value; }
        }

        /// <summary>
        /// 图片管理用户组
        /// </summary>
        public string Photomangegroups
        {
            get { return m_photomangegroups; }
            set { m_photomangegroups = value; }
        }

        /// <summary>
        /// 是否开启Silverlight
        /// </summary>
        public int Silverlight
        {
            get { return m_silverlight; }
            set { m_silverlight = value; }
        }

        /// <summary>
        /// 是否开启个人空间服务
        /// </summary>
        public int Enablespace
        {
            get { return m_enablespace; }
            set { m_enablespace = value; }
        }

        /// <summary>
        /// 是否开启相册服务
        /// </summary>
        public int Enablealbum
        {
            get { return m_enablealbum; }
            set { m_enablealbum = value; }
        }


        /// <summary>
        /// 空间名称
        /// </summary>
        public string Spacename
        {
            get { return m_spacename; }
            set { m_spacename = value; }
        }

        /// <summary>
        /// 空间URL地址
        /// </summary>
        public string Spaceurl
        {
            get { return m_spaceurl; }
            set { m_spaceurl = value; }
        }

        /// <summary>
        /// 相册名称
        /// </summary>
        public string Albumname
        {
            get { return m_albumname; }
            set { m_albumname = value; }
        }


        /// <summary>
        /// 相册URL地址
        /// </summary>
        public string Albumurl
        {
            get { return m_albumurl; }
            set { m_albumurl = value; }
        }

        /// <summary>
        /// 浏览时创建模板
        /// </summary>
        public int BrowseCreateTemplate
        {
            get { return m_browsecreatetemplate; }
            set { m_browsecreatetemplate = value; }
        }

        /// <summary>
        /// 评分阀值
        /// </summary>
        public string Ratevalveset
        {
            get { return m_ratevalveset; }
            set { m_ratevalveset = value; }
        }

        /// <summary>
        /// 开通空间后被转移的主题数
        /// </summary>
        public int Topictoblog
        {
            get { return m_topictoblog; }
            set { m_topictoblog = value; }
        }


        /// <summary>
        /// 是否使用伪aspx, 如:showforum-1.aspx等.
        /// </summary>
        public int Aspxrewrite
        {
            get { return m_aspxrewrite; }
            set { m_aspxrewrite = value; }
        }

        /// <summary>
        /// 查看新帖的分钟数
        /// </summary>
        public int Viewnewtopicminute
        {
            get { return m_viewnewtopicminute; }
            set { m_viewnewtopicminute = value; }
        }

        /// <summary>
        /// 是否使用HTML标题
        /// </summary>
        //public int Htmltitle
        //{
        //    get { return m_htmltitle; }
        //    set { m_htmltitle = value; }
        //}

        /// <summary>
        /// 可以使用html标题的用户组
        /// </summary>
        //public string Htmltitleusergroup
        //{
        //    get { return m_htmltitleusergroup; }
        //    set { m_htmltitleusergroup = value; }
        //}


        /// <summary>
        /// 版块是否指定模板
        /// </summary>
        public int Specifytemplate
        {
            get { return m_specifytemplate; }
            set { m_specifytemplate = value; }
        }
        /// <summary>
        /// 验证码生成所使用的程序集
        /// </summary>
        public string VerifyImageAssemly
        {
            get { return m_verifyimageassemly; }
            set { m_verifyimageassemly = value; }
        }

        /// <summary>
        /// 我的主题保留时间
        /// </summary>
        public int Mytopicsavetime
        {
            get { return m_mytopicsavetime; }
            set { m_mytopicsavetime = value; }
        }

        /// <summary>
        /// 我的帖子保留时间
        /// </summary>
        public int Mypostsavetime
        {
            get { return m_mypostsavetime; }
            set { m_mypostsavetime = value; }
        }

        /// <summary>
        /// 我的附件保留时间
        /// </summary>
        public int Myattachmentsavetime
        {
            get { return m_myattachmentsavetime; }
            set { m_myattachmentsavetime = value; }
        }

        /// <summary>
        /// 是否启用Tag功能
        /// </summary>
        public int Enabletag
        {
            get { return m_enabletag; }
            set { m_enabletag = value; }
        }

        /// <summary>
        /// 是否启用商场功能(0:不开启 1:开启普通模式 2:开启高级模式)
        /// </summary>
        public int Enablemall
        {
            get { return m_enablemall; }
            set { m_enablemall = value; }
        }

        /// <summary>
        /// 统计缓存时间(分钟)
        /// </summary>
        public int Statscachelife
        {
            get { return m_statscachelife; }
            set { m_statscachelife = value; }
        }

        /// <summary>
        /// 统计是否开启
        /// </summary>
        public int Statstatus
        {
            get { return m_statstatus; }
            set { m_statstatus = value; }
        }

        /// <summary>
        /// 页面访问量更新频率
        /// </summary>
        public int Pvfrequence
        {
            get { return m_pvfrequence; }
            set { m_pvfrequence = value; }
        }

        /// <summary>
        /// 用户在线时间更新时长(分钟)
        /// </summary>
        public int Oltimespan
        {
            get { return m_oltimespan; }
            set { m_oltimespan = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Recommenddebates
        {
            get { return m_recommenddebates; }
            set { m_recommenddebates = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Gpp
        {
            get { return m_gpp; }
            set { m_gpp = value; }
        }

        /// <summary>
        /// 首页热门标签数量
        /// </summary>
        public int Hottagcount
        {
            get { return m_hottagcount; }
            set { m_hottagcount = value; }
        }
        //public int Allowdiggs
        //{
        //    get { return m_allowdiggs; }
        //    set { m_allowdiggs = value; }
        //}

        /// <summary>
        /// 新用户广告强力屏蔽
        /// </summary>
        public int Disablepostad
        {
            get { return m_disablepostad; }
            set { m_disablepostad = value; }
        }

        /// <summary>
        /// 用户注册N分钟内进行新用户广告强力屏蔽功能检查
        /// </summary>
        public int Disablepostadregminute
        {
            get { return m_disablepostadregminute; }
            set { m_disablepostadregminute = value; }
        }

        /// <summary>
        /// 用户发帖N帖内进行新用户广告强力屏蔽功能检查
        /// </summary>
        public int Disablepostadpostcount
        {
            get { return m_disablepostadpostcount; }
            set { m_disablepostadpostcount = value; }
        }

        /// <summary>
        /// 新用户广告强力屏蔽功能正则表达式
        /// </summary>
        public string Disablepostadregular
        {
            get { return m_disablepostadregular; }
            set { m_disablepostadregular = value; }
        }

        /// <summary>
        /// 在线列表是否隐藏游客
        /// </summary>
        public int Whosonlinecontract
        {
            get { return m_whosonlinecontract; }
            set { m_whosonlinecontract = value; }
        }

        /// <summary>
        /// 帖子自定义楼号
        /// </summary>
        public string Postnocustom
        {
            get { return m_postnocustom; }
            set { m_postnocustom = value; }
        }

        /// <summary>
        /// 是否启用IIS的URL重写
        /// </summary>
        public int Iisurlrewrite
        {
            get { return m_iisurlrewrite; }
            set { m_iisurlrewrite = value; }
        }

        /// <summary>
        /// 通知保留天数
        /// </summary>
        public int Notificationreserveddays
        {
            get { return m_notificationreserveddays; }
            set { m_notificationreserveddays = value; }
        }
        /// <summary>
        /// 首页显示最大子版数
        /// </summary>
        public int Maxindexsubforumcount
        {
            get { return m_maxindexsubforumcount; }
            set { m_maxindexsubforumcount = value; }
        }
        /// <summary>
        /// 删除过期用户频率
        /// </summary>
        public int Deletingexpireduserfrequency
        {
            get
            {
                if (m_deletingexpireduserfrequency < 1)
                    m_deletingexpireduserfrequency = 5;
                return m_deletingexpireduserfrequency;
            }
            set { m_deletingexpireduserfrequency = value; }
        }
        /// <summary>
        /// 设置是否默认回帖通知用户
        /// </summary>
        public int Replynotificationstatus
        {
            get { return m_replynotificationstatus; }
            set { m_replynotificationstatus = value; }
        }

        /// <summary>
        /// 设置是否默认回帖短信息通知用户
        /// </summary>
        public int Replyemailstatus
        {
            get { return m_replyemailstatus; }
            set { m_replyemailstatus = value; }
        }

        public string Disallowfloatwin
        {
            get { return m_disallowfloatwin; }
            set { m_disallowfloatwin = value; }
        }

        public int Allwoforumindexpost
        {
            get { return m_allwoforumindexpost; }
            set { m_allwoforumindexpost = value; }
        }

        /// <summary>
        /// 用户在线表性能优化开关,默认为不开启
        /// </summary>
        public int Onlineoptimization
        {
            get { return m_onlineoptimization; }
            set { m_onlineoptimization = value; }
        }

        /// <summary>
        /// 在线用户数统计缓存时间，0为实时统计
        /// </summary>
        public int OnlineUserCountCacheMinute
        {
            get { return m_onlineusercountcacheminute; }
            set { m_onlineusercountcacheminute = value; }
        }

        /// <summary>
        /// 头像显示方式，0使用动态地址调用头像，1使用静态地址调用头像
        /// </summary>
        public int AvatarMethod
        {
            get { return m_avatarmethod; }
            set { m_avatarmethod = value; }
        }

        /// <summary>
        /// 不需要提示信息的页面
        /// </summary>
        public string Msgforwardlist
        {
            get { return m_msgforwardlist; }
            set { m_msgforwardlist = value; }
        }

        /// <summary>
        /// 是否开启跳转
        /// </summary>
        public int Quickforward
        {
            get { return m_quickforward; }
            set { m_quickforward = value; }
        }

        /// <summary>
        /// 论坛通过读取用户最后发帖时间的存储介质  0：database  1：cookie
        /// </summary>
        public int PostTimeStorageMedia
        {
            get { return m_posttimestoragemedia; }
            set { m_posttimestoragemedia = value; }
        }
        #endregion

        /// <summary>
        /// 是否开启分享功能
        /// </summary>
        public int Disableshare
        {
            get { return m_disableshare; }
            set { m_disableshare = value; }
        }

        /// <summary>
        /// 被分享的网站
        /// </summary>
        public string Sharelist
        {
            get { return m_sharelist; }
            set { m_sharelist = value; }
        }

        /// <summary>
        /// 支付宝卖家帐号
        /// </summary>
        public string Alipayaccout
        {
            get { return m_alipayaccout; }
            set { m_alipayaccout = value; }
        }

        /// <summary>
        /// 交易安全校验码 (key)
        /// </summary>
        public string Alipaypartnercheckkey
        {
            get { return m_alipaypartnercheckkey; }
            set { m_alipaypartnercheckkey = value; }
        }

        /// <summary>
        /// 合作者身份 (partnerID)
        /// </summary>
        public string Alipaypartnerid
        {
            get { return m_alipaypartnerid; }
            set { m_alipaypartnerid = value; }
        }

        /// <summary>
        /// 财付通卖家帐号
        /// </summary>
        public string Tenpayaccout
        {
            get { return m_tenpayaccout; }
            set { m_tenpayaccout = value; }
        }

        /// <summary>
        /// 财付通卖家密码
        /// </summary>
        public string Tenpaysecretkey
        {
            get { return m_tenpaysecretkey; }
            set { m_tenpaysecretkey = value; }
        }

        /// <summary>
        /// 是否使用支付宝签约接口
        /// </summary>
        public int Usealipaycustompartnerid
        {
            get { return m_usealipaycustompartnerid; }
            set { m_usealipaycustompartnerid = value; }
        }

        /// <summary>
        /// 是否使用即时到帐接口
        /// </summary>
        public int Usealipayinstantpay
        {
            get { return m_usealipayinstantpay; }
            set { m_usealipayinstantpay = value; }
        }

        /// <summary>
        /// 现金/积分兑换比率
        /// </summary>
        public int Cashtocreditrate
        {
            get { return m_cashtocreditrate; }
            set { m_cashtocreditrate = value; }
        }

        /// <summary>
        /// 单次购买最小积分数额
        /// </summary>
        public int Mincreditstobuy
        {
            get { return m_mincreditstobuy; }
            set { m_mincreditstobuy = value; }
        }

        /// <summary>
        /// 单次购买最大积分数额
        /// </summary>
        public int Maxcreditstobuy
        {
            get { return m_maxcreditstobuy; }
            set { m_maxcreditstobuy = value; }
        }

        /// <summary>
        /// 每日最多购买积分的次数
        /// </summary>
        public int Userbuycreditscountperday
        {
            get { return m_userbuycreditscountperday; }
            set { m_userbuycreditscountperday = value; }
        }

        /// <summary>
        /// 首页是否显示版块前有无新帖图标
        /// </summary>
        public int Shownewposticon
        {
            get { return m_shownewposticon; }
            set { m_shownewposticon = value; }
        }

        /// <summary>
        /// Jquery库地址
        /// </summary>
        public string Jqueryurl
        {
            get { return m_jqueryurl != "" ? m_jqueryurl : BaseConfigs.GetForumPath + "javascript/jquery.js"; }
            set { m_jqueryurl = value; }
        }

        #region Anti-spam
        /// <summary>
        /// anti-spam_register_username
        /// </summary>
        private string m_antispamregisterusername = "username";
        public string Antispamregisterusername
        {
            get { return m_antispamregisterusername; }
            set { m_antispamregisterusername = value; }
        }

        /// <summary>
        /// anti-spam_register_email
        /// </summary>
        private string m_antispamregisteremail = "email";
        public string Antispamregisteremail
        {
            get { return m_antispamregisteremail; }
            set { m_antispamregisteremail = value; }
        }

        /// <summary>
        /// anti-spam_post_title
        /// </summary>
        private string m_antispamposttitle = "title";
        public string Antispamposttitle
        {
            get { return m_antispamposttitle; }
            set { m_antispamposttitle = value; }
        }

        /// <summary>
        /// anti-spam_post_message
        /// </summary>
        private string m_antispampostmessage = "message";
        public string Antispampostmessage
        {
            get { return m_antispampostmessage; }
            set { m_antispampostmessage = value; }
        }

        /// <summary>
        /// 将此字符串中单个字符替换成空字符后进行过滤判断
        /// </summary>
        private string m_antispamreplacement = string.Empty;
        public string Antispamreplacement
        {
            get { return m_antispamreplacement; }
            set { m_antispamreplacement = value; }
        }
        #endregion

        /// <summary>
        /// 自定义验证码
        /// </summary>
        public string Verifycode
        {
            get { return m_verifycode; }
            set { m_verifycode = value; }
        }

        #region manyou
        ///// <summary>
        ///// Manyou站点id
        ///// </summary>
        //public string Mysiteid
        //{
        //    get { return m_mysiteid; }
        //    set { m_mysiteid = value; }
        //}

        ///// <summary>
        ///// Manyou站点key
        ///// </summary>
        //public string Mysitekey
        //{
        //    get { return m_mysitekey; }
        //    set { m_mysitekey = value; }
        //}

        ///// <summary>
        ///// 站点key
        ///// </summary>
        //public string Sitekey
        //{
        //    get
        //    {
        //        if (m_sitekey == "")
        //        {
        //            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //            Random rnd = new Random();
        //            for (int i = 1; i <= 16; i++)
        //            {
        //                m_sitekey += chars.Substring(rnd.Next(chars.Length), 1);
        //            }
        //        }
        //        return m_sitekey;
        //    }
        //    set { m_sitekey = value; }
        //}

        ///// <summary>
        ///// 是否开启Manyou应用
        ///// </summary>
        //public int Manyouenabled
        //{
        //    get { return m_manyouenabled; }
        //    set { m_manyouenabled = value; }
        //}
        #endregion

        /// <summary>
        /// 站点是否已执行了安装程序
        /// </summary>
        public int Installation
        {
            get { return m_installation; }
            set { m_installation = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Emaillogin
        {
            get { return m_emaillogin; }
            set { m_emaillogin = value; }
        }

        /// <summary>
        /// 评分列表类型
        /// </summary>
        public int Ratelisttype
        {
            get { return m_ratelisttype; }
            set { m_ratelisttype = value; }
        }

        ///// <summary>
        ///// 是否允许用户通过用户名搜索好友
        ///// </summary>
        //public int Allowsearchfriendbyusername
        //{
        //    get { return m_allowsearchfriendbyusername; }
        //    set { m_allowsearchfriendbyusername = value; }
        //}

        ///// <summary>
        ///// 用户最多可创建好友分组数
        ///// </summary>
        //public int Friendgroupmaxcount
        //{
        //    get { return m_friendgroupmaxcount; }
        //    set { m_friendgroupmaxcount = value; }
        //}

        /// <summary>
        /// 批量上传，1为开启flash批量上传，0为开启silverlight批量上传
        /// </summary>
        public int Swfupload
        {
            get { return m_swfupload; }
            set { m_swfupload = value; }
        }

        /// <summary>
        /// 设置图片附件加载方式 0：自动 1：手动点击加载
        /// </summary>
        public int Showimgattachmode
        {
            get { return m_showimgattachmode; }
            set { m_showimgattachmode = value; }
        }

        /// <summary>
        /// 当前站点web园进程数
        /// </summary>
        public int Webgarden
        {
            get { return m_webgarden; }
            set { m_webgarden = value; }
        }

        /// <summary>
        /// 是否允许用户切换页面的宽窄显示 1：允许 0：不允许
        /// </summary>
        public int Allowchangewidth
        {
            get { return m_allowchangewidth; }
            set { m_allowchangewidth = value; }
        }

        /// <summary>
        /// 页面默认显示的宽窄模式 0：宽屏 1：窄屏
        /// </summary>
        public int Showwidthmode
        {
            get { return m_showwidthmode; }
            set { m_showwidthmode = value; }
        }

        /// <summary>
        /// 删除帖子时间限制(分钟),0为不限制
        /// </summary>
        public int Deletetimelimit
        {
            get { return m_deletetimelimit; }
            set { m_deletetimelimit = value; }
        }

        ///<summary>
        ///是否开启人性化时间1开启,0关闭
        ///</summary>
        public int DateDiff
        {
            get { return m_datediff; }
            set { m_datediff = value; }
        }
    }
}
