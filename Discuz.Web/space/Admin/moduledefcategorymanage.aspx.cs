using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web.UI.HtmlControls;

using Discuz.Forum;
using Discuz.Common;
using Discuz.Config;
using Discuz.Web.Admin;


namespace Discuz.Space.Admin
{

#if NET1
    public class ModuleDefCategoryManage : AdminPage
#else
    public partial class ModuleDefCategoryManage : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.DataGrid DataGrid1;
        protected Discuz.Control.TextBox newcategoryname;
        protected Discuz.Control.Button btnAdd;
        #endregion
#endif

        private string listfilename = Utils.GetMapPath(BaseConfigs.GetForumPath + "space/modules/list_gadget.xml");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(listfilename);
            }
            catch
            {
                base.RegisterStartupScript("", "<script>alert('缺少对列表文件的访问权限或者列表文件不存在');window.location.href='space_moduledefmanage.aspx';</script>");
                return;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("categoryname");

            XmlNodeList xnl = xml.GetElementsByTagName("Category");

            foreach (XmlNode node in xnl)
            {
                DataRow dr = dt.NewRow();
                dr["categoryname"] = node.Attributes["name"].Value;

                dt.Rows.Add(dr);
            }
            DataGrid1.TableHeaderName = "现有分类";
            DataGrid1.DataKeyField = "categoryname";
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            DataGrid1.LoadDefaultColumn();
            DataGrid1.AllowPaging = false;
        }

        #endregion

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.CheckCookie())
            {
                string cname = this.newcategoryname.Text;
                if (cname != string.Empty)
                {
                    XmlDocument xml = new XmlDocument();
                    try
                    {
                        xml.Load(listfilename);
                    }
                    catch
                    {
                        base.RegisterStartupScript("", "<script>alert('缺少对列表文件的访问权限或者列表文件不存在');window.location.href='space_moduledefmanage.aspx';</script>");
                        return;
                    }

                    XmlNode node = xml.CreateNode(XmlNodeType.Element, "Category", "");
                    XmlAttribute xa = xml.CreateAttribute("name");
                    xa.Value = cname;

                    node.Attributes.Append(xa);
                    xml.DocumentElement.AppendChild(node);
                    try
                    {
                        xml.Save(listfilename);
                    }
                    catch
                    {
                        base.RegisterStartupScript("", "<script>alert('缺少对列表文件的访问权限或者列表文件不存在');window.location.href='space_moduledefmanage.aspx';</script>");
                        return;
                    }
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('分类名称不能为空');</script>");
                }
                base.RegisterStartupScript( "", "<script>window.location.href='space_moduledefcategorymanage.aspx';</script>");
            }
        }

        protected void DataGrid1_EditCommand(object source, DataGridCommandEventArgs e)
        {
            DataGrid1.EditItemIndex = e.Item.ItemIndex;
            BindData();
        }

        protected void DataGrid1_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            DataGrid1.EditItemIndex = -1;
            BindData();
        }

        protected void DataGrid1_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            string oldname = DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            string newname = ((System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0]).Text.Trim();

            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(listfilename);
            }
            catch
            {
                base.RegisterStartupScript("", "<script>alert('缺少对列表文件的访问权限或者列表文件不存在');window.location.href='space_moduledefmanage.aspx';</script>");
                return;
            }

            XmlNodeList xnl = xml.GetElementsByTagName("Category");

            foreach (XmlNode node in xnl)
            {
                if (node.Attributes["name"].Value == oldname)
                {
                    node.Attributes["name"].Value = newname;
                    break;
                }
            }

            try
            {
                xml.Save(listfilename);
            }
            catch
            {
                base.RegisterStartupScript("", "<script>alert('缺少对列表文件的访问权限或者列表文件不存在');window.location.href='space_moduledefmanage.aspx';</script>");
                return;
            }
            DataGrid1.EditItemIndex = -1;
            BindData();

        }

        protected void DataGrid1_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            string oldname = DataGrid1.DataKeys[e.Item.ItemIndex].ToString();

            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(listfilename);
            }
            catch
            {
                base.RegisterStartupScript("", "<script>alert('缺少对列表文件的访问权限或者列表文件不存在');window.location.href='space_moduledefmanage.aspx';</script>");
                return;
            }

            XmlNodeList xnl = xml.GetElementsByTagName("Category");

            for (int i = 0; i < xnl.Count; i++)
            {
                if (xnl[i].Attributes["name"].Value == oldname)
                {
                    if (!xnl[i].HasChildNodes)
                    {
                        xnl[i].ParentNode.RemoveChild(xnl[i]);
                    }
                    else
                    {
                        base.RegisterStartupScript( "", "<script>alert('分类下有模块时不能删除');</script>");
                    }
                    break;
                }
            }

            try
            {
                xml.Save(listfilename);
            }
            catch
            {
                base.RegisterStartupScript("", "<script>alert('缺少对列表文件的访问权限或者列表文件不存在');window.location.href='space_moduledefmanage.aspx';</script>");
                return;
            }
            DataGrid1.EditItemIndex = -1;
            BindData();
        }


    }
}
