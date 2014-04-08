using System;
using System.Data;
using System.Text;
using System.IO;
using System.Drawing;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Entities;
using Discuz.Space.Provider;
using Discuz.Space.Utilities;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Data;
using Discuz.Common.Generic;
using System.Collections;
using Discuz.Space.Data;

namespace Discuz.Space
{
	/// <summary>
	/// Spaces 的摘要说明。
	/// </summary>
	public class Spaces
	{
		public Spaces()
		{}

        /// <summary>
        /// 获得指定TabID的模块数量
        /// </summary>
        /// <param name="tabId">tabID</param>
        /// <param name="uid">用户ID</param>
        /// <returns>模块数量</returns>
		public static int GetModulesCountByTabId(int tabId, int uid)
		{
			return SpaceProvider.GetModulesCountByTabId(tabId, uid);
		}

        /// <summary>
        /// 得到指定用户的Tab集合
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns>Tab集合</returns>
        public static Discuz.Common.Generic.List<TabInfo> GetTabInfoCollectionByUserID(int userid)
		{
			return SpaceProvider.GetTabInfosByUid(userid);
		}

        /// <summary>
        /// 根据ID获得模块
        /// </summary>
        /// <param name="mid">模块ID</param>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
		public static ModuleInfo GetModuleById(int mid, int uid)
		{
			return SpaceProvider.GetModuleInfoById(mid, uid);
		}

        /// <summary>
        /// 获得指定ID的内建模块定义数据
        /// </summary>
        /// <param name="moduleDefId">内建模块类型ID</param>
        /// <returns>内建模块类型对象</returns>
		public static ModuleDefInfo GetModuleDefById(int moduleDefId)
		{
			return SpaceProvider.GetModuleDefInfoById(moduleDefId);
		}

        /// <summary>
        /// 根据TabID获得模块集合
        /// </summary>
        /// <param name="tabid">tabID</param>
        /// <param name="uid">用户ID</param>
        /// <returns>模块集合</returns>
        public static Discuz.Common.Generic.List<ModuleInfo> GetModuleCollectionByTabId(int tabid, int uid)
		{
			return SpaceProvider.GetModuleInfosByTabId(tabid, uid);
		}

        /// <summary>
        /// 更新模块
        /// </summary>
        /// <param name="moduleInfo">模块对象</param>
		public static void UpdateModule(ModuleInfo moduleInfo)
		{
			Space.Data.DbProvider.GetInstance().UpdateModule(moduleInfo);
		}

        /// <summary>
        /// 根据用户ID获得空间的配置
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns>空间的配置</returns>
		public static SpaceConfigInfo GetSpaceConfigByUserId(int uid)
		{
            return BlogProvider.GetSpaceConfigInfo(uid);
		}

		#region Helper
		/// <summary>
		/// 设置模块实例
		/// </summary>
		/// <param name="moduleInfo">模块对象</param>
		/// <returns>模块基类</returns>
		public static ModuleBase SetModuleBase(ModuleInfo moduleInfo)
		{
			ModuleBase desktopModule = new ModuleBase();
			if (moduleInfo.ModuleDefID > 0)
			{
				// 重构，是否需要缓存？
				ModuleDefInfo moduleDef = Spaces.GetModuleDefById(moduleInfo.ModuleDefID);
				try
				{
					Type type = Type.GetType(moduleDef.BussinessController, false, true);
					desktopModule = (ModuleBase)Activator.CreateInstance(type);
				}
				catch(Exception ex)
				{
					string a = ex.Message;
				}
				desktopModule.ModuleDef = moduleDef;//貌似不需要，重构？

			}
			desktopModule.Module = moduleInfo;
			desktopModule.ModuleID = moduleInfo.ModuleID;
			if (moduleInfo.ModuleType == ModuleType.Local)
			{
				string path = BaseConfigs.GetForumPath + "space/modules/" + moduleInfo.ModuleUrl;
				string modulefilepath = Utils.GetMapPath(path);
				try
				{

					desktopModule.UserPrefCollection = ModuleXmlHelper.LoadUserPrefs(modulefilepath);
					desktopModule.ModulePref = ModuleXmlHelper.LoadModulePref(modulefilepath);
					desktopModule.ModuleContent = ModuleXmlHelper.LoadContent(modulefilepath);
				}
				catch (Exception ex)
				{
                    desktopModule.UserPrefCollection = new UserPrefCollection<UserPref>();
					desktopModule.ModulePref = new ModulePref();
					desktopModule.ModuleContent = new ModuleContent();
					desktopModule.ModuleContent.ContentHtml = ex.Message;
					desktopModule.ModuleContent.Type = ModuleContentType.HtmlInline;
				}
				if (desktopModule.UserPrefCollection == null)
                    desktopModule.UserPrefCollection = new UserPrefCollection<UserPref>();
				if (desktopModule.ModulePref == null)
					desktopModule.ModulePref = new ModulePref();

				if (desktopModule.ModuleContent == null)
				{
					desktopModule.ModuleContent = new ModuleContent();
					desktopModule.ModuleContent.ContentHtml = "模块" + path + "不存在";
					desktopModule.ModuleContent.Type = ModuleContentType.HtmlInline;
				}
			}	
			return desktopModule;
		}
		#endregion

        /// <summary>
        /// 获得指定ID的Tab
        /// </summary>
        /// <param name="tabid">TabID</param>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
		public static TabInfo GetTabById(int tabid, int uid)
		{
			return SpaceProvider.GetTabInfoById(tabid, uid);
		}
 
        /// <summary>
        /// 更新模块顺序
        /// </summary>
        /// <param name="mid">模块ID</param>
        /// <param name="uid">用户ID</param>
        /// <param name="panename">格子名称(插入哪一列)</param>
        /// <param name="displayorder">顺序</param>
		public static void UpdateModuleOrder(int mid, int uid, string panename, int displayorder)
		{
			Space.Data.DbProvider.GetInstance().UpdateModuleOrder(mid, uid, panename, displayorder);
		}

        /// <summary>
        /// 更新Tab
        /// </summary>
        /// <param name="tab">Tab对象</param>
		public static void UpdateTab(TabInfo tab)
		{
			Space.Data.DbProvider.GetInstance().UpdateTab(tab);
		}

        /// <summary>
        /// 获得每个用户的Tab数量
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns>Tab数量</returns>
		public static int GetTabCountByUserId(int userid)
		{
			return Space.Data.DbProvider.GetInstance().GetTabInfoCountByUserId(userid);
		}

        /// <summary>
        /// 添加Tab
        /// </summary>
        /// <param name="tabinfo">Tab对象</param>
		public static void AddTab(TabInfo tabinfo)
		{
			Space.Data.DbProvider.GetInstance().AddTab(tabinfo);
		}

        /// <summary>
        /// 清除用户设置的默认Tab
        /// </summary>
        /// <param name="userid">用户ID</param>
		public static void ClearDefaultTab(int userid)
		{
			Space.Data.DbProvider.GetInstance().ClearDefaultTab(userid);
		}

        /// <summary>
        /// 设置用户默认Tab
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="tabid">TabID</param>
		public static void SetDefaultTab(int userid, int tabid)
		{
			Space.Data.DbProvider.GetInstance().SetDefaultTab(userid, tabid);
		}

        /// <summary>
        /// 删除指定用户的制定Tab
        /// </summary>
        /// <param name="tabid">TabID</param>
        /// <param name="uid">用户ID</param>
		public static void DeleteTabById(int tabid, int uid)
		{
			Space.Data.DbProvider.GetInstance().DeleteTab(tabid, uid);
		}

        /// <summary>
        /// 移动模块
        /// </summary>
        /// <param name="moduleid">模块ID</param>
        /// <param name="uid">用户ID</param>
        /// <param name="tabid">TabID</param>
		public static void MoveModule(int moduleid, int uid, int tabid)
		{
			Space.Data.DbProvider.GetInstance().UpdateModuleTab(moduleid, uid, tabid);
		}

        /// <summary>
        /// 设置主题风格
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="themeid">主题ID</param>
        /// <param name="themepath">主题路径</param>
		public static void SetTheme(int userid, int themeid, string themepath)
		{
			Space.Data.DbProvider.GetInstance().SetSpaceTheme(userid, themeid, themepath);
		}

        /// <summary>
        /// 根据模块文件URL获得内建模块定义ID
        /// </summary>
        /// <param name="url">模块文件URL</param>
        /// <returns>内建模块定义ID</returns>
		public static int GetModuleDefIdByUrl(string url)
		{
			return Space.Data.DbProvider.GetInstance().GetModuleDefIdByUrl(url);
		}

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="moduleinfo">模块对象</param>
		public static void AddModule(ModuleInfo moduleinfo)
		{
			Space.Data.DbProvider.GetInstance().AddModule(moduleinfo);
		}

        /// <summary>
        /// 删除指定模块
        /// </summary>
        /// <param name="moduleid">模块ID</param>
        /// <param name="uid">用户ID</param>
		public static void DeleteModuleById(int moduleid, int uid)
		{
			Space.Data.DbProvider.GetInstance().DeleteModule(moduleid, uid);
		}

        /// <summary>
        /// 获得用户的默认TabID
        /// </summary>
        /// <param name="spaceConfig">用户的空间设置</param>
        /// <param name="tc">Tab集合</param>
        /// <returns>TabID</returns>
        public static int GetDefaultTabId(SpaceConfigInfo spaceConfig, Discuz.Common.Generic.List<TabInfo> tc)
		{
			if (tc == null)
				return 0;

			int defaulttabid = 0;
			if (tc.Count > 0)
				defaulttabid = tc[tc.Count - 1].TabID;

			foreach (TabInfo tab in tc)
			{
				if (tab.TabID == spaceConfig.DefaultTab)
				{
					defaulttabid = tab.TabID;
					break;
				}
			}
			return defaulttabid;
		}

		/// <summary>
		/// 添加本地模块
		/// </summary>
		/// <param name="moduleUrl">模块地址</param>
		/// <param name="userId">用户Id</param>
		/// <param name="tabId">标签Id</param>
		/// <param name="col">列Id</param>
		public static void AddLocalModule(string moduleUrl, int userId, int tabId, int col)
		{
			TabInfo tab = Spaces.GetTabById(tabId, userId);
			if (tab == null || tab.UserID != userId)
				return;

            int moduledef = 0;
			if (moduleUrl.StartsWith("builtin_"))
				moduledef = Spaces.GetModuleDefIdByUrl(moduleUrl);

            ModuleType mt = Utilities.ModuleValidate.ValidateModuleType(moduleUrl);
			if (moduledef > 0)
				mt = ModuleType.Local;

			if (mt == ModuleType.Remote || mt == ModuleType.Error)
				return;

            ModuleInfo moduleinfo = new ModuleInfo();
			moduleinfo.ModuleID = Spaces.GetNewModuleId(userId);
			moduleinfo.DisplayOrder = 0;
			moduleinfo.ModuleDefID = moduledef;
			moduleinfo.ModuleType = mt;
			moduleinfo.ModuleUrl = moduleUrl;
			moduleinfo.PaneName = "pane" + col;
			moduleinfo.TabID = tabId;
			moduleinfo.Uid = userId;
			moduleinfo.Val = 6;

			Spaces.AddModule(moduleinfo);
		}

        /// <summary>
        /// 获取新建模块的Id
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
		public static int GetNewModuleId(int userid)
		{
			return 1 + Space.Data.DbProvider.GetInstance().GetMaxModuleIdByUid(userid);
		}

        /// <summary>
        /// 获取新建Tab的Id
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
		public static int GetNewTabId(int userid)
		{
			return 1 + Space.Data.DbProvider.GetInstance().GetMaxTabIdByUid(userid);
		}

        /// <summary>
        /// 更新自定义面板的内容
        /// </summary>
        /// <param name="moduleid"></param>
        /// <param name="userid"></param>
        /// <param name="modulecontent"></param>
        public static void UpdateCustomizePanelContent(int moduleid, int userid, string modulecontent)
        {
            Space.Data.DbProvider.GetInstance().UpdateCustomizePanelContent(moduleid, userid, modulecontent);
        }

        /// <summary>
        /// 检查自定义面板是否存在内容
        /// </summary>
        /// <param name="moduleid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static bool ExistCustomizePanelContent(int moduleid, int userid)
        {
            return Space.Data.DbProvider.GetInstance().ExistCustomizePanelContent(moduleid, userid);
        }

        /// <summary>
        /// 添加自定义面板内容
        /// </summary>
        /// <param name="moduleid"></param>
        /// <param name="userid"></param>
        /// <param name="modulecontent"></param>
        public static void AddCustomizePanelContent(int moduleid, int userid, string modulecontent)
        {
            Space.Data.DbProvider.GetInstance().AddCustomizePanelContent(moduleid, userid, modulecontent);
        }

        /// <summary>
        /// 获取自定义面板内容
        /// </summary>
        /// <param name="moduleid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static string GetCustomizePanelContent(int moduleid, int userid)
        {
            object val = Space.Data.DbProvider.GetInstance().GetCustomizePanelContent(moduleid, userid);

            return val == null ? string.Empty : val.ToString();
        }

        /// <summary>
        /// 删除自定义面板内容
        /// </summary>
        /// <param name="moduleid"></param>
        /// <param name="userid"></param>
        public static void DeleteCustomizePanelContent(int moduleid, int userid)
        {
            Space.Data.DbProvider.GetInstance().DeleteCustomizePanelContent(moduleid, userid);
        }

        /// <summary>
        /// 根据用户获取模块集合
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static List<ModuleInfo> GetModuleCollectionByUserId(int uid)
        {
            return SpaceProvider.GetModuleCollectionByUserId(uid);
        }

        /// <summary>
        /// 删除模块定义
        /// </summary>
        /// <param name="url"></param>
        public static void DeleteModuleDefByUrl(string url)
        {
            SpaceProvider.DeleteModuleDefByUrl(url);
        }

        public static int GetSpacePostCountWithSameTag(int tagid)
        {
            return SpaceProvider.GetSpacePostCountWithSameTag(tagid);
        }

        public static List<SpacePostInfo> GetSpacePostsWithSameTag(int tagid, int pageid, int pagesize)
        {
            IDataReader reader = Space.Data.DbProvider.GetInstance().GetSpacePostsWithSameTag(tagid, pageid, pagesize);
            List<SpacePostInfo> postlist = new List<SpacePostInfo>();
            while (reader.Read())
            {
                postlist.Add(GetSpacePostEntity(reader));                
            }
            reader.Close();

            return postlist;
        }

        private static SpacePostInfo GetSpacePostEntity(IDataReader reader)
        {
            SpacePostInfo post = new SpacePostInfo();
            post.Postid = Utils.StrToInt(reader["postid"], 0);
            post.Title = reader["title"].ToString();
            post.Author = reader["author"].ToString();
            post.Postdatetime = Convert.ToDateTime(reader["postdatetime"]);
            post.Commentcount = Utils.StrToInt(reader["commentcount"], 0);
            post.Views = Utils.StrToInt(reader["views"], 0);
            return post;
        }


        /// <summary>
        /// 删除空间日志
        /// </summary>
        /// <param name="postid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static bool DeleteSpacePost(string postid, int userid)
        {
            bool success = Space.Data.DbProvider.GetInstance().DeleteSpacePosts(postid, userid);
            if (!success)
                return success;

            foreach (string pid in postid.Split(','))
            {
                int spacepostid = Utils.StrToInt(pid, 0);
                if (spacepostid > 0)
                {
                    SpaceTags.DeleteSpacePostTags(spacepostid);
                }
            }
            return true;

        }

        /// <summary>
        /// 根据类别id获得类别名称
        /// </summary>
        /// <param name="categoryids">类别ids字符串</param>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        public static string[] GetCategories(string categoryids, int userid)
        {
            List<string> list = new List<string>();
            string[] cateids = categoryids.Split(','); 
            DataTable categorylist = Data.DbProvider.GetInstance().GetSpaceCategoryListByUserId(userid);
            foreach (DataRow dr in categorylist.Rows)
            {
                foreach (string id in cateids)
                {
                    if (id == dr["categoryid"].ToString())
                    {
                        list.Add(dr["title"].ToString());
                    }
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 根据分类数组得到分类id字符串
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static string GetCategoryIds(string[] categories, int userid)
        {
            List<string> list = new List<string>();
            DataTable categorylist = Data.DbProvider.GetInstance().GetSpaceCategoryListByUserId(userid);
            foreach (DataRow dr in categorylist.Rows)
            {
                foreach (string cate in categories)
                {
                    if (cate == dr["title"].ToString())
                    {
                        list.Add(dr["categoryid"].ToString());
                    }
                }
            }

            string[] cateidArray = list.ToArray();
            return string.Join(",", cateidArray);
        }

        /// <summary>
        /// 获取指定日志的分类
        /// </summary>
        /// <param name="spacepostids"></param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> GetSpacePostCategorys(string spacepostids)
        {
            if (!Utils.IsNumericArray(spacepostids.Split(',')))
            {
                return new Dictionary<string, Dictionary<string, string>>();
            }
            Dictionary<string, Dictionary<string, string>> categorys = new Dictionary<string, Dictionary<string, string>>();

            IDataReader reader = Space.Data.DbProvider.GetInstance().GetSpacePostCategorys(spacepostids);

            while (reader.Read())
            {
                string postid = reader["postid"].ToString();
                string categoryid = reader["categoryid"].ToString();
                string title = reader["title"].ToString();

                if (!categorys.ContainsKey(postid))
                {
                    categorys.Add(postid, new Dictionary<string,string>());
                    
                }
                categorys[postid][categoryid] = title;
            }
            reader.Close();

            return categorys;
        }

        public static void DeleteSpace(int userid)
        {
            DbProvider.GetInstance().DeleteSpaceComments(userid);
            DbProvider.GetInstance().DeleteSpacePosts(userid);
            DbProvider.GetInstance().DeleteSpaceByUid(userid);
        }
    }
}
