using System;
using System.Web.UI;
using System.Data;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 添加表情
    /// </summary>
    public partial class addsmile : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string smilieUrl = "";
                foreach (DataRow dr in Smilies.GetSmilies().Rows)
                {
                    if (dr["id"].ToString() == DNTRequest.GetString("typeid"))
                        smilieUrl = dr["url"].ToString();
                }
                url.UpFilePath = Server.MapPath(string.Format("../../editor/images/smilies/{0}/", smilieUrl));
            }
        }

        private void AddSmileInfo_Click(object sender, EventArgs e)
        {
            #region 添加表情记录

            if (this.CheckCookie())
            {
                AdminForums.CreateSmilies(int.Parse(displayorder.Text), 
                    DNTRequest.GetInt("typeid", 0), code.Text,
                    Forum.Smilies.GetSmiliesTypeById(DNTRequest.GetInt("typeid", 0)).Url + "/" + url.UpdateFile(),
                    userid, username, usergroupid, grouptitle, ip
                    );
                base.RegisterStartupScript( "PAGE", "window.location.href='forum_smilegrid.aspx?typeid=" + DNTRequest.GetInt("typeid", 0) + "';");
            }

            #endregion
        }

        #region Web 窗体设计器生成的代码

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AddSmileInfo.Click += new EventHandler(this.AddSmileInfo_Click);
        }

        #endregion
    }
}