using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Forum;
using Discuz.Common;
using DataGrid = Discuz.Control.DataGrid;
using TextBox = Discuz.Control.TextBox;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 数据库备份和恢复
    /// </summary>
    public partial class backupandrestore : AdminPage
    {
        private static string backuppath = HttpContext.Current.Server.MapPath("backup/");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Databases.IsBackupDatabase() == true)
            {
                if (!Page.IsPostBack)
                {
                    if (!base.IsFounderUid(userid))
                    {
                        Response.Write(base.GetShowMessage());
                        Response.End();
                        return;
                    }

                    LoadDataBaseConnectString();

                    Grid1.DataSource = buildGridData();
                    Grid1.DataBind();
                }
                backuppath = HttpContext.Current.Server.MapPath("backup/");
            }
            else
            {
                Response.Write("<script>alert('您所使用的数据库不支持在线备份!');history.go(-1);</script>");
                Response.End();
            }
        }

        private void LoadDataBaseConnectString()
        {
            #region 绑定数据库链接串信息
            foreach (string info in BaseConfigs.GetDBConnectString.Split(';'))
            {
                if (info.ToLower().IndexOf("data source") >= 0 || info.ToLower().IndexOf("server") >= 0)
                {
                    ServerName.Text = info.Split('=')[1].Trim();
                    continue;
                }
                if (info.ToLower().IndexOf("user id") >= 0 || info.ToLower().IndexOf("uid") >= 0)
                {
                    UserName.Text = info.Split('=')[1].Trim();
                    continue;
                }
                if (info.ToLower().IndexOf("password") >= 0 || info.ToLower().IndexOf("pwd") >= 0)
                {
                    Password.Text = info.Split('=')[1].Trim();
                    continue;
                }

                if (info.ToLower().IndexOf("initial catalog") >= 0 || info.ToLower().IndexOf("database") >= 0)
                {
                    strDbName.Text = info.Split('=')[1].Trim();
                    break;
                }
            }
            #endregion
        }


        public DataTable buildGridData()
        {
            #region 绑定数据

            DataTable templatefilelist = new DataTable("templatefilelist");
            int count = 1;
            templatefilelist.Columns.Add("id", Type.GetType("System.Int32"));
            templatefilelist.Columns.Add("filename", Type.GetType("System.String"));
            templatefilelist.Columns.Add("createtime", Type.GetType("System.String"));
            templatefilelist.Columns.Add("fullname", Type.GetType("System.String"));

            foreach (FileSystemInfo file in new DirectoryInfo(Server.MapPath("backup")).GetFileSystemInfos())
            {
                if (file.Extension == ".config")
                {
                    DataRow dr = templatefilelist.NewRow();
                    dr["id"] = count++;
                    dr["filename"] = file.Name.Substring(0, file.Name.LastIndexOf("."));
                    dr["createtime"] = file.CreationTime.ToString();
                    dr["fullname"] = "backup/" + file.Name;
                    templatefilelist.Rows.Add(dr);
                }
            }
            return templatefilelist;

            #endregion
        }

        public bool BackUPDB(string ServerName, string UserName, string Password, string strDbName, string strFileName)
        {
            #region 数据库的备份的代码
            string message = Databases.BackUpDatabase(backuppath, ServerName, UserName, Password, strDbName, strFileName);
            if (message != "")
            {
                base.RegisterStartupScript("", GetMessageScript("备份数据库失败,原因:" + message + "!"));
                return false;
            }

            return true;
            #endregion
        }

        public bool RestoreDB(string ServerName, string UserName, string Password, string strDbName, string strFileName)
        {
            #region 数据库的恢复的代码
            string message = Databases.RestoreDatabase(backuppath, ServerName, UserName, Password, strDbName, strFileName);
            if (message != string.Empty)
            {
                base.RegisterStartupScript("", GetMessageScript("恢复数据库失败,原因:" + message + "!"));
                return false;
            }
            return true;
            #endregion
        }

        private void BackUP_Click(object sender, EventArgs e)
        {
            #region 开始备份数据

            if (this.CheckCookie())
            {
                if (!base.IsFounderUid(userid))
                {
                    Response.Write(base.GetShowMessage());
                    Response.End();
                    return;
                }
                if (backupname.Text == "")
                {
                    base.RegisterStartupScript("PAGE", "alert('备份名称不能为空');");
                    return;
                }
                aysncallback = new delegateBackUpDatabase(BackUPDB);
                AsyncCallback myCallBack = new AsyncCallback(CallBack);
                aysncallback.BeginInvoke(ServerName.Text, UserName.Text, Password.Text, strDbName.Text, backupname.Text, myCallBack, this.username); //
                LoadRegisterStartupScript("PAGE", "window.location.href='global_backupandrestore.aspx';");
            }
            #endregion
        }

        #region 异步建立备份或恢复的代理

        private delegate bool delegateBackUpDatabase(string ServerName, string UserName, string Password, string strDbName, string strFileName);

        //异步建立索引并进行填充的代理
        private delegateBackUpDatabase aysncallback;


        public void CallBack(IAsyncResult e)
        {
            aysncallback.EndInvoke(e);
        }

        #endregion

        public string GetHttpLink(string filename)
        {
            return "<a href=" + filename + ">下载</a>";
        }

        private void Restore_Click(object sender, EventArgs e)
        {
            #region 恢复备份

            if (this.CheckCookie())
            {
                if (!base.IsFounderUid(userid))
                {
                    Response.Write(base.GetShowMessage());
                    Response.End();
                    return;
                }

                if (DNTRequest.GetString("id") != "")
                {
                    string id = DNTRequest.GetString("id");
                    if (id.IndexOf(",0") > 0)
                    {
                        base.RegisterStartupScript("", GetMessageScript("您一次只能选择一个备份进行提交"));
                        return;
                    }
                    DataRow[] drs = buildGridData().Select("id=" + id.Replace("0 ", ""));

                    aysncallback = new delegateBackUpDatabase(RestoreDB);
                    AsyncCallback myCallBack = new AsyncCallback(CallBack);
                    aysncallback.BeginInvoke(ServerName.Text, UserName.Text, Password.Text, strDbName.Text, drs[0]["filename"].ToString(), myCallBack, this.username); //
                    base.LoadRegisterStartupScript("PAGE", "window.location.href='global_backupandrestore.aspx';");
                }
                else
                {
                    base.RegisterStartupScript("", GetMessageScript("您未选中任何选项"));
                }
            }

            #endregion
        }

        private string GetMessageScript(string msg)
        {
            return string.Format("<script>alert('{0}');window.location.href='global_backupandrestore.aspx';</script>",msg);
        }

        private void DeleteBackup_Click(object sender, EventArgs e)
        {
            #region 删除指定的备份文件

            if (DNTRequest.GetString("id") == "")
            {
                base.RegisterStartupScript("", GetMessageScript("您未选中任何选项"));
                return;
            }
            foreach (DataRow dr in buildGridData().Select("id IN(" + DNTRequest.GetString("id").Replace("0 ", "") + ")"))
                    File.Delete(Utils.GetMapPath(dr["fullname"].ToString()));
            base.RegisterStartupScript("PAGE", "window.location.href='global_backupandrestore.aspx';");
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
            this.BackUP.Click += new EventHandler(this.BackUP_Click);
            this.Restore.Click += new EventHandler(this.Restore_Click);
            this.DeleteBackup.Click += new EventHandler(this.DeleteBackup_Click);
            Grid1.TableHeaderName = "数据库备份列表";
        }

        #endregion
    }
}