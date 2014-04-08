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
    /// ��������û���
    /// </summary>
    public partial class addusergroupspecial : AdminPage
    {

        public UserGroupInfo userGroupInfo = new UserGroupInfo();
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
                    SetGroupRights(DNTRequest.GetInt("groupid", 0));
            }
        }

        public void SetGroupRights(int groupid)
        {
            #region ������Ȩ�������Ϣ
            UserGroupInfo userGroupInfo = UserGroups.GetUserGroupInfo(groupid);
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
                        base.RegisterStartupScript("", "<script>alert('�������," + de.Key.ToString() + "ֻ����0����������');window.location.href='global_addusergroupspecial.aspx';</script>");
                        return;
                    }
                }

                LoadUserGroupInfo();
                if (AddUserGroupInfo())
                {
                    base.RegisterStartupScript("PAGE", "window.location.href='global_usergroupspecialgrid.aspx';");
                }
                else
                {
                    if (AdminUserGroups.opresult != "")
                    {
                        base.RegisterStartupScript("", "<script>alert('����ʧ��,ԭ��:" + AdminUserGroups.opresult + "');window.location.href='global_usergroupspecialgrid.aspx';</script>");
                    }
                    else
                    {
                        base.RegisterStartupScript("", "<script>alert('����ʧ��');window.location.href='global_usergroupspecialgrid.aspx';</script>");
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// ����Ϣװ�����û���Ϣ����
        /// </summary>
        private void LoadUserGroupInfo()
        {
            userGroupInfo.System = 0;
            userGroupInfo.Type = 0;
            userGroupInfo.Readaccess = Convert.ToInt32(readaccess.Text == "" ? "0" : readaccess.Text);
            userGroupInfo.Allowdirectpost = 1;
            userGroupInfo.Allowmultigroups = 0;
            userGroupInfo.Allowcstatus = 0;
            userGroupInfo.Allowuseblog = 0;
            userGroupInfo.Allowinvisible = 0;
            userGroupInfo.Allowtransfer = 0;
            userGroupInfo.Allowhtml = 0;
            userGroupInfo.Allownickname = 0;
            userGroupInfo.Allowviewstats = 0;
            userGroupInfo.Radminid = -1;
            userGroupInfo.Grouptitle = groupTitle.Text;
            userGroupInfo.Creditshigher = 0;
            userGroupInfo.Creditslower = 0;
            userGroupInfo.Stars = TypeConverter.StrToInt(stars.Text);
            userGroupInfo.Color = color.Text;
            userGroupInfo.Groupavatar = groupavatar.Text;
            userGroupInfo.Maxprice = TypeConverter.StrToInt(maxprice.Text);
            userGroupInfo.Maxpmnum = TypeConverter.StrToInt(maxpmnum.Text);
            userGroupInfo.Maxsigsize = TypeConverter.StrToInt(maxsigsize.Text);
            userGroupInfo.Maxattachsize = TypeConverter.StrToInt(maxattachsize.Text);
            userGroupInfo.Maxsizeperday = TypeConverter.StrToInt(maxsizeperday.Text);
            userGroupInfo.Maxspaceattachsize = TypeConverter.StrToInt(maxspaceattachsize.Text);
            userGroupInfo.Maxspacephotosize = TypeConverter.StrToInt(maxspacephotosize.Text);
            //userGroupInfo.MaxFriendsCount = TypeConverter.StrToInt(maxfriendscount.Text);
            userGroupInfo.Attachextensions = attachextensions.GetSelectString(",");
            userGroupInfo.Raterange = "";
            usergrouppowersetting.GetSetting(ref userGroupInfo);
        }

        /// <summary>
        /// �����û���Ϣ
        /// </summary>
        /// <returns>�ɹ���</returns>
        private bool AddUserGroupInfo()
        {
            if (AdminUserGroups.AddUserGroupInfo(userGroupInfo))
            {
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                UserGroups.GetUserGroupList();
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "��̨��������û���", "����:" + groupTitle.Text);
                return true;
            }
            return false;
        }

        #region Web ������������ɵĴ���

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷���������.
        /// </summary>
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