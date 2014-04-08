using System;
using System.Data;
using System.Collections;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.Album;
using Discuz.Plugin.Space;
using Discuz.Common.Generic;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ��ӹ�����
    /// </summary>
    public partial class addadminusergroup : AdminPage
    {
        protected bool haveAlbum;
        protected bool haveSpace;

        protected void Page_Load(object sender, EventArgs e)
        {
            haveAlbum = AlbumPluginProvider.GetInstance() != null;
            haveSpace = SpacePluginProvider.GetInstance() != null;
            if (!Page.IsPostBack)
                usergrouppowersetting.Bind();
        }

        public void SetGroupRights(string groupid)
        {
            #region ������Ȩ�������Ϣ
            UserGroupInfo userGroupInfo = AdminUserGroups.AdminGetUserGroupInfo(Convert.ToInt32(groupid));

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
            radminid.SelectedValue = userGroupInfo.Radminid.ToString();

            DataTable attachmentType = Attachments.GetAttachmentType();
            attachextensions.AddTableData(attachmentType,userGroupInfo.Attachextensions.ToString());

            //�����û�Ȩ�����ʼ����Ϣ
            usergrouppowersetting.Bind(userGroupInfo);

            AdminGroupInfo adminGroupInfo = AdminUserGroups.AdminGetAdminGroupInfo(Convert.ToInt32(groupid));
            if (adminGroupInfo != null)
            {
                admingroupright.Items[0].Selected = adminGroupInfo.Alloweditpost == 1;
                admingroupright.Items[1].Selected = adminGroupInfo.Alloweditpoll == 1;
                admingroupright.Items[2].Selected = adminGroupInfo.Allowdelpost == 1;
                admingroupright.Items[3].Selected = adminGroupInfo.Allowmassprune == 1;
                admingroupright.Items[4].Selected = adminGroupInfo.Allowviewip == 1;
                admingroupright.Items[5].Selected = adminGroupInfo.Allowedituser == 1;
                admingroupright.Items[6].Selected = adminGroupInfo.Allowviewlog == 1;
                admingroupright.Items[7].Selected = adminGroupInfo.Disablepostctrl == 1;
                admingroupright.Items[8].Selected = adminGroupInfo.Allowviewrealname == 1;
                admingroupright.Items[9].Selected = adminGroupInfo.Allowbanuser == 1;
                admingroupright.Items[10].Selected = adminGroupInfo.Allowbanip == 1;
                admingroupright.Items[11].Selected = adminGroupInfo.Allowmodpost == 1;
                admingroupright.Items[12].Selected = adminGroupInfo.Allowpostannounce == 1;
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                admingroupright.Items[13].Selected = ("," + configInfo.Reportusergroup + ",").IndexOf("," + groupid + ",") != -1; //�Ƿ�������վٱ���Ϣ
                admingroupright.Items[14].Selected = ("," + configInfo.Photomangegroups + ",").IndexOf("," + groupid + ",") != -1;//�Ƿ��������ͼƬ���� 
            }

            if (radminid.SelectedValue == "1")
            {
                allowstickthread.Enabled = false;
                allowstickthread.SelectedValue = "3";
            }
            else
            {
                allowstickthread.Enabled = true;
            }
            #endregion
        }

        public byte BoolToByte(bool a)
        {
            return (byte)(a ? 1 : 0);
        }

        private void AddUserGroupInf_Click(object sender, EventArgs e)
        {
            #region �����������Ϣ����

            if (this.CheckCookie())
            {
                if (radminid.SelectedValue == "0")
                {
                    base.RegisterStartupScript("", "<script>alert('����ʧ��,����ѡ����Ӧ�Ĺ�����, �ٵ���ύ��ť!');</script>");
                    return;
                }

                if (groupTitle.Text.Trim() == string.Empty)
                {
                    base.RegisterStartupScript("", "<script>alert('�û������Ʋ���Ϊ��!');</script>");
                    return;
                }

                Hashtable ht = new Hashtable();
                ht.Add("�������ߴ�", maxattachsize.Text);
                ht.Add("ÿ����󸽼��ܳߴ�", maxsizeperday.Text);
                ht.Add("���˿ռ丽���ܳߴ�", maxspaceattachsize.Text);
                ht.Add("���ռ��ܳߴ�", maxspacephotosize.Text);
                foreach (DictionaryEntry de in ht)
                {
                    if (!Utils.IsInt(de.Value.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('�������," + de.Key.ToString() + "ֻ����0����������');window.location.href='global_addadminusergroup.aspx';</script>");
                        return;
                    }
                }

                UserGroupInfo userGroupInfo = new UserGroupInfo();
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
                userGroupInfo.Raterange = "";
                userGroupInfo.Radminid = Convert.ToInt32(radminid.SelectedValue);
                //userGroupInfo.MaxFriendsCount = Convert.ToInt32(maxfriendscount.Text);
                usergrouppowersetting.GetSetting(ref userGroupInfo);
                if (AdminUserGroups.AddUserGroupInfo(userGroupInfo))
                {
                    #region �Ƿ�������վٱ���Ϣ�͹���ͼƬ����
                    GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                    //�Ƿ�������վٱ���Ϣ
                    int groupid = UserGroups.GetMaxUserGroupId();
                    if (admingroupright.Items[13].Selected)
                    {
                        if (("," + configInfo.Reportusergroup + ",").IndexOf("," + groupid + ",") == -1)
                        {
                            if (configInfo.Reportusergroup == "")
                            {
                                configInfo.Reportusergroup = groupid.ToString();
                            }
                            else
                            {
                                configInfo.Reportusergroup += "," + groupid.ToString();
                            }
                        }
                    }
                    //�Ƿ��������ͼƬ����
                    if (admingroupright.Items[14].Selected)
                    {
                        if (("," + configInfo.Photomangegroups + ",").IndexOf("," + groupid + ",") == -1)
                        {
                            if (configInfo.Photomangegroups == "")
                            {
                                configInfo.Photomangegroups = groupid.ToString();
                            }
                            else
                            {
                                configInfo.Photomangegroups += "," + groupid.ToString();
                            }
                        }
                    }
                    GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));
                    #endregion
                    AdminGroupInfo adminGroupInfo = new AdminGroupInfo();
                    //int adminId = DatabaseProvider.GetInstance().GetMaxUserGroupId() + 1;
                    adminGroupInfo.Admingid = (short)UserGroups.GetMaxUserGroupId();

                    //������Ӧ�Ĺ�����
                    adminGroupInfo.Alloweditpost = BoolToByte(admingroupright.Items[0].Selected);
                    adminGroupInfo.Alloweditpoll = BoolToByte(admingroupright.Items[1].Selected);
                    adminGroupInfo.Allowstickthread = (byte)Convert.ToInt16(allowstickthread.SelectedValue);
                    adminGroupInfo.Allowmodpost = 0;
                    adminGroupInfo.Allowdelpost = BoolToByte(admingroupright.Items[2].Selected);
                    adminGroupInfo.Allowmassprune = BoolToByte(admingroupright.Items[3].Selected);
                    adminGroupInfo.Allowrefund = 0;
                    adminGroupInfo.Allowcensorword = 0;
                    adminGroupInfo.Allowviewip = BoolToByte(admingroupright.Items[4].Selected);
                    adminGroupInfo.Allowbanip = 0;
                    adminGroupInfo.Allowedituser = BoolToByte(admingroupright.Items[5].Selected);
                    adminGroupInfo.Allowmoduser = 0;
                    adminGroupInfo.Allowbanuser = 0;
                    adminGroupInfo.Allowpostannounce = 0;
                    adminGroupInfo.Allowviewlog = BoolToByte(admingroupright.Items[6].Selected);
                    adminGroupInfo.Disablepostctrl = BoolToByte(admingroupright.Items[7].Selected);
                    adminGroupInfo.Allowviewrealname = BoolToByte(admingroupright.Items[8].Selected);
                    adminGroupInfo.Allowbanuser = BoolToByte(admingroupright.Items[9].Selected);
                    adminGroupInfo.Allowbanip = BoolToByte(admingroupright.Items[10].Selected);
                    adminGroupInfo.Allowmodpost = BoolToByte(admingroupright.Items[11].Selected);
                    adminGroupInfo.Allowpostannounce = BoolToByte(admingroupright.Items[12].Selected);

                    AdminGroups.CreateAdminGroupInfo(adminGroupInfo);
                
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "��̨��ӹ�����", "����:" + groupTitle.Text);

                    base.RegisterStartupScript("PAGE", "window.location.href='global_adminusergroupgrid.aspx';");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('����ʧ��');window.location.href='global_adminusergroupgrid.aspx';</script>");
                }
            }

            #endregion
        }


        private void radminid_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region ���û�����Ϣ
            SetGroupRights(radminid.SelectedValue);
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
            this.radminid.SelectedIndexChanged += new EventHandler(this.radminid_SelectedIndexChanged);

            DataTable dt = Attachments.GetAttachmentType();
            attachextensions.AddTableData(dt);
            radminid.Items.Add(new System.Web.UI.WebControls.ListItem("��ѡ��", "0"));
            List<UserGroupInfo> list = UserGroups.GetAdminUserGroup();
            foreach (UserGroupInfo userGroupInfo in list)
            {
                if (userGroupInfo.Groupid > 0 && userGroupInfo.Groupid <= 3)
                {
                    radminid.Items.Add(new System.Web.UI.WebControls.ListItem(userGroupInfo.Grouptitle, userGroupInfo.Groupid.ToString()));
                }
            }
            if (DNTRequest.GetString("groupid") != "")
            {
                SetGroupRights(DNTRequest.GetString("groupid"));
            }
        }

        #endregion


    }
}