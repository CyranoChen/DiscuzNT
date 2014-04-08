using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 搜索引擎优化
    /// </summary>
    
    public partial class searchengine : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadConfigInfo();
            }
        }

        public void LoadConfigInfo()
        {
            #region 加载配置信息

            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            seotitle.Text = configInfo.Seotitle.ToString();
            seokeywords.Text = configInfo.Seokeywords.ToString();
            seodescription.Text = configInfo.Seodescription.ToString();
            seohead.Text = configInfo.Seohead.ToString();
            archiverstatus.SelectedValue = configInfo.Archiverstatus.ToString();
            sitemapstatus.SelectedValue = configInfo.Sitemapstatus.ToString();
            sitemapttl.Text = configInfo.Sitemapttl.ToString();
            aspxrewrite.SelectedValue = configInfo.Aspxrewrite.ToString();
            extname.Text = configInfo.Extname.Trim();
            iisurlrewrite.SelectedValue = configInfo.Iisurlrewrite.ToString();
            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存设置信息

            if (this.CheckCookie())
            {
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();

                configInfo.Seotitle = seotitle.Text;
                configInfo.Seokeywords = seokeywords.Text;
                configInfo.Seodescription = seodescription.Text;
                configInfo.Seohead = seohead.Text;
                configInfo.Archiverstatus = Convert.ToInt16(archiverstatus.SelectedValue);
                configInfo.Sitemapstatus = Convert.ToInt16(sitemapstatus.SelectedValue);
                configInfo.Sitemapttl = Convert.ToInt32(sitemapttl.Text);
                configInfo.Aspxrewrite = Convert.ToInt16(aspxrewrite.SelectedValue);
                if (extname.Text.Trim() == "")
                {
                    base.RegisterStartupScript("", "<script>alert('您未输入相应的伪静态url扩展名!');</script>");
                    return;
                }
                configInfo.Extname = extname.Text.Trim();

                if (configInfo.Aspxrewrite == 1)
                    AdminForums.SetForumsPathList(true, configInfo.Extname);
                else
                    AdminForums.SetForumsPathList(false, configInfo.Extname);

                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/ForumList");
                configInfo.Iisurlrewrite = Convert.ToInt16(iisurlrewrite.SelectedValue);
                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "搜索引擎优化设置", "");
                base.RegisterStartupScript( "PAGE",  "window.location.href='global_searchengine.aspx';");
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
        }

        #endregion
    }
}