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
    /// 重设用户密码
    /// </summary>

    public partial class resetpassword : AdminPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (DNTRequest.GetString("uid") == "")
                {
                    Response.Write("用户UID不能为空");
                    Response.End();
                    return;
                }
                else
                {
                    #region 加载用户名信息

                    UserInfo userInfo = Users.GetUserInfo(int.Parse(DNTRequest.GetString("uid")));
                    if (userInfo != null)
                    {
                        userName.Text = userInfo.Username;
                    }
                    else
                    {
                        Response.Write("当前用户不存在");
                        Response.End();
                        return;
                    }

                    #endregion
                }
            }
        }


        private void ResetUserPWs_Click(object sender, EventArgs e)
        {
            #region 重设用户密码

            if (!Utils.StrIsNullOrEmpty(password.Text) && password.Text == passwordagain.Text)
            {
                UserInfo userInfo = Users.GetUserInfo(int.Parse(DNTRequest.GetString("uid")));
                userInfo.Password = password.Text.Trim();
                Users.ResetPassword(userInfo);
                //更新用户密码同步
                Discuz.Forum.Sync.UpdatePassword(userInfo.Username, userInfo.Password, "");

                base.RegisterStartupScript("PAGE", "window.location.href='global_edituser.aspx?uid=" + DNTRequest.GetString("uid") + "';");
            }
            else
            {
                base.RegisterStartupScript("", "<script>alert('新密码不能为空, 且两次输入的密码必须相同!');</script>");
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
            this.ResetUserPWs.Click += new EventHandler(this.ResetUserPWs_Click);
        }

        #endregion

    }
}