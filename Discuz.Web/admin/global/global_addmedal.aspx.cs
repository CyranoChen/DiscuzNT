using System;
using System.Data;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 添加勋章信息
    /// </summary>
    
    public partial class addmedal : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DataTable dt = Medals.GetMedal();
                if (dt.Rows.Count >= 100)
                {
                    base.RegisterStartupScript( "", "<script>alert('勋章列表记录已经达到99枚,因此系统不再允许添加勋章');window.location.href='global_medalgrid.aspx';</script>");
                    return;
                }
                image.UpFilePath = Utils.GetMapPath("../../images/medals");
            }
        }

        public void AddMedalInfo_Click(object sender, EventArgs e)
        {
            #region 添加勋章节
            if (this.CheckCookie())
            {
                if (image.Text == "")
                {
                    base.RegisterStartupScript( "", "<script>alert('上传图片不能为空');</script>");
                    return;
                }
                Medals.CreateMedal(name.Text, int.Parse(available.SelectedValue), image.Text);
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "勋章文件添加", name.Text);
                base.RegisterStartupScript( "PAGE", "window.location.href='global_medalgrid.aspx';");
            }
            #endregion
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        #endregion
    }
}