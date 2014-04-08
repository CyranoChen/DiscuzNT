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
    /// ��������
    /// </summary>
    public partial class searchattchment : AdminPage
    {
        private void SaveSearchCondition_Click(object sender, EventArgs e)
        {
            #region ���ɲ�ѯ����

            if (this.CheckCookie())
            {
                Session["attchmentwhere"] = Attachments.SearchAttachment(Utils.StrToInt(forumid.SelectedValue, 0), Discuz.Forum.Posts.GetPostTableName(), filesizemin.Text, filesizemax.Text, downloadsmin.Text, downloadsmax.Text, postdatetime.Text, filename.Text, description.Text, poster.Text);
                Response.Redirect("forum_attchemntgrid.aspx");
            }

            #endregion
        }

        #region ��VIEWSTATEд������

        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            base.DiscuzForumSavePageState(viewState);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            return base.DiscuzForumLoadPageState();
        }

        #endregion

        #region Web ������������ɵĴ���

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
            forumid.TypeID.Items.Insert(0, new ListItem("ȫ��", "0"));
        }

        #endregion

    }
}