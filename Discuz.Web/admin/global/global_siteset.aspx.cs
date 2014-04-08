using System;
using System.Web.UI;
using System.Data;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;
using Discuz.Plugin.Space;
using Discuz.Plugin.Album;
using Discuz.Plugin.Mall;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 基本设置
    /// </summary>

    public partial class siteset : AdminPage
    {
        protected Discuz.Control.RadioButtonList iisurlrewrite;
        protected bool haveAlbum;
        protected bool haveSpace;
        protected bool haveMall;
        protected void Page_Load(object sender, EventArgs e)
        {
            haveAlbum = AlbumPluginProvider.GetInstance() != null;
            haveSpace = SpacePluginProvider.GetInstance() != null;
            haveMall = MallPluginProvider.GetInstance() != null;
            if (!Page.IsPostBack)
            {
                LoadConfigInfo();
                closed.Items[0].Attributes.Add("onclick", "setStatus(true)");
                closed.Items[1].Attributes.Add("onclick", "setStatus(false)");
            }
        }

        public void LoadConfigInfo()
        {
            #region 加载配置信息
            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            forumtitle.Text = configInfo.Forumtitle.ToString();
            webtitle.Text = configInfo.Webtitle.ToString();
            weburl.Text = configInfo.Weburl.ToString();
            licensed.SelectedValue = configInfo.Licensed.ToString();
            icp.Text = configInfo.Icp.ToString();
            debug.SelectedValue = configInfo.Debug.ToString();
            Statcode.Text = configInfo.Statcode;
            Linktext.Text = configInfo.Linktext;
            spacename.Text = configInfo.Spacename.ToString();
            albumname.Text = configInfo.Albumname.ToString();

            closed.SelectedValue = configInfo.Closed.ToString();
            closedreason.Text = configInfo.Closedreason.ToString();
            EnableSpace.SelectedValue = configInfo.Enablespace.ToString();
            EnableAlbum.SelectedValue = configInfo.Enablealbum.ToString();
            EnableMall.SelectedValue = configInfo.Enablemall.ToString();

            if (!haveSpace)
            {
                EnableSpace.Visible = false;
                EnableSpaceLabel.Visible = false;
            }
            if (!haveAlbum)
            {
                EnableAlbum.Visible = false;
                EnableAlbumLabel.Visible = false;
            }

            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存信息
            if (this.CheckCookie())
            {
                
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                configInfo.Forumtitle = forumtitle.Text;
                configInfo.Webtitle = webtitle.Text;
                configInfo.Weburl = weburl.Text;
                configInfo.Licensed = TypeConverter.StrToInt(licensed.SelectedValue);
                configInfo.Icp = icp.Text;
                configInfo.Debug = TypeConverter.StrToInt(debug.SelectedValue);
                configInfo.Statcode = Statcode.Text;
                configInfo.Linktext = Linktext.Text;
                configInfo.Spacename = spacename.Text;
                configInfo.Albumname = albumname.Text;
                configInfo.Closed = TypeConverter.StrToInt(closed.SelectedValue);
                configInfo.Closedreason = closedreason.Text;
                configInfo.Enablespace = Convert.ToInt32(EnableSpace.SelectedValue);
                configInfo.Enablealbum = Convert.ToInt32(EnableAlbum.SelectedValue);
                configInfo.Enablemall = Convert.ToInt32(EnableMall.SelectedValue);

                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));
               if (configInfo.Aspxrewrite == 1)
                    AdminForums.SetForumsPathList(true, configInfo.Extname);
                else
                    AdminForums.SetForumsPathList(false, configInfo.Extname);
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/ForumList");
                Discuz.Forum.TopicStats.SetQueueCount();
                Caches.ReSetConfig();
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "站点设置", "");
                base.RegisterStartupScript("PAGE", "window.location.href='global_siteset.aspx';");
            }
            #endregion
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
            forumtitle.IsReplaceInvertedComma = false;
            webtitle.IsReplaceInvertedComma = false;
            weburl.IsReplaceInvertedComma = false;
            icp.IsReplaceInvertedComma = false;
        }
        #endregion

    }
}