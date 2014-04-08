using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Cache;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 合并用户组
    /// </summary>
    public partial class combinationusergroup : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                foreach (UserGroupInfo userGroupInfo in UserGroups.GetUserGroupList())
                {
                    if (userGroupInfo.Radminid == 0)
                    {
                        sourceusergroup.Items.Add(new ListItem(userGroupInfo.Grouptitle, userGroupInfo.Groupid.ToString()));
                        targetusergroup.Items.Add(new ListItem(userGroupInfo.Grouptitle, userGroupInfo.Groupid.ToString()));
                    }
                }
                foreach (UserGroupInfo adminUserGroupInfo in UserGroups.GetAdminAndSpecialGroup())
                {
                    sourceadminusergroup.Items.Add(new ListItem(adminUserGroupInfo.Grouptitle, adminUserGroupInfo.Groupid.ToString()));
                    targetadminusergroup.Items.Add(new ListItem(adminUserGroupInfo.Grouptitle, adminUserGroupInfo.Groupid.ToString()));
                }
            }
        }

        private void ComUsergroup_Click(object sender, EventArgs e)
        {
            #region 合并用户组
            if (this.CheckCookie())
            {
                if ((sourceusergroup.SelectedIndex == 0) || (targetusergroup.SelectedIndex == 0))
                {
                    base.RegisterStartupScript( "", "<script>alert('操作失败,请您选择有效的用户组!');</script>");
                    return;
                }

                if (sourceusergroup.SelectedValue == targetusergroup.SelectedValue)
                {
                    base.RegisterStartupScript( "", "<script>alert('操作失败,同一个用户组不能够合并!');</script>");
                    return;
                }

                if (UserGroups.GetUserGroupInfo(int.Parse(sourceusergroup.SelectedValue)).Creditslower != 
                    UserGroups.GetUserGroupInfo(int.Parse(targetusergroup.SelectedValue)).Creditshigher)
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败,要合并的用户组必须是积分相连的两个用户组!');</script>");
                    return;
                }

                //合并用户积分上下限
                UserGroupInfo userGroupInfo = UserGroups.GetUserGroupInfo(int.Parse(targetusergroup.SelectedValue));
                userGroupInfo.Creditshigher = UserGroups.GetUserGroupInfo(int.Parse(sourceusergroup.SelectedValue)).Creditshigher;
                UserGroups.UpdateUserGroup(userGroupInfo);
                //UserGroups.CombinationUserGroupScore(int.Parse(sourceusergroup.SelectedValue), int.Parse(targetusergroup.SelectedValue));
                //删除被合并的源用户组
                //DatabaseProvider.GetInstance().DeleteUserGroupInfo(int.Parse(sourceusergroup.SelectedValue));
                UserGroups.DeleteUserGroupInfo(int.Parse(sourceusergroup.SelectedValue));

                //更新用户组中的信息
                //Data.DatabaseProvider.GetInstance().ChangeUsergroup(int.Parse(sourceusergroup.SelectedValue), int.Parse(targetusergroup.SelectedValue));
                UserGroups.ChangeAllUserGroupId(int.Parse(sourceusergroup.SelectedValue), int.Parse(targetusergroup.SelectedValue));

                DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "合并用户组", "把组ID:" + sourceusergroup.SelectedIndex + " 合并到组ID:" + targetusergroup.SelectedIndex);
                base.RegisterStartupScript("PAGE", "window.location.href='global_usergroupgrid.aspx';");
            }

            #endregion
        }

        private void ComAdminUsergroup_Click(object sender, EventArgs e)
        {
            #region 合并管理组

            if (this.CheckCookie())
            {
                if ((sourceadminusergroup.SelectedIndex == 0) || (targetadminusergroup.SelectedIndex == 0))
                {
                    base.RegisterStartupScript( "", "<script>alert('操作失败,请您选择有效的管理组!');</script>");
                    return;
                }

                if ((Convert.ToInt32(sourceadminusergroup.SelectedValue) <= 3) || (Convert.ToInt32(sourceadminusergroup.SelectedValue) <= 3))
                {
                    base.RegisterStartupScript( "", "<script>alert('操作失败,您选择的组为系统初始化的管理组,这些组不允许合并!');</script>");
                    return;
                }

                if (sourceadminusergroup.SelectedValue == targetadminusergroup.SelectedValue)
                {
                    base.RegisterStartupScript( "", "<script>alert('操作失败,同一个管理组不能够合并!');</script>");
                    return;
                }

                //删除被合并的源用户组
                //DatabaseProvider.GetInstance().DeleteAdminGroupInfo(int.Parse(sourceadminusergroup.SelectedValue));
                AdminGroups.DeleteAdminGroupInfo(Convert.ToInt16(sourceadminusergroup.SelectedValue));

                //删除被合并的源用户组
                //DatabaseProvider.GetInstance().DeleteUserGroupInfo(int.Parse(sourceadminusergroup.SelectedValue));
                UserGroups.DeleteUserGroupInfo(int.Parse(sourceadminusergroup.SelectedValue));
             
                //更新用户组中的信息
                //Data.DatabaseProvider.GetInstance().UpdateAdminUsergroup(targetadminusergroup.SelectedValue.ToString(), sourceadminusergroup.SelectedValue.ToString());
                UserGroups.ChangeAllUserGroupId(int.Parse(sourceusergroup.SelectedValue), int.Parse(targetadminusergroup.SelectedValue));
                
                //Data.DatabaseProvider.GetInstance().ChangeUsergroup(int.Parse(sourceadminusergroup.SelectedValue), int.Parse(targetadminusergroup.SelectedValue));

                DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "合并管理组", "把组ID:" + sourceusergroup.SelectedIndex + " 合并到组ID:" + targetusergroup.SelectedIndex);
                base.RegisterStartupScript("PAGE", "window.location.href='global_adminusergroupgrid.aspx';");
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
            this.ComUsergroup.Click += new EventHandler(this.ComUsergroup_Click);
            this.ComAdminUsergroup.Click += new EventHandler(this.ComAdminUsergroup_Click);
        }
        #endregion
    }
}