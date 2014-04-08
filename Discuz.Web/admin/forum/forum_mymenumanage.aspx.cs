using System;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Xml;

using Discuz.Aggregation;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common.Xml;

namespace Discuz.Web.Admin
{
	
    public partial class mymenumanage : AdminPage
	{
        private string configPath;

		private void Page_Load(object sender, System.EventArgs e)
        {
            #region 设置前台Js操作
            atext.Attributes.Add("onkeyup", "setexample()");
            ahref.Attributes.Add("onkeyup", "setexample()");
            aonclick.Attributes.Add("onkeyup", "setexample()");
            atarget.Attributes.Add("onkeyup", "setexample()");
            #endregion

            if (!Page.IsPostBack)
			{
				BindData();
			}
		}

		public void BindData()
        {
            #region 绑定数据
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.DataKeyField = "menuid";
			DataGrid1.TableHeaderName = "我的菜单列表";

            
		    DataSet dsSrc = new DataSet();
			XmlDocumentExtender xmldocument = new XmlDocumentExtender();
			xmldocument.Load(configPath);
			XmlNode node = xmldocument.SelectSingleNode("/menuset");
            if (node == null ||node.ChildNodes.Count == 0)
                return;
			XmlNodeReader rdr = new XmlNodeReader(node);
			dsSrc.ReadXml(rdr);
            dsSrc.Tables[0].Columns.Add("menuid");
            int i = 0;
            foreach (DataRow dr in dsSrc.Tables[0].Rows)
            {
                dr["menuid"] = i.ToString();
                i++;
            }
			DataGrid1.DataSource = dsSrc.Tables[0];
			DataGrid1.DataBind();
            #endregion
        }

        private void addmenu_Click(Object sender, EventArgs e)
        {
            #region 插入菜单

            if ((atext.Text.Trim() != "") && (ahref.Text.Trim() != ""))
            {
                XmlDocumentExtender doc = new XmlDocumentExtender();
                doc.Load(configPath);
                int lastmenuorder = 0;
                if (doc.SelectSingleNode("/menuset").ChildNodes.Count != 0)
                {
                   lastmenuorder = int.Parse(doc.SelectSingleNode("/menuset").LastChild["menuorder"].InnerText);
                }
                lastmenuorder++;
                XmlElement menunode = doc.CreateElement("menuitem");
                doc.AppendChildElementByNameValue(ref menunode, "menuorder", lastmenuorder.ToString());
                doc.AppendChildElementByNameValue(ref menunode, "text", atext.Text.Trim());
                doc.AppendChildElementByNameValue(ref menunode, "href", ahref.Text.Trim());
                doc.AppendChildElementByNameValue(ref menunode, "onclick", aonclick.Text.Trim(),true);
                doc.AppendChildElementByNameValue(ref menunode, "target", atarget.Text.Trim());

                doc.CreateNode("/menuset").AppendChild(menunode);
                doc.Save(configPath);
                CreateJsFile();
                try
                {
                    BindData();
                    ResetForm();
                    base.RegisterStartupScript("PAGE", "window.location.href='forum_mymenumanage.aspx';");
                    return;
                }
                catch
                {
                    base.RegisterStartupScript("", "<script>alert('无法更新XML文件');window.location.href='forum_mymenumanage.aspx';</script>");
                    return;
                }

            }
            else
            {
                base.RegisterStartupScript("", "<script>alert('链接文字和链接地址是必须输入的，如果无链接地址请输入\"#\".');window.location.href='forum_mymenumanage.aspx';</script>");
                return;
            }

            #endregion
        }
      
        private void ResetForm()
        {
            #region 重置输入框
            atext.Text = "";
            ahref.Text = "";
            aonclick.Text = "";
            atarget.Text = "";
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

		protected void SaveMyMenu_Click(object sender, EventArgs e)
        {
            #region 保存“我的”菜单
            int menuid = 0;
            bool error = false;
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList __xmlnodelist = doc.SelectSingleNode("/menuset").ChildNodes;
            if ((__xmlnodelist != null) && (__xmlnodelist.Count > 0))
            {
                doc.InitializeNode("/menuset");
            }
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                string menuorder = DataGrid1.GetControlValue(menuid, "menuorder");
                string text = DataGrid1.GetControlValue(menuid, "text");
                string href = DataGrid1.GetControlValue(menuid, "href");
                string onclick = DataGrid1.GetControlValue(menuid, "onclick");
                string target = DataGrid1.GetControlValue(menuid, "target");
                if ((text.Trim() == "") && (href.Trim() == ""))
                {
                    error = true;
                    continue;
                }
                                
                bool insert = false;
                XmlElement __newxmlnode = doc.CreateElement("menuitem");
                doc.AppendChildElementByNameValue(ref __newxmlnode, "menuorder", menuorder);
                doc.AppendChildElementByNameValue(ref __newxmlnode, "text", text);
                doc.AppendChildElementByNameValue(ref __newxmlnode, "href", href);
                doc.AppendChildElementByNameValue(ref __newxmlnode, "onclick", onclick, true);
                doc.AppendChildElementByNameValue(ref __newxmlnode, "target", target);
                foreach (XmlNode __node in __xmlnodelist)
                {
                    if (int.Parse(__node["menuorder"].InnerText) > int.Parse(menuorder))
                    {
                        doc.SelectSingleNode("/menuset").InsertBefore(__newxmlnode, __node);
                        insert = true;
                        break;
                    }
                }
                if (!insert)
                {
                    doc.SelectSingleNode("/menuset").AppendChild(__newxmlnode);
                }
                menuid++;
            }
            doc.Save(configPath);
            CreateJsFile();
            if (error)
            {
                base.RegisterStartupScript("", "<script>alert('链接文字和链接地址是必须输入的，如果无链接地址请输入\"#\".');window.location.href='forum_mymenumanage.aspx';</script>");
            }
            else
            {
                base.RegisterStartupScript("", "<script>window.location.href='forum_mymenumanage.aspx';</script>");
            }
            #endregion
        }

		private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 设置文本框宽度
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
                ((TextBox)e.Item.Cells[1].Controls[0]).Width = 30;
                ((TextBox)e.Item.Cells[2].Controls[0]).Width = 60;
                ((TextBox)e.Item.Cells[3].Controls[0]).Width = 150;
                ((TextBox)e.Item.Cells[4].Controls[0]).Width = 150;
                ((TextBox)e.Item.Cells[5].Controls[0]).Width = 50;
            }
            #endregion
        }

		private void DelRec_Click(object sender, EventArgs e)
		{
			#region 删除菜单项
			if (this.CheckCookie())
			{
                string menuidlist = DNTRequest.GetString("menuid");
                if (menuidlist != "")
                {
                    int delcount = 0;
                    XmlDocumentExtender doc = new XmlDocumentExtender();
                    doc.Load(configPath);
                    XmlNodeList __xmlnodelist = doc.SelectSingleNode("/menuset").ChildNodes;
                    foreach (string menuid in menuidlist.Split(','))
                    {
                        doc.SelectSingleNode("/menuset").RemoveChild(__xmlnodelist.Item(int.Parse(menuid) - delcount));
                        delcount++;
                    }
                    doc.Save(configPath);
                    CreateJsFile();
                    Response.Redirect("forum_mymenumanage.aspx");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='forum_mymenumanage.aspx';</script>");
                }
                CreateJsFile();
			}

			#endregion
		}

        /// <summary>
        /// 建立前台所使用的JS文件
        /// </summary>
        private void CreateJsFile()
        {
            #region 建立JS文件
            string jspath = Server.MapPath(BaseConfigs.GetForumPath + "javascript/mymenu.js");
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList __xmlnodelist = doc.SelectSingleNode("/menuset").ChildNodes;
            if (__xmlnodelist.Count == 0)
                return;
            StringBuilder jscontent = new StringBuilder();
            foreach (XmlNode __xmlnode in __xmlnodelist)
            {
                jscontent.Append(string.Format("document.write('<li><a href=\"{0}\" {1} {2}>{3}</a></li>');\r\n",
                    __xmlnode["href"].InnerText,
                    (__xmlnode["onclick"].InnerText == "" ? "" : "onclick=\"" + __xmlnode["onclick"].InnerText.Replace("'","\\'") + "\""),
                    (__xmlnode["target"].InnerText == "" ? "" : "target=\"" + __xmlnode["target"].InnerText + "\""),
                    __xmlnode["text"].InnerText));
            }
            jscontent = jscontent.Replace(" onclick=\"\"", "").Replace(" target=\"\"","");
            using (FileStream fs = new FileStream(jspath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                Byte[] info = System.Text.Encoding.UTF8.GetBytes(jscontent.ToString());
                fs.Write(info, 0, info.Length);
                fs.Close();
            }
            #endregion

        }

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{
			this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.addmenu.Click += new EventHandler(this.addmenu_Click);
            this.SaveMyMenu.Click += new EventHandler(this.SaveMyMenu_Click);
			this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            configPath = Server.MapPath(BaseConfigs.GetForumPath + "config/mymenu.config");
		}
		#endregion
	}
}
