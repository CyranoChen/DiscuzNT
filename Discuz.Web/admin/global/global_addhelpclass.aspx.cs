using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    public partial class addhelpclass : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Utils.StrIsNullOrEmpty(username))
                    poster.Text = this.username;
            }
        }

        protected void add_Click(object sender, EventArgs e)
        {
            #region ���Ӱ������
            if (this.CheckCookie())
            {
                Helps.AddHelp(title.Text,"", 0);
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "��Ӱ�������", "��Ӱ�������,����Ϊ:" + title.Text);
                base.RegisterStartupScript("", "<script>window.location.href='global_helplist.aspx';</script>");
            }
            #endregion
        }

        #region Web ������������ɵĴ���
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        #endregion
    }
}
