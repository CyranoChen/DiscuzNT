using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using Discuz.Cache;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 添加第一个分类页
    /// 说明: 当论坛版块表中没有记录时,则会运行该页面
    /// </summary>
    public partial class addfirstforum : AdminPage
    {
        public ForumInfo forumInfo = new ForumInfo();

        public void InitInfo()
        {
            #region 加载初始化信息
            //绑定模板
            templateid.AddTableData(Templates.GetValidTemplateList(),"templatetitle","templateid");
            //绑定用户组
            DataTable dt = UserGroups.GetUserGroupForDataTable();
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell td = new HtmlTableCell("td");
                if (i % 2 == 1)
                    td.Attributes.Add("class", "td_alternating_item1");
                else
                    td.Attributes.Add("class", "td_alternating_item2");
                td.Controls.Add(new LiteralControl("<input type='checkbox' id='r" + i + "' onclick='selectRow(" + i + ",this.checked)'>"));
                tr.Cells.Add(td);
                td = new HtmlTableCell("td");
                if (i % 2 == 1)
                    td.Attributes.Add("class", "td_alternating_item1");
                else
                    td.Attributes.Add("class", "td_alternating_item2");
                td.Controls.Add(new LiteralControl("<label for='r" + i + "'>" + dr["grouptitle"].ToString() + "</lable>"));
                tr.Cells.Add(td);
                tr.Cells.Add(GetTD("viewperm", dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("postperm", dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("replyperm", dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("getattachperm", dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("postattachperm", dr["groupid"].ToString(), i));
                powerset.Rows.Add(tr);
                i++;
            }
            //绑定附件类型
            dt = Attachments.GetAttachmentType();
            attachextensions.AddTableData(dt);

            showclose.Attributes.Add("style", "display:none");
            autocloseoption.SelectedIndex = 0;
			autocloseoption.Attributes.Add("onclick","javascript:document.getElementById('" + showclose.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage2_autocloseoption_0').checked ? 'none' : 'block');");

            #endregion
        }

        private HtmlTableCell GetTD(string strPerfix, string groupId, int ctlId)
        {
            #region 生成组权限控制项

            string strTd = "<input type='checkbox' name='" + strPerfix + "' id='" + strPerfix + ctlId + "' value='" + groupId + "'>";
            HtmlTableCell td = new HtmlTableCell("td");
            if (ctlId % 2 == 1)
                td.Attributes.Add("class", "td_alternating_item1");
            else
                td.Attributes.Add("class", "td_alternating_item2");
            td.Controls.Add(new LiteralControl(strTd));
            return td;

            #endregion
        }


        public int BoolToInt(bool a)
        {
            return a ? 1 : 0;
        }


        private void SubmitSame_Click(object sender, EventArgs e)
        {
            if (this.CheckCookie())
            {
                InsertForum("0", "0", "0", "0", "1");
            }
        }

        /// <summary>
        /// 插入论坛版块
        /// </summary>
        /// <param name="parentid"></param>
        /// <param name="layer"></param>
        /// <param name="parentidlist"></param>
        /// <param name="subforumcount"></param>
        /// <param name="systemdisplayorder"></param>
        public void InsertForum(string parentid, string layer, string parentidlist, string subforumcount, string systemdisplayorder)
        {
            #region 插入论坛版块记录

            if (rewritename.Text.Trim() != "" && Discuz.Forum.Forums.CheckRewriteNameInvalid(rewritename.Text.Trim()))
            {
                rewritename.Text = "";
                base.RegisterStartupScript("", "<script>alert('URL重写非法!');</script>");
                return;
            }
            forumInfo.Parentid = Convert.ToInt32(parentid);
            forumInfo.Layer = Convert.ToInt32(layer);
            forumInfo.Parentidlist = parentidlist;
            forumInfo.Subforumcount = Convert.ToInt32(subforumcount);
            forumInfo.Name = name.Text.Trim();
            forumInfo.Status = Convert.ToInt32(status.SelectedValue);
            forumInfo.Colcount = 1;
            forumInfo.Displayorder = Convert.ToInt32(systemdisplayorder);
            forumInfo.Templateid = Convert.ToInt32(templateid.SelectedValue);
            forumInfo.Allowhtml = 0;
            forumInfo.Allowblog = 0;
            forumInfo.Istrade = 0;

            forumInfo.Alloweditrules = 0;
            forumInfo.Allowsmilies = BoolToInt(setting.Items[0].Selected);
            forumInfo.Allowrss = BoolToInt(setting.Items[1].Selected);
            forumInfo.Allowbbcode = BoolToInt(setting.Items[2].Selected);
            forumInfo.Allowimgcode = BoolToInt(setting.Items[3].Selected);
            forumInfo.Recyclebin = BoolToInt(setting.Items[4].Selected);
            forumInfo.Modnewposts = BoolToInt(setting.Items[5].Selected);
            forumInfo.Modnewtopics = BoolToInt(setting.Items[6].Selected);
            forumInfo.Jammer = BoolToInt(setting.Items[7].Selected);
            forumInfo.Disablewatermark = BoolToInt(setting.Items[8].Selected);
            forumInfo.Inheritedmod = BoolToInt(setting.Items[9].Selected);
            forumInfo.Allowthumbnail = BoolToInt(setting.Items[10].Selected);
            forumInfo.Allowtag = BoolToInt(setting.Items[11].Selected);
            forumInfo.Istrade = 0;
            int temppostspecial = 0;
            temppostspecial = setting.Items[12].Selected ? temppostspecial | 1 : temppostspecial & ~1;
            temppostspecial = setting.Items[13].Selected ? temppostspecial | 16 : temppostspecial & ~16;
            temppostspecial = setting.Items[14].Selected ? temppostspecial | 4 : temppostspecial & ~4;
            forumInfo.Allowpostspecial = temppostspecial;
            forumInfo.Alloweditrules = BoolToInt(setting.Items[15].Selected);
            forumInfo.Allowspecialonly = Convert.ToInt16(allowspecialonly.SelectedValue);
            forumInfo.Autoclose = autocloseoption.SelectedValue == "0" ? 0 : Convert.ToInt32(autocloseday.Text);
            forumInfo.Description = description.Text;
            forumInfo.Password = password.Text;
            forumInfo.Icon = icon.Text;
            forumInfo.Postcredits = "";
            forumInfo.Replycredits = "";
            forumInfo.Redirect = redirect.Text;
            forumInfo.Attachextensions = attachextensions.GetSelectString(",");
            forumInfo.Moderators = moderators.Text;
            forumInfo.Rules = rules.Text;
            forumInfo.Seokeywords = seokeywords.Text.Trim();
            forumInfo.Seodescription = seodescription.Text.Trim();
            forumInfo.Rewritename = rewritename.Text.Trim();
            forumInfo.Topictypes = topictypes.Text;
            forumInfo.Viewperm = Request.Form["viewperm"];
            forumInfo.Postperm = Request.Form["postperm"];
            forumInfo.Replyperm = Request.Form["replyperm"];
            forumInfo.Getattachperm = Request.Form["getattachperm"];
            forumInfo.Postattachperm = Request.Form["postattachperm"];

            string result;
            AdminForums.CreateForums(forumInfo,out result,userid,username,usergroupid,grouptitle,ip);

            if (result == "")
                base.RegisterStartupScript( "PAGE", "window.location.href='forum_ForumsTree.aspx';");
            else
                base.RegisterStartupScript( "PAGE", "alert('用户:" + result + "不存在,因为无法设为版主');window.location.href='forum_ForumsTree.aspx';");

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
            this.TabControl1.InitTabPage();
            TabControl1.Items.Remove(tabPage22);
            tabPage22.Visible = false;
           
            this.Submit.Click += new EventHandler(this.SubmitSame_Click);
            InitInfo();
        }

        #endregion

    }
}