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
        #region 控件声明
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
            #region 保存信息

            if (this.CheckCookie())
            {
                SpaceActiveConfigInfo __configinfo = SpaceActiveConfigs.GetConfig();
                __configinfo.SpaceFooterInfo = spacefooterinfo.Text;
                __configinfo.Spacegreeting = greeting.Text;
                __configinfo.Enablespacerewrite = Utils.StrToInt(enablerewrite.SelectedValue, 0);
                SpaceActiveConfigs.SaveConfig(__configinfo);

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "空间其它信息设置", "");

                if (!Utils.IsNumeric(topictoblog.Text) || Convert.ToInt32(topictoblog.Text) > 10 || Convert.ToInt32(topictoblog.Text) < 0)
                {
                    base.RegisterStartupScript("PAGE", "alert('自动加入日志数量取值必须在0~10之间');window.location='space_spacefooterinfo.aspx';");
                    return;
                }

                config.Topictoblog = Convert.ToInt32(topictoblog.Text);
                GeneralConfigs.Serialiaze(config, Server.MapPath("../../config/general.config"));

                base.RegisterStartupScript("PAGE", "window.location='space_spacefooterinfo.aspx';");
            }

            #endregion
        }

        #region Web 窗体设计器生成的代码

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