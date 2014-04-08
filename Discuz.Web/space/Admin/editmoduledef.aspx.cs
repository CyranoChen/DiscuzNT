using System;
using System.Xml;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Space;
using Discuz.Space.Provider;
using Discuz.Cache;
using Discuz.Web.Admin;


namespace Discuz.Space.Admin
{

#if NET1
    public class EditModuleDef : AdminPage
#else
    public partial class EditModuleDef : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TextBox modulename;
        protected System.Web.UI.WebControls.Literal moduletype;
        protected Discuz.Control.DropDownList category;
        protected System.Web.UI.WebControls.Literal configfile;
        protected Discuz.Control.Button btnSave;
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
            string url = DNTRequest.GetString("url");

            if (url != string.Empty)
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
                XmlNodeList xnl = xml.GetElementsByTagName("Gadget");
                XmlNodeList nCategory = xml.GetElementsByTagName("Category");

                this.category.Items.Clear();
                foreach (XmlNode node in nCategory)
                {
                    this.category.Items.Add(node.Attributes["name"].Value);
                    this.category.Items[this.category.Items.Count - 1].Value = node.Attributes["name"].Value;
                }


                foreach (XmlNode node in xnl)
                {
                    if (node.Attributes["url"].Value == url)
                    {
                        this.category.SelectedValue = node.ParentNode.Attributes["name"].Value;
                        this.modulename.Text = node.Attributes["name"].Value;
                        if (url.StartsWith("builtin_"))
                        {
                            this.moduletype.Text = "内置模块";
                        }
                        else
                        {
                            this.moduletype.Text = "外置模块";
                        }
                        this.configfile.Text = url;

                        break;
                    }
                }
            }
            else
            {
                base.RegisterStartupScript( "", "<script>window.location.href='space_moduledefmanage.aspx';</script>");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.CheckCookie())
            {
                string url = DNTRequest.GetString("url");
                if (this.modulename.Text == "")
                {
                    base.RegisterStartupScript("", "<script>alert('模块名称不能为空');</script>");
                    return;
                }

                if (url != string.Empty)
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
                    XmlNodeList xnl = xml.GetElementsByTagName("Gadget");
                    XmlNodeList xnlCategory = xml.GetElementsByTagName("Category");
                    for (int i = 0; i < xnl.Count; i++)
                    {
                        if (xnl[i].Attributes["url"].Value == url)
                        {
                            xnl[i].Attributes["name"].Value = this.modulename.Text;
                            XmlNode newnode = xnl[i].Clone();
                            xnl[i].ParentNode.RemoveChild(xnl[i]);

                            foreach (XmlNode node in xnlCategory)
                            {
                                if (node.Attributes["name"].Value == this.category.SelectedValue)
                                {
                                    node.AppendChild(newnode);
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

                            break;
                        }
                    }

                    if (url.StartsWith("builtin_"))
                    { 
                        //内置模块,更改数据库中的模块名称
                        ModuleDefInfo mdi = Spaces.GetModuleDefById(Spaces.GetModuleDefIdByUrl(url));
                        mdi.ModuleName = this.modulename.Text;
                        SpaceProvider.UpdateModuleDefInfo(mdi);
                        DNTCache.GetCacheService().RemoveObject("/Space/ModuleDefList");
                    }
                }
                
                base.RegisterStartupScript( "", "<script>window.location.href='space_moduledefmanage.aspx';</script>");
            }
        }


    }
}
