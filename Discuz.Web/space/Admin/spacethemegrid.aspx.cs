using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using Discuz.Config;
using Discuz.Data;
using Discuz.Cache;
using Discuz.Web.Admin;
using Discuz.Space.Data;


namespace Discuz.Space.Admin
{
	/// <summary>
	/// smilegrid 的摘要说明.
	/// </summary>

#if NET1
    public class SpaceThemeGrid : AdminPage
#else
    public partial class SpaceThemeGrid : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.Button Button1;
        protected Discuz.Control.DataGrid themesgrid;
        protected Discuz.Control.Button EditTheme;
        protected Discuz.Control.Button SubmitButton;

        protected System.Web.UI.WebControls.Literal themeInfoList;
        #endregion
#endif


        private ArrayList dirList = new ArrayList();
		private string themePath = BaseConfigs.GetForumPath + "Space/skins/themes";

		private void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				BindData();
			}
			BindThemesList();
		}

		public void BindData()
		{
			themesgrid.AllowCustomPaging = false;
			themesgrid.TableHeaderName = "个人空间主题列表";
			themesgrid.DataKeyField = "themeid";
            themesgrid.BindData(DbProvider.GetInstance().GetSapceThemeList(DNTRequest.GetInt("themeid", 0)));
		}

		protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
		{
			themesgrid.Sort = e.SortExpression.ToString();
		}

		protected void DataGrid_Delete(Object sender, DataGridCommandEventArgs E)
		{
			string id = themesgrid.DataKeys[E.Item.ItemIndex].ToString();
            themesgrid.DeleteByString(DbProvider.GetInstance().DeleteSpaceThemeByThemeid(int.Parse(id)));
            DNTCache.GetCacheService().RemoveObject("/Space/ThemeList");
		}

		protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			themesgrid.LoadCurrentPageIndex(e.NewPageIndex);
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
			this.EditTheme.Click += new EventHandler(this.EditTheme_Click);
			this.SubmitButton.Click += new EventHandler(this.SubmitButton_Click);
			this.Load += new EventHandler(this.Page_Load);
			themesgrid.ColumnSpan = 7;
		}

		#endregion

        private void DelRec_Click(object sender, EventArgs e)
		{
			if (this.CheckCookie())
			{
				if (DNTRequest.GetString("id") != "")
				{
					string idlist = DNTRequest.GetString("id");

                    DbProvider.GetInstance().DeleteSpaceThemes(idlist);
                    DNTCache.GetCacheService().RemoveObject("/Space/ThemeList");
					Response.Redirect("space_spacethemegrid.aspx?themeid=" + DNTRequest.GetInt("themeid", 0));
					Response.End();
                    return;
				}
				else
				{
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');window.location.href='space_spacethemegrid.aspx?themeid=" + DNTRequest.GetInt("themeid", 0) + "';</script>");
				}
			}
		}


		private void EditTheme_Click(object sender, EventArgs e)
		{
			int row = 0;

			foreach (object o in themesgrid.GetKeyIDArray())
			{
				if (themesgrid.GetControlValue(row, "name") == "")
				{
					continue;
				}

                DbProvider.GetInstance().UpdateSpaceThemeInfo(Utils.StrToInt(o, 0), themesgrid.GetControlValue(row, "name"), themesgrid.GetControlValue(row, "author"), themesgrid.GetControlValue(row, "copyright"));
                row++;
			}
            DNTCache.GetCacheService().RemoveObject("/Space/ThemeList");
            base.RegisterStartupScript( "", "<script>window.location.href='space_spacethemegrid.aspx?themeid=" + DNTRequest.GetInt("themeid", 0) + "';</script>");
		}

		private ArrayList GetThemeDirList()
		{
			DirectoryInfo themeDir = new DirectoryInfo(Utils.GetMapPath(themePath));
			DirectoryInfo[] dirs = themeDir.GetDirectories();
			ArrayList temp = new ArrayList();
			foreach (DirectoryInfo dir in dirs)
			{
				temp.Add(dir.Name);
			}
			return temp;
		}

		private void BindThemesList()
		{
			themeInfoList.Text = "";
			dirList = GetThemeDirList();
            DataTable dt = DbProvider.GetInstance().GetSpaceThemeDirectory();
			foreach (DataRow theme in dt.Rows)
			{
				dirList.Remove(theme["directory"].ToString());
			}
			int i = 1;
			foreach (string dir in dirList)
			{
				themeInfoList.Text += "<tr onmouseover=\"this.className='mouseoverstyle'\" onmouseout=\"this.className='mouseoutstyle'\" style=\"cursor:hand;\">\n";
				themeInfoList.Text += "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='checkbox' id='id" + i + "' name='id" + i + "' value='" + i + "'/></td>\n";
				SpaceThemes.SpaceThemeAboutInfo themeInfo = SpaceThemes.GetThemeAboutInfo(Utils.GetMapPath(themePath + "/" + dir));
                themeInfoList.Text += "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='text' id='themename" + i + "' name='themename" + i + "' value='" + (themeInfo.name == "" ? dir : themeInfo.name) + "' size='10' /></td>\n";
				themeInfoList.Text += "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='hidden' name='dirname" + i + "' value='" + dir + "' />" + dir + "</td>\n";
				themeInfoList.Text += "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='text' name='author" + i + "' value='" + (themeInfo.author == "" ? username : themeInfo.author) + "' size='10' /></td>\n";
				themeInfoList.Text += "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='text' name='createdate" + i + "' value='" + (themeInfo.createdate == "" ? DateTime.Now.ToShortDateString() : themeInfo.createdate) + "' size='10'></td>\n";
				themeInfoList.Text += "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='text' name='copyright" + i + "' value='" + (themeInfo.copyright == "" ? "版权归" + username + "所有" : themeInfo.copyright) + "' size='30'></td>\n";
                themeInfoList.Text += "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><span id='layerimg" + i + "' onmouseover='showMenu(this.id, 0, 0, 1, 0);' >预览</span>\n";
                themeInfoList.Text += "<div id='layerimg" + i + "_menu' style='display:none'>\n";
				themeInfoList.Text += "<img src='../../space/skins/themes/" + dir + "/about.png' onerror=\"this.src='../../images/common/none.gif'\"/></div></td>\n";							
				themeInfoList.Text += "</tr>\n";
				i++;
			}
		}

		public void SubmitButton_Click(object sender, EventArgs e)
		{
			int type = DNTRequest.GetInt("themeid", 0);
			for (int i = 1; i <= dirList.Count; i++)
			{
				if (DNTRequest.GetFormString("id" + i) != "")
				{
					try
					{
                        DbProvider.GetInstance().AddSpaceTheme(DNTRequest.GetFormString("dirname" + i), DNTRequest.GetFormString("themename" + i), type, DNTRequest.GetFormString("author" + i), DNTRequest.GetFormString("createdate" + i), DNTRequest.GetFormString("copyright" + i));
                        DNTCache.GetCacheService().RemoveObject("/Space/ThemeList");
					}
					catch
					{
                        base.RegisterStartupScript( "", "<script>alert('增加主题时发生错误！');window.location.href='space_spacethemegrid.aspx?themeid=" + DNTRequest.GetInt("themeid", 0) + "';</script>");
					}
				}
			}
            base.RegisterStartupScript( "", "<script>window.location.href='space_spacethemegrid.aspx?themeid=" + DNTRequest.GetInt("themeid", 0) + "';</script>");
		}
	}
}