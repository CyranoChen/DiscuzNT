using System;
using System.Data;
using System.Web.UI;
using System.Xml;
using System.Web.UI.WebControls;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Common.Xml;

using Discuz.Aggregation;
using Discuz.Web.Admin;
using Discuz.Space.Data;

namespace Discuz.Space.Admin
{
#if NET1
    public class PostAggset : AdminPage
#else
    public partial class PostAggset : AdminPage
#endif
    {

        /// <summary>
        /// 要写入aggregation.config数据文件中的节点名称
        /// </summary>
        private string pagename = "";

#if NET1
        #region 控件声明
        protected Discuz.Control.TextBox poster;
        protected Discuz.Control.TextBox title;
        protected Discuz.Control.Calendar postdatetimeStart;
        protected Discuz.Control.Calendar postdatetimeEnd;
        protected Discuz.Control.Button SearchTopicAudit;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Web.Admin.ajaxspacepostinfo AjaxSpaceInfo1;
        protected Discuz.Control.Button SaveTopic;
        protected System.Web.UI.WebControls.Literal postlist;
        protected System.Web.UI.HtmlControls.HtmlInputHidden poststatus;
        #endregion
#endif

        private string configPath;
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (Discuz.Common.DNTRequest.GetString("pagename").ToLower())
            {
                case "website":
                    {
                        pagename = "Website";
                        break;
                    }
                case "spaceindex":
                    {
                        pagename = "Spaceindex";
                        break;
                    }
                default:
                    {
                        pagename = "Website";
                        break;
                    }
            }

            if (!IsPostBack)
            {
                LoadWebSiteConfig();
            }
        }

        private void LoadWebSiteConfig()
        {
            #region 装载日志信息
            XmlDocument doc = new XmlDocument();
            doc.Load(configPath);
            XmlNodeList postlistNode = doc.SelectNodes("/Aggregationinfo/Aggregationdata/" + pagename + "aggregationdata/" + pagename + "_spacearticlelist/Article");
            XmlNodeList index_spacelistnode = doc.SelectNodes("/Aggregationinfo/Aggregationpage/" + pagename + "/" + pagename + "_spacearticlelist/Article");
            XmlNodeInnerTextVisitor data_postvisitor = new XmlNodeInnerTextVisitor();
            XmlNodeInnerTextVisitor index_postvisitor = new XmlNodeInnerTextVisitor();
            postlist.Text = "";
            int i = 0;
            foreach (XmlNode post in postlistNode)
            {
                data_postvisitor.SetNode(post);
                bool isCheck = false;
                foreach (XmlNode index in index_spacelistnode)
                {
                    index_postvisitor.SetNode(index);
                    if (data_postvisitor["postid"].ToString() == index_postvisitor["postid"].ToString())
                    {
                        isCheck = true;
                        break;
                    }
                }
                postlist.Text += "<div class='mo' id='m" + i + "' flag='f" + i + "'><h1><input type='checkbox' name='pid' " + (isCheck ? "checked" : "") + " value='" + data_postvisitor["postid"] + "'>" + data_postvisitor["title"] + "</h1></div>\n";
                i++;
            }
            #endregion
        }

        private void SaveTopic_Click(object sender, EventArgs e)
        {
            #region 装载修改信息
            string pid = DNTRequest.GetString("pid");
            string pidlist = Utils.ClearLastChar(poststatus.Value);
            if (pidlist == "")
            {
                XmlDocumentExtender doc = new XmlDocumentExtender();
                doc.Load(configPath);
                doc.RemoveNodeAndChildNode("/Aggregationinfo/Aggregationdata/" + pagename + "aggregationdata/" + pagename + "_spacearticlelist");
                doc.RemoveNodeAndChildNode("/Aggregationinfo/Aggregationpage/" + pagename + "/" + pagename + "_spacearticlelist");
                doc.Save(configPath);
                Response.Redirect("aggregation_postaggset.aspx");
                return;
            }
            else
            {
                DataTable dt = DbProvider.GetInstance().GetSpacepostLitByTidlist(pidlist);
                XmlDocumentExtender doc = new XmlDocumentExtender();
                doc.Load(configPath);
                XmlNode data_spacearticlelistnode = doc.InitializeNode("/Aggregationinfo/Aggregationdata/" + pagename + "aggregationdata/" + pagename + "_spacearticlelist");
                XmlNode pagearticlelistnode = doc.InitializeNode("/Aggregationinfo/Aggregationpage/" + pagename + "/" + pagename + "_spacearticlelist");
                foreach (DataRow dr in dt.Rows)
                {
                    XmlElement article = doc.CreateElement("Article");
                    doc.AppendChildElementByDataRow(ref article, dt.Columns, dr);
                    data_spacearticlelistnode.AppendChild(article);
                    if (("," + pid + ",").IndexOf("," + dr["postid"].ToString() + ",") >= 0)
                    {
                        pagearticlelistnode.AppendChild(article.Clone());
                    }
                }
                doc.Save(configPath);
                AggregationFacade.BaseAggregation.ClearAllDataBind();
                Response.Redirect("aggregation_postaggset.aspx?pagename=" + pagename);
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
            this.SaveTopic.Click += new EventHandler(this.SaveTopic_Click);
            this.SaveTopic.ValidateForm = true;
            configPath = Server.MapPath(BaseConfigs.GetForumPath + "config/aggregation.config");
        }

        #endregion

    }
}