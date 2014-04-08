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
    /// �༭�û���
    /// </summary>

    public partial class editusergroup : AdminPage
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
            #region �����������Ϣ

            userGroupInfo = AdminUserGroups.AdminGetUserGroupInfo(groupid);

            groupTitle.Text = Utils.RemoveFontTag(userGroupInfo.Grouptitle);
            creditshigher.Text = userGroupInfo.Creditshigher.ToString();
            creditslower.Text = userGroupInfo.Creditslower.ToString();

            DataTable dt = UserGroups.GetUserGroupExceptGroupid(groupid);
            if (dt.Rows.Count == 0)
            {
                creditshigher.Enabled = false;
                creditslower.Enabled = false;
            }

            ViewState["creditshigher"] = userGroupInfo.Creditshigher.ToString();
            ViewState["creditslower"] = userGroupInfo.Creditslower.ToString();

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


            dt = Attachments.GetAttachmentType();
            attachextensions.SetSelectByID(userGroupInfo.Attachextensions.Trim());
            //��Ȩ����Ϣ
            usergrouppowersetting.Bind(userGroupInfo);

            if (userGroupInfo.System == 1) DeleteUserGroupInf.Enabled = false;

            #endregion
        }

        private void DeleteUserGroupInf_Click(object sender, EventArgs e)
        {
            #region ɾ���������Ϣ
            if (this.CheckCookie())
            {
                int groupid = DNTRequest.GetInt("groupid", -1);
                if (AdminUserGroups.DeleteUserGroupInfo(groupid))
                {
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                    Discuz.Forum.UserGroups.GetUserGroupList();
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "��̨ɾ���û���", "��ID:" + DNTRequest.GetInt("groupid", -1));
                    base.RegisterStartupScript("PAGE", "window.location.href='global_usergroupgrid.aspx';");
                }
                else
                {
                    if (AdminUserGroups.opresult != "")
                    {
                        base.RegisterStartupScript("", "<script>alert('����ʧ��,ԭ��:" + AdminUserGroups.opresult + "');window.location.href='global_usergroupgrid.aspx';</script>");
                    }
                    else
                    {
                        base.RegisterStartupScript("", "<script>alert('����ʧ��');window.location.href='global_usergroupgrid.aspx';</script>");
                    }
                }
            }
            #endregion
        }

        public int BoolToInt(bool a)
        {
            return a ? 1 : 0;
        }


        private void UpdateUserGroupInf_Click(object sender, EventArgs e)
        {
            #region �����û�����Ϣ

            if (this.CheckCookie())
            {

                Hashtable ht = new Hashtable();
                ht.Add("�������ߴ�", maxattachsize.Text);
                ht.Add("ÿ����󸽼��ܳߴ�", maxsizeperday.Text);
                ht.Add("���˿ռ丽���ܳߴ�", maxspaceattachsize.Text);
                ht.Add("���ռ��ܳߴ�", maxspacephotosize.Text);

                foreach (DictionaryEntry de in ht)
                {
                    if (!Utils.IsInt(de.Value.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('�������," + de.Key.ToString() + "ֻ����0����������');window.location.href='global_editusergroup.aspx';</script>");
                        return;
                    }

                }

                if (creditshigher.Enabled == true)
                {
                    if (Convert.ToInt32(creditshigher.Text) < Convert.ToInt32(ViewState["creditshigher"].ToString()) || Convert.ToInt32(creditslower.Text) > Convert.ToInt32(ViewState["creditslower"].ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('����ʧ��, ��������Ļ��������޷�ΧӦ��" + ViewState["creditshigher"].ToString() + "��" + ViewState["creditslower"].ToString() + "֮��');</script>");
                        return;
                    }
                }

                userGroupInfo = AdminUserGroups.AdminGetUserGroupInfo(DNTRequest.GetInt("groupid", -1));
                userGroupInfo.System = 0;
                userGroupInfo.Type = 0;
                userGroupInfo.Readaccess = Convert.ToInt32(readaccess.Text);
                usergrouppowersetting.GetSetting(ref userGroupInfo);
                userGroupInfo.Grouptitle = groupTitle.Text;

                userGroupInfo.Creditshigher = Convert.ToInt32(creditshigher.Text);
                userGroupInfo.Creditslower = Convert.ToInt32(creditslower.Text);

                if (userGroupInfo.Creditshigher >= userGroupInfo.Creditslower)
                {
                    base.RegisterStartupScript("", "<script>alert('����ʧ��, �������ޱ���С�ڻ�������');</script>");
                    return;
                }
                if (userGroupInfo.Allowbonus == 1 && (userGroupInfo.Minbonusprice >= userGroupInfo.Maxbonusprice))
                {
                    base.RegisterStartupScript("", "<script>alert('����ʧ��, ������ͼ۸����С��������ͼ۸�');</script>");
                    return;
                }

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
                //userGroupInfo.MaxFriendsCount = Convert.ToInt32(maxfriendscount.Text);

                if (AdminUserGroups.UpdateUserGroupInfo(userGroupInfo))
                {
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                    Discuz.Forum.UserGroups.GetUserGroupList();
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "��̨�����û���", "��ID:" + DNTRequest.GetInt("groupid", -1));
                    base.RegisterStartupScript("PAGE", "window.location.href='global_usergroupgrid.aspx';");
                }
                else
                {
                    if (AdminUserGroups.opresult != "")
                    {
                        base.RegisterStartupScript("", "<script>alert('����ʧ��,ԭ��:" + AdminUserGroups.opresult + "');window.location.href='global_usergroupgrid.aspx';</script>");
                    }
                    else
                    {
                        base.RegisterStartupScript("", "<script>alert('����ʧ��');window.location.href='global_usergroupgrid.aspx';</script>");
                    }

                }
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
            this.TabControl1.InitTabPage();
            this.UpdateUserGroupInf.Click += new EventHandler(this.UpdateUserGroupInf_Click);
            this.DeleteUserGroupInf.Click += new EventHandler(this.DeleteUserGroupInf_Click);

            DataTable dt = Attachments.GetAttachmentType();

            attachextensions.AddTableData(dt);

            if (DNTRequest.GetString("groupid") != "")
            {
                LoadUserGroupInf(DNTRequest.GetInt("groupid", -1));
            }
            else
            {
                Response.Redirect("global_usergroupgrid.aspx");
            }
        }
        #endregion
    }
}