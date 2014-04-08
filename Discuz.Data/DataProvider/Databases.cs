using System;
using System.Text;

namespace Discuz.Data
{
    public class Databases
    {
        /// <summary>
        /// 恢复备份数据库          
        /// </summary>
        /// <param name="backupPath">备份文件路径</param>
        /// <param name="serverName">服务器名称</param>
        /// <param name="userName">数据库用户名</param>
        /// <param name="password">数据库密码</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="fileName">备份文件名</param>
        /// <returns></returns>
        public static string RestoreDatabase(string backupPath, string serverName, string userName, string password, string dbName, string fileName)
        {
            return DatabaseProvider.GetInstance().RestoreDatabase(backupPath, serverName, userName, password, dbName, fileName);
        }

        /// <summary>
        /// 备份数据库          
        /// </summary>
        /// <param name="backupPath">备份文件路径</param>
        /// <param name="serverName">服务器名称</param>
        /// <param name="userName">数据库用户名</param>
        /// <param name="password">数据库密码</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="fileName">备份文件名</param>
        /// <returns></returns>
        public static string BackUpDatabase(string backupPath, string serverName, string userName, string password, string dbName, string fileName)
        {
            return DatabaseProvider.GetInstance().BackUpDatabase(backupPath, serverName, userName, password, dbName, fileName);
        }

        /// <summary>
        /// 是否允许备份数据库
        /// </summary>
        /// <returns></returns>
        public static bool IsBackupDatabase()
        {
            return DbHelper.Provider.IsBackupDatabase();
        }

        /// <summary>
        /// 检测全文索引
        /// </summary>
        /// <param name="tableId">分表ID</param>
        public static void TestFullTextIndex(int tableId)
        {
            DatabaseProvider.GetInstance().TestFullTextIndex(tableId);
        }

        /// <summary>
        /// 是否可用全文搜索
        /// </summary>
        /// <returns></returns>
        public static bool IsFullTextSearchEnabled()
        {
            return DbHelper.Provider.IsFullTextSearchEnabled();
        }

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <returns></returns>
        public static string GetDbName()
        {
            return DatabaseProvider.GetInstance().GetDbName();
        }

        /// <summary>
        /// 开始填充索引
        /// </summary>
        /// <param name="dbname"></param>
        public static void StartFullIndex(string dbname)
        {
            DatabaseProvider.GetInstance().StartFullIndex(dbname);
        }

        /// <summary>
        /// 构建相应表及全文索引
        /// </summary>
        /// <param name="?"></param>
        public static void CreatePostTableAndIndex(string tablename)
        {
            DatabaseProvider.GetInstance().CreatePostTableAndIndex(tablename);
        }

        /// <summary>
        /// 是否允许创建存储过程
        /// </summary>
        /// <returns></returns>
        public static bool IsStoreProc()
        {
            return DbHelper.Provider.IsStoreProc();
        }

        /// <summary>
        /// 是否支持收缩数据库
        /// </summary>
        /// <returns></returns>
        public static bool IsShrinkData()
        {
            return DbHelper.Provider.IsShrinkData();
        }

        /// <summary>
        /// 收缩数据库
        /// </summary>
        /// <param name="shrinksize">收缩大小</param>
        /// <param name="dbname">数据库名</param>
        public static void ShrinkDataBase(string shrinksize, string dbname)
        {
            DatabaseProvider.GetInstance().ShrinkDataBase(shrinksize, dbname);
        }

        /// <summary>
        /// 清空数据库日志
        /// </summary>
        /// <param name="dbname"></param>
        public static void ClearDBLog(string dbname)
        {
            DatabaseProvider.GetInstance().ClearDBLog(dbname);
        }

        /// <summary>
        /// 运行Sql语句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        public static string RunSql(string sql)
        {
            return DatabaseProvider.GetInstance().RunSql(sql);
        }

        /// <summary>
        /// 建立全文索引
        /// </summary>
        /// <param name="tableName">表名</param>
        public static void CreateFullTextIndex(string tableName)
        {
            DatabaseProvider.GetInstance().CreateFullTextIndex(tableName);
        }

        ///// <summary>
        ///// 更新分表存储过程
        ///// </summary>
        //public static void UpdatePostSP()
        //{
        //    DatabaseProvider.GetInstance().UpdatePostSP();
        //}

        /// <summary>
        /// 更新分表存储过程
        /// </summary>
        public static void UpdatePostSP(int postTableID)
        {
            DatabaseProvider.GetInstance().CreateStoreProc(postTableID);
        }

        /// <summary>
        /// 获取数据库版本
        /// </summary>
        /// <returns></returns>
        public static string GetDataBaseVersion()
        {
            return DatabaseProvider.GetInstance().GetDataBaseVersion();
        }
    }
}
