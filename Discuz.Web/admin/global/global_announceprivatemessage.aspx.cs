using System;
using System.Data;
using System.Data.Common;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;

using Discuz.Common;
using Discuz.Entity;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using Discuz.Config;
using Discuz.Cache;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 过滤词列表
    /// </summary>
    public partial class announceprivatemessage : AdminPage
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
            #region 绑定公共列表
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "公共消息列表";
            DataGrid1.DataKeyField = "pmid";
            DataGrid1.BindData(PrivateMessages.GetAnnouncePrivateMessageCollection(-1, 0));
            #endregion
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            #region 删除公共消息

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("id") != "")
                {
                    PrivateMessages.DeletePrivateMessage(0, Utils.SplitString(DNTRequest.GetString("id"), ","));
                    DNTCache.GetCacheService().RemoveObject("/Forum/AnnouncePrivateMessageCount");
                    Response.Redirect("global_announceprivatemessage.aspx");
                }
                else
                {
                    base.RegisterStartupScript("", GetMessageScript("您未选中任何选项"));
                }
            }

            #endregion
        }

        private void AddNewRec_Click(object sender, EventArgs e)
        {
            #region 添加公共消息

            if (subject.Text == "")
            {
                base.RegisterStartupScript("", GetMessageScript("公共消息标题不能为空"));
                return;
            }
            if (message.Text == "")
            {
                base.RegisterStartupScript("", GetMessageScript("公共消息内容不能为空"));
                return;
            }
            
            try
            {
                PrivateMessageInfo pm = new PrivateMessageInfo();
                pm.Message = message.Text;
                pm.Subject = subject.Text;
                pm.Msgto = "";
                pm.Msgtoid = 0;
                pm.Msgfrom = "";
                pm.Msgfromid = 0;
                pm.New = 1;
                pm.Postdatetime = DateTime.Now.ToString();
                PrivateMessages.CreatePrivateMessage(pm, 0);
                BindData();
                DNTCache.GetCacheService().RemoveObject("/Forum/AnnouncePrivateMessageCount");
                base.RegisterStartupScript("PAGE", "window.location.href='global_announceprivatemessage.aspx';");
                return;
            }
            catch
            {
                base.RegisterStartupScript("", GetMessageScript("无法更新数据库."));
                return;
            }

            #endregion
        }

        private string GetMessageScript(string message)
        {
            return string.Format("<script>alert('{0}');window.location.href='global_announceprivatemessage.aspx';</script>", message);
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
            DataGrid1.ColumnSpan = 5;
        }

        #endregion

    }
}