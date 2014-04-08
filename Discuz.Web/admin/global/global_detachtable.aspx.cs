using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// �ֱ�����
    /// </summary>
    
    public partial class detachtable : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentpost_tablename = "";
            currentpost_tablename = Discuz.Forum.Posts.GetPostTableName();
            info2.Text = "ϵͳ��ǰʹ�õ����ӷֱ���: <b>" + currentpost_tablename + "</b>";
            if (!Page.IsPostBack)
            {
                if (!Databases.IsFullTextSearchEnabled())
                {
                    StartFullIndex.Visible = false;
                    DataGrid1.Columns[0].Visible = false;  
                }
                 BindData();
                detachtabledescription.AddAttributes("maxlength", "50");
                SaveInfo.Attributes.Add("onclick", "if(!confirm('��Ŀǰ��������������" + Posts.GetPostTableCount(currentpost_tablename) + "��,Ҫ�������ӷֱ���?')){return false;}");
            }
        }

        public void BindData()
        {
            DataGrid1.DataSource = Posts.GetPostTableList();
            DataGrid1.DataBind();
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataView dv = new DataView(Posts.GetPostTableList());
            dv.Sort = e.SortExpression.ToString();
            DataGrid1.DataSource = dv;
            DataGrid1.DataBind();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }

        protected void DataGrid_Edit(Object sender, DataGridCommandEventArgs E)
        {
            DataGrid1.EditItemIndex = E.Item.ItemIndex;
            BindData();
        }

        protected void DataGrid_Cancel(Object sender, DataGridCommandEventArgs e)
        {
            DataGrid1.EditItemIndex = -1;
            BindData();
        }

        protected void DataGrid_Update(Object sender, DataGridCommandEventArgs e)
        {
            #region ����ָ���ķֱ���Ϣ
            try
            {
                Posts.UpdateDetachTable(Utils.StrToInt(DataGrid1.DataKeys[e.Item.ItemIndex].ToString(), 0),
                    ((System.Web.UI.WebControls.TextBox)e.Item.Cells[4].Controls[0]).Text);
                base.RegisterStartupScript( "", "<script>window.location.href='global_detachtable.aspx';</script>");
            }
            catch
            {
                base.RegisterStartupScript( "", "<script>alert('�޷��������ݿ�.');window.location.href='global_detachtable.aspx';</script>");
                return;
            }
            #endregion
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region �������ݰ󶨵ĳ���
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                System.Web.UI.WebControls.TextBox t = (System.Web.UI.WebControls.TextBox)e.Item.Cells[4].Controls[0];
                t.Attributes.Add("maxlength", "50");
                t.Attributes.Add("size", "20");
            }
            if (e.Item.ItemType == ListItemType.Item)
            {
                if (e.Item.Cells[2].Text.ToString().Length > 40)
                    e.Item.Cells[2].Text = e.Item.Cells[2].Text.Substring(0, 40) + "��";
            }
            #endregion
        }

        private void StartFullIndex_Click(object sender, EventArgs e)
        {
            #region ��ʼ����������

            if (this.CheckCookie())
            {
                string msg = new Databases().StartFullIndex(DNTRequest.GetString("id"), Databases.GetDbName(), this.username);

                base.RegisterStartupScript(msg.StartsWith("window") ? "PAGE" : "", msg);
                //string DbName = Databases.GetDbName();

                //if (DNTRequest.GetString("id") != "")
                //{
                //    try
                //    {
                //        Databases.StartFullIndex(DbName);                        
                //        aysncallback = new delegateCreateOrFillText(StarFillIndexWithPostid);
                //        AsyncCallback myCallBack = new AsyncCallback(CallBack);
                //        aysncallback.BeginInvoke(DbName, DNTRequest.GetString("id"), myCallBack, this.username); //
                //        base.LoadRegisterStartupScript("PAGE", "window.location.href='global_detachtable.aspx';");
                //    }
                //    catch (Exception ex)
                //    {
                //        string message = ex.Message.Replace("'", " ");
                //        message = message.Replace("\\", "/");
                //        message = message.Replace("\r\n", "\\r\\n");
                //        message = message.Replace("\r", "\\r");
                //        message = message.Replace("\n", "\\n");
                //        base.RegisterStartupScript( "", "<script>alert('" + message + "');</script>");
                //    }
                //}
                //else
                //{
                //    base.RegisterStartupScript( "", "<script>alert('��δѡ���κ�ѡ��');window.location.href='global_detachtable.aspx';</script>");
                //}
            }

            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region ���ֱ����ִ�к��������Ӧ�Ļ�������
            if (CreateDetachTable(detachtabledescription.Text))
            {
                Caches.ReSetLastPostTableName();
                Caches.ReSetAllPostTableName();
                Posts.ResetPostTables();//����posttables��̬����
                base.RegisterStartupScript( "PAGE",  "window.location.href='global_detachtable.aspx';");

                if (DNTRequest.GetString("createtype") == "on")
                    Utils.RestartIISProcess(); 	    
            }
            #endregion
        }

        public bool CreateDetachTable(string description)
        {
            #region �����ֱ�

            try
            {
                string currentdbprefix = BaseConfigs.GetTablePrefix + "posts"; //��ǰ���ݱ���ʹ�õ�ǰ�
                //ȡ����ǰ�������ID�ļ�¼������
                int tablelistmaxid = Posts.GetMaxPostTableId();
                if (!Posts.UpdateMinMaxField(tablelistmaxid)) //��ֵ�������ܴ���213
                {
                    base.RegisterStartupScript( "", "<script>alert('��ֵ�������ܴ���213,��ǰ���ֵΪ" + tablelistmaxid + "!');window.location.href='global_detachtable.aspx';</script>");
                    return false;
                }
                //���µ�ǰ�������ID�ļ�¼�õ�������Сtid�ֶ�		
                //if (tablelistmaxid > 0)
                //{
                //    Posts.UpdateMinMaxField(currentdbprefix + tablelistmaxid, tablelistmaxid);
                //}

                string tablename = currentdbprefix + (tablelistmaxid + 1);

                try
                {
                    //������Ӧ��ȫ������
                     Databases.CreatePostTableAndIndex(tablename);
                }
                catch (Exception ex)
                {
                    string message = ex.Message.Replace("'", " ");
                    message = message.Replace("\\", "/");
                    message = message.Replace("\r\n", "\\r\\n");
                    message = message.Replace("\r", "\\r");
                    message = message.Replace("\n", "\\n");
                    base.RegisterStartupScript( "", "<script>alert('" + message + "');</script>");
                }
                finally
                {
                    if (tablelistmaxid > 0)
                    {
                        Posts.AddPostTableToTableList(description, Posts.GetMaxPostTableTid(currentdbprefix + tablelistmaxid));
                    }
                    else
                    {
                       // DatabaseProvider.GetInstance().AddPostTableToTableList(description, DatabaseProvider.GetInstance().GetMaxPostTableTid(currentdbprefix), 0);
                        Posts.AddPostTableToTableList(description, Posts.GetMaxPostTableTid(currentdbprefix));
                    }
                    Caches.ReSetPostTableInfo();
                   
                    Posts.CreateStoreProc(tablelistmaxid + 1);                                  
                }
                return true;
            }
            catch
            {
                return false;
            }

            #endregion
        }

        public string DisplayTid(string mintid, string maxtid)
        {
            #region ��ʾ��ǰ�ֱ�������TID�ķ�Χ

            if (maxtid == "0")
            {
                DataTable dt = Posts.GetMaxTid();
                if (dt.Rows.Count > 0)
                {
                    return mintid + " �� " + (dt.Rows[0][0].ToString() == "" ? mintid : dt.Rows[0][0].ToString());
                }
                else
                {
                    return mintid + " �� " + mintid;
                }
            }
            else
            {
                return mintid + " �� " + maxtid;
            }

            #endregion
        }

        public string CurrentPostsCount(string postsid)
        {
            #region �õ���ǰ�ֱ��������

            try
            {
                DataTable dt = Posts.GetPostCountFromIndex(postsid);
                if (dt.Rows.Count > 0)
                    return dt.Rows[0][0].ToString();
                else
                    return "0";
            }
            catch
            {
                DataTable dt = Posts.GetPostCountTable(postsid);
                if (dt.Rows.Count > 0)
                    return dt.Rows[0][0].ToString();
                else
                    return "0";
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
            this.DataGrid1.EditCommand += new DataGridCommandEventHandler(this.DataGrid_Edit);
            this.DataGrid1.CancelCommand += new DataGridCommandEventHandler(DataGrid_Cancel);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.DataGrid1.UpdateCommand += new DataGridCommandEventHandler(this.DataGrid_Update);
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
            this.StartFullIndex.Click += new EventHandler(this.StartFullIndex_Click);

            this.Load += new EventHandler(this.Page_Load);

            DataGrid1.LoadEditColumn();
            DataGrid1.DataKeyField = "id";
            DataGrid1.TableHeaderName = "���ӷֱ��б�";
            DataGrid1.ColumnSpan = 4;
            DataGrid1.SaveDSViewState = true;

        }

        #endregion
    }
}
