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
    /// �ϲ��û���
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
            #region �ϲ��û���
            if (this.CheckCookie())
            {
                if ((sourceusergroup.SelectedIndex == 0) || (targetusergroup.SelectedIndex == 0))
                {
                    base.RegisterStartupScript( "", "<script>alert('����ʧ��,����ѡ����Ч���û���!');</script>");
                    return;
                }

                if (sourceusergroup.SelectedValue == targetusergroup.SelectedValue)
                {
                    base.RegisterStartupScript( "", "<script>alert('����ʧ��,ͬһ���û��鲻�ܹ��ϲ�!');</script>");
                    return;
                }

                if (UserGroups.GetUserGroupInfo(int.Parse(sourceusergroup.SelectedValue)).Creditslower != 
                    UserGroups.GetUserGroupInfo(int.Parse(targetusergroup.SelectedValue)).Creditshigher)
                {
                    base.RegisterStartupScript("", "<script>alert('����ʧ��,Ҫ�ϲ����û�������ǻ��������������û���!');</script>");
                    return;
                }

                //�ϲ��û�����������
                UserGroupInfo userGroupInfo = UserGroups.GetUserGroupInfo(int.Parse(targetusergroup.SelectedValue));
                userGroupInfo.Creditshigher = UserGroups.GetUserGroupInfo(int.Parse(sourceusergroup.SelectedValue)).Creditshigher;
                UserGroups.UpdateUserGroup(userGroupInfo);
                //UserGroups.CombinationUserGroupScore(int.Parse(sourceusergroup.SelectedValue), int.Parse(targetusergroup.SelectedValue));
                //ɾ�����ϲ���Դ�û���
                //DatabaseProvider.GetInstance().DeleteUserGroupInfo(int.Parse(sourceusergroup.SelectedValue));
                UserGroups.DeleteUserGroupInfo(int.Parse(sourceusergroup.SelectedValue));

                //�����û����е���Ϣ
                //Data.DatabaseProvider.GetInstance().ChangeUsergroup(int.Parse(sourceusergroup.SelectedValue), int.Parse(targetusergroup.SelectedValue));
                UserGroups.ChangeAllUserGroupId(int.Parse(sourceusergroup.SelectedValue), int.Parse(targetusergroup.SelectedValue));

                DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "�ϲ��û���", "����ID:" + sourceusergroup.SelectedIndex + " �ϲ�����ID:" + targetusergroup.SelectedIndex);
                base.RegisterStartupScript("PAGE", "window.location.href='global_usergroupgrid.aspx';");
            }

            #endregion
        }

        private void ComAdminUsergroup_Click(object sender, EventArgs e)
        {
            #region �ϲ�������

            if (this.CheckCookie())
            {
                if ((sourceadminusergroup.SelectedIndex == 0) || (targetadminusergroup.SelectedIndex == 0))
                {
                    base.RegisterStartupScript( "", "<script>alert('����ʧ��,����ѡ����Ч�Ĺ�����!');</script>");
                    return;
                }

                if ((Convert.ToInt32(sourceadminusergroup.SelectedValue) <= 3) || (Convert.ToInt32(sourceadminusergroup.SelectedValue) <= 3))
                {
                    base.RegisterStartupScript( "", "<script>alert('����ʧ��,��ѡ�����Ϊϵͳ��ʼ���Ĺ�����,��Щ�鲻����ϲ�!');</script>");
                    return;
                }

                if (sourceadminusergroup.SelectedValue == targetadminusergroup.SelectedValue)
                {
                    base.RegisterStartupScript( "", "<script>alert('����ʧ��,ͬһ�������鲻�ܹ��ϲ�!');</script>");
                    return;
                }

                //ɾ�����ϲ���Դ�û���
                //DatabaseProvider.GetInstance().DeleteAdminGroupInfo(int.Parse(sourceadminusergroup.SelectedValue));
                AdminGroups.DeleteAdminGroupInfo(Convert.ToInt16(sourceadminusergroup.SelectedValue));

                //ɾ�����ϲ���Դ�û���
                //DatabaseProvider.GetInstance().DeleteUserGroupInfo(int.Parse(sourceadminusergroup.SelectedValue));
                UserGroups.DeleteUserGroupInfo(int.Parse(sourceadminusergroup.SelectedValue));
             
                //�����û����е���Ϣ
                //Data.DatabaseProvider.GetInstance().UpdateAdminUsergroup(targetadminusergroup.SelectedValue.ToString(), sourceadminusergroup.SelectedValue.ToString());
                UserGroups.ChangeAllUserGroupId(int.Parse(sourceusergroup.SelectedValue), int.Parse(targetadminusergroup.SelectedValue));
                
                //Data.DatabaseProvider.GetInstance().ChangeUsergroup(int.Parse(sourceadminusergroup.SelectedValue), int.Parse(targetadminusergroup.SelectedValue));

                DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "�ϲ�������", "����ID:" + sourceusergroup.SelectedIndex + " �ϲ�����ID:" + targetusergroup.SelectedIndex);
                base.RegisterStartupScript("PAGE", "window.location.href='global_adminusergroupgrid.aspx';");
            }

            #endregion
        }

        #region Web ������������ɵĴ���

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