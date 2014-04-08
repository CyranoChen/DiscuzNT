using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 合并用户
    /// </summary>
    /// 
    public partial class combinationuser : AdminPage
    {
        private void CombinationUserInfo_Click(object sender, EventArgs e)
        {
            #region 合并用户
            if (this.CheckCookie())
            {
                int targetuid = Users.GetUserId(targetusername.Text);
                string result = "";
                if (targetuid > 0)
                {
                    result = CombinationUser(username1.Text.Trim(), targetusername.Text.Trim(), targetuid);
                    result += CombinationUser(username2.Text.Trim(), targetusername.Text.Trim(), targetuid);
                    result += CombinationUser(username3.Text.Trim(), targetusername.Text.Trim(), targetuid);
                }
                else
                    result += "目标用户:" + targetusername.Text + "不存在!,";

                if (result == "")
                    base.RegisterStartupScript( "PAGE",  "window.location.href='global_usergrid.aspx';");
                else
                    base.RegisterStartupScript( "", "<script>alert('" + result.Replace("'", "’") + "');</script>");
            }
            #endregion
        }

        private string CombinationUser(string userName, string targetUserName, int targetUid)
        {
            int srcuid = 0;
            string result = "";
            if ((userName != "") && (targetUserName != userName))
            {
                srcuid = Users.GetUserId(userName);
                if (srcuid > 0)
                {
                    AdminUsers.CombinationUser(srcuid, targetUid);
                    AdminUsers.UpdateForumsFieldModerators(userName);
                    AdminVistLogs.InsertLog(userid, username, usergroupid, grouptitle, ip, "合并用户", "把用户" + userName + " 合并到" + targetUserName);
                }
                else
                    result = "用户:" + userName + "不存在!,";
            }
            return result;
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.CombinationUserInfo.Click += new EventHandler(this.CombinationUserInfo_Click);
        }

        #endregion

    }
}