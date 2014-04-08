using System;
using System.Data;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ��̳���ϲ�
    /// </summary>
    
    public partial class forumcombination : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sourceforumid.BuildTree(Discuz.Forum.Forums.GetForumListForDataTable(), "name", "fid");
            targetforumid.BuildTree(Discuz.Forum.Forums.GetForumListForDataTable(), "name", "fid");
            
            if (!Page.IsPostBack)
            {
                if (DNTRequest.GetString("fid") != "")
                {
                    sourceforumid.SelectedValue = DNTRequest.GetString("fid");
                }
            }
        }


        private void SaveCombinationInfo_Click(object sender, EventArgs e)
        {
            #region �ϲ���̳���

            if (this.CheckCookie())
            {
                if (sourceforumid.SelectedValue == "0")
                {
                    base.RegisterStartupScript( "", "<script>alert('��ѡ����Ӧ��Դ��̳!');</script>");
                    return;
                }

                if (targetforumid.SelectedValue == "0")
                {
                    base.RegisterStartupScript( "", "<script>alert('��ѡ����Ӧ��Ŀ����̳!');</script>");
                    return;
                }

                ForumInfo forumInfo = Forums.GetForumInfo(Utils.StrToInt(targetforumid.SelectedValue, 0));
                if (forumInfo != null && forumInfo.Parentid == 0 && forumInfo.Layer == 0)
                {
                    base.RegisterStartupScript( "", "<script>alert('����ѡ���Ŀ����̳��\"��̳����\"������\"��̳���\",��˺ϲ���Ч!');</script>");
                    return;
                }

                string result;
                if (!AdminForums.CombinationForums(sourceforumid.SelectedValue, targetforumid.SelectedValue))
                {
                    result = "<script>alert('��ǰ�ڵ��������ӽ��,��˺ϲ���Ч!');window.location.href='forum_forumcombination.aspx';</script>";
                    base.RegisterStartupScript( "", result);
                    return;
                }
                else
                {
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "�ϲ���̳���", "�ϲ���̳���" + sourceforumid.SelectedValue + "��" + targetforumid.SelectedValue);

                    base.RegisterStartupScript( "PAGE", "window.location.href='forum_forumstree.aspx';");
                    return;
                }
            }

            #endregion
        }

        #region Web ������������ɵĴ���

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveCombinationInfo.Click += new EventHandler(this.SaveCombinationInfo_Click);
        }

        #endregion

    }
}