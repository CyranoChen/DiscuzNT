using System;
using System.Web.UI;
using System.Data;
using System.Xml;
using System.Web.UI.WebControls;
using System.IO;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;
using Discuz.Common.Xml;

namespace Discuz.Web.Admin
{
    public partial class managemenubackupfile : AdminPage
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDataGrid();
            }
            if (DNTRequest.GetString("filename") != "")
            {
                Utils.RestoreFile(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/backup/" + DNTRequest.GetString("filename")),
                                  Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config"));
                MenuManage.CreateMenuJson();
                base.RegisterStartupScript("", "<script>alert('恢复成功！');window.location.href='managemainmenu.aspx';</script>");
                return;
            }
        }

        private void BindDataGrid()
        {
            DataGrid1.TableHeaderName = "菜单备份管理";
            DataTable dt = new DataTable();
            dt.Columns.Add("backupname");
            dt.Columns.Add("backupdate");
            string[] filelist = Directory.GetFiles(Server.MapPath("../xml/backup"),"*.config");
            
            //foreach(string file in filelist)
            for (int i = filelist.Length - 1; i >= 0; i--)
            {
                string filename = Path.GetFileName(filelist[i]);
                DataRow dr = dt.NewRow();
                dr["backupname"] = filename;
                dr["backupdate"] = Path.GetFileNameWithoutExtension(filename).Replace("_", ":");
                dt.Rows.Add(dr);
            }
            DataGrid1.DataSource = dt;
            DataGrid1.DataKeyField = "backupname";
            DataGrid1.DataBind();
        }



        #region Web Form Designer generated code
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion

        protected void Delbackupfile_Click(object sender, EventArgs e)
        {
            string backupname = DNTRequest.GetString("backupname");
            if (backupname == "")
            {
                base.RegisterStartupScript("", "<script>alert('未选中任何记录！');</script>");
                return;
            }
            foreach (string file in backupname.Split(','))
            {
                File.Delete(Server.MapPath("../xml/backup/" + file));
            }
            base.RegisterStartupScript("", "<script>alert('删除成功！');window.location.href='managemenubackupfile.aspx';</script>");
        }

        protected void backupfile_Click(object sender, EventArgs e)
        {
            MenuManage.BackupMenuFile();
            base.RegisterStartupScript("", "<script>alert('备份成功！');window.location.href='managemenubackupfile.aspx';</script>");
        }
    }
}
