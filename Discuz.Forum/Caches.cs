using System;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Cache;
using Discuz.Config.Provider;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    /// <summary>
    /// 缓存论坛前台的一些界面HTML数据
    /// </summary>
    public class Caches
    {
        private static object lockHelper = new object();

        /// <summary>
        /// 获得版块下拉列表
        /// </summary>
        /// <param name="cateUnselectable">是否不可选择版块分类</param>
        /// <returns>列表内容的html</returns>
        public static string GetForumListBoxOptionsCache(bool cateUnselectable)
        {
            DNTCache cache = DNTCache.GetCacheService();
            string str = cache.RetrieveObject("/Forum/UI/ForumListBoxOptions") as string;
            if (Utils.StrIsNullOrEmpty(str))
            {
                StringBuilder sb = new StringBuilder();
                AddOptionTree(Data.Forums.GetVisibleForumList(), "0", cateUnselectable, sb);
                str = sb.ToString();
                cache.AddObject("/Forum/UI/ForumListBoxOptions", str);
            }
            return str;
        }

        /// <summary>
        /// 创建Select版块列表
        /// </summary>
        /// <param name="dt">版块列表</param>
        /// <param name="parentid">parentid</param>
        /// <param name="cateUnselectable">是否不可选择版块分类</param>
        /// <param name="sb">生成结果</param>
        private static void AddOptionTree(DataTable dt, string parentid, bool cateUnselectable, StringBuilder sb)
        {
            DataRow[] drs = dt.Select("parentid=" + parentid);
            foreach (DataRow dr in drs)
            {
                if (cateUnselectable && dr["layer"].ToString().Trim() == "0")
                {
                    sb.AppendFormat("<optgroup label=\"--{0}\">", dr["name"].ToString().Trim());
                    sb.Append(Utils.GetSpacesString(TypeConverter.ObjectToInt(dr["layer"])));
                    sb.Append(dr["name"].ToString().Trim());
                }
                else
                {
                    sb.AppendFormat("<option value=\"{0}\">", dr["fid"]);
                    sb.Append(Utils.GetSpacesString(TypeConverter.ObjectToInt(dr["layer"])));
                    sb.Append(dr["name"].ToString().Trim());
                    sb.Append("</option>");
                }
                AddOptionTree(dt, dr["fid"].ToString().Trim(), cateUnselectable, sb);
                if (cateUnselectable && dr["layer"].ToString().Trim() == "0")
                    sb.Append("</optgroup>");
            }
        }

        /// <summary>
        /// 获得版块下拉列表
        /// </summary>
        /// <returns></returns>
        public static string GetForumListBoxOptionsCache()
        {
            return GetForumListBoxOptionsCache(false);
        }

        /// <summary>
        /// 前台版块列表弹出菜单
        /// </summary>
        /// <param name="usergroupid">用户组id</param>
        /// <param name="userid">当前用户id</param>
        /// <param name="extname">扩展名称</param>
        /// <returns>版块列表弹出菜单</returns>
        public static string GetForumListMenuDivCache(int usergroupid, int userid, string extname)
        {
            DNTCache cache = DNTCache.GetCacheService();
            string str = cache.RetrieveObject("/Forum/ForumListMenuDiv") as string;
            if (Utils.StrIsNullOrEmpty(str))
            {
                StringBuilder sb = new StringBuilder();
                List<ForumInfo> forumList = Forums.GetForumList();

                if (forumList.Count > 0)
                {
                    sb.Append("<div class=\"popupmenu_popup\" id=\"forumlist_menu\" style=\"overflow-y: auto; display:none\">");

                    foreach (ForumInfo info in forumList)
                    {
                        if (info.Layer >= 0 && info.Layer <= 1 && info.Status == 1)
                        {
                            //判断是否为私密论坛
                            //if (info.Viewperm != "" && !Utils.InArray(usergroupid.ToString(), info.Viewperm))
                            //如果对当前执行该程序的用户组权限进行判断，则会将整站的导航下拉缓存都以当前用户组权限去设置，
                            //这样会出现用户有时能看到自己并没有权限访问的论坛板块
                            if (info.Viewperm.Trim() == string.Empty || Utils.InArray("7", info.Viewperm))
                            {
                                if (info.Layer == 0)
                                {
                                    sb.AppendFormat("<dl><dt><a href=\"{0}\">{1}</a></dt><dd><ul>",
                                                     BaseConfigs.GetForumPath + Urls.ShowForumAspxRewrite(info.Fid, 0, info.Rewritename),
                                                     info.Name);

                                    foreach (ForumInfo forum in forumList)
                                    {
                                        if (Utils.StrToInt(forum.Parentidlist.Split(',')[0], 0) == info.Fid && forum.Layer == 1 && forum.Status == 1)
                                        {
                                            sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>",
                                                             BaseConfigs.GetForumPath + Urls.ShowForumAspxRewrite(forum.Fid, 0, forum.Rewritename),
                                                             forum.Name.Trim());
                                        }
                                    }
                                    sb.Append("</ul></dd></dl>");
                                }
                            }
                        }
                    }
                }
                sb.Append("</div>");
                str = sb.ToString().Replace("<dd><ul></ul></dd>", "");
                cache.AddObject("/Forum/ForumListMenuDiv", str);
            }
            return str;
        }


        /// <summary>
        /// 返回模板列表的下拉框html
        ///</summary>
        ///<param name="topMenu">是否是首页顶部菜单操作</param>
        /// <returns>下拉框html</returns>
        public static string GetTemplateListBoxOptionsCache(bool topMenu)
        {
            lock (lockHelper)
            {
                DNTCache cache = DNTCache.GetCacheService();
                string str = topMenu ? cache.RetrieveObject("/Forum/UI/TemplateListBoxOptionsForForumIndex") as string :
                    cache.RetrieveObject("/Forum/UI/TemplateListBoxOptions") as string;
                if (Utils.StrIsNullOrEmpty(str))
                {
                    StringBuilder sb = new StringBuilder();
                    DataTable dt = Templates.GetValidTemplateList();

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (topMenu)
                        {
                            sb.AppendFormat("<li><a onclick=\"window.location.href='{0}showtemplate.aspx?templateid={1}';return false;\" href=\"###\"><i style=\"background: url(&quot;templates/{2}/about.png&quot;) no-repeat scroll 0% 0% transparent;\">&nbsp;</i><span>{3}</span><em></em></a></li>",
                                BaseConfigs.GetForumPath,
                                dr["templateid"],
                                dr["directory"],
                                dr["name"].ToString().Trim());
                        }
                        else
                        {
                            sb.AppendFormat("<li><a onclick=\"window.location.href='{0}showtemplate.aspx?templateid={1}';return false;\" href=\"###\">{2}</a></li>",
                               BaseConfigs.GetForumPath,
                               dr["templateid"],
                               dr["name"].ToString().Trim());
                        }
                    }
                    str = sb.ToString();
                    cache.AddObject(topMenu ? "/Forum/UI/TemplateListBoxOptionsForForumIndex" : "/Forum/UI/TemplateListBoxOptions", str);
                    dt.Dispose();
                }
                return str;
            }
        }

        public static string GetTemplateListBoxOptionsCache()
        {
            return GetTemplateListBoxOptionsCache(false);
        }

        /// <summary>
        /// 获得表情符的json数据
        /// </summary>
        /// <returns>表情符的json数据</returns>
        public static string GetSmiliesCache()
        {
            DNTCache cache = DNTCache.GetCacheService();
            string str = cache.RetrieveObject("/Forum/UI/SmiliesList") as string;
            if (Utils.StrIsNullOrEmpty(str))
            {
                StringBuilder builder = new StringBuilder();
                DataTable dt = Discuz.Data.Smilies.GetSmiliesListDataTable();

                foreach (DataRow drCate in dt.Copy().Rows)
                {
                    if (drCate["type"].ToString() == "0")
                    {
                        builder.AppendFormat("'{0}': [\r\n", drCate["code"].ToString().Trim().Replace("'", "\\'"));
                        bool flag = false;
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["type"].ToString() == drCate["id"].ToString())
                            {
                                builder.Append("{'code' : '");
                                builder.Append(dr["code"].ToString().Trim().Replace("'", "\\'"));
                                builder.Append("', 'url' : '");
                                builder.Append(dr["url"].ToString().Trim().Replace("'", "\\'"));
                                builder.Append("'},\r\n");
                                flag = true;
                            }
                        }
                        if (builder.Length > 0 && flag)
                            builder.Remove(builder.Length - 3, 3);
                        builder.Append("\r\n],\r\n");
                    }
                }
                builder.Remove(builder.Length - 3, 3);
                str = builder.ToString();
                cache.AddObject("/Forum/UI/SmiliesList", str);
            }
            return str;
        }

        /// <summary>
        /// 获取第一页的表情
        /// </summary>
        /// <returns>获取第一页的表情</returns>
        public static string GetSmiliesFirstPageCache()
        {
            DNTCache cache = DNTCache.GetCacheService();
            string str = cache.RetrieveObject("/Forum/UI/SmiliesListFirstPage") as string;
            if (Utils.StrIsNullOrEmpty(str))
            {
                StringBuilder builder = new StringBuilder();
                DataTable dt = Discuz.Data.Smilies.GetSmiliesListDataTable();
                foreach (DataRow drCate in dt.Copy().Rows)
                {
                    if (drCate["type"].ToString() == "0")
                    {
                        builder.AppendFormat("'{0}': [\r\n", drCate["code"].ToString().Trim().Replace("'", "\\'"));
                        bool flag = false;
                        int smiliescount = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["type"].ToString() == drCate["id"].ToString() && smiliescount < 16)
                            {
                                builder.Append("{'code' : '");
                                builder.Append(dr["code"].ToString().Trim().Replace("'", "\\'"));
                                builder.Append("', 'url' : '");
                                builder.Append(dr["url"].ToString().Trim().Replace("'", "\\'"));
                                builder.Append("'},\r\n");
                                flag = true;
                                smiliescount++;
                            }
                        }
                        if (builder.Length > 0 && flag)
                            builder.Remove(builder.Length - 3, 3);

                        builder.Append("\r\n],\r\n");
                        break;
                    }
                }
                builder.Remove(builder.Length - 3, 3);
                str = builder.ToString();
                cache.AddObject("/Forum/UI/SmiliesListFirstPage", str);
            }
            return str;
        }


        /// <summary>
        /// 获得表情分类列表
        /// </summary>
        /// <returns>表情分类列表</returns>
        public static DataTable GetSmilieTypesCache()
        {
            DNTCache cache = DNTCache.GetCacheService();
            DataTable smilietypes = cache.RetrieveObject("/Forum/UI/SmiliesTypeList") as DataTable;
            if (smilietypes == null || smilietypes.Rows.Count == 0)
            {
                smilietypes = Discuz.Data.Smilies.GetSmiliesTypes();
                cache.AddObject("/Forum/UI/SmiliesTypeList", smilietypes);
            }
            return smilietypes;
        }

        /// <summary>
        /// 获得编辑器自定义按钮信息的javascript数组
        /// </summary>
        /// <returns>表情符的javascript数组</returns>
        public static string GetCustomEditButtonList()
        {
            lock (lockHelper)
            {
                DNTCache cache = DNTCache.GetCacheService();
                string str = cache.RetrieveObject("/Forum/UI/CustomEditButtonList") as string;
                if (str == null)//此处这样判断是为了防止数据库中无记录时会将str赋值成""的情况，参见下面加载数据代码
                {
                    StringBuilder sb = new StringBuilder();
                    IDataReader dr = DatabaseProvider.GetInstance().GetCustomEditButtonList();
                    try
                    {
                        while (dr.Read())
                        {
                            //说明:[标签名,对应图标文件名,[参数1描述,参数2描述,...],[参数1默认值,参数2默认值,...]]
                            //实例["fly","swf.gif",["请输入flash网址","请输入flash宽度","请输入flash高度"],["http://","200","200"],3]
                            sb.AppendFormat(",'{0}':['", Utils.ReplaceStrToScript(dr["tag"].ToString()));
                            sb.Append(Utils.ReplaceStrToScript(dr["tag"].ToString()));
                            sb.Append("','");
                            sb.Append(Utils.ReplaceStrToScript(dr["icon"].ToString()));
                            sb.Append("','");
                            sb.Append(Utils.ReplaceStrToScript(dr["explanation"].ToString()));
                            sb.Append("',['");
                            sb.Append(Utils.ReplaceStrToScript(dr["paramsdescript"].ToString()).Replace(",", "','"));
                            sb.Append("'],['");
                            sb.Append(Utils.ReplaceStrToScript(dr["paramsdefvalue"].ToString()).Replace(",", "','"));
                            sb.Append("'],");
                            sb.Append(Utils.ReplaceStrToScript(dr["params"].ToString()));
                            sb.Append("]");
                        }
                        if (sb.Length > 0)
                            sb.Remove(0, 1);

                        str = Utils.ClearBR(sb.ToString());
                        cache.AddObject("/Forum/UI/CustomEditButtonList", str);
                    }
                    finally
                    {
                        dr.Close();
                    }
                }
                return str;
            }
        }

        /// <summary>
        /// 获得在线用户列表图例
        /// </summary>
        /// <returns>在线用户列表图例</returns>
        public static string GetOnlineGroupIconList()
        {
            DNTCache cache = DNTCache.GetCacheService();
            string str = cache.RetrieveObject("/Forum/UI/OnlineIconList") as string;
            if (Utils.StrIsNullOrEmpty(str))
            {
                StringBuilder sb = new StringBuilder();
                IDataReader dr = DatabaseProvider.GetInstance().GetOnlineGroupIconList();
                string forumpath = BaseConfigs.GetForumPath;
                try
                {
                    while (dr.Read())
                    {
                        sb.AppendFormat("<img src=\"{0}images/groupicons/{1}\" /> {2} &nbsp; &nbsp; &nbsp; ", forumpath, dr["img"], dr["title"]);
                    }
                    str = sb.ToString();
                    cache.AddObject("/Forum/UI/OnlineIconList", str);
                }
                finally
                {
                    dr.Close();
                }
            }
            return str;
        }

        /// <summary>
        /// 获得友情链接列表
        /// </summary>
        /// <returns>友情链接列表</returns>
        public static DataTable GetForumLinkList()
        {
            DNTCache cache = DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject("/Forum/ForumLinkList") as DataTable;
            if (dt == null)
            {
                dt = DatabaseProvider.GetInstance().GetForumLinkList();
                if (dt != null && dt.Rows.Count > 0)
                {
                    StringBuilder linkBuilder = new StringBuilder();
                    StringBuilder logoLinkBuilder = new StringBuilder();
                    StringBuilder textLinkBuilder = new StringBuilder();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Utils.StrIsNullOrEmpty(dr["note"].ToString()))
                        {
                            if (Utils.StrIsNullOrEmpty(dr["name"].ToString()))
                                dr["name"] = "未知";

                            if (Utils.StrIsNullOrEmpty(dr["logo"].ToString()))
                                textLinkBuilder.AppendFormat("<li><a title=\"{0}\" href=\"{1}\" target=\"_blank\">{0}</a></li>\r\n", dr["name"], dr["url"]);
                            else
                            {
                                logoLinkBuilder.AppendFormat("<li><a title=\"{0}\" href=\"{1}\" target=\"_blank\"><img alt=\"{0}\" class=\"friendlinkimg\" src=\"{2}\" /></a></li>\r\n",
                                                             dr["name"], dr["url"], dr["logo"]);
                            }
                            dr.Delete();
                        }
                    }
                    if (logoLinkBuilder.Length > 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr["name"] = "$$otherlink$$";
                        dr["url"] = "forumimglink";
                        dr["note"] = logoLinkBuilder.ToString();
                        dr["logo"] = "";
                        dt.Rows.Add(dr);
                    }
                    if (textLinkBuilder.Length > 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr["name"] = "$$otherlink$$";
                        dr["url"] = "forumtxtlink";
                        dr["note"] = textLinkBuilder.ToString();
                        dr["logo"] = "";
                        dt.Rows.Add(dr);
                    }
                    dt.AcceptChanges();
                }
                cache.AddObject("/Forum/ForumLinkList", dt);
            }
            return dt;
        }

        /// <summary>
        /// 数字正则式静态实例
        /// </summary>
        private static Regex r = new Regex("\\{(\\d+)\\}", Utils.GetRegexCompiledOptions());

        /// <summary>
        /// 返回脏字过滤列表
        /// </summary>
        /// <returns>返回脏字过滤列表数组</returns>
        public static string[,] GetBanWordList()
        {
            DNTCache cache = DNTCache.GetCacheService();
            string[,] str = cache.RetrieveObject("/Forum/BanWordList") as string[,];
            if (str == null)
            {
                DataTable dt = DatabaseProvider.GetInstance().GetBanWordList();
                str = new string[dt.Rows.Count, 2];
                string temp = "";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    temp = dt.Rows[i]["find"].ToString().Trim();
                    foreach (Match m in r.Matches(temp))
                    {
                        temp = temp.Replace(m.Groups[0].ToString(), m.Groups[0].ToString().Replace("{", ".{0,"));
                    }
                    str[i, 0] = BanWords.ConvertRegexCode(temp);
                    str[i, 1] = dt.Rows[i]["replacement"].ToString().Trim();
                }
                cache.AddObject("/Forum/BanWordList", str);
                dt.Dispose();
            }
            return str;
        }

        /// <summary>
        /// 获取自带头像列表
        /// </summary>
        /// <returns>自带头像列表</returns>
        public static DataTable GetAvatarList()
        {
            DNTCache cache = DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject("/Forum/CommonAvatarList") as DataTable;
            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("filename", Type.GetType("System.String"));

                DirectoryInfo dirinfo = new DirectoryInfo(Utils.GetMapPath(BaseConfigs.GetForumPath + "avatars/common/"));
                string extname = "";
                foreach (FileSystemInfo file in dirinfo.GetFileSystemInfos())
                {
                    if (file != null)
                    {
                        extname = file.Extension.ToLower();
                        if (extname.Equals(".jpg") || extname.Equals(".gif") || extname.Equals(".png"))
                        {
                            DataRow dr = dt.NewRow();
                            dr["filename"] = @"avatars/common/" + file.Name;
                            dt.Rows.Add(dr);
                        }
                    }
                }
                cache.AddObject("/Forum/CommonAvatarList", dt);
            }
            return dt;
        }


        /// <summary>
        /// 获得干扰码字符串
        /// </summary>
        /// <returns>干扰码字符串</returns>
        public static string GetJammer()
        {
            ///干扰码组成(10 位随机字符　+ 网站域名 + 10位随机字符)
            DNTCache cache = DNTCache.GetCacheService();
            string str = cache.RetrieveObject("/Forum/UI/Jammer") as string;

            if (str == null)
            {
                Random rdm1 = new Random(unchecked((int)DateTime.Now.Ticks));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 10; i++)
                {
                    sb.Append(Convert.ToChar(rdm1.Next(1, 256)));
                }
                sb.Append(System.Web.HttpContext.Current.Request.Url.Authority);
                for (int i = 0; i < 10; i++)
                {
                    sb.Append(Convert.ToChar(rdm1.Next(1, 256)));
                }
                str = sb.ToString();
                str = Utils.HtmlEncode(str);

                if (sb.Length > 0)
                    sb.Remove(0, sb.Length);

                str = sb.AppendFormat("<span class=\"jammer\">{0}</span>", str).ToString();
                cache.AddObject("/Forum/UI/Jammer", str, 720);
            }
            return str;
        }


        /// <summary>
        /// 获得勋章列表
        /// </summary>
        /// <returns>获得勋章列表</returns>
        public static DataTable GetMedalsList()
        {
            DNTCache cache = DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject("/Forum/UI/MedalsList") as DataTable;
            if (dt == null)
            {
                dt = DatabaseProvider.GetInstance().GetMedalsList();
                string forumpath = BaseConfigs.GetBaseConfig().Forumpath;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["available"].ToString() == "1")
                    {
                        if (!Utils.StrIsNullOrEmpty(dr["image"].ToString()))
                        {
                            //当启用远程布署勋章图片时
                            if (EntLibConfigs.GetConfig() != null && !Utils.StrIsNullOrEmpty(EntLibConfigs.GetConfig().Medaldir))
                                dr["image"] = "<img border=\"0\" src=\"" + EntLibConfigs.GetConfig().Medaldir + dr["image"] + "\" alt=\"" + dr["name"] + "\" title=\"" + dr["name"] + "\" />";
                            else
                                dr["image"] = "<img border=\"0\" src=\"" + forumpath + "images/medals/" + dr["image"] + "\" alt=\"" + dr["name"] + "\" title=\"" + dr["name"] + "\" />";
                        }
                        else
                            dr["image"] = "";
                    }
                    else
                        dr["image"] = "";
                }
                cache.AddObject("/Forum/UI/MedalsList", dt);
            }
            return dt;
        }


        /// <summary>
        /// 获取指定id的勋章列表html
        /// </summary>
        /// <param name="mdealList">勋章id</param>
        /// <returns>勋章列表html</returns>
        public static string GetMedalsList(string mdealList)
        {
            DataTable dt = GetMedalsList();
            string[] list = Utils.SplitString(mdealList, ",");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Length; i++)
            {
                sb.Append(dt.Rows[TypeConverter.StrToInt(list[i], 1) - 1]["image"]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取主题鉴定项
        /// </summary>
        /// <param name="identifyid">主题签定id</param>
        /// <returns>主题鉴定信息</returns>
        public static TopicIdentify GetTopicIdentify(int identifyid)
        {
            foreach (TopicIdentify ti in GetTopicIdentifyCollection())
            {
                if (ti.Identifyid == identifyid)
                {
                    return ti;
                }
            }
            return new TopicIdentify();
        }

        /// <summary>
        /// 获取主题鉴定图片地址缓存数组
        /// </summary>
        /// <returns>主题鉴定图片地址缓存数组</returns>
        public static string GetTopicIdentifyFileNameJsArray()
        {
            DNTCache cache = DNTCache.GetCacheService();

            string jsArray = cache.RetrieveObject("/Forum/TopicIndentifysJsArray") as string;

            if (Utils.StrIsNullOrEmpty(jsArray))
            {
                GetTopicIdentifyCollection();
                jsArray = cache.RetrieveObject("/Forum/TopicIndentifysJsArray") as string;
            }

            return jsArray;
        }

        /// <summary>
        /// 获得禁止的ip列表
        /// </summary>
        /// <returns>禁止列表</returns>
        public static List<IpInfo> GetBannedIpList()
        {
            List<IpInfo> list = DNTCache.GetCacheService().RetrieveObject("/Forum/BannedIp") as List<IpInfo>;

            if (list == null)
            {
                list = Ips.GetBannedIpList();
                DNTCache.GetCacheService().AddObject("/Forum/BannedIp", list);
            }
            return list;
        }

        /// <summary>
        /// 获得主题类型数组
        /// </summary>
        /// <returns>主题类型数组</returns>
        public static Discuz.Common.Generic.SortedList<int, string> GetTopicTypeArray()
        {
            DNTCache cache = DNTCache.GetCacheService();
            Discuz.Common.Generic.SortedList<int, string> topictypeList;
            topictypeList = cache.RetrieveObject("/Forum/TopicTypes") as Discuz.Common.Generic.SortedList<int, string>;

            if (topictypeList == null)
            {
                topictypeList = new Discuz.Common.Generic.SortedList<int, string>();
                DataTable dt = DatabaseProvider.GetInstance().GetTopicTypeList();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!Utils.StrIsNullOrEmpty(dr["typeid"].ToString()) && !Utils.StrIsNullOrEmpty(dr["name"].ToString()))
                        {
                            topictypeList.Add(TypeConverter.ObjectToInt(dr["typeid"]), dr["name"].ToString());
                        }
                    }
                }
                cache.AddObject("/Forum/TopicTypes", topictypeList);
            }
            return topictypeList;
        }


        /// <summary>
        /// 获取主题签定集合项
        /// </summary>
        /// <returns>主题签定集合项</returns>
        public static Discuz.Common.Generic.List<TopicIdentify> GetTopicIdentifyCollection()
        {
            DNTCache cache = DNTCache.GetCacheService();
            Discuz.Common.Generic.List<TopicIdentify> topicidentifyList = cache.RetrieveObject("/Forum/TopicIdentifys") as Discuz.Common.Generic.List<TopicIdentify>;
            if (topicidentifyList == null)
            {
                topicidentifyList = new Discuz.Common.Generic.List<TopicIdentify>();
                IDataReader reader = DatabaseProvider.GetInstance().GetTopicsIdentifyItem();
                StringBuilder jsArray = new StringBuilder("<script type='text/javascript'>var topicidentify = { ");

                while (reader.Read())
                {
                    TopicIdentify topic = new TopicIdentify();
                    topic.Identifyid = TypeConverter.ObjectToInt(reader["identifyid"]);
                    topic.Name = reader["name"].ToString();
                    topic.Filename = reader["filename"].ToString();

                    topicidentifyList.Add(topic);
                    jsArray.AppendFormat("'{0}':'{1}',", reader["identifyid"], reader["filename"]);
                }
                reader.Close();
                jsArray.Remove(jsArray.Length - 1, 1);
                jsArray.Append("};</script>");
                cache.AddObject("/Forum/TopicIdentifys", topicidentifyList);
                cache.AddObject("/Forum/TopicIndentifysJsArray", jsArray.ToString());
            }

            return topicidentifyList;
        }

        #region 后台管理重设缓存
        private static void RemoveObject(string key)
        {
            DNTCache.GetCacheService().RemoveObject(key);
        }

        /// <summary>
        /// 重新设置管理组信息
        ///</summary>
        public static void ReSetAdminGroupList()
        {
            RemoveObject(CacheKeys.FORUM_ADMIN_GROUP_LIST);
        }

        /// <summary>
        /// 重新设置用户组信息
        ///</summary>
        public static void ReSetUserGroupList()
        {
            RemoveObject(CacheKeys.FORUM_USER_GROUP_LIST);
        }

        /// <summary>
        /// 重新设置版主信息
        ///</summary>
        public static void ReSetModeratorList()
        {
            RemoveObject(CacheKeys.FORUM_MODERATOR_LIST);
        }

        /// <summary>
        /// 重新设置指定时间内的公告列表
        ///</summary>
        public static void ReSetAnnouncementList()
        {
            RemoveObject(CacheKeys.FORUM_ANNOUNCEMENT_LIST);
        }

        /// <summary>
        /// 重新设置第一条公告
        ///</summary>
        public static void ReSetSimplifiedAnnouncementList()
        {
            RemoveObject(CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST);
        }

        /// <summary>
        /// 重新设置版块下拉列表
        ///</summary>
        public static void ReSetForumListBoxOptions()
        {
            RemoveObject(CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS);
        }

        /// <summary>
        /// 重新设置表情
        ///</summary>
        public static void ReSetSmiliesList()
        {
            RemoveObject(CacheKeys.FORUM_UI_SMILIES_LIST);
            RemoveObject(CacheKeys.FORUM_UI_SMILIES_LIST_WITH_INFO);

        }

        /// <summary>
        /// 重新设置主题图标
        ///</summary>
        public static void ReSetIconsList()
        {
            RemoveObject(CacheKeys.FORUM_UI_ICONS_LIST);
        }

        /// <summary>
        /// 重新用户自定义标签
        ///</summary>
        public static void ReSetCustomEditButtonList()
        {
            RemoveObject(CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_LIST);
            RemoveObject(CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_INFO);
        }

        /// <summary>
        /// 重新设置论坛基本设置
        ///</summary>
        public static void ReSetConfig()
        {
            RemoveObject(CacheKeys.FORUM_SETTING);
        }

        /// <summary>
        /// 重新设置论坛积分
        ///</summary>
        public static void ReSetScoreset()
        {
            RemoveObject(CacheKeys.FORUM_SCORESET);
            RemoveObject(CacheKeys.FORUM_VALID_SCORE_NAME);
            RemoveObject(CacheKeys.FORUM_SCORESET_CREDITS_TAX);
            RemoveObject(CacheKeys.FORUM_SCORESET_CREDITS_TRANS);
            RemoveObject(CacheKeys.FORUM_SCORESET_TRANSFER_MIN_CREDITS);
            RemoveObject(CacheKeys.FORUM_SCORESET_EXCHANGE_MIN_CREDITS);
            RemoveObject(CacheKeys.FORUM_SCORESET_MAX_INC_PER_THREAD);
            RemoveObject(CacheKeys.FORUM_SCORESET_MAX_CHARGE_SPAN);
            RemoveObject(CacheKeys.FORUM_VALID_SCORE_UNIT);
        }

        /// <summary>
        /// 重新设置地址对照表
        ///</summary>
        public static void ReSetSiteUrls()
        {
            RemoveObject(CacheKeys.FORUM_URLS);
        }

        /// <summary>
        /// 重新设置论坛统计信息
        ///</summary>
        public static void ReSetStatistics()
        {
            RemoveObject(CacheKeys.FORUM_STATISTICS);
        }


        /// <summary>
        /// 重新设置系统允许的附件类型和大小
        ///</summary>
        public static void ReSetAttachmentTypeArray()
        {
            RemoveObject(CacheKeys.FORUM_FORUM_SETTING_ATTACHMENT_TYPE);
        }

        /// <summary>
        /// 模板列表的下拉框html
        ///</summary>
        public static void ReSetTemplateListBoxOptionsCache()
        {
            RemoveObject(CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS_FOR_FORUMINDEX);
            RemoveObject(CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS);
        }

        /// <summary>
        /// 重新设置在线用户列表图例
        /// </summary>
        public static void ReSetOnlineGroupIconList()
        {
            RemoveObject(CacheKeys.FORUM_UI_ONLINE_ICON_LIST);
            RemoveObject(CacheKeys.FORUM_ONLINE_ICON_TABLE);

        }

        /// <summary>
        /// 重新设置友情链接列表
        /// </summary>
        public static void ReSetForumLinkList()
        {
            RemoveObject(CacheKeys.FORUM_FORUM_LINK_LIST);
        }


        /// <summary>
        /// 重新设置脏字过滤列表
        /// </summary>
        public static void ReSetBanWordList()
        {
            RemoveObject(CacheKeys.FORUM_BAN_WORD_LIST);
        }


        /// <summary>
        /// 论坛列表
        /// </summary>
        public static void ReSetForumList()
        {
            RemoveObject(CacheKeys.FORUM_FORUM_LIST);
        }


        /// <summary>
        /// 在线用户信息
        /// </summary>
        public static void ReSetOnlineUserTable()
        {
            ;
        }

        /// <summary>
        /// 论坛整体RSS及指定版块RSS
        /// </summary>
        public static void ReSetRss()
        {
            RemoveObject(CacheKeys.FORUM_RSS);
        }


        /// <summary>
        /// 指定版块RSS
        /// </summary>
        /// <param name="fid">版块Id</param>
        public static void ReSetForumRssXml(int fid)
        {
            RemoveObject(string.Format(CacheKeys.FORUM_RSS_FORUM, fid));
        }


        /// <summary>
        /// 论坛整体RSS
        /// </summary>
        public static void ReSetRssXml()
        {
            RemoveObject(CacheKeys.FORUM_RSS_INDEX);
        }


        /// <summary>
        /// 模板id列表
        /// </summary>
        public static void ReSetValidTemplateIDList()
        {
            RemoveObject(CacheKeys.FORUM_TEMPLATE_ID_LIST);
        }


        /// <summary>
        /// 有效的用户表扩展字段
        /// </summary>
        public static void ReSetValidScoreName()
        {
            RemoveObject(CacheKeys.FORUM_VALID_SCORE_NAME);
        }


        /// <summary>
        /// 重设勋章列表
        /// </summary>
        public static void ReSetMedalsList()
        {
            RemoveObject(CacheKeys.FORUM_UI_MEDALS_LIST);
        }

        /// <summary>
        /// 重设数据链接串和数据表前缀
        /// </summary>
        public static void ReSetDBlinkAndTablePrefix()
        {
            RemoveObject(CacheKeys.FORUM_BASE_SETTING_DBCONNECTSTRING);
            RemoveObject(CacheKeys.FORUM_BASE_SETTING_TABLE_PREFIX);
        }

        /// <summary>
        /// 重设最后的帖子表
        /// </summary>
        public static void ReSetLastPostTableName()
        {
            RemoveObject(CacheKeys.FORUM_LAST_POST_TABLE_NAME);
        }


        /// <summary>
        /// 重设帖子列表
        /// </summary>
        public static void ReSetAllPostTableName()
        {
            RemoveObject(CacheKeys.FORUM_POST_TABLE_NAME);

        }

        /// <summary>
        /// 重设广告列表
        /// </summary>
        public static void ReSetAdsList()
        {
            RemoveObject(CacheKeys.FORUM_ADVERTISEMENTS);
        }

        /// <summary>
        /// 重新设置用户上一次执行搜索操作的时间
        /// </summary>
        public static void ReSetStatisticsSearchtime()
        {
            RemoveObject(CacheKeys.FORUM_STATISTICS_SEARCHTIME);
        }


        /// <summary>
        /// 重新设置用户在一分钟内搜索的次数
        /// </summary>
        public static void ReSetStatisticsSearchcount()
        {
            RemoveObject(CacheKeys.FORUM_STATISTICS_SEARCHCOUNT);
        }


        /// <summary>
        /// 重新设置用户头象列表
        /// </summary>
        public static void ReSetCommonAvatarList()
        {
            RemoveObject(CacheKeys.FORUM_COMMON_AVATAR_LIST);
        }

        /// <summary>
        /// 重新设置干扰码字符串
        /// </summary>
        public static void ReSetJammer()
        {
            RemoveObject(CacheKeys.FORUM_UI_JAMMER);
        }

        /// <summary>
        /// 重新设置魔力列表
        /// </summary>
        public static void ReSetMagicList()
        {
            RemoveObject(CacheKeys.FORUM_MAGIC_LIST);
        }

        /// <summary>
        /// 重新设置兑换比率的可交易积分策略
        /// </summary>
        public static void ReSetScorePaySet()
        {
            RemoveObject(CacheKeys.FORUM_SCORE_PAY_SET);
        }


        /// <summary>
        /// 重新设置当前帖子表相关信息
        /// </summary>
        public static void ReSetPostTableInfo()
        {
            RemoveObject(CacheKeys.FORUM_POST_TABLE_NAME);
            RemoveObject(CacheKeys.FORUM_LAST_POST_TABLE_NAME);
            PostTables.ResetPostTables();
        }


        /// <summary>
        /// 重新设置相应的主题列表
        /// </summary>
        /// <param name="fid"></param>
        public static void ReSetTopiclistByFid(string fid)
        {
            RemoveObject(string.Format(CacheKeys.FORUM_TOPIC_LIST_FID, fid));
        }



        /// <summary>
        /// 重新设置全部版块精华主题列表
        /// </summary>
        /// <param name="count">精华个数</param>
        public static void ReSetDigestTopicList(int count)
        {
            ReSetFocusTopicList(count, -1, 0, TopicTimeType.All, TopicOrderType.ID, true);
        }

        //重新设置指定版块精华主题列表[暂未调用]
        public static void ReSetDigestTopicList(int count, int fid, TopicTimeType timetype, TopicOrderType ordertype)
        {
            ReSetFocusTopicList(count, -1, fid, timetype, ordertype, true);
        }

        /// <summary>
        /// 重新设置全部版块热帖主题列表
        /// </summary>
        /// <param name="count"></param>
        /// <param name="views"></param>
        public static void ReSetHotTopicList(int count, int views)
        {
            ReSetFocusTopicList(count, views, 0, TopicTimeType.All, TopicOrderType.ID, false);
        }

        //重新设置指定版块热帖主题列表[暂未调用]
        public static void ReSetHotTopicList(int count, int views, int fid, TopicTimeType timetype, TopicOrderType ordertype)
        {
            ReSetFocusTopicList(count, views, fid, timetype, ordertype, false);
        }

        /// <summary>
        /// 重新设置最近主题列表
        /// </summary>
        /// <param name="count"></param>
        public static void ReSetRecentTopicList(int count)
        {
            ReSetFocusTopicList(count, -1, 0, TopicTimeType.All, TopicOrderType.ID, false);
        }

        private static void ReSetFocusTopicList(int count, int views, int fid, TopicTimeType timetype, TopicOrderType ordertype, bool isdigest)
        {
            string cacheKey = string.Format(CacheKeys.FORUM_TOPIC_LIST_FORMAT,
                count,
                views,
                fid,
                timetype,
                ordertype,
                isdigest
                );
            RemoveObject(cacheKey);
        }

        public static void ResetAlbumCategory()
        {
            RemoveObject(CacheKeys.SPACE_ALBUM_CATEGORY);
        }

        public static void ReSetNavPopupMenu()
        {
            RemoveObject(CacheKeys.FORUM_FORUM_LIST_MENU_DIV);
        }

        /// <summary>
        /// 更新所有缓存
        /// </summary>
        public static void ReSetAllCache()
        {
            DNTCache.GetCacheService().FlushAll();
            EditDntConfig();
            Discuz.Data.OnlineUsers.CreateOnlineTable();
        }

        /// <summary>
        /// 重设BaseConfig
        /// </summary>
        /// <returns></returns>
        public static bool EditDntConfig()
        {
            BaseConfigInfo config = null;
            string filename = DefaultConfigFileManager.ConfigFilePath;
            try
            {
                config = (BaseConfigInfo)SerializationHelper.Load(typeof(BaseConfigInfo), filename);
            }
            catch
            {
                config = null;
            }
            try
            {
                if (config != null)
                {
                    BaseConfigProvider.SetInstance(config);
                    return true;
                }
            }
            catch
            { }
            if (config == null)
            {
                try
                {
                    BaseConfigInfoCollection bcc = (BaseConfigInfoCollection)SerializationHelper.Load(typeof(BaseConfigInfoCollection), filename);
                    foreach (BaseConfigInfo bc in bcc)
                    {
                        if (Utils.GetTrueForumPath() == bc.Forumpath)
                        {
                            config = bc;
                            break;
                        }
                    }

                    if (config == null)
                    {
                        foreach (BaseConfigInfo bc in bcc)
                        {
                            if (Utils.GetTrueForumPath().StartsWith(bc.Forumpath))
                            {
                                config = bc;
                                break;
                            }
                        }
                    }

                    if (config != null)
                    {
                        BaseConfigProvider.SetInstance(config);
                        return true;
                    }

                }
                catch
                { }
            }
            return false;
        }
        #endregion

    }//class end

}
