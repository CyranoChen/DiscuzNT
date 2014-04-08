using System;
using System.Data;
using System.Web.UI.WebControls;

using Discuz.Forum;
using Discuz.Common;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 帖子列表
    /// </summary>
    public partial class postgrid : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            #region 数据绑定

            DataGrid1.AllowCustomPaging = false;
            DataTable dt = AdminTopics.AdminGetPostList(DNTRequest.GetInt("tid", -1), DataGrid1.PageSize, DataGrid1.CurrentPageIndex + 1).Tables[0];

            foreach (DataRow dr in dt.Select("layer=0"))
            {
                dt.Rows.Remove(dr);
            }

            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();

            #endregion
        }


        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }


        protected void DataGrid_Cancel(Object sender, DataGridCommandEventArgs E)
        {
            DataGrid1.Cancel();
        }

        public string Invisible(string invisible)
        {
            return invisible == "1" ? "<div align=center><img src=../images/OK.gif /></div>" : "<div align=center><img src=../images/Cancel.gif /></div>";
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            DataGrid1.DataKeyField = "pid";
            DataGrid1.TableHeaderName = "发帖列表";
            DataGrid1.ColumnSpan = 7;
        }

        #endregion
    }
}