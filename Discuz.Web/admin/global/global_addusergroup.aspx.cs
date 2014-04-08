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
    /// ����û���
    /// </summary>

    public partial class addusergroup : AdminPage
    {
        protected bool haveAlbum;
        protected bool haveSpace;

        protected void Page_Load(object sender, EventArgs e)
        {
            haveAlbum = AlbumPluginProvider.GetInstance() != null;
            haveSpace = SpacePluginProvider.GetInstance() != null;
            if (!Page.IsPostBack)
            {
                usergrouppowersetting.Bind();
                if (DNTRequest.GetString("groupid") != "")
                {
                    SetGroupRights(DNTRequest.GetInt("groupid", 0));
                }
            }
        }

        public void SetGroupRights(int groupid)
        {
            #region ������Ȩ�������Ϣ
            UserGroupInfo userGroupInfo = UserGroups.GetUserGroupInfo(groupid);
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
            //maxfriendscount.Text = userGroupInfo.MaxFriendsCount.ToString();
            #endregion
        }

        private void AddUserGroupInf_Click(object sender, EventArgs e)
        {
            #region �����������Ϣ����

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
                UserGroupInfo userGroupInfo = new UserGroupInfo();
                userGroupInfo.System = 0;
                userGroupInfo.Type = 0;
                userGroupInfo.Readaccess = Convert.ToInt32(readaccess.Text == "" ? "0" : readaccess.Text);
                userGroupInfo.Radminid = 0;
                userGroupInfo.Grouptitle = groupTitle.Text;
                userGroupInfo.Creditshigher = Convert.ToInt32(creditshigher.Text);
                userGroupInfo.Creditslower = Convert.ToInt32(creditslower.Text);
                usergrouppowersetting.GetSetting(ref userGroupInfo);
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
                userGroupInfo.Raterange = "";

                if (AdminUserGroups.AddUserGroupInfo(userGroupInfo))
                {
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                    UserGroups.GetUserGroupList();

                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "��̨����û���", "����:" + groupTitle.Text);

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
            this.AddUserGroupInf.Click += new EventHandler(this.AddUserGroupInf_Click);

            DataTable dt = Attachments.GetAttachmentType();
            attachextensions.AddTableData(dt);
        }

        #endregion

    }
}