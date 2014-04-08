using System;
using System.Web.UI.WebControls;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
	/// <summary>
	/// Discuz!NT代码列表. 
	/// </summary>
    public partial class bbcodegrid : AdminPage
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
            DataGrid1.DataSource = BBCodes.GetBBCode();
            DataGrid1.DataBind();
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
	
		private void DelRec_Click(object sender, EventArgs e)
		{
			#region 删除选定的discuz!NT代码

			if (this.CheckCookie())
			{
				if (DNTRequest.GetString("id") != "")
				{
                    BBCodes.DeleteBBCode(DNTRequest.GetString("id"));
                    Response.Redirect("forum_bbcodegrid.aspx");
				}
				else
				{
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');window.location.href='forum_bbcodegrid.aspx';</script>");
				}
			}

			#endregion
		}

		public string BoolStr(string closed)
		{
			#region 根据记录信息设置显示图片

			if (closed == "1")
			{
				return "<div align=center><img src=../images/OK.gif /></div>";
			}
			else
			{
				return "<div align=center><img src=../images/Cancel.gif /></div>";
			}

			#endregion
		}

		private void SetAvailable_Click(object sender, EventArgs e)
		{
			#region 将选定的Discuz!NT代码置为有效状态

			if (this.CheckCookie())
			{
				if (DNTRequest.GetString("id") != "")
				{
                    BBCodes.BatchUpdateAvailable(1, DNTRequest.GetString("id"));
                    Response.Redirect("forum_bbcodegrid.aspx");
				}
				else
				{
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');window.location.href='forum_bbcodegrid.aspx';</script>");
				}
			}

			#endregion
		}

		private void SetUnAvailable_Click(object sender, EventArgs e)
		{
			#region 将选定的Discuz!NT代码置为无效状态

			if (this.CheckCookie())
			{
				if (DNTRequest.GetString("id") != "")
				{
                    BBCodes.BatchUpdateAvailable(0, DNTRequest.GetString("id"));
                    Response.Redirect("forum_bbcodegrid.aspx");
				}
				else
				{
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');window.location.href='forum_bbcodegrid.aspx';</script>");
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
			this.DelRec.Click += new EventHandler(this.DelRec_Click);
			this.SetAvailable.Click += new EventHandler(this.SetAvailable_Click);
			this.SetUnAvailable.Click += new EventHandler(this.SetUnAvailable_Click);

			DataGrid1.TableHeaderName = "Discuz!NT代码列表";
			DataGrid1.ColumnSpan = 6;
		}

		#endregion
	}
}