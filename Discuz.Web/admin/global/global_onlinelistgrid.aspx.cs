using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

using Discuz.Common;
using Discuz.Forum;
using DataGrid = Discuz.Control.DataGrid;
using Discuz.Config;



namespace Discuz.Web.Admin
{
    /// <summary>
    /// 在线图例列表
    /// </summary>
    public partial class onlinelistgrid : AdminPage
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
            #region 数据控件绑定

            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "在线列表定制";
            DataGrid1.DataSource = LoadDataInfo();
            DataGrid1.DataBind();

            #endregion
        }

        private DataSet LoadDataInfo()
        {
            #region 加载数据

            DataSet dsSrc = new DataSet();
            DataTable dt = UserGroups.GetOnlineList();
            dsSrc.Tables.Add(dt.Clone());
            foreach (DataRow dr in dt.Rows)
            {
                dsSrc.Tables[0].ImportRow(dr);
            }
            dsSrc.Tables[0].Columns.Add("newdisplayorder");
            dsSrc.Tables[0].Rows[0]["GroupName"] = "普通用户";
            foreach (DataRow dr in dsSrc.Tables[0].Rows)
            {
                if (!Utils.IsNumeric(dr["displayorder"].ToString()) || dr["displayorder"].ToString() == "0")
                {
                    dr["newdisplayorder"] = "不显示";
                }
                else
                {
                    dr["newdisplayorder"] = dr["displayorder"].ToString();
                }
            }

            DataTable dt2 = new DataTable("img");
            dt2.Columns.Add("imgfile", Type.GetType("System.String"));
            DataRow dr2 = dt2.NewRow();
            dr2["imgfile"] = "";
            dt2.Rows.Add(dr2);

            try
            {
                DirectoryInfo dirinfo = new DirectoryInfo(Server.MapPath("../../images/groupicons"));
                foreach (FileSystemInfo file in dirinfo.GetFileSystemInfos())
                {
                    if ((file != null) && (file.Extension != ""))
                    {
                        dr2 = dt2.NewRow();
                        if (file.Name.ToLower() == "thumbs.db") continue; //过滤掉Thumbs.db文件
                        dr2["imgfile"] = file.Name;
                        dt2.Rows.Add(dr2);
                    }
                }
                dsSrc.Tables.Add(dt2);
                dsSrc.Relations.Add(dt2.Columns["imgfile"], dsSrc.Tables[0].Columns["img"]);
            }
            catch
            {
                ;
            }
            return dsSrc;

            #endregion
        }

        protected void DataGrid_Update(Object sender, DataGridCommandEventArgs E)
        {
            #region 更新相关的在线图例信息

            int groupid = Utils.StrToInt(DataGrid1.DataKeys[E.Item.ItemIndex].ToString(), 0);
            int displayorder = Utils.StrToInt(((TextBox)E.Item.Cells[2].Controls[0]).Text, 0);
            if (displayorder < 0)
                displayorder = 0;   //不允许出现负数
            string title = ((TextBox)E.Item.Cells[3].Controls[0]).Text;
            string img = ((DropDownList)E.Item.FindControl("imgdropdownlist")).SelectedValue;

            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "在线列表定制", title);

            try
            {
                UserGroups.UpdateOnlineList(groupid, displayorder, img, title);
                BindData();
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UI/OnlineIconList");

                base.RegisterStartupScript( "PAGE",  "window.location.href='global_onlinelistgrid.aspx';");
            }
            catch
            {
                base.RegisterStartupScript( "", "<script>alert('无法更新数据库');window.location.href='global_onlinelistgrid.aspx';</script>");
            }

            #endregion
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 设置数据绑定的长度

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                DropDownList imgdropdownlist = (DropDownList)e.Item.FindControl("imgdropdownlist");

                imgdropdownlist.Items.Clear();
                foreach (DataRow r in LoadDataInfo().Tables[1].Rows)
                {
                    imgdropdownlist.Items.Add(new ListItem(r[0].ToString(), r[0].ToString()));
                }
                imgdropdownlist.DataBind();
                try
                {
                    imgdropdownlist.SelectedValue = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "img"));
                }
                catch
                {
                    ;
                }

                TextBox t = (TextBox)e.Item.Cells[2].Controls[0];
                t.Attributes.Add("maxlength", "6");
                t.Attributes.Add("size", "5");
                if (!Utils.IsNumeric(t.Text))
                    t.Text = "0";

                t = (TextBox)e.Item.Cells[3].Controls[0];
                t.Attributes.Add("maxlength", "50");
                t.Attributes.Add("size", "30");
            }

            #endregion
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
            BindData();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }

        protected void DataGrid_Edit(Object sender, DataGridCommandEventArgs E)
        {
            DataGrid1.EditItemIndex = E.Item.ItemIndex;
            BindData();
        }

        protected void DataGrid_Cancel(Object sender, DataGridCommandEventArgs E)
        {
            DataGrid1.EditItemIndex = -1;
            BindData();
        }

        public string GetImgLink(string img)
        {
            if (img != "")
            {
                return "<img src=../../images/groupicons/" + img + " height=16px width=16px border=0 />";
            }
            return "";
        }

        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DataGrid1.EditCommand += new DataGridCommandEventHandler(this.DataGrid_Edit);
            this.DataGrid1.CancelCommand += new DataGridCommandEventHandler(DataGrid_Cancel);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.DataGrid1.UpdateCommand += new DataGridCommandEventHandler(this.DataGrid_Update);

            DataGrid1.LoadEditColumn();
            DataGrid1.TableHeaderName = "在线列表";
            DataGrid1.DataKeyField = "groupid";
            DataGrid1.ColumnSpan = 5;
            DataGrid1.SaveDSViewState = true;
        }

        #endregion
    }
}