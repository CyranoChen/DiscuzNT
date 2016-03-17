using System;
using System.Data;
using System.Text;

using Discuz.Entity;
using Discuz.Data;
using Discuz.Common;
using Discuz.Config;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    public class Navs
    {
        /// <summary>
        /// 主菜单变量
        /// </summary>
        private static DataTable mainNavigation;
        /// <summary>
        /// 全部子菜单变量
        /// </summary>
        private static DataTable subNavigation;
        /// <summary>
        /// 拥有子菜单的主菜单ID数组
        /// </summary>
        private static string[] mainNavigationHasSub;
        /// <summary>
        /// 加载导航数据列表
        /// </summary>
        private static List<NavInfo> navigationList = Discuz.Data.Navs.GetNavigation();

        private static Predicate<NavInfo> matchParent = new Predicate<NavInfo>(delegate(NavInfo navInfo) { return navInfo.Parentid == 0; });

        static Navs()
        {
            InitNavigation();
        }

        /// <summary>
        /// 得到自定义菜单信息
        /// </summary>
        /// <returns></returns>
        private static List<NavInfo> GetNavigation()
        {
            if (navigationList == null)
            {
                navigationList = Discuz.Data.Navs.GetNavigation();
            }
            return navigationList;
        }

        /// <summary>
        /// 返回主菜单缓存
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMainNavigation()
        {
            if (mainNavigation == null)
            {
                mainNavigation = new DataTable();
                mainNavigation.Columns.Add("id", System.Type.GetType("System.Int32"));
                mainNavigation.Columns.Add("nav", System.Type.GetType("System.String"));
                mainNavigation.Columns.Add("level", System.Type.GetType("System.Int32"));
                mainNavigation.Columns.Add("url", System.Type.GetType("System.String"));
                mainNavigation.Columns.Add("name", System.Type.GetType("System.String"));

                foreach (NavInfo m in Discuz.Data.Navs.GetNavigation().FindAll(matchParent))
                {
                    if (m.Parentid == 0)
                    {
                        DataRow navmaindr = mainNavigation.NewRow();
                        StringBuilder str = new StringBuilder();
                        str.AppendFormat("<li><a id=\"menu_{0}\" onMouseOver=\"showMenu({{\'ctrlid\':this.id}});\" href=\"", m.Id);

                        if (!m.Url.StartsWith("http://") && !m.Url.StartsWith("/"))
                            str.Append(BaseConfigs.GetForumPath);

                        str.Append(m.Url.Trim() + "\"");
                        if (m.Target != 0)
                            str.Append(" target=\"_blank\"");

                        str.Append(">");

                        if (Utils.InArray(m.Id.ToString(), GetMainNavigationHasSub()))
                            navmaindr["nav"] = str.Append("<span class=\"drop\">" + m.Name.Trim() + "</span></a></li>").ToString();
                        else
                            navmaindr["nav"] = str.Append(m.Name.Trim() + "</a></li>").ToString();

                        navmaindr["id"] = m.Id;
                        navmaindr["url"] = m.Url.Trim();
                        navmaindr["level"] = m.Level;
                        navmaindr["name"] = m.Name;
                        mainNavigation.Rows.Add(navmaindr);
                    }
                }
            }
            return mainNavigation;
        }

        /// <summary>
        /// 返回子菜单缓存
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSubNavigation()
        {
            if (subNavigation == null)
            {
                subNavigation = new DataTable();
                subNavigation.Columns.Add("id", System.Type.GetType("System.Int32"));
                subNavigation.Columns.Add("nav", System.Type.GetType("System.String"));
                subNavigation.Columns.Add("level", System.Type.GetType("System.Int32"));
                subNavigation.Columns.Add("parentid", System.Type.GetType("System.Int32"));
                List<NavInfo> menu = Discuz.Data.Navs.GetNavigation();

                foreach (NavInfo m in menu.FindAll(matchParent))
                {
                    foreach (NavInfo info in menu)
                    {
                        if (info.Parentid == m.Id)
                        {
                            StringBuilder strs = new StringBuilder();
                            DataRow navssubdr = subNavigation.NewRow();
                            strs.Append("<li><a href=\"");
                            if (!info.Url.StartsWith("http://") && !info.Url.StartsWith("/"))
                                strs.Append(BaseConfigs.GetForumPath);

                            strs.Append(info.Url.Trim());
                            if (info.Target != 0)
                                strs.Append("\" target=\"_blank");
                            strs.AppendFormat("\">{0}</a></li>", info.Name.Trim());

                            navssubdr["nav"] = strs.ToString();
                            navssubdr["id"] = info.Id;
                            navssubdr["parentid"] = info.Parentid;
                            navssubdr["level"] = info.Level;
                            subNavigation.Rows.Add(navssubdr);
                        }
                    }
                }
            }
            return subNavigation;
        }

        /// <summary>
        /// 返回拥有子菜单的主菜单ID数组
        /// </summary>
        /// <returns></returns>
        public static string[] GetMainNavigationHasSub()
        {
            if (mainNavigationHasSub == null)
            {
                string menustr = "";

                foreach (NavInfo i in Discuz.Data.Navs.GetNavigationHasSub())
                {
                    menustr += i.Parentid + ",";
                    menustr.Remove(menustr.Length - 1, 1);
                }
                mainNavigationHasSub = menustr.Split(',');
            }
            return mainNavigationHasSub;
        }


        /// <summary>
        /// 初始化导航菜单
        /// </summary>
        public static void InitNavigation()
        {
            GetMainNavigation();
            GetSubNavigation();
            GetMainNavigationHasSub();
        }

        /// <summary>
        /// 清空导航数据
        /// </summary>
        public static void ClearNavigation()
        {
            mainNavigation = null;
            subNavigation = null;
            mainNavigationHasSub = null;
            navigationList = null;
        }

        /// <summary>
        /// 将主菜单转化为字符串
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="useradminid">用户管理组ID</param>
        /// <returns></returns>
        public static string GetNavigationString(int userid, int useradminid)
        {
            List<string> mainNavList = new List<string>();
            string nav;
            DataTable dt = GetMainNavigation();
            if (dt != null)
            {
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    nav = ChangeStyleForCurrentUrl(dr);
                    //使用等级  0 游客 1 会员  2 版主  3 管理员
                    switch (Utils.StrToInt(dr["level"].ToString(), 4))
                    {
                        case 0:
                            mainNavList.Add(nav);
                            break;
                        case 1:
                            {
                                if (userid != -1)
                                    mainNavList.Add(nav);
                                break;
                            }
                        case 2:
                            {
                                if (useradminid == 3 || useradminid == 1 || useradminid == 2)
                                    mainNavList.Add(nav);
                                break;
                            }
                        case 3:
                            {
                                if (useradminid == 1)
                                    mainNavList.Add(nav);
                                break;
                            }
                    }
                }
            }

            return string.Join(string.Empty, mainNavList.ToArray());
        }

        /// <summary>
        /// 当前页面与导航栏匹配时改变样式
        /// </summary>
        /// <param name="nav">菜单名称</param>
        /// <param name="url">链接地址</param>
        /// <returns></returns>
        private static string ChangeStyleForCurrentUrl(DataRow dr)
        {
            string nav = dr["nav"].ToString();
            string url = dr["url"].ToString().Trim();

            //如果url的值为/或者index.aspx，证明这个页面可能会是forumindex或者website.aspx
            if (Utils.InArray(url, "/,index.aspx") && DNTRequest.GetPageName() == (GeneralConfigs.GetConfig().Indexpage == 0 ? "forumindex.aspx" : "website.aspx"))
                return ReplaceCurrentCssClass(nav);

            if (!Utils.StrIsNullOrEmpty(url) && System.Web.HttpContext.Current.Request.RawUrl.ToString().Contains(url))
                return ReplaceCurrentCssClass(nav);

            return nav;
        }

        /// <summary>
        /// 替换当前菜单项样式
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="nav"></param>
        /// <returns></returns>
        private static string ReplaceCurrentCssClass(string nav)
        {
            return nav.Replace("<a", "<a class=\"current\"");
        }

        /// <summary>
        /// 删除导航
        /// </summary>
        /// <param name="id">菜单ID</param>
        public static void DeleteNavigation(int id)
        {
            Data.Navs.DeleteNavigation(id);
            ClearNavigation();
            InitNavigation();
        }

        /// <summary>
        /// 添加导航菜单
        /// </summary>
        /// <param name="nav"></param>
        public static void InsertNavigation(NavInfo nav)
        {
            Data.Navs.InsertNavigation(nav);
            ClearNavigation();
            InitNavigation();
        }

        /// <summary>
        /// 更新导航菜单
        /// </summary>
        /// <param name="nav">导航菜单类</param>
        public static void UpdateNavigation(NavInfo nav)
        {
            Data.Navs.UpdateNavigation(nav);
            ClearNavigation();
            InitNavigation();
        }

        public static DataTable GetNavigation(bool getAll)
        {
            DataTable navmenu = new DataTable();
            navmenu.Columns.Add("id", System.Type.GetType("System.Int32"));
            navmenu.Columns.Add("parentid", System.Type.GetType("System.Int32"));
            navmenu.Columns.Add("name", System.Type.GetType("System.String"));
            navmenu.Columns.Add("title", System.Type.GetType("System.String"));
            navmenu.Columns.Add("url", System.Type.GetType("System.String"));
            navmenu.Columns.Add("target", System.Type.GetType("System.Int16"));
            navmenu.Columns.Add("type", System.Type.GetType("System.Int16"));
            navmenu.Columns.Add("available", System.Type.GetType("System.Int16"));
            navmenu.Columns.Add("displayorder", System.Type.GetType("System.Int32"));
            navmenu.Columns.Add("highlight", System.Type.GetType("System.Int16"));
            navmenu.Columns.Add("level", System.Type.GetType("System.Int32"));
            IDataReader reader = Discuz.Data.Navs.GetNavigationData(true);
            while (reader.Read())
            {
                DataRow dr = navmenu.NewRow();
                dr["id"] = Utils.StrToInt(reader["id"], 0);
                dr["parentid"] = Utils.StrToInt(reader["parentid"], 0);
                dr["name"] = reader["name"].ToString().Trim();
                dr["title"] = reader["title"].ToString().Trim();
                dr["url"] = reader["url"].ToString().Trim();
                dr["target"] = Utils.StrToInt(reader["target"], 0);
                dr["type"] = Utils.StrToInt(reader["type"], 0);
                dr["available"] = Utils.StrToInt(reader["available"], 0);
                dr["displayorder"] = Utils.StrToInt(reader["displayorder"], 0);
                dr["highlight"] = Utils.StrToInt(reader["highlight"], 0);
                dr["level"] = Utils.StrToInt(reader["level"], 0);
                navmenu.Rows.Add(dr);
            }
            reader.Close();
            return navmenu;
        }

        /// <summary>
        /// 获取菜单项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static NavInfo GetNavigation(int id)
        {
            foreach (NavInfo nav in Discuz.Data.Navs.GetNavigation(true))
            {
                if (nav.Id == id)
                    return nav;
            }
            return null;
        }
    }
}
