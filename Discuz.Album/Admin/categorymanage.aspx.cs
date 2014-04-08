using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Discuz.Forum;
using Discuz.Data;
using Discuz.Common;
using Discuz.Entity;
using Discuz.Web.Admin;
using Discuz.Album.Data;


namespace Discuz.Album.Admin
{
#if NET1
    public class categorymanage : AdminPage
#else
    public partial class CategoryManage : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.DataGrid DataGrid1;
        protected Discuz.Control.TextBox albumcateTitle;
        protected Discuz.Control.TextBox displayorder;
        protected Discuz.Control.TextBox description;
        protected Discuz.Control.Button SubmitButton;
        protected Discuz.Control.Button DelRec;

        protected System.Web.UI.HtmlControls.HtmlInputHidden albumCateId;
		protected System.Web.UI.WebControls.Label prompt;
        #endregion
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            #region 绑定数据
            DataGrid1.BindData(DbProvider.GetInstance().GetAlbumCategorySql());
            #endregion
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            string title = this.albumcateTitle.Text;
            string description = this.description.Text;
            int displayorder = Utils.StrToInt(this.displayorder.Text, 0);
            //bool error = false;
            if(title == "" || this.displayorder.Text == "")
            {
                //error = true;
                return;
            }
            AlbumCategoryInfo aci = new AlbumCategoryInfo();
            aci.Title = title;
            aci.Description = description;
            aci.Displayorder = displayorder;
            DbProvider.GetInstance().AddAlbumCategory(aci);
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            #region 保存相册分类
            if (this.CheckCookie())
            {
                //

                //AlbumCategoryInfo aci = new AlbumCategoryInfo();
                //aci.Title = title;
                //aci.Description = description;
                //aci.Displayorder = displayorder;

                //int albumcateid = Utils.StrToInt(this.albumCateId.Value, 0);
                ////增加新分类
                //if (albumcateid == 0)
                //{
                //    DatabaseProvider.GetInstance().AddAlbumCategory(aci);
                //}
                //else//更新原分类
                //{
                //    aci.Albumcateid = albumcateid;
                //    DatabaseProvider.GetInstance().UpdateAlbumCategory(aci);
                //}
                int row = 0;
                bool error = false;
                foreach (object o in DataGrid1.GetKeyIDArray())
                {
                    int id = int.Parse(o.ToString());
                    string title = DataGrid1.GetControlValue(row, "title").Trim();
                    string description = DataGrid1.GetControlValue(row, "description").Trim();
                    string displayorder = DataGrid1.GetControlValue(row, "displayorder");
                    if (title == "" || displayorder == "")
                    {
                        error = true;
                        continue;
                    }
                    AlbumCategoryInfo aci = new AlbumCategoryInfo();
                    aci.Title = title;
                    aci.Description = description;
                    aci.Displayorder = int.Parse(displayorder);
                    aci.Albumcateid = id;
                    DbProvider.GetInstance().UpdateAlbumCategory(aci);
                    row++;
                    if (error)
                    {
                        //
                    }
                }

                //更新缓存
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Space/AlbumCategory");
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Photo/AlbumCategoryMenu");
                BindData();
      			Response.Redirect("album_categorymanage.aspx");
            }
            #endregion
        }

        protected void DelRec_Click(object sender, EventArgs e)
        {
            #region 删除编辑分类
            if (this.CheckCookie())
            {
                string delid = DNTRequest.GetString("delid");
                if (delid != "")
                {
                    foreach (string id in delid.Split(','))
                    {
                        DbProvider.GetInstance().DeleteAlbumCategory(int.Parse(id));
                    }
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Space/AlbumCategory");
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Photo/AlbumCategoryMenu");
                    BindData();
                }
                Response.Redirect("album_categorymanage.aspx");
                //int albumcateid = Utils.StrToInt(DataGrid1.DataKeys[e.Item.ItemIndex], 0);

                //if (albumcateid > 0)
                //{
                //    DatabaseProvider.GetInstance().DeleteAlbumCategory(albumcateid);
                //    //更新缓存
                //    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Space/AlbumCategory");
                //    BindData();
                //}
                //Response.Redirect("album_categorymanage.aspx");
            }
            #endregion
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        protected void DataGrid1_EditCommand(object source, DataGridCommandEventArgs e)
        {
            #region 加载编辑分类
            if (this.CheckCookie())
            {
                //编辑一个分类
                BindData();
                int albumcateid = Utils.StrToInt(DataGrid1.DataKeys[e.Item.ItemIndex], 0);
                if (albumcateid > 0)
                {
                    AlbumCategoryInfo aci = DTOProvider.GetAlbumCategory(albumcateid);
                    this.albumcateTitle.Text = aci.Title;
                    this.description.Text = aci.Description;
                    this.displayorder.Text = aci.Displayorder.ToString();
                }
                prompt.Text = "编辑相册分类";
            }
            #endregion
        }

        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 绑定JS脚本
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TextBox t = (TextBox)e.Item.Cells[1].Controls[0];
                t.Width = 200;

                t = (TextBox)e.Item.Cells[2].Controls[0];
                t.Width = 40;

                t = (TextBox)e.Item.Cells[3].Controls[0];
                t.Width = 400;
            }
            #endregion
        }
    }
}
