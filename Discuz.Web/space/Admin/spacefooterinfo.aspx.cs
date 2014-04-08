using System;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Control;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Web.Admin;


namespace Discuz.Space.Admin
{

#if NET1
    public class SpaceFooterInfoPage : AdminPage
#else
    public partial class SpaceFooterInfoPage : AdminPage
#endif
    {

#if NET1
        #region �ؼ�����
        protected Discuz.Web.Admin.TextareaResize spacefooterinfo;
        protected Discuz.Web.Admin.TextareaResize greeting;
		protected Discuz.Control.TextBox topictoblog;
        protected Discuz.Control.RadioButtonList enablerewrite;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button SaveInfo;
        #endregion
#endif
    
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SpaceActiveConfigInfo __configinfo = SpaceActiveConfigs.GetConfig();
                spacefooterinfo.Text = __configinfo.SpaceFooterInfo;
                greeting.Text = __configinfo.Spacegreeting;
                topictoblog.Text = config.Topictoblog.ToString();
                enablerewrite.SelectedValue = __configinfo.Enablespacerewrite.ToString();
            }
        }


        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region ������Ϣ

            if (this.CheckCookie())
            {
                SpaceActiveConfigInfo __configinfo = SpaceActiveConfigs.GetConfig();
                __configinfo.SpaceFooterInfo = spacefooterinfo.Text;
                __configinfo.Spacegreeting = greeting.Text;
                __configinfo.Enablespacerewrite = Utils.StrToInt(enablerewrite.SelectedValue, 0);
                SpaceActiveConfigs.SaveConfig(__configinfo);

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "�ռ�������Ϣ����", "");

                if (!Utils.IsNumeric(topictoblog.Text) || Convert.ToInt32(topictoblog.Text) > 10 || Convert.ToInt32(topictoblog.Text) < 0)
                {
                    base.RegisterStartupScript("PAGE", "alert('�Զ�������־����ȡֵ������0~10֮��');window.location='space_spacefooterinfo.aspx';");
                    return;
                }

                config.Topictoblog = Convert.ToInt32(topictoblog.Text);
                GeneralConfigs.Serialiaze(config, Server.MapPath("../../config/general.config"));

                base.RegisterStartupScript("PAGE", "window.location='space_spacefooterinfo.aspx';");
            }

            #endregion
        }

        #region Web ������������ɵĴ���

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion

    }
}