using System;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 公告列表
    /// </summary>
    public partial class announcegrid : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                postdatetimeStart.SelectedDate = DateTime.Now.AddDays(-30);
                postdatetimeEnd.SelectedDate = DateTime.Now;
                BindData();
            }
        }

        public void BindData()
        {
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.BindData(Forum.Announcements.GetAnnouncementList());
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            #region 删除公告

            if (CheckCookie())
            {
                if (DNTRequest.GetString("id") != "")
                {
                    Forum.Announcements.DeleteAnnouncements(DNTRequest.GetString("id"));
                    AdminVistLogs.InsertLog(userid, username, usergroupid, grouptitle, ip, "删除公告", "删除公告,公告ID为: " + DNTRequest.GetString("id"));
                    Response.Redirect("global_announcegrid.aspx");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');window.location.href='global_announcegrid.aspx';</script>");
                }
            }

            #endregion
        }

        //TODO:传Sql语句，需要改进
        private void Search_Click(object sender, EventArgs e)
        {
            #region 按条件搜索公告

            if (CheckCookie())
            {
                StringBuilder builder = new StringBuilder();
                if (!poster.Text.Equals(""))
                {
                    builder.Append("[poster] LIKE '%");
                    builder.Append(poster.Text);
                    builder.Append("%'");
                }

                if (!title.Text.Equals(""))
                {
                    if (builder.Length > 0)
                        builder.Append(" AND ");
                    builder.Append("[title] LIKE '%");
                    builder.Append(title.Text);
                    builder.Append("%'");
                }

                if (!postdatetimeStart.SelectedDate.ToString().Equals(""))
                {
                    if (builder.Length > 0)
                        builder.Append(" AND ");
                    builder.Append("[starttime] >= '");
                    builder.Append(postdatetimeStart.SelectedDate.ToString());
                    builder.Append("'");
                }

                if (!postdatetimeEnd.SelectedDate.AddDays(1).ToString().Equals(""))
                {
                    if (builder.Length > 0)
                        builder.Append(" AND ");
                    builder.Append("[starttime] <= '");
                    builder.Append(postdatetimeEnd.SelectedDate.ToString());
                    builder.Append("'");
                }

                if (builder.Length > 0)
                    builder.Insert(0, " WHERE ");
                DataGrid1.BindData(Announcements.GetAnnouncementsByCondition(builder.ToString()));
            }

            #endregion
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            Search.Click += new EventHandler(Search_Click);
            DelRec.Click += new EventHandler(DelRec_Click);

            DataGrid1.TableHeaderName = "公告列表";
            DataGrid1.ColumnSpan = 7;
        }

        #endregion

    }
}