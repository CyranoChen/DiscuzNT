using System;
using System.Web.UI.WebControls;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common;



namespace Discuz.Web.Admin
{
    /// <summary>
    /// 附件搜索
    /// </summary>
    public partial class searchattchment : AdminPage
    {
        private void SaveSearchCondition_Click(object sender, EventArgs e)
        {
            #region 生成查询条件

            if (this.CheckCookie())
            {
                Session["attchmentwhere"] = Attachments.SearchAttachment(Utils.StrToInt(forumid.SelectedValue, 0), Discuz.Forum.Posts.GetPostTableName(), filesizemin.Text, filesizemax.Text, downloadsmin.Text, downloadsmax.Text, postdatetime.Text, filename.Text, description.Text, poster.Text);
                Response.Redirect("forum_attchemntgrid.aspx");
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
            forumid.BuildTree(Forums.GetForumListForDataTable(),"name","fid");
            forumid.TypeID.Items.RemoveAt(0);
            forumid.TypeID.Items.Insert(0, new ListItem("全部", "0"));
        }

        #endregion

    }
}