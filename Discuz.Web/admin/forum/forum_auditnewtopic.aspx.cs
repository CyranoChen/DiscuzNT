using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Entity;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 审核新主题 
    /// </summary>

    public partial class auditnewtopic : AdminPage
    {
        public int pageid = DNTRequest.GetInt("pageid", 1);

        public List<TopicInfo> auditTopicList = new List<TopicInfo>();

        public int pageCount = 0;

        public int topicCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            #region 绑定审核主题
            
            topicCount = Topics.GetUnauditNewTopicCount("0", -2);
            pageCount = ((topicCount - 1) / 20) + 1;
            pageid = (pageid > pageCount) ? pageCount : pageid;
            pageid = pageid < 1 ? 1 : pageid;
            auditTopicList = Topics.GetUnauditNewTopic("0", 20, pageid, -2);
            #endregion
        }

        private void SelectPass_Click(object sender, EventArgs e)
        {
            #region 对选中的主题设置为通过审核

            if (this.CheckCookie())
            {
                string tidlist = DNTRequest.GetString("tid");
                if (tidlist != "")
                {
                    Topics.PassAuditNewTopic(tidlist);
                    base.RegisterStartupScript("", "<script>window.location='forum_auditnewtopic.aspx';</script>");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location='forum_auditnewtopic.aspx';</script>");
                }
            }

            #endregion
        }

        private void SelectDelete_Click(object sender, EventArgs e)
        {
            #region 对选中的主题进行删除

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("tid") != "")
                {
                    Discuz.Forum.TopicAdmins.DeleteTopicsWithoutChangingCredits(DNTRequest.GetString("tid"), false);
                    base.RegisterStartupScript("",  "<script>window.location='forum_auditnewtopic.aspx';</script>");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location='forum_auditnewtopic.aspx';</script>");
                }
            }

            #endregion
        }

        public string GetTopicType(string topicType)
        {
            switch(topicType)
            {
                case "0":
                    return "普通主题";
                case "1":
                    return "投票帖";
                case "2":
                case "3":
                    return "悬赏帖";
                case "4":
                    return "辩论帖";
                default:
                    return topicType;
            }
        }

        protected string GetTopicStatus(string displayOrder)
        {
            //>0为置顶,<0不显示,==0正常   -1为回收站   -2待审核 -3为被忽略
            int order = int.Parse(displayOrder);
            if (order > 0)
                return "置顶";
            if (order == 0)
                return "正常";
            if (order == -1)
                return "回收站";
            if (order == -2)
                return "待审核";
            if (order == -3)
                return "被忽略";
            return displayOrder;
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SelectPass.Click += new EventHandler(this.SelectPass_Click);
            this.SelectDelete.Click += new EventHandler(this.SelectDelete_Click);
        }

        /// <summary>
        /// 页码生成
        /// </summary>
        /// <returns></returns>
        public string ShowPageIndex()
        {
            string str = "";
            int startIndex = pageid - 5 > 0 ? (pageid - 5 - (pageid + 5 < pageCount ? 0 : (pageid + 5 - pageCount))) : 1;
            int lastIndex = pageid + 5 < pageCount ? pageid + 5 + (pageid - 5 > 0 ? 0 : ((pageid - 5) * -1) + 1) : pageCount;
            for (int i = startIndex; i <= lastIndex; i++)
            {
                if (i != pageid)
                    str += string.Format("<a style=\"border:1px solid #E8E8E8;padding:2px 4px;margin-right:2px;\" href=\"forum_auditnewtopic.aspx?pageid={0}\">{0}</a>", i);
                else
                    str += string.Format("<span style=\"border:1px solid #E8E8E8;padding:2px 4px;margin-right:2px;background:#09C;color:#FFF\" >{0}</span> ", i);
            }
            return str;
        }

        #endregion
    }
}