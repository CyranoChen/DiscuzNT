using System;
using System.Web.UI;
using System.Collections;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 时间段设置
    /// </summary>
    public partial class timespan : AdminPage
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
            visitbanperiods.Text = configInfo.Visitbanperiods.ToString();
            postbanperiods.Text = configInfo.Postbanperiods.ToString();
            postmodperiods.Text = configInfo.Postmodperiods.ToString();
            searchbanperiods.Text = configInfo.Searchbanperiods.ToString();
            attachbanperiods.Text = configInfo.Attachbanperiods.ToString();

            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存设置信息

            if (this.CheckCookie())
            {
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                Hashtable TimeHash = new Hashtable();
                TimeHash.Add("禁止访问时间段", visitbanperiods.Text);
                TimeHash.Add("禁止发帖时间段", postbanperiods.Text);
                TimeHash.Add("发帖审核时间段", postmodperiods.Text);
                TimeHash.Add("禁止下载附件时间段", attachbanperiods.Text);
                TimeHash.Add("禁止全文搜索时间段", searchbanperiods.Text);
                string key = "";
                if (Utils.IsRuleTip(TimeHash, "timesect", out key) == false)
                {
                    base.RegisterStartupScript("erro", "<script>alert('" + key.ToString() + ",时间格式错误');</script>");
                    return;
                }

                configInfo.Visitbanperiods = visitbanperiods.Text;
                configInfo.Postbanperiods = postbanperiods.Text;
                configInfo.Postmodperiods = postmodperiods.Text;
                configInfo.Searchbanperiods = searchbanperiods.Text;
                configInfo.Attachbanperiods = attachbanperiods.Text;
                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "时间段设置", "");
                base.RegisterStartupScript( "PAGE", "window.location.href='global_timespan.aspx';");
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