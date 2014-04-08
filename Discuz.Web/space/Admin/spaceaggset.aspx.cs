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
    /// <summary>
    /// 查找要审核的主题 
    /// </summary>
#if NET1
    public class SpaceAggset : AdminPage
#else
    public partial class SpaceAggset : AdminPage
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
        protected Discuz.Web.Admin.ajaxspaceinfo AjaxSpaceInfo1;
        protected Discuz.Control.Button SaveTopic;

        protected System.Web.UI.WebControls.Literal spacelist;
        protected System.Web.UI.HtmlControls.HtmlInputHidden spacestatus;
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
            #region 装载个人空间信息
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList data_spacelistNode = doc.SelectNodes("/Aggregationinfo/Aggregationdata/" + pagename + "aggregationdata/" + pagename + "_spacelist/Space");
            XmlNodeList index_spacelistnode = doc.SelectNodes("/Aggregationinfo/Aggregationpage/" + pagename + "/" + pagename + "_spacelist/Space");
            XmlNodeInnerTextVisitor dataspacevisitor = new XmlNodeInnerTextVisitor();
            XmlNodeInnerTextVisitor indexspacevisitor = new XmlNodeInnerTextVisitor();
            int i = 0;
            spacelist.Text = "";
            foreach (XmlNode data in data_spacelistNode)
            {
                dataspacevisitor.SetNode(data);
                bool isCheck = false;
                foreach (XmlNode index in index_spacelistnode)
                {
                    indexspacevisitor.SetNode(index);
                    if (dataspacevisitor["spaceid"].ToString() == indexspacevisitor["spaceid"].ToString())
                    {
                        isCheck = true;
                        break;
                    }
                }
                spacelist.Text += "<div class='mo' id='m" + i + "' flag='f" + i + "'><h1><input type='checkbox' name='sid' " + (isCheck ? "checked" : "") + " value='" + dataspacevisitor["spaceid"] + "'>" + dataspacevisitor["title"] + "</h1></div>\n";
                i++;
            }
            #endregion
        }

        private void SaveTopic_Click(object sender, EventArgs e)
        {
            #region 保存个人空间信息
            string sidlist = DNTRequest.GetString("spacestatus");
            if (sidlist == "")
            {
                XmlDocumentExtender doc = new XmlDocumentExtender();
                doc.Load(configPath);
                doc.RemoveNodeAndChildNode("/Aggregationinfo/Aggregationdata/" + pagename + "aggregationdata/" + pagename + "_spacelist");
                doc.RemoveNodeAndChildNode("/Aggregationinfo/Aggregationpage/" + pagename + "/" + pagename + "_spacelist");
                doc.Save(configPath);
                Response.Redirect("aggregation_spaceaggset.aspx");
                return;
            }
            else
            {
                DataTable dt = DbProvider.GetInstance().GetSpaceLitByTidlist(sidlist);

                XmlDocumentExtender doc = new XmlDocumentExtender();
                doc.Load(configPath);
                XmlNode data_spacelistnode = doc.InitializeNode("/Aggregationinfo/Aggregationdata/" + pagename + "aggregationdata/" + pagename + "_spacelist");
                XmlNode pagelistnode = doc.InitializeNode("/Aggregationinfo/Aggregationpage/" + pagename + "/" + pagename + "_spacelist");
                sidlist = DNTRequest.GetString("sid");
                foreach (DataRow dr in dt.Rows)
                {
                    XmlElement space = doc.CreateElement("Space");
                    doc.AppendChildElementByDataRow(ref space, dt.Columns, dr, "title,avatar,description");
                    doc.AppendChildElementByNameValue(ref space, "title", dr["spacetitle"].ToString().Trim());
                    doc.AppendChildElementByNameValue(ref space, "pic", dr["avatar"].ToString().Trim());
                    doc.AppendChildElementByNameValue(ref space, "description", Utils.RemoveHtml(dr["description"].ToString().Trim()), true);

                    string[] postinfo = DbProvider.GetInstance().GetSpaceLastPostInfo(int.Parse(dr["userid"].ToString()));
                    doc.AppendChildElementByNameValue(ref space, "postid", postinfo[0]);
                    doc.AppendChildElementByNameValue(ref space, "posttitle", postinfo[1]);

                    data_spacelistnode.AppendChild(space);
                    if (("," + sidlist + ",").IndexOf("," + dr["spaceid"].ToString() + ",") >= 0)
                    {
                        pagelistnode.AppendChild(space.Clone());
                   }
                }
                doc.Save(configPath);
                AggregationFacade.BaseAggregation.ClearAllDataBind();
                Response.Redirect("aggregation_spaceaggset.aspx?pagename=" + pagename);
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