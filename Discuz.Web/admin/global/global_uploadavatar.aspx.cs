using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 上传用户头像
    /// </summary>
    
   public partial class uploadavatar : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                url.UpFilePath = Server.MapPath(url.UpFilePath);
            }
        }

        private void UpdateAvatarCache_Click(object sender, EventArgs e)
        {
            if (this.CheckCookie())
            {
                Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
                cache.RemoveObject("/Forum/CommonAvatarList");
                url.UpdateFile();
                base.RegisterStartupScript( "PAGE",  "window.location.href='global_avatargrid.aspx';");
            }
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.UpdateAvatarCache.Click += new EventHandler(this.UpdateAvatarCache_Click);
        }

        #endregion
    }
}