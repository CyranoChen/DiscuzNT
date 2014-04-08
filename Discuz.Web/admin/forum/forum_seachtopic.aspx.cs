using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 搜索主题
    /// </summary>
    public partial class seachtopic : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                postdatetimeStart.SelectedDate = DateTime.Now.AddDays(-30);
                postdatetimeEnd.SelectedDate = DateTime.Now;
            }
            forumid.BuildTree(Forums.GetForumListForDataTable(),"name","fid");
            forumid.TypeID.Items.RemoveAt(0);
            forumid.TypeID.Items.Insert(0, new ListItem("全部", "0"));

        }

        private void SaveSearchCondition_Click(object sender, EventArgs e)
        {
            #region 生成查询条件

            if (this.CheckCookie())
            {
                //TODO:条件，先各个
                
                string sqlstring = Topics.GetSearchTopicsCondition(Utils.StrToInt(forumid.SelectedValue, 0), keyword.Text, displayorder.SelectedValue,
                    digest.SelectedValue, attachment.SelectedValue,poster.Text, lowerupper.Checked, viewsmin.Text, viewsmax.Text, repliesmax.Text, repliesmin.Text,
                    rate.Text, lastpost.Text, postdatetimeStart.SelectedDate, postdatetimeEnd.SelectedDate);

                Session["topicswhere"] = sqlstring;
                Response.Redirect("forum_topicsgrid.aspx");
            }

            #endregion
        }

        #region 把VIEWSTATE写入容器

        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            base.DiscuzForumSavePageState(viewState);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            return base.DiscuzForumLoadPageState();
        }

        #endregion

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveSearchCondition.Click += new EventHandler(this.SaveSearchCondition_Click);
        }

        #endregion
    }
}