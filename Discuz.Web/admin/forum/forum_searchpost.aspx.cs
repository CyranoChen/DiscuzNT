using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 搜索帖子
    /// </summary>
    public partial class searchpost : AdminPage
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


        private void SaveConditionInf_Click(object sender, EventArgs e)
        {
            #region 生成查询条件

            if (this.CheckCookie())
            {
                //TODO:条件，先各个
                string sqlstring = Posts.SearchPost(Utils.StrToInt(forumid.SelectedValue, 0), postlist.SelectedValue, postdatetimeStart.SelectedDate, postdatetimeEnd.SelectedDate, poster.Text, lowerupper.Checked, Ip.Text, message.Text);
                Session["seachpost_fid"] = forumid.SelectedValue;
                //Session["posttablename"] = BaseConfigs.GetTablePrefix + "posts" + postlist.SelectedValue;
                Session["posttablename"] = BaseConfigs.GetTablePrefix + "posts" + DNTRequest.GetString("postlist:postslist");
                Session["postswhere"] = sqlstring;
                Response.Redirect("forum_postgridmanage.aspx");

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
            this.SaveConditionInf.Click += new EventHandler(this.SaveConditionInf_Click);
        }

        #endregion

    }
}