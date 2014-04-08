using System;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Xml;

using Discuz.Aggregation;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common.Xml;

namespace Discuz.Web.Admin
{
    public partial class aggregation_rotatepic : AdminPage
	{
        private string configPath;
        private string nodeName;
		private string targetNode;

		public DataSet dsSrc = new DataSet();

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				BindData();
			}
		}

		public void BindData()
        {
            #region 绑定轮换图片列表
            DataGrid1.AllowCustomPaging = false;
			DataGrid1.DataKeyField = "rotatepicid";
            DataGrid1.TableHeaderName = "聚合轮换图片列表";

			XmlDocumentExtender xmldocument = new XmlDocumentExtender();
			xmldocument.Load(configPath);
			XmlNode node = xmldocument.SelectSingleNode(targetNode);
            if (node == null || node.ChildNodes.Count == 0)
            {
                DataGrid1.Visible = SaveRotatepic.Visible = false;
                return;
            }
			XmlNodeReader rdr = new XmlNodeReader(node);
			dsSrc.ReadXml(rdr);
            dsSrc.Tables[0].Columns.Add("rowid");
            int i = 0;
            foreach (DataRow dr in dsSrc.Tables[0].Rows)
            {
                dr["rowid"] = i.ToString();
                i++;
            }
			DataGrid1.DataSource = dsSrc.Tables[0];
			DataGrid1.DataBind();
            #endregion
        }

        private void addrota_Click(Object sender, EventArgs e)
        {
            #region 插入聚合页图版轮换广告

            if ((rotaimg.Text.Trim() != "") && (url.Text.Trim() != "") && (titlecontent.Text.Trim() != ""))
            {
                if((!Utils.IsURL(rotaimg.Text.Trim()) || (!Utils.IsURL(url.Text.Trim()))))
                {
                    base.RegisterStartupScript("", "<script>alert('图片路径或点击链接可能是非法URL');</script>");
                    BindData();
                    ResetForm();
                    return;
                }
               
                XmlDocumentExtender doc = new XmlDocumentExtender();
                doc.Load(configPath);
                int lastRotatepicid = 0;
				if (doc.SelectSingleNode(targetNode) != null)
				{
					if (doc.SelectSingleNode(targetNode).InnerText != "")
					{
						lastRotatepicid = int.Parse(doc.SelectSingleNode(targetNode).LastChild["rotatepicid"].InnerText);
					}
				}
                lastRotatepicid++;

                XmlElement rotatepicNode = doc.CreateElement(nodeName);
                doc.AppendChildElementByNameValue(ref rotatepicNode, "rotatepicid", lastRotatepicid.ToString());
                doc.AppendChildElementByNameValue(ref rotatepicNode, "pagetype", "1");
                doc.AppendChildElementByNameValue(ref rotatepicNode, "img", rotaimg.Text.Trim());
                doc.AppendChildElementByNameValue(ref rotatepicNode, "url", url.Text.Trim());
                doc.AppendChildElementByNameValue(ref rotatepicNode, "titlecontent", titlecontent.Text.Trim());

                doc.CreateNode(targetNode).AppendChild(rotatepicNode);
                doc.Save(configPath);
                AggregationFacade.BaseAggregation.ClearAllDataBind();
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "添加聚合页图版轮换广告", "添加聚合页图版轮换广告,名称为: " + titlecontent.Text.Trim());

                try
                {
                    BindData();
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/ForumLinkList");
                    ResetForm();
					base.RegisterStartupScript("PAGE", "window.location.href='aggregation_rotatepic.aspx?pagename=" + DNTRequest.GetString("pagename") + "';");
                    return;
                }
                catch
                {
                    base.RegisterStartupScript("", "<script>alert('无法更新XML文件');window.location.href='aggregation_rotatepic.aspx?pagename=" + DNTRequest.GetString("pagename") + "';</script>");
                    return;
                }

            }
            else
            {
                base.RegisterStartupScript("", "<script>alert('图片或链接地址以及标题不能为空.');window.location.href='aggregation_rotatepic.aspx?pagename=" + DNTRequest.GetString("pagename") + "';</script>");
                return;
            }

            #endregion
        }
      
        private void ResetForm()
        {
            #region 重置输入框
            rotaimg.Text = "";
            url.Text = "";
            titlecontent.Text = "";
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

        private void SaveRotatepic_Click(object sender, EventArgs e)
        {
            #region 保存轮换图片修改
            int rowid = 0;
            bool error = false;

            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList __xmlnodelist = doc.SelectSingleNode(targetNode).ChildNodes;

            if ((__xmlnodelist != null) && (__xmlnodelist.Count > 0))
            {
                doc.InitializeNode(targetNode);
            }

            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                #region 轮换图片
                string rotatepicid = DataGrid1.GetControlValue(rowid, "rotatepicid");
                //string name = ((TextBox)E.Item.FindControl("rotatepicid")).Text;
                string img = DataGrid1.GetControlValue(rowid, "img");
                string url = DataGrid1.GetControlValue(rowid, "url");
                string titlecontent = DataGrid1.GetControlValue(rowid, "titlecontent").Trim();
                if (!Utils.IsNumeric(rotatepicid) || !Utils.IsURL(img) || !Utils.IsURL(url) || titlecontent == "")
                {
                    //base.RegisterStartupScript("", "<script>alert('序号、图片路径或点击链接可能是非法URL或说明文字为空');window.location.href='aggregation_rotatepic.aspx?pagename=" + DNTRequest.GetString("pagename") + "';</script>");
                    //return;
                    error = true;
                    //continue;
                    break;//如果用continue，则导致正确的设置可以被保存，有错误的设置却被删除，即使被修改之前是正确的。会丢失数据
                }
                bool insert = false;
                XmlElement rotatepicNode = doc.CreateElement(nodeName);
                doc.AppendChildElementByNameValue(ref rotatepicNode, "rotatepicid", rotatepicid);
                doc.AppendChildElementByNameValue(ref rotatepicNode, "pagetype", "1");
                doc.AppendChildElementByNameValue(ref rotatepicNode, "img", img);
                doc.AppendChildElementByNameValue(ref rotatepicNode, "url", url);
                doc.AppendChildElementByNameValue(ref rotatepicNode, "titlecontent", titlecontent);

                foreach (XmlNode __node in __xmlnodelist)
                {
                    if (int.Parse(__node["rotatepicid"].InnerText) > int.Parse(rotatepicid))
                    {
                        doc.SelectSingleNode(targetNode).InsertBefore(rotatepicNode, __node);
                        insert = true;
                        break;
                    }
                }
                if (!insert)
                {
                    doc.SelectSingleNode(targetNode).AppendChild(rotatepicNode);
                }
                rowid++;
                #endregion
            }
            AggregationFacade.BaseAggregation.ClearAllDataBind();
            if(!error)
            {
                SiteUrls.SetInstance();
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "聚合页面论坛广告编辑", "");
                doc.Save(configPath);

                base.RegisterStartupScript("PAGE", "window.location.href='aggregation_rotatepic.aspx?pagename=" + DNTRequest.GetString("pagename") + "';");
                return;
            }
            else
            {
                base.RegisterStartupScript("", "<script>alert('某行序号、图片路径或点击链接可能是非法URL或说明文字为空，不能进行更新.');window.location.href='aggregation_rotatepic.aspx?pagename=" + DNTRequest.GetString("pagename") + "';</script>");
                return;
            }
            #endregion
        }

		private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			#region 对name设置为只读属性
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
                TextBox t = (TextBox)e.Item.Cells[1].Controls[0];
				t.Attributes.Add("size", "5");
                t.Width = 30;

                t = (TextBox)e.Item.Cells[2].Controls[0];
                t.Width = 200;

                t = (TextBox)e.Item.Cells[3].Controls[0];
                t.Width = 200;

                t = (TextBox)e.Item.Cells[4].Controls[0];
                t.Width = 200;

			}
			#endregion
		}

		private void DelRec_Click(object sender, EventArgs e)
		{
			#region 删除选定的图版轮换页
			if (this.CheckCookie())
			{
				string rowidlist = DNTRequest.GetString("rowid");
				if (rowidlist != "")
				{
                    int delcount = 0;
                    XmlDocumentExtender doc = new XmlDocumentExtender();
                    doc.Load(configPath);
                    XmlNodeList __xmlnodelist = doc.SelectSingleNode(targetNode).ChildNodes;
                    foreach (string menuid in rowidlist.Split(','))
                    {
                        doc.SelectSingleNode(targetNode).RemoveChild(__xmlnodelist.Item(int.Parse(menuid) - delcount));
                        delcount++;
                    }
                    doc.Save(configPath);

					AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除选定的图版轮换页", "删除选定的图版轮换页,ID为: " + DNTRequest.GetString("id").Replace("0 ", ""));
					Response.Redirect("aggregation_rotatepic.aspx?pagename=" + DNTRequest.GetString("pagename"));
				}
				else
				{
					base.RegisterStartupScript( "","<script>alert('您未选中任何选项');window.location.href='aggregation_rotatepic.aspx?pagename=" + DNTRequest.GetString("pagename") + "';</script>");
				}
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
            this.addrota.Click += new EventHandler(this.addrota_Click);
            this.SaveRotatepic.Click += new EventHandler(this.SaveRotatepic_Click);
			this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            configPath = Server.MapPath(BaseConfigs.GetForumPath + "config/aggregation.config");
            switch (Discuz.Common.DNTRequest.GetString("pagename").ToLower())
            {
                case "website":
                    {
                        nodeName = "Website_rotatepic";
						targetNode = "/Aggregationinfo/Aggregationpage/Website/Website_rotatepiclist";
                        break;
                    }
                case "spaceindex":
                    {
                        nodeName = "Spaceindex_rotatepic";
						targetNode = "/Aggregationinfo/Aggregationpage/Spaceindex/Spaceindex_rotatepiclist";
                        break;
                    }
                case "albumindex":
                    {
                        nodeName = "Albumindex_rotatepic";
						targetNode = "/Aggregationinfo/Aggregationpage/Albumindex/Albumindex_rotatepiclist";
                        break;
                    }
                default:
                    {
                        nodeName = "Website_rotatepic";
						targetNode = "/Aggregationinfo/Aggregationpage/Website/Website_rotatepiclist";
                        break;
                    }
            }
		}
		#endregion
	}
}
