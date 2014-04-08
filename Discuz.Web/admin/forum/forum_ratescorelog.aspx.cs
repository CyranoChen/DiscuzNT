using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ������־�б�
    /// </summary>
    
    public partial class ratescorelog : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                postdatetimeStart.SelectedDate = DateTime.Now.AddDays(-30);
                postdatetimeEnd.SelectedDate = DateTime.Now;
                BindData();
            }
        }

        public void BindData()
        {
            #region ���ݰ�

            DataGrid1.AllowCustomPaging = true;
            DataGrid1.VirtualItemCount = GetRecordCount();

            if (ViewState["condition"] == null)
            {
                DataGrid1.DataSource = AdminRateLogs.GetRateLogList(DataGrid1.PageSize, DataGrid1.CurrentPageIndex + 1);
            }
            else
            {
                DataGrid1.DataSource = AdminRateLogs.LogList(DataGrid1.PageSize, DataGrid1.CurrentPageIndex + 1, ViewState["condition"].ToString());
            }
            DataGrid1.DataBind();

            #endregion
        }

        //private void DelRec_Click(object sender, EventArgs e)
        //{
        //    #region ɾ��ָ����������־��Ϣ

        //    if (this.CheckCookie())
        //    {
        //        string condition = "";
        //        condition = AdminModeratorLogs.GetDeleteModeratorManageCondition(Request.Form["deleteMode"].ToString(), 
        //            DNTRequest.GetString("id").ToString(), deleteNum.Text.ToString(), deleteFrom.SelectedDate.ToString());
        //        if (condition != "")
        //        {
        //            AdminRateLogs.DeleteLog(condition);
        //            Response.Redirect("forum_ratescorelog.aspx");
        //        }
        //        else
        //        {
        //            base.RegisterStartupScript( "", "<script>alert('��δѡ���κ�ѡ��������������');window.location.href='forum_ratescorelog.aspx';</script>");
        //        }
        //    }
        //    #endregionȥ���������
        //}

        public string ExtcreditsStr(string extcredits, string score)
        {
            #region ��ȡ��չ�����ֶβ���ʾ

            DataRow dr = Scoresets.GetScoreSet().Rows[0];

            string extcredit = "";
            switch (extcredits)
            {
                case "1":
                    extcredit = dr["extcredits1"].ToString() + " " + score;
                    break;
                case "2":
                    extcredit = dr["extcredits2"].ToString() + " " + score;
                    break;
                case "3":
                    extcredit = dr["extcredits3"].ToString() + " " + score;
                    break;
                case "4":
                    extcredit = dr["extcredits4"].ToString() + " " + score;
                    break;
                case "5":
                    extcredit = dr["extcredits5"].ToString() + " " + score;
                    break;
                case "6":
                    extcredit = dr["extcredits6"].ToString() + " " + score;
                    break;
                case "7":
                    extcredit = dr["extcredits7"].ToString() + " " + score;
                    break;
                case "8":
                    extcredit = dr["extcredits8"].ToString() + " " + score;
                    break;
            }
            return extcredit;

            #endregion
        }

        private void SearchLog_Click(object sender, EventArgs e)
        {
            #region ��ָ����ѯ����������־��Ϣ

            if (this.CheckCookie())
            {
                string sqlstring = AdminRateLogs.GetSearchRateLogCondition(postdatetimeStart.SelectedDate, postdatetimeEnd.SelectedDate, 
                    Username.Text, others.Text);

                ViewState["condition"] = sqlstring;
                DataGrid1.CurrentPageIndex = 0;
                BindData();
            }

            #endregion
        }

        private int GetRecordCount()
        {
            #region �õ���־��¼��

            if (ViewState["condition"] == null)
            {
                return AdminRateLogs.RecordCount();
            }
            else
            {
                return AdminRateLogs.RecordCount(ViewState["condition"].ToString());
            }

            #endregion
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }

        public void GoToPagerButton_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.Cells[9].Text.ToString().Length > 8)
            {
                e.Item.Cells[9].Text = Utils.HtmlEncode(e.Item.Cells[9].Text.Substring(0, 8)) + "��";
            }
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SearchLog.Click += new EventHandler(this.SearchLog_Click);
            //this.DelRec.Click += new EventHandler(this.DelRec_Click);ȥ���������
            this.DataGrid1.GoToPagerButton.Click += new EventHandler(GoToPagerButton_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);

            DataGrid1.TableHeaderName = "�û�������־";
            DataGrid1.AllowSorting = false;
            DataGrid1.ColumnSpan = 8;
        }

        #endregion
    }
}