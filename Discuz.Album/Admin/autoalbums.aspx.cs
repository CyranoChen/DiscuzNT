using System;
using System.Data;
using System.Xml;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;
using Discuz.Aggregation;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Common.Xml;
using Discuz.Web.Admin;


namespace Discuz.Album.Admin
{
    /// <summary>
    /// 查找要审核的主题 
    /// </summary>
#if NET1
    public class AutoAlbums : AdminPage
#else
    public partial class AutoAlbums : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected System.Web.UI.HtmlControls.HtmlForm Form1;
        protected System.Web.UI.HtmlControls.HtmlInputHidden recommendphoto;
        protected System.Web.UI.HtmlControls.HtmlInputHidden recommendalbum;

        protected Discuz.Control.RadioButtonList focusphotoshowtype;
        protected Discuz.Control.TextBox focusphotodays;
        protected Discuz.Control.TextBox focusphotocount;
        protected Discuz.Control.RadioButtonList recommendalbumtype;
        protected Discuz.Control.TextBox focusalbumdays;
        protected Discuz.Control.TextBox focusalbumcount;
        protected Discuz.Control.TextBox weekhot;
        protected Discuz.Control.Button savetopic;
        #endregion
#endif

        private string configPath;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadWebSiteConfig();
            }
        }

        private void LoadWebSiteConfig()
        {
            #region 绑定自动推荐相册
            //装载配置文件
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNode albumconfig = doc.SelectSingleNode("/Aggregationinfo/Aggregationpage/Albumindex/Albumconfig");
            focusphotoshowtype.SelectedIndex =  Convert.ToInt32(doc.GetSingleNodeValue(albumconfig, "Focusphotoshowtype"));
            focusphotodays.Text = doc.GetSingleNodeValue(albumconfig, "Focusphotodays");
            focusphotocount.Text = doc.GetSingleNodeValue(albumconfig, "Focusphotocount");
            recommendalbumtype.SelectedIndex = Convert.ToInt32(doc.GetSingleNodeValue(albumconfig, "Focusalbumshowtype"));
            focusalbumdays.Text = doc.GetSingleNodeValue(albumconfig, "Focusalbumdays");
            focusalbumcount.Text = doc.GetSingleNodeValue(albumconfig, "Focusalbumcount");
            weekhot.Text = doc.GetSingleNodeValue(albumconfig, "Weekhot");
            #endregion
        }

        private void savetopic_Click(object sender, EventArgs e)
        {
            #region 绑定自动推荐相册修改
            //验证值是否正确
            if (!ValidateValue(focusphotodays.Text)) return;
            if (!ValidateValue(focusphotocount.Text)) return;
            if (!ValidateValue(focusalbumdays.Text)) return;
            if (!ValidateValue(focusalbumcount.Text)) return;
            if (!ValidateValue(weekhot.Text)) return;
            string strfocusphotoshowtype = focusphotoshowtype.SelectedIndex.ToString();
            string strfocusphotodays = focusphotodays.Text;
            string strfocusphotocount = focusphotocount.Text;
            string strfocusalbumshowtype = recommendalbumtype.SelectedIndex.ToString();
            string strfocusalbumdays = focusalbumdays.Text;
            string strfocusalbumcount = focusalbumcount.Text;
            string strweekhot = weekhot.Text;
            //保存信息
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            doc.RemoveNodeAndChildNode("/Aggregationinfo/Aggregationpage/Albumindex/Albumconfig");
            XmlNode albumconfig = doc.InitializeNode("/Aggregationinfo/Aggregationpage/Albumindex/Albumconfig");
            XmlElement node = doc.CreateElement("Focusphotoshowtype");
            node.InnerText = strfocusphotoshowtype;
            albumconfig.AppendChild(node);
            node = doc.CreateElement("Focusphotodays");
            node.InnerText = strfocusphotodays;
            albumconfig.AppendChild(node);
            node = doc.CreateElement("Focusphotocount");
            node.InnerText = strfocusphotocount;
            albumconfig.AppendChild(node);
            node = doc.CreateElement("Focusalbumshowtype");
            node.InnerText = strfocusalbumshowtype;
            albumconfig.AppendChild(node);
            node = doc.CreateElement("Focusalbumdays");
            node.InnerText = strfocusalbumdays;
            albumconfig.AppendChild(node);
            node = doc.CreateElement("Focusalbumcount");
            node.InnerText = strfocusalbumcount;
            albumconfig.AppendChild(node);
            node = doc.CreateElement("Weekhot");
            node.InnerText = strweekhot;
            albumconfig.AppendChild(node);
            doc.Save(configPath);
            AggregationFacade.BaseAggregation.ClearAllDataBind();
            Response.Redirect("aggregation_autoalbums.aspx");
            #endregion
        }
        /// <summary>
        /// 验证值是否正确
        /// </summary>
        /// <param name="val">要验证的值</param>
        /// <returns>正确否</returns>
        private bool ValidateValue(string val)
        {
            #region 验证值是否正确
            if (Utils.IsNumeric(val))
            {
                if (Convert.ToInt32(val) < 0)
                {
                    base.RegisterStartupScript("PAGE", "alert('页面中各项配置输入必须为正整数');window.location.href='aggregation_autoalbums.aspx';");
                    return false;
                }
                else
                    return true;
            }
            else
            {
                base.RegisterStartupScript("PAGE", "alert('页面中各项配置输入必须为数字');window.location.href='aggregation_autoalbums.aspx';");
                return false;
            }
            #endregion
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.savetopic.Click += new EventHandler(this.savetopic_Click);
            this.savetopic.ValidateForm = true;

            configPath = Server.MapPath(BaseConfigs.GetForumPath + "config/aggregation.config");
        }

        #endregion

    }
}