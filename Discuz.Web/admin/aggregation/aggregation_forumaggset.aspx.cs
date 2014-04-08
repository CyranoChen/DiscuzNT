using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;

using Discuz.Aggregation;
using Discuz.Common;
using Discuz.Common.Xml;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ����Ҫ��˵����� 
    /// </summary>
    public partial class forumaggset : AdminPage
    {
        private string configPath;
        protected void Page_Load(object sender, EventArgs e)
        {
            forumid.BuildTree(Forums.GetOpenForumList(), "name", "fid");
            if (!IsPostBack)
            {
                LoadWebSiteConfig();
                LoadPostTableList();
            }
        }

        private void LoadPostTableList()
        {
            #region װ�������б�
            DataTable dt = Posts.GetAllPostTable();
            foreach (DataRow dr in dt.Rows)
            {
                tablelist.Items.Add(new ListItem(BaseConfigs.GetTablePrefix + "posts" + dr["id"].ToString(), dr["id"].ToString()));
            }
            #endregion
        }

        /// <summary>
        /// װ��WebSite��Ϣ
        /// </summary>
        private void LoadWebSiteConfig()
        {
            #region װ��������Ϣ
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList topiclistNode = doc.SelectNodes("/Aggregationinfo/Aggregationdata/Websiteaggregationdata/Topiclist/Topic");
            XmlNodeList website_spacelistnode = doc.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Forum/Topiclist/Topic");
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
            topnumber.Text = doc.GetSingleNodeValue(doc.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Forum")[0], "Bbs/Topnumber");
            showtype.SelectedValue = doc.GetSingleNodeValue(doc.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Forum")[0], "Bbs/Showtype");
            #endregion
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveTopic_Click(object sender, EventArgs e)
        {
            #region ������Ϣ
            string tidlist = DNTRequest.GetString("forumtopicstatus");
            //��δѡ������ʱ�����������ѡ��
            if (tidlist == "")
            {
                XmlDocumentExtender doc = new XmlDocumentExtender();
                doc.Load(configPath);
                doc.RemoveNodeAndChildNode("/Aggregationinfo/Aggregationdata/Websiteaggregationdata/Topiclist");
                doc.RemoveNodeAndChildNode("/Aggregationinfo/Aggregationpage/Website/Forum/Topiclist");
                doc.Save(configPath);
                Response.Redirect("aggregation_editforumaggset.aspx");
                return;
            }
            else
            {
                PostInfo[] posts = new ForumAggregationData().GetPostListFromFile("Website");
                //�õ���ѡ��������Ϣ
                Posts.WriteAggregationPostData(posts, tablelist.SelectedValue, tidlist, configPath,
                    "/Aggregationinfo/Aggregationdata/Websiteaggregationdata/Topiclist",
                    "/Aggregationinfo/Aggregationpage/Website/Forum/Topiclist");

                AggregationFacade.BaseAggregation.ClearAllDataBind();
                Response.Redirect("aggregation_editforumaggset.aspx");
            }
            #endregion
        }

        /// <summary>
        /// ����������ʾ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveTopicDisplay_Click(object sender, EventArgs e)
        {
            #region ����������ʾ
            //if (!Utils.IsNumeric(topnumber.Text))
            //{
            //    base.RegisterStartupScript("", "<script>alert('��ʾ������������Ϊ����!');</script>");
            //    return;
            //}
            //if (Convert.ToInt32(topnumber.Text) <= 0)
            //{
            //    base.RegisterStartupScript("", "<script>alert('��ʾ��������������1��!');</script>");
            //    return;
            //}
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            doc.RemoveNodeAndChildNode("/Aggregationinfo/Aggregationpage/Website/Forum/Bbs");
            //doc.InitializeNode("/Aggregationinfo/Aggregationpage/Website/Forum");

            if (doc.SelectSingleNode("/Aggregationinfo/Aggregationpage/Website/Forum") == null)
                doc.InitializeNode("/Aggregationinfo/Aggregationpage/Website/Forum");

            XmlElement BBS = doc.CreateElement("Bbs");
            doc.AppendChildElementByNameValue(ref BBS, "Topnumber", topnumber.Text, false);
            doc.AppendChildElementByNameValue(ref BBS, "Showtype", showtype.SelectedValue, false);
            doc.SelectSingleNode("/Aggregationinfo/Aggregationpage/Website/Forum").AppendChild(BBS);
            doc.Save(configPath);
            AggregationConfig.ResetConfig();
            AggregationFacade.ForumAggregation.ClearAllDataBind();
            #endregion
        }

        #region Web ������������ɵĴ���

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