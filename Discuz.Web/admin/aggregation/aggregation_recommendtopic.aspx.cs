using System;
using System.Web.UI;
using System.Data;
using System.Xml;
using System.Text;
using System.Web.UI.WebControls;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Common;
using Discuz.Aggregation;
using Discuz.Config;
using Discuz.Common.Xml;
using Discuz.Cache;
using Discuz.Entity;
using Discuz.Common.Generic;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 缓存管理
    /// </summary>
    public partial class aggregation_recommendtopic : AdminPage
    {
        private string configPath;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 装载信息
            if (!IsPostBack)
            {
                /*list1.BuildTree(DatabaseProvider.GetInstance().GetForumsTree());
                list1.TypeID.Items.RemoveAt(0);
                list1.TypeID.Width = 260;
                list1.TypeID.Height = 290;
                list1.TypeID.SelectedIndex = 0;*/
                LoadInfo();
            }
            #endregion
        }

        private void LoadInfo()
        {
            string fids = "";
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            fids = doc.SelectSingleNode("/Aggregationinfo/Aggregationpage/Website/Website_forumrecomend/fidlist").InnerText;
            XmlNodeList forumrecomendtopic = doc.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Website_forumrecomendtopiclist/Website_forumrecomendtopic");
            List<ForumInfo> lists = Discuz.Forum.Forums.GetForumList();
            DataTable forumdata = new DataTable();
            forumdata.Columns.Add("fid");
            forumdata.Columns.Add("name");
            forumdata.Columns.Add("tid");
            forumdata.Columns.Add("title");
            forumdata.Columns.Add("img");

            foreach (string fid in fids.Split(','))
            {
                foreach (ForumInfo foruminfo in lists)
                {
                    if (foruminfo.Fid.ToString() == fid)    //在板块信息中找到当前选择的板块
                    {
                        DataRow dr = forumdata.NewRow();
                        dr["fid"] = fid;
                        dr["name"] = foruminfo.Name;
                        dr["tid"] = "";
                        dr["title"] = "";
                        dr["img"] = "";
                        forumdata.Rows.Add(dr);
                        break;
                    }
                }
            }

            foreach (XmlNode topic in forumrecomendtopic)
            {
                foreach (DataRow dr in forumdata.Rows)
                {
                    if (topic["fid"].InnerText == dr["fid"].ToString())
                    {
                        dr["tid"] = topic["tid"].InnerText;
                        dr["title"] = topic["title"].InnerText;
                        dr["img"] = topic["img"].InnerText;
                        break;
                    }
                }
            }

            DataGrid1.TableHeaderName = "推荐版块图片选择";
            DataGrid1.DataKeyField = "fid";
            DataGrid1.DataSource = forumdata;
            DataGrid1.DataBind();
        }

        protected void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                System.Web.UI.WebControls.TextBox t = (System.Web.UI.WebControls.TextBox)e.Item.Cells[1].Controls[0];
                t.Attributes.Add("size", "5");
                t.Width = 60;

                t = (System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0];
                t.Width = 200;

                t = (System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0];
                t.Width = 200;

            }
            #endregion
        }


        private void Btn_SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存自动提取数据
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            int rowid = 0;
            XmlNode topiclist = doc.InitializeNode("/Aggregationinfo/Aggregationpage/Website/Website_forumrecomendtopiclist");
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                string fid = o.ToString();
                string tid = DataGrid1.GetControlValue(rowid, "tid");
                string title = DataGrid1.GetControlValue(rowid, "title");
                string img = DataGrid1.GetControlValue(rowid, "img");
                XmlElement topicNode = doc.CreateElement("Website_forumrecomendtopic");
                doc.AppendChildElementByNameValue(ref topicNode, "fid", fid);
                doc.AppendChildElementByNameValue(ref topicNode, "tid", tid);
                doc.AppendChildElementByNameValue(ref topicNode, "img", img);
                doc.AppendChildElementByNameValue(ref topicNode, "title", title);
                topiclist.AppendChild(topicNode);
                rowid++;
            }

            /*
            XmlNode fidlist = doc.InitializeNode("/Aggregationinfo/Aggregationpage/Website/Website_forumrecomend/fidlist");
            fidlist.InnerText = DNTRequest.GetString("rst");*/
            doc.Save(configPath);
            Response.Redirect("aggregation_recommendtopic.aspx");
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
            this.Btn_SaveInfo.Click += new EventHandler(this.Btn_SaveInfo_Click);
            configPath = Server.MapPath(BaseConfigs.GetForumPath + "config/aggregation.config");
        }

        #endregion

    }
}