using System;
using System.Data;
using System.Data.Common;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Xml;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common.Xml;
using Discuz.Cache;
using Discuz.Common.Generic;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 附件分类列表
    /// </summary>
    public partial class forum_attchemnttypes : AdminPage
    {
        private DataTable att = new DataTable();
        private DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            att.Columns.Add("typeid");
            att.Columns.Add("typename");
            att.Columns.Add("extname");
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(Server.MapPath(BaseConfigs.GetForumPath + "config/myattachment.config"));
            XmlNodeList attachtype = doc.SelectNodes("/MyAttachmentsTypeConfigInfo/attachtypes/AttachmentType");
            foreach (XmlNode node in attachtype)
            {
                DataRow dr = att.NewRow();
                dr["typeid"] = node["TypeId"].InnerText;
                dr["typename"] = node["TypeName"].InnerText;
                dr["extname"] = node["ExtName"].InnerText != "" ? node["ExtName"].InnerText : "无绑定类型";
                att.Rows.Add(dr);
            }
            dt = Attachments.GetAttachmentType();
            string typeid = DNTRequest.GetString("typeid");

            if (!Page.IsPostBack)
            {
                BindData();

                string attlist = "";    //得到已绑定的附件列表
                if (att != null)
                {
                    foreach (DataRow dr in att.Rows)
                    {
                        attlist += dr["extname"].ToString() + ",";
                    }
                    attlist = attlist.TrimEnd(',');
                }
                attachextensions.AddTableData(dt);
                foreach (string atttype in attlist.Split(','))
                {
                    for (int i = 0; i < attachextensions.Items.Count; i++)
                    {
                        if (atttype == attachextensions.Items[i].Text)
                        {
                            attachextensions.Items[i].Enabled = false;
                            break;
                        }
                    }
                }
                string script = "var atttype = \r\n{";
                if (att != null)
                {
                    foreach (DataRow dr in att.Rows)
                    {
                        script += "\r\n\ttype" + dr["typeid"].ToString() + ":{typename:'" + dr["typename"].ToString() + "',extname:'" + dr["extname"].ToString() + "'},";
                    }
                    script = script.TrimEnd(',');
                }
                script += "\r\n};";
                base.RegisterStartupScript("", "<script type='text/javascript'>\r\n" + script + "\r\n</script>");
            }
        }

        public void BindData()
        {
            #region 绑定附件分类列表
            
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "附件分类列表";
            if (att == null)
                return;
            DataGrid1.DataSource = att;
            DataGrid1.DataKeyField = "TypeId";
            DataGrid1.DataBind();
            #endregion
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 数据绑定显示长度控制

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                TextBox t = (TextBox)e.Item.Cells[4].Controls[0];
                t.Attributes.Add("maxlength", "254");
                t.Attributes.Add("size", "30");

                t = (TextBox)e.Item.Cells[5].Controls[0];
                t.Attributes.Add("maxlength", "254");
                t.Attributes.Add("size", "30");
            }

            #endregion
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            #region 删除附件分类

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("typeid") != "")
                {
                    string idlist = DNTRequest.GetString("typeid");

                    XmlDocumentExtender doc = new XmlDocumentExtender();
                    doc.Load(Server.MapPath(BaseConfigs.GetForumPath + "config/myattachment.config"));
                    XmlNodeList xnl = doc.SelectNodes("/MyAttachmentsTypeConfigInfo/attachtypes/AttachmentType");
                    foreach (string id in idlist.Split(','))
                    {
                        foreach (XmlNode xn in xnl)
                        {
                            if (id == xn["TypeId"].InnerText)
                            {
                                xn.ParentNode.RemoveChild(xn);
                                break;
                            }
                        }
                    }
                    doc.Save(Server.MapPath(BaseConfigs.GetForumPath + "config/myattachment.config"));
                    UpdateAttchmentTypes();
                    Response.Redirect("forum_attchemnttypes.aspx");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='forum_attchemnttypes.aspx';</script>");
                }
            }

            #endregion
        }

        private int GetMaxTypeid()
        {
            if (att == null || att.Rows.Count == 0)
                return 0;
            return int.Parse(att.Rows[att.Rows.Count - 1]["typeid"].ToString());
        }

        private string GetAttTypeList()
        {
            string typelist = "";
            for (int i = 0; i < attachextensions.Items.Count; i++)
            {
                typelist += DNTRequest.GetString("attachextensions:" + i) == "on" ? attachextensions.Items[i].Text + "," : "";
            }
            return typelist.TrimEnd(',');
        }

        private void AddNewRec_Click(object sender, EventArgs e)
        {
            #region 添加附件分类
            if (typename.Text == "")
            {
                base.RegisterStartupScript("", "<script>alert('附件分类名称不能为空!');window.location.href='forum_attchemnttypes.aspx';</script>");
                return;
            }

            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(Server.MapPath(BaseConfigs.GetForumPath + "config/myattachment.config"));
            if (DNTRequest.GetString("atttypeid") == "")   //增加附件分类
            {
                XmlNode attachtypes = doc.SelectSingleNode("/MyAttachmentsTypeConfigInfo/attachtypes");
                XmlElement attachtype = doc.CreateElement("AttachmentType");
                XmlElement node = doc.CreateElement("TypeId");
                int maxTypeid = GetMaxTypeid();
                node.InnerText = (++maxTypeid).ToString();
                attachtype.AppendChild(node);
                node = doc.CreateElement("TypeName");
                node.InnerText = typename.Text;
                attachtype.AppendChild(node);
                node = doc.CreateElement("ExtName");
                node.InnerText = GetAttTypeList();
                attachtype.AppendChild(node);
                attachtypes.AppendChild(attachtype);
            }
            else
            {
                XmlNodeList xnl = doc.SelectNodes("/MyAttachmentsTypeConfigInfo/attachtypes/AttachmentType");
                foreach (XmlNode xn in xnl)
                {
                    if (xn["TypeId"].InnerText == DNTRequest.GetString("atttypeid"))
                    {
                        xn["TypeName"].InnerText = typename.Text;
                        xn["ExtName"].InnerText = GetAttTypeList();
                    }
                }
            }
            doc.Save(Server.MapPath(BaseConfigs.GetForumPath + "config/myattachment.config"));
            UpdateAttchmentTypes();
            base.RegisterStartupScript("", "<script>window.location.href='forum_attchemnttypes.aspx';</script>");
            #endregion
        }

        private void UpdateAttchmentTypes()
        {
            #region 更新附件类型缓存
            DNTCache.GetCacheService().RemoveObject("/Forum/MyAttachments");
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
            this.AddNewRec.Click += new EventHandler(this.AddNewRec_Click);
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            DataGrid1.ColumnSpan = 5;
        }

        #endregion

    }
}