using System;
using System.Data;
using System.Data.Common;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ���������б�.
    /// </summary>
    public partial class attachtypesgrid : AdminPage
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
            DataGrid1.TableHeaderName = "�ϴ����������б�";
            DataGrid1.BindData(Attachments.GetAttachmentType());
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region �󶨸���������ʾ��ʽ

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                TextBox t = (TextBox)e.Item.Cells[3].Controls[0];
                t.Attributes.Add("maxlength", "255");
                t.Attributes.Add("size", "30");

                t = (TextBox)e.Item.Cells[4].Controls[0];
                t.Attributes.Add("maxlength", "9");
                t.Attributes.Add("size", "10");
            }

            if ((e.Item.ItemType == ListItemType.AlternatingItem) || (e.Item.ItemType == ListItemType.Item))
            {
                if (e.Item.Cells[3].Text.ToString().Length > 40)
                {
                    e.Item.Cells[3].Text = e.Item.Cells[3].Text.Substring(0, 40) + "��";
                }
            }

            #endregion
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            #region ɾ����صĸ�������

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("id") != "")
                {
                    Attachments.DeleteAttchType(DNTRequest.GetString("id"));
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "ɾ����������", "ɾ����������,IDΪ:" + DNTRequest.GetString("id").Replace("0 ", ""));

                    Response.Redirect("forum_attachtypesgrid.aspx");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('��δѡ���κ�ѡ��');window.location.href='forum_attachtypesgrid.aspx';</script>");
                }
            }

            #endregion
        }

        private void AddNewRec_Click(object sender, EventArgs e)
        {
            #region ����µĸ�����Ϣ

            if (extension.Text == "")
            {
                base.RegisterStartupScript( "", "<script>alert('Ҫ��ӵĸ�����չ������Ϊ��');window.location.href='forum_attachtypesgrid.aspx';</script>");
                return;
            }

            if ((maxsize.Text == "") || (Convert.ToInt32(maxsize.Text) <= 0))
            {
                base.RegisterStartupScript( "", "<script>alert('Ҫ��ӵĸ������ߴ粻��Ϊ����Ҫ����0');window.location.href='forum_attachtypesgrid.aspx';</script>");
                return;
            }


            //if (DbHelper.ExecuteDataset("Select Top 1  * From [" + BaseConfigs.GetTablePrefix + "attachtypes] WHERE [extension]='" + extension.Text + "'").Tables[0].Rows.Count > 0)
            foreach (DataRow dr in Attachments.GetAttachmentType().Rows)
            {
                if (dr["extension"].ToString() == extension.Text)
                {
                    base.RegisterStartupScript("", "<script>alert('���ݿ����Ѵ�����ͬ�ĸ�����չ��');window.location.href='forum_attachtypesgrid.aspx';</script>");
                    return;
                }
            }
            
            //if(Discuz.Data.DatabaseProvider.GetInstance().IsExistExtensionInAttachtypes(extension.Text))
            //{
            //    base.RegisterStartupScript( "", "<script>alert('���ݿ����Ѵ�����ͬ�ĸ�����չ��');window.location.href='forum_attachtypesgrid.aspx';</script>");
            //    return;
            //}

            //string sql = string.Format("INSERT INTO [" + BaseConfigs.GetTablePrefix + "attachtypes] ([extension], [maxsize]) VALUES ('{0}','{1}')",
            //    extension.Text,
            //    maxsize.Text
            //    );
            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "��Ӹ�������", "��Ӹ�������,��չ��Ϊ:" + extension.Text);
            try
            {
                //DataGrid1.Insert(sql);
                Attachments.AddAttchType(extension.Text, maxsize.Text);   
                base.RegisterStartupScript( "PAGE", "window.location.href='forum_attachtypesgrid.aspx';");
                return;
            }
            catch
            {
                base.RegisterStartupScript( "", "<script>alert('�޷��������ݿ�.');window.location.href='forum_attachtypesgrid.aspx';</script>");
                return;
            }

            #endregion
        }

        private void SaveAttachType_Click(object sender, EventArgs e)
        {
            #region ���渽�������޸�
            int rowid = 0;
            bool error = false;
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                string extension = DataGrid1.GetControlValue(rowid, "extension").Trim();
                string maxsize = DataGrid1.GetControlValue(rowid, "maxsize").Trim();
                if ((extension == "") || (maxsize == ""))
                {
                    error = true;
                    continue;
                }
                Attachments.UpdateAttchType(extension, maxsize, int.Parse(o.ToString()));
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "�༭��������", "�༭��������,��չ��Ϊ:" + extension);
                rowid++;
            }
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/ForumSetting/AttachmentType");
            if(error)
                base.RegisterStartupScript("", "<script>alert('ĳЩ��¼ȡֵ����ȷ��δ�ܱ����£�');window.location.href='forum_attachtypesgrid.aspx';</script>");
            else
                base.RegisterStartupScript("PAGE", "window.location.href='forum_attachtypesgrid.aspx';");
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
            this.AddNewRec.Click += new EventHandler(this.AddNewRec_Click);
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.SaveAttachType.Click += new EventHandler(this.SaveAttachType_Click);
            DataGrid1.ColumnSpan = 4;
        }

        #endregion
    }
}