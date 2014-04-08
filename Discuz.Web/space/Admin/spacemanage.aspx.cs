using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Entities;
using Button = Discuz.Control.Button;
using CheckBoxList = Discuz.Control.CheckBoxList;
using DataGrid = Discuz.Control.DataGrid;
using DropDownList = Discuz.Control.DropDownList;
using TextBox = Discuz.Control.TextBox;
using Calendar = Discuz.Control.Calendar;
using Discuz.Entity;
using Discuz.Data;
using Discuz.Space;
using Discuz.Web.Admin;
using Discuz.Space.Data;


namespace Discuz.Space.Admin
{
    /// <summary>
    /// usergrid 的摘要说明.
    /// </summary>
     
#if NET1
    public class SpaceManage : AdminPage
#else
    public partial class SpaceManage : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Calendar joindateStart;
        protected TextBox Users;
        protected Calendar joindateEnd;
        protected Button Search;
        protected Button CloseSpace;
        protected Button OpenSpace;
        protected Button DeleteSpace;
        protected TextBox Username;
        protected Panel searchtable;
        protected DataGrid DataGrid1;
        #endregion
#endif

        
        private void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            DataGrid1.AllowCustomPaging = true;
            string username = Username.Text;
            //string dateStart = joindateStart.SelectedDate.ToString().IndexOf("1900") >= 0 ? "" : joindateStart.SelectedDate.ToString();
            //string dateEnd = joindateEnd.SelectedDate.AddDays(1).ToString().IndexOf("1900") >= 0 ? "" : joindateEnd.SelectedDate.AddDays(1).ToString();
            string dateStart = joindateStart.SelectedDate.ToString();
            string dateEnd = joindateEnd.SelectedDate.AddDays(1).ToString().IndexOf("1900") >= 0 ? DateTime.Now.AddDays(1).ToString() : joindateEnd.SelectedDate.AddDays(1).ToString();
            DataGrid1.VirtualItemCount = DbProvider.GetInstance().GetSpaceRecordCount(username, dateStart, dateEnd);
            DataGrid1.DataSource = buildGridData(username,dateStart,dateEnd);
            DataGrid1.DataBind();
        }


        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }

        private DataTable buildGridData(string username, string dateStart, string dateEnd)
        {
            DataTable dt =  DbProvider.GetInstance().GetSpaceList(DataGrid1.PageSize, DataGrid1.CurrentPageIndex + 1, username, dateStart, dateEnd);
            dt.Columns.Add("tstatus");
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt32(dr["status"].ToString()) == (int)SpaceStatusType.Natural)
                    dr["tstatus"] = "正常";
                else
                {
                    if ((int.Parse(dr["status"].ToString()) & (int)SpaceStatusType.OwnerClose) != 0)
                    {
                        dr["tstatus"] = "所有者关闭 ";
                    }
                    if ((int.Parse(dr["status"].ToString()) & (int)SpaceStatusType.AdminClose) != 0)
                    {
                        dr["tstatus"] = dr["tstatus"].ToString() + "管理员关闭";
                    }
                }
            }
            return dt;

        }


        public void GoToPagerButton_Click(object sender, EventArgs e)
        {
            BindData();
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Search.Click += new EventHandler(this.Search_Click);
            this.CloseSpace.Click += new EventHandler(this.CloseSpace_Click);
            this.OpenSpace.Click += new EventHandler(this.OpenSpace_Click);
            this.DeleteSpace.Click += new EventHandler(this.DeleteSpace_Click);
            this.DataGrid1.GoToPagerButton.Click += new EventHandler(GoToPagerButton_Click);

            this.Load += new EventHandler(this.Page_Load);
            DataGrid1.TableHeaderName = "个人空间列表";
            DataGrid1.DataKeyField = "spaceid";
            DataGrid1.AllowSorting = false;
            DataGrid1.ColumnSpan = 8;
            DataGrid1.PageSize = 10;
        }

        #endregion

        private void ResetSearchTable_Click(object sender, EventArgs e)
        {
            Response.Redirect("space_spacemanage.aspx");
        }

        private void CloseSpace_Click(object sender, EventArgs e)
        {
            if (DNTRequest.GetString("spaceid") != "")
            {
                string spaceidlist = DNTRequest.GetString("spaceid");
                DbProvider.GetInstance().AdminCloseSpaceStatusBySpaceidlist(spaceidlist);
                base.RegisterStartupScript("PAGE",  "window.location.href='space_spacemanage.aspx';");
            }
            else
            {
                base.RegisterStartupScript("PAGE",  "alert('请选择要关闭的个人空间!');window.location.href='space_spacemanage.aspx';");
            }
        }

        public void OpenSpace_Click(object sender, EventArgs e)
        {
            if (DNTRequest.GetString("spaceid") != "")
            {
                string spaceidlist = DNTRequest.GetString("spaceid");
                DbProvider.GetInstance().AdminOpenSpaceStatusBySpaceidlist(spaceidlist);
                base.RegisterStartupScript("PAGE", "window.location.href='space_spacemanage.aspx';");
            }
            else
            {
                base.RegisterStartupScript("PAGE", "alert('请选择要关闭的个人空间!');window.location.href='space_spacemanage.aspx';");
            }
        }

        //删除相关用户
        private void DeleteSpace_Click(object sender, EventArgs e)
        {
            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("spaceid") != "")
                {
                    foreach (string spaceid in DNTRequest.GetString("spaceid").Split(','))
                    {
                        if (!Utils.StrIsNullOrEmpty(spaceid) && Utils.StrToInt(spaceid,0) > 0)
                        {
                            DeleteSapceInfo(spaceid);
                        }
                    }
                    base.RegisterStartupScript("PAGE",  "window.location.href='space_spacemanage.aspx';");
                }
                else
                {
                    base.RegisterStartupScript("PAGE", "alert('请选择相应的用户!');window.location.href='global_usergrid.aspx?condition=" + DNTRequest.GetString("condition") + "';");
                }
            }
        }

        private void DeleteSapceInfo(string spaceid)
        {
            int uid = int.Parse(DbProvider.GetInstance().GetUidBySpaceid(spaceid));
            //删除附件
            string aidlist = DbProvider.GetInstance().GetSpaceattachmentsAidListByUid(uid);
            if (aidlist != "")
                DbProvider.GetInstance().DeleteSpaceAttachmentByIDList(aidlist, uid);
            //删除主题分类
            DbProvider.GetInstance().DeleteSpaceCategory(uid);
            //删除评论
            DbProvider.GetInstance().DeleteSpaceComments(uid);
            //删除友情链接
            DbProvider.GetInstance().DeleteSpaceLink(uid);
            ////删除模块
            
#if NET1
			ModuleInfoCollection milist = Spaces.GetModuleCollectionByUserId(uid);
#else
            Discuz.Common.Generic.List<ModuleInfo> milist = Spaces.GetModuleCollectionByUserId(uid);
#endif

            if (milist != null)
            {
                foreach (ModuleInfo mi in milist)
                {
                    ISpaceCommand isc = Spaces.SetModuleBase(mi);
                    isc.RemoveModule();
                }
            }
            //删除日志
            DbProvider.GetInstance().DeleteSpacePosts(uid);
            //删除版块
            DbProvider.GetInstance().DeleteTab(uid);
            //删除个人空间记录
            DbProvider.GetInstance().DeleteSpaceByUid(uid);
            //重置User表中的个人空间
            DatabaseProvider.GetInstance().ClearUserSpace(uid);
        }


        private void Search_Click(object sender, EventArgs e)
        {
            if (this.CheckCookie())
            {
                DataGrid1.CurrentPageIndex = 0;
                BindData();
            }
        }
    }
}