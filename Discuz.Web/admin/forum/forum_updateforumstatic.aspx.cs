using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 更新论坛统计
    /// </summary>
    public partial class updateforumstatic : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UpdateStoreProcPanel.Visible = Databases.IsStoreProc();
            endtid.Text = Discuz.Forum.Posts.GetPostTableId();
            starttid.Text = (TypeConverter.StrToInt(endtid.Text) - 5).ToString();
        }


        private void SubmitClearFlag_Click(object sender, EventArgs e)
        {
            #region 清理移动标记

            if (this.CheckCookie())
            {
                AdminForumStats.ReSetClearMove();
                base.RegisterStartupScript( "", "<script language=javascript>clearflag();</script>");
            }

            #endregion
        }


        private void ReSetStatistic_Click(object sender, EventArgs e)
        {
            #region 重建论坛统计(表)数据

            if (this.CheckCookie())
            {
                AdminForumStats.ReSetStatistic();
                Caches.ReSetStatistics();
                base.RegisterStartupScript( "", "<script language=javascript>clearflag();</script>");
            }

            #endregion
        }

        private void SysteAutoSet_Click(object sender, EventArgs e)
        {
            #region 系统调整论坛版块

            if (this.CheckCookie())
            {
                AdminForums.SetForumslayer();
                AdminForums.SetForumsSubForumCountAndDispalyorder();
                AdminForums.SetForumsPathList();
                AdminForums.SetForumsStatus();
                Caches.ReSetForumLinkList();
                Caches.ReSetForumList();
                Caches.ReSetForumListBoxOptions();

                base.RegisterStartupScript( "", "<script language=javascript>clearflag();</script>");
            }

            #endregion
        }

        private void UpdatePostSP_Click(object sender, EventArgs e)
        {
            if (this.CheckCookie())
            {
                UpdatePostStoreProc();
                base.RegisterStartupScript("", "<script language=javascript>clearflag();</script>");
            }
        }

        public void UpdatePostStoreProc()
        {
            Databases.UpdatePostSP();
        }

        private void UpdatePostMaxMinTid_Click(object sender, EventArgs e)
        {
            #region 更新帖子分表中相关的最大/小的主题ID

            if (this.CheckCookie())
            {
                Posts.UpdateMinMaxField();
                //string tableprefix = BaseConfigs.GetTablePrefix + "posts";
                //foreach (DataRow dr in Posts.GetAllPostTable().Rows)
                //{
                //    //对除当前表之外的帖子表进行统计
                //    if (Discuz.Forum.Posts.GetPostTableName() != (tableprefix + dr["id"].ToString()))
                //    {
                //        //更新当前表中最大ID的记录用的最大和最小tid字段		
                //        Posts.UpdateMinMaxField(tableprefix + dr["id"].ToString(), Utils.StrToInt(dr["id"], 0));
                //    }
                //}
                base.RegisterStartupScript( "", "<script language=javascript>clearflag();</script>");
            }

            #endregion
        }

        public void UpdateCurTopics_Click(object sender, EventArgs e)
        {
            #region 更新所有版块的主题数

            //foreach (ForumInfo info in Forums.GetForumList())
            //{
            //    Discuz.Forum.Forums.SetRealCurrentTopics(info.Fid);
            //}
            Discuz.Forum.Forums.ResetForumsTopics();
            #endregion
        }

        public void UpdateForumLastPost_Click(object sender, EventArgs e)
        {
            #region 更新版块最后发帖

            Discuz.Forum.Forums.ResetLastPostInfo();

            #endregion
        }

        
        public void CreateFullTextIndex_Click(object sender, EventArgs e)
        {
            #region 建立全文索引

            if (this.CheckCookie())
            {
                if (Databases.IsFullTextSearchEnabled()==false)
                    return;

                string msg = new Databases().CreateFullTextIndex(Discuz.Forum.Databases.GetDbName(), this.username);
                if (msg.StartsWith("window"))
                    base.LoadRegisterStartupScript("PAGE", msg);
                else
                    base.RegisterStartupScript("", msg);

                //try
                //{
                //    Databases.CreateFullTextIndex(Discuz.Forum.Databases.GetDbName());

                //    aysncallback = new delegateCreateFillIndex(CreateFullText);
                //    AsyncCallback myCallBack = new AsyncCallback(CallBack);
                //    aysncallback.BeginInvoke(Discuz.Forum.Databases.GetDbName(), myCallBack, this.username); //
                //    base.LoadRegisterStartupScript("PAGE", "window.location.href='forum_updateforumstatic.aspx';");
                //}
                //catch (Exception ex)
                //{
                //    string message = ex.Message.Replace("'", " ");
                //    message = message.Replace("\\", "/");
                //    message = message.Replace("\r\n", "\\r\\n");
                //    message = message.Replace("\r", "\\r");
                //    message = message.Replace("\n", "\\n");
                //    base.RegisterStartupScript( "", "<script>alert('" + message + "');</script>");
                //}
            }

            #endregion
        }

        private void UpdateMyTopic_Click(object sender, EventArgs e)
        {
            Topics.UpdateMyTopic();
        }

        private void UpdateMyPost_Click(object sender, EventArgs e)
        {
            Posts.UpdateMyPost();
        }

        public void ResetTodayPosts_Click(object sender, EventArgs e)
        {
            Discuz.Forum.Forums.ResetTodayPosts();
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SysteAutoSet.Click += new EventHandler(this.SysteAutoSet_Click);
            this.SubmitClearFlag.Click += new EventHandler(this.SubmitClearFlag_Click);
            //this.UpdatePostSP.Click += new EventHandler(this.UpdatePostSP_Click);
            this.UpdatePostMaxMinTid.Click += new EventHandler(this.UpdatePostMaxMinTid_Click);
            this.CreateFullTextIndex.Click += new EventHandler(this.CreateFullTextIndex_Click);
            this.ReSetStatistic.Click += new EventHandler(this.ReSetStatistic_Click);
            this.UpdateCurTopics.Click += new EventHandler(this.UpdateCurTopics_Click);
            this.UpdateMyTopic.Click += new EventHandler(this.UpdateMyTopic_Click);
            this.ResetTodayPosts.Click += new EventHandler(this.ResetTodayPosts_Click);
            //this.UpdateMyPost.Click += new EventHandler(this.UpdateMyPost_Click);
            this.UpdateForumLastPost.Click += new EventHandler(this.UpdateForumLastPost_Click);
        }

        #endregion
    }
}
