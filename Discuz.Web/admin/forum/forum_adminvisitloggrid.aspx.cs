using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using TextBox = Discuz.Control.TextBox;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ��̨������־�б�
    /// </summary>
    public partial class adminvisitloggrid : AdminPage
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
            DataGrid1.AllowCustomPaging = true;
            DataGrid1.VirtualItemCount = GetRecordCount();
            DataGrid1.DataSource = buildGridData();
            DataGrid1.DataBind();
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

        private DataTable buildGridData()
        {
            return (ViewState["condition"] == null) ? AdminVistLogs.LogList(DataGrid1.PageSize, DataGrid1.CurrentPageIndex + 1) : 
                AdminVistLogs.LogList(DataGrid1.PageSize, DataGrid1.CurrentPageIndex + 1, ViewState["condition"].ToString());
        }

        private int GetRecordCount()
        {
            #region �õ���־��¼��

            return (ViewState["condition"] == null) ? AdminVistLogs.RecordCount() : AdminVistLogs.RecordCount(ViewState["condition"].ToString());

            #endregion
        }


        private void DelRec_Click(object sender, EventArgs e)
        {
            #region ɾ��ָ����������־��Ϣ

            if (this.CheckCookie())
            {
                string condition = "";

                condition = AdminVistLogs.DelVisitLogCondition(Request.Form["deleteMode"].ToString(), DNTRequest.GetString("visitid").ToString(), deleteNum.Text, deleteFrom.SelectedDate.ToString());
                if (condition != "")
                {
                    AdminVistLogs.DeleteLog(condition);
                    Response.Redirect("forum_adminvisitloggrid.aspx");
                }                      
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('��δѡ���κ�ѡ��������������');window.location.href='forum_adminvisitloggrid.aspx';</script>");
                }                                                                                                                                           
            }

            #endregion
        }

        public string BoolStr(string closed)
        {
            #region ����ͼ��

            return (closed == "1") ? "<div align=center><img src=../images/OK.gif /></div>" : "<div align=center><img src=../images/Cancel.gif /></div>";

            #endregion
        }

        private void SearchLog_Click(object sender, EventArgs e)
        {
            #region ��ָ������������̨������־

            if (this.CheckCookie())
            {
                string sqlstring = AdminVistLogs.GetVisitLogCondition(postdatetimeStart.SelectedDate, postdatetimeEnd.SelectedDate, 
                    Username.Text, others.Text);               

                ViewState["condition"] = sqlstring;
                DataGrid1.CurrentPageIndex = 0;
                BindData();
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
            this.SearchLog.Click += new EventHandler(this.SearchLog_Click);
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.DataGrid1.GoToPagerButton.Click += new EventHandler(GoToPagerButton_Click);

            DataGrid1.TableHeaderName = "��̨���ʼ�¼";
            DataGrid1.AllowSorting = false;
            DataGrid1.DataKeyField = "visitid";
            DataGrid1.ColumnSpan = 7;
        }

        #endregion
    }
}