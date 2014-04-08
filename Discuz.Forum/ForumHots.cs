using System;
using System.Text;
using System.Data;

using Discuz.Cache;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;
using System.IO;
using System.Text.RegularExpressions;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    /// <summary>
    /// 热点管理类
    /// </summary>
    public class ForumHots
    {
        /// <summary>
        /// 获得帖子列表
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="views">最小浏览量</param>
        /// <param name="fid">板块ID</param>
        /// <param name="timetype">期限类型,一天、一周、一月、不限制</param>
        /// <param name="ordertype">排序类型,时间倒序、浏览量倒序、最后回复倒序</param>
        /// <param name="isdigest">是否精华</param>
        /// <param name="cachetime">缓存的有效期(单位:分钟)</param>
        /// <returns></returns>
        public static DataTable GetTopicList(ForumHotItemInfo forumHotItemInfo)
        {
            //防止恶意行为
            forumHotItemInfo.Cachetimeout = forumHotItemInfo.Cachetimeout == 0 ? 1 : forumHotItemInfo.Cachetimeout;
            forumHotItemInfo.Dataitemcount = forumHotItemInfo.Dataitemcount > 50 ? 50 : (forumHotItemInfo.Dataitemcount < 1 ? 1 : forumHotItemInfo.Dataitemcount);

            DataTable dt = new DataTable();

            if (forumHotItemInfo.Cachetimeout > 0)
                dt = DNTCache.GetCacheService().RetrieveObject("/Forum/ForumHostList-" + forumHotItemInfo.Id) as DataTable;

            if (dt == null)
            {
                //如果版块idlist设置为空，则默认读取所有可见板块的idlist
                string forumList = string.IsNullOrEmpty(forumHotItemInfo.Forumlist) ? Forums.GetVisibleForum() : forumHotItemInfo.Forumlist;
                string orderFieldName = Focuses.GetFieldName((TopicOrderType)Enum.Parse(typeof(TopicOrderType), forumHotItemInfo.Sorttype));

                dt = Discuz.Data.Topics.GetTopicList(forumHotItemInfo.Dataitemcount, -1, 0, "",
                    Focuses.GetStartDate((TopicTimeType)Enum.Parse(typeof(TopicTimeType), forumHotItemInfo.Datatimetype)),
                    orderFieldName, forumList, orderFieldName == "digest", false);

                if (forumHotItemInfo.Cachetimeout > 0)
                    DNTCache.GetCacheService().AddObject("/Forum/ForumHostList-" + forumHotItemInfo.Id, dt, forumHotItemInfo.Cachetimeout);
            }
            return dt;
        }

        /// <summary>
        /// 获取一个帖子的缓存
        /// </summary>
        /// <param name="tid">帖子ID</param>
        /// <param name="cachetime">缓存的有效期</param>
        /// <returns></returns>
        public static DataTable GetFirstPostInfo(int tid, int cachetime)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject("/Forum/HotForumFirst_" + tid) as DataTable;
            if (dt == null)
            {
                dt = Posts.GetPostList(tid.ToString());
                cache.AddObject("/Forum/HotForumFirst_" + tid, dt, cachetime);
            }
            return dt;
        }


        /// <summary>
        /// 获取热门板块
        /// </summary>
        /// <param name="topNumber">获取的数量</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="fid">板块ID</param>
        /// <param name="cachetime">缓存时间</param>
        /// <returns></returns>
        public static List<ForumInfo> GetHotForumList(int topNumber, string orderby, int cachetime, int tabid)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            List<ForumInfo> forumList = cache.RetrieveObject("/Aggregation/HotForumList_" + tabid) as List<ForumInfo>;
            if (forumList == null)
            {
                forumList = Stats.GetForumArray(orderby);
                if (forumList.Count > topNumber)
                {
                    List<ForumInfo> list = new List<ForumInfo>();
                    for (int i = 0; i < topNumber; i++)
                        list.Add(forumList[i]);

                    forumList = list;
                }
                cache.AddObject("/Aggregation/HotForumList" + tabid, forumList, cachetime);
            }
            return forumList;
        }

        /// <summary>
        /// 获取热门用户
        /// </summary>
        /// <param name="topNumber">获取的数量</param>
        /// <param name="orderBy">排序方式</param>
        /// <param name="cachetime">缓存时间</param>
        /// <returns></returns>
        public static ShortUserInfo[] GetUserList(int topNumber, string orderBy, int cachetime, int tabid)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            ShortUserInfo[] userList = cache.RetrieveObject("/Aggregation/Users_" + tabid + "List") as ShortUserInfo[];
            if (userList == null)
            {
                if (Utils.InArray(orderBy, "lastactivity,joindate"))
                {
                    List<ShortUserInfo> list = new List<ShortUserInfo>();
                    DataTable dt = Users.GetUserList(topNumber, 1, orderBy, "desc");
                    foreach (DataRow dr in dt.Rows)
                    {
                        ShortUserInfo info = new ShortUserInfo();
                        info.Uid = TypeConverter.ObjectToInt(dr["uid"]);
                        info.Username = dr["username"].ToString();
                        info.Lastactivity = dr["lastactivity"].ToString();
                        info.Joindate = dr["joindate"].ToString();
                        list.Add(info);
                    }
                    userList = list.ToArray();
                }
                else
                {
                    userList = Stats.GetUserArray(orderBy);
                    if (userList.Length > topNumber)
                    {
                        List<ShortUserInfo> list = new List<ShortUserInfo>();
                        for (int i = 0; i < topNumber; i++)
                            list.Add(userList[i]);

                        userList = list.ToArray();
                    }
                }
                cache.AddObject("/Aggregation/Users_" + tabid + "List", userList, cachetime);
            }
            return userList;
        }

        /// <summary>
        /// 获取热门图片
        /// </summary>
        /// <param name="topNumber">获取的数量</param>
        /// <param name="orderBy">排序方式</param>
        /// <param name="cachetime">缓存时间</param>
        /// <returns></returns>
        private static DataTable HotImages(int count, int cachetime, string orderby, int tabid, string fidlist, int continuous)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            DataTable imagelist = cache.RetrieveObject("/Aggregation/HotImages_" + tabid + "List") as DataTable;
            if (imagelist == null)
            {
                imagelist = Discuz.Data.DatabaseProvider.GetInstance().GetWebSiteAggHotImages(count, orderby, fidlist, continuous);
                cache.AddObject("/Aggregation/HotImages_" + tabid + "List", imagelist, cachetime);
            }
            return imagelist;
        }

        /// <summary>
        /// 转换热门图片为数组
        /// </summary>
        /// <param name="topNumber">获取的数量</param>
        /// <param name="orderBy">排序方式</param>
        /// <param name="cachetime">缓存时间</param>
        /// <returns></returns>
        public static string HotImagesArray(ForumHotItemInfo forumHotItemInfo)
        {
            string imagesItemTemplate = "title:\"{0}\",img:\"{1}\",url:\"{2}\"";
            StringBuilder hotImagesArray = new StringBuilder();

            //如果没有缩略图目录，则去生成
            if (!Directory.Exists(Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/rotatethumbnail/")))
                Utils.CreateDir(Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/rotatethumbnail/"));

            //如果版块idlist设置为空，则默认读取所有可见板块的idlist
            string forumList = string.IsNullOrEmpty(forumHotItemInfo.Forumlist) ? Forums.GetVisibleForum() : forumHotItemInfo.Forumlist;

            foreach (DataRow dr in HotImages(forumHotItemInfo.Dataitemcount, forumHotItemInfo.Cachetimeout, forumHotItemInfo.Sorttype, forumHotItemInfo.Id, forumList, forumHotItemInfo.Enabled).Rows)
            {
                int tid = TypeConverter.ObjectToInt(dr["tid"]);
                string fileName = dr["filename"].ToString().Trim();
                string title = dr["title"].ToString().Trim();

                title = Utils.JsonCharFilter(title).Replace("'", "\\'");

                if (fileName.StartsWith("http://"))
                {
                    DeleteCacheImageFile();
                    Thumbnail.MakeRemoteThumbnailImage(fileName, Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/rotatethumbnail/r_" + Utils.GetFilename(fileName)), 360, 240);
                    hotImagesArray.Append("{");
                    hotImagesArray.AppendFormat(imagesItemTemplate, title, "cache/rotatethumbnail/r_" + Utils.GetFilename(fileName), Urls.ShowTopicAspxRewrite(tid, 0));
                    hotImagesArray.Append("},");
                    continue;
                }
                //图片文件名称
                string fullFileName = BaseConfigs.GetForumPath + "upload/" + fileName.Replace('\\', '/').Trim();
                //图片缩略后的名称
                string thumbnailFileName = "cache/rotatethumbnail/r_" + Utils.GetFilename(fullFileName);

                if (!File.Exists(Utils.GetMapPath(BaseConfigs.GetForumPath + thumbnailFileName)) && File.Exists(Utils.GetMapPath(fullFileName)))
                {
                    DeleteCacheImageFile();
                    Thumbnail.MakeThumbnailImage(Utils.GetMapPath(fullFileName), Utils.GetMapPath(BaseConfigs.GetForumPath + thumbnailFileName), 360, 240);
                }
                hotImagesArray.Append("{");
                hotImagesArray.AppendFormat(imagesItemTemplate, title, "cache/rotatethumbnail/r_" + Utils.GetFilename(fullFileName), Urls.ShowTopicAspxRewrite(tid, 0));
                hotImagesArray.Append("},");
            }

            return "[" + hotImagesArray.ToString().TrimEnd(',') + "]";
        }

        /// <summary>
        /// 返回删除了图片附件
        /// </summary>
        /// <param name="message">帖子内容</param>
        /// <param name="length">截取内容的长度</param>
        /// <returns></returns>
        public static string RemoveUbb(string message, int length)
        {
            message = Regex.Replace(message, @"\[attachimg\](\d+)(\[/attachimg\])*", "{图片}", RegexOptions.IgnoreCase);
            message = Regex.Replace(message, @"\[img\]\s*([^\[\<\r\n]+?)\s*\[\/img\]", "{图片}", RegexOptions.IgnoreCase);
            message = Regex.Replace(message, @"\[img=(\d{1,4})[x|\,](\d{1,4})\]\s*([^\[\<\r\n]+?)\s*\[\/img\]", "{图片}", RegexOptions.IgnoreCase);
            message = Regex.Replace(message, @"\[attach\](\d+)(\[/attach\])*", "{附件}", RegexOptions.IgnoreCase);
            //隐藏内容有两种UBB格式，正则需要区分，两种正则不会冲突
            message = Regex.Replace(message, @"\s*\[hide\][\n\r]*([\s\S]+?)[\n\r]*\[\/hide\]\s*", "{隐藏内容}", RegexOptions.IgnoreCase);
            message = Regex.Replace(message, @"\s*\[hide=(\d+?)\][\n\r]*([\s\S]+?)[\n\r]*\[\/hide\]\s*", "{隐藏内容}", RegexOptions.IgnoreCase);

            if (message.IndexOf("[free]") > -1)
            {
                Match match = Regex.Match(message, @"\s*\[free\][\n\r]*([\s\S]+?)[\n\r]*\[\/free\]\s*", RegexOptions.IgnoreCase);
                message = match.Groups[0] != null && match.Groups[0].Value != "" ? match.Groups[0].Value : message;
            }
            return Utils.GetSubString(Utils.ClearUBB(Utils.RemoveHtml(message)).Replace("{", "[").Replace("}", "]"), length, "......");
        }
        private static void DeleteCacheImageFile()
        {
            FileInfo[] files = new DirectoryInfo(Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/rotatethumbnail/")).GetFiles();
            //如果缓存文件夹cache/rotatethumbnail 下的文件大于100个，则删除
            if (files.Length > 100)
            {
                Attachments.QuickSort(files, 0, files.Length - 1);

                for (int i = files.Length - 1; i >= 50; i--)
                {
                    try
                    {
                        files[i].Delete();
                    }
                    catch
                    { }
                }
            }

        }
    }
}