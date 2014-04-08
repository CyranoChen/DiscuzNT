using System;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ͼ���ļ��б�
    /// </summary>
    public partial class iconfilegrid : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "��̳ͼ���ļ��б�";
            DataGrid1.BindData(DatabaseProvider.GetInstance().GetIcons());
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_Delete(Object sender, DataGridCommandEventArgs E)
        {
            int id = Utils.StrToInt(DataGrid1.DataKeys[E.Item.ItemIndex].ToString(), 0);
            DataGrid1.DeleteByString(DatabaseProvider.GetInstance().DeleteSmily(id));
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        protected void DataGrid_Edit(Object sender, DataGridCommandEventArgs E)
        {
            DataGrid1.EditByItemIndex(E.Item.ItemIndex);
        }

        protected void DataGrid_Cancel(Object sender, DataGridCommandEventArgs E)
        {
            DataGrid1.Cancel();
        }

        protected void DataGrid_Update(Object sender, DataGridCommandEventArgs E)
        {
            #region ������ص�ͼ����Ϣ

            int id = Utils.StrToInt(DataGrid1.DataKeys[E.Item.ItemIndex].ToString(), 0);
            int displayorder = Utils.StrToInt(((TextBox)E.Item.Cells[3].Controls[0]).Text, 0);
            string code = ((TextBox)E.Item.Cells[5].Controls[0]).Text;
            string url = ((TextBox)E.Item.Cells[6].Controls[0]).Text;

            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "�����ļ�����", code);

            try
            {
                DatabaseProvider.GetInstance().UpdateSmilies(id, displayorder, 1, code, url);
                BindData();
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UI/IconsList");
            }
            catch
            {
                base.RegisterStartupScript( "", "<script>alert('�޷��������ݿ�.');window.location.href='forum_iconfilegrid.aspx';</script>");
                return;
            }

            #endregion
        }



        private void DelRec_Click(object sender, EventArgs e)
        {
            #region ɾ��ָ���ı����¼

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("id") != "")
                {
                    DatabaseProvider.GetInstance().DeleteSmilies(DNTRequest.GetString("id").Replace("0 ", ""));
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UI/IconsList");
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "ͼ���ļ�ɾ��", "ID:" + DNTRequest.GetString("id").Replace("0 ", ""));
                    Response.Redirect("forum_iconfilegrid.aspx");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('��δѡ���κ�ѡ��');</script>");
                }
            }

            #endregion
        }


        public string PicStr(string filename)
        {
            if (filename.IndexOf("icon") >= 0)
            {
                return "<img src=../../images/posticons/" + filename.Replace("icon", "") + " />";
            }
            else
            {
                return "<img src=../../images/posticons/" + filename + " />";
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
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.DataGrid1.EditCommand += new DataGridCommandEventHandler(this.DataGrid_Edit);
            this.DataGrid1.CancelCommand += new DataGridCommandEventHandler(DataGrid_Cancel);
            this.DataGrid1.UpdateCommand += new DataGridCommandEventHandler(this.DataGrid_Update);
            this.Load += new EventHandler(this.Page_Load);

            DataGrid1.LoadEditColumn();
            DataGrid1.ColumnSpan = 7;

        }

        #endregion

    }
}