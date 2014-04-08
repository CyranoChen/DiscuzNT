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
    /// ������̳ͳ��
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
            #region �����ƶ����

            if (this.CheckCookie())
            {
                AdminForumStats.ReSetClearMove();
                base.RegisterStartupScript( "", "<script language=javascript>clearflag();</script>");
            }

            #endregion
        }


        private void ReSetStatistic_Click(object sender, EventArgs e)
        {
            #region �ؽ���̳ͳ��(��)����

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
            #region ϵͳ������̳���

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
            #region �������ӷֱ�����ص����/С������ID

            if (this.CheckCookie())
            {
                Posts.UpdateMinMaxField();
                //string tableprefix = BaseConfigs.GetTablePrefix + "posts";
                //foreach (DataRow dr in Posts.GetAllPostTable().Rows)
                //{
                //    //�Գ���ǰ��֮������ӱ����ͳ��
                //    if (Discuz.Forum.Posts.GetPostTableName() != (tableprefix + dr["id"].ToString()))
                //    {
                //        //���µ�ǰ�������ID�ļ�¼�õ�������Сtid�ֶ�		
                //        Posts.UpdateMinMaxField(tableprefix + dr["id"].ToString(), Utils.StrToInt(dr["id"], 0));
                //    }
                //}
                base.RegisterStartupScript( "", "<script language=javascript>clearflag();</script>");
            }

            #endregion
        }

        public void UpdateCurTopics_Click(object sender, EventArgs e)
        {
            #region �������а���������

            //foreach (ForumInfo info in Forums.GetForumList())
            //{
            //    Discuz.Forum.Forums.SetRealCurrentTopics(info.Fid);
            //}
            Discuz.Forum.Forums.ResetForumsTopics();
            #endregion
        }

        public void UpdateForumLastPost_Click(object sender, EventArgs e)
        {
            #region ���°�������

            Discuz.Forum.Forums.ResetLastPostInfo();

            #endregion
        }

        
        public void CreateFullTextIndex_Click(object sender, EventArgs e)
        {
            #region ����ȫ������

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

        #region Web ������������ɵĴ���

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
