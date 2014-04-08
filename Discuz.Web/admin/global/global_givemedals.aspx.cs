using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using TextBox = Discuz.Control.TextBox;
using Discuz.Config;
using Discuz.Entity;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 给予勋章
    /// </summary>
    
    public partial class givemedals : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (DNTRequest.GetString("uid") == "") return;
                else
                {
                    int uid = DNTRequest.GetInt("uid", -1);
                    UserInfo ui = Discuz.Forum.Users.GetUserInfo(uid);
                    givenusername.Text = ui.Username;

                    if (ui.Medals.Trim() == "")
                    {
                        ui.Medals = "0";
                    }

                    LoadDataInfo("," + ui.Medals + ",");
                }
            }
        }

        public void LoadDataInfo(string begivenmedal)
        {
            #region 加载数据用绑定到控件

            //DataTable dt = DbHelper.ExecuteDataset("SELECT * FROM [" + BaseConfigs.GetTablePrefix + "medals] WHERE [available]=1").Tables[0];
            DataTable dt = Medals.GetAvailableMedal(); ;

            if (dt != null)
            {
                DataColumn dc = new DataColumn();
                dc.ColumnName = "isgiven";
                dc.DataType = Type.GetType("System.Boolean");
                dc.DefaultValue = false;
                dc.AllowDBNull = false;
                dt.Columns.Add(dc);

                foreach (DataRow dr in dt.Rows)
                {
                    if (begivenmedal.IndexOf("," + dr["medalid"].ToString() + ",") >= 0)
                    {
                        dr["isgiven"] = true;
                    }
                }
                medallist.DataSource = dt;
                medallist.DataBind();
            }

            #endregion
        }

        public string BeGivenMedal(string isgiven, string medalid)
        {
            #region 勋章的显示方式

            if (isgiven == "True")
            {
                return "<INPUT id=\"medalid\" type=\"checkbox\" value=\"" + medalid + "\"  name=\"medalid\" checked>";
            }
            else
            {
                return "<INPUT id=\"medalid\" type=\"checkbox\" value=\"" + medalid + "\"  name=\"medalid\">";
            }

            #endregion
        }

        private void GivenMedal_Click(object sender, EventArgs e)
        {
            #region 给予勋章

            if (this.CheckCookie())
            {
                int uid = DNTRequest.GetInt("uid", -1);
                Users.UpdateMedals(uid, DNTRequest.GetString("medalid"), userid, username, DNTRequest.GetIP(), reason.Text.Trim());

                if (DNTRequest.GetString("codition") == "")
                {
                    Session["codition"] = null;
                }
                else
                {
                    Session["codition"] = DNTRequest.GetString("codition").Replace("^", "'");
                }

                base.RegisterStartupScript( "PAGE", "window.location.href='global_edituser.aspx?uid=" + uid + "&condition=" + DNTRequest.GetString("condition") + "';");
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
            this.GivenMedal.Click += new EventHandler(this.GivenMedal_Click);
        }

        #endregion

    }
}