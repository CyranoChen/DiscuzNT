using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Cache;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 广告列表
    /// </summary>
    public partial class advsgrid : AdminPage
    {
        public int type = DNTRequest.GetInt("type", -1);
        public string[] advtypes = new string[] { "头部横幅广告", "尾部横幅广告", "页内文字广告", "帖内广告", "浮动广告", "对联广告", "Silverlight媒体广告", "帖间通栏广告", "分类间广告", "快速发帖栏上方广告", "快速编辑器背景广告", "聚合首页头部广告", "聚合首页热贴下方广告", "聚合首页发帖排行上方广告", "聚合首页推荐版块上方广告", "聚合首页推荐版块下方广告", "聚合首页推荐相册下方广告", "聚合首页底部广告", "页内横幅广告" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitAdvertisementAvailable();
                BindData(type);
            }
        }

        private void InitAdvertisementAvailable()
        {
            DataRow[] drs = Advertisements.GetAdvertisements(type).Select("endtime < '" + DateTime.Now.ToString() + "'");
            if (drs.Length == 0)
                return;
            string aidList = "";
            foreach (DataRow dr in drs)
            {
                aidList += dr["advid"] + ",";
            }
            if (aidList != "")
                Advertisements.UpdateAdvertisementAvailable(aidList.TrimEnd(','), 0);
        }

        public void BindData(int type)
        {
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.BindData(Advertisements.GetAdvertisements(type));
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        private void DelAds_Click(object sender, EventArgs e)
        {
            #region 删除指定的广告
            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("advid") != "")
                {
                    Advertisements.DeleteAdvertisementList(DNTRequest.GetString("advid"));
                    DNTCache.GetCacheService().RemoveObject("/Forum/Advertisements");
                    base.RegisterStartupScript("PAGE", "window.location.href='global_advsgrid.aspx';");
                }
                else
                    base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='global_advsgrid.aspx';</script>");
            }
            #endregion
        }

        public string BoolStr(string closed)
        {
            #region 广告是否有效图片,用于前台绑定
            if (closed == "1")
                return "<div align=center><img src=../images/OK.gif /></div>";
            else
                return "<div align=center><img src=../images/Cancel.gif /></div>";
            #endregion
        }

        public string ParameterType(string parameters)
        {
            return parameters.Split('|')[0];
        }

        public string TargetsType(string targets)
        {
            #region 将广告投放范围的标识串转换为文字
            string result = ""; //广告投放范围的标识串
            if (targets.IndexOf("全部") >= 0) return "全部";
            else
            {
                if (targets.IndexOf("首页") >= 0)
                {
                    result = "首页,";
                    targets = targets.Replace("首页,", "");
                }
            }

            if (targets.Trim() != "首页")
                foreach (ForumInfo info in Forums.GetForumList(targets))
                    result += info.Name + ",";

            return result.Length > 0 ? result.Substring(0, result.Length - 1) : "";
            #endregion
        }


        private void SetUnAvailable_Click(object sender, EventArgs e)
        {
            UpdateAdvertisementAvailable(0);
        }

        private void SetAvailable_Click(object sender, EventArgs e)
        {
            UpdateAdvertisementAvailable(1);
        }

        private void UpdateAdvertisementAvailable(int available)
        {
            #region 设置公告为有效状态
            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("advid") != "")
                {
                    Advertisements.UpdateAdvertisementAvailable(DNTRequest.GetString("advid"), available);
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/Advertisements");
                    base.RegisterStartupScript("PAGE", "window.location.href='global_advsgrid.aspx';");
                }
                else
                    base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='global_advsgrid.aspx';</script>");
            }
            #endregion
        }

        public string GetAdType(string adtype)
        {
            return advtypes[TypeConverter.StrToInt(adtype)];
        }


        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SetAvailable.Click += new EventHandler(this.SetAvailable_Click);
            this.SetUnAvailable.Click += new EventHandler(this.SetUnAvailable_Click);
            this.DelAds.Click += new EventHandler(this.DelAds_Click);

            #region 绑定数据
            DataGrid1.TableHeaderName = "广告列表";
            DataGrid1.DataKeyField = "advid";
            DataGrid1.ColumnSpan = 12;
            #endregion
        }

        #endregion
    }
}