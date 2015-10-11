using System;
using System.Data;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.Space;

namespace Discuz.Forum
{
    /// <summary>
    /// 焦点数据操作类
    /// </summary>
    public class Focuses
    {
        /// <summary>
        /// 精华主题列表
        /// </summary>
        /// <param name="count">获取数</param>
        /// <returns></returns>
        public static DataTable GetDigestTopicList(int count)
        {
            return GetTopicList(count, -1, 0, "", TopicTimeType.All, TopicOrderType.ID, true, 20, false,"");
        }

        /// <summary>
        /// 获取精华主题列表
        /// </summary>
        /// <param name="count">获取数</param>
        /// <param name="fid">版块id</param>
        /// <param name="timetype">时间类型</param>
        /// <param name="ordertype">排序类型</param>
        /// <returns></returns>
        public static DataTable GetDigestTopicList(int count, int fid, TopicTimeType timetype, TopicOrderType ordertype)
        {
            return GetTopicList(count, -1, fid, "", timetype, ordertype, true, 20, false,"");
        }


        /// <summary>
        /// 获得热门主题列表
        /// </summary>
        /// <param name="count">获取数</param>
        /// <param name="views">查看数</param>
        /// <returns></returns>
        public static DataTable GetHotTopicList(int count, int views)
        {
            return GetTopicList(count, views, 0, "", TopicTimeType.All, TopicOrderType.ID, false, 20, false,"");
        }

        /// <summary>
        /// 获得热门主题列表
        /// </summary>
        /// <param name="count">获取数</param>
        /// <param name="views">查看数</param>
        /// <param name="fid">版块id</param>
        /// <param name="timetype">时间类型</param>
        /// <param name="ordertype">排序类型</param>
        /// <returns></returns>
        public static DataTable GetHotTopicList(int count, int views, int fid, TopicTimeType timetype, TopicOrderType ordertype)
        {
            return GetTopicList(count, views, fid, "", timetype, ordertype, false, 20, false,"");
        }

        /// <summary>
        /// 获得最新发表主题列表
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static DataTable GetRecentTopicList(int count)
        {
            return GetTopicList(count, -1, 0, "", TopicTimeType.All, TopicOrderType.ID, false, 20, false,"");
        }

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
        public static DataTable GetTopicList(int count, int views, int fid, string typeIdList, TopicTimeType timetype, TopicOrderType ordertype, bool isdigest, int cachetime, bool onlyimg)
        {
            return GetTopicList(count, views, fid, typeIdList, timetype, ordertype, isdigest, cachetime, onlyimg, "");
        }
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
        public static DataTable GetTopicList(int count, int views, int fid, string typeIdList, TopicTimeType timetype, TopicOrderType ordertype, bool isdigest, int cachetime, bool onlyimg,string fidlist)
        {
            //防止恶意行为
            if (cachetime == 0)
                cachetime = 1;

            if (count > 50)
                count = 50;

            if (count < 1)
                count = 1;

            string cacheKey = string.Format("/Forum/TopicList-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}", count, views, fid, timetype, ordertype, isdigest, onlyimg, typeIdList.Replace(',', 'd'));
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject(cacheKey) as DataTable;

            if (dt == null)
            {
                if (fidlist == "")
                {
                    fidlist = Forums.GetVisibleForum();
                }
                //dt = Discuz.Data.Topics.GetTopicList(count, views, fid, typeIdList, GetStartDate(timetype), GetFieldName(ordertype), Forums.GetVisibleForum(), isdigest, onlyimg);
                dt = Discuz.Data.Topics.GetTopicList(count, views, fid, typeIdList, GetStartDate(timetype), GetFieldName(ordertype), fidlist, isdigest, onlyimg);

                //声明新的缓存策略接口
                //Discuz.Cache.ICacheStrategy ics = new ForumCacheStrategy();
                //ics.TimeOut = cachetime * 60;
                //cache.LoadCacheStrategy(ics);
                cache.AddObject(cacheKey, dt, cachetime * 60);
                //cache.LoadDefaultCacheStrategy();
            }
            return dt;
        }

        public static DataTable GetUpdatedSpaces(int count, int cachetime)
        {
            //防止恶意行为
            if (cachetime == 0)
                cachetime = 1;

            if (count > 50)
                count = 50;

            if (count < 1)
                count = 1;

            string cacheKey = "/Space/UpdatedSpace-" + count.ToString();
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject(cacheKey) as DataTable;

            if (dt == null)
            {
                dt = SpacePluginProvider.GetInstance().GetWebSiteAggRecentUpdateSpaceList(count);

                //声明新的缓存策略接口
                //Discuz.Cache.ICacheStrategy ics = new ForumCacheStrategy();
                //ics.TimeOut = cachetime * 60;
                //cache.LoadCacheStrategy(ics);
                cache.AddObject(cacheKey, dt, cachetime * 60);
                //cache.LoadDefaultCacheStrategy();
            }
            return dt;
        }

        public static DataTable GetNewSpacePosts(int count, int cachetime)
        {
            //防止恶意行为
            if (cachetime == 0)
                cachetime = 1;

            if (count > 50)
                count = 50;

            if (count < 1)
                count = 1;

            string cacheKey = "/Space/NewSpacePosts-" + count.ToString();
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject(cacheKey) as DataTable;

            if (dt == null)
            {
                dt = SpacePluginProvider.GetInstance().GetWebSiteAggSpacePostList(count);

                //声明新的缓存策略接口
                //Discuz.Cache.ICacheStrategy ics = new ForumCacheStrategy();
                //ics.TimeOut = cachetime * 60;
                //cache.LoadCacheStrategy(ics);
                cache.AddObject(cacheKey, dt, cachetime * 60);
                //cache.LoadDefaultCacheStrategy();
            }
            return dt;
        }

        #region Helper
        /// <summary>
        /// 获取开始日期
        /// </summary>
        /// <param name="type">日期类型</param>
        /// <returns></returns>
        public static string GetStartDate(TopicTimeType type)
        {
            DateTime dtnow = DateTime.Now;
            switch (type)
            {
                case TopicTimeType.Day:
                    return dtnow.AddDays(-1).ToString();
                case TopicTimeType.ThreeDays:
                    return dtnow.AddDays(-3).ToString();
                case TopicTimeType.FiveDays:
                    return dtnow.AddDays(-5).ToString();
                case TopicTimeType.Week:
                    return dtnow.AddDays(-7).ToString();
                case TopicTimeType.Month:
                    return dtnow.AddDays(-30).ToString();
                case TopicTimeType.SixMonth:
                    return dtnow.AddMonths(-6).ToString();
                case TopicTimeType.Year:
                    return dtnow.AddYears(-1).ToString();
                default: return "1754-1-1";
            }
        }

        /// <summary>
        /// 获取字段名
        /// </summary>
        /// <param name="type">排序类型</param>
        /// <returns></returns>
        public static string GetFieldName(TopicOrderType type)
        {
            switch (type)
            {
                case TopicOrderType.Views:
                    return "views";
                case TopicOrderType.LastPost:
                    return "lastpost";
                case TopicOrderType.PostDateTime:
                    return "postdatetime";
                case TopicOrderType.Digest:
                    return "digest";
                case TopicOrderType.Replies:
                    return "replies";
                case TopicOrderType.Rate:
                    return "rate";

                default: return "tid";
            }
        }
        #endregion

    }

}
