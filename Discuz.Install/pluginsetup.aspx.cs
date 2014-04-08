using System;
using System.IO;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;


using Discuz.Common;
using Discuz.Config;
using Discuz.Data;
using Discuz.Forum;
using Discuz.Config.Provider;
using Discuz.Cache;
using Discuz.Plugin;

namespace Discuz.Install
{
    /// <summary>
    /// 插件安装
    /// </summary>
    public class pluginsetup : SetupPage
    {
        /// <summary>
        /// 插件安装复选框
        /// </summary>
        public Discuz.Control.CheckBoxList PlugIn;

        public System.Web.UI.WebControls.Panel Panel1;

        public System.Web.UI.WebControls.Panel Panel2;
        /// <summary>
        /// 已安装成功的插件
        /// </summary>
        public string installedPlugIn;

        public static string tableprefix = BaseConfigs.GetTablePrefix;

        public string scriptpath = "sqlscript/sqlserver/" ;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //进行可安装的插件检查
                PlugInCheck();

                //当没有可安装插件时进行脚本提示
                if (PlugIn.Items.Count < 1)
                {
#if NET1
                    Page.RegisterStartupScript( "", "<script>alert('当前插件安装程序未检测到任何插件！');</script>");
#else
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('当前插件安装程序未检测到任何插件！');</script>");
#endif
                }
            }
                                                        
        }

        //插件检测和加载插件选项列表
        protected void PlugInCheck()
        {
            if (Discuz.Plugin.Space.SpacePluginProvider.GetInstance() != null)
            {
                PlugIn.Items.Add(new ListItem("空间", "space"));
            }

            if (Discuz.Plugin.Album.AlbumPluginProvider.GetInstance() != null)
            {
                PlugIn.Items.Add(new ListItem("相册", "album"));
            }

            if (Discuz.Plugin.Mall.MallPluginProvider.GetInstance() != null)
            {
                PlugIn.Items.Add(new ListItem("交易", "mall"));
            }
        }

        /// <summary>
        /// 开始安装插件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SetupPlugIn_Click(object sender, EventArgs e)
        {
            //检查是否选中要安装的相应插件
            if (PlugIn.GetSelectString() == "")
            {
#if NET1
                Page.RegisterStartupScript( "", "<script>alert('您未选中任何插件！');</script>");
#else
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('您未选中任何插件！');</script>");
#endif
                return;
            }

            Panel1.Visible = false;
            Panel2.Visible = true;

            if (PlugIn.GetSelectString().IndexOf("space") >= 0)
            {
                InitTableAndSP(scriptpath + "space");
                MenuManage.ImportPluginMenu(Utils.GetMapPath("space.xml"));
                installedPlugIn += " 空间 ";
            }

            if (PlugIn.GetSelectString().IndexOf("album") >= 0)
            {
                InitTableAndSP(scriptpath + "album");
                MenuManage.ImportPluginMenu(Utils.GetMapPath("album.xml"));
                installedPlugIn += " 相册 ";
            }


            if (PlugIn.GetSelectString().IndexOf("mall") >= 0)
            {
                InitTableAndSP(scriptpath + "mall");
                MenuManage.ImportPluginMenu(Utils.GetMapPath("mall.xml"));
                installedPlugIn += " 交易 ";
            }

            installedPlugIn += "<br />";

        }

        /// <summary>
        /// 删除数据库中原有的表和存储过程
        /// </summary>
        /// <param name="dbscriptpath">脚本路径</param>
        private void InitTableAndSP(string dbscriptpath)
        {
            StringBuilder sb = new StringBuilder();

            #region 删除数据库中原有的表和存储过程
            
            using (StreamReader objReader = new StreamReader(Server.MapPath(dbscriptpath + "/setup1.sql"), Encoding.UTF8))
            {
                sb.Append(objReader.ReadToEnd());
                objReader.Close();
            }

            if (tableprefix.ToLower() == "dnt_")
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
            }
            else
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString().Replace("dnt_", tableprefix));
            }
            #endregion

            #region 建表和存储过程

            sb = new StringBuilder();
            using (StreamReader objReader = new StreamReader(Server.MapPath(dbscriptpath + "/setup2.1.sql"), Encoding.UTF8))
            {
                sb.Append(objReader.ReadToEnd());
                objReader.Close();
            }

            if (tableprefix.ToLower() == "dnt_")
            {
                DbHelper.ExecuteCommandWithSplitter(sb.ToString());
            }
            else
            {
                DbHelper.ExecuteCommandWithSplitter(sb.ToString().Replace("dnt_", tableprefix));
            }

            sb.Remove(0, sb.Length);
            using (StreamReader objReader = new StreamReader(Server.MapPath(dbscriptpath + "/setup2.2.sql"), Encoding.UTF8))
            {
                sb.Append(objReader.ReadToEnd());
                objReader.Close();
            }


            if (tableprefix.ToLower() == "dnt_")
            {
                DbHelper.ExecuteCommandWithSplitter(sb.ToString().Trim().Replace("\"", "'"));
            }
            else
            {
                DbHelper.ExecuteCommandWithSplitter(sb.ToString().Trim().Replace("\"", "'").Replace("dnt_", tableprefix));
            }

            #endregion

            #region 初始化新创建的数据库
            try
            {
                sb = new StringBuilder();
                using (StreamReader objReader = new StreamReader(Server.MapPath(dbscriptpath + "/setup3.sql"), Encoding.UTF8))
                {
                    sb.Append(objReader.ReadToEnd());
                    objReader.Close();
                }
                if (tableprefix.ToLower() == "dnt_")
                {
                    DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
                }
                else
                {
                    DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString().Replace("dnt_", tableprefix));
                }
            }
            catch
            {
                ;
            }
            #endregion
        }
        
      
    }
}
