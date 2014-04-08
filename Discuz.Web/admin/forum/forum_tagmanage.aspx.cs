using System;
using System.Data;
using System.Data.Common;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.IO;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 审核用户
    /// </summary>
    public partial class forum_tagmanage : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "标签列表";
            DataGrid1.DataKeyField = "tagid";
            DataGrid1.BindData(Tags.GetForumTags("",Convert.ToInt32(radstatus.SelectedValue)));
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }


        protected void savetags_Click(object sender, EventArgs e)
        {
            #region 保存标签修改
            int row = 0;
            bool error = false;
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                int id = int.Parse(o.ToString());
                string orderid = DataGrid1.GetControlValue(row, "orderid").Trim();
                string color = DataGrid1.GetControlValue(row, "color").Trim().ToUpper();
                if (!Tags.UpdateForumTags(id, int.Parse(orderid), color))
                {
                    error = true;
                    continue;
                }
                //Regex r = new Regex("^#?([0-9|A-F]){6}$");
                //if (orderid == "" || !Utils.IsNumeric(orderid) || (color != "" && !r.IsMatch(color)))
                //{
                //    error = true;
                //    continue;
                //}
                //Tags.UpdateForumTags(id,int.Parse(orderid),color.Replace("#",""));
                row++;
            }
            Topics.NeatenRelateTopics();
            WriteTagsStatus();
            if (error)
                base.RegisterStartupScript("PAGE", "alert('某些记录输入错误，未能被更新！');window.location.href='forum_tagmanage.aspx';");
            else
                base.RegisterStartupScript("PAGE", "window.location.href='forum_tagmanage.aspx';");
            #endregion
        }

        private void SelectDelete_Click(object sender, EventArgs e)
        {
            #region 删除选中的用户信息

            if (this.CheckCookie())
            {
                string uidlist = DNTRequest.GetString("uid");
                if (uidlist != "")
                {
                    Users.DeleteUsers(uidlist);
                    base.RegisterStartupScript("PAGE", "window.location='forum_audituser.aspx';");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('请选择相应的用户!');window.location='forum_audituser.aspx';</script>");
                }
            }

            #endregion
        }

        //private void AllPass_Click(object sender, EventArgs e)
        //{
        //    #region 将用户调整到相应的用户组

        //    if (this.CheckCookie())
        //    {
        //        if (Discuz.Forum.UserCredits.GetCreditsUserGroupId(0) != null)
        //        {
        //            int tmpGroupID = UserCredits.GetCreditsUserGroupId(0).Groupid; //添加注册用户审核机制后需要修改
        //            UserGroups.ChangeAllUserGroupId(8, tmpGroupID);
        //            foreach (DataRow dr in Users.GetUserListByGroupid(8).Rows)
        //            {
        //                Discuz.Forum.UserCredits.UpdateUserCredits(Convert.ToInt32(dr["uid"].ToString()));
        //            }
        //            Users.ClearUsersAuthstrByUncheckedUserGroup();
        //        }
        //        base.RegisterStartupScript("PAGE", "window.location='forum_audituser.aspx';");
        //    }

        //    #endregion
        //}

        private static void WriteTextFile(string filename,string content)
        {
            FileStream fs = new FileStream(Utils.GetMapPath("../../cache/tag/" + filename), FileMode.Create);
            byte[] bt = System.Text.Encoding.UTF8.GetBytes(content);
            fs.Write(bt, 0, bt.Length);
            fs.Close();
        }
        private void WriteTagsStatus()
        {
            string closedtags = "";
            string colorfultags = "";

            //TODO:过于复杂的方法
            DataTable tags = Tags.GetForumTags("",Convert.ToInt32(radstatus.SelectedValue));
            foreach (DataRow dr in tags.Rows)
            {
                if (dr["orderid"].ToString() == "-1")
                {
                    closedtags += "'" + dr["tagid"].ToString() + "',";
                }
                if (dr["color"].ToString().Trim() != "")
                {
                    colorfultags += "'" + dr["tagid"].ToString() + "':{'tagid' : '" + dr["tagid"].ToString() + "', 'color' : '" + dr["color"].ToString() + "'},";
                }
            }
            closedtags = "var closedtags = [" + closedtags.TrimEnd(',') + "];";
            WriteTextFile("closedtags.txt", closedtags);
            colorfultags = "var colorfultags = {" + colorfultags.TrimEnd(',') + "};";
            WriteTextFile("colorfultags.txt", colorfultags);
        }

        protected void searchtag_Click(object sender, System.EventArgs e)
        {
            BindData();
            string tag_name = this.tagname.Text.Trim();
            int from = Utils.StrToInt(txtfrom.Text.Trim(),101);
            int end = Utils.StrToInt(txtend.Text.Trim(),102);

            //三个文本框都为空，返回
            if ((from == 101 && end == 102 && tag_name == "") || (from == 101 && end != 102) || (from != 101 && end == 102))
            {
                return;
            }

            //当名称不为空，范围为空，按名字搜索
            int selected = Convert.ToInt32(this.radstatus.SelectedValue);

            if (tag_name != "" && ((from == 101) && (end == 102)))
            {
                if (this.CheckCookie())
                {
                    DataGrid1.BindData(Tags.GetForumTags(tag_name, selected));
                }
            }
            else
            {
                DataGrid1.BindData(Topics.GetTopicNumber(tag_name, from, end, selected));
            }
        }

        protected void DisableRec_Click(object sender, System.EventArgs e)
        {
            int row = 0;
            string tagid = DNTRequest.GetString("tagid");
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                int id = int.Parse(o.ToString());
                if (("," + tagid + ",").IndexOf("," + id + ",") == -1)
                    continue;
                string color = DataGrid1.GetControlValue(row, "color").Trim().ToUpper();
                Tags.UpdateForumTags(id, -1, color);
                //Regex r = new Regex("^#?([0-9|A-F]){6}$");
                //if (color != "" && !r.IsMatch(color))
                //{
                //    continue;
                //}
                //Tags.UpdateForumTags(id, int.Parse(orderid), color.Replace("#", ""));
                row++;
            }
            Topics.NeatenRelateTopics();
            WriteTagsStatus();
            base.RegisterStartupScript("PAGE", "window.location.href='forum_tagmanage.aspx';");
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 数据绑定显示长度控制

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                System.Web.UI.WebControls.TextBox t = (System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0];
                t.Attributes.Add("maxlength", "3");
                t.Attributes.Add("size", "3");

                t = (System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0];
                t.Attributes.Add("maxlength", "6");
                t.Attributes.Add("size", "6");
            }

            #endregion
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            DataGrid1.DataKeyField = "tagid";
            DataGrid1.TableHeaderName = "审核用户列表";
            DataGrid1.ColumnSpan = 8;
        }

        #endregion
    }
}