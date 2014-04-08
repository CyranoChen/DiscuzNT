using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ���ӹ���
    /// </summary>
    public partial class addannounce : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Utils.StrIsNullOrEmpty(username))
                {
                    poster.Text = this.username;
                    starttime.Text = DateTime.Now.ToString();
                    endtime.Text = DateTime.Now.AddDays(7).ToString();
                    AddAnnounceInfo.ValidateForm = true;
                    title.AddAttributes("maxlength", "200");
                    title.AddAttributes("rows", "2");
                }
            }
        }

        private void AddAnnounceInfo_Click(object sender, EventArgs e)
        {
            #region ��ӹ���
            if (this.CheckCookie())
            {
                Announcements.CreateAnnouncement(username, userid, title.Text, Utils.StrToInt(displayorder.Text, 0), starttime.Text, endtime.Text, DNTRequest.GetString("announcemessage_hidden"));

                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AnnouncementList");
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/SimplifiedAnnouncementList");

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "��ӹ���", "��ӹ���,����Ϊ:" + title.Text);
                base.RegisterStartupScript( "PAGE", "window.location.href='global_announcegrid.aspx';");
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
            this.AddAnnounceInfo.Click += new EventHandler(this.AddAnnounceInfo_Click);
            this.Load += new EventHandler(this.Page_Load);
        }
        #endregion


    }
}