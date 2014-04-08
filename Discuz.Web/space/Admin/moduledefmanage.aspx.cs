using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;

using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Space.Provider;
using Discuz.Space;
using Discuz.Data;
using Discuz.Space.Entities;
using Discuz.Cache;
using Discuz.Web.Admin;


namespace Discuz.Space.Admin
{

#if NET1
    public class ModuleDefManage : AdminPage
#else
    public partial class ModuleDefManage : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.Button DeleteApply;
        protected Discuz.Control.DataGrid DataGrid1;
        protected Discuz.Control.Button AddApply;
        protected Discuz.Control.DataGrid DataGrid2;
        #endregion
#endif

        private string listfilename = Utils.GetMapPath(BaseConfigs.GetForumPath + "space/modules/list_gadget.xml");
        private string moduledefpath = Utils.GetMapPath(BaseConfigs.GetForumPath + "space/modules");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (DNTRequest.GetString("mod") == "refresh")
            {
                RefreshModuledefInfo(DNTRequest.GetString("url"));
            }
            if (!IsPostBack)
            {
                BindGridData();
            }
        }

        private void RefreshModuledefInfo(string moduleName)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(moduledefpath + "\\" + moduleName);
            XmlNode modulePrefs = xml.SelectSingleNode("/Module/ModulePrefs");
            string controller = modulePrefs.Attributes["controller"].Value;
            Space.Data.DbProvider.GetInstance().UpdateModuleDefInfo(moduleName, controller);
        }

        private void BindGridData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("moduledefname");
            dt.Columns.Add("moduledefcatelog");
            dt.Columns.Add("moduledeftype");
            dt.Columns.Add("hadindb");
            dt.Columns.Add("url");
            dt.Columns.Add("modulestatus");
            dt.Columns.Add("moduledefop");
            DNTCache.GetCacheService().RemoveObject("/Space/ModuleDefList");
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

            XmlNodeList xnl = xml.GetElementsByTagName("Gadget");

            //枚举xml文件列表
            string[] files = Directory.GetFiles(moduledefpath, "*.xml");
            foreach (string file in files)
            {
                DataRow dr = dt.NewRow();
                string moduleurl = Path.GetFileName(file);
                XmlNode node = HadInList(moduleurl, xnl);
                dr["url"] = moduleurl;
                //<a href="space_editmoduledef.aspx?url=<%#DataBinder.Eval(Container, "DataItem.url")%>">编辑</a>
				//				<a runat="server" id="enablemoduledef" href="###" onclick="document.getElementById('url<%#DataBinder.Eval(Container, "DataItem.url").ToString()%>').checked=true;submitDeleteApply();">置为无效</a>
							
                if (node != null)
                { //已经在列表使用中的模块
                    dr["moduledefop"] = string.Format("<a href=\"space_editmoduledef.aspx?url={0}\">编辑</a>&nbsp;<a href=\"#\" onclick=\"document.getElementById('url{0}').checked=true;submitDeleteApply();\">置为无效</a>", moduleurl);
                    if (moduleurl.StartsWith("builtin_"))
                    { //内置模块
                        ModuleDefInfo mdi = SpaceProvider.GetModuleDefInfoByUrl(moduleurl);
                        try
                        {
                            Activator.CreateInstance(Type.GetType(mdi.BussinessController,false, true));
                            dr["modulestatus"] = "<img title='模块可以正常工作' alt='模块可以正常工作' src='../images/state2.gif' />";
                        }
                        catch
                        { //本模块逻辑控制反射失败
                            dr["modulestatus"] = "<img title='模块当前无法正常工作' alt='模块当前无法正常工作' src='../images/state1.gif' />";
                            dr["moduledefop"] = string.Format("<a href=\"?mod=refresh&url={0}\">刷新</a>&nbsp;<a href=\"#\" onclick=\"document.getElementById('url{0}').checked=true;submitDeleteApply();\">置为无效</a>", moduleurl);
                        }
                        dr["moduledeftype"] = "内置模块";
                    }
                    else
                    { //外置模块
                        dr["modulestatus"] = "<img title='模块可以正常工作' alt='模块可以正常工作' src='../images/state2.gif' />";
                        dr["moduledeftype"] = "外置模块";
                    }

                    dr["moduledefname"] = string.Format("{0}({1})", node.Attributes["name"].Value, node.Attributes["url"].Value);
                    dr["moduledefcatelog"] = node.ParentNode.Attributes["name"].Value;
                    
                    dt.Rows.Add(dr);
                }
                else
                { //未被使用的模块 
                    dr["modulestatus"] = "<img title='尚未启用的模块' alt='尚未启用的模块' src='../images/state3.gif' />";
                    ModulePref mp = ModuleXmlHelper.LoadModulePref(file);
                    if (moduleurl.StartsWith("builtin_"))
                    {
                        dr["moduledeftype"] = "内置模块";
                    }
                    else
                    {
                        dr["moduledeftype"] = "外置模块";
                    }
                    if (mp != null)
                    {
                        dr["moduledefname"] = string.Format("{0}({1})", mp.Title.StartsWith("_") ? moduleurl : mp.Title, moduleurl);
                        dr["moduledefcatelog"] = mp.Category == string.Empty ? "其他" : mp.Category;
                        dr["moduledefop"] = string.Format("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a runat=\"server\" href=\"###\" onclick=\"document.getElementById('url{0}').checked=true;submitAddApply();\">置为有效</a>", moduleurl);
                        dt.Rows.Add(dr);
                    }
                }
                
            }

            this.DataGrid1.TableHeaderName = "现有模块";
            this.DataGrid1.AllowPaging = false;
            this.DataGrid1.AllowCustomPaging = false;
            this.DataGrid1.DataKeyField = "url";
            this.DataGrid1.DataSource = dt;
            this.DataGrid1.DataBind();
        }

        private XmlNode HadInList(string url, XmlNodeList xnl)
        {
            foreach (XmlNode node in xnl)
            {                
                if (url == node.Attributes["url"].Value)
                    return node;
            }

            return null;
        }

        protected void AddApply_Click(object sender, EventArgs e)
        {
            if (this.CheckCookie())
            {
                string[] urllist = DNTRequest.GetString("url").Split(',');
                bool hasemptyvalue = false;
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
                XmlNodeList xnlGadgets = xml.GetElementsByTagName("Gadget");
                BindGridData();
                foreach (string url in urllist)
                {
                    foreach (DataGridItem dgi in DataGrid1.Items)
                    {
                        if (dgi.ItemType == ListItemType.AlternatingItem || dgi.ItemType == ListItemType.Item)
                        {
                            if (url != DataGrid1.DataKeys[dgi.ItemIndex].ToString() || HadInList(url, xnlGadgets) != null)
                            {
                                continue;
                            }

                            string category = DataGrid1.Items[dgi.ItemIndex].Cells[3].Text;
                            string modulename = DataGrid1.Items[dgi.ItemIndex].Cells[2].Text.Substring(0, DataGrid1.Items[dgi.ItemIndex].Cells[2].Text.LastIndexOf("(")); ;//DataGrid1.GetControlValue(dgi.ItemIndex, "moduledefname");

                            if (category == string.Empty || modulename == string.Empty)
                            {
                                hasemptyvalue = true;
                                continue;
                            }

                            //分类节点如果不存在则创建
                            XmlNodeList xnl = xml.GetElementsByTagName("Category");
                            XmlNode categorynode = GetCategoryNode(category, xnl);

                            if (categorynode == null)
                            {
                                categorynode = xml.CreateNode(XmlNodeType.Element, "Category", "");
                                XmlAttribute xa = xml.CreateAttribute("name");
                                xa.Value = category;
                                categorynode.Attributes.Append(xa);
                                xml.DocumentElement.AppendChild(categorynode);
                            }
                            //添加至列表文件
                            XmlNode node = xml.CreateNode(XmlNodeType.Element, "Gadget", "");
                            XmlAttribute xanode = xml.CreateAttribute("name");
                            xanode.Value = modulename;
                            node.Attributes.Append(xanode);
                            xanode = xml.CreateAttribute("url");
                            xanode.Value = url;
                            node.Attributes.Append(xanode);

                            categorynode.AppendChild(node);
                            try
                            {
                                xml.Save(listfilename);
                            }
                            catch
                            {
                                base.RegisterStartupScript("", "<script>alert('缺少对列表文件的访问权限或者列表文件不存在');window.location.href='space_moduledefmanage.aspx';</script>");
                                return;
                            }
                            if (url.StartsWith("builtin_"))
                            { //内置模块添加至数据库                                
                                ModulePref mp = ModuleXmlHelper.LoadModulePref(Utils.GetMapPath(BaseConfigs.GetForumPath + "space/modules/" + url));
                                if (mp != null)
                                {
                                    ModuleDefInfo mdi = new ModuleDefInfo();
                                    mdi.ModuleName = modulename;
                                    mdi.CacheTime = 0;
                                    mdi.BussinessController = mp.Controller;
                                    mdi.ConfigFile = url;
                                    SpaceProvider.AddModuleDefInfo(mdi);
                                }                                
                            }    
                        }
                    }
                }
                if (hasemptyvalue)
                {
                    base.RegisterStartupScript( "", "<script>alert('模块名称或分类名称为空的模块未被添加');window.location.href='space_moduledefmanage.aspx';</script>");
                }

                base.RegisterStartupScript( "", "<script>window.location.href='space_moduledefmanage.aspx';</script>");
                BindGridData();
            }
        }

        private XmlNode GetCategoryNode(string category, XmlNodeList xnl)
        {
            foreach (XmlNode node in xnl)
            {
                if (node.Attributes["name"].Value == category)
                    return node;
            }
            return null;
        }

        protected void DeleteApply_Click(object sender, EventArgs e)
        {
            if (this.CheckCookie())
            {
                string[] urllist = DNTRequest.GetString("url").Split(',');
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
                foreach (string url in urllist)
                {
                    XmlNodeList xnl = xml.GetElementsByTagName("Gadget");

                    for (int i = 0; i < xnl.Count; i++)
                    {
                        if (url == xnl[i].Attributes["url"].Value)
                        {
                            xnl[i].ParentNode.RemoveChild(xnl[i]);
                            if (url.StartsWith("builtin_"))
                            {
                                Spaces.DeleteModuleDefByUrl(url);
                            }
                        }
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

                base.RegisterStartupScript( "", "<script>window.location.href='space_moduledefmanage.aspx';</script>");
            }
        }
    }
}
