using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using System.Collections;
using Discuz.Data;
using System.Data;
using System.Data.SqlClient;

namespace Discuz.Install
{
    public class InstallUtils
    {

        public const string dbScriptPath = @"sqlscript\sqlserver\";//数据库脚本文件存放路径


        /// <summary>
        /// 返回系统环境检测结果
        /// </summary>
        /// <param name="error">是否有异常</param>
        /// <returns></returns>
        public static string InitialSystemValidCheck()
        {
            StringBuilder sb = new StringBuilder();
            HttpContext context = HttpContext.Current;
            string messageTemplate = "{{'result':'{0}','msg':'{1}'}},";

            string fileName = context != null ? context.Server.MapPath("/DNT.config") : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DNT.config");

            //系统BIN目录检查
            sb.Append(IISSystemBINCheck());


            //检查Dnt.config文件的有效性
            if (!GetRootDntconfigPath())
            {
                sb.AppendFormat(messageTemplate, "false", "DNT.config 不可写或没有放置正确, 相关问题详见安装文档!");
            }
            else
                sb.AppendFormat(messageTemplate, "true", "对 DNT.config 验证通过!");

            //检查系统目录的有效性
            string folderstr = "admin,aspx,avatars,cache,config,editor,images,templates,upload";
            foreach (string foldler in folderstr.Split(','))
            {
                if (!SystemFolderCheck(foldler))
                {
                    sb.AppendFormat(messageTemplate, "false", "对 " + foldler + " 目录没有写入和删除权限!");
                }
                else
                    sb.AppendFormat(messageTemplate, "true", "对 " + foldler + " 目录权限验证通过!");
            }
            string filestr = "admin\\xml\\navmenu.config,javascript\\common.js,install\\systemfile.aspx,upgrade\\systemfile.aspx";
            foreach (string file in filestr.Split(','))
            {
                if (!SystemFileCheck(file))
                {
                    sb.AppendFormat(messageTemplate, "false", "对 " + file.Substring(0, file.LastIndexOf('\\')) + " 目录没有写入和删除权限!");
                }
                else
                    sb.AppendFormat(messageTemplate, "true", "对 " + file.Substring(0, file.LastIndexOf('\\')) + " 目录权限验证通过!");
            }

            if (!TempTest())
            {
                sb.AppendFormat(messageTemplate, "false", "您没有开启对 " + Path.GetTempPath() + " 文件夹访问权限，详情参见安装文档.");
            }
            else
            {
                if (!SerialiazeTest())
                {
                    sb.AppendFormat(messageTemplate, "false", "对config文件反序列化失败，详情参见安装文档.");
                }
                else
                    sb.AppendFormat(messageTemplate, "true", "反序列化验证通过!");

            }
            return ("[" + sb.ToString().Trim(',') + "]").Replace("\\", "\\\\");
        }

        /// <summary>
        /// 返回DNT.config文件是否正确读取
        /// </summary>
        /// <returns></returns>
        public static bool GetRootDntconfigPath()
        {
            try
            {
                HttpContext context = HttpContext.Current;

                string webconfigfile = "";
                if (!Utils.FileExists(webconfigfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dnt.config"))
                    && (!Utils.FileExists(webconfigfile = Utils.GetMapPath("~/dnt.config")))
                    && (!Utils.FileExists(webconfigfile = Path.Combine(context.Request.PhysicalApplicationPath, "dnt.config")))
                    && (!Utils.FileExists(webconfigfile = Utils.GetMapPath("../dnt.config")))
                    && (!Utils.FileExists(webconfigfile = Utils.GetMapPath("../../dnt.config")))
                    && (!Utils.FileExists(webconfigfile = Utils.GetMapPath("../../../dnt.config"))))
                {
                    return false;
                }
                else
                {
                    StreamReader sr = new StreamReader(webconfigfile);
                    string content = sr.ReadToEnd();
                    sr.Close();
                    content += " ";
                    StreamWriter sw = new StreamWriter(webconfigfile, false);
                    sw.Write(content);
                    sw.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 检测目录是否有读写权限
        /// </summary>
        /// <returns></returns>
        public static bool SystemRootCheck()
        {
            HttpContext context = HttpContext.Current;

            string physicsPath = context != null ? context.Server.MapPath("/") : AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                using (FileStream fs = new FileStream(physicsPath + "\\a.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Close();
                }
                System.IO.File.Delete(physicsPath + "\\a.txt");
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 返回bin文件的检测结果
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string IISSystemBINCheck()
        {
            string binfolderpath = HttpRuntime.BinDirectory;
            string messageTemplate = "{{'result':'{0}','msg':'{1}'}},";

            string result = "";
            try
            {
                string[] assemblylist = new string[] { "Discuz.Aggregation.dll", "Discuz.Cache.dll", "Discuz.Common.dll", "Discuz.Config.dll", 
                    "Discuz.Control.dll", "Discuz.Data.dll", "Discuz.Data.SqlServer.dll","Discuz.Entity.dll","Discuz.Event.dll", "Discuz.Forum.dll",
                    "Discuz.Install.dll", "Discuz.Plugin.dll","Discuz.Plugin.Spread.dll", "Discuz.Web.Admin.dll",
                    "Discuz.Web.dll", "Discuz.Web.Services.dll","Interop.SQLDMO.dll","Newtonsoft.Json.dll" };
                bool isAssemblyInexistence = false;
                ArrayList inexistenceAssemblyList = new ArrayList();
                foreach (string assembly in assemblylist)
                {
                    if (!File.Exists(binfolderpath + assembly))
                    {
                        isAssemblyInexistence = true;
                        inexistenceAssemblyList.Add(assembly);
                    }
                }
                if (isAssemblyInexistence)
                {
                    foreach (string assembly in inexistenceAssemblyList)
                    {
                        result += string.Format(messageTemplate, "false", assembly + " 文件放置不正确,请将所有的dll文件复制到目录" + binfolderpath + " 中.");
                    }
                }
            }
            catch
            {
                result += string.Format(messageTemplate, "false", "请将所有的dll文件复制到目录 " + binfolderpath + " 中.");
            }
            return result;
        }

        /// <summary>
        /// 检测指定目录是否有读写权限
        /// </summary>
        /// <param name="foldername"></param>
        /// <returns></returns>
        public static bool SystemFolderCheck(string foldername)
        {
            string physicsPath = Utils.GetMapPath(@"..\" + foldername);
            try
            {
                using (FileStream fs = new FileStream(physicsPath + "\\a.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Close();
                }
                if (File.Exists(physicsPath + "\\a.txt"))
                {
                    System.IO.File.Delete(physicsPath + "\\a.txt");
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 检测是否有操作文件的权限
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool SystemFileCheck(string filename)
        {
            filename = Utils.GetMapPath(@"..\" + filename);
            try
            {
                if (filename.IndexOf("systemfile.aspx") == -1 && !File.Exists(filename))
                    return false;
                if (filename.IndexOf("systemfile.aspx") != -1)  //做删除测试
                {
                    File.Delete(filename);
                    return true;
                }
                StreamReader sr = new StreamReader(filename);
                string content = sr.ReadToEnd();
                sr.Close();
                content += " ";
                StreamWriter sw = new StreamWriter(filename, false);
                sw.Write(content);
                sw.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 配置文件反序列化检测
        /// </summary>
        /// <returns></returns>
        private static bool SerialiazeTest()
        {
            try
            {
                string configPath = HttpContext.Current.Server.MapPath("../config/general.config");
                GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(configPath);
                __configinfo.Passwordkey = ForumUtils.CreateAuthStr(10);
                SerializationHelper.Save(__configinfo, configPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 服务器temp目录权限检测
        /// </summary>
        /// <returns></returns>
        public static bool TempTest()
        {
            string UserGuid = Guid.NewGuid().ToString();
            string TempPath = Path.GetTempPath();
            string path = TempPath + UserGuid;
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine(DateTime.Now);
                }

                using (StreamReader sr = new StreamReader(path))
                {
                    sr.ReadLine();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建表和相关索引，约束
        /// </summary>
        /// <returns></returns>
        public static string CreateTable()
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader objReader = new StreamReader(Utils.GetMapPath(dbScriptPath + "setup2.1.sql"), Encoding.UTF8))
            {
                sb.Append(objReader.ReadToEnd());
                objReader.Close();
            }

            if (BaseConfigs.GetTablePrefix.ToLower() == "dnt_")
                DbHelper.ExecuteCommandWithSplitter(sb.ToString());
            else
                DbHelper.ExecuteCommandWithSplitter(sb.ToString().Replace("dnt_", BaseConfigs.GetTablePrefix));

            return "{result:true,message:1}";
        }

        /// <summary>
        /// 创建存储过程
        /// </summary>
        /// <returns></returns>
        public static string CreateStorePocedure()
        {
            StringBuilder sb = new StringBuilder();
            string sqlServerVersion = DbHelper.ExecuteScalar(CommandType.Text, "SELECT @@VERSION").ToString().Substring(20, 24).Trim();
            if (sqlServerVersion.IndexOf("2000") >= 0)
            {
                using (StreamReader objReader = new StreamReader(Utils.GetMapPath(dbScriptPath + "setup2.2.sql"), Encoding.UTF8))
                {
                    sb.Append(objReader.ReadToEnd());
                    objReader.Close();
                }
            }
            else
            {
                using (StreamReader objReader = new StreamReader(Utils.GetMapPath(dbScriptPath + "setup2.2 - 2005.sql"), Encoding.UTF8))
                {
                    sb.Append(objReader.ReadToEnd());
                    objReader.Close();
                }
            }
            if (BaseConfigs.GetTablePrefix.ToLower() == "dnt_")
                DbHelper.ExecuteCommandWithSplitter(sb.ToString().Trim().Replace("\"", "'"));
            else
                DbHelper.ExecuteCommandWithSplitter(sb.ToString().Trim().Replace("\"", "'").Replace("dnt_", BaseConfigs.GetTablePrefix));

            return "{result:true,message:\"" + sqlServerVersion + "\"}";
        }

        /// <summary>
        /// 初始化起始数据
        /// </summary>
        /// <param name="adminName"></param>
        /// <param name="adminPassword"></param>
        /// <returns></returns>
        public static string InitialForumSource(string adminName, string adminPassword)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                using (StreamReader objReader = new StreamReader(Utils.GetMapPath(dbScriptPath + "setup3.sql"), Encoding.UTF8))
                {
                    sb.Append(objReader.ReadToEnd());
                    objReader.Close();
                }
                if (BaseConfigs.GetTablePrefix.ToLower() == "dnt_")
                    DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
                else
                    DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString().Replace("dnt_", BaseConfigs.GetTablePrefix));

                DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("INSERT INTO [{0}users] ([username],[nickname],[password],[adminid],[groupid],[invisible],[email]) VALUES('{1}','{1}','{2}','1','1','0','')",
                    BaseConfigs.GetTablePrefix, adminName, Utils.MD5(adminPassword)));

                DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "userfields] ([uid]) VALUES('1')");

                //将论坛是否执行安装程序的状态改为1(已安装)
                GeneralConfigInfo config = GeneralConfigs.GetConfig();
                config.Installation = 1;
                GeneralConfigs.Serialiaze(config, Utils.GetMapPath("../config/general.config"));
                GeneralConfigs.ResetConfig();

                return "{result:true,message:\"数据初始化完毕\"}";
            }
            catch (Exception e)
            {
                return "{result:false,message:\"初始化过程出现错误(" + JsonCharFilter(e.Message) + ")\"}";
            }
        }

        /// <summary>
        /// 将用户填写的数据库信息写入DNT.config文件
        /// </summary>
        /// <param name="dataSource">数据库地址</param>
        /// <param name="userID">数据库账号</param>
        /// <param name="password">数据库账号密码</param>
        /// <param name="databaseName">数据库名</param>
        /// <param name="tablePrefix">表前缀</param>
        public static void EditDntConfig(string dataSource, string userID, string password, string databaseName, string tablePrefix)
        {
            BaseConfigInfo baseConfig = BaseConfigs.GetBaseConfig();
            string connectionString = string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=true",
                                             dataSource, userID, password, databaseName);
            baseConfig.Dbconnectstring = connectionString;
            baseConfig.Tableprefix = tablePrefix;
            baseConfig.Dbtype = "SqlServer";
            string dntPath = Utils.GetMapPath("~/DNT.config");
            if (!Utils.FileExists(dntPath))
            {
                dntPath = Utils.GetMapPath("/DNT.config");
            }
            SerializationHelper.Save(baseConfig, dntPath);
            DbHelper.ConnectionString = baseConfig.Dbconnectstring;
            BaseConfigs.ResetRealConfig();
        }

        /// <summary>
        /// 自动检查当前程序的目录状态并保存
        /// </summary>
        /// <param name="forumPath"></param>
        public static void SaveDntConfigForumPath()
        {
            HttpRequest request = HttpContext.Current.Request;

            string forumPath = request.Url.ToString().Replace("http://" + request.Url.Authority, "");

            if (forumPath.IndexOf("install") < 0)
            {
                return;
            }

            forumPath = forumPath.Substring(0, forumPath.IndexOf("install"));
            BaseConfigInfo baseConfig = BaseConfigs.GetBaseConfig();
            if (baseConfig.Forumpath.Trim() != forumPath)
            {
                baseConfig.Forumpath = forumPath;
                string dntPath = Utils.GetMapPath("~/DNT.config");
                if (!Utils.FileExists(dntPath))
                {
                    dntPath = Utils.GetMapPath("/DNT.config");
                }
                SerializationHelper.Save(baseConfig, dntPath);
                BaseConfigs.ResetRealConfig();
                Utils.RestartIISProcess();
            }
        }

        public static SqlConnection connection = new SqlConnection();

        public static string JsonCharFilter(string sourceStr)
        {
            sourceStr = sourceStr.Replace("\\", "");
            sourceStr = sourceStr.Replace("\b", "");
            sourceStr = sourceStr.Replace("\t", "");
            sourceStr = sourceStr.Replace("\n", "");
            sourceStr = sourceStr.Replace("\f", "");
            sourceStr = sourceStr.Replace("\r", "");
            sourceStr = sourceStr.Replace("'", "\\'");
            return sourceStr.Replace("\"", "\\\"");
        }

        /// <summary>
        /// 测试用户填写的数据库信息是否正确
        /// </summary>
        /// <returns>false:数据库用户名或密码错误</returns>
        public static string CheckDBConnection(string sqlIp, string sqlUsername, string sqlPassword, string dbName)
        {
            string result = "";
            dbName = string.IsNullOrEmpty(dbName) ? "master" : dbName;
            try
            {
                connection.ConnectionString = string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=false",
                                                  sqlIp, sqlUsername, sqlPassword, dbName);
                connection.Open();
            }
            catch (SqlException e)
            {
                result = "{result:false,message:\"" + JsonCharFilter(e.Message) + "\",code:\"" + e.Number + "\"}";
            }
            finally
            {
                connection.Close();
            }
            return string.IsNullOrEmpty(result) ? "{result:true,message:\"连接成功\"}" : result.Replace("'", "\'");
        }

        public static string CheckDBCollation(string sqlIp, string sqlUsername, string sqlPassword, string dbName)
        {
            string result = "";
            try
            {
                string dbCollation = GetDBDefaultCollation(string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=false",
                                                      sqlIp, sqlUsername, sqlPassword, dbName), dbName);
                if (dbCollation.IndexOf("Chinese_PRC") < 0)
                    result = "{result:false,message:\"数据库排序规则不是简体中文,请调整为简体中文后重新运行安装程序\"}";
            }
            catch (SqlException e)
            {
                result = "{result:false,message:\"" + JsonCharFilter(e.Message) + "\",code:\"" + e.Number + "\"}";
            }

            return string.IsNullOrEmpty(result) ? "{result:true,message:\"字符集检测完毕\"}" : result.Replace("'", "\'");

        }

        /// <summary>
        /// 执行SQL语句，用来测试指定数据库是否存在
        /// </summary>
        /// <param name="commandText">t-sql</param>
        public static void ExcuteSQL(string commandText, string connectionString)
        {
            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.ConnectionString = connectionString;
                connection.Open();
                sqlCommand = new SqlCommand(commandText, connection);
                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                sqlCommand.Connection.Close();
            }
        }

        public static string DBSourceExist(string sqlIp, string sqlUsername, string sqlPassword, string dbName, string tablePrefix)
        {
            string result = "";
            try
            {
                ExcuteSQL("SELECT COUNT(1) FROM [" + tablePrefix + "users]", string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=false",
                                                      sqlIp, sqlUsername, sqlPassword, dbName));
            }
            catch (SqlException e)
            {
                result = "{result:false,message:\"" + JsonCharFilter(e.Message) + "\",code:\"" + e.Number + "\"}";
            }

            return string.IsNullOrEmpty(result) ? "{result:true,message:\"数据库已存在\",code:0}" : result.Replace("'", "\'");
        }

        /// <summary>
        /// 检测数据库版本
        /// </summary>
        /// <param name="commandText">t-sql</param>
        /// <param name="connectionString">数据库连接串</param>
        public static string GetSqlVersion(string connectionString)
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
            connection.ConnectionString = connectionString;
            connection.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT @@VERSION", connection);
            string sqlVersion = sqlCommand.ExecuteScalar().ToString().Trim();
            sqlCommand.Connection.Close();
            return sqlVersion;
        }

        public static string GetDBDefaultCollation(string connectionString, string dbName)
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
            connection.ConnectionString = connectionString;
            connection.Open();
            SqlCommand sqlCommand = new SqlCommand(string.Format("SELECT DATABASEPROPERTYEX('{0}', 'Collation')", dbName), connection);
            string collation = sqlCommand.ExecuteScalar().ToString().Trim();
            sqlCommand.Connection.Close();
            return collation;
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        public static string CreateDatabase(string sqlIp, string sqlManager, string sqlManagerPassword, string sqlName)
        {
            string connectionString = string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=false",
                                                  sqlIp, sqlManager, sqlManagerPassword, "master");
            string commandText = string.Format("CREATE DATABASE [{0}]", sqlName);
            try
            {
                ExcuteSQL(commandText, connectionString);//执行创建数据库的TSQL；
                return "{result:true,message:\"数据库创建成功\"}";
            }
            catch (SqlException e)
            {
                return "{result:false,message:\"" + JsonCharFilter(e.Message) + "\",code:\"" + e.Number + "\"}";
            }
        }
    }
}
