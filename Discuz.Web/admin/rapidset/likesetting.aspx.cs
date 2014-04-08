using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;

using Discuz.Common.Xml;
using System.IO;

namespace Discuz.Web.Admin
{
    public partial class likesetting : AdminPage
    {
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected Discuz.Control.RadioButtonList showhelp;
        protected Discuz.Control.RadioButtonList showupgrade;
		protected Discuz.Control.Button saveinfo;
		protected Discuz.Control.Hint Hint1;

        protected void Page_Load(object sender, EventArgs e)
        {
            string configPath = Page.Server.MapPath("../xml/user_" + this.userid + ".config");
            if (!IsPostBack)
            {
                if (File.Exists(configPath))
                {
                    XmlDocumentExtender doc = new XmlDocumentExtender();
                    doc.Load(configPath);
                    showhelp.SelectedValue = doc.SelectSingleNode("/UserConfig/ShowInfo") != null ? doc.SelectSingleNode("/UserConfig/ShowInfo").InnerText : "1";
                    showupgrade.SelectedValue = doc.SelectSingleNode("/UserConfig/ShowUpgrade") != null ? doc.SelectSingleNode("/UserConfig/ShowUpgrade").InnerText : "1";
                }
                else
                {
                    showhelp.SelectedValue = "1";
                    showupgrade.SelectedValue = "1";
                }
            }
        }

        protected void saveinfo_Click(object sender, EventArgs e)
        {
			string configPath = Page.Server.MapPath("../xml/user_" + this.userid + ".config");
            if (File.Exists(configPath))
                File.Delete(configPath);
            XmlDocumentExtender doc = new XmlDocumentExtender();
            XmlElement userconfig = doc.CreateElement("UserConfig");
            XmlElement showinfo = doc.CreateElement("ShowInfo");
            showinfo.InnerText = showhelp.SelectedValue.ToString();
            userconfig.AppendChild(showinfo);
            doc.AppendChild(userconfig);
            XmlElement showupgradenode = doc.CreateElement("ShowUpgrade");
            showupgradenode.InnerText = showupgrade.SelectedValue.ToString();
            userconfig.AppendChild(showupgradenode);
            doc.Save(configPath);
            this.RegisterStartupScript("PAGE", "window.location='likesetting.aspx'");
        }
    }
}
