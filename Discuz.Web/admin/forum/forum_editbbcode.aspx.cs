using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Collections;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 编辑Discuz!NT代码
    /// </summary>
    public partial class editbbcode : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (DNTRequest.GetString("id") == "")
            {
                Response.Redirect("forum_bbcodegrid.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    icon.UpFilePath = Server.MapPath(icon.UpFilePath);
                    LoadAnnounceInf(DNTRequest.GetInt("id", -1));
                }
            }
        }

        public void LoadAnnounceInf(int id)
        {
            #region 加载当前Discuz!NT代码相关信息

            DataTable dt = BBCodes.GetBBCode(id);
            if (dt.Rows.Count > 0)
            {
                available.SelectedValue = dt.Rows[0]["available"].ToString();
                tag.Text = dt.Rows[0]["tag"].ToString();
                replacement.Text = dt.Rows[0]["replacement"].ToString();
                example.Text = dt.Rows[0]["example"].ToString();
                explanation.Text = dt.Rows[0]["explanation"].ToString();
                paramsdescript.Text = dt.Rows[0]["paramsdescript"].ToString();
                paramsdefvalue.Text = dt.Rows[0]["paramsdefvalue"].ToString();
                nest.Text = dt.Rows[0]["nest"].ToString();
                param.Text = dt.Rows[0]["params"].ToString();
                icon.Text = dt.Rows[0]["icon"].ToString();
                ViewState["inco"] = dt.Rows[0]["icon"].ToString();
            }

            #endregion
        }

        private void UpdateBBCodeInfo_Click(object sender, EventArgs e)
        {
            #region 更新当前Discuz!NT代码信息

            if (this.CheckCookie())
            {
                SortedList sl = new SortedList();
                sl.Add("参数个数", param.Text);
                sl.Add("嵌套次数", nest.Text);

                foreach (DictionaryEntry s in sl)
                {
                    if (!Utils.IsInt(s.Value.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('输入错误:" + s.Key.ToString() + ",只能是0或者正整数');window.location.href='forum_editbbcode.aspx';</script>");
                        return;
                    }
                }
                string filepath = icon.UpdateFile();
                if (filepath=="")
                {
                 filepath = ViewState["inco"].ToString();
                }

                BBCodes.UpdateBBCode(
                    int.Parse(available.SelectedValue),
                    Regex.Replace(tag.Text.Replace("<", "").Replace(">", ""), @"^[\>]|[\{]|[\}]|[\[]|[\]]|[\']|[\.]", ""),
                    filepath, replacement.Text, example.Text, explanation.Text, param.Text, nest.Text, paramsdescript.Text,
                    paramsdefvalue.Text, DNTRequest.GetInt("id", 0));

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "更新Discuz!NT代码", "TAB为:" + tag.Text);
                base.RegisterStartupScript( "PAGE", "window.location.href='forum_bbcodegrid.aspx';");
            }

            #endregion
        }

        private void DeleteBBCode_Click(object sender, EventArgs e)
        {
            #region 删除当前Discuz!NT代码信息

            if (this.CheckCookie())
            {
                //DbHelper.ExecuteNonQuery("DELETE FROM [" + BaseConfigs.GetTablePrefix + "bbcodes] WHERE [id]=" + DNTRequest.GetString("id"));
                BBCodes.DeleteBBCode(DNTRequest.GetString("id"));
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除Discuz!NT代码", "TAB为:" + tag.Text);
                base.RegisterStartupScript( "PAGE", "window.location.href='forum_bbcodegrid.aspx';");
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
            this.UpdateBBCodeInfo.Click += new EventHandler(this.UpdateBBCodeInfo_Click);
            this.DeleteBBCode.Click += new EventHandler(this.DeleteBBCode_Click);
        }

        #endregion
    }
}