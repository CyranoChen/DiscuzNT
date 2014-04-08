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
using System.Web.UI.WebControls;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 管理用户组编辑
    /// </summary>

    public partial class editadminusergroup : AdminPage
    {
        public AdminGroupInfo adminGroupInfo = new AdminGroupInfo();
        public UserGroupInfo userGroupInfo = new UserGroupInfo();
        protected bool haveAlbum;
        protected bool haveSpace;

        protected void Page_Load(object sender, EventArgs e)
        {
            haveAlbum = AlbumPluginProvider.GetInstance() != null;
            haveSpace = SpacePluginProvider.GetInstance() != null;
            if (!IsPostBack)
            {
                if (DNTRequest.GetString("groupid") != "")
                {
                    LoadUserGroupInf(DNTRequest.GetInt("groupid", -1));
                }
                else
                {
                    Response.Redirect("global_adminusergroupgrid.aspx");
                    return;
                }
                if (AlbumPluginProvider.GetInstance() == null)
                {
                    admingroupright.Items.RemoveAt(admingroupright.Items.Count - 1);
                }
            }
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
            //maxfriendscount.Text = userGroupInfo.MaxFriendsCount.ToString();

            if (groupid > 0 && groupid <= 3) radminid.Enabled = false;

            radminid.SelectedValue = userGroupInfo.Radminid.ToString();

            attachextensions.SetSelectByID(userGroupInfo.Attachextensions.Trim());

            //设置用户权限组初始化信息
            adminGroupInfo = AdminUserGroups.AdminGetAdminGroupInfo(userGroupInfo.Groupid);
            usergrouppowersetting.Bind(userGroupInfo);

            if (adminGroupInfo != null)
            {
                //设置管理权限组初始化信息
                admingroupright.SelectedIndex = -1;
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
                admingroupright.Items[13].Selected = ("," + configInfo.Reportusergroup + ",").IndexOf("," + groupid + ",") != -1; //是否允许接收举报信息
                admingroupright.Items[admingroupright.Items.Count - 1].Selected = ("," + configInfo.Photomangegroups + ",").IndexOf("," + groupid + ",") != -1;//是否允许管理图片评论
                if (adminGroupInfo.Allowstickthread.ToString() != "") allowstickthread.SelectedValue = adminGroupInfo.Allowstickthread.ToString();

            }

            if (radminid.SelectedValue == "1")
            {
                allowstickthread.Enabled = false;
                allowstickthread.SelectedValue = "3";
            }

            #endregion
        }

        private void DeleteUserGroupInf_Click(object sender, EventArgs e)
        {
            #region 删除相关组信息

            if (this.CheckCookie())
            {
                if (AdminUserGroups.DeleteUserGroupInfo(DNTRequest.GetInt("groupid", -1)))
                {
                    //删除举报组
                    GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                    string tempstr = "";
                    foreach (string report in configInfo.Reportusergroup.Split(','))
                    {
                        if (report != userGroupInfo.Groupid.ToString())
                        {
                            if (tempstr == "")
                                tempstr = report;
                            else
                                tempstr += "," + report;
                        }
                    }
                    configInfo.Reportusergroup = tempstr;
                    tempstr = "";
                    foreach (string photomangegroup in configInfo.Photomangegroups.Split(','))
                    {
                        if (photomangegroup != userGroupInfo.Groupid.ToString())
                        {
                            if (tempstr == "")
                                tempstr = photomangegroup;
                            else
                                tempstr += "," + photomangegroup;
                        }
                    }
                    configInfo.Photomangegroups = tempstr;
                    GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AdminGroupList");
                    Discuz.Forum.AdminGroups.GetAdminGroupList();
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除管理组", "组ID:" + DNTRequest.GetInt("groupid", -1));
                    base.RegisterStartupScript("PAGE", "window.location.href='global_adminusergroupgrid.aspx';");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='global_adminusergroupgrid.aspx';</script>");
                }
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
            #region 更新管理组信息

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
                        base.RegisterStartupScript("", "<script>alert('输入错误," + de.Key.ToString() + "只能是0或者正整数');window.location.href='global_editadminusergroup.aspx';</script>");
                        return;
                    }

                }
                userGroupInfo = AdminUserGroups.AdminGetUserGroupInfo(DNTRequest.GetInt("groupid", -1));
                userGroupInfo.System = 0;
                userGroupInfo.Type = 0;
                userGroupInfo.Readaccess = Convert.ToInt32(readaccess.Text);

                int selectradminid = Convert.ToInt32(radminid.SelectedValue);
                //对于当前用户组中,有管理权限的,则设置管理权限
                if (selectradminid > 0 && selectradminid <= 3)
                {
                    adminGroupInfo = new AdminGroupInfo();
                    adminGroupInfo.Admingid = (short)userGroupInfo.Groupid;
                    //插入相应的管理组
                    adminGroupInfo.Alloweditpost = BoolToByte(admingroupright.Items[0].Selected);
                    adminGroupInfo.Alloweditpoll = BoolToByte(admingroupright.Items[1].Selected);
                    adminGroupInfo.Allowstickthread = (byte)Convert.ToInt16(allowstickthread.SelectedValue);
                    adminGroupInfo.Allowmodpost = 0;
                    adminGroupInfo.Allowdelpost = BoolToByte(admingroupright.Items[2].Selected);
                    adminGroupInfo.Allowmassprune = BoolToByte(admingroupright.Items[3].Selected);
                    adminGroupInfo.Allowrefund = 0;
                    adminGroupInfo.Allowcensorword = 0; ;
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

                    Discuz.Forum.AdminGroups.SetAdminGroupInfo(adminGroupInfo, userGroupInfo.Groupid);
                    userGroupInfo.Radminid = selectradminid;
                }
                else
                    userGroupInfo.Radminid = 0;

                AdminGroups.ChangeUserAdminidByGroupid(userGroupInfo.Radminid, userGroupInfo.Groupid);

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
                //userGroupInfo.MaxFriendsCount = Convert.ToInt32(maxfriendscount.Text);
                userGroupInfo.Attachextensions = attachextensions.GetSelectString(",");

                usergrouppowersetting.GetSetting(ref userGroupInfo);


                if (AdminUserGroups.UpdateUserGroupInfo(userGroupInfo))
                {
                    #region 是否允许接收举报信息和管理图片评论
                    GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                    //是否允许接收举报信息
                    int groupid = userGroupInfo.Groupid;
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
                    else
                    {
                        string tempstr = "";
                        foreach (string report in configInfo.Reportusergroup.Split(','))
                        {
                            if (report != groupid.ToString())
                            {
                                if (tempstr == "")
                                {
                                    tempstr = report;
                                }
                                else
                                {
                                    tempstr += "," + report;
                                }
                            }
                        }
                        configInfo.Reportusergroup = tempstr;
                    }
                    //是否允许管理图片评论
                    if (AlbumPluginProvider.GetInstance() != null)
                    {
                        if (admingroupright.Items[admingroupright.Items.Count - 1].Selected)
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
                        else
                        {
                            string tempstr = "";
                            foreach (string photomangegroup in configInfo.Photomangegroups.Split(','))
                            {
                                if (photomangegroup != groupid.ToString())
                                {
                                    if (tempstr == "")
                                    {
                                        tempstr = photomangegroup;
                                    }
                                    else
                                    {
                                        tempstr += "," + photomangegroup;
                                    }
                                }
                            }
                            configInfo.Photomangegroups = tempstr;
                        }
                    }

                    GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));
                    #endregion
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");

                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台更新管理组", "组名:" + groupTitle.Text);
                    base.RegisterStartupScript("PAGE", "window.location.href='global_adminusergroupgrid.aspx';");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='global_adminusergroupgrid.aspx';</script>");
                }
            }

            #endregion
        }


        private void radminid_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region 绑定关联组
            //DataTable usergrouprightstable = Discuz.Data.DatabaseProvider.GetInstance().GetUserGroupInfoByGroupid(int.Parse(radminid.SelectedValue));
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
                admingroupright.Items[8].Selected = radminUserGroup.Allowviewrealname == 1;
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

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }


        private void InitializeComponent()
        {
            this.TabControl1.InitTabPage();
            this.radminid.SelectedIndexChanged += new EventHandler(this.radminid_SelectedIndexChanged);
            this.UpdateUserGroupInf.Click += new EventHandler(this.UpdateUserGroupInf_Click);
            this.DeleteUserGroupInf.Click += new EventHandler(this.DeleteUserGroupInf_Click);
            //this.Load += new EventHandler(this.Page_Load);

            radminid.Items.Add(new ListItem("请选择     ", "0"));
            foreach (UserGroupInfo userGroupInfo in UserGroups.GetAdminUserGroup())
            {
                if (userGroupInfo.Groupid > 0 && userGroupInfo.Groupid <= 3)
                {
                    radminid.Items.Add(new ListItem(userGroupInfo.Grouptitle, userGroupInfo.Groupid.ToString()));
                }
            }
            DataTable dt = Attachments.GetAttachmentType();
            attachextensions.AddTableData(dt);
        }

        #endregion
    }
}