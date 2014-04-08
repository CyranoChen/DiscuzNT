using System;
using System.Web.UI;
using System.Data;
using System.Xml;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Common;
using Discuz.Aggregation;
using Discuz.Config;
using Discuz.Common.Xml;
using Discuz.Cache;
using Discuz.Web.Admin;


namespace Discuz.Space.Admin
{
    /// <summary>
    /// 缓存管理
    /// </summary>
    
#if NET1
    public class SpaceIndexAggset : AdminPage
#else
    public partial class SpaceIndexAggset : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TextBox newcommentcount;
        protected Discuz.Control.TextBox newcommentcounttimeout;
        protected Discuz.Control.TextBox maxarticlecommentcount;
        protected Discuz.Control.TextBox maxarticlecommentcounttimeout;
        protected Discuz.Control.TextBox maxarticleviewcount;
        protected Discuz.Control.TextBox maxarticleviewcounttimeout;
        protected Discuz.Control.TextBox maxcommentcount;
        protected Discuz.Control.TextBox maxcommentcounttimeout;
        protected Discuz.Control.TextBox maxspaceviewcount;
        protected Discuz.Control.TextBox maxspaceviewcounttimeout;
        protected Discuz.Control.TextBox maxpostarticlespacecount;
        protected Discuz.Control.TextBox maxpostarticlespacecounttimeout;
        protected Discuz.Control.TextBox updatespacecount;
        protected Discuz.Control.TextBox updatespacetimeout;
        protected Discuz.Control.Button Btn_SaveInfo;
        #endregion
#endif


        protected void Page_Load(object sender, EventArgs e)
        {
            #region 装载信息
            if (!IsPostBack)
            {
                AggregationConfigInfo aci = AggregationConfig.GetConfig();
                newcommentcount.Text = aci.SpaceTopNewCommentsCount.ToString();
                newcommentcounttimeout.Text = aci.SpaceTopNewCommentsTimeout.ToString();
                maxarticlecommentcount.Text = aci.TopcommentcountPostListCount.ToString();
                maxarticlecommentcounttimeout.Text = aci.TopcommentcountPostListTimeout.ToString();
                maxarticleviewcount.Text = aci.TopviewsPostListCount.ToString();
                maxarticleviewcounttimeout.Text = aci.TopviewsPostListTimeout.ToString();
                maxcommentcount.Text = aci.TopcommentcountSpaceListCount.ToString();
                maxcommentcounttimeout.Text = aci.TopcommentcountSpaceListTimeout.ToString();
                maxspaceviewcount.Text = aci.TopvisitedtimesSpaceListCount.ToString();
                maxspaceviewcounttimeout.Text = aci.TopvisitedtimesSpaceListTimeout.ToString();
                maxpostarticlespacecount.Text = aci.ToppostcountSpaceListCount.ToString();
                maxpostarticlespacecounttimeout.Text = aci.ToppostcountSpaceListTimeout.ToString();
                updatespacecount.Text = aci.RecentUpdateSpaceAggregationListCount.ToString();
                updatespacetimeout.Text = aci.RecentUpdateSpaceAggregationListTimeout.ToString();
            }
            #endregion
        }


        private void Btn_SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存自动提取数据
            int inewcommentcount = Convert.ToInt32(newcommentcount.Text);
            if (!ValidateCount(inewcommentcount)) return;
            int inewcommentcounttimeout = Convert.ToInt32(newcommentcounttimeout.Text);
            if (!ValidateTimeout(inewcommentcounttimeout)) return;
            int imaxarticlecommentcount = Convert.ToInt32(maxarticlecommentcount.Text);
            if (!ValidateCount(imaxarticlecommentcount)) return;
            int imaxarticlecommentcounttimeout = Convert.ToInt32(maxarticlecommentcounttimeout.Text);
            if (!ValidateTimeout(imaxarticlecommentcounttimeout)) return;
            int imaxarticleviewcount = Convert.ToInt32(maxarticleviewcount.Text);
            if (!ValidateCount(imaxarticleviewcount)) return;
            int imaxarticleviewcounttimeout = Convert.ToInt32(maxarticleviewcounttimeout.Text);
            if (!ValidateTimeout(imaxarticleviewcounttimeout)) return;
            int imaxcommentcount = Convert.ToInt32(maxcommentcount.Text);
            if (!ValidateCount(imaxcommentcount)) return;
            int imaxcommentcounttimeout = Convert.ToInt32(maxcommentcounttimeout.Text);
            if (!ValidateTimeout(imaxcommentcounttimeout)) return;
            int imaxspaceviewcount = Convert.ToInt32(maxspaceviewcount.Text);
            if (!ValidateCount(imaxspaceviewcount)) return;
            int imaxspaceviewcounttimeout = Convert.ToInt32(maxspaceviewcounttimeout.Text);
            if (!ValidateTimeout(imaxspaceviewcounttimeout)) return;
            int imaxpostarticlespacecount = Convert.ToInt32(maxpostarticlespacecount.Text);
            if (!ValidateCount(imaxpostarticlespacecount)) return;
            int imaxpostarticlespacecounttimeout = Convert.ToInt32(maxpostarticlespacecounttimeout.Text);
            if (!ValidateTimeout(imaxpostarticlespacecounttimeout)) return;
            int iupdatespacecount = Convert.ToInt32(updatespacecount.Text);
            if (!ValidateCount(iupdatespacecount)) return;
            int iupdatespacetimeout = Convert.ToInt32(updatespacetimeout.Text);
            if (!ValidateTimeout(iupdatespacetimeout)) return;


            DNTCache cache = DNTCache.GetCacheService();
            cache.RemoveObject("/Space/SpaceTopNewComments");
            cache.RemoveObject("/Space/TopcommentcountPostList");
            cache.RemoveObject("/Space/TopviewsPostList");
            cache.RemoveObject("/Space/TopcommentcountSpaceList");
            cache.RemoveObject("/Space/TopvisitedtimesSpaceList");
            cache.RemoveObject("/Space/ToppostcountSpaceList");
            cache.RemoveObject("/Space/RecentUpdateSpaceAggregationList");

            AggregationConfigInfo aci = AggregationConfig.GetConfig();
            aci.SpaceTopNewCommentsCount = inewcommentcount;
            aci.SpaceTopNewCommentsTimeout = inewcommentcounttimeout;
            aci.TopcommentcountPostListCount = imaxarticlecommentcount;
            aci.TopcommentcountPostListTimeout = imaxarticlecommentcounttimeout;
            aci.TopviewsPostListCount = imaxarticleviewcount;
            aci.TopviewsPostListTimeout = imaxarticleviewcounttimeout;
            aci.TopcommentcountSpaceListCount = imaxcommentcount;
            aci.TopcommentcountSpaceListTimeout = imaxcommentcounttimeout;
            aci.TopvisitedtimesSpaceListCount = imaxspaceviewcount;
            aci.TopvisitedtimesSpaceListTimeout = imaxspaceviewcounttimeout;
            aci.ToppostcountSpaceListCount = imaxpostarticlespacecount;
            aci.ToppostcountSpaceListTimeout = imaxpostarticlespacecounttimeout;
            aci.RecentUpdateSpaceAggregationListCount = iupdatespacecount;
            aci.RecentUpdateSpaceAggregationListTimeout = iupdatespacetimeout;
            AggregationConfig.SaveConfig(aci);
            AggregationFacade.BaseAggregation.ClearAllDataBind();
            #endregion
        }

        private bool ValidateCount(int val)
        {
            #region 验证提取条数
            if (val <= 0)
            {
                base.RegisterStartupScript("", "<script>alert('页面中各项提取条数必须大于0条');window.location.href='aggregation_spaceindexaggset.aspx';</script>");
                return false;
            }
            else
                return true;
            #endregion
        }

        private bool ValidateTimeout(int val)
        {
            #region 验证提取分钟数
            if (val >= 10 && val <= 300)
                return true;
            else
            {
                base.RegisterStartupScript("", "<script>alert('页面中各项提取条数时间间隔必须10～300分钟之间');window.location.href='aggregation_spaceindexaggset.aspx';</script>");
                return false;
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
            //this.Btn_topncommentspace.Click += new EventHandler(this.Btn_topncommentspace_Click);
            //this.Btn_topcommentarticle.Click += new EventHandler(this.Btn_topcommentarticle_Click);
            //this.Btn_topviewarticle.Click += new EventHandler(this.Btn_topviewarticle_Click);
            //this.Btn_topcommentspace.Click += new EventHandler(this.Btn_topcommentspace_Click);
            //this.Btn_topviewspace.Click += new EventHandler(this.Btn_topviewspace_Click);
            //this.Btn_toppostcountspace.Click += new EventHandler(this.Btn_toppostcountspace_Click);
            this.Btn_SaveInfo.Click += new EventHandler(this.Btn_SaveInfo_Click);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion

    }
}