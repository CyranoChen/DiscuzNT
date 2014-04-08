using System;
using System.Data;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.PasswordMode;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// �����û�����
    /// </summary>

    public partial class resetpassword : AdminPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (DNTRequest.GetString("uid") == "")
                {
                    Response.Write("�û�UID����Ϊ��");
                    Response.End();
                    return;
                }
                else
                {
                    #region �����û�����Ϣ

                    UserInfo userInfo = Users.GetUserInfo(int.Parse(DNTRequest.GetString("uid")));
                    if (userInfo != null)
                    {
                        userName.Text = userInfo.Username;
                    }
                    else
                    {
                        Response.Write("��ǰ�û�������");
                        Response.End();
                        return;
                    }

                    #endregion
                }
            }
        }


        private void ResetUserPWs_Click(object sender, EventArgs e)
        {
            #region �����û�����

            if (!Utils.StrIsNullOrEmpty(password.Text) && password.Text == passwordagain.Text)
            {
                UserInfo userInfo = Users.GetUserInfo(int.Parse(DNTRequest.GetString("uid")));
                userInfo.Password = password.Text.Trim();
                Users.ResetPassword(userInfo);
                //�����û�����ͬ��
                Discuz.Forum.Sync.UpdatePassword(userInfo.Username, userInfo.Password, "");

                base.RegisterStartupScript("PAGE", "window.location.href='global_edituser.aspx?uid=" + DNTRequest.GetString("uid") + "';");
            }
            else
            {
                base.RegisterStartupScript("", "<script>alert('�����벻��Ϊ��, ��������������������ͬ!');</script>");
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
            this.ResetUserPWs.Click += new EventHandler(this.ResetUserPWs_Click);
        }

        #endregion

    }
}