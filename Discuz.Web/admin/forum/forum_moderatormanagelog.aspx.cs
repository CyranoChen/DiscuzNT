using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 版主管理日志列表
    /// </summary>

    public partial class moderatormanagelog : AdminPage
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
            #region 数据绑定

            DataGrid1.AllowCustomPaging = true;
            DataGrid1.VirtualItemCount = GetRecordCount();

            DataTable LogList = ViewState["condition"] == null ?
                AdminModeratorLogs.LogList(DataGrid1.PageSize, DataGrid1.CurrentPageIndex + 1) :
                AdminModeratorLogs.LogList(DataGrid1.PageSize, DataGrid1.CurrentPageIndex + 1, ViewState["condition"].ToString());


            foreach (DataRow dr in LogList.Rows)
            {
                dr["reason"] = dr["reason"].ToString().Trim();
                dr["title"] = dr["title"].ToString().Trim() != "" ?
                    string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", BaseConfigs.GetForumPath + Urls.ShowTopicAspxRewrite(TypeConverter.ObjectToInt(dr["tid"]), 1), dr["title"]) :
                    "没有标题";
            }
            DataGrid1.DataSource = LogList;
            DataGrid1.DataBind();

            #endregion
        }

        private int GetRecordCount()
        {
            #region 得到日志记录数

            if (ViewState["condition"] == null)
            {
                return AdminModeratorLogs.RecordCount();
            }
            else
            {
                return AdminModeratorLogs.RecordCount(ViewState["condition"].ToString());
            }

            #endregion
        }


        private void DelRec_Click(object sender, EventArgs e)
        {
            #region 删除指定条件的日志信息

            if (this.CheckCookie())
            {
                string condition = "";

                condition = AdminModeratorLogs.GetDeleteModeratorManageCondition(Request.Form["deleteMode"].ToString(), DNTRequest.GetString("id").ToString(), deleteNum.Text.ToString(), deleteFrom.SelectedDate.ToString());
                if (condition != "")
                {
                    AdminModeratorLogs.DeleteLog(condition);
                    Response.Redirect("forum_moderatormanagelog.aspx");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('您未选中任何选项或输入参数错误！');window.location.href='forum_moderatormanagelog.aspx';</script>");
                }
            }

            #endregion
        }

        public string GroupName(string groupid)
        {
            #region 通过组ID获取组的名称

            UserGroupInfo userGroupInfo = UserGroups.GetUserGroupInfo(TypeConverter.StrToInt(groupid));
            return userGroupInfo != null ? userGroupInfo.Grouptitle : "";

            #endregion
        }

        private void SearchLog_Click(object sender, EventArgs e)
        {
            #region 按指定查询条件搜索日志信息

            if (this.CheckCookie())
            {
                string sqlstring = AdminModeratorLogs.GetSearchModeratorManageLogCondition(postdatetimeStart.SelectedDate, postdatetimeEnd.SelectedDate,
                    Username.Text, others.Text);

                ViewState["condition"] = sqlstring;
                DataGrid1.CurrentPageIndex = 0;
                BindData();
            }

            #endregion
        }


        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }

        public void GoToPagerButton_Click(object sender, EventArgs e)
        {
            BindData();
        }


        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.Cells[9].Text.ToString().Length > 8)
            {
                e.Item.Cells[9].Text = Utils.HtmlEncode(e.Item.Cells[9].Text.Substring(0, 8)) + "…";
            }
        }

        public string BoolStr(string closed)
        {
            return closed == "1" ? "<div align=center><img src=../images/OK.gif /></div>" : "<div align=center><img src=../images/Cancel.gif /></div>";
        }


        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SearchLog.Click += new EventHandler(this.SearchLog_Click);
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.DataGrid1.GoToPagerButton.Click += new EventHandler(GoToPagerButton_Click);
            DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);

            this.Load += new EventHandler(this.Page_Load);

            DataGrid1.TableHeaderName = "版主管理日志记录";
            DataGrid1.AllowSorting = false;
            DataGrid1.ColumnSpan = 9;
        }

        #endregion
    }
}