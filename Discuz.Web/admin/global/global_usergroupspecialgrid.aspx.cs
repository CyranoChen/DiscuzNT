using System;
using System.Web.UI.WebControls;

using Discuz.Forum;
using DataGrid = Discuz.Control.DataGrid;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 特殊用户组列表
    /// </summary>
    
    public partial class usergroupspecialgrid : AdminPage
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
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "特殊组列表";
            DataGrid1.Attributes.Add("borderStyle", "2");
            DataGrid1.BindData(UserGroups.GetSpecialUserGroup());
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression; 
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            DataGrid1.DataKeyField = "groupid";
            DataGrid1.ColumnSpan = 12;
        }

        #endregion
    }
}