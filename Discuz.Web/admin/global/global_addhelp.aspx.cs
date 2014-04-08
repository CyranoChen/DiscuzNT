using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    public partial class addhelp : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Utils.StrIsNullOrEmpty(username))
                {
                    poster.Text = this.username;
                    type.AddTableData(Helps.GetHelpTypes(), "title", "id");
                    Addhelp.ValidateForm = true;
                    title.AddAttributes("maxlength", "200");
                    title.AddAttributes("rows", "2");
                    type.DataBind();
                }
            }
        }

        protected void Addhelp_Click(object sender, EventArgs e)
        {
            #region 增加帮助项
            if (this.CheckCookie())
            {
                if (int.Parse(type.SelectedItem.Value) == 0)
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');window.location.href='global_addhelp.aspx';</script>");
                }
                else
                {
                    Helps.AddHelp(title.Text, DNTRequest.GetString("helpmessage_hidden").Trim(), int.Parse(type.SelectedItem.Value));
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "添加帮助", "添加帮助,标题为:" + title.Text);
                    base.RegisterStartupScript("", "<script>window.location.href='global_helplist.aspx';</script>");
                }
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
            this.Addhelp.Click += new EventHandler(this.Addhelp_Click);
        }
        #endregion
    }
}
