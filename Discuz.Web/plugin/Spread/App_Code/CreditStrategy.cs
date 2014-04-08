using System;
using Discuz.Forum;

using System.Data;
using Discuz.Plugin.Spread.Config;
using Discuz.Web.Admin;

namespace Discuz.Plugin.Spread.Admin
{
#if NET1
	public class CreditStrategy : AdminPage
#else
    public partial class CreditStrategy : AdminPage
#endif
    {

#if NET1
		#region 控件声明
		protected System.Web.UI.HtmlControls.HtmlForm Form1;
		protected System.Web.UI.WebControls.Literal Literal1;
		protected Discuz.Control.TextBox txtTransferUrl;
		protected System.Web.UI.WebControls.Literal extcredits1name;
		protected Discuz.Control.TextBox extcredits1;
		protected System.Web.UI.WebControls.Literal extcredits2name;
		protected Discuz.Control.TextBox extcredits2;
		protected System.Web.UI.WebControls.Literal extcredits3name;
		protected Discuz.Control.TextBox extcredits3;
		protected System.Web.UI.WebControls.Literal extcredits4name;
		protected Discuz.Control.TextBox extcredits4;
		protected System.Web.UI.WebControls.Literal extcredits5name;
		protected Discuz.Control.TextBox extcredits5;
		protected System.Web.UI.WebControls.Literal extcredits6name;
		protected Discuz.Control.TextBox extcredits6;
		protected System.Web.UI.WebControls.Literal extcredits7name;
		protected Discuz.Control.TextBox extcredits7;
		protected System.Web.UI.WebControls.Literal extcredits8name;
		protected Discuz.Control.TextBox extcredits8;
		protected Discuz.Control.Button SaveInfo;
		protected Discuz.Control.Button DeleteSet;
		#endregion
#endif
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadScoreInf();
            }
        }

        public void LoadScoreInf()
        {
            #region 加载积分策略信息

            Literal1.Text = "转向地址和推广积分策略";
            txtTransferUrl.Text = Spread.Config.SpreadConfigs.GetConfig().TransferUrl;

            string credits = Spread.Config.SpreadConfigs.GetConfig().SpreadCredits;
            if ((credits != "") && (credits != "0"))
            {
                string[] creditinf = credits.Split(',');
                extcredits1.Text = creditinf[0].Trim();
                extcredits2.Text = creditinf[1].Trim();
                extcredits3.Text = creditinf[2].Trim();
                extcredits4.Text = creditinf[3].Trim();
                extcredits5.Text = creditinf[4].Trim();
                extcredits6.Text = creditinf[5].Trim();
                extcredits7.Text = creditinf[6].Trim();
                extcredits8.Text = creditinf[7].Trim();
            }
            else
            {
                extcredits1.Text = "";
                extcredits2.Text = "";
                extcredits3.Text = "";
                extcredits4.Text = "";
                extcredits5.Text = "";
                extcredits6.Text = "";
                extcredits7.Text = "";
                extcredits8.Text = "";
            }

            DataRow dr = Scoresets.GetScoreSet().Rows[0];

            if (dr[2].ToString().Trim() != "")
            {
                extcredits1name.Text = dr[2].ToString().Trim();
            }
            else
            {
                extcredits1.Enabled = false;
            }

            if (dr[3].ToString().Trim() != "")
            {
                extcredits2name.Text = dr[3].ToString().Trim();
            }
            else
            {
                extcredits2.Enabled = false;
            }

            if (dr[4].ToString().Trim() != "")
            {
                extcredits3name.Text = dr[4].ToString().Trim();
            }
            else
            {
                extcredits3.Enabled = false;
            }

            if (dr[5].ToString().Trim() != "")
            {
                extcredits4name.Text = dr[5].ToString().Trim();
            }
            else
            {
                extcredits4.Enabled = false;
            }

            if (dr[6].ToString().Trim() != "")
            {
                extcredits5name.Text = dr[6].ToString().Trim();
            }
            else
            {
                extcredits5.Enabled = false;
            }

            if (dr[7].ToString().Trim() != "")
            {
                extcredits6name.Text = dr[7].ToString().Trim();
            }
            else
            {
                extcredits6.Enabled = false;
            }

            if (dr[8].ToString().Trim() != "")
            {
                extcredits7name.Text = dr[8].ToString().Trim();
            }
            else
            {
                extcredits7.Enabled = false;
            }

            if (dr[9].ToString().Trim() != "")
            {
                extcredits8name.Text = dr[9].ToString().Trim();
            }
            else
            {
                extcredits8.Enabled = false;
            }

            #endregion
        }

        private void DeleteSet_Click(object sender, EventArgs e)
        {
            #region 删除设置

            SpreadConfigInfo config = SpreadConfigs.GetConfig();
            config.SpreadCredits = "";
            config.TransferUrl = "";
            Config.SpreadConfigs.SaveConfig(config);
            //base.RegisterStartupScript("PAGE", "window.location.href='CreditStrategy.aspx';");
            LoadScoreInf();

            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存设置信息

            string creditinf = extcredits1.Text + "," + extcredits2.Text + "," + extcredits3.Text + "," + extcredits4.Text + "," + extcredits5.Text + "," + extcredits6.Text + "," + extcredits7.Text + "," + extcredits8.Text;

            SpreadConfigInfo config = SpreadConfigs.GetConfig();
            config.SpreadCredits = creditinf;
            config.TransferUrl = txtTransferUrl.Text;
            Config.SpreadConfigs.SaveConfig(config);
            //base.RegisterStartupScript("PAGE", "window.location.href='CreditStrategy.aspx';");
            LoadScoreInf();

            #endregion
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
            this.DeleteSet.Click += new EventHandler(this.DeleteSet_Click);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion

    }
}
