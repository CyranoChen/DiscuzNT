using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 添加图标文件
    /// </summary>
    public partial class addiconfile : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                url.UpFilePath = Server.MapPath(url.UpFilePath);
            }
        }

        private void AddIncoInfo_Click(object sender, EventArgs e)
        {
            #region 添加图标记录

            if (this.CheckCookie())
            {
                AdminForums.CreateSmilies(Utils.StrToInt(displayorder.Text, 0), 1, code.Text, url.UpdateFile(),userid,username,usergroupid,grouptitle,ip);

                base.RegisterStartupScript( "PAGE", "window.location.href='forum_iconfilegrid.aspx';");
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
            this.AddIncoInfo.Click += new EventHandler(this.AddIncoInfo_Click);
        }

        #endregion

    }
}