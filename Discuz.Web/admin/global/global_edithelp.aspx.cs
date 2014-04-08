using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    public partial class edithelp : AdminPage
    {
        public int id = DNTRequest.GetInt("id",0);
        public HelpInfo helpinfo = new HelpInfo();
                 
        protected void Page_Load(object sender, EventArgs e)
        {
            helpinfo = Helps.GetMessage(id);
            if (helpinfo.Pid == 0)
            {
                Response.Redirect("global_edithelpclass.aspx?id="+id);
                return;
            }
            if (!Page.IsPostBack)
            {
                if ((this.username != null) && (this.username != ""))
                {
                    if (id == 0)
                        return;
                    poster.Text = this.username;
                    type.AddTableData(Helps.GetHelpTypes(), "title", "id");
                    type.SelectedValue = helpinfo.Pid.ToString();
                    orderby.Text = helpinfo.Orderby.ToString();
                    title.Text = helpinfo.Title;
                    help.Text = helpinfo.Message;
                    updatehelp.ValidateForm = true;
                    title.AddAttributes("maxlength", "200");
                    title.AddAttributes("rows", "2");
                    type.DataBind();
                }
            }
        }

        protected void updatehelp_Click(object sender, EventArgs e)
        {
            Helps.UpdateHelp(id, title.Text, DNTRequest.GetString("helpmessage_hidden").Trim(), int.Parse(type.SelectedValue),int.Parse(orderby.Text));
            Response.Redirect("global_helplist.aspx");
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.updatehelp.Click += new EventHandler(this.updatehelp_Click);
        }

        #endregion
    }
}
