using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 手动调整版块
    /// </summary>
    public partial class forumsgrid : AdminPage
    {
        #region 控件声明

        protected Button SysteAutoSet;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            #region 绑定数据
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "论坛列表";
            DataGrid1.DataSource = buildGridData();
            DataGrid1.DataBind();
            #endregion
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataView dv = new DataView(buildGridData());
            dv.Sort = e.SortExpression.ToString();
            DataGrid1.DataSource = dv;
            DataGrid1.DataBind();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }

        private void SaveForum_Click(object sender, EventArgs e)
        {
            #region 保存版块修改信息
            int row = -1;
            bool error = false;
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                int fid = int.Parse(o.ToString());
                string name = DataGrid1.GetControlValue(row, "name").Trim();
                string subforumcount = DataGrid1.GetControlValue(row, "subforumcount").Trim();
                string displayorder = DataGrid1.GetControlValue(row, "displayorder").Trim();
                if (name == "" || !Utils.IsNumeric(subforumcount) || !Utils.IsNumeric(displayorder))
                {
                    error = true;
                    continue;
                }
                ForumInfo forumInfo = Forums.GetForumInfo(fid);
                forumInfo.Name = name;
                forumInfo.Subforumcount = int.Parse(subforumcount);
                forumInfo.Displayorder = int.Parse(displayorder);
                AdminForums.UpdateForumInfo(forumInfo);
                row++;
            }
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UI/ForumListBoxOptions");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/ForumList");
            if(error)
                base.RegisterStartupScript("PAGE", "alert('某些记录取值不正确，未能被更新！');window.location.href='forum_forumsgrid.aspx';");
            else
                base.RegisterStartupScript("PAGE", "window.location.href='forum_forumsgrid.aspx';");
            #endregion
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 数据绑定显示长度控制

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TextBox t = (TextBox)e.Item.Cells[1].Controls[0];
                t.Attributes.Add("maxlength", "50");
                t.Width = 80;

                t = (TextBox)e.Item.Cells[5].Controls[0];
                t.Attributes.Add("maxlength", "8");
                t.Width = 30;

                t = (TextBox)e.Item.Cells[6].Controls[0];
                t.Attributes.Add("maxlength", "8");
                t.Width = 30;
            }

            #endregion
        }

        private DataTable buildGridData()
        {
            #region 数据绑定

            DataTable dt = Forums.GetForumListForDataTable();
            foreach (DataRow dr in dt.Rows)
            {
                dr["parentidlist"] = dr["parentidlist"].ToString().Trim();
                dr["name"] = dr["name"].ToString().Trim().Replace("\"", "'");
            }
            return dt;

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
            this.SaveForum.Click += new EventHandler(this.SaveForum_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);

            DataGrid1.DataKeyField = "fid";
            DataGrid1.TableHeaderName = "论坛列表";
            DataGrid1.AllowPaging = false;
            DataGrid1.ShowFooter = false;
            DataGrid1.SaveDSViewState = true;
        }

        #endregion

    }
}