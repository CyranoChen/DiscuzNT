using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    public partial class forum_userrights : AdminPage
    {
        private GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadConfigInfo();
                //LoadUserGroup();
                //foreach (string groupid in configInfo.Htmltitleusergroup.Split(','))
                //{
                //    for (int i = 0; i < UserGroup.Items.Count; i++)
                //    {
                //        if (UserGroup.Items[i].Value == groupid)
                //        {
                //            UserGroup.Items[i].Selected = true;
                //            break;
                //        }
                //    }
                //}
            }
        }

        public void LoadConfigInfo()
        {
            #region 加载配置信息
            dupkarmarate.SelectedValue = configInfo.Dupkarmarate.ToString();
            minpostsize.Text = configInfo.Minpostsize.ToString();
            maxpostsize.Text = configInfo.Maxpostsize.ToString();
            maxfavorites.Text = configInfo.Maxfavorites.ToString();
            maxpolloptions.Text = configInfo.Maxpolloptions.ToString();
            maxattachments.Text = configInfo.Maxattachments.ToString();
            karmaratelimit.Text = configInfo.Karmaratelimit.ToString();
            moderactions.SelectedValue = configInfo.Moderactions.ToString();
            edittimelimit.Text = configInfo.Edittimelimit.ToString();
            deletetimelimit.Text = configInfo.Deletetimelimit.ToString();
            //allowusesearchfriend.SelectedValue = configInfo.Allowsearchfriendbyusername.ToString();
            //maxfriendgroups.Text = configInfo.Friendgroupmaxcount.ToString();
            #endregion
        }

        //private void LoadUserGroup()
        //{
        //    #region 加载用户组
        //    UserGroup.DataSource = UserGroups.GetUserGroupForDataTable();
        //    UserGroup.DataValueField = "groupid";
        //    UserGroup.DataTextField = "grouptitle";
        //    UserGroup.DataBind();
        //    #endregion
        //}

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存设置信息

            if (this.CheckCookie())
            {
                if (TypeConverter.StrToInt(edittimelimit.Text) > 9999999 || TypeConverter.StrToInt(edittimelimit.Text) < -1)
                {
                    base.RegisterStartupScript("", "<script>alert('编辑帖子时间限制只能在-1-9999999之间');window.location.href='forum_option.aspx';</script>");
                    return;
                }

                if (TypeConverter.StrToInt(deletetimelimit.Text) > 9999999 || TypeConverter.StrToInt(deletetimelimit.Text) < -1)
                {
                    base.RegisterStartupScript("", "<script>alert('删除帖子时间限制只能在-1-9999999之间');window.location.href='forum_option.aspx';</script>");
                    return;
                }

                if (Convert.ToInt32(minpostsize.Text) > 9999999 || (Convert.ToInt32(minpostsize.Text) < 0))
                {
                    base.RegisterStartupScript("", "<script>alert('帖子最小字数只能在0-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }

                if (Convert.ToInt32(maxpostsize.Text) > 9999999 || (Convert.ToInt32(maxpostsize.Text) < 0))
                {
                    base.RegisterStartupScript("", "<script>alert('帖子最大字数只能在0-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }

                if (Convert.ToInt32(maxfavorites.Text) > 9999999 || (Convert.ToInt32(maxfavorites.Text) < 0))
                {
                    base.RegisterStartupScript("", "<script>alert('收藏夹容量只能在0-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }

                if (Convert.ToInt32(maxpolloptions.Text) > 9999999 || (Convert.ToInt32(maxpolloptions.Text) < 0))
                {
                    base.RegisterStartupScript("", "<script>alert('最大签名高度只能在0-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }

                if (Convert.ToInt32(maxattachments.Text) > 9999999 || (Convert.ToInt32(maxattachments.Text) < 0))
                {
                    base.RegisterStartupScript("", "<script>alert('投票最大选项数只能在0-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }
                if (Convert.ToInt32(karmaratelimit.Text) > 9999 || (Convert.ToInt32(karmaratelimit.Text) < 0))
                {
                    base.RegisterStartupScript("", "<script>alert('评分时间限制只能在0-9999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }


                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();

                configInfo.Dupkarmarate = Convert.ToInt16(dupkarmarate.SelectedValue);
                configInfo.Minpostsize = Convert.ToInt32(minpostsize.Text);
                configInfo.Maxpostsize = Convert.ToInt32(maxpostsize.Text);
                configInfo.Maxfavorites = Convert.ToInt32(maxfavorites.Text);
                configInfo.Maxpolloptions = Convert.ToInt32(maxpolloptions.Text);
                configInfo.Maxattachments = Convert.ToInt32(maxattachments.Text);
                configInfo.Karmaratelimit = Convert.ToInt16(karmaratelimit.Text);
                configInfo.Moderactions = Convert.ToInt16(moderactions.SelectedValue);
                configInfo.Edittimelimit = TypeConverter.StrToInt(edittimelimit.Text);
                configInfo.Deletetimelimit = TypeConverter.StrToInt(deletetimelimit.Text);
                //configInfo.Allowsearchfriendbyusername = Convert.ToInt16(allowusesearchfriend.SelectedValue);
                //configInfo.Friendgroupmaxcount = Convert.ToInt16(maxfriendgroups.Text);

                //string groupList = "";
                //for (int i = 0; i < UserGroup.Items.Count; i++)
                //{
                //    if (UserGroup.Items[i].Selected)
                //    {
                //        groupList += UserGroup.Items[i].Value + ",";
                //    }
                //}
               // configInfo.Htmltitleusergroup = groupList.TrimEnd(',');

                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "用户权限设置", "");
                base.RegisterStartupScript("PAGE", "window.location.href='forum_userrights.aspx';");
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
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
        }

        #endregion
    }
}
