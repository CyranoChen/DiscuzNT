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
    /// ��������
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
            #region ����������Ϣ
            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            allowswitcheditor.SelectedValue = configInfo.Allowswitcheditor.ToString();
            bbcodemode.SelectedValue = configInfo.Bbcodemode.ToString();
            defaulteditormode.SelectedValue = configInfo.Defaulteditormode.ToString();
            swfupload.SelectedValue = configInfo.Swfupload.ToString();
            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region ������Ϣ
            if (this.CheckCookie())
            {  
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                configInfo.Bbcodemode = TypeConverter.StrToInt(bbcodemode.SelectedValue);
                configInfo.Defaulteditormode = TypeConverter.StrToInt(defaulteditormode.SelectedValue);
                configInfo.Allowswitcheditor = TypeConverter.StrToInt(allowswitcheditor.SelectedValue);
                configInfo.Swfupload = Convert.ToInt16(swfupload.SelectedValue);
                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "�༭������", "");
                base.RegisterStartupScript("PAGE", "window.location.href='global_editorset.aspx';");
            }
            #endregion
        }

        #region Web ������������ɵĴ���

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