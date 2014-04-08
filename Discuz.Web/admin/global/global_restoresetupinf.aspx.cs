using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ����Config�ļ�
    /// </summary>
    public partial class restoresetupinf : AdminPage
    {
       protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GeneralConfigs.GetConfig();
            }
        }

        private void RestoreInf_Click(object sender, EventArgs e)
        {
            if (this.CheckCookie())
            {
                GeneralConfigInfo __configinfo = AdminConfigs.GetConfig();
                GeneralConfigs.Serialiaze(__configinfo, Server.MapPath("../../config/general.config"));

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "�ָ���̳��ʼ������", "");
                base.RegisterStartupScript( "PAGE",  "window.location.href='global_restoresetupinf.aspx';");
            }
        }

        #region Web ������������ɵĴ���

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.RestoreInf.Click += new EventHandler(this.RestoreInf_Click);
        }

        #endregion

    }
}