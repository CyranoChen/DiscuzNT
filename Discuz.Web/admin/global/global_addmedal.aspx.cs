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
    /// ���ѫ����Ϣ
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
                    base.RegisterStartupScript( "", "<script>alert('ѫ���б��¼�Ѿ��ﵽ99ö,���ϵͳ�����������ѫ��');window.location.href='global_medalgrid.aspx';</script>");
                    return;
                }
                image.UpFilePath = Utils.GetMapPath("../../images/medals");
            }
        }

        public void AddMedalInfo_Click(object sender, EventArgs e)
        {
            #region ���ѫ�½�
            if (this.CheckCookie())
            {
                if (image.Text == "")
                {
                    base.RegisterStartupScript( "", "<script>alert('�ϴ�ͼƬ����Ϊ��');</script>");
                    return;
                }
                Medals.CreateMedal(name.Text, int.Parse(available.SelectedValue), image.Text);
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "ѫ���ļ����", name.Text);
                base.RegisterStartupScript( "PAGE", "window.location.href='global_medalgrid.aspx';");
            }
            #endregion
        }

        #region Web ������������ɵĴ���

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        #endregion
    }
}