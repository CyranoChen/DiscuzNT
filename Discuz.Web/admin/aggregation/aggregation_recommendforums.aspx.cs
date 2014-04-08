using System;
using System.Web.UI;
using System.Data;
using System.Xml;
using System.Text;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Common;
using Discuz.Aggregation;
using Discuz.Config;
using Discuz.Common.Xml;
using Discuz.Cache;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 缓存管理
    /// </summary>
    public partial class aggregation_recommendforums : AdminPage
    {
        private string configPath;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 装载信息
            if (!IsPostBack)
            {
                list1.BuildTree(Forums.GetForumListForDataTable(), "name", "fid");
                list1.TypeID.Items.RemoveAt(0);
                list1.TypeID.Width = 260;
                list1.TypeID.Height = 290;
                list1.TypeID.SelectedIndex = 0;
                LoadInfo();
            }
            #endregion
        }

        private void LoadInfo()
        {
            string fids = "";
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNode fidlist = doc.SelectSingleNode("/Aggregationinfo/Aggregationpage/Website/Website_forumrecomend/fidlist");
            if (fidlist == null)
            {
                return;
            }
            fids = fidlist.InnerText;
            string strJson = "";
            Discuz.Common.Generic.List<ForumInfo> lists = Discuz.Forum.Forums.GetForumList();
            foreach (string fid in fids.Split(','))
            {
                foreach (ForumInfo foruminfo in lists)
                {
                    if (foruminfo.Fid.ToString() == fid)
                    {
                        strJson += "{'fid':'" + foruminfo.Fid + "','forumtitle':'" + foruminfo.Name + "'},";
                        break;
                    }
                }
            }
            if (strJson != "")
            {
                strJson = strJson.TrimEnd(',');
            }

            strJson = "<script type='text/javascript'>\r\nvar fidlist = [" + strJson + "];\r\nfor(var i = 0 ; i < fidlist.length ; i++)\r\n{\r\nvar no = new Option();\r\nno.value = fidlist[i]['fid'];\r\nno.text = fidlist[i]['forumtitle'];\r\nForm1.list2.options[Form1.list2.options.length] = no;\r\n}\r\n</script>";
            base.RegisterStartupScript("", strJson);
        }


        private void Btn_SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存自动提取数据
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNode fidlist = doc.InitializeNode("/Aggregationinfo/Aggregationpage/Website/Website_forumrecomend/fidlist");
            fidlist.InnerText = DNTRequest.GetString("rst");
            doc.Save(configPath);
            AggregationFacade.BaseAggregation.ClearAllDataBind();
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