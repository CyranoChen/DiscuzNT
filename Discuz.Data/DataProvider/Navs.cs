using System;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Common.Generic;
using Discuz.Common;

namespace Discuz.Data
{
    public class Navs
    {
        /// <summary>
        /// 得到自定义菜单信息
        /// </summary>
        /// <param name="getAll">是否获取全部导航菜单</param>
        /// <returns></returns>
        public static List<NavInfo> GetNavigation(bool getAll)
        {
            List<NavInfo> info = new List<NavInfo>();
            IDataReader reader = GetNavigationData(getAll);
            while (reader.Read())
            {
                NavInfo m = new NavInfo();
                m.Id = TypeConverter.ObjectToInt(reader["id"], 0);
                m.Level = TypeConverter.ObjectToInt(reader["level"], 0);
                m.Name = reader["name"].ToString().Trim();
                m.Parentid = TypeConverter.ObjectToInt(reader["parentid"], 0);
                m.Target = TypeConverter.ObjectToInt(reader["target"], 0);
                m.Title = reader["title"].ToString().Trim();
                m.Type = TypeConverter.ObjectToInt(reader["type"], 0);
                m.Url = reader["url"].ToString().Trim();
                m.Available = TypeConverter.ObjectToInt(reader["available"], 0);
                m.Displayorder = TypeConverter.ObjectToInt(reader["displayorder"], 0);
                info.Add(m);
            }
            reader.Close();
            return info;
        }

        /// <summary>
        /// 获取可用导航菜单
        /// </summary>
        /// <returns></returns>
        public static List<NavInfo> GetNavigation()
        {
            return GetNavigation(false);
        }

        /// <summary>
        /// 得到自定义菜单信息数据
        /// </summary>
        /// <param name="getAll">是否获取全部导航菜单</param>
        /// <returns></returns>
        public static IDataReader GetNavigationData(bool getAll)
        {
            return DatabaseProvider.GetInstance().GetNavigationData(getAll);
        }

        /// <summary>
        /// 得到自定义菜单不重复的PARENTID
        /// </summary>
        /// <returns></returns>
        public static List<NavInfo> GetNavigationHasSub()
        {
            List<NavInfo> info = new List<NavInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetNavigationHasSub();
            while (reader.Read())
            {
                NavInfo m = new NavInfo();
                m.Parentid = TypeConverter.ObjectToInt(reader["parentid"], 0);
                info.Add(m);
            }
            reader.Close();
            return info;
        }

        /// <summary>
        /// 删除导航
        /// </summary>
        /// <param name="id">菜单ID</param>
        public static void DeleteNavigation(int id)
        {
            DatabaseProvider.GetInstance().DeleteNavigation(id);
        }

        /// <summary>
        /// 添加导航菜单
        /// </summary>
        /// <param name="nav">导航菜单类</param>
        public static void InsertNavigation(NavInfo nav)
        {
            DatabaseProvider.GetInstance().InsertNavigation(nav);
        }

        /// <summary>
        /// 更新导航菜单
        /// </summary>
        /// <param name="nav">导航菜单类</param>
        public static void UpdateNavigation(NavInfo nav)
        {
            DatabaseProvider.GetInstance().UpdateNavigation(nav);
        }

    }
}
