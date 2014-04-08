using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Common;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 支付日志列表
    /// </summary>

    public partial class paymentloggrid : AdminPage
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
            DataGrid1.VirtualItemCount = AdminPaymentLogs.RecordCount(ViewState["condition"] == null ? "" : ViewState["condition"].ToString());

            if (ViewState["condition"] == null)
            {
                DataGrid1.DataSource = AdminPaymentLogs.LogList(DataGrid1.PageSize, DataGrid1.CurrentPageIndex + 1);
            }
            else
            {
                DataGrid1.DataSource = AdminPaymentLogs.LogList(DataGrid1.PageSize, DataGrid1.CurrentPageIndex + 1, ViewState["condition"].ToString());
            }
            DataGrid1.DataBind();

            #endregion
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            #region 删除指定条件的日志信息

            if (this.CheckCookie())
            {
                string condition = "";
                condition = AdminModeratorLogs.GetDeleteModeratorManageCondition(Request.Form["deleteMode"].ToString(), 
                    DNTRequest.GetString("id").ToString(), deleteNum.Text.ToString(), deleteFrom.SelectedDate.ToString());

                if (condition != "")
                {
                    AdminPaymentLogs.DeleteLog(condition);
                    Response.Redirect("forum_paymentloggrid.aspx");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项或输入参数错误！');window.location.href='forum_paymentloggrid.aspx';</script>");
                }
            }
            #endregion
        }

        private void SearchLog_Click(object sender, EventArgs e)
        {
            #region 按指定查询条件搜索日志信息

            if (this.CheckCookie())
            {
                string sqlstring = AdminPaymentLogs.GetSearchPaymentLogCondition(postdatetimeStart.SelectedDate, postdatetimeEnd.SelectedDate,
                    Username.Text);

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
            if (e.Item.Cells[8].Text.ToString().Length > 8)
            {
                e.Item.Cells[8].Text = Utils.HtmlEncode(e.Item.Cells[8].Text.Substring(0, 8)) + "…";
            }
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
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);

            DataGrid1.TableHeaderName = "积分交易记录";
            DataGrid1.AllowSorting = false;
            DataGrid1.ColumnSpan = 8;
        }

        #endregion
    }
}