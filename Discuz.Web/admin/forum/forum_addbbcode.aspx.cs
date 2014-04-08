using System;
using System.Text.RegularExpressions;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ���Discuz!NT����
    /// </summary>
    public partial class addbbcode : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                icon.UpFilePath = Server.MapPath(icon.UpFilePath);
            }
        }

        /// <summary>
        /// ����Discuz!NT����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAdInfo_Click(object sender, EventArgs e)
        {
            #region ���Discuz!NT����

            if (this.CheckCookie())
            {
                BBCodes.CreateBBCCode(int.Parse(available.SelectedValue), Regex.Replace(tag.Text.Replace("<", "").Replace(">", ""), @"^[\>]|[\{]|[\}]|[\[]|[\]]|[\']|[\.]", ""),
                    icon.UpdateFile(), replacement.Text, example.Text, explanation.Text, param.Text, nest.Text, paramsdescript.Text, paramsdefvalue.Text);

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "���Discuz!NT����", "TAGΪ:" + tag.Text);

                base.RegisterStartupScript("", "<script>window.location.href='forum_bbcodegrid.aspx';</script>");
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
            this.AddAdInfo.Click += new EventHandler(this.AddAdInfo_Click);
        }

        #endregion
    }
}