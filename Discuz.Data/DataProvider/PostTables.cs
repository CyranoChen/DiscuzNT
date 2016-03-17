using System;
using System.Text;
using System.Data;

using Discuz.Config;
using Discuz.Cache;

namespace Discuz.Data
{
    public class PostTables
    {   
        /// <summary>
        /// 得到当前所用帖子表分表ID
        /// </summary>
        /// <returns>分表ID</returns>
        public static string GetPostTableId()
        {
            DNTCache cache = DNTCache.GetCacheService();
            string postTableId = cache.RetrieveObject("/Forum/PostTableId") as string;
            if (postTableId == null)
            {
                DataTable dt = GetAllPostTableName();
                if (dt != null && dt.Rows.Count > 0)
                {
                    postTableId = dt.Rows[0]["id"].ToString();
                    cache.AddObject("/Forum/PostTableId", postTableId);
                }
            }
            return postTableId;
        }

        /// <summary>
        /// 得到用户帖子分表信息
        /// </summary>
        /// <returns>分表记录集</returns>
        public static DataTable GetAllPostTableName()
        {
            DNTCache cache = DNTCache.GetCacheService();
            DataTable postTables = cache.RetrieveObject("/Forum/PostTables") as DataTable;
            if (postTables == null)
            {
                DataSet ds = DatabaseProvider.GetInstance().GetAllPostTableName();
                if (ds != null && ds.Tables.Count > 0)
                {
                    postTables = ds.Tables[0];
                    cache.AddObject("/Forum/PostTables", postTables);
                }
            }
            return postTables;
        }

        /// <summary>
        /// .重置分表信息(仅限管理后台新建分表时使用
        /// </summary>
        public static void ResetPostTables()
        {
            //如果新建分表后当前IIS站点为多web园且net版本低于4.0， 则重启iis站点
            if (GeneralConfigs.GetConfig().Webgarden > 1 && Environment.Version.Major < 4)
            {
                Discuz.Common.Utils.RestartIISProcess();
                return;
            }
            
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/PostTables");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/PostTableId");        
        }        
        

        /// <summary>
        /// 得到指定主题的帖子所在分表ID
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>分表ID</returns>
        public static string GetPostTableId(int tid)
        {
            if (tid < 1)
                return GetPostTableId();

            string id = "1";
            DataTable dt = GetAllPostTableName();
            if (dt != null)
            {
                DataRow[] dr = dt.Select(string.Format("[mintid]<={0} AND ([maxtid]<=0 OR [maxtid]>={0})", tid.ToString()));

                if (dr != null && dr.Length > 0)
                   id = dr[dr.Length - 1]["id"].ToString();              
            }
            dt.Dispose();
            return id;
        }

        /// <summary>
        /// 得到当前所用帖子表分表的表名
        /// </summary>
        /// <returns>分表表名</returns>
        public static string GetPostTableName()
        {
            return string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, GetPostTableId());
        }

        /// <summary>
        /// 得到指定主题的帖子所分表的表名
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>分表表名</returns>
        public static string GetPostTableName(int tid)
        {
            return string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, GetPostTableId(tid));
        }

        /// <summary>
        /// 获取指定分表的帖数
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public static int GetPostTableCount(string tableName)
        {
            return DatabaseProvider.GetInstance().GetPostCount(tableName);
        }

        /// <summary>
        /// 获取分表列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPostTableList()
        {
            return DatabaseProvider.GetInstance().GetPostTableList();
        }

        /// <summary>
        /// 更新分表描述
        /// </summary>
        /// <param name="detachTableId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static int UpdateDetachTable(int detachTableId,string  description)
        {
            return DatabaseProvider.GetInstance().UpdateDetachTable(detachTableId, description);
        }

        /// <summary>
        /// 更新当前表中最大ID的记录用的最大和最小tid字段		
        /// </summary>
        /// <param name="posttablename"></param>
        /// <param name="tablelistmaxid"></param>
        public static void UpdateMinMaxField(string posttablename, int tablelistmaxid)
        {
            DatabaseProvider.GetInstance().UpdateMinMaxField(posttablename, tablelistmaxid);
        }

        public static void AddPostTableToTableList(string description,int posttablename)
        {
            DatabaseProvider.GetInstance().AddPostTableToTableList(description, posttablename, 0);
        }

        public static int GetMaxPostTableTid(string posttabelname)
        {
            return DatabaseProvider.GetInstance().GetMaxPostTableTid(posttabelname);
        }
        
        public static DataTable GetMaxTid()
        {
            return DatabaseProvider.GetInstance().GetMaxTid();
        }

        public static void CreateStoreProc(int tablelistmaxid)
        {
            DatabaseProvider.GetInstance().CreateStoreProc(tablelistmaxid);
        }

        public static bool CreateORFillIndex(string DbName, string postid)
        {
           return  DatabaseProvider.GetInstance().CreateORFillIndex(DbName, postid);
        }
    }
}
