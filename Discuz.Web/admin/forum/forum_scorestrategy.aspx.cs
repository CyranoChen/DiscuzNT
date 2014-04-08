using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Cache;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 版块积分策略编辑
    /// </summary>
    public partial class scorestrategy : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if ((DNTRequest.GetString("fid") != "") && (DNTRequest.GetString("fieldname") != ""))
                {
                    LoadScoreInf(DNTRequest.GetString("fid"), DNTRequest.GetString("fieldname"));
                }
                else
                {
                    Response.Write("<script>alert('数据不存在');self.close();</script>");
                    Response.End();
                }
            }
        }

        public void LoadScoreInf(string fid, string fieldName)
        {
            #region 加载积分策略信息

            if (fieldName == "postcredits")
            {
                Literal1.Text = "发主题积分策略";
            }
            else
            {
                Literal1.Text = "发回复积分策略";
            }

            DataTable dt = Forums.GetForumField(Utils.StrToInt(fid, 0), fieldName);
            if (dt.Rows.Count == 0)
            {
                Response.Write("<script>alert('数据不存在');</script>");
            }
            else
            {
                string credits = dt.Rows[0][0].ToString().Trim();
                if ((credits != "") && (credits != "0"))
                {
                    string[] creditInfo = credits.Split(',');
                    available.SelectedValue = creditInfo[0].Trim();
                    extcredits1.Text = creditInfo[1].Trim();
                    extcredits2.Text = creditInfo[2].Trim();
                    extcredits3.Text = creditInfo[3].Trim();
                    extcredits4.Text = creditInfo[4].Trim();
                    extcredits5.Text = creditInfo[5].Trim();
                    extcredits6.Text = creditInfo[6].Trim();
                    extcredits7.Text = creditInfo[7].Trim();
                    extcredits8.Text = creditInfo[8].Trim();
                }
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

            if (this.CheckCookie())
            {
                Forums.UpdateForumField(DNTRequest.GetInt("fid", 0), DNTRequest.GetString("fieldname"), "''");
                base.RegisterStartupScript( "PAGE",  "window.location.href='forum_editforums.aspx?fid=" + DNTRequest.GetString("fid") + "&tabindex=1';");
            }

            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存设置信息

            if (this.CheckCookie())
            {
                string creditinf = available.SelectedValue + "," + extcredits1.Text + "," + extcredits2.Text + "," + extcredits3.Text + "," + extcredits4.Text + "," + extcredits5.Text + "," + extcredits6.Text + "," + extcredits7.Text + "," + extcredits8.Text;
                Forums.UpdateForumField(DNTRequest.GetInt("fid", 0), DNTRequest.GetString("fieldname"), creditinf);
                DNTCache.GetCacheService().RemoveObject("/Forum/ForumList");
                base.RegisterStartupScript( "PAGE", "window.location.href='forum_editforums.aspx?fid=" + DNTRequest.GetString("fid") + "&tabindex=1';");
            }

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
        }

        #endregion

    }
}