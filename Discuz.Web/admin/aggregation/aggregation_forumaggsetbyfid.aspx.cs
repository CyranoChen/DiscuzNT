using System;
using System.Data;
using System.Web.UI;
using System.Xml;
using System.Web.UI.WebControls;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Common.Xml;
using Discuz.Aggregation;
using System.IO;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 查找要审核的主题 
    /// </summary>
    public partial class forumaggsetbyfid : AdminPage
    {
        private string configPath;
        protected string fid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            fid = DNTRequest.GetString("fid");
            if (!IsPostBack)
            {
                if(File.Exists(configPath))
                    LoadWebSiteConfig();
                LoadPostTableList();
            }
        }

        private void LoadPostTableList()
        {
            #region 装载帖子列表
            DataTable dt = Posts.GetAllPostTable();
            foreach (DataRow dr in dt.Rows)
            {
                tablelist.Items.Add(new ListItem(BaseConfigs.GetTablePrefix + "posts" + dr["id"].ToString(), dr["id"].ToString()));
            }
            #endregion
        }

        /// <summary>
        /// 装载WebSite信息
        /// </summary>
       private void LoadWebSiteConfig()
       {
           #region 装载主题信息
           XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList topiclistNode = doc.SelectNodes("/Aggregationinfo/Data/Topiclist/Topic");
            XmlNodeList website_spacelistnode = doc.SelectNodes("/Aggregationinfo/Forum/Topiclist/Topic");
            XmlNodeInnerTextVisitor topicvisitor = new XmlNodeInnerTextVisitor();
            XmlNodeInnerTextVisitor pagetopicvisitor = new XmlNodeInnerTextVisitor();
            forumlist.Text = "";
            int i = 0;
            foreach (XmlNode topic in topiclistNode)
            {
                topicvisitor.SetNode(topic);
                bool isCheck = false;
                foreach (XmlNode index in website_spacelistnode)
                {
                    pagetopicvisitor.SetNode(index);
                    if (topicvisitor["topicid"].ToString() == pagetopicvisitor["topicid"].ToString())
                    {
                        isCheck = true;
                        break;
                    }
                }
                forumlist.Text += "<div class='mo' id='m" + i + "' flag='f" + i + "'><h1><input type='checkbox' name='tid' " + (isCheck ? "checked" : "") + " value='" + topicvisitor["topicid"] + "'>" + topicvisitor["title"] + "</h1></div>\n";
                i++;
            }
           #endregion
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveTopic_Click(object sender, EventArgs e)
        {
            #region 保存信息
            string tidlist = DNTRequest.GetString("forumtopicstatus");
            //当未选择主题时，则清除所有选择
            if (tidlist == "")
            {
                if (File.Exists(configPath))
                {
                    XmlDocumentExtender doc = new XmlDocumentExtender();
                    doc.Load(configPath);
                    doc.RemoveNodeAndChildNode("/Aggregationinfo/Data/Topiclist");
                    doc.RemoveNodeAndChildNode("/Aggregationinfo/Forum/Topiclist");
                    doc.Save(configPath);
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Aggregation/TopicByForumId_" + fid);
                }
                Response.Redirect("aggregation_editforumaggset.aspx?fid=" + fid);
                return;
            }
            else
            {
                PostInfo[] posts = new ForumAggregationData().GetPostListFromFile("Website");
                //得到所选择帖子信息
                Posts.WriteAggregationPostData(posts, tablelist.SelectedValue, tidlist, configPath,
                    "/Aggregationinfo/Data/Topiclist",
                    "/Aggregationinfo/Forum/Topiclist");
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Aggregation/TopicByForumId_" + fid);
                Response.Redirect("aggregation_editforumaggset.aspx?fid=" + fid);
            }
            
            #endregion
        }

        /// <summary>
        /// 保存主题显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void SaveTopicDisplay_Click(object sender, EventArgs e)
        //{
        //    #region 保存主题显示
        //    XmlDocumentExtender doc = new XmlDocumentExtender();
        //    doc.Load(configPath);
        //    //doc.RemoveNodeAndChildNode("/Aggregationinfo/Aggregationpage/Website/Forum/Bbs");
        //    doc.InitializeNode("/Aggregationinfo/Aggregationpage/Website/Forum");

        //    XmlElement BBS = doc.CreateElement("Bbs");
        //    doc.SelectSingleNode("/Aggregationinfo/Aggregationpage/Website/Forum").AppendChild(BBS);
        //    doc.Save(configPath);
        //    #endregion
        //}

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
            configPath = Server.MapPath(BaseConfigs.GetForumPath + "config/agg_" + DNTRequest.GetString("fid") + ".config");
        }

        #endregion

    }
}