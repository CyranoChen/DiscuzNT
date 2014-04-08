using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 编辑论坛版块信息
    /// </summary>

    public partial class editmall : AdminPage
    {
        public string runforumsstatic;
        public DataRow dr;
        public ForumInfo forumInfo = new ForumInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.TabControl1.Items.Remove(this.TabControl1.Items[5]);
                this.TabControl1.Items.Remove(this.TabControl1.Items[4]);
                if (DNTRequest.GetString("fid") == "")
                {
                    return;
                }

                BindTopicType();
                DataGridBind("");
            }
        }

        public void LoadCurrentForumInfo(int fid)
        {
            #region 加载相关信息

            if (fid > 0)
            {
                forumInfo = Forums.GetForumInfo(fid);
            }
            else
            {
                return;
            }

            if (forumInfo.Layer > 0)
            {
                tabPage2.Visible = true;
                tabPage6.Visible = true;
            }
            else
            {
                //删除掉"高级设置"属性页
                TabControl1.Items.Remove(tabPage2);
                tabPage2.Visible = false;

                //删除掉"特殊用户"属性页
                TabControl1.Items.Remove(tabPage4);
                tabPage4.Visible = false;

                //删除掉"主题分类"属性页
                TabControl1.Items.Remove(tabPage5);
                tabPage5.Visible = false;

                //删除掉"统计信息"属性页
                TabControl1.Items.Remove(tabPage6);
                tabPage6.Visible = false;
                templatestyle.Visible = false;
            }

            forumname.Text = forumInfo.Name.Trim();
            name.Text = forumInfo.Name.Trim();
            displayorder.Text = forumInfo.Displayorder.ToString();

            status.SelectedValue = forumInfo.Status.ToString();

            if (forumInfo.Colcount == 1)
            {
                showcolnum.Attributes.Add("style", "display:none");
                colcount.SelectedIndex = 0;
            }
            else
            {
                showcolnum.Attributes.Add("style", "display:block");
                colcount.SelectedIndex = 1;
            }
            colcount.Attributes.Add("onclick", "javascript:document.getElementById('" + showcolnum.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage1_colcount_0').checked ? 'none' : 'block');");
            colcountnumber.Text = forumInfo.Colcount.ToString();

            templateid.SelectedValue = forumInfo.Templateid.ToString();

            forumsstatic.Text = string.Format("主题总数:{0}<br />帖子总数:{1}<br />今日回帖数总数:{2}<br />最后提交日期:{3}",
                                              forumInfo.Topics.ToString(),
                                              forumInfo.Posts.ToString(),
                                              forumInfo.Todayposts.ToString(),
                                              forumInfo.Lastpost.ToString());

            ViewState["forumsstatic"] = forumsstatic.Text;

            if (forumInfo.Allowsmilies == 1) setting.Items[0].Selected = true;
            if (forumInfo.Allowrss == 1) setting.Items[1].Selected = true;
            if (forumInfo.Allowbbcode == 1) setting.Items[2].Selected = true;
            if (forumInfo.Allowimgcode == 1) setting.Items[3].Selected = true;
            if (forumInfo.Recyclebin == 1) setting.Items[4].Selected = true;
            if (forumInfo.Modnewposts == 1) setting.Items[5].Selected = true;
            if (forumInfo.Disablewatermark == 1) setting.Items[6].Selected = true;
            if (forumInfo.Inheritedmod == 1) setting.Items[7].Selected = true;
            if (forumInfo.Allowthumbnail == 1) setting.Items[8].Selected = true;
            if (forumInfo.Allowtag == 1) setting.Items[9].Selected = true;
            allowspecialonly.SelectedValue = forumInfo.Allowspecialonly.ToString();

            if (forumInfo.Autoclose == 0)
            {
                showclose.Attributes.Add("style", "display:none");
                autocloseoption.SelectedIndex = 0;
            }
            else
            {
                autocloseoption.SelectedIndex = 1;
            }
            autocloseoption.Attributes.Add("onclick", "javascript:document.getElementById('" + showclose.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage2_autocloseoption_0').checked ? 'none' : 'block');");
            autocloseday.Text = forumInfo.Autoclose.ToString();

            //提取高级信息
            description.Text = forumInfo.Description.Trim();
            password.Text = forumInfo.Password.Trim();
            icon.Text = forumInfo.Icon.Trim();
            redirect.Text = forumInfo.Redirect.Trim();
            moderators.Text = forumInfo.Moderators.Trim();
            inheritmoderators.Text = Users.GetModerators(fid);
            rules.Text = forumInfo.Rules.Trim();
            topictypes.Text = forumInfo.Topictypes.Trim();

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
                tr.Cells.Add(GetTD("viewperm", forumInfo.Viewperm.Trim(), dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("postperm", forumInfo.Postperm.Trim(), dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("replyperm", forumInfo.Replyperm.Trim(), dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("getattachperm", forumInfo.Getattachperm.Trim(), dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("postattachperm", forumInfo.Postattachperm.Trim(), dr["groupid"].ToString(), i));
                powerset.Rows.Add(tr);
                i++;
            }


            dt = Attachments.GetAttachmentType();
            attachextensions.SetSelectByID(forumInfo.Attachextensions.Trim());

            if (fid > 0)
            {
                forumInfo = Forums.GetForumInfo(fid);
            }
            else
            {
                return;
            }
            applytopictype.SelectedValue = forumInfo.Applytopictype.ToString();
            postbytopictype.SelectedValue = forumInfo.Postbytopictype.ToString();
            viewbytopictype.SelectedValue = forumInfo.Viewbytopictype.ToString();
            topictypeprefix.SelectedValue = forumInfo.Topictypeprefix.ToString();


            #endregion
        }

        private void BindTopicType()
        {
            #region 主题分类绑定
            TopicTypeDataGrid.BindData(TopicTypes.GetTopicTypes());
            TopicTypeDataGrid.TableHeaderName = "当前版块:  " + forumInfo.Name;
            #endregion
        }

        public void TopicTypeDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 主题分类列绑定
            string[] topictype = forumInfo.Topictypes.Split('|');
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                string id = e.Item.Cells[0].Text;
                foreach (string type in topictype)
                {
                    if (type.Split(',')[0] == id)
                    {
                        e.Item.Cells[3].Text = "<input type='hidden' name='oldtopictype" + e.Item.ItemIndex + "' value='" + type + "|' /><input type='radio' name='type" + e.Item.ItemIndex + "' value='-1' />";
                        if ((type + "&").IndexOf(",0&") < 0)	//加上一个“&”可以指定是尾部，以防止type中出现"26,1111111111,0"，注意26后面就出现了“,1”，从而选取了“,1”样式
                            e.Item.Cells[4].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' value='" + id + "," + e.Item.Cells[1].Text + ",0|' />";
                        else
                            e.Item.Cells[4].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' checked value='" + id + "," + e.Item.Cells[1].Text + ",0|' />";
                        if ((type + "&").IndexOf(",1&") < 0) //加上一个“&”可以指定是尾部，以防止type中出现"26,0111111111,0"，注意26后面就出现了“,0”，从而选取了“,0”样式
                            e.Item.Cells[5].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' value='" + id + "," + e.Item.Cells[1].Text + ",1|' />";
                        else
                            e.Item.Cells[5].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' checked value='" + id + "," + e.Item.Cells[1].Text + ",1|' />";
                        return;
                    }
                }
                e.Item.Cells[3].Text = "<input type='hidden' name='oldtopictype" + e.Item.ItemIndex + "' value='' /><input type='radio' name='type" + e.Item.ItemIndex + "' checked value='-1' />";
                e.Item.Cells[4].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' value='" + id + "," + e.Item.Cells[1].Text + ",0|' />";
                e.Item.Cells[5].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' value='" + id + "," + e.Item.Cells[1].Text + ",1|' />";
            }
            #endregion
        }

        public void TopicTypeDataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            TopicTypeDataGrid.LoadCurrentPageIndex(e.NewPageIndex);
            BindTopicType();
            DataGridBind("");
            this.TabControl1.SelectedIndex = 4;
        }

        private HtmlTableCell GetTD(string strPerfix, string groupList, string groupId, int ctlId)
        {
            #region 生成组权限控制项

            groupList = "," + groupList + ",";
            string strTd = "<input type='checkbox' name='" + strPerfix + "' id='" + strPerfix + ctlId + "' value='" + groupId + "' "
                + (groupList.IndexOf("," + groupId + ",") == -1 ? "" : "checked='checked'") + ">";
            HtmlTableCell td = new HtmlTableCell("td");
            if (ctlId % 2 == 1)
                td.Attributes.Add("class", "td_alternating_item1");
            else
                td.Attributes.Add("class", "td_alternating_item2");
            td.Controls.Add(new LiteralControl(strTd));
            return td;

            #endregion
        }

        public void BindPower_Click(object sender, EventArgs e)
        {
            #region 特殊用户绑定
            if (UserList.Text != "")
            {
                string result = forumInfo.Permuserlist;
                string[] userpowerlist = new string[1] { "" };
                if (result != null)
                {
                    userpowerlist = forumInfo.Permuserlist.Split('|');
                }

                foreach (string adduser in UserList.Text.Split(','))
                {
                    string uid = Discuz.Forum.Users.GetUserId(adduser).ToString();
                    if (uid == "-1")
                        continue;
                    bool find = false;
                    foreach (string u in userpowerlist)
                    {
                        if (u.IndexOf(adduser + ",") == 0)
                        {
                            result = result.Replace(u, adduser + "," + uid + "," + 0);
                            find = true;
                            break;
                        }
                    }
                    if (!find)
                    {
                        result = adduser + "," + uid + "," + 0 + "|" + result;
                    }
                }
                if (result != "")
                {
                    if (result.Substring(result.Length - 1, 1) == "|")
                        result = result.Substring(0, result.Length - 1);
                }
                forumInfo.Permuserlist = result;
                AdminForums.UpdateForumInfo(forumInfo);
                UserList.Text = "";
            }
            DataGridBind("");
            BindTopicType();
            this.TabControl1.SelectedIndex = 3;
            #endregion
        }

        public void DelButton_Click(object sender, EventArgs e)
        {
            #region 删除特殊用户
            int row = 0;
            ArrayList al = new ArrayList(forumInfo.Permuserlist.Split('|'));
            foreach (object o in SpecialUserList.GetKeyIDArray())
            {
                if (SpecialUserList.GetCheckBoxValue(row, "userid"))
                {
                    string uid = o.ToString();
                    foreach (string user in forumInfo.Permuserlist.Split('|'))
                    {
                        if (user.IndexOf("," + uid + ",") > 0)
                        {
                            al.Remove(user);
                            break;
                        }
                    }
                }
                row++;
            }
            string result = "";
            foreach (string user in al)
            {
                result += user + "|";
            }
            if (result != "")
                result = result.Substring(0, result.Length - 1);
            forumInfo.Permuserlist = result;
            AdminForums.UpdateForumInfo(forumInfo);
            if (SpecialUserList.Items.Count == 1 && SpecialUserList.CurrentPageIndex > 0)
            {
                SpecialUserList.CurrentPageIndex--;
            }
            DataGridBind("");
            BindTopicType();
            this.TabControl1.SelectedIndex = 3;
            #endregion
        }

        private void DataGridBind(string userList)
        {
            #region 特殊用户设置
            SpecialUserList.TableHeaderName = "特殊用户权限设置";
            string Permuserlist = forumInfo.Permuserlist;
            DataTable dt = new DataTable();
            dt.Columns.Add("id", System.Type.GetType("System.Int32"));
            dt.Columns.Add("uid", System.Type.GetType("System.Int32"));
            dt.Columns.Add("name", System.Type.GetType("System.String"));
            dt.Columns.Add("viewbyuser", System.Type.GetType("System.Boolean"));
            dt.Columns.Add("postbyuser", System.Type.GetType("System.Boolean"));
            dt.Columns.Add("replybyuser", System.Type.GetType("System.Boolean"));
            dt.Columns.Add("getattachbyuser", System.Type.GetType("System.Boolean"));
            dt.Columns.Add("postattachbyuser", System.Type.GetType("System.Boolean"));
            foreach (string user in userList.Split(','))
            {
                if (user.Trim() == "")
                    continue;
                int uid = Discuz.Forum.Users.GetUserId(user);
                if (uid != -1)
                {
                    DataRow dr = dt.NewRow();
                    dr["id"] = dt.Rows.Count + 1;
                    dr["uid"] = uid.ToString();
                    dr["name"] = user;
                    dr["viewbyuser"] = false;
                    dr["postbyuser"] = false;
                    dr["replybyuser"] = false;
                    dr["getattachbyuser"] = false;
                    dr["postattachbyuser"] = false;
                    dt.Rows.Add(dr);
                }
            }

            if (Permuserlist != null)
            {
                foreach (string p in Permuserlist.Split('|'))
                {
                    if (("," + userList + ",").IndexOf("," + p.Split(',')[0] + ",") >= 0)
                        continue;
                    int power = Convert.ToInt32(p.Split(',')[2]);
                    DataRow dr = dt.NewRow();
                    dr["id"] = dt.Rows.Count + 1;
                    dr["uid"] = p.Split(',')[1];
                    dr["name"] = p.Split(',')[0];
                    dr["viewbyuser"] = power & (int)Discuz.Entity.ForumSpecialUserPower.ViewByUser;
                    dr["postbyuser"] = power & (int)Discuz.Entity.ForumSpecialUserPower.PostByUser;
                    dr["replybyuser"] = power & (int)Discuz.Entity.ForumSpecialUserPower.ReplyByUser;
                    dr["getattachbyuser"] = power & (int)Discuz.Entity.ForumSpecialUserPower.DownloadAttachByUser;
                    dr["postattachbyuser"] = power & (int)Discuz.Entity.ForumSpecialUserPower.PostAttachByUser;
                    dt.Rows.Add(dr);
                }
            }
            SpecialUserList.DataSource = dt;
            SpecialUserList.DataBind();
            #endregion
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            #region 翻页

            SpecialUserList.LoadCurrentPageIndex(e.NewPageIndex);
            DataGridBind("");
            BindTopicType();
            this.TabControl1.SelectedIndex = 3;
            //base.CallBaseRegisterStartupScript("PAGE", "<script>Tab_OnSelectClientClick(document.getElementById('TabControl1:tabPage34_H2'),'TabControl1:tabPage34');</script>");

            #endregion
        }

        public int BoolToInt(bool a)
        {
            return a ? 1 : 0;
        }

        private void SubmitInfo_Click(object sender, EventArgs e)
        {
            #region 提交同级版块

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("fid") != "")
                {
                    forumInfo = Forums.GetForumInfo(DNTRequest.GetInt("fid", 0));
                    forumInfo.Name = name.Text.Trim();
                    forumInfo.Displayorder = Convert.ToInt32(displayorder.Text);
                    forumInfo.Status = Convert.ToInt16(status.SelectedValue);

                    if (colcount.SelectedValue == "1") //传统模式[默认]
                    {
                        forumInfo.Colcount = 1;
                    }
                    else
                    {
                        if (Convert.ToInt16(colcountnumber.Text) < 1 || Convert.ToInt16(colcountnumber.Text) > 9)
                        {
                            base.RegisterStartupScript("", "<script>alert('列值必须在2~9范围内');</script>");
                            return;
                        }
                        forumInfo.Colcount = Convert.ToInt16(colcountnumber.Text);
                    }

                    forumInfo.Templateid = (Convert.ToInt32(templateid.SelectedValue) == config.Templateid ? 0 : Convert.ToInt32(templateid.SelectedValue));
                    forumInfo.Allowhtml = 0;
                    forumInfo.Allowblog = 0;
                    //__foruminfo.Istrade = 0;
                    //__foruminfo.Allowpostspecial = 0; //需要作与运算如下
                    //__foruminfo.Allowspecialonly = 0;　//需要作与运算如下
                    ////$allow辩论 = allowpostspecial & 16;
                    ////$allow悬赏 = allowpostspecial & 4;
                    ////$allow投票 = allowpostspecial & 1;

                    forumInfo.Alloweditrules = 0;
                    forumInfo.Allowsmilies = BoolToInt(setting.Items[0].Selected);
                    forumInfo.Allowrss = BoolToInt(setting.Items[1].Selected);
                    forumInfo.Allowbbcode = BoolToInt(setting.Items[2].Selected);
                    forumInfo.Allowimgcode = BoolToInt(setting.Items[3].Selected);
                    forumInfo.Recyclebin = BoolToInt(setting.Items[4].Selected);
                    forumInfo.Modnewposts = BoolToInt(setting.Items[5].Selected);
                    //__foruminfo.Jammer = BoolToInt(setting.Items[6].Selected);
                    forumInfo.Disablewatermark = BoolToInt(setting.Items[6].Selected);
                    forumInfo.Inheritedmod = BoolToInt(setting.Items[7].Selected);
                    forumInfo.Allowthumbnail = BoolToInt(setting.Items[8].Selected);
                    forumInfo.Allowtag = BoolToInt(setting.Items[9].Selected);
                    //__foruminfo.Istrade = BoolToInt(setting.Items[11].Selected);
                    int temppostspecial = 0;
                    //temppostspecial = setting.Items[11].Selected ? temppostspecial | 1 : temppostspecial & ~1;
                    //temppostspecial = setting.Items[12].Selected ? temppostspecial | 16 : temppostspecial & ~16;
                    //temppostspecial = setting.Items[13].Selected ? temppostspecial | 4 : temppostspecial & ~4;
                    forumInfo.Allowpostspecial = temppostspecial;
                    forumInfo.Allowspecialonly = Convert.ToInt16(allowspecialonly.SelectedValue);

                    if (autocloseoption.SelectedValue == "0")
                        forumInfo.Autoclose = 0;
                    else
                        forumInfo.Autoclose = Convert.ToInt32(autocloseday.Text);

                    forumInfo.Description = description.Text;
                    forumInfo.Password = password.Text;
                    forumInfo.Icon = icon.Text;
                    forumInfo.Redirect = redirect.Text;
                    forumInfo.Attachextensions = attachextensions.GetSelectString(",");

                    AdminForums.CompareOldAndNewModerator(forumInfo.Moderators, moderators.Text.Replace("\r\n",","), DNTRequest.GetInt("fid", 0));

                    forumInfo.Moderators = moderators.Text.Replace("\r\n", ",");
                    forumInfo.Rules = rules.Text;
                    forumInfo.Topictypes = topictypes.Text;
                    forumInfo.Viewperm = Request.Form["viewperm"];
                    forumInfo.Postperm = Request.Form["postperm"];
                    forumInfo.Replyperm = Request.Form["replyperm"];
                    forumInfo.Getattachperm = Request.Form["getattachperm"];
                    forumInfo.Postattachperm = Request.Form["postattachperm"];

                    forumInfo.Applytopictype = Convert.ToInt32(applytopictype.SelectedValue);
                    forumInfo.Postbytopictype = Convert.ToInt32(postbytopictype.SelectedValue);
                    forumInfo.Viewbytopictype = Convert.ToInt32(viewbytopictype.SelectedValue);
                    forumInfo.Topictypeprefix = Convert.ToInt32(topictypeprefix.SelectedValue);
                    forumInfo.Topictypes = GetTopicType();

                    forumInfo.Permuserlist = GetPermuserlist();

                    Discuz.Aggregation.AggregationFacade.ForumAggregation.ClearDataBind();
                    string result = AdminForums.UpdateForumInfo(forumInfo).Replace("'", "’");
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "编辑论坛版块", "编辑论坛版块,名称为:" + name.Text.Trim());

                    GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                    configInfo.Specifytemplate = Forums.GetSpecifyForumTemplateCount() > 0 ? 1 : 0;
                    GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));
                    if (result == "")
                    {
                        Response.Redirect("forum_ForumsTree.aspx");
                    }
                    else
                    {
                        Response.Write("<script>alert('用户:" + result + "不存在或因为它们所属组为\"游客\",\"等待验证会员\",因为无法设为版主');window.location.href='forum_ForumsTree.aspx';</script>");
                        Response.End();
                    }
                }
            }

            #endregion
        }

        private string GetTopicType()
        {
            #region 获取主题分类
            string tmpType = forumInfo.Topictypes;
            int i = 0;
            DataTable topicTypes = TopicTypes.GetTopicTypes();
            while (true)
            {
                #region
                if (DNTRequest.GetFormString("type" + i) == "") //循环处理选择的主题分类
                    break;
                else
                {
                    if (DNTRequest.GetFormString("type" + i) != "-1")   //-1不使用，0平板显示，1下拉显示
                    {
                        string oldtopictype = DNTRequest.GetFormString("oldtopictype" + i); //旧主题分类
                        string newtopictype = DNTRequest.GetFormString("type" + i); //新主题分类
                        if (oldtopictype == null || oldtopictype == "")
                        {
                            //tmpType += newtopictype;
                            int insertOrder = GetDisplayOrder(newtopictype.Split(',')[1], topicTypes);
                            ArrayList topictypesal = new ArrayList();
                            foreach (string topictype in tmpType.Split('|'))
                            {
                                if (topictype != "")
                                    topictypesal.Add(topictype);
                            }
                            bool isInsert = false;
                            for (int j = 0; j < topictypesal.Count; j++)
                            {
                                int curDisplayOrder = GetDisplayOrder(topictypesal[j].ToString().Split(',')[1], topicTypes);
                                if (curDisplayOrder > insertOrder)
                                {
                                    topictypesal.Insert(j, newtopictype);
                                    isInsert = true;
                                    break;
                                }
                            }
                            if (!isInsert)
                            {
                                topictypesal.Add(newtopictype);
                            }
                            tmpType = "";
                            foreach (object t in topictypesal)
                            {
                                tmpType += t.ToString() + "|";
                            }
                        }
                        else
                            tmpType = tmpType.Replace(oldtopictype, newtopictype);
                    }
                    else
                    {
                        if (DNTRequest.GetFormString("oldtopictype" + i) != "")
                            tmpType = tmpType.Replace(DNTRequest.GetFormString("oldtopictype" + i), "");
                    }
                }
                #endregion
                i++;

            }
            return tmpType;
            #endregion
        }

        private int GetDisplayOrder(string topicTypeName, DataTable topicTypes)
        {
            #region 返回显示顺序
            foreach (DataRow dr in topicTypes.Rows)
            {
                if (dr["name"].ToString().Trim() == topicTypeName.Trim())
                {
                    return int.Parse(dr["displayorder"].ToString());
                }
            }
            return -1;
            #endregion
        }

        private string GetTopicTypeString(string topicTypes, string topicName)
        {
            #region 获取主题分类
            foreach (string type in topicTypes.Split('|'))
            {
                if (type.IndexOf("," + topicName.Trim() + ",") != -1)
                    return type;
            }
            return "";
            #endregion
        }

        private string GetPermuserlist()
        {
            #region 获取特殊用户
            int row = 0;
            string result = forumInfo.Permuserlist;
            if (result == null)
                return "";
            foreach (object o in SpecialUserList.GetKeyIDArray())
            {
                string uid = o.ToString();
                int power = 0;
                if (SpecialUserList.GetCheckBoxValue(row, "viewbyuser"))
                    power |= (int)Discuz.Entity.ForumSpecialUserPower.ViewByUser;
                if (SpecialUserList.GetCheckBoxValue(row, "postbyuser"))
                    power |= (int)Discuz.Entity.ForumSpecialUserPower.PostByUser;
                if (SpecialUserList.GetCheckBoxValue(row, "replybyuser"))
                    power |= (int)Discuz.Entity.ForumSpecialUserPower.ReplyByUser;
                if (SpecialUserList.GetCheckBoxValue(row, "getattachbyuser"))
                    power |= (int)Discuz.Entity.ForumSpecialUserPower.DownloadAttachByUser;
                if (SpecialUserList.GetCheckBoxValue(row, "postattachbyuser"))
                    power |= (int)Discuz.Entity.ForumSpecialUserPower.PostAttachByUser;
                string[] userpowerlist = forumInfo.Permuserlist.Split('|');
                bool find = false;
                foreach (string u in userpowerlist)
                {
                    if (u.IndexOf("," + uid + ",") > 0)
                    {
                        result = result.Replace(u, u.Split(',')[0] + "," + uid + "," + power);
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    ShortUserInfo shortUserInfo = Users.GetShortUserInfo(Convert.ToInt32(uid));
                    result = ((shortUserInfo != null) ? shortUserInfo.Username.Trim() : "") + "," + uid + "," + power + "|" + result;
                }
                row++;
            }
            if (result == "")
                return "";
            else
            {
                if (result.Substring(result.Length - 1, 1) == "|")
                    return result.Substring(0, result.Length - 1);
                else
                    return result;
            }
            #endregion
        }

        private void RunForumStatic_Click(object sender, EventArgs e)
        {
            #region 运行论坛统计

            if (this.CheckCookie())
            {
                forumsstatic.Text = ViewState["forumsstatic"].ToString();

                int fid = DNTRequest.GetInt("fid", -1);
                if (fid > 0)
                {
                    forumInfo = Forums.GetForumInfo(fid);
                }
                else
                {
                    return;
                }

                int topiccount = 0;
                int postcount = 0;
                int lasttid = 0;
                string lasttitle = "";
                string lastpost = "";
                int lastposterid = 0;
                string lastposter = "";
                int replypost = 0;
                AdminForumStats.ReSetFourmTopicAPost(fid, out topiccount, out postcount, out lasttid, out lasttitle, out lastpost, out lastposterid, out lastposter, out replypost);

                runforumsstatic = string.Format("<br /><br />运行结果<hr style=\"height:1px; width:600; color:#CCCCCC; background:#CCCCCC; border: 0; \" align=\"left\" />主题总数:{0}<br />帖子总数:{1}<br />今日回帖数总数:{2}<br />最后提交日期:{3}",
                                                topiccount,
                                                postcount,
                                                replypost,
                                                lastpost);

                if ((forumInfo.Topics == topiccount) && (forumInfo.Posts == postcount) && (forumInfo.Todayposts == replypost) && (forumInfo.Lastpost.Trim() == lastpost))
                {
                    runforumsstatic += "<br /><br /><br />结果一致";
                }
                else
                {
                    runforumsstatic += "<br /><br /><br />比较<hr style=\"height:1px; width:600; color:#CCCCCC; background:#CCCCCC; border: 0; \" align=\"left\" />";
                    if (forumInfo.Topics != topiccount)
                    {
                        runforumsstatic += "主题总数有差异<br />";
                    }
                    if (forumInfo.Posts != postcount)
                    {
                        runforumsstatic += "帖子总数有差异<br />";
                    }
                    if (forumInfo.Todayposts != replypost)
                    {
                        runforumsstatic += "今日回帖数总数有差异<br />";
                    }
                    if (forumInfo.Lastpost != lastpost)
                    {
                        runforumsstatic += "最后提交日期有差异<br />";
                    }
                }
            }
            this.TabControl1.SelectedIndex = 5;
            DataGridBind("");
            BindTopicType();
            #endregion
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            #region 排序

            TopicTypeDataGrid.Sort = e.SortExpression.ToString();

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
            this.TabControl1.SelectedIndex = DNTRequest.GetInt("tabindex", 0);
            this.SpecialUserList.PageIndexChanged += new DataGridPageChangedEventHandler(this.DataGrid_PageIndexChanged);
            this.TopicTypeDataGrid.ItemDataBound += new DataGridItemEventHandler(this.TopicTypeDataGrid_ItemDataBound);
            this.TopicTypeDataGrid.SortCommand += new DataGridSortCommandEventHandler(this.Sort_Grid);
            this.TopicTypeDataGrid.PageIndexChanged += new DataGridPageChangedEventHandler(this.TopicTypeDataGrid_PageIndexChanged);

            TopicTypeDataGrid.AllowCustomPaging = false;
            TopicTypeDataGrid.DataKeyField = "id";
            TopicTypeDataGrid.ColumnSpan = 6;

            this.SubmitInfo.Click += new System.EventHandler(this.SubmitInfo_Click);
            this.RunForumStatic.Click += new System.EventHandler(this.RunForumStatic_Click);
            this.BindPower.Click += new EventHandler(this.BindPower_Click);
            this.DelButton.Click += new EventHandler(this.DelButton_Click);

            templateid.AddTableData(Templates.GetValidTemplateList(), "name", "templateid");
            attachextensions.AddTableData(Attachments.GetAttachmentType());

            LoadCurrentForumInfo(DNTRequest.GetInt("fid", -1));

            SpecialUserList.AllowPaging = true;
            SpecialUserList.DataKeyField = "id";
        }

        #endregion
    }
}