using System;
using System.Data;
using System.Web.UI;
using System.Collections;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.Album;
using Discuz.Plugin.Space;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 编辑特殊用户组
    /// </summary>

    public partial class editusergroupspecial : AdminPage
    {
        public UserGroupInfo userGroupInfo = new UserGroupInfo();
        protected bool haveAlbum;
        protected bool haveSpace;

        protected void Page_Load(object sender, EventArgs e)
        {
            haveAlbum = AlbumPluginProvider.GetInstance() != null;
            haveSpace = SpacePluginProvider.GetInstance() != null;
        }

        public void LoadUserGroupInf(int groupid)
        {
            #region 加载相关组信息

            userGroupInfo = AdminUserGroups.AdminGetUserGroupInfo(groupid);

            groupTitle.Text = Utils.RemoveFontTag(userGroupInfo.Grouptitle);

            stars.Text = userGroupInfo.Stars.ToString();
            color.Text = userGroupInfo.Color;
            groupavatar.Text = userGroupInfo.Groupavatar;
            readaccess.Text = userGroupInfo.Readaccess.ToString();
            maxprice.Text = userGroupInfo.Maxprice.ToString();
            maxpmnum.Text = userGroupInfo.Maxpmnum.ToString();
            maxsigsize.Text = userGroupInfo.Maxsigsize.ToString();
            maxattachsize.Text = userGroupInfo.Maxattachsize.ToString();
            maxsizeperday.Text = userGroupInfo.Maxsizeperday.ToString();
            maxspaceattachsize.Text = userGroupInfo.Maxspaceattachsize.ToString();
            maxspacephotosize.Text = userGroupInfo.Maxspacephotosize.ToString();
            //maxfriendscount.Text = userGroupInfo.MaxFriendsCount.ToString();

            radminid.SelectedValue = userGroupInfo.Radminid == -1 ? "0" : userGroupInfo.Radminid.ToString();
            ViewState["radminid"] = userGroupInfo.Radminid;

            //DataTable dt = DbHelper.ExecuteDataset("Select id,extension  From [" + BaseConfigs.GetTablePrefix + "attachtypes]  Order By [id] ASC").Tables[0];
            DataTable dt = Attachments.GetAttachmentType();
            attachextensions.SetSelectByID(userGroupInfo.Attachextensions.Trim());

            //设置用户权限组初始化信息
            //if (__usergroupinfo.Allowvisit == 1) usergroupright.Items[0].Selected = true;
            //if (__usergroupinfo.Allowpost == 1) usergroupright.Items[1].Selected = true;
            //if (__usergroupinfo.Allowreply == 1) usergroupright.Items[2].Selected = true;
            //if (__usergroupinfo.Allowpostpoll == 1) usergroupright.Items[3].Selected = true;
            //if (__usergroupinfo.Allowgetattach == 1) usergroupright.Items[4].Selected = true;
            //if (__usergroupinfo.Allowpostattach == 1) usergroupright.Items[5].Selected = true;
            //if (__usergroupinfo.Allowvote == 1) usergroupright.Items[6].Selected = true;
            //if (__usergroupinfo.Allowsetreadperm == 1) usergroupright.Items[7].Selected = true;
            //if (__usergroupinfo.Allowsetattachperm == 1) usergroupright.Items[8].Selected = true;
            //if (__usergroupinfo.Allowhidecode == 1) usergroupright.Items[9].Selected = true;
            //if (__usergroupinfo.Allowcusbbcode == 1) usergroupright.Items[10].Selected = true;
            //if (__usergroupinfo.Allowsigbbcode == 1) usergroupright.Items[11].Selected = true;
            //if (__usergroupinfo.Allowsigimgcode == 1) usergroupright.Items[12].Selected = true;
            //if (__usergroupinfo.Allowviewpro == 1) usergroupright.Items[13].Selected = true;
            //if (__usergroupinfo.Disableperiodctrl == 1) usergroupright.Items[14].Selected = true;

            //if (__usergroupinfo.Allowsearch.ToString() == "0") allowsearch.Items[0].Selected = true;
            //if (__usergroupinfo.Allowsearch.ToString() == "1") allowsearch.Items[1].Selected = true;
            //if (__usergroupinfo.Allowsearch.ToString() == "2") allowsearch.Items[2].Selected = true;

            //if (__usergroupinfo.Allowavatar >= 0) allowavatar.Items[__usergroupinfo.Allowavatar].Selected = true;

            usergrouppowersetting.Bind(userGroupInfo);
            if (userGroupInfo.System == 1) DeleteUserGroupInf.Enabled = false;

            #endregion
        }

        private void DeleteUserGroupInf_Click(object sender, EventArgs e)
        {
            #region 删除用户组
            if (this.CheckCookie())
            {
                if (AdminUserGroups.DeleteUserGroupInfo(DNTRequest.GetInt("groupid", -1)))
                {
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                    Discuz.Forum.UserGroups.GetUserGroupList();

                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除特殊用户组", "组ID:" + DNTRequest.GetInt("groupid", -1));

                    base.RegisterStartupScript( "PAGE",  "window.location.href='global_usergroupspecialgrid.aspx';");
                }
                else
                {
                    if (AdminUserGroups.opresult != "")
                    {
                        base.RegisterStartupScript( "","<script>alert('操作失败,原因:" + AdminUserGroups.opresult + "');window.location.href='global_usergroupspecialgrid.aspx';</script>");
                    }
                    else
                    {
                        base.RegisterStartupScript( "","<script>alert('操作失败');window.location.href='global_usergroupspecialgrid.aspx';</script>");
                    }
                }
            }
            #endregion
        }

        public int BoolToInt(bool a)
        {
            if (a) return 1;
            else return 0;
        }


        private void UpdateUserGroupInf_Click(object sender, EventArgs e)
        {
            #region 更新特殊用户组信息

            if (this.CheckCookie())
            {

                Hashtable ht = new Hashtable();
                ht.Add("附件最大尺寸", maxattachsize.Text);
                ht.Add("每天最大附件总尺寸", maxsizeperday.Text);
                ht.Add("个人空间附件总尺寸", maxspaceattachsize.Text);
                ht.Add("相册空间总尺寸", maxspacephotosize.Text);

                foreach (DictionaryEntry de in ht)
                {
                    if (!Utils.IsInt(de.Value.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('输入错误," + de.Key.ToString() + "只能是0或者正整数');window.location.href='global_usergroupspecialgrid.aspx';</script>");
                        return;
                    }

                }


                userGroupInfo = AdminUserGroups.AdminGetUserGroupInfo(DNTRequest.GetInt("groupid", -1));
                userGroupInfo.System = 0;
                userGroupInfo.Type = 0;
                userGroupInfo.Readaccess = Convert.ToInt32(readaccess.Text);

                int selectradminid = radminid.SelectedValue == "0" ? -1 : Convert.ToInt32(radminid.SelectedValue);
                userGroupInfo.Radminid = selectradminid;

                if (selectradminid.ToString() != ViewState["radminid"].ToString())
                {
                    Users.UpdateUserAdminIdByGroupId(userGroupInfo.Radminid, userGroupInfo.Groupid);
                }

                userGroupInfo.Grouptitle = groupTitle.Text;
                userGroupInfo.Stars = Convert.ToInt32(stars.Text);
                userGroupInfo.Color = color.Text;
                userGroupInfo.Groupavatar = groupavatar.Text;
                userGroupInfo.Maxprice = Convert.ToInt32(maxprice.Text);
                userGroupInfo.Maxpmnum = Convert.ToInt32(maxpmnum.Text);
                userGroupInfo.Maxsigsize = Convert.ToInt32(maxsigsize.Text);
                userGroupInfo.Maxattachsize = Convert.ToInt32(maxattachsize.Text);
                userGroupInfo.Maxsizeperday = Convert.ToInt32(maxsizeperday.Text);
                userGroupInfo.Maxspaceattachsize = Convert.ToInt32(maxspaceattachsize.Text);
                userGroupInfo.Maxspacephotosize = Convert.ToInt32(maxspacephotosize.Text);
                //userGroupInfo.MaxFriendsCount = Convert.ToInt32(maxfriendscount.Text);
                userGroupInfo.Attachextensions = attachextensions.GetSelectString(",");

                usergrouppowersetting.GetSetting(ref userGroupInfo);
                if (AdminUserGroups.UpdateUserGroupInfo(userGroupInfo))
                {
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                    Discuz.Forum.UserGroups.GetUserGroupList();
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除特殊用户组", "组ID:" + DNTRequest.GetInt("groupid", -1));
                    base.RegisterStartupScript( "PAGE", "window.location.href='global_usergroupspecialgrid.aspx';");
                }
                else
                {
                    if (AdminUserGroups.opresult != "")
                    {
                        base.RegisterStartupScript( "", "<script>alert('操作失败,原因:" + AdminUserGroups.opresult + "');window.location.href='global_usergroupspecialgrid.aspx';</script>");
                    }
                    else
                    {
                        base.RegisterStartupScript( "", "<script>alert('操作失败');window.location.href='global_usergroupspecialgrid.aspx';</script>");
                    }

                }
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
            this.TabControl1.InitTabPage();
            this.UpdateUserGroupInf.Click += new EventHandler(this.UpdateUserGroupInf_Click);
            this.DeleteUserGroupInf.Click += new EventHandler(this.DeleteUserGroupInf_Click);
            radminid.AddTableData(UserGroups.GetAdminGroups(),"grouptitle","groupid");
            DataTable dt = Attachments.GetAttachmentType();
            attachextensions.AddTableData(dt);
            if (DNTRequest.GetString("groupid") != "")
            {
                LoadUserGroupInf(DNTRequest.GetInt("groupid", -1));
            }
            else
            {
                Response.Redirect("global_sysadminusergroupgrid.aspx");
            }
        }

        #endregion
    }
}