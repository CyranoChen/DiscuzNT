using System;
using System.IO;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text.RegularExpressions;


using Discuz.Common;
using Discuz.Config;
using Discuz.Data;
using Discuz.Forum;
using Discuz.Config.Provider;
using TextBox = Discuz.Control.TextBox;
using DropDownList = Discuz.Control.DropDownList;
using Discuz.Cache;
using System.Data.SqlClient;
namespace Discuz.Install
{
    public class install : SetupPage
    {
        protected string sqlServerIP = "";
        protected string dataBaseName = "";
        protected string sqlUID = "";
        protected string sqlPassword = "";
        protected string sqlPasswordConfirm = "";
        protected string tablePrefix = "";
        protected string connectionString = "";
        protected string commandText = "";
        protected string sqlVersion = "";
        private SqlConnection connection = new SqlConnection();
        protected System.Web.UI.HtmlControls.HtmlGenericControl dataBaseInfo;
        protected System.Web.UI.HtmlControls.HtmlButton next;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox cb_newDatabase;
        protected System.Web.UI.WebControls.Literal MessageInfo;
        protected System.Web.UI.WebControls.HiddenField Hidden1;

        protected void Page_Load(object sender, EventArgs e)
        {
            //读取默认dnt.config文件内容
            BaseConfigInfo dntConfigInfo = BaseConfigProvider.GetRealBaseConfig();
            if (dntConfigInfo != null)
            {
                FillDatabaseInfo(dntConfigInfo.Dbconnectstring);
                tablePrefix = dntConfigInfo.Tableprefix;
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["type"] == "unchecked")
                {
                    CheckBoxUnchecked();
                }
                else if (Request.QueryString["type"] == "checked")
                {
                    CheckBoxChecked();
                }
            }
        }

        /// <summary>
        /// 从配置文件中的连接字符串填充界面上的数据库配置信息
        /// </summary>
        /// <param name="connectionstring"></param>
        protected void FillDatabaseInfo(string connectionstring)
        {

            foreach (string info in connectionstring.Split(';'))
            {
                if (info.ToLower().IndexOf("data source") >= 0)
                {
                    sqlServerIP = info.Split('=')[1].Trim();
                    continue;
                }
                if (info.ToLower().IndexOf("initial catalog") >= 0)
                {
                    dataBaseName = info.Split('=')[1].Trim();
                    continue;
                }
                if (info.ToLower().IndexOf("user id") >= 0)
                {
                    sqlUID = info.Split('=')[1].Trim();
                    continue;
                }
                if (info.ToLower().IndexOf("password") >= 0)
                {
                    sqlPassword = info.Split('=')[1].Trim();
                    continue;
                }
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
        protected void EditDntConfig(string dataSource, string userID, string password, string databaseName, string tablePrefix)
        {
            BaseConfigInfo baseConfig = BaseConfigs.GetBaseConfig();
            connectionString = string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=true",
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
        /// 检测连接并执行数据库相关操作
        /// </summary>
        /// <param name="dataSource">数据库地址</param>
        /// <param name="userID">数据库账号</param>
        /// <param name="password">数据库账号密码</param>
        /// <param name="databaseName">数据库名</param>
        /// <param name="tablePrefix">表前缀</param>
        /// <returns>0，1，2</returns>0：用户名或密码错误；1：指定的数据库不存在；2：都存在
        protected void CheckConnection(object sender, EventArgs e)
        {
            #region 检查信息
            if (!cb_newDatabase.Checked)
            {
                #region 没有选择新建数据库的操作
                    EditDntConfig(Request["sql_ip"], Request["sql_username"], Request["sql_password"], Request["sql_name"], Request["table_prefix"]);//执行成功后，将数据库连接信息写入DNT.config文件
                    ClientScript.RegisterStartupScript(typeof(Page), "error", "location.href='step4.aspx'", true);
                #endregion
            }
            else
            {
                #region 选择新建数据库的操作
                if (!CheckDatabaseUserInfo())//所填用户不存在或者密码错误
                {
                    #region
                    CreateDatabaseUser();//创建SQL登录名
                    if (CheckDatabaseExistsByDbManager() == 0)//指定数据库不存在
                    {
                        CreateDatabase();//创建指定数据库
                        CreateUserMap();//设置新创建的SQL用户的映射
                        EditDntConfig(Request["sql_ip"], Request["sql_username"], Request["sql_password"], Request["sql_name"], Request["table_prefix"]);//执行成功后，将数据库连接信息写入DNT.config文件
                        ClientScript.RegisterStartupScript(typeof(Page), "error", "location.href='step4.aspx'", true);
                    }
                    else
                    {
                        DeleteDatabase();//删除数据库
                        CreateDatabase();//创建数据库
                        if (Request["sql_username"].Trim() != "sa")
                        {
                            CreateUserMap();//设置新创建的SQL用户的映射
                        }
                        EditDntConfig(Request["sql_ip"], Request["sql_username"], Request["sql_password"], Request["sql_name"], Request["table_prefix"]);//执行成功后，将数据库连接信息写入DNT.config文件
                        ClientScript.RegisterStartupScript(typeof(Page), "error", "location.href='step4.aspx'", true);
                    }
                    #endregion
                }
                else
                {
                    if (CheckDatabaseExistsByDbManager() == 0)//指定数据库不存在
                    {
                        #region 创建数据库
                        CreateDatabase();//创建数据库
                        if (Request["sql_username"].Trim() != "sa")
                        {
                            CreateUserMap();//设置新创建的SQL用户的映射
                        }
                        #endregion
                        EditDntConfig(Request["sql_ip"], Request["sql_username"], Request["sql_password"], Request["sql_name"], Request["table_prefix"]);//执行成功后，将数据库连接信息写入DNT.config文件
                        ClientScript.RegisterStartupScript(typeof(Page), "error", "location.href='step4.aspx'", true);
                    }
                    else
                    {
                        DeleteDatabase();//删除数据库
                        CreateDatabase();//创建数据库
                        if (Request["sql_username"].Trim() != "sa")
                        {
                            CreateUserMap();//设置新创建的SQL用户的映射
                        }
                        EditDntConfig(Request["sql_ip"], Request["sql_username"], Request["sql_password"], Request["sql_name"], Request["table_prefix"]);//执行成功后，将数据库连接信息写入DNT.config文件
                        ClientScript.RegisterStartupScript(typeof(Page), "error", "location.href='step4.aspx'", true);
                    }
                }
                #endregion
            }
            #endregion
        }

        /// <summary>
        /// 测试用户填写的数据库账号和密码是否正确
        /// </summary>
        /// <returns>false:数据库用户名或密码错误</returns>
        protected bool CheckDatabaseUserInfo()
        {
            try
            {
                if ((Request.QueryString["type"] == "unchecked") || Request.QueryString["type"] == "checked")
                {
                    connection.ConnectionString = string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=false",
                                                  Request.QueryString["sql_ip"], Request.QueryString["sql_username"], Request.QueryString["sql_password"], "master");
                    connection.Open();
                }
                else
                {
                    connection.ConnectionString = string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=false",
                                                  Request["sql_ip"], Request["sql_username"], Request["sql_password"], "master");
                    connection.Open();
                }
            }
            catch (Exception)
            {
                if ((Request.QueryString["type"] == "unchecked") || Request.QueryString["type"] == "checked")
                {
                    Response.Write("{\"Result\":false,\"Message\":\"数据库用户名或者密码错误！请重新填写\"}");
                    Response.End();
                }
                return false;
            }
            finally 
            { 
                connection.Close();
            }
            return true;
        }

        /// <summary>
        /// 测试用户填写的数据库管理员账号和密码是否正确
        /// </summary>
        /// <returns>false:数据库用户名或密码错误</returns>
        protected bool CheckDBManagerInfo()
        {
            try
            {
                connection.ConnectionString = string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=false",
                                              Request["sql_ip"], Request["sql_manager"], Request["sql_managerpassword"], "master");
                connection.Open();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }
        

        /// <summary>
        /// 执行SQL语句，用来测试指定数据库是否存在
        /// </summary>
        /// <param name="commandText">t-sql</param>
        protected void ExcuteSQL(string commandText,string connectionString)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.ConnectionString = connectionString;
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(commandText, connection);
                //sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// 检测数据库版本
        /// </summary>
        /// <param name="commandText">t-sql</param>
        /// <param name="connectionString">数据库连接串</param>
        protected void GetSqlVersion(string connectionString)
        {
            try
            {
                
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.ConnectionString = connectionString;
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT @@VERSION", connection);
                sqlVersion = sqlCommand.ExecuteScalar().ToString().Trim();
                sqlCommand.Connection.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 执行TSQL并返回查询结果的第一行第一列的值
        /// </summary>
        /// <param name="commandText">TSQL</param>
        /// <param name="connectionString">连接串</param>
        /// <returns></returns>
        protected int ExcuteReader(string commandText, string connectionString)
        {
            int result = 0;
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.ConnectionString = connectionString;
                SqlCommand sqlCommand = new SqlCommand(commandText, connection);
                sqlCommand.Connection.Open();
                result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlCommand.Connection.Close();
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }
        /// <summary>
        /// 检测数据库是否存在
        /// </summary>
        /// <returns></returns>
        protected int CheckDatabaseExists()
        {
            int result = 0;
            try
            {
                connectionString = string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=false",
                                          Request.QueryString["sql_ip"], Request.QueryString["sql_username"], Request.QueryString["sql_password"], Request.QueryString["sql_name"]);
                commandText = string.Format("SELECT DB_ID('{0}')", Request.QueryString["sql_name"]);
                result = ExcuteReader(commandText, connectionString);
                //EditDntConfig(Request["sql_ip"], Request["sql_username"], Request["sql_password"], Request["sql_name"], Request["table_prefix"]);//执行成功后，将数据库连接信息写入DNT.config文件
            }
            catch (Exception)
            {
                Response.Write("{\"Result\":false,\"Message\":\"数据库不存在！请重新填写；或者选择自动创建数据库\"}");
                Response.End();
                return result;
            }
            if (result == 0)
            {
                Response.Write("{\"Result\":false,\"Message\":\"数据库不存在！请重新填写；或者选择自动创建数据库\"}");
                Response.End();
                return result;
            }
            else
            {
                Response.Write("{\"Result\":true,\"Message\":true}");
                Response.End();
                return result;
            }
        }

        /// <summary>
        /// 用数据库管理员的账号检测数据库是否存在
        /// </summary>
        /// <returns></returns>
        protected int CheckDatabaseExistsByDbManager()
        {
            int result = 0;
            try
            {
                connectionString = string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=false",
                                          Request["sql_ip"], Request["sql_manager"], Request["sql_managerpassword"], Request["sql_name"]);
                commandText = string.Format("SELECT DB_ID('{0}')", Request["sql_name"]);
                result = ExcuteReader(commandText, connectionString);
                EditDntConfig(Request["sql_ip"], Request["sql_username"], Request["sql_password"], Request["sql_name"], Request["table_prefix"]);//执行成功后，将数据库连接信息写入DNT.config文件
            }
            catch (Exception)
            {
                return result;
            }
            return result;
        }
       

        /// <summary>
        /// 创建数据库
        /// </summary>
        protected void CreateDatabase()
        {
            connectionString = string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=false",
                                                  Request["sql_ip"], Request["sql_manager"], Request["sql_managerpassword"], "master");
            commandText = string.Format("CREATE DATABASE [{0}]", Request["sql_name"]);
            try
            {
                ExcuteSQL(commandText, connectionString);//执行创建数据库的TSQL；
            }
            catch {
                ClientScript.RegisterStartupScript(typeof(Page), "error", "alert('创建数据库失败，管理员密码填写错误！')", true);
                return;
            }
        }

        /// <summary>
        /// 检查数据库管理员的账号信息
        /// </summary>
        /// <returns></returns>
        protected bool CheckDbManagerInfo()
        {
            connectionString = string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=false",
                                                              Request.QueryString["sql_ip"], Request.QueryString["sql_manager"], Request.QueryString["sql_managerpassword"], "master");
            commandText = string.Format("CREATE DATABASE [{0}]; DROP DATABASE [{0}]", "comsenz111");
            try
            {
                ExcuteSQL(commandText, connectionString);//执行创建数据库的TSQL；
            }
            catch
            {
                //ClientScript.RegisterStartupScript(typeof(Page), "error", "alert('创建数据库失败，管理员密码填写错误！')", true);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 创建数据库登录用户
        /// </summary>
        protected void CreateDatabaseUser()
        {
            connectionString = string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=false",
                                              Request["sql_ip"], Request["sql_manager"], Request["sql_managerpassword"], "master");
            GetSqlVersion(connectionString);
            if (sqlVersion.Contains("2000"))
                commandText = string.Format(@"USE [master]; EXEC master.dbo.sp_addlogin @loginame = N'{0}', @passwd = N'{1}', @defdb = N'master'", Request["sql_username"], Request["sql_password"]);
            else
                commandText = string.Format(@"USE [master]; CREATE LOGIN [{0}] WITH PASSWORD='{1}', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF", Request["sql_username"], Request["sql_password"]);
            try
            {
                ExcuteSQL(commandText, connectionString);
            }
            catch {
                ClientScript.RegisterStartupScript(typeof(Page), "error", "alert('创建数据库登录用户失败，管理员密码填写错误！')", true);
                return;
            }
        }

        /// <summary>
        /// 创建用户映射
        /// </summary>
        protected void CreateUserMap()
        {
            connectionString = string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=false",
                                              Request["sql_ip"], Request["sql_manager"], Request["sql_managerpassword"], "master");
            GetSqlVersion(connectionString);
            if (sqlVersion.Contains("2000"))
                commandText = string.Format("USE [{0}];EXEC dbo.sp_grantdbaccess @loginame = N'{1}', @name_in_db = N'{1}';EXEC sp_addrolemember 'db_datareader', '{1}';EXEC sp_addrolemember 'db_datawriter', '{1}';EXEC sp_addrolemember 'db_ddladmin', '{1}';EXEC sp_addrolemember 'db_owner', '{1}'",
                                             Request["sql_name"], Request["sql_username"]);
            else
                commandText = string.Format("USE [{0}]; CREATE USER [{1}] FOR LOGIN [{1}];EXEC sp_addrolemember 'db_datareader', '{1}';EXEC sp_addrolemember 'db_datawriter', '{1}';EXEC sp_addrolemember 'db_ddladmin', '{1}';EXEC sp_addrolemember 'db_owner', '{1}'",
                                                 Request["sql_name"], Request["sql_username"]);
            try
            {
                ExcuteSQL(commandText, connectionString);//设置新创建的SQL用户的映射
            }
            catch {
                ClientScript.RegisterStartupScript(typeof(Page), "error", "alert('创建数据库用户映射失败，请检查数据库管理员权限！或者该用户映射已经存在')", true);
                return;
            }
        }

        /// <summary>
        /// 删除数据库
        /// </summary>
        protected void DeleteDatabase()
        {
            connectionString = string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=false",
                                              Request["sql_ip"], Request["sql_manager"], Request["sql_managerpassword"], "master");
            GetSqlVersion(connectionString);
            if (sqlVersion.Contains("2000"))
                commandText = string.Format("EXEC msdb.dbo.sp_delete_database_backuphistory @db_nm = '{0}';ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;DROP DATABASE [{0}]"
                                            , Request["sql_name"]);
            else
                commandText = string.Format("EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = '{0}';ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;DROP DATABASE [{0}]"
                                            , Request["sql_name"]);
            try
            {
                ExcuteSQL(commandText, connectionString);//执行删除数据库
            }
            catch {
                ClientScript.RegisterStartupScript(typeof(Page), "error", "alert('删除数据库失败，请检查数据库管理员权限！')", true);
                return;
            }
        }

        /// <summary>
        /// 没有选择自动创建数据库时,需要检测的内容,ajax使用
        /// </summary>
        protected void CheckBoxUnchecked()
        { 
            if (CheckDatabaseUserInfo() && CheckDatabaseExists() == 0)
                return; 
        }

        /// <summary>
        /// 选择自动创建数据库时，需要检测的内容
        /// </summary>
        protected void CheckBoxChecked()
        {
            if (Request.QueryString["sql_password"] != Request.QueryString["sql_confirmPassword"])
            {
                Response.Write("{\"Result\":false,\"Message\":\"两次密码输入不一致！请重新填写\"}");
                Response.End();
            } 
            else if (Utils.StrIsNullOrEmpty(Request.QueryString["sql_managerpassword"]))
            {
                Response.Write("{\"Result\":false,\"Message\":\"数据库管理员的密码不能为空！请重新填写\"}");
                Response.End();
            }
            else if (!CheckDbManagerInfo())//指定数据库管理员账号错误（密码错误或者没有建库权限）
            {
                Response.Write("{\"Result\":false,\"Message\":\"数据库管理员密码错误，或者没有建库权限\"}");
                Response.End();
            }
            else if (Request.QueryString["sql_username"] == "sa" && !CheckDbUserConnection())
            {
                Response.Write("{\"Result\":false,\"Message\":\"不能创建数据库用户名，sa为SQL SERVER保留账号；或者是密码填写错误！\"}");
                Response.End();
            }
            else if (CheckDatabaseExistsByDbManager_ajax() != 0) //指定数据库存在
            {
                Response.Write("{\"Result\":false,\"Exists\":true,\"Message\":\"指定数据库已存在，是否继续并覆盖？\"}");
                Response.End();
            }
        }

        /// <summary>
        /// 检查数据库用户是否存在（ajax使用）
        /// </summary>
        /// <returns></returns>
        protected bool CheckDbUserConnection()
        {
            connection.ConnectionString = string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=false",
                                                 Request.QueryString["sql_ip"], Request.QueryString["sql_username"], Request.QueryString["sql_password"], "master");
            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                
                return false;
            }
            return true;
        }

        /// <summary>
        /// 用数据库管理员的账号检测数据库是否存在
        /// </summary>
        /// <returns></returns>
        protected int CheckDatabaseExistsByDbManager_ajax()
        {
            int result = 0;
            try
            {
                connectionString = string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Pooling=false",
                                          Request.QueryString["sql_ip"], Request.QueryString["sql_manager"], Request.QueryString["sql_managerpassword"], Request.QueryString["sql_name"]);
                commandText = string.Format("SELECT DB_ID('{0}')", Request.QueryString["sql_name"]);
                result = ExcuteReader(commandText, connectionString);
            }
            catch (Exception)
            {
                return result;
            }
            return result;
        }
    }
}
