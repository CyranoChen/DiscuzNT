using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// audittopicgrid 的摘要说明. 
    /// </summary>
    public partial class audittopicgrid : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["audittopicswhere"] != null)
                {
                    ViewState["condition"] = Session["audittopicswhere"].ToString();
                }
                else
                {
                    Response.Redirect("forum_auditingtopic.aspx");
                    return ;
                }

                BindData();

                #region 当无记录时,则按钮无效
                if (Posts.IsExistAuditTopic(ViewState["condition"].ToString()))
                {
                    AllAudioPass.Enabled = false;
                    AllDelete.Enabled = false;
                    DeleteSelectTopic.Enabled = false;
                    AudioSelectTopic.Enabled = false;
                }
                #endregion
            }
        }


        public void BindData()
        {
            #region
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.BindData(Topics.GetAuditTopicList(ViewState["condition"].ToString()));
            #endregion
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        private void AudioSelectTopic_Click(object sender, EventArgs e)
        {
            #region 恢复选中的帖子

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("tid") != "")
                {
                    Discuz.Forum.TopicAdmins.RestoreTopics(DNTRequest.GetString("tid"));
                    base.RegisterStartupScript( "", "<script>alert('提交成功');window.location.href='forum_auditingtopic.aspx';</script>");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选择任何主题!');window.location.href='forum_auditingtopic.aspx';</script>");
                }
            }

            #endregion
        }

        private void AllAudioPass_Click(object sender, EventArgs e)
        {
            #region 全部恢复

            if (this.CheckCookie())
            {
                //彻底删除
                string topiclist = "";
                DataTable dt = Topics.GetAuditTopicList(ViewState["condition"].ToString());
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        topiclist += dr["tid"].ToString() + ",";
                    }
                    TopicAdmins.RestoreTopics(topiclist.TrimEnd(','));
                }
                base.RegisterStartupScript( "", "<script>alert('提交成功');window.location.href='forum_auditingtopic.aspx';</script>");
            }

            #endregion
        }

        private void DeleteSelectTopic_Click(object sender, EventArgs e)
        {
            #region 删除选中的主题

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("tid") != "")
                {
                    Discuz.Forum.TopicAdmins.DeleteTopics(DNTRequest.GetString("tid"), false);
                    this.RegisterStartupScript( "", "<script>alert('提交成功');window.location.href='forum_auditingtopic.aspx';</script>");
                }
                else
                {
                    this.RegisterStartupScript( "", "<script>alert('您未选择任何主题!');window.location.href='forum_auditingtopic.aspx';</script>");
                }
            }

            #endregion
        }

        private void AllDelete_Click(object sender, EventArgs e)
        {
            #region 全部删除

            if (this.CheckCookie())
            {
                string topiclist = "";
                DataTable dt = Topics.GetAuditTopicList(ViewState["condition"].ToString());
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        topiclist += dr["tid"].ToString() + ",";
                    }
                    TopicAdmins.DeleteTopics(topiclist.TrimEnd(','), false);
                }
                this.RegisterStartupScript("", "<script>alert('提交成功');window.location.href='forum_auditingtopic.aspx';</script>");
            }

            #endregion
        }


        public string BoolStr(string closed)
        {
            #region 将逻辑值转换为图片
            if (closed == "1")
            {
                return "<div align=center><img src=../images/OK.gif /></div>";
            }
            else
            {
                return "<div align=center><img src=../images/Cancel.gif /></div>";
            }
            #endregion
        }

        public string GetPostLink(string tid, string replies)
        {
            return "<a href=forum/forum_postgrid.aspx?tid=" + tid + ">" + replies + "</a>";
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AudioSelectTopic.Click += new EventHandler(this.AudioSelectTopic_Click);
            this.AllAudioPass.Click += new EventHandler(this.AllAudioPass_Click);
            this.DeleteSelectTopic.Click += new EventHandler(this.DeleteSelectTopic_Click);
            this.AllDelete.Click += new EventHandler(this.AllDelete_Click);

            DataGrid1.DataKeyField = "tid";
            DataGrid1.TableHeaderName = "主题列表";
            DataGrid1.ColumnSpan = 11;
        }

        #endregion

    }
}