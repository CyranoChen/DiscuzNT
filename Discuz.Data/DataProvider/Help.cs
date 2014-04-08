using System;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Common.Generic;
using Discuz.Common;

namespace Discuz.Data
{
    public class Help
    {
        /// <summary>
        /// 获取帮助列表
        /// </summary>
        /// <returns>帮助列表</returns>
        public static List<HelpInfo> GetHelpList()
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetHelpList();
            List<HelpInfo> helplist = new List<HelpInfo>();

            while (reader.Read())
            {
                HelpInfo info = new HelpInfo();
                info.Id = TypeConverter.ObjectToInt(reader["id"]);
                info.Title = reader["title"].ToString();
                info.Message = reader["message"].ToString();
                info.Pid = TypeConverter.ObjectToInt(reader["pid"]);
                info.Orderby = TypeConverter.ObjectToInt(reader["orderby"]);
                helplist.Add(info);
            }
            reader.Close();
            return helplist;
        }

        /// <summary>
        /// 获取帮助内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns>帮助内容</returns>
        public static HelpInfo GetMessage(int id)
        {
            IDataReader reader = DatabaseProvider.GetInstance().ShowHelp(id);
            HelpInfo info = null;
            if (reader != null)
            {
                if (reader.Read())
                {
                    info = new HelpInfo();
                    info.Title = reader["title"].ToString();
                    info.Message = reader["message"].ToString();
                    info.Pid = TypeConverter.ObjectToInt(reader["pid"]);
                    info.Orderby = TypeConverter.ObjectToInt(reader["orderby"]);
                }
                reader.Close();
            }
            return info;
        }


        /// <summary>
        /// 更新帮助信息
        /// </summary>
        /// <param name="id">帮助ID</param>
        /// <param name="title">帮助标题</param>
        /// <param name="message">帮助内容</param>
        /// <param name="pid">帮助</param>
        /// <param name="orderby">排序方式</param>
        public static void UpdateHelp(int id, string title, string message, int pid, int orderby)
        {
            DatabaseProvider.GetInstance().UpdateHelp(id, title, message, pid, orderby);
        }

        /// <summary>
        /// 增加帮助
        /// </summary>
        /// <param name="title">帮助标题</param>
        /// <param name="message">帮助内容</param>
        /// <param name="pid">帮助</param>
        public static void AddHelp(string title, string message, int pid)
        {
            DatabaseProvider.GetInstance().AddHelp(title, message, pid, DatabaseProvider.GetInstance().HelpCount());
        }

        /// <summary>
        /// 删除帮助
        /// </summary>
        /// <param name="idlist">帮助ID序列</param>
        public static void DelHelp(string idlist)
        {
            DatabaseProvider.GetInstance().DelHelp(idlist);
        }

        /// <summary>
        /// 返回帮助的分类列表的SQL语句
        /// </summary>
        /// <returns>帮助的分类列表的SQL语句</returns>
        public static DataTable GetHelpTypes()
        {
            return DatabaseProvider.GetInstance().GetHelpTypes();
        }

        /// <summary>
        /// 更新帮助序号
        /// </summary>
        /// <param name="orderby">排序号</param>
        /// <param name="id">帮助Id</param>
        public static void UpdateOrder(string orderby, string id)
        {
            DatabaseProvider.GetInstance().UpdateOrder(orderby, id);
        }     
    }
}
