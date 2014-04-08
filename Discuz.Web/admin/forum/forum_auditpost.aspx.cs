using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 审核帖子
    /// </summary>
    public partial class auditpost : AdminPage
    {
        public int pageid = DNTRequest.GetInt("pageid", 1);

        public int pageCount = 0;

        public int postCount = 0;

        public List<PostInfo> auditPostList = new List<PostInfo>();

        public int postTableId = DNTRequest.GetInt("table", Posts.GetMaxPostTableId());


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Posts.GetPostsCount(postlist.SelectedValue) == 0)
                {
                    msg.Visible = true;
                }
                postlist.SelectedValue = postTableId.ToString();
                BindData();
            }
        }

        public void BindData()
        {
            #region 绑定审核帖子
            
            postCount = Posts.GetUnauditNewPostCount("0", TypeConverter.StrToInt(postlist.SelectedValue), 1);
            pageCount = ((postCount - 1) / 20) + 1;
            pageid = (pageid > pageCount) ? pageCount : pageid;
            pageid = pageid < 1 ? 1 : pageid;
            auditPostList = Posts.GetUnauditPost("0", 20, pageid, TypeConverter.StrToInt(postlist.SelectedValue), 1);
            #endregion
        }

        private void postslist_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        public void initPostTable()
        {
            #region 初始化分表控件

            postlist.AutoPostBack = true;

            DataTable dt = Discuz.Forum.Posts.GetAllPostTableName();
            postlist.Items.Clear();
            foreach (DataRow r in dt.Rows)
            {
                postlist.Items.Add(new ListItem(BaseConfigs.GetTablePrefix + "posts" + r[0].ToString(), r[0].ToString()));
            }
            postlist.DataBind();
            postlist.SelectedValue = Discuz.Forum.Posts.GetPostTableId();

            #endregion
        }

        private void SelectPass_Click(object sender, EventArgs e)
        {
            #region 对选中的帖子设置为通过审核

            string idlist = DNTRequest.GetString("pid");
            string pidlist = "";
            string tidlist = "";
            foreach (string doubleid in idlist.Split(','))
            {
                string[] idarray = doubleid.Split('|');
                pidlist += idarray[0] + ",";
                tidlist += idarray[1] + ",";
            }
            pidlist = pidlist.TrimEnd(',');
            tidlist = tidlist.TrimEnd(',');
            if (this.CheckCookie())
            {
                if (pidlist != "")
                {
                    UpdateUserCredits(tidlist, pidlist);
                    Posts.PassPost(int.Parse(postlist.SelectedValue), pidlist);
                    base.RegisterStartupScript( "PAGE", "window.location.href='forum_auditpost.aspx';");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');window.location.href='forum_auditpost.aspx';</script>");
                }
            }

            #endregion
        }

        /// <summary>
        /// 更新用户积分
        /// </summary>
        /// <param name="tidlist">审核主题id</param>
        /// <param name="pidlist">通过审核帖子的Pid列表</param>
        private void UpdateUserCredits(string tidlist, string pidlist)
        {
            string[] tidarray = tidlist.Split(',');
            string[] pidarray = pidlist.Split(',');
            float[] values = null;
            ForumInfo forum = null;
            PostInfo post = null;
            int fid = -1;
            for(int i = 0; i < pidarray.Length; i++)
            {
                //Topics.get
                post = Discuz.Forum.Posts.GetPostInfo(int.Parse(tidarray[i]), int.Parse(pidarray[i]));  //获取帖子信息
                if (fid != post.Fid)    //当上一个和当前主题不在一个版块内时，重新读取版块的积分设置
                {
                    fid = post.Fid;
                    forum = Discuz.Forum.Forums.GetForumInfo(fid);
                    if (!forum.Replycredits.Equals(""))
                    {
                        int index = 0;
                        float tempval = 0;
                        values = new float[8];
                        foreach (string ext in Utils.SplitString(forum.Replycredits, ","))
                        {

                            if (index == 0)
                            {
                                if (!ext.Equals("True"))
                                {
                                    values = null;
                                    break;
                                }
                                index++;
                                continue;
                            }
                            tempval = Utils.StrToFloat(ext, 0.0f);
                            values[index - 1] = tempval;
                            index++;
                            if (index > 8)
                            {
                                break;
                            }
                        }
                    }
                }

                if (values != null)
                {
                    ///使用版块内积分
                    Discuz.Forum.UserCredits.UpdateUserExtCredits(post.Posterid, values, false);
                    Discuz.Forum.UserCredits.UpdateUserCredits(post.Posterid);                  
                }
                else
                {
                    ///使用默认积分
                    Discuz.Forum.UserCredits.UpdateUserCreditsByPosts(post.Posterid);
                }
            }
        }

        private void SelectDelete_Click(object sender, EventArgs e)
        {
            #region 对选中的帖子进行删除

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("pid") != "")
                {
                    Posts.GetPostLayer(int.Parse(postlist.SelectedValue));
                    base.RegisterStartupScript( "PAGE", "window.location.href='forum_auditpost.aspx';");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');window.location.href='forum_auditpost.aspx';</script>");
                }
            }

            #endregion
        }

        protected string GetPostStatus(string invisible)
        {
            if (invisible == "1")
                return "未审核";
            if (invisible == "-3")
                return "忽略";
            return invisible;
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }


        private void InitializeComponent()
        {
            this.postlist.SelectedIndexChanged += new EventHandler(this.postslist_SelectedIndexChanged);
            this.SelectPass.Click += new EventHandler(this.SelectPass_Click);
            this.SelectDelete.Click += new EventHandler(this.SelectDelete_Click);
            initPostTable();
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
                    str += string.Format("<a style=\"border:1px solid #E8E8E8;padding:2px 4px;margin-right:2px;\" href=\"forum_auditpost.aspx?pageid={0}&table={1}\">{0}</a>", i, postlist.SelectedValue);
                else
                    str += string.Format("<span style=\"border:1px solid #E8E8E8;padding:2px 4px;margin-right:2px;background:#09C;color:#FFF\" >{0}</span> ", i);
            }
            return str;
        }

        #endregion

    }
}