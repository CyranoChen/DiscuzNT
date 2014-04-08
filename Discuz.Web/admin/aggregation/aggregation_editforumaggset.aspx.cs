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

using Discuz.Aggregation;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;
using Discuz.Common.Xml;
using System.IO;

namespace Discuz.Web.Admin
{
    public partial class editforumaggset : AdminPage
    {
        private string configPath;
        private string fid;
        protected string fidstr = "";
        protected string returnlink = "aggregation_forumaggset.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {
            fid = DNTRequest.GetString("fid");
            if (fid == "")
                configPath = Server.MapPath(BaseConfigs.GetForumPath + "config/aggregation.config");
            else
            {
                configPath = Server.MapPath(BaseConfigs.GetForumPath + "config/agg_" + fid + ".config");
                fidstr = "&fid=" + fid;
                returnlink = "aggregation_forumaggsetbyfid.aspx?fid=" + fid;
            }
            if (!IsPostBack)
            {
                DataTable dt = GetWebsiteConfig();
                websiteconfig.TableHeaderName = "帖子列表";
                websiteconfig.DataKeyField = "tid";
                websiteconfig.DataSource = dt;
                websiteconfig.DataBind();
                string tid = DNTRequest.GetString("tid");
                if(tid != "")
                    BindEditData(tid);
            }
        }

        private void BindEditData(string tid)
        {
            #region 装载主题编辑
            panel1.Visible = true;
            XmlDocumentExtender doc = new XmlDocumentExtender();
            if (!File.Exists(configPath))
                return;
            doc.Load(configPath);
            string topicPath = "/Aggregationinfo/Aggregationpage/Website/Forum/Topiclist/Topic";
            if (fid != "")
                topicPath = "/Aggregationinfo/Forum/Topiclist/Topic";
            XmlNodeList topiclistNode = doc.SelectNodes(topicPath);
            XmlNodeInnerTextVisitor topicvisitor = new XmlNodeInnerTextVisitor();
            foreach (XmlNode topic in topiclistNode)
            {
                topicvisitor.SetNode(topic);
                if (topicvisitor["topicid"] == tid)
                {
                    topicid.Value = topicvisitor["topicid"];
                    title.Text = topicvisitor["title"];
                    poster.Text = topicvisitor["poster"];
                    postdatetime.Text = topicvisitor["postdatetime"];
                    shortdescription.Text = topicvisitor["shortdescription"];
                    fulldescription.Value = topicvisitor["fulldescription"];
                }
            }
            #endregion
        }

        private DataTable GetWebsiteConfig()
        {
            #region 装载主题
            DataTable dt = new DataTable();
            dt.Columns.Add("tid");
            dt.Columns.Add("title");
            dt.Columns.Add("poster");
            dt.Columns.Add("postdatetime");
            dt.Columns.Add("showtype");
            XmlDocumentExtender doc = new XmlDocumentExtender();
            if(!File.Exists(configPath))
                return new DataTable();
            doc.Load(configPath);
            string topicPath = "/Aggregationinfo/Aggregationpage/Website/Forum/Topiclist/Topic";
            if(fid != "")
                topicPath = "/Aggregationinfo/Forum/Topiclist/Topic";
            XmlNodeList topiclistNode = doc.SelectNodes(topicPath);
            XmlNodeInnerTextVisitor topicvisitor = new XmlNodeInnerTextVisitor();
            foreach (XmlNode topic in topiclistNode)
            {
                topicvisitor.SetNode(topic);
                DataRow dr = dt.NewRow();
                dr["tid"] = topicvisitor["topicid"];
                dr["title"] = topicvisitor["title"];
                dr["poster"] = topicvisitor["poster"];
                dr["postdatetime"] = topicvisitor["postdatetime"];
                dt.Rows.Add(dr);
            }
            return dt;
            #endregion
        }

        protected void savetopic_Click(object sender, EventArgs e)
        {
            #region 保存主题修改
            XmlDocumentExtender doc = new XmlDocumentExtender();
            if (!File.Exists(configPath))
                return;
            doc.Load(configPath);
            string topicPath = "/Aggregationinfo/Aggregationpage/Website/Forum/Topiclist/Topic";
            string dataTopicPath = "/Aggregationinfo/Aggregationdata/Websiteaggregationdata/Topiclist/Topic";
            if (fid != "")
            {
                topicPath = "/Aggregationinfo/Forum/Topiclist/Topic";
                dataTopicPath = "/Aggregationinfo/Data/Topiclist/Topic";
            }
            ModifyNode(doc, topicPath);
            ModifyNode(doc, dataTopicPath);
            doc.Save(configPath);
            if (fid == "")
            {
                AggregationFacade.BaseAggregation.ClearAllDataBind();
                Response.Redirect("aggregation_editforumaggset.aspx");
            }
            else
            {
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Aggregation/TopicByForumId_" + fid);
                Response.Redirect("aggregation_editforumaggset.aspx?fid=" + fid);
            }
            #endregion
        }

        private void ModifyNode(XmlDocumentExtender doc,string topicPath)
        {
            XmlNodeList topiclistNode = doc.SelectNodes(topicPath);
            XmlNodeInnerTextVisitor topicvisitor = new XmlNodeInnerTextVisitor();
            foreach (XmlNode topic in topiclistNode)
            {
                topicvisitor.SetNode(topic);
                if (topicvisitor["topicid"] == topicid.Value)
                {
                    topicvisitor["topicid"] = topicid.Value;
                    topicvisitor["title"] = title.Text;
                    topicvisitor["poster"] = poster.Text;
                    topicvisitor["postdatetime"] = postdatetime.Text;
                    XmlCDataSection shortDes = doc.CreateCDataSection("shortdescription");
                    shortDes.InnerText = shortdescription.Text;
                    topicvisitor.GetNode("shortdescription").RemoveAll();
                    topicvisitor.GetNode("shortdescription").AppendChild(shortDes);
                    break;
                }
            }
        }

        protected string GetEditLink(string tid)
        {
            return "?tid=" + tid + fidstr;
        }
    }
}
