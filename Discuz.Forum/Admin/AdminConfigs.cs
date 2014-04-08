using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Forum
{
	/// <summary>
	/// AdminConfigFactory 的摘要说明。
	/// </summary>
	public class AdminConfigs : GeneralConfigs
	{
        /// <summary>
        /// 获取原始的缺省论坛设置
        /// </summary>
        /// <returns></returns>
        public static GeneralConfigInfo GetDefaultConifg()
        {
            GeneralConfigInfo configInfo = new GeneralConfigInfo();

            configInfo.Forumtitle = "论坛名称"; //论坛名称
            configInfo.Forumurl = "/"; //论坛url地址
            configInfo.Webtitle = "网站名称"; //网站名称
            configInfo.Weburl = "/"; //论坛网站url地址
            configInfo.Licensed = 1; //是否显示商业授权链接
            configInfo.Icp = ""; //网站备案信息
            configInfo.Closed = 0; //论坛关闭
            configInfo.Closedreason = "抱歉!论坛暂时关闭,稍后才能访问."; //论坛关闭提示信息
            configInfo.Passwordkey = ForumUtils.CreateAuthStr(16); //用户密码Key
            configInfo.Regstatus = 1; //是否允许新用户注册
            configInfo.Regadvance = 1; //注册时候是否显示高级选项
            configInfo.Censoruser = "Administrator\r\nAdmin\r\n管理员\r\n版主"; //用户信息保留关键字
            configInfo.Doublee = 0; //允许同一 Email 注册不同用户
            configInfo.Regverify = 0; //新用户注册验证 0=不验证 1=email验证 2=人工验证
            configInfo.Accessemail = ""; //Email允许地址
            configInfo.Censoremail = ""; //Email禁止地址
            configInfo.Hideprivate = 1; //隐藏无权访问的论坛
            configInfo.Regctrl = 0; //IP 注册间隔限制(小时)
            configInfo.Ipregctrl = ""; //特殊 IP 注册限制
            configInfo.Ipaccess = ""; //IP访问列表
            configInfo.Adminipaccess = ""; //管理员后台IP访问列表
            configInfo.Newbiespan = 0; //新手见习期限(单位:小时)
            configInfo.Welcomemsg = 1; //发送欢迎短消息
            configInfo.Welcomemsgtxt = "欢迎您注册加入本论坛!"; //欢迎短消息内容
            configInfo.Rules = 1; //是否显示注册许可协议
            configInfo.Rulestxt = ""; //许可协议内容

            configInfo.Templateid = 1; //默认论坛风格
            configInfo.Hottopic = 15; //热门话题最低帖数
            configInfo.Starthreshold = 5; //星星升级阀值
            configInfo.Visitedforums = 10; //显示最近访问论坛数量
            configInfo.Maxsigrows = 20; //最大签名高度(行)
            configInfo.Moddisplay = 0; //版主显示方式 0=平面显示 1=下拉菜单
            configInfo.Subforumsindex = 0; //首页是否显示论坛的下级子论坛
            configInfo.Stylejump = 0; //显示风格下拉菜单
            configInfo.Fastpost = 1; //快速发帖
            configInfo.Showsignatures = 1; //是否显示签名
            configInfo.Showavatars = 1; //是否显示头像
            configInfo.Showimages = 1; //是否在帖子中显示图片

            configInfo.Archiverstatus = 1; //启用 Archiver
            configInfo.Seotitle = ""; //标题附加字
            configInfo.Seokeywords = ""; //Meta Keywords
            configInfo.Seodescription = ""; //Meta Description
            configInfo.Seohead = ""; //其他头部信息

            configInfo.Rssstatus = 1; //rssstatus
            configInfo.Rssttl = 60; //RSS TTL(分钟)
            configInfo.Nocacheheaders = 0; //禁止浏览器缓冲
            configInfo.Fullmytopics = 0; //我的话题全文搜索 0=只搜索用户是主题发表者的主题 1=搜索用户是主题发表者或回复者的主题
            configInfo.Debug = 1; //显示程序运行信息
            configInfo.Rewriteurl = ""; //伪静态url的替换规则

            configInfo.Whosonlinestatus = 3; //显示在线用户 0=不显示 1=仅在首页显示 2=仅在分论坛显示 3=在首页和分论坛显示
            configInfo.Maxonlinelist = 300; //最多显示在线人数
            configInfo.Userstatusby = 2; //衡量并显示用户头衔
            configInfo.Forumjump = 1; //显示论坛跳转菜单
            configInfo.Modworkstatus = 1; //论坛管理工作统计
            configInfo.Maxmodworksmonths = 3; //管理记录保留时间(月)

            configInfo.Seccodestatus = "register.aspx"; //使用验证码的页面列表,用","分隔 例如:register.aspx,login.aspx
            configInfo.Maxonlines = 9000; //最大在线人数
            configInfo.Postinterval = 20; //发帖灌水预防(秒)
            configInfo.Searchctrl = 0; //搜索时间限制(秒)
            configInfo.Maxspm = 0; //60 秒最大搜索次数

            configInfo.Visitbanperiods = ""; //禁止访问时间段
            configInfo.Postbanperiods = ""; //禁止发帖时间段
            configInfo.Postmodperiods = ""; //发帖审核时间段
            configInfo.Attachbanperiods = ""; //禁止下载附件时间段
            configInfo.Searchbanperiods = ""; //禁止全文搜索时间段

            configInfo.Memliststatus = 1; //允许查看会员列表
            configInfo.Dupkarmarate = 0; //允许重复评分
            configInfo.Minpostsize = 10; //帖子最小字数(字)
            configInfo.Maxpostsize = 500000; //帖子最大字数(字)
            configInfo.Tpp = 25; //每页主题数
            configInfo.Ppp = 20; //每页帖子数
            configInfo.Maxfavorites = 100; //收藏夹容量
            //configInfo.Maxavatarsize = 20480; //头像最大尺寸(字节)
            //configInfo.Maxavatarwidth = 120; //头像最大宽度(像素)
            //configInfo.Maxavatarheight = 120; //头像最大高度(像素);
            configInfo.Maxpolloptions = 10; //投票最大选项数
            configInfo.Maxattachments = 10; //最大允许的上传附件数

            configInfo.Attachimgpost = 1; //帖子中显示图片附件
            configInfo.Attachrefcheck = 0; //下载附件来路检查
            configInfo.Attachsave = 3; //附件保存方式 0=全部存入同一目录 1=按论坛存入不同目录 2=按文件类型存入不同目录 3=按年月日存入不同目录
            configInfo.Watermarkstatus = 0; //图片附件添加水印 0=不使用 1=左上 2=中上 3=右上 4=左中 ... 9=右下

            configInfo.Karmaratelimit = 10; //评分时间限制(小时)
            configInfo.Losslessdel = 5; //删帖不减积分时间期限(天)
            configInfo.Edittimelimit = 0; //编辑帖子时间限制(分钟)
            configInfo.Editedby = 1; //编辑帖子附加编辑记录
            configInfo.Defaulteditormode = 1; //默认的编辑器模式 0=ubb代码编辑器 1=可视化编辑器
            configInfo.Allowswitcheditor = 1; //是否允许切换编辑器模式
            configInfo.Smileyinsert = 1; //显示可点击表情

            return configInfo;
        }
	}
}