using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Discuz.Cache;
using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using System.Web;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 添加版块
    /// </summary>
    public partial class addforums : AdminPage
    {
        public ForumInfo forumInfo = new ForumInfo();
        protected string root = Utils.GetRootUrl(BaseConfigs.GetBaseConfig().Forumpath);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //如果版块表中没有任何版块, 则跳转到"添加第一个版块"页面. 
                if (Forums.GetForumList().Count == 0)
                {
                    Server.Transfer("forum_AddFirstForum.aspx");
                }
            }
        }

        public void InitInfo()
        {
            #region 初始化信息绑定
            targetforumid.BuildTree(Forums.GetForumListForDataTable(), "name", "fid");
            templateid.AddTableData(Templates.GetValidTemplateList(), "name", "templateid");

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
                td.Controls.Add(new LiteralControl("<input type='checkbox' id='r" + i + "' onclick='selectRow(" + i + ",this.checked)'><label for='r" + i + "'>" + dr["grouptitle"].ToString() + "</lable>"));
                tr.Cells.Add(td);
                tr.Cells.Add(GetTD("viewperm", dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("postperm", dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("replyperm", dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("getattachperm", dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("postattachperm", dr["groupid"].ToString(), i));
                powerset.Rows.Add(tr);
                i++;
            }

            dt = Attachments.GetAttachmentType();
            attachextensions.AddTableData(dt);


            if (DNTRequest.GetString("fid") != "")
            {
                targetforumid.SelectedValue = DNTRequest.GetString("fid");
                addtype.SelectedValue = "1";
                targetforumid.Visible = true;
            }

            showcolnum.Attributes.Add("style", "display:none");
            colcount.SelectedIndex = 0;
            colcount.Attributes.Add("onclick", "javascript:document.getElementById('" + showcolnum.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage51_colcount_0').checked ? 'none' : 'block');");

            showclose.Attributes.Add("style", "display:none");
            autocloseoption.SelectedIndex = 0;

            showtargetforum.Attributes.Add("style", "display:block");
            addtype.Attributes.Add("onclick", "javascript:document.getElementById('" + showtargetforum.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage51_addtype_0').checked ? 'none' : 'block');setColDisplayer(document.getElementById('TabControl1_tabPage51_addtype_0').checked);");
            autocloseoption.Attributes.Add("onclick", "javascript:document.getElementById('" + showclose.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage22_autocloseoption_0').checked ? 'none' : 'block');");

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


        private void SubmitSameAfter()
        {
            if (this.CheckCookie())
            {
                int maxdisplayorder = TypeConverter.ObjectToInt(Forums.GetForumListForDataTable().Compute("Max(displayorder)", ""));
                InsertForum("0", "0", "0", "0", maxdisplayorder.ToString());
            }
        }


        public string SetAfterDisplayOrder(int currentdisplayorder)
        {
            #region 在当前节点之后加入同级论坛时的displayorder字段值

            Forums.UpdateFourmsDisplayOrder(currentdisplayorder);
            return Convert.ToString(currentdisplayorder + 1);

            #endregion
        }

        private void SubmitAddChild()
        {
            #region 保存新增论坛信息
            if (this.CheckCookie())
            {
                if (name.Text.Trim() == "")
                {
                    base.RegisterStartupScript("", "<script>alert('论坛名称不能为空');</script>");
                    return;
                }
                if (rewritename.Text.Trim() != "" && Discuz.Forum.Forums.CheckRewriteNameInvalid(rewritename.Text.Trim()))
                {
                    rewritename.Text = "";
                    base.RegisterStartupScript("", "<script>alert('URL重写非法!');</script>");
                    return;
                }
                if (Convert.ToInt16(colcountnumber.Text) < 1 || Convert.ToInt16(colcountnumber.Text) > 9)
                {
                    base.RegisterStartupScript("", "<script>alert('列值必须在2~9范围内');</script>");
                    return;
                }
                if (targetforumid.SelectedValue != "0")
                {
                    #region 添加与当前论坛同级的论坛

                    //添加与当前论坛同级的论坛
                    ForumInfo forumInfo = Forums.GetForumInfo(Utils.StrToInt(targetforumid.SelectedValue, 0));

                    //找出当前要插入的记录所用的FID
                    string parentidlist = null;
                    if (forumInfo.Parentidlist == "0")
                    {
                        parentidlist = forumInfo.Fid.ToString();
                    }
                    else
                    {
                        parentidlist = forumInfo.Parentidlist + "," + forumInfo.Fid;
                    }

                    int maxdisplayorder = 0;

                    DataTable dt = Forums.GetForumList(Utils.StrToInt(targetforumid.SelectedValue, 0));
                    if (dt.Rows.Count > 0)
                    {
                        maxdisplayorder = TypeConverter.ObjectToInt(dt.Compute("Max(displayorder)", ""));
                    }
                    else
                    {
                        maxdisplayorder = forumInfo.Displayorder;
                    }

                    InsertForum(forumInfo.Fid.ToString(),
                                (forumInfo.Layer + 1).ToString(),
                                parentidlist,
                                "0",
                                SetAfterDisplayOrder(maxdisplayorder));

                    Forums.UpdateSubForumCount(forumInfo.Fid);

                    #endregion
                }
                else
                {
                    #region 按根论坛插入

                    int maxdisplayorder = TypeConverter.ObjectToInt(Forums.GetForumListForDataTable().Compute("Max(displayorder)", "")) + 1;
                    InsertForum("0", "0", "0", "0", maxdisplayorder.ToString());

                    #endregion
                }
            }
            #endregion
        }


        private void SubmitAdd_Click(object sender, EventArgs e)
        {
            if (addtype.SelectedValue == "0")
            {
                SubmitSameAfter();
            }
            else
            {
                if (targetforumid.SelectedValue == "0")
                {
                    base.RegisterStartupScript("", "<script>alert('请选择所属论坛版块');</script>");
                    return;
                }
                SubmitAddChild();
            }
        }

        public void InsertForum(string parentid, string layer, string parentidlist, string subforumcount, string systemdisplayorder)
        {
            #region 添加新论坛
            forumInfo.Parentid = Convert.ToInt32(parentid);
            forumInfo.Layer = Convert.ToInt32(layer);
            forumInfo.Parentidlist = parentidlist;
            forumInfo.Subforumcount = Convert.ToInt32(subforumcount);
            forumInfo.Name = name.Text.Trim();
            forumInfo.Status = Convert.ToInt16(status.SelectedValue);
            forumInfo.Displayorder = Convert.ToInt32(systemdisplayorder);
            forumInfo.Templateid = Convert.ToInt32(templateid.SelectedValue);
            forumInfo.Allowsmilies = BoolToInt(setting.Items[0].Selected);
            forumInfo.Allowrss = BoolToInt(setting.Items[1].Selected);
            forumInfo.Allowhtml = 0;
            forumInfo.Allowbbcode = BoolToInt(setting.Items[2].Selected);
            forumInfo.Allowimgcode = BoolToInt(setting.Items[3].Selected);
            forumInfo.Allowblog = 0;
            forumInfo.Istrade = 0;
            forumInfo.Alloweditrules = 0;
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
            forumInfo.Colcount = colcount.SelectedValue == "1" ? 1 : Convert.ToInt16(colcountnumber.Text);   //传统模式[默认]
            forumInfo.Viewperm = Request.Form["viewperm"];
            forumInfo.Postperm = Request.Form["postperm"];
            forumInfo.Replyperm = Request.Form["replyperm"];
            forumInfo.Getattachperm = Request.Form["getattachperm"];
            forumInfo.Postattachperm = Request.Form["postattachperm"];
            string result;
            int fid = AdminForums.CreateForums(forumInfo, out result, userid, username, usergroupid, grouptitle, ip);

            //如果有上传版块图片的操作
            if (HttpContext.Current.Request.Files.Count > 0 && !string.IsNullOrEmpty(HttpContext.Current.Request.Files[0].FileName))
            {
                forumInfo = Forums.GetForumInfo(fid);
                forumInfo.Icon = AdminForums.UploadForumIcon(forumInfo.Fid);
                AdminForums.UpdateForumInfo(forumInfo).Replace("'", "’");
                ForumOperator.RefreshForumCache();
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                configInfo.Specifytemplate = Forums.GetSpecifyForumTemplateCount() > 0 ? 1 : 0;
                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));
            }

            if (string.IsNullOrEmpty(result))
                base.RegisterStartupScript("PAGE", "self.location.href='forum_ForumsTree.aspx';");
            else
                base.RegisterStartupScript("PAGE", "alert('用户:" + result + "不存在,因为无法设为版主');self.location.href='forum_ForumsTree.aspx';");

            #endregion
        }

        #region Web 窗体设计器生成的代码

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.TabControl1.InitTabPage();
            this.SubmitAdd.Click += new EventHandler(this.SubmitAdd_Click);
            InitInfo();
        }

        #endregion

    }
}