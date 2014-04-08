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
    /// ʱ�������
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
            #region ����������Ϣ

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
            #region ����������Ϣ

            if (this.CheckCookie())
            {
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                Hashtable TimeHash = new Hashtable();
                TimeHash.Add("��ֹ����ʱ���", visitbanperiods.Text);
                TimeHash.Add("��ֹ����ʱ���", postbanperiods.Text);
                TimeHash.Add("�������ʱ���", postmodperiods.Text);
                TimeHash.Add("��ֹ���ظ���ʱ���", attachbanperiods.Text);
                TimeHash.Add("��ֹȫ������ʱ���", searchbanperiods.Text);
                string key = "";
                if (Utils.IsRuleTip(TimeHash, "timesect", out key) == false)
                {
                    base.RegisterStartupScript("erro", "<script>alert('" + key.ToString() + ",ʱ���ʽ����');</script>");
                    return;
                }

                configInfo.Visitbanperiods = visitbanperiods.Text;
                configInfo.Postbanperiods = postbanperiods.Text;
                configInfo.Postmodperiods = postmodperiods.Text;
                configInfo.Searchbanperiods = searchbanperiods.Text;
                configInfo.Attachbanperiods = attachbanperiods.Text;
                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "ʱ�������", "");
                base.RegisterStartupScript( "PAGE", "window.location.href='global_timespan.aspx';");
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