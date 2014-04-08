using System;
using System.Data;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 查找要审核的主题 
    /// </summary>
    
    public partial class auditingtopic : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            forumid.BuildTree(Forums.GetForumListForDataTable(),"name","fid");
        }

        public void SearchTopicAudit_Click(object sender, EventArgs e)
        {
            #region 设置查询条件并跳转到audittopicgrid.aspx 进行显示
            if (this.CheckCookie())
            {
                string sqlstring = Posts.GetTopicAuditCondition(Utils.StrToInt(forumid.SelectedValue, 0), poster.Text, title.Text,
                    moderatorname.Text, postdatetimeStart.SelectedDate,postdatetimeEnd.SelectedDate, deldatetimeStart.SelectedDate, deldatetimeEnd.SelectedDate);
                Session["audittopicswhere"] = sqlstring;
                Response.Redirect("forum_audittopicgrid.aspx");
            }
            #endregion
        }        

        public void DeleteRecycle_Click(object sender, EventArgs e)
        {
            #region 回收站主题删除
            if (this.CheckCookie())
            {
                Topics.DeleteRecycleTopic(Convert.ToInt32(RecycleDay.Text));
                //string topiclist = "";
                //DataTable dt = Topics.GetTidForModeratorManagelogByPostdatetime(DateTime.Now.AddDays(-Convert.ToInt32(RecycleDay.Text)));
                //if (dt.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        topiclist += dr["tid"].ToString() + ",";
                //    }
                //    TopicAdmins.DeleteTopics(topiclist.Trim(','), 0, false);
                //}
                base.RegisterStartupScript( "PAGE","window.location.href='forum_auditingtopic.aspx';");
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
            this.SearchTopicAudit.Click += new EventHandler(this.SearchTopicAudit_Click);
            this.DeleteRecycle.Click += new EventHandler(this.DeleteRecycle_Click);
        }

        #endregion

    }
}