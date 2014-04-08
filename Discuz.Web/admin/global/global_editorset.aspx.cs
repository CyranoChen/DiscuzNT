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

    public partial class editorset : AdminPage
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
            allowswitcheditor.SelectedValue = configInfo.Allowswitcheditor.ToString();
            bbcodemode.SelectedValue = configInfo.Bbcodemode.ToString();
            defaulteditormode.SelectedValue = configInfo.Defaulteditormode.ToString();
            swfupload.SelectedValue = configInfo.Swfupload.ToString();
            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存信息
            if (this.CheckCookie())
            {  
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                configInfo.Bbcodemode = TypeConverter.StrToInt(bbcodemode.SelectedValue);
                configInfo.Defaulteditormode = TypeConverter.StrToInt(defaulteditormode.SelectedValue);
                configInfo.Allowswitcheditor = TypeConverter.StrToInt(allowswitcheditor.SelectedValue);
                configInfo.Swfupload = Convert.ToInt16(swfupload.SelectedValue);
                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "编辑器设置", "");
                base.RegisterStartupScript("PAGE", "window.location.href='global_editorset.aspx';");
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