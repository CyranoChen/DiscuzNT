using System;
using System.Web.UI;
using System.Data;
using System.Xml;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using Discuz.Cache;
using Discuz.Control;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;
using Discuz.Common.Xml;
using TextBox = System.Web.UI.WebControls.TextBox;

using System.Text;

namespace Discuz.Web.Admin
{
    public partial class global_navigationmanage : AdminPage
    {
        private DataTable navMenuTable = Navs.GetNavigation(true);
        protected void Page_Load(object sender, EventArgs e)
        {
            DataGrid1.DataKeyField = "id";
            string menuid = DNTRequest.GetString("menuid");
            string mode = DNTRequest.GetString("mode");
            if (mode != "")
            {
                if (mode == "del")
                {
                    Navs.DeleteNavigation(DNTRequest.GetQueryInt("id",0));
                    Response.Redirect(Request.Path + (DNTRequest.GetString("parentid") != "" ? "?parentid=" + DNTRequest.GetString("parentid") : ""),true);
                }
                else
                {
                    if (DNTRequest.GetFormString("name").Trim() == "" || DNTRequest.GetFormString("displayorder").Trim() == "" || DNTRequest.GetFormInt("displayorder", 0) > Int16.MaxValue)
                    {
                        this.RegisterStartupScript("", "<script type='text/javascript'>alert('名称或序号输入不合法。');window.location=window.location;</script>");
                        return;
                    }
                    if (menuid == "0")
                    {
                        NavInfo nav = new NavInfo();
                        nav.Parentid = DNTRequest.GetQueryInt("parentid",0);
                        GetFromData(nav);
                        Navs.InsertNavigation(nav);

                    }
                    else
                    {
                        NavInfo nav = new NavInfo();
                        nav.Id = DNTRequest.GetFormInt("menuid", 0);
                        GetFromData(nav);
                        Navs.UpdateNavigation(nav);
                    }
                    Response.Redirect(Request.RawUrl, true);
                }
            }
            else
            {
                BindDataGrid(DNTRequest.GetQueryInt("parentid",0));
                if(DNTRequest.GetString("parentid") == "")
                {
                    returnbutton.Visible = false;
                }
            }
        }

        private void GetFromData(NavInfo nav)
        {
            nav.Name = GetMaxlengthString(DNTRequest.GetFormString("name"),50);
            nav.Title = GetMaxlengthString(DNTRequest.GetFormString("title"), 255);
            nav.Url = GetMaxlengthString(DNTRequest.GetFormString("url"), 255);
            nav.Target = DNTRequest.GetFormInt("target", 0);
            nav.Available = DNTRequest.GetFormInt("available", 0);
            nav.Displayorder = DNTRequest.GetFormInt("displayorder", 0);
            nav.Level = DNTRequest.GetFormInt("level", 0);
        }

        private string GetMaxlengthString(string str,int len)
        {
            return str.Length <= len ? str : str.Substring(0, len);
        }

        private void BindDataGrid(int parentid)
        {
            DataGrid1.TableHeaderName = (parentid != 0 ? "子" : "") + "导航菜单管理";
            DataGrid1.AllowCustomPaging = false;

            DataTable navmenu = navMenuTable.Clone();
            foreach (DataRow dr in navMenuTable.Select("parentid=" + parentid))
            {
                navmenu.ImportRow(dr);
            }
            DataGrid1.DataSource = navmenu;
            DataGrid1.DataBind();
            string navscript = "\r\n<script type='text/javascript'>\r\nnav = [";
            foreach(DataRow dr in navmenu.Rows)
            {
                navscript += String.Format("\r\n{{id:'{0}',parentid:'{1}',name:'{2}',title:'{3}',url:'{4}',target:'{5}',type:'{6}',available:'{7}',displayorder:'{8}',level:'{9}'}},",
                    dr["id"], dr["parentid"], dr["name"], dr["title"], dr["url"], dr["target"], dr["type"], dr["available"], dr["displayorder"], dr["level"]);
            }
            navscript = navscript.TrimEnd(',') + "]\r\n</script>";
            this.RegisterStartupScript("", navscript);
        }

        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 数据绑定显示长度控制

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TextBox t = (TextBox)e.Item.Cells[0].Controls[0];
                t.Attributes.Add("size", "4");

                t = (TextBox)e.Item.Cells[2].Controls[0];
                t.Attributes.Add("size", "20");
            }

            #endregion
        }

        protected void saveNav_Click(object sender, EventArgs e)
        {
            int row = 0;
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                int id = int.Parse(o.ToString());
                string displayorder = DataGrid1.GetControlValue(row, "displayorder").Trim();
                string url = DataGrid1.GetControlValue(row, "url").Trim();
                NavInfo nav = Navs.GetNavigation(id);
                if (nav == null)
                    continue;
                if (!Utils.IsNumeric(displayorder) || url == "")
                {
                    row++;
                    continue;
                }
                if (nav.Displayorder != int.Parse(displayorder) || nav.Url != url)
                {
                    nav.Displayorder = int.Parse(displayorder);
                    nav.Url = url;
                    Navs.UpdateNavigation(nav);
                }
                row++;
            }
            Response.Redirect(Request.RawUrl, true);
        }
        //private DataTable GetNavmenuDataTable()
        //{
        //    DataTable navmenu = new DataTable();
        //    navmenu.Columns.Add("id", System.Type.GetType("System.Int32"));
        //    navmenu.Columns.Add("parentid", System.Type.GetType("System.Int32"));
        //    navmenu.Columns.Add("name", System.Type.GetType("System.String"));
        //    navmenu.Columns.Add("title", System.Type.GetType("System.String"));
        //    navmenu.Columns.Add("url", System.Type.GetType("System.String"));
        //    navmenu.Columns.Add("target", System.Type.GetType("System.Int16"));
        //    navmenu.Columns.Add("type", System.Type.GetType("System.Int16"));
        //    navmenu.Columns.Add("available", System.Type.GetType("System.Int16"));
        //    navmenu.Columns.Add("displayorder", System.Type.GetType("System.Int32"));
        //    navmenu.Columns.Add("highlight", System.Type.GetType("System.Int16"));
        //    navmenu.Columns.Add("level", System.Type.GetType("System.Int32"));
        //    IDataReader reader = Navs.GetNavigation(true);
        //    while (reader.Read())
        //    {
        //        DataRow dr = navmenu.NewRow();
        //        dr["id"] = Utils.StrToInt(reader["id"], 0);
        //        dr["parentid"] = Utils.StrToInt(reader["parentid"], 0);
        //        dr["name"] = reader["name"].ToString().Trim();
        //        dr["title"] = reader["title"].ToString().Trim();
        //        dr["url"] = reader["url"].ToString().Trim();
        //        dr["target"] = Utils.StrToInt(reader["target"], 0);
        //        dr["type"] = Utils.StrToInt(reader["type"], 0);
        //        dr["available"] = Utils.StrToInt(reader["available"], 0);
        //        dr["displayorder"] = Utils.StrToInt(reader["displayorder"], 0);
        //        dr["highlight"] = Utils.StrToInt(reader["highlight"], 0);
        //        dr["level"] = Utils.StrToInt(reader["level"], 0);
        //        navmenu.Rows.Add(dr);
        //    }
        //    return navmenu;
        //}

        protected string GetSubNavMenuManage(string id,string type)
        {
            if((navMenuTable.Select("parentid=" + id).Length != 0 || type == "1") && DNTRequest.GetString("parentid") == "")
            {
                return String.Format("<a href=\"?parentid={0}\">管理子菜单</a>", id);
            }
            return "";
        }

        protected string GetDeleteLink(string id,string type)
        {
            if(type == "1" && navMenuTable.Select("parentid=" + id).Length == 0)
            {
                return String.Format("<a href=\"?{0}mode=del&id={1}\" onclick=\"return confirm('确认要将该菜单项删除吗?');\">删除</a>", 
                    (DNTRequest.GetString("parentid") != "" ? "parentid=" + DNTRequest.GetString("parentid") + "&" : ""), id);
            }
            return "";
        }

        protected string GetLink(string url)
        {
            if (url.ToLower().StartsWith("http://"))
                return url;
            return String.Format("../../{0}", url);
        }

        protected string GetLevel(string level)
        {
            switch(level)
            {
                case "0":
                    return "游客";
                case "1":
                    return "会员";
                case "2":
                    return "版主";
                case "3":
                    return "管理员";
            }
            return "";
        }
    }
}

