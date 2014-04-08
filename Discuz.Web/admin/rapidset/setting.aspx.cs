using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 快速设置向导. 
    /// </summary>
    public partial class setting : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                forumtitle.Text = configInfo.Forumtitle.ToString();
                forumurl.Text = configInfo.Forumurl.ToString();
                webtitle.Text = configInfo.Webtitle.ToString();
                weburl.Text = configInfo.Weburl.ToString().ToLower();

                SetOption(configInfo);
            }
        }

        public void SetOption(GeneralConfigInfo configInfo)
        {
            if (configInfo.Maxonlines == 500) size.SelectedValue = "1";
            if (configInfo.Maxonlines == 5000) size.SelectedValue = "2";
            if (configInfo.Maxonlines == 50000) size.SelectedValue = "3";

            if (configInfo.Regctrl == 0) safe.SelectedValue = "1";
            if (configInfo.Regctrl == 12) safe.SelectedValue = "2";
            if (configInfo.Regctrl == 48) safe.SelectedValue = "3";

            if (configInfo.Visitedforums == 0) func.SelectedValue = "1";
            if (configInfo.Visitedforums == 10) func.SelectedValue = "2";
            if (configInfo.Visitedforums == 20) func.SelectedValue = "3";
        }

        private void submitsetting_Click(object sender, EventArgs e)
        {
            if (this.CheckCookie())
            {
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();

                #region

                switch (size.SelectedValue)
                {
                    case "1":
                        {
                            configInfo.Attachsave = 0;
                            configInfo.Fullmytopics = 0;
                            configInfo.Maxonlines = 500;
                            configInfo.Starthreshold = 2;
                            configInfo.Searchctrl = 10;
                            configInfo.Hottopic = 10;
                            configInfo.Losslessdel = 365;
                            configInfo.Maxmodworksmonths = 5;
                            configInfo.Moddisplay = 0;
                            configInfo.Tpp = 30;
                            configInfo.Ppp = 20;
                            configInfo.Maxpolloptions = 10;
                            configInfo.Maxpostsize = 10000;
                            configInfo.Maxfavorites = 500;
                            configInfo.Nocacheheaders = 1;
                            configInfo.Guestcachepagetimeout = 0;
                            configInfo.Topiccachemark = 0;
                            configInfo.Postinterval = 5;
                            configInfo.Maxspm = 5;
                            configInfo.Fulltextsearch = 0;
                            configInfo.TopicQueueStats = 0;
                            configInfo.TopicQueueStatsCount = 20;
                            break;
                        }
                    case "2":
                        {
                            configInfo.Attachsave = 1;
                            configInfo.Fullmytopics = 1;
                            configInfo.Maxonlines = 5000;
                            configInfo.Starthreshold = 2;
                            configInfo.Searchctrl = 30;
                            configInfo.Hottopic = 20;
                            configInfo.Losslessdel = 200;
                            configInfo.Maxmodworksmonths = 3;
                            configInfo.Moddisplay = 0;
                            configInfo.Tpp = 20;
                            configInfo.Ppp = 15;
                            configInfo.Maxpolloptions = 1000;
                            configInfo.Maxpostsize = 10000;
                            configInfo.Maxfavorites = 200;
                            configInfo.Nocacheheaders = 0;
                            configInfo.Guestcachepagetimeout = 10;
                            configInfo.Topiccachemark = 20;
                            configInfo.Postinterval = 10;
                            configInfo.Maxspm = 4;
                            configInfo.Fulltextsearch = 0;
                            configInfo.TopicQueueStats = 1;
                            configInfo.TopicQueueStatsCount = 30;
                            break;
                        }
                    case "3":
                        {
                            configInfo.Attachsave = 2;
                            configInfo.Fullmytopics = 1;
                            configInfo.Maxonlines = 50000;
                            configInfo.Starthreshold = 2;
                            configInfo.Searchctrl = 60;
                            configInfo.Hottopic = 100;
                            configInfo.Maxmodworksmonths = 1;
                            configInfo.Moddisplay = 1;
                            configInfo.Tpp = 15;
                            configInfo.Ppp = 10;
                            configInfo.Maxpolloptions = 20000;
                            configInfo.Maxfavorites = 100;
                            configInfo.Nocacheheaders = 0;
                            configInfo.Guestcachepagetimeout = 20;
                            configInfo.Topiccachemark = 50;
                            configInfo.Postinterval = 15;
                            configInfo.Maxspm = 3;
                            configInfo.Fulltextsearch = 0;
                            configInfo.TopicQueueStats = 1;
                            configInfo.TopicQueueStatsCount = 100;
                            break;
                        }
                }

                #endregion

                #region

                switch (safe.SelectedValue)
                {
                    case "1":
                        {
                            configInfo.Doublee = 1; //允许同一 Email 注册不同用户
                            configInfo.Dupkarmarate = 1; //debug 安全	允许重复评分 (开关)
                            configInfo.Hideprivate = 0; //debug 安全	隐藏无权访问的论坛 (开关)
                            configInfo.Memliststatus = 1; //debug 安全	允许查看会员列表 (开关)
                            configInfo.Seccodestatus = ""; //debug 安全	启用验证码
                            configInfo.Rules = 0; //debug 安全	注册许可协议, 结合bbrulestxt使用 (开关)
                            configInfo.Edittimelimit = 0; //debug 安全	编辑帖子时间限制 (分钟)
                            configInfo.Karmaratelimit = 0; //debug 安全	评分时间限制 (小时)
                            configInfo.Regctrl = 0; //debug 安全	同一IP 注册间隔限制(小时)
                            configInfo.Regstatus = 1; //debug 安全	允许新用户注册 (开关) ?
                            configInfo.Regverify = 0; //debug 功能	新用户注册验证 (0:直接注册成功 1:Email 验证 2:人工审核)
                            configInfo.Secques = 5;
                            configInfo.Defaulteditormode = 0;
                            configInfo.Allowswitcheditor = 0;
                            configInfo.Watermarktype = 0;
                            configInfo.Attachimgquality = 80;
                            break;
                        }
                    case "2":
                        {
                            configInfo.Attachrefcheck = 1;
                            configInfo.Doublee = 0; //允许同一 Email 注册不同用户
                            configInfo.Dupkarmarate = 0; //debug 安全	允许重复评分 (开关)
                            configInfo.Hideprivate = 1; //debug 安全	隐藏无权访问的论坛 (开关)
                            configInfo.Memliststatus = 1; //debug 安全	允许查看会员列表 (开关)
                            configInfo.Seccodestatus = "login.aspx"; //debug 安全	启用验证码
                            configInfo.Rules = 1; //debug 安全	注册许可协议, 结合bbrulestxt使用 (开关)
                            configInfo.Edittimelimit = 20; //debug 安全	编辑帖子时间限制 (分钟)
                            configInfo.Karmaratelimit = 1; //debug 安全	评分时间限制 (小时)
                            configInfo.Newbiespan = 1; //debug 安全	新手见习期限 (小时)
                            configInfo.Regctrl = 12; //debug 安全	同一IP 注册间隔限制(小时)
                            configInfo.Regstatus = 1; //debug 安全	允许新用户注册 (开关) ?
                            configInfo.Regverify = 1; //debug 功能	新用户注册验证 (0:直接注册成功 1:Email 验证 2:人工审核)
                            configInfo.Secques = 10;
                            configInfo.Defaulteditormode = 0;
                            configInfo.Allowswitcheditor = 1;
                            configInfo.Watermarktype = 1;
                            configInfo.Attachimgquality = 85;
                            break;
                        }
                    case "3":
                        {
                            configInfo.Attachrefcheck = 1;
                            configInfo.Doublee = 0; //允许同一 Email 注册不同用户
                            configInfo.Dupkarmarate = 0; //debug 安全	允许重复评分 (开关)
                            configInfo.Hideprivate = 1; //debug 安全	隐藏无权访问的论坛 (开关)
                            configInfo.Memliststatus = 0; //debug 安全	允许查看会员列表 (开关)
                            configInfo.Seccodestatus = "login.aspx"; //debug 安全	启用验证码
                            configInfo.Rules = 1; //debug 安全	注册许可协议, 结合bbrulestxt使用 (开关)
                            configInfo.Edittimelimit = 10; //debug 安全	编辑帖子时间限制 (分钟)
                            configInfo.Karmaratelimit = 4; //debug 安全	评分时间限制 (小时)
                            configInfo.Newbiespan = 4; //debug 安全	新手见习期限 (小时)
                            configInfo.Regctrl = 48; //debug 安全	同一IP 注册间隔限制(小时)
                            configInfo.Regstatus = 1; //debug 安全	允许新用户注册 (开关) ?
                            configInfo.Regverify = 1; //debug 功能	新用户注册验证 (0:直接注册成功 1:Email 验证 2:人工审核)
                            configInfo.Secques = 20;
                            configInfo.Defaulteditormode = 1;
                            configInfo.Allowswitcheditor = 1;
                            configInfo.Watermarktype = 1;
                            configInfo.Attachimgquality = 100;
                            break;
                        }
                }

                #endregion

                #region

                switch (func.SelectedValue)
                {
                    case "1":
                        {
                            configInfo.Archiverstatus = 0; //debug 功能	启用 Archiver (开关)
                            configInfo.Attachimgpost = 0; //debug 功能	帖子中显示图片附件 (开关)
                            configInfo.Fastpost = 0; //debug 功能	快速发帖 (开关)
                            configInfo.Editedby = 0; //debug 功能	显示编辑信息 (开关)
                            configInfo.Forumjump = 0; //debug 功能	显示论坛跳转菜单 (开关)
                            configInfo.Modworkstatus = 0; //debug 功能	论坛管理工作统计 (开关)
                            configInfo.Rssstatus = 0; //debug 功能	启用 RSS
                            configInfo.Smileyinsert = 0; //debug 功能	显示可点击 Smilies , 与smcols项结合使用可以控制是否显示编辑器
                            configInfo.Stylejump = 0; //debug 功能	显示风格下拉菜单
                            configInfo.Subforumsindex = 0; //debug 功能	首页显示论坛的下级子论坛
                            configInfo.Visitedforums = 0; //debug 功能	显示最近访问论坛数量
                            configInfo.Welcomemsg = 0; //debug 功能	发送欢迎短消息
                            configInfo.Watermarkstatus = 0; //debug 功能	图片附件添加水印
                            configInfo.Whosonlinestatus = 0; //debug 功能	在线显示状态
                            configInfo.Debug = 0; //debug 功能	debug 打开模式
                            configInfo.Regadvance = 0; //debug 功能	是否显示高级注册选项
                            configInfo.Showsignatures = 0; //debug 功能	是否显示签名, 头象
                            break;
                        }

                    case "2":
                        {
                            configInfo.Archiverstatus = 1; //debug 功能	启用 Archiver (开关)
                            configInfo.Attachimgpost = 1; //debug 功能	帖子中显示图片附件 (开关)
                            configInfo.Fastpost = 1; //debug 功能	快速发帖 (开关)
                            configInfo.Editedby = 1; //debug 功能	显示编辑信息 (开关)
                            configInfo.Forumjump = 1; //debug 功能	显示论坛跳转菜单 (开关)
                            configInfo.Modworkstatus = 0; //debug 功能	论坛管理工作统计 (开关)
                            configInfo.Rssstatus = 1; //debug 功能	启用 RSS
                            configInfo.Smileyinsert = 1; //debug 功能	显示可点击 Smilies , 与smcols项结合使用可以控制是否显示编辑器
                            configInfo.Stylejump = 0; //debug 功能	显示风格下拉菜单
                            configInfo.Subforumsindex = 0; //debug 功能	首页显示论坛的下级子论坛
                            configInfo.Visitedforums = 10; //debug 功能	显示最近访问论坛数量
                            configInfo.Welcomemsg = 0; //debug 功能	发送欢迎短消息
                            configInfo.Watermarkstatus = 0; //debug 功能	图片附件添加水印
                            configInfo.Whosonlinestatus = 1; //debug 功能	在线显示状态
                            configInfo.Debug = 1; //debug 功能	debug 打开模式
                            configInfo.Regadvance = 0; //debug 功能	是否显示高级注册选项
                            configInfo.Showsignatures = 1; //debug 功能	是否显示签名, 头象
                            break;
                        }
                    case "3":
                        {
                            configInfo.Archiverstatus = 1; //debug 功能	启用 Archiver (开关)
                            configInfo.Attachimgpost = 1; //debug 功能	帖子中显示图片附件 (开关)
                            configInfo.Fastpost = 1; //debug 功能	快速发帖 (开关)
                            configInfo.Editedby = 1; //debug 功能	显示编辑信息 (开关)
                            configInfo.Forumjump = 1; //debug 功能	显示论坛跳转菜单 (开关)
                            configInfo.Modworkstatus = 1; //debug 功能	论坛管理工作统计 (开关)
                            configInfo.Rssstatus = 1; //debug 功能	启用 RSS
                            configInfo.Smileyinsert = 1; //debug 功能	显示可点击 Smilies , 与smcols项结合使用可以控制是否显示编辑器
                            configInfo.Stylejump = 1; //debug 功能	显示风格下拉菜单
                            configInfo.Subforumsindex = 1; //debug 功能	首页显示论坛的下级子论坛
                            configInfo.Visitedforums = 20; //debug 功能	显示最近访问论坛数量
                            configInfo.Welcomemsg = 1; //debug 功能	发送欢迎短消息
                            configInfo.Watermarkstatus = 1; //debug 功能	图片附件添加水印
                            configInfo.Whosonlinestatus = 1; //debug 功能	在线显示状态
                            configInfo.Debug = 1; //debug 功能	debug 打开模式
                            configInfo.Regadvance = 1; //debug 功能	是否显示高级注册选项
                            configInfo.Showsignatures = 1; //debug 功能	是否显示签名, 头象
                            break;
                        }
                }

                #endregion

                configInfo.Forumtitle = forumtitle.Text.Trim();
                configInfo.Forumurl = forumurl.Text.Trim().ToLower();
                configInfo.Webtitle = webtitle.Text.Trim();
                configInfo.Weburl = weburl.Text.Trim().ToLower();

                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "快速设置", "");
                base.RegisterStartupScript( "PAGE",  "window.location.href='setting.aspx';");
            }
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.submitsetting.Click += new EventHandler(this.submitsetting_Click);

            forumtitle.IsReplaceInvertedComma = false;
            forumurl.IsReplaceInvertedComma = false;
            webtitle.IsReplaceInvertedComma = false;
            weburl.IsReplaceInvertedComma = false;
        }

        #endregion

    }
}