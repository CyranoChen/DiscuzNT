using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Discuz.Forum;
using DataGrid = Discuz.Control.DataGrid;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// URL重定向
    /// </summary>
    
    public partial class urlgrid : AdminPage
    {
        public DataSet dsSrc = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            #region 绑定伪静态url的替换规则
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.DataKeyField = "name";
            DataGrid1.TableHeaderName = "伪静态url的替换规则";
            dsSrc.ReadXml(Server.MapPath("../../config/urls.config"));
            DataGrid1.DataSource = dsSrc.Tables[0];
            DataGrid1.DataBind();
            #endregion
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }


        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
            BindData();
        }

        protected void DataGrid_Edit(Object sender, DataGridCommandEventArgs E)
        {
            #region 编辑伪静态Url替换规则
            DataGrid1.EditItemIndex = E.Item.ItemIndex;
            dsSrc.Reset();
            dsSrc.ReadXml(Server.MapPath("../../config/urls.config"));
            DataGrid1.DataSource = dsSrc.Tables[0];
            DataGrid1.DataBind();
            #endregion
        }

        protected void DataGrid_Cancel(Object sender, DataGridCommandEventArgs E)
        {
            #region 取消编辑
            DataGrid1.EditItemIndex = -1;
            dsSrc.Reset();
            dsSrc.ReadXml(Server.MapPath("../../config/urls.config"));
            DataGrid1.DataSource = dsSrc.Tables[0];
            DataGrid1.DataBind();
            #endregion
        }

        protected void DataGrid_Update(Object sender, DataGridCommandEventArgs E)
        {
            #region url 内容更新

            string name = ((TextBox)E.Item.FindControl("nametext")).Text;
            string path = ((TextBox)E.Item.Cells[2].Controls[0]).Text;
            string pattern = ((TextBox)E.Item.Cells[3].Controls[0]).Text;
            string page = ((TextBox)E.Item.Cells[4].Controls[0]).Text;
            string querystring = ((TextBox)E.Item.Cells[5].Controls[0]).Text;

            dsSrc.Reset();
            dsSrc.ReadXml(Server.MapPath("../../config/urls.config"));

            foreach (DataRow dr in dsSrc.Tables["rewrite"].Rows)
            {
                if (name == dr["name"].ToString().Trim())
                {
                    dr["path"] = path;
                    dr["pattern"] = pattern;
                    dr["page"] = page;
                    dr["querystring"] = querystring;
                }
            }

            try
            {
                dsSrc.WriteXml(Server.MapPath("../../config/urls.config"));
                dsSrc.Reset();
                dsSrc.Dispose();
                SiteUrls.SetInstance();
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "Url设置", "");

                DataGrid1.EditItemIndex = -1;
                dsSrc.Reset();
                dsSrc.ReadXml(Server.MapPath("../../config/urls.config"));
                DataGrid1.DataSource = dsSrc.Tables[0];
                DataGrid1.DataBind();
            }
            catch
            {
                base.RegisterStartupScript( "", "<script>alert('无法更新数据库.');window.location.href='global_urlgrid.aspx';</script>");
                return;
            }

            #endregion
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 对name设置为只读属性
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                TextBox nametext = (TextBox)e.Item.FindControl("nametext");
                nametext.Attributes.Add("size", "5");
                nametext.ReadOnly = true;
                nametext.Text = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "name"));
                for (int i = 0; i < DataGrid1.Columns.Count; i++) //只调整被编辑的列 
                {
                    if (i >= 2)
                    {
                        System.Web.UI.WebControls.TextBox txt = (System.Web.UI.WebControls.TextBox)e.Item.Cells[i].Controls[0];
                        txt.Width = 120;
                    }
                }
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
            this.DataGrid1.EditCommand += new DataGridCommandEventHandler(this.DataGrid_Edit);
            this.DataGrid1.CancelCommand += new DataGridCommandEventHandler(this.DataGrid_Cancel);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.DataGrid1.UpdateCommand += new DataGridCommandEventHandler(this.DataGrid_Update);

            DataGrid1.LoadEditColumn();
            DataGrid1.DataKeyField = "name";
            DataGrid1.AllowSorting = false;
            DataGrid1.AllowPaging = false;
            DataGrid1.ShowFooter = false;
        }

        #endregion


    }
}