using System;
using System.Data;
using System.Data.Common;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ����û�
    /// </summary>

    public partial class auditnewuser : AdminPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            #region ������û��б�
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "����û��б�";
            DataGrid1.DataKeyField = "uid";
            DataTable auditUsersTable = Users.GetUserListByGroupid(8);
            DataGrid1.BindData(auditUsersTable);
            AllDelete.Enabled = auditUsersTable.Rows.Count > 0;
            AllPass.Enabled = auditUsersTable.Rows.Count > 0;
            #endregion
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }


        private void SelectPass_Click(object sender, EventArgs e)
        {
            #region ��ѡ���û���������Ӧ���û���

            if (this.CheckCookie())
            {
                string uidList = DNTRequest.GetString("uid");
                if (uidList != "")
                {
                    //���û���������Ӧ���û���
                    if (Discuz.Forum.UserCredits.GetCreditsUserGroupId(0) != null)
                    {
                        int tmpGroupId = UserCredits.GetCreditsUserGroupId(0).Groupid; //���ע���û���˻��ƺ���Ҫ�޸�
                        Users.UpdateUserGroupByUidList(tmpGroupId, uidList);
                        foreach (string uid in uidList.Split(','))
                        {
                            UserCredits.UpdateUserCredits(Convert.ToInt32(uid));
                        }

                        Users.ClearUsersAuthstr(uidList);
                    }
                    if (sendemail.Checked)
                    {
                        Users.SendEmailForAccountCreateSucceed(uidList);
                    }
                    base.RegisterStartupScript( "PAGE", "window.location='forum_audituser.aspx';");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('��ѡ����Ӧ���û�!');window.location='forum_audituser.aspx';</script>");
                }
            }

            #endregion
        }

        private void SelectDelete_Click(object sender, EventArgs e)
        {
            #region ɾ��ѡ�е��û���Ϣ

            if (this.CheckCookie())
            {
                string uidlist = DNTRequest.GetString("uid");
                if (uidlist != "")
                {
                    Users.DeleteUsers(uidlist);
                    base.RegisterStartupScript("PAGE", "window.location='forum_audituser.aspx';");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('��ѡ����Ӧ���û�!');window.location='forum_audituser.aspx';</script>");
                }
            }

            #endregion
        }

        private void AllPass_Click(object sender, EventArgs e)
        {
            #region ���û���������Ӧ���û���

            if (this.CheckCookie())
            {
                if (UserCredits.GetCreditsUserGroupId(0) != null)
                {
                    int tmpGroupId = Discuz.Forum.UserCredits.GetCreditsUserGroupId(0).Groupid; //���ע���û���˻��ƺ���Ҫ�޸�
                    UserGroups.ChangeAllUserGroupId(8, tmpGroupId); ;
                    foreach (DataRow dr in Users.GetUserListByGroupid(8).Rows)
                    {
                        UserCredits.UpdateUserCredits(Convert.ToInt32(dr["uid"].ToString()));
                    }
                    Users.ClearUsersAuthstrByUncheckedUserGroup();
                }

                if (sendemail.Checked)
                {
                    Users.SendEmailForUncheckedUserGroup();
                }
                base.RegisterStartupScript("PAGE", "window.location='forum_audituser.aspx';");
            }

            #endregion
        }

        private void AllDelete_Click(object sender, EventArgs e)
        {
            #region ɾ�����д�����û������Ϣ

            if (this.CheckCookie())
            {
                Users.DeleteAuditUser();
                base.RegisterStartupScript("PAGE", "window.location='forum_audituser.aspx';");
            }

            #endregion
        }

        //public void SendEmail()
        //{
        //    #region ������ͨ����˵��û������ʼ�

        //    //foreach (DataRow dr in DbHelper.ExecuteDataset("SELECT [username],[password],[email] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]=8").Tables[0].Rows)
        //    foreach (DataRow dr in Users.GetUserListByGroupid(8).Rows)
        //    {
        //        Emails.DiscuzSmtpMail(dr["username"].ToString().Trim(), dr["email"].ToString().Trim(), dr["password"].ToString().Trim());
        //    }

        //    #endregion
        //}

        //public void SendEmail(string uidlist)
        //{
        //    #region ��ָ����ͨ����˵��û������ʼ�

        //    foreach (DataRow dr in Users.GetUsersByUidlLst(uidlist).Rows)
        //    {
        //        Emails.DiscuzSmtpMail(dr["username"].ToString().Trim(), dr["email"].ToString().Trim(), "");
        //    }

        //    #endregion
        //}

        protected void searchuser_Click(object sender, System.EventArgs e)
        {
            if (this.CheckCookie())
                DataGrid1.BindData(Users.AuditNewUserClear(searchusername.Text, regbefore.Text, regip.Text));
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SelectPass.Click += new EventHandler(this.SelectPass_Click);
            this.SelectDelete.Click += new EventHandler(this.SelectDelete_Click);
            this.AllPass.Click += new EventHandler(this.AllPass_Click);
            this.AllDelete.Click += new EventHandler(this.AllDelete_Click);

            DataGrid1.DataKeyField = "uid";
            DataGrid1.TableHeaderName = "����û��б�";
            DataGrid1.ColumnSpan = 8;
        }

        #endregion
    }
}