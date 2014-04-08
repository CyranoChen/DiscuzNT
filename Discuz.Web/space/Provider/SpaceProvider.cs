using System;
using System.Data;

using Discuz.Cache;
using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Forum;
using Discuz.Space.Entities;
using Discuz.Entity;
using Discuz.Data;

namespace Discuz.Space.Provider
{
	/// <summary>
	/// SpaceProvider 的摘要说明。
	/// </summary>
	public class SpaceProvider
	{
		public SpaceProvider()
		{}

		#region 对ThemeInfo的操作
		public static ThemeInfo GetThemeInfoById(int themeInfoId)
		{
            List<ThemeInfo> themes = GetThemeInfos();
            foreach (ThemeInfo info in themes)
            {
                if (info.ThemeId == themeInfoId)
                {
                    return info;
                }
            }
            return null;
		}

        public static Discuz.Common.Generic.List<ThemeInfo> GetThemeInfos()
        {
            DNTCache cache = DNTCache.GetCacheService();
            List<ThemeInfo> themes = cache.RetrieveObject("/Space/ThemeList") as List<ThemeInfo>;
            if (themes == null)
            {
                IDataReader reader = Space.Data.DbProvider.GetInstance().GetThemeInfos();
                themes = GetThemeInfoArray(reader);
                cache.AddObject("/Space/ThemeList", themes);
            }
            return themes;
        }        

        private static Discuz.Common.Generic.List<ThemeInfo> GetThemeInfoArray(IDataReader reader)
		{
			if (reader == null)
				return null;

            Discuz.Common.Generic.List<ThemeInfo> tic = new Discuz.Common.Generic.List<ThemeInfo>();
			while (reader.Read())
			{
				tic.Add(GetThemeEntity(reader));
			}
            reader.Close();
			return tic;
		}

		private static ThemeInfo GetThemeEntity(IDataReader reader)
		{
			ThemeInfo ti = new ThemeInfo();
			ti.ThemeId = TypeConverter.ObjectToInt(reader["themeid"], 0);
			ti.Directory = reader["directory"].ToString();
			ti.Name = reader["name"].ToString();
			ti.Type = TypeConverter.ObjectToInt(reader["type"], 0);
			ti.Author = reader["author"].ToString();
			ti.CreateDate = reader["createdate"].ToString();
			ti.CopyRight = reader["copyright"].ToString();
			return ti;
		}
		#endregion
      

		#region 对ModuleDefInfo的操作


        /// <summary>
        /// 根据Url获得ModuleDef对象
        /// </summary>
        /// <returns></returns>
        public static ModuleDefInfo GetModuleDefInfoByUrl(string moduleDefInfoUrl)
        {
            foreach (ModuleDefInfo info in GetModuleDefInfoList())
            {
                if (info.ConfigFile == moduleDefInfoUrl)
                    return info;
            }
            return null;
        }

		/// <summary>
		/// 根据Id获得ModuleDef对象
		/// </summary>
		/// <param name="moduleDefInfoId"></param>
		/// <returns></returns>
		public static ModuleDefInfo GetModuleDefInfoById(int moduleDefInfoId)
		{
		    foreach (ModuleDefInfo info in GetModuleDefInfoList())
		    {
		        if (info.ModuleDefID == moduleDefInfoId)
		            return info;
		    }
		    return null;
		}

        public static List<ModuleDefInfo> GetModuleDefInfoList()
        {
            DNTCache cache = DNTCache.GetCacheService();
            List<ModuleDefInfo> result = cache.RetrieveObject("/Space/ModuleDefList") as List<ModuleDefInfo>;

            if (result == null)
            {
                result = GetModuleDefInfoArray(Space.Data.DbProvider.GetInstance().GetModuleDefList());
                cache.AddObject("/Space/ModuleDefList", result);
            }
            return result;
        }

        private static Discuz.Common.Generic.List<ModuleDefInfo> GetModuleDefInfoArray(IDataReader reader)
		{
			if (reader == null)
				return null;

            Discuz.Common.Generic.List<ModuleDefInfo> mdic = new Discuz.Common.Generic.List<ModuleDefInfo>();
			while (reader.Read())
			{
				mdic.Add(GetModuleDefEntity(reader));
			}
            reader.Close();
			return mdic.Count == 0 ? null : mdic;
		}

		private static ModuleDefInfo GetModuleDefEntity(IDataReader reader)
		{
			ModuleDefInfo mdi = new ModuleDefInfo();
			mdi.ModuleDefID = TypeConverter.ObjectToInt(reader["moduledefid"], 0);
			mdi.ModuleName = reader["modulename"].ToString();
			mdi.CacheTime = TypeConverter.ObjectToInt(reader["cachetime"], 0);
			mdi.ConfigFile = reader["configfile"].ToString();
			mdi.BussinessController = reader["controller"].ToString();
			return mdi;
		}

        public static void UpdateModuleDefInfo(ModuleDefInfo mdi)
        {
            Space.Data.DbProvider.GetInstance().UpdateModuleDef(mdi);
        }
        
        public static void AddModuleDefInfo(ModuleDefInfo mdi)
        {
            Space.Data.DbProvider.GetInstance().AddModuleDefInfo(mdi);
        }

        public static void DeleteModuleDefByUrl(string url)
        {
            Space.Data.DbProvider.GetInstance().DeleteModuleDefByUrl(url);
        }
		#endregion

		#region 对ModuleInfo的操作

		public static int GetModulesCountByTabId(int tabId, int uid)
		{
			return Space.Data.DbProvider.GetInstance().GetModulesCountByTabId(tabId, uid);
		}

        public static Discuz.Common.Generic.List<ModuleInfo> GetModuleInfosByTabId(int tabId, int uid)
    	{
			return GetModuleInfoArray(Space.Data.DbProvider.GetInstance().GetModulesByTabId(tabId, uid));
		}

		public static ModuleInfo GetModuleInfoById(int moduleInfoId, int uid)
		{
			IDataReader reader = Space.Data.DbProvider.GetInstance().GetModuleInfoById(moduleInfoId, uid);
			return reader.Read() ? GetModuleEntity(reader) : null;
		}

        private static Discuz.Common.Generic.List<ModuleInfo> GetModuleInfoArray(IDataReader reader)
    	{
			if (reader == null)
				return null;

            Discuz.Common.Generic.List<ModuleInfo> mdic = new Discuz.Common.Generic.List<ModuleInfo>();
			while (reader.Read())
			{
				mdic.Add(GetModuleEntity(reader));
			}
            reader.Close();
			return mdic.Count == 0 ? null : mdic;
		}

		private static ModuleInfo GetModuleEntity(IDataReader reader)
		{
			ModuleInfo mi = new ModuleInfo();
			mi.ModuleID = TypeConverter.ObjectToInt(reader["moduleid"]);
			mi.TabID = TypeConverter.ObjectToInt(reader["tabid"]);
			mi.Uid = TypeConverter.ObjectToInt(reader["uid"], -1);
			mi.ModuleDefID = TypeConverter.ObjectToInt(reader["moduledefid"]);
			mi.PaneName = reader["panename"].ToString();
			mi.DisplayOrder = TypeConverter.ObjectToInt(reader["displayorder"]);
			mi.UserPref = reader["userpref"].ToString();
			mi.Val = TypeConverter.ObjectToInt(reader["val"]);
			mi.ModuleUrl = reader["moduleurl"].ToString();
			int moduletype = TypeConverter.ObjectToInt(reader["moduletype"], 4);
			if (moduletype < 1 || moduletype > 4)
				moduletype = 4;

            mi.ModuleType = (ModuleType)moduletype;
			return mi;
		}

        public static Common.Generic.List<ModuleInfo> GetModuleCollectionByUserId(int uid)
        {
            return GetModuleInfoArray(Space.Data.DbProvider.GetInstance().GetModulesByUserId(uid));
        }

		#endregion

		#region 对TabInfo的操作

		/// <summary>
		/// 根据Uid获得TabInfo
		/// </summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		public static Discuz.Common.Generic.List<TabInfo> GetTabInfosByUid(int uid)
		{
			return GetTabInfoArray(Space.Data.DbProvider.GetInstance().GetTabInfosByUid(uid));
		}


		public static TabInfo GetTabInfoById(int tabInfoId, int uid)
		{
			IDataReader reader = Space.Data.DbProvider.GetInstance().GetTabInfoById(tabInfoId, uid);
			return reader.Read() ? GetTabEntity(reader) : null;
		}

		private static Discuz.Common.Generic.List<TabInfo> GetTabInfoArray(IDataReader reader)
		{
			if (reader == null)
				return null;

            Discuz.Common.Generic.List<TabInfo> tabc = new Discuz.Common.Generic.List<TabInfo>();
			while (reader.Read())
			{
				tabc.Add(GetTabEntity(reader));
			}
            reader.Close();
			return tabc.Count == 0 ? null : tabc;
		}

		private static TabInfo GetTabEntity(IDataReader reader)
		{
			TabInfo tab = new TabInfo();
			tab.TabID = TypeConverter.ObjectToInt(reader["tabid"]);
			tab.UserID = TypeConverter.ObjectToInt(reader["uid"]);
			tab.DisplayOrder = TypeConverter.ObjectToInt(reader["displayorder"]);
			tab.TabName = reader["tabname"].ToString();
			tab.IconFile = reader["iconfile"].ToString();
			tab.Template = reader["template"].ToString();
			return tab;
		}
		#endregion

        /// <summary>
        /// 获取使用指定Tag的空间日志数
        /// </summary>
        /// <param name="tagid">TagId</param>
        /// <returns></returns>
        public static int GetSpacePostCountWithSameTag(int tagid)
        {
            return Space.Data.DbProvider.GetInstance().GetSpacePostCountWithSameTag(tagid);
        }
    }
}
