using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{

    /// <summary>
    ///	�����ֱ�ؼ�
    /// </summary>

    public partial class DropDownPost : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                #region ��ʼ���ֱ�ؼ�

                postslist.Items.Clear();
                foreach (DataRow r in Posts.GetPostTableList().Rows)
                {
                    postslist.Items.Add(new ListItem(BaseConfigs.GetTablePrefix + "posts" + r["id"].ToString(), r["id"].ToString()));
                }
                postslist.DataBind();
                postslist.SelectedValue = Forum.Posts.GetPostTableId();

                #endregion
            }
        }

        public string SelectedValue
        {
            get { return postslist.SelectedValue; }
            set { postslist.SelectedValue = value; }
        }
    }
}