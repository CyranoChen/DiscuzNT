using System;
using System.Data;
using System.Threading;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 用户组邮件发送. 
    /// </summary>
     
    public partial class usergroupsendemail : AdminPage
    {
        public string groupidlist = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EmailConfigInfo __emailinfo = EmailConfigs.GetConfig();
                GeneralConfigInfo configinfo = GeneralConfigs.GetConfig();

                string strbody = __emailinfo.Emailcontent.Replace("{forumtitle}", configinfo.Forumtitle);
                strbody = strbody.Replace("{forumurl}", "<a href=" + configinfo.Forumurl + ">" + configinfo.Forumurl + "</a>");
                strbody = strbody.Replace("{webtitle}", configinfo.Webtitle);
                strbody = strbody.Replace("{weburl}", "<a href=" + configinfo.Forumurl + ">" + configinfo.Weburl + "</a>");
                body.Text = strbody;

            }
            if (DNTRequest.GetString("flag") == "1")
            {
                this.ExportUserEmails();
            }
        }


        private void BatchSendEmail_Click(object sender, EventArgs e)
        {
            #region 批量发送邮件

            if (this.CheckCookie())
            {
                groupidlist = Usergroups.GetSelectString(",");

                if (groupidlist == "" && usernamelist.Text.Trim() == "")
                {
                    base.RegisterStartupScript("", "<script>alert('您需要输入接收邮件用户名称或选取相关的用户组,因此邮件无法发送');</script>");
                    return;
                }

                int percount = 5; //每多少记录为一次等待

                //发送用户列表邮件
                if (usernamelist.Text.Trim() != "")
                {
                    DataTable dt = Users.GetEmailListByUserNameList(usernamelist.Text);
                    if (dt.Rows.Count <= 0)
                    {
                        base.RegisterStartupScript("", "<script>alert('您输入的接收邮件用户名未能查找到相关用户,因此邮件无法发送');</script>");
                        return;
                    }
                    Thread[] lThreads = new Thread[dt.Rows.Count];
                    int count = 0;

                    foreach (DataRow dr in dt.Rows)
                    {
                        EmailMultiThread emt = new EmailMultiThread(dr["UserName"].ToString(), dr["Email"].ToString(), subject.Text, body.Text);
                        lThreads[count] = new Thread(new ThreadStart(emt.Send));
                        lThreads[count].Start();

                        if (count >= percount)
                        {
                            Thread.Sleep(5000);
                            count = 0;
                        }
                        count++;
                    }
                }

                if (groupidlist == "")
                {
                    base.LoadRegisterStartupScript("PAGE", "window.location.href='global_usergroupsendemail.aspx';");
                    return;
                }
                ClientScript.RegisterStartupScript(this.GetType(), "Page", "<script>submit_Click();</script>");
            }

            #endregion
        }

        private void ExportUserEmails()
        {
            string groupidlist="";

            if (this.CheckCookie())
            {
                groupidlist = Usergroups.GetSelectString(",");
            }


            if (groupidlist == "")
            {
                return;
            }

            DataTable dt = Users.GetEmailListByGroupidList(groupidlist);
            
            string words = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    words += dt.Rows[i][1].ToString().Trim() + "; ";
                }
            }

            string filename = "Useremail.txt";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = false;
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(filename));
            HttpContext.Current.Response.ContentType = "text/plain";
            this.EnableViewState = false;
            HttpContext.Current.Response.Write(words);
            HttpContext.Current.Response.End();
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.BatchSendEmail.Click += new EventHandler(this.BatchSendEmail_Click);
            DataTable dt = UserGroups.GetUserGroupWithOutGuestTitle();
            foreach (DataRow dr in dt.Rows)
            {
                dr["grouptitle"] = "<img src=../images/usergroup.GIF border=0  style=\"position:relative;top:2 ;height:18 ;\">" + dr["grouptitle"];
            }
            Usergroups.AddTableData(dt);
        }

        #endregion
    }
}