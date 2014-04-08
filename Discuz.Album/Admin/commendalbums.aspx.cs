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
    public class CommendAlbums : AdminPage
#else
    public partial class CommendAlbums : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected System.Web.UI.HtmlControls.HtmlForm Form1;
        protected System.Web.UI.WebControls.Literal albumlist;
        protected System.Web.UI.HtmlControls.HtmlInputHidden recommendalbum;

        protected Discuz.Control.Button searchalbum;
        protected Discuz.Web.Admin.ajaxalbumlist ajaxalbumlist1;
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
            #region 装载相册
            XmlDocument doc = new XmlDocument();
            doc.Load(configPath);
            string dataNode = "";
            string indexNode = "";
            string pagename = DNTRequest.GetString("pagename").ToLower();
            if(pagename == "albumindex")
            {
                dataNode = "/Aggregationinfo/Aggregationdata/Albumindexaggregationdata/Albumindex_albumlist/Album";
                indexNode = "/Aggregationinfo/Aggregationpage/Albumindex/Albumindex_albumlist/Album";
            }
            else if (pagename == "website")
            {
                dataNode = "/Aggregationinfo/Aggregationdata/Websiteaggregationdata/Website_albumlist/Album";
                indexNode = "/Aggregationinfo/Aggregationpage/Website/Website_albumlist/Album";
            }
            else
            {
                dataNode = "/Aggregationinfo/Aggregationdata/Spaceindexaggregationdata/Spaceindex_albumlist/Album";
                indexNode = "/Aggregationinfo/Aggregationpage/Spaceindex/Spaceindex_albumlist/Album";
            }
            XmlNodeList data_albumlistNode = doc.SelectNodes(dataNode);
            XmlNodeList index_albumlistNode = doc.SelectNodes(indexNode);
            XmlNodeInnerTextVisitor data_albumvisitor = new XmlNodeInnerTextVisitor();
            XmlNodeInnerTextVisitor index_albumvisitor = new XmlNodeInnerTextVisitor();
            albumlist.Text = "";
            int i = 0;
            foreach (XmlNode album in data_albumlistNode)
            {
                data_albumvisitor.SetNode(album);
                bool isCheck = false;
                foreach (XmlNode index in index_albumlistNode)
                {
                    index_albumvisitor.SetNode(index);
                    if (data_albumvisitor["albumid"].ToString() == index_albumvisitor["albumid"].ToString())
                    {
                        isCheck = true;
                        break;
                    }
                }
                albumlist.Text += "<div class='mo' id='m" + i + "' flag='f" + i + "'><h1><input type='checkbox' name='aid' " + (isCheck ? "checked" : "") + " value='" + data_albumvisitor["albumid"] + "'>" + data_albumvisitor["title"] + "</h1></div>\n";
                i++;
            }
            #endregion
        }

        private void savetopic_Click(object sender, EventArgs e)
        {
            #region 保存相册
            string aid = DNTRequest.GetString("aid");
            string aidlist = Utils.ClearLastChar(recommendalbum.Value);
            string dataNode = "";
            string indexNode = "";
            string pagename = DNTRequest.GetString("pagename").ToLower();
            if (pagename == "albumindex")
            {
                dataNode = "/Aggregationinfo/Aggregationdata/Albumindexaggregationdata/Albumindex_albumlist";
                indexNode = "/Aggregationinfo/Aggregationpage/Albumindex/Albumindex_albumlist";
            }
            else if (pagename == "website")
            {
                dataNode = "/Aggregationinfo/Aggregationdata/Websiteaggregationdata/Website_albumlist";
                indexNode = "/Aggregationinfo/Aggregationpage/Website/Website_albumlist";
            }
            else
            {
                dataNode = "/Aggregationinfo/Aggregationdata/Spaceindexaggregationdata/Spaceindex_albumlist";
                indexNode = "/Aggregationinfo/Aggregationpage/Spaceindex/Spaceindex_albumlist";
            }
            if (aidlist == "")
            {
                XmlDocumentExtender doc = new XmlDocumentExtender();
                doc.Load(configPath);
                doc.RemoveNodeAndChildNode(dataNode);
                doc.RemoveNodeAndChildNode(indexNode);
                doc.Save(configPath);
                Response.Redirect("aggregation_commendalbums.aspx?pagename=" + DNTRequest.GetString("pagename"));
                return;
            }
            else
            {
                DataTable dt = DbProvider.GetInstance().GetAlbumLitByAlbumidList(aidlist);
                XmlDocumentExtender doc = new XmlDocumentExtender();
                doc.Load(configPath);
                XmlNode data_albumslistnode = doc.InitializeNode(dataNode);
                XmlNode index_albumslistnode = doc.InitializeNode(indexNode);
                foreach (DataRow dr in dt.Rows)
                {
                    XmlElement album = doc.CreateElement("Album");
                    doc.AppendChildElementByDataRow(ref album, dt.Columns, dr, "description");
                    data_albumslistnode.AppendChild(album);
                    if (("," + aid + ",").IndexOf("," + dr["albumid"].ToString() + ",") >= 0)
                        index_albumslistnode.AppendChild(album.Clone());
                }
                doc.Save(configPath);
                AggregationFacade.BaseAggregation.ClearAllDataBind();
                Response.Redirect("aggregation_commendalbums.aspx?pagename=" + DNTRequest.GetString("pagename"));
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