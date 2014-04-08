using System;
using System.Data;
using System.Web.UI;
using System.Xml;
using System.Web.UI.WebControls;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;
using Discuz.Aggregation;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Common.Xml;
using Discuz.Web.Admin;
using Discuz.Album.Data;

namespace Discuz.Album.Admin
{
    /// <summary>
    /// 查找要审核的主题 
    /// </summary>
#if NET1
    public class PhotoAggset : AdminPage
#else
    public partial class PhotoAggset : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TabControl TabControl1;
        protected Discuz.Control.TabPage tabPage51;
        protected Discuz.Control.Button SearchPhoto;
        protected Discuz.Web.Admin.ajaxphotoinfo AjaxPhotoInfo1;
        protected Discuz.Control.TabPage tabPage22;
        protected Discuz.Control.Button SearchAlbum;
        protected Discuz.Web.Admin.ajaxalbumlist AjaxAlbumList1;
        protected Discuz.Control.TabPage tabPage33;
        protected Discuz.Control.RadioButtonList focusphotoshowtype;
        protected Discuz.Control.TextBox focusphotodays;
        protected Discuz.Control.TextBox focusphotocount;
        protected Discuz.Control.RadioButtonList recommendalbumtype;
        protected Discuz.Control.TextBox focusalbumdays;
        protected Discuz.Control.TextBox focusalbumcount;
        protected Discuz.Control.TextBox weekhot;
        protected Discuz.Control.TextBox lastupdatespace;
        protected Discuz.Control.Button SaveTopic;

		protected System.Web.UI.WebControls.Literal photolist;
        protected System.Web.UI.WebControls.Literal spacelist;
        protected System.Web.UI.HtmlControls.HtmlInputHidden recommendphoto;
        protected System.Web.UI.HtmlControls.HtmlInputHidden recommendalbum;
        #endregion
#endif

        private string configPath;
        private string pagename;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadWebSiteConfig();
            }
        }

        /// <summary>
        /// 装载WebSite信息
        /// </summary>
        private void LoadWebSiteConfig()
        {
            #region 相册信息
            XmlDocument doc = new XmlDocument();
            doc.Load(configPath);
            string dataNode = "";
            string indexNode = "";
            if(pagename == "Website")
            {
                dataNode = "/Aggregationinfo/Aggregationdata/Websiteaggregationdata/Website_photolist/Photo";
                indexNode = "/Aggregationinfo/Aggregationpage/Website/Website_hotolist/Photo";
            }
            else
            {
                dataNode = "/Aggregationinfo/Aggregationdata/Albumaggregationdata/Albumindex_photolist/Photo";
                indexNode = "/Aggregationinfo/Aggregationpage/Albumindex/Albumindex_photolist/Photo";
            }
            XmlNodeList data_photolistNode = doc.SelectNodes(dataNode);
            XmlNodeList index_photolistNode = doc.SelectNodes(indexNode);
            XmlNodeInnerTextVisitor data_photovisitor = new XmlNodeInnerTextVisitor();
            XmlNodeInnerTextVisitor index_photovisitor = new XmlNodeInnerTextVisitor();
            photolist.Text = "";
            int i = 0;
            foreach (XmlNode photo in data_photolistNode)
            {
                data_photovisitor.SetNode(photo);
                bool isCheck = false;
                foreach (XmlNode index in index_photolistNode)
                {
                    index_photovisitor.SetNode(index);
                    if (data_photovisitor["photoid"].ToString() == index_photovisitor["photoid"].ToString())
                    {
                        isCheck = true;
                        break;
                    }
                }
                photolist.Text += "<div class='mo' id='m" + i + "' flag='f" + i + "'><h1><input type='checkbox' name='pid' " + (isCheck ? "checked" : "") + " value='" + data_photovisitor["photoid"].ToString() + "'>" + data_photovisitor["title"].ToString() + "</h1></div>\n";
                i++;
            }
            #endregion
        }

        private void SaveTopic_Click(object sender, EventArgs e)
        {
            #region 保存相册修改
            string pid = DNTRequest.GetString("pid");
            string pidlist = Utils.ClearLastChar(recommendphoto.Value);
            string dataNode = "";
            string indexNode = "";
            if (pagename == "Website")
            {
                dataNode = "/Aggregationinfo/Aggregationdata/Websiteaggregationdata/Website_photolist";
                indexNode = "/Aggregationinfo/Aggregationpage/Website/Website_photolist";
            }
            else
            {
                dataNode = "/Aggregationinfo/Aggregationdata/Albumaggregationdata/Albumindex_photolist";
                indexNode = "/Aggregationinfo/Aggregationpage/Albumindex/Albumindex_photolist";
            }
            if (pidlist == "")
            {
                XmlDocumentExtender doc = new XmlDocumentExtender();
                doc.Load(configPath);
                doc.RemoveNodeAndChildNode(dataNode);
                doc.RemoveNodeAndChildNode(indexNode);
                doc.Save(configPath);
                Response.Redirect("aggregation_photoaggset.aspx?pagename="  + DNTRequest.GetString("pagename"));
                return;
            }
            else
            {
                IDataReader dr = DbProvider.GetInstance().GetRecommendPhotoList(pidlist);
                XmlDocumentExtender doc = new XmlDocumentExtender();
                doc.Load(configPath);
                XmlNode data_photolistnode = doc.InitializeNode(dataNode);
                XmlNode index_photolistnode = doc.InitializeNode(indexNode);
                string[] colName = { "photoid", "filename", "attachment", "filesize", "title", "description", "postdate", "albumid", "userid", "username",
                                     "views", "commentstatus", "tagstatus", "comments", "isattachment" };

                while(dr.Read())
                {
                    XmlElement photo = doc.CreateElement("Photo");
                    foreach (string col in colName)
                    {
                        XmlElement node = doc.CreateElement(col);
                        if(col == "filename")
                            node.InnerText = dr[col].ToString().Trim().Replace(".","_thumbnail.");
                        else
                            node.InnerText = dr[col].ToString().Trim() ;
                        photo.AppendChild(node);
                    }
                    data_photolistnode.AppendChild(photo);
                    if (("," + pid + ",").IndexOf("," + dr["photoid"].ToString() + ",") >= 0)
                        index_photolistnode.AppendChild(photo.Clone());
                }
                dr.Close();
                doc.Save(configPath);
                AggregationFacade.BaseAggregation.ClearAllDataBind();
                AggregationFacade.AlbumAggregation.ClearDataBind();
                Response.Redirect("aggregation_photoaggset.aspx?pagename=" + DNTRequest.GetString("pagename"));
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
            switch (DNTRequest.GetString("pagename").ToLower())
            {
                case "website":
                    {
                        pagename = "Website";
                        break;
                    }
                case "albumindex":
                    {
                        pagename = "Albumindex";
                        break;
                    }
                default:
                    {
                        pagename = "Website";
                        break;
                    }
            }
        }

        #endregion

    }
}