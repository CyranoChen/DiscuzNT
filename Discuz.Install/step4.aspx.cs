using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;

using Discuz.Common;
using Discuz.Config;
using Discuz.Data;
using Discuz.Forum;
using Discuz.Install;
using Discuz.Config.Provider;

namespace Discuz.Install
{
   public class InstallStep4 : SetupPage
    {
       protected string forumPath = "";//论坛路径
       string dbScriptPath = @"sqlscript\sqlserver\";//数据库脚本文件存放路径
       string sqlServerVersion = "";//存放数据库版本号
       protected static BaseConfigInfo baseConfig = new BaseConfigInfo();//dnt.config
       //static string connectionStr = baseConfig.Dbconnectstring;//数据库连接串
       protected string tableprefix = "";//表前缀
       protected System.Web.UI.HtmlControls.HtmlButton next;
       private BaseConfigInfo dntConfigInfo = BaseConfigProvider.GetRealBaseConfig();
       protected void Page_Load(object sender,EventArgs e) 
       {
           forumPath = dntConfigInfo.Forumpath;
           if(!IsPostBack)
           {
               if (Request.QueryString["checkForumPath"] == "exists")
                   CheckForumPath(Request.QueryString["forumpath"]);
           }
       }

       /// <summary>
       /// Button：next的点击事件
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
       protected void Setup(object sender, EventArgs e)
       {
           CreateTableAndSP();//建表和存储过程
           if (InitialDB())
           {
               Server.Transfer("succeed.aspx", true);
           }
           else
           {
               ClientScript.RegisterStartupScript(typeof(Page), "error", "alert('初始化数据库失败！请检查权限')", true);
           }
       }

       /// <summary>
       /// 检查论坛路径是否正确
       /// </summary>
       /// <returns>false为不正确</returns>
       protected void CheckForumPath(string ntPath)
       {
           string forumPath = Utils.GetMapPath("~" + ntPath + "DNT.config");
           if (!Utils.FileExists(forumPath))
           {
               forumPath = Utils.GetMapPath(ntPath + "DNT.config");
           }
           //if (!Utils.FileExists(forumPath) || !Utils.FileExists(Utils.GetMapPath("~/" + "DNT.config")))
           //{
           //    Response.Write("{\"Result\":false}");
           //    Response.End();
           //}
           //else
           //{
           //    Response.Write("{\"Result\":true}");
           //    Response.End();
           //}
           if (Utils.FileExists(forumPath) || Utils.FileExists(Utils.GetMapPath("~/" + "DNT.config")))
           {
               Response.Write("{\"Result\":true}");
               Response.End();
           }
           else
           {
               Response.Write("{\"Result\":false}");
               Response.End();
           }
       }

       /// <summary>
       /// 将论坛路径写入DNT.config文件
       /// </summary>
       /// <param name="forumPath">论坛路径</param>
       //protected void EditDntConfig(string forumPath)
       //{
       //    string dntPath = Utils.GetMapPath("~/DNT.config");
       //    baseConfig = BaseConfigs.GetBaseConfig();
       //    baseConfig.Forumpath = forumPath;
           
       //    if (!Utils.FileExists(dntPath))
       //    {
       //        dntPath = Utils.GetMapPath("/DNT.config");
       //    }
       //    SerializationHelper.Save(baseConfig, dntPath);
       //    BaseConfigs.ResetRealConfig();
       //}

       /// <summary>
       /// 建表和存储过程
       /// </summary>
       private void CreateTableAndSP()
       {
           tableprefix = BaseConfigs.GetTablePrefix;
           #region 建表
           StringBuilder sb = new StringBuilder();
           using (StreamReader objReader = new StreamReader(Server.MapPath(dbScriptPath + "setup2.1.sql"), Encoding.UTF8))
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
               //DbHelper.ExecuteNonQuery(sb.ToString().Replace("dnt_",tableprefix));
           }
           #endregion

           #region 建存储过程
           sb.Remove(0, sb.Length);
           sqlServerVersion = DbHelper.ExecuteScalar(CommandType.Text, "SELECT @@VERSION").ToString().Substring(20, 24).Trim();
           if (sqlServerVersion.IndexOf("2000") >= 0)
           {
               using (StreamReader objReader = new StreamReader(Server.MapPath(dbScriptPath + "setup2.2.sql"), Encoding.UTF8))
               {
                   sb.Append(objReader.ReadToEnd());
                   objReader.Close();
               }
           }
           else
           {
               using (StreamReader objReader = new StreamReader(Server.MapPath(dbScriptPath + "setup2.2 - 2005.sql"), Encoding.UTF8))
               {
                   sb.Append(objReader.ReadToEnd());
                   objReader.Close();
               }
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
       }

       /// <summary>
       /// 初始化数据库
       /// </summary>
       /// <returns></returns>
       private bool InitialDB()
       {
           #region 初始化
           try
           {
               StringBuilder sb = new StringBuilder();
               using (StreamReader objReader = new StreamReader(Server.MapPath(dbScriptPath + "setup3.sql"), Encoding.UTF8))
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
               return false;
           }

           try
           {
               DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO [" + tableprefix + "users] ([username],[nickname],[password],[adminid],[groupid],[invisible],[email]) VALUES('" + Request["adminName"] + "','" + Request["adminName"] + "','" + Utils.MD5(Request["adminPassword"]) + "','1','1','0','')");
               DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO [" + tableprefix + "userfields] ([uid]) VALUES('1')");
               return true;
           }
           catch
           {
               return false;
           }
           #endregion
       }

    }
}
