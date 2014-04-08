using System;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.PasswordMode;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ����û�
    /// </summary>
    
    public partial class adduser : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                #region ��ʼ���ؼ�
                foreach (UserGroupInfo userGroupInfo in UserGroups.GetUserGroupList())
                    groupid.Items.Add(new ListItem(userGroupInfo.Grouptitle,userGroupInfo.Groupid.ToString()));
                AddUserInfo.Attributes.Add("onclick", "return IsValidPost();");
                //�������������ݼ��ص�Javascript���飬��ǰ̨�ı�
                string scriptText = "var creditarray = new Array(";
                for(int i = 1; i < groupid.Items.Count; i++)
                {
                    scriptText += AdminUserGroups.AdminGetUserGroupInfo(Convert.ToInt32(groupid.Items[i].Value)).Creditshigher.ToString() + ",";
                }
                scriptText = scriptText.TrimEnd(',') + ");";
                this.RegisterStartupScript("begin", "<script type='text/javascript'>" + scriptText + "</script>");
                groupid.Attributes.Add("onchange", "document.getElementById('" + credits.ClientID + "').value=creditarray[this.selectedIndex];");
                groupid.Items.RemoveAt(0);
                try
                {
                    groupid.SelectedValue = "10";
                }
                catch
                {
                    //��������·������ʱ
                    groupid.SelectedValue = UserCredits.GetCreditsUserGroupId(0) != null ? UserCredits.GetCreditsUserGroupId(0).Groupid.ToString() : "3" ;
                }

                try
                {
                    UserGroupInfo _usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(Convert.ToInt32(groupid.SelectedValue));
                    credits.Text = _usergroupinfo.Creditshigher.ToString();
                }
                catch
                {
                    ;
                }

                #endregion
            }
        }

        private void AddUserInfo_Click(object sender, EventArgs e)
        {
            #region ������û���Ϣ
            if (this.CheckCookie())
            {
                if (userName.Text.Trim() == "" || password.Text.Trim() == "")
                {
                    base.RegisterStartupScript("", "<script>alert('�û���������Ϊ��,����޷��ύ!');window.location.href='global_adduser.aspx';</script>");
                    return;
                }
                if (!Utils.IsSafeSqlString(userName.Text))
                {
                    base.RegisterStartupScript( "", "<script>alert('��������û�����������ȫ���ַ�,����޷��ύ!');window.location.href='global_adduser.aspx';</script>");
                    return;
                }

                if (PrivateMessages.SystemUserName == userName.Text)
                {
                    base.RegisterStartupScript( "", "<script>alert('�����ܴ������û���,��Ϊ����ϵͳ�������û���,���������������û���!');window.location.href='global_adduser.aspx';</script>");
                    return;
                }

                if (!Utils.IsValidEmail(email.Text.Trim()))
                {
                    base.RegisterStartupScript("", "<script>alert('E-mailΪ�ջ��ʽ����ȷ,����޷��ύ!');window.location='global_adduser.aspx';</script>");
                    return;
                }

                UserInfo userInfo = CreateUserInfo();

                if (AdminUsers.GetUserId(userName.Text) > 0)
                {
                    base.RegisterStartupScript( "", "<script>alert('����������û����ѱ�ʹ�ù�, �������������û���!');window.location.href='global_adduser.aspx';</script>");
                    return;
                }

                if (!Users.ValidateEmail(email.Text))
                {
                    base.RegisterStartupScript("", "<script>alert('��������������ַ�ѱ�ʹ�ù�, ����������������!');window.location.href='global_adduser.aspx';</script>");
                    return;
                }

                if (config.Passwordmode > 1 && PasswordModeProvider.GetInstance() != null)
                    PasswordModeProvider.GetInstance().CreateUserInfo(userInfo);
                else
                {
                    userInfo.Password = Utils.MD5(userInfo.Password);
                    AdminUsers.CreateUser(userInfo);
                }
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "��̨����û�", "�û���:" + userName.Text);

                string emailresult = null;
                if (sendemail.Checked)
                {
                    emailresult = SendEmail(email.Text);
                }
                base.RegisterStartupScript( "PAGE", "window.location.href='global_usergrid.aspx';");
            }
            #endregion
        }

        private UserInfo CreateUserInfo()
        {
            UserInfo userInfo = new UserInfo();
            userInfo.Username = userName.Text;
            userInfo.Nickname = userName.Text;
            userInfo.Password = password.Text;
            userInfo.Secques = "";
            userInfo.Gender = 0;
            int selectgroupid = Convert.ToInt32(groupid.SelectedValue);
            userInfo.Adminid = AdminUserGroups.AdminGetUserGroupInfo(selectgroupid).Radminid;
            userInfo.Groupid = selectgroupid;
            userInfo.Groupexpiry = 0;
            userInfo.Extgroupids = "";
            userInfo.Regip = "";
            userInfo.Joindate = Utils.GetDate();
            userInfo.Lastip = "";
            userInfo.Lastvisit = Utils.GetDate();
            userInfo.Lastactivity = Utils.GetDate();
            userInfo.Lastpost = Utils.GetDate();
            userInfo.Lastpostid = 0;
            userInfo.Lastposttitle = "";
            userInfo.Posts = 0;
            userInfo.Digestposts = 0;
            userInfo.Oltime = 0;
            userInfo.Pageviews = 0;
            userInfo.Credits = Convert.ToInt32(credits.Text);
            userInfo.Extcredits1 = 0;
            userInfo.Extcredits2 = 0;
            userInfo.Extcredits3 = 0;
            userInfo.Extcredits4 = 0;
            userInfo.Extcredits5 = 0;
            userInfo.Extcredits6 = 0;
            userInfo.Extcredits7 = 0;
            userInfo.Extcredits8 = 0;
            userInfo.Salt = "0";
            //userInfo.Avatarshowid = 1;
            userInfo.Email = email.Text;
            userInfo.Bday = "";
            userInfo.Sigstatus = 0;

            userInfo.Templateid = GeneralConfigs.GetConfig().Templateid;
            userInfo.Tpp = 16;
            userInfo.Ppp = 16;
            userInfo.Pmsound = 1;
            userInfo.Showemail = 1;
            userInfo.Newsletter = (ReceivePMSettingType)7;
            userInfo.Invisible = 0;
            userInfo.Newpm = 0;
            userInfo.Accessmasks = 0;

            //��չ��Ϣ
            userInfo.Website = "";
            userInfo.Icq = "";
            userInfo.Qq = "";
            userInfo.Yahoo = "";
            userInfo.Msn = "";
            userInfo.Skype = "";
            userInfo.Location = "";
            userInfo.Customstatus = "";
            //userInfo.Avatar = "";
            //userInfo.Avatarwidth = 32;
            //userInfo.Avatarheight = 32;
            userInfo.Medals = "";
            userInfo.Bio = "";
            userInfo.Signature = userName.Text;
            userInfo.Sightml = "";
            userInfo.Authstr = "";
            userInfo.Realname = realname.Text;
            userInfo.Idcard = idcard.Text;
            userInfo.Mobile = mobile.Text;
            userInfo.Phone = phone.Text;
            return userInfo;
        }

        public string SendEmail(string emailaddress)
        {
            #region �����ʼ�
            bool send = Emails.DiscuzSmtpMail(userName.Text, emailaddress, password.Text);
            if (send)
                return "���������Ѿ��ɹ����͵�����E-mail��, ��ע�����!";
            return "�������ʼ�����, ��������ȡ������!";
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
            this.AddUserInfo.Click += new EventHandler(this.AddUserInfo_Click);

            userName.IsReplaceInvertedComma = false;
            password.IsReplaceInvertedComma = false;
            email.IsReplaceInvertedComma = false;
        }

        #endregion

    }
}
