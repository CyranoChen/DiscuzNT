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
    /// �༭Discuz!NT����
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
            #region ���ص�ǰDiscuz!NT���������Ϣ

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
            #region ���µ�ǰDiscuz!NT������Ϣ

            if (this.CheckCookie())
            {
                SortedList sl = new SortedList();
                sl.Add("��������", param.Text);
                sl.Add("Ƕ�״���", nest.Text);

                foreach (DictionaryEntry s in sl)
                {
                    if (!Utils.IsInt(s.Value.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('�������:" + s.Key.ToString() + ",ֻ����0����������');window.location.href='forum_editbbcode.aspx';</script>");
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

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "����Discuz!NT����", "TABΪ:" + tag.Text);
                base.RegisterStartupScript( "PAGE", "window.location.href='forum_bbcodegrid.aspx';");
            }

            #endregion
        }

        private void DeleteBBCode_Click(object sender, EventArgs e)
        {
            #region ɾ����ǰDiscuz!NT������Ϣ

            if (this.CheckCookie())
            {
                //DbHelper.ExecuteNonQuery("DELETE FROM [" + BaseConfigs.GetTablePrefix + "bbcodes] WHERE [id]=" + DNTRequest.GetString("id"));
                BBCodes.DeleteBBCode(DNTRequest.GetString("id"));
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "ɾ��Discuz!NT����", "TABΪ:" + tag.Text);
                base.RegisterStartupScript( "PAGE", "window.location.href='forum_bbcodegrid.aspx';");
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
            this.UpdateBBCodeInfo.Click += new EventHandler(this.UpdateBBCodeInfo_Click);
            this.DeleteBBCode.Click += new EventHandler(this.DeleteBBCode_Click);
        }

        #endregion
    }
}