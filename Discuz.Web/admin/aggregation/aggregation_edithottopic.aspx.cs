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

using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;
using Discuz.Common.Xml;
using System.IO;

namespace Discuz.Web.Admin
{
    public partial class edithottopic : AdminPage
    {
        private string configPath;
        //private string fid;
        protected string fidstr = "";
        protected string returnlink = "aggregation_edithottopic.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {
            configPath = Server.MapPath(BaseConfigs.GetForumPath + "config/agg_hottopics.config");
            if (!IsPostBack)
            {
                DataTable dt = GetWebsiteConfig();
                websiteconfig.TableHeaderName = "热帖列表";
                websiteconfig.DataKeyField = "tid";
                websiteconfig.DataSource = dt;
                websiteconfig.DataBind();
                string tid = DNTRequest.GetString("tid");
                if (tid != "")
                    BindEditData(tid);
            }
        }

        private void BindEditData(string tid)
        {
            #region 装载主题编辑
            panel1.Visible = true;
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList topiclistNode = doc.SelectNodes("/Aggregationinfo/Forum/Hottopiclist/Topic");
            XmlNodeInnerTextVisitor topicvisitor = new XmlNodeInnerTextVisitor();
            foreach (XmlNode topic in topiclistNode)
            {
                topicvisitor.SetNode(topic);
                if (topicvisitor["tid"] == tid)
                {
                    topicid.Value = topicvisitor["tid"];
                    title.Text = topicvisitor["title"];
                    poster.Text = topicvisitor["poster"];
                    postdatetime.Text = topicvisitor["postdatetime"];
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
            if (!File.Exists(configPath))
                return new DataTable();
            doc.Load(configPath);
            XmlNodeList website_spacelistnode = doc.SelectNodes("/Aggregationinfo/Forum/Hottopiclist/Topic");

            XmlNodeInnerTextVisitor topicvisitor = new XmlNodeInnerTextVisitor();
            foreach (XmlNode topic in website_spacelistnode)
            {
                topicvisitor.SetNode(topic);
                DataRow dr = dt.NewRow();
                dr["tid"] = topicvisitor["tid"];
                dr["title"] = topicvisitor["title"];
                dr["poster"] = topicvisitor["poster"];
                dr["postdatetime"] = topicvisitor["postdatetime"];
                dt.Rows.Add(dr);
            }
            return dt;
            #endregion
        }

        /// <summary>
        /// 装载WebSite信息
        /// </summary>
        //private void LoadWebSiteConfig()
        //{
        //    #region 装载主题信息
        //    XmlDocumentExtender doc = new XmlDocumentExtender();
        //    doc.Load(configPath);
        //    XmlNodeList topiclistNode = doc.SelectNodes("/Aggregationinfo/Data/Hottopiclist/Topic");
        //    XmlNodeList website_spacelistnode = doc.SelectNodes("/Aggregationinfo/Forum/Hottopiclist/Topic");
        //    XmlNodeInnerTextVisitor topicvisitor = new XmlNodeInnerTextVisitor();
        //    XmlNodeInnerTextVisitor pagetopicvisitor = new XmlNodeInnerTextVisitor();
        //    forumlist.Text = "";
        //    int i = 0;
        //    foreach (XmlNode topic in topiclistNode)
        //    {
        //        topicvisitor.SetNode(topic);
        //        bool isCheck = false;
        //        foreach (XmlNode index in website_spacelistnode)
        //        {
        //            pagetopicvisitor.SetNode(index);
        //            if (topicvisitor["tid"].ToString() == pagetopicvisitor["tid"].ToString())
        //            {
        //                isCheck = true;
        //                break;
        //            }
        //        }
        //        //forumlist.Text += "<div class='mo' id='m" + i + "' flag='f" + i + "'><h1><input type='checkbox' name='tid' " + (isCheck ? "checked" : "") + " value='" + topicvisitor["tid"] + "'>" + topicvisitor["title"] + "</h1></div>\n";
        //        i++;
        //    }
        //    #endregion
        //}

        protected void savetopic_Click(object sender, EventArgs e)
        {
            #region 保存主题修改
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList topiclistNode = doc.SelectNodes("/Aggregationinfo/Forum/Hottopiclist/Topic");
            XmlNodeInnerTextVisitor topicvisitor = new XmlNodeInnerTextVisitor();
            foreach (XmlNode topic in topiclistNode)
            {
                topicvisitor.SetNode(topic);
                if (topicvisitor["tid"] == topicid.Value)
                {
                    topicvisitor["tid"] = topicid.Value;
                    topicvisitor["title"] = title.Text;
                    break;
                }
            }
            topiclistNode = doc.SelectNodes("/Aggregationinfo/Data/Hottopiclist/Topic");
            foreach (XmlNode topic in topiclistNode)
            {
                topicvisitor.SetNode(topic);
                if (topicvisitor["tid"] == topicid.Value)
                {
                    topicvisitor["tid"] = topicid.Value;
                    topicvisitor["title"] = title.Text;
                    break;
                }
            }
            doc.Save(configPath);
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Aggregation/Hottopiclist");
            Response.Redirect("aggregation_edithottopic.aspx");
            #endregion
        }
    }
}
