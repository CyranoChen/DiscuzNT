using System;
using System.Data;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.Album;
using Discuz.Plugin.Space;
using System.Web.UI.WebControls;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 编辑系统管理组
    /// </summary>
    
    public partial class editsysadminusergroup : AdminPage
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
            creditshigher.Text = userGroupInfo.Creditshigher.ToString();
            creditslower.Text = userGroupInfo.Creditslower.ToString();
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

            attachextensions.SetSelectByID(userGroupInfo.Attachextensions.Trim());

            if (groupid > 0 && groupid <= 3) radminid.Enabled = false;
            radminid.SelectedValue = userGroupInfo.Radminid.ToString();

            usergrouppowersetting.Bind(userGroupInfo);

            if (radminid.SelectedValue == "1")
            {
                allowstickthread.Enabled = false;
                allowstickthread.SelectedValue = "3";
            }

            #endregion
        }


        public int BoolToInt(bool a)
        {
            return a ? 1 : 0;
        }


        public byte BoolToByte(bool a)
        {
            return (byte)(a ? 1 : 0);
        }


        private void UpdateUserGroupInf_Click(object sender, EventArgs e)
        {
            #region 更新系统管理组信息

            if (this.CheckCookie())
            {
                userGroupInfo = AdminUserGroups.AdminGetUserGroupInfo(DNTRequest.GetInt("groupid", -1));
                userGroupInfo.System = 0;
                userGroupInfo.Type = 0;
                userGroupInfo.Readaccess = Convert.ToInt32(readaccess.Text);
                userGroupInfo.Allowviewstats = 0;
                userGroupInfo.Allownickname = 0;
                userGroupInfo.Allowhtml = 0;
                userGroupInfo.Allowcstatus = 0;
                userGroupInfo.Allowuseblog = 0;
                userGroupInfo.Allowinvisible = 0;
                userGroupInfo.Allowtransfer = 0;
                userGroupInfo.Allowmultigroups = 0;
                userGroupInfo.Reasonpm = 0;

                //if (radminid.SelectedValue == "0") //当未选取任何管理模板时
                //{
                //    Discuz.Forum.AdminGroups.DeleteAdminGroupInfo((short)userGroupInfo.Groupid);
                //    userGroupInfo.Radminid = 0;
                //}

                Users.UpdateUserAdminIdByGroupId(userGroupInfo.Radminid, userGroupInfo.Groupid);
                userGroupInfo.Grouptitle = groupTitle.Text;
                userGroupInfo.Creditshigher = Convert.ToInt32(creditshigher.Text);
                userGroupInfo.Creditslower = Convert.ToInt32(creditslower.Text);
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
                userGroupInfo.Attachextensions = attachextensions.GetSelectString(",");

                usergrouppowersetting.GetSetting(ref userGroupInfo);

                if (AdminUserGroups.UpdateUserGroupInfo(userGroupInfo))
                {
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AdminGroupList");
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台更新系统组", "组ID:" + DNTRequest.GetInt("groupid", -1));
                    base.RegisterStartupScript( "PAGE",  "window.location.href='global_sysadminusergroupgrid.aspx';");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('操作失败');window.location.href='global_sysadminusergroupgrid.aspx';</script>");
                }
            }

            #endregion
        }


        private void radminid_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region 绑定关联用户组信息
            UserGroupInfo radminUserGroupInfo = UserGroups.GetUserGroupInfo(int.Parse(radminid.SelectedValue));
            if (radminUserGroupInfo != null)
            {
                //设置管理组初始化信息
                //DataRow usergrouprights = usergrouprightstable.Rows[0];
                creditshigher.Text = radminUserGroupInfo.Creditslower.ToString();
                creditslower.Text = radminUserGroupInfo.Creditslower.ToString();
                stars.Text = radminUserGroupInfo.Stars.ToString();
                color.Text = radminUserGroupInfo.Color;
                groupavatar.Text = radminUserGroupInfo.Groupavatar;
                readaccess.Text = radminUserGroupInfo.Readaccess.ToString();
                maxprice.Text = radminUserGroupInfo.Maxprice.ToString();
                maxpmnum.Text = radminUserGroupInfo.Maxpmnum.ToString();
                maxsigsize.Text = radminUserGroupInfo.Maxsigsize.ToString();
                maxattachsize.Text = radminUserGroupInfo.Maxattachsize.ToString();
                maxsizeperday.Text = radminUserGroupInfo.Maxsizeperday.ToString();
                DataTable dt = Attachments.GetAttachmentType();
                attachextensions.AddTableData(dt, radminUserGroupInfo.Attachextensions);
            }

            AdminGroupInfo radminUserGroup = AdminGroups.GetAdminGroupInfo(int.Parse(radminid.SelectedValue));
            if (radminUserGroup != null)
            {
                //设置管理权限组初始化信息
                //DataRow dr = admingrouprights.Rows[0];
                admingroupright.SelectedIndex = -1;
                admingroupright.Items[0].Selected = radminUserGroup.Alloweditpost == 1;
                admingroupright.Items[1].Selected = radminUserGroup.Alloweditpoll == 1;
                admingroupright.Items[2].Selected = radminUserGroup.Allowdelpost == 1;
                admingroupright.Items[3].Selected = radminUserGroup.Allowmassprune == 1;
                admingroupright.Items[4].Selected = radminUserGroup.Allowviewip == 1;
                admingroupright.Items[5].Selected = radminUserGroup.Allowedituser == 1;
                admingroupright.Items[6].Selected = radminUserGroup.Allowviewlog == 1;
                admingroupright.Items[7].Selected = radminUserGroup.Disablepostctrl == 1;
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
            radminid.Items.Add(new ListItem("请选择     ", "0"));
            foreach (UserGroupInfo userGroupInfo in UserGroups.GetAdminUserGroup())
                radminid.Items.Add(new ListItem(userGroupInfo.Grouptitle, userGroupInfo.Groupid.ToString()));
            DataTable dt = Attachments.GetAttachmentType();
            attachextensions.AddTableData(dt);

            string groupid = DNTRequest.GetString("groupid");
            if (groupid != "")
            {
                LoadUserGroupInf(DNTRequest.GetInt("groupid", -1));
            }
            else
            {
                Response.Redirect("sysglobal_sysadminusergroupgrid.aspx");
            }

        }

        #endregion
    }
}