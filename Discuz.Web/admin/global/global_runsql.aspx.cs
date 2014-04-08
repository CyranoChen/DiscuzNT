using System;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 运行指定的SQL语句
    /// </summary>
    public partial class runsql : AdminPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!base.IsFounderUid(userid))
                {
                    Response.Write(base.GetShowMessage());
                    Response.End();
                    return;
                }
            }
        }

        private void RunSqlString_Click(object sender, EventArgs e)
        {
            #region 运行指定的SQL语句

            if (this.CheckCookie())
            {
                if (!base.IsFounderUid(userid))
                {
                    Response.Write(base.GetShowMessage());
                    Response.End();
                    return;
                }

                if (sqlstring.Text.Trim() == "")
                {
                    base.RegisterStartupScript( "", "<script language=\"javascript\">alert('请您输入SQL语句!');</script>");
                    return;
                }

                string message = Databases.RunSql(sqlstring.Text.Replace("dnt_", BaseConfigs.GetTablePrefix));
                if (message != string.Empty)
                {
                    base.RegisterStartupScript("", "<script language=\"javascript\">showalert('" + message + "');</script>");
                    return;
                }

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "运行SQL语句", sqlstring.Text);
                base.RegisterStartupScript( "PAGE", "window.location.href='global_runsql.aspx';");
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
            this.RunSqlString.Click += new EventHandler(this.RunSqlString_Click);
        }

        #endregion
    }
}