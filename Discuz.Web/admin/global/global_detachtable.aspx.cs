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
    /// 分表设置
    /// </summary>
    
    public partial class detachtable : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentpost_tablename = "";
            currentpost_tablename = Discuz.Forum.Posts.GetPostTableName();
            info2.Text = "系统当前使用的帖子分表是: <b>" + currentpost_tablename + "</b>";
            if (!Page.IsPostBack)
            {
                if (!Databases.IsFullTextSearchEnabled())
                {
                    StartFullIndex.Visible = false;
                    DataGrid1.Columns[0].Visible = false;  
                }
                 BindData();
                detachtabledescription.AddAttributes("maxlength", "50");
                SaveInfo.Attributes.Add("onclick", "if(!confirm('您目前表中帖子数不足" + Posts.GetPostTableCount(currentpost_tablename) + "万,要进行帖子分表吗?')){return false;}");
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
            #region 更新指定的分表信息
            try
            {
                Posts.UpdateDetachTable(Utils.StrToInt(DataGrid1.DataKeys[e.Item.ItemIndex].ToString(), 0),
                    ((System.Web.UI.WebControls.TextBox)e.Item.Cells[4].Controls[0]).Text);
                base.RegisterStartupScript( "", "<script>window.location.href='global_detachtable.aspx';</script>");
            }
            catch
            {
                base.RegisterStartupScript( "", "<script>alert('无法更新数据库.');window.location.href='global_detachtable.aspx';</script>");
                return;
            }
            #endregion
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 设置数据绑定的长度
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                System.Web.UI.WebControls.TextBox t = (System.Web.UI.WebControls.TextBox)e.Item.Cells[4].Controls[0];
                t.Attributes.Add("maxlength", "50");
                t.Attributes.Add("size", "20");
            }
            if (e.Item.ItemType == ListItemType.Item)
            {
                if (e.Item.Cells[2].Text.ToString().Length > 40)
                    e.Item.Cells[2].Text = e.Item.Cells[2].Text.Substring(0, 40) + "…";
            }
            #endregion
        }

        private void StartFullIndex_Click(object sender, EventArgs e)
        {
            #region 开始进行添充操作

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
                //    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');window.location.href='global_detachtable.aspx';</script>");
                //}
            }

            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region 当分表操作执行后则更新相应的缓存数据
            if (CreateDetachTable(detachtabledescription.Text))
            {
                Caches.ReSetLastPostTableName();
                Caches.ReSetAllPostTableName();
                Posts.ResetPostTables();//重置posttables静态对象
                base.RegisterStartupScript( "PAGE",  "window.location.href='global_detachtable.aspx';");

                if (DNTRequest.GetString("createtype") == "on")
                    Utils.RestartIISProcess(); 	    
            }
            #endregion
        }

        public bool CreateDetachTable(string description)
        {
            #region 创建分表

            try
            {
                string currentdbprefix = BaseConfigs.GetTablePrefix + "posts"; //当前数据表所使用的前辍
                //取出当前表中最大ID的记录表名称
                int tablelistmaxid = Posts.GetMaxPostTableId();
                if (!Posts.UpdateMinMaxField(tablelistmaxid)) //表值总数不能大于213
                {
                    base.RegisterStartupScript( "", "<script>alert('表值总数不能大于213,当前最大值为" + tablelistmaxid + "!');window.location.href='global_detachtable.aspx';</script>");
                    return false;
                }
                //更新当前表中最大ID的记录用的最大和最小tid字段		
                //if (tablelistmaxid > 0)
                //{
                //    Posts.UpdateMinMaxField(currentdbprefix + tablelistmaxid, tablelistmaxid);
                //}

                string tablename = currentdbprefix + (tablelistmaxid + 1);

                try
                {
                    //构建相应表及全文索引
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
            #region 显示当前分表所处的TID的范围

            if (maxtid == "0")
            {
                DataTable dt = Posts.GetMaxTid();
                if (dt.Rows.Count > 0)
                {
                    return mintid + " 至 " + (dt.Rows[0][0].ToString() == "" ? mintid : dt.Rows[0][0].ToString());
                }
                else
                {
                    return mintid + " 至 " + mintid;
                }
            }
            else
            {
                return mintid + " 至 " + maxtid;
            }

            #endregion
        }

        public string CurrentPostsCount(string postsid)
        {
            #region 得到当前分表的帖子数

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

   

        #region Web 窗体设计器生成的代码

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
            DataGrid1.TableHeaderName = "帖子分表列表";
            DataGrid1.ColumnSpan = 4;
            DataGrid1.SaveDSViewState = true;

        }

        #endregion
    }
}
