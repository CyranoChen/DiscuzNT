using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 搜索主题列表
    /// </summary>
    public partial class topicsgrid : AdminPage
    {
        public string condition = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["topicswhere"] != null)
                {
                    condition = Session["topicswhere"].ToString();
                }
                else
                {
                    Response.Redirect("forum_seachtopic.aspx");
                    return;
                }
                BindData();
            }
            //绑定数据到控件
            forumid.BuildTree(Forums.GetForumListForDataTable(),"name","fid");
        }

        public void BindData()
        {
            #region
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.BindData(Topics.GetTopicsByCondition(condition));
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

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 数据绑定显示长度控制

            if (e.Item.Cells[2].Text.ToString().Length > 15)
            {
                e.Item.Cells[2].Text = e.Item.Cells[2].Text.Substring(0, 15) + "…";
            }

            #endregion
        }


        private void SetTopicInfo_Click(object sender, EventArgs e)
        {
            #region 按选定的操作管理主题

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("tid") != "")
                {
                    string tidlist = DNTRequest.GetString("tid");
                    switch (DNTRequest.GetString("operation"))
                    {
                        case "moveforum":
                            {
                                if (forumid.SelectedValue != "0")
                                {
                                    AdminTopics.BatchMoveTopics(tidlist, Convert.ToInt16(forumid.SelectedValue), userid, username, usergroupid, grouptitle, ip);
                                    ////先找出当前主题列表中所属的FID
                                    //foreach (DataRow olddr in Topics.GetTopicFidByTid(tidlist).Rows)
                                    //{
                                    //    string oldtidlist = "0";
                                    //    //以FID和列表为条件列出在当前FID下的主题列表
                                    //    foreach (DataRow mydr in Topics.GetTopicTidByFid(tidlist, int.Parse(olddr["fid"].ToString())).Rows)
                                    //    {
                                    //        oldtidlist += "," + mydr["tid"].ToString();
                                    //    }
                                    //    //调用前台操作函数
                                    //    TopicAdmins.MoveTopics(oldtidlist, Convert.ToInt16(forumid.SelectedValue), Convert.ToInt16(olddr["fid"].ToString()));
                                    //}
                                    //AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "批量移动主题", "主题ID:" + tidlist + " <br />目标论坛fid:" + forumid.SelectedValue);

                                }
                                break;
                            }
                        case "movetype":
                            {
                                if (typeid.SelectedValue != "0")
                                {
                                    AdminTopics.SetTypeid(tidlist, Convert.ToInt16(typeid.SelectedValue));
                                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "批量修改主题类型", "主题ID:" + tidlist + " <br />类型tid:" + typeid.SelectedValue);
                                }
                                break;
                            }
                        case "delete":
                            {
                                AdminTopics.BatchDeleteTopics(tidlist, !nodeletepostnum.Checked, userid, username, usergroupid, grouptitle, ip);
                                //if (nodeletepostnum.Checked)
                                //{
                                //    Discuz.Forum.TopicAdmins.DeleteTopics(tidlist, 0, false);
                                //}
                                //else
                                //{
                                //    Discuz.Forum.TopicAdmins.DeleteTopics(tidlist, 1, false);
                                //}
                                //Discuz.Forum.Attachments.UpdateTopicAttachment(tidlist);
                                //AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "批量删除主题", "主题ID:" + tidlist);
                                break;
                            }
                        case "displayorder":
                            {
                                AdminTopics.BatchChangeTopicsDisplayOrderLevel(tidlist, Convert.ToInt32(DNTRequest.GetString("displayorder_level")), userid, username, usergroupid, grouptitle, ip);
                                //AdminTopics.SetDisplayorder(tidlist, Convert.ToInt16(DNTRequest.GetString("displayorder_level")));
                                //AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "批量置顶主题", "主题ID:" + tidlist + "<br /> 置顶级为:" + DNTRequest.GetString("displayorder_level"));
                                break;
                            }
                        case "adddigest":
                            {
                                AdminTopics.BatchChangeTopicsDigest(tidlist,Convert.ToInt32(DNTRequest.GetString("digest_level")), userid, username, usergroupid, grouptitle, ip);
                                //Discuz.Forum.TopicAdmins.SetDigest(DNTRequest.GetString("tid").Replace("0 ", ""), (short)Convert.ToInt16(DNTRequest.GetString("digest_level")));
                                //AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "批量加精主题", "主题ID:" + tidlist + "<br /> 加精级为:" + DNTRequest.GetString("digest_level"));
                                break;
                            }
                        case "deleteattach":
                            {
                                AdminTopics.BatchDeleteTopicAttachs(tidlist, userid, username, usergroupid, grouptitle, ip);
                                //AdminTopicOperations.DeleteAttachmentByTid(DNTRequest.GetString("tid").Replace("0 ", ""));
                                //AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除主题中的附件", "主题ID:" + tidlist);
                                break;
                            }
                    }
                    base.RegisterStartupScript("PAGE", "window.location.href='forum_topicsgrid.aspx';");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('请选择相应的主题!');window.location.href='forum_topicsgrid.aspx';</script>");
                }
            }

            #endregion
        }


        public string BoolStr(string closed)
        {
            return closed == "1" ? "<div align=center><img src=../images/OK.gif /></div>" : "<div align=center><img src=../images/Cancel.gif /></div>";
        }

        public string GetPostLink(string tid, string replies)
        {
            return "<a href=forum_postgrid.aspx?tid=" + tid + ">" + replies + "</a>";

        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SetTopicInfo.Click += new EventHandler(this.SetTopicInfo_Click);
            DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);

            DataGrid1.TableHeaderName = "主题列表";
            DataGrid1.ColumnSpan = 11;
            DataGrid1.DataKeyField = "tid";
        }

        #endregion
    }
}