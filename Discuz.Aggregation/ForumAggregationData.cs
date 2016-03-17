using System;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Common.Xml;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Forum;

namespace Discuz.Aggregation
{
    /// <summary>
    /// 论坛聚合数据类
    /// </summary>
    public class ForumAggregationData : AggregationData
    {
        /// <summary>
        /// 推荐的论坛主题帖
        /// </summary>
        private static PostInfo[] postInfos;
        /// <summary>
        /// 聚合首页BBS版块(主题)列表
        /// </summary>
        private static DataTable topicList;
        /// <summary>
        /// 聚合首页版块推荐主题
        /// </summary>
        private static StringBuilder topicJson;

        private static int[] recommendForumidArray;

        private static TopicOrderType? topicOrderType;

        /// <summary>
        /// 清空数据绑定
        /// </summary>
        public override void ClearDataBind()
        {
            postInfos = null;
            topicList = null;
            topicJson = null;
            recommendForumidArray = null;
            topicOrderType = null;
        }

        /// <summary>
        /// 获取后台推荐的版块id串
        /// </summary>
        /// <returns></returns>
        public int[] GetRecommendForumID()
        {
            if (recommendForumidArray != null)
                return recommendForumidArray;

            XmlNodeList xmlnodelist = xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Website_forumrecomend");
            if (xmlnodelist.Count > 0)
            {
                string forumidlist = xmlDoc.GetSingleNodeValue(xmlnodelist[0], "fidlist") == null ? "" : xmlDoc.GetSingleNodeValue(xmlnodelist[0], "fidlist");
                if (!Utils.StrIsNullOrEmpty(forumidlist))
                {
                    string[] forumidarray = forumidlist.Split(',');
                    if (Utils.IsNumericArray(forumidarray))
                    {
                        recommendForumidArray = new int[forumidarray.Length];
                        int i = 0;
                        foreach (string forumid in forumidarray)
                        {
                            if (forumid != "")
                            {
                                recommendForumidArray[i] = Convert.ToInt32(forumid);
                                i++;
                            }
                        }
                    }
                }
            }
            return recommendForumidArray == null ? new int[] { 0 } : recommendForumidArray;
        }

        /// <summary>
        /// 获取聚合首页自动推荐主题列表的排序规则
        /// </summary>
        /// <returns></returns>
        public TopicOrderType GetForumAggregationTopicListOrder()
        {
            if (topicOrderType == null)
            {
                XmlDocumentExtender doc = new XmlDocumentExtender();
                doc.Load(Utils.GetMapPath(BaseConfigs.GetForumPath + "config/aggregation.config"));
                topicOrderType = (TopicOrderType)TypeConverter.ObjectToInt(doc.GetSingleNodeValue(doc.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Forum")[0], "Bbs/Showtype"));
            }
            return (TopicOrderType)topicOrderType;
        }

   

        #region 从XML中检查出指定的主题信息

        /// <summary>
        /// 获得推荐的论坛主题帖对象数组
        /// </summary>
        /// <param name="nodename">节点名称</param>
        /// <returns></returns>
        public PostInfo[] GetPostListFromFile(string nodeName)
        {
            if (postInfos != null)
                return postInfos;

            XmlNodeList xmlnodelist = xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/" + nodeName + "/Forum/Topiclist/Topic");
            postInfos = new PostInfo[xmlnodelist.Count];
            int rowcount = 0;

            foreach (XmlNode xmlnode in xmlnodelist)
            {
                postInfos[rowcount] = new PostInfo();
                postInfos[rowcount].Tid = TypeConverter.ObjectToInt(xmlDoc.GetSingleNodeValue(xmlnode, "topicid"));
                postInfos[rowcount].Title = (xmlDoc.GetSingleNodeValue(xmlnode, "title") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "title");
                postInfos[rowcount].Poster = (xmlDoc.GetSingleNodeValue(xmlnode, "poster") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "poster");
                postInfos[rowcount].Posterid = TypeConverter.ObjectToInt(xmlDoc.GetSingleNodeValue(xmlnode, "posterid"));
                postInfos[rowcount].Postdatetime = (xmlDoc.GetSingleNodeValue(xmlnode, "postdatetime") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "postdatetime");
                postInfos[rowcount].Message = (xmlDoc.GetSingleNodeValue(xmlnode, "shortdescription") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "shortdescription");
                postInfos[rowcount].Fid = TypeConverter.ObjectToInt(xmlDoc.GetSingleNodeValue(xmlnode, "fid"));
                postInfos[rowcount].Forumname = (xmlDoc.GetSingleNodeValue(xmlnode, "forumname") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "forumname");
                postInfos[rowcount].ForumRewriteName = (xmlDoc.GetSingleNodeValue(xmlnode, "forumrewritename") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "forumrewritename");
                rowcount++;
            }
            return postInfos;
        }

        public string GetTopicJsonFromFile()
        {
            if (topicJson != null)
                return topicJson.ToString();

            topicJson = new StringBuilder();
            topicJson.Append("[");
            XmlNodeList xmlnodelist = xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Website_forumrecomendtopiclist/Website_forumrecomendtopic");
            int rowcount = 1;

            foreach (XmlNode xmlnode in xmlnodelist)
            {
                topicJson.Append(string.Format("{{'id' : {0}, 'title' : '{1}', 'fid' : {2}, 'img' : '{3}', 'tid' : {4}}},",
                       rowcount,
                       xmlDoc.GetSingleNodeValue(xmlnode, "title") == null ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "title"),
                       xmlDoc.GetSingleNodeValue(xmlnode, "fid") == null ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "fid")),
                       xmlDoc.GetSingleNodeValue(xmlnode, "img") == null ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "img"),
                       xmlDoc.GetSingleNodeValue(xmlnode, "tid") == null ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "tid"))
                       ));
                rowcount++;
            }
            if (topicJson.ToString().EndsWith(","))
                topicJson.Remove(topicJson.Length - 1, 1);

            topicJson.Append("]");
            return topicJson.ToString();
        }



        #endregion

        #region 得到主题列表

        /// <summary>
        /// 获得聚合首页BBS版块(主题)列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetForumTopicList()
        {
            if (topicList != null)
                return topicList;

            //返回的记录数
            int topnumber = 10;
            XmlNode xmlnode = xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Forum/Bbs").Item(0);

            if (xmlDoc.GetSingleNodeValue(xmlnode, "Topnumber") != null)
            {
                try
                {
                    topnumber = Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "Topnumber").ToLower());
                }
                catch
                {
                    topnumber = 10;
                }
            }

            if (topicOrderType != null)
                topicList = Discuz.Data.DatabaseProvider.GetInstance().GetWebSiteAggForumTopicList(xmlDoc.GetSingleNodeValue(xmlnode, "Showtype").ToLower(), topnumber);
            else
                topicList = Discuz.Data.DatabaseProvider.GetInstance().GetWebSiteAggForumTopicList("3", topnumber);

            return topicList;
        }

        #endregion

        #region 得到热门版块列表

        /// <summary>
        /// 得到热门版块列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetHotForumList(int topNumber, string orderby, int fid)
        {
            orderby = orderby == "" ? "posts" : orderby;
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            DataTable forumList = cache.RetrieveObject("/Aggregation/HotForumList") as DataTable;
            if (forumList == null)
            {
                forumList = Discuz.Data.DatabaseProvider.GetInstance().GetWebSiteAggHotForumList(topNumber <= 0 ? 10 : topNumber, orderby,fid);

                //声明新的缓存策略接口
                //Discuz.Cache.ICacheStrategy ics = new Discuz.Forum.ForumCacheStrategy();
                //ics.TimeOut = 300;
                //cache.LoadCacheStrategy(ics);
                cache.AddObject("/Aggregation/HotForumList", forumList, 300);
                //cache.LoadDefaultCacheStrategy();
            }
            return forumList;
        }

        #endregion

        #region 得到主题列表

        /// <summary>
        /// 获取论坛主题列表
        /// </summary>
        /// <param name="count">主题数</param>
        /// <param name="views">浏览量</param>
        /// <param name="forumid">版块ID</param>
        /// <param name="timetype">时间类型</param>
        /// <param name="ordertype">排序字段</param>
        /// <param name="isdigest">是否精化</param>
        /// <param name="onlyimg">是否包含附件</param>
        /// <returns></returns>
        public DataTable GetForumTopicList(int count, int views, int forumid, TopicTimeType timeType, TopicOrderType orderType, bool isDigest, bool onlyImg)
        {
            return Focuses.GetTopicList(count, views, forumid, "", timeType, orderType, isDigest, 5, onlyImg,"");
        }


        #endregion

        #region 得到用户列表

        /// <summary>
        /// 获取用户列表信息
        /// </summary>
        /// <param name="topnumber">获取用户数量</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="orderby">排序方式</param>
        /// <returns></returns>
        public DataTable GetUserList(int topNumber, string orderBy)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            DataTable userList = cache.RetrieveObject("/Aggregation/Users_" + orderBy + "List") as DataTable;
            if (userList == null)
            {
                userList = Users.GetUserList(topNumber, 1, orderBy, "desc");

                ////声明新的缓存策略接口
                //Discuz.Cache.ICacheStrategy ics = new AggregationCacheStrategy();
                //ics.TimeOut = 300;
                //cache.LoadCacheStrategy(ics);
                cache.AddObject("/Aggregation/Users_" + orderBy + "List", userList, 300);
                //cache.LoadDefaultCacheStrategy();
            }
            return userList;
        }

        /// <summary>
        /// 获取指定版块下的最新回复
        /// </summary>
        /// <param name="fid">指定的版块</param>
        /// <param name="count">返回记录数</param>
        /// <returns></returns>
        public DataTable GetLastPostList(int fid, int count)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            DataTable postList = cache.RetrieveObject("/Aggregation/lastpostList_" + fid) as DataTable;
            if (postList == null)
            {
                postList = Discuz.Data.DatabaseProvider.GetInstance().GetLastPostList(fid, count, Posts.GetPostTableName(), Forums.GetVisibleForum());

                //声明新的缓存策略接口
                //Discuz.Cache.ICacheStrategy ics = new AggregationCacheStrategy();
                //ics.TimeOut = 300;
                //cache.LoadCacheStrategy(ics);
                cache.AddObject("/Aggregation/lastpostList_" + fid, postList, 300);
                //cache.LoadDefaultCacheStrategy();
            }
            return postList;
        }

        /// <summary>
        /// 获取用户单位时间内的发帖数
        /// </summary>
        /// <param name="topNumber">Top条数</param>
        /// <param name="dateType">时间类型</param>
        /// <param name="dateNum">时间数</param>
        /// <returns></returns>
        public List<UserPostCountInfo> GetUserPostCountList(int topNumber, DateType dateType, int dateNum)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            List<UserPostCountInfo> userPostCountInfoList = cache.RetrieveObject("/Aggregation/UserPostCountList") as List<UserPostCountInfo>;
            if (userPostCountInfoList == null)
            {
                userPostCountInfoList = Posts.GetUserPostCountList(topNumber, dateType, (dateNum > 1 ? dateNum : 1));

                //声明新的缓存策略接口
                //Discuz.Cache.ICacheStrategy ics = new AggregationCacheStrategy();
                //ics.TimeOut = 120;
                //cache.LoadCacheStrategy(ics);
                cache.AddObject("/UserPostCountList", userPostCountInfoList, 120);
                //cache.LoadDefaultCacheStrategy();
            }
            return userPostCountInfoList;
        }

        #endregion
    }

    public class TopicAggregationData
    {
        public static DataTable GetForumAggregationTopic(int fid)
        {
            //从缓存读取主题列表
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            DataTable topicList = cache.RetrieveObject("/Aggregation/TopicByForumId_" + fid) as DataTable;
            if (topicList != null)
                return topicList;

            //无缓存查检config文件存在否
            string configPath = HttpContext.Current.Server.MapPath(BaseConfigs.GetForumPath + "config/agg_" + fid + ".config");
            if (!File.Exists(configPath))
                return new DataTable();

            //从config文件中重建缓存
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(configPath);
                XmlNode node = xmlDoc.SelectSingleNode("/Aggregationinfo/Forum");
                if (node != null)
                {
                    DataSet topicDataSet = new DataSet();
                    using (MemoryStream topicListXml = new MemoryStream(Encoding.UTF8.GetBytes(Regex.Replace(node.InnerXml, "[\x00-\x08|\x0b-\x0c|\x0e-\x1f]", ""))))
                    {
                        topicDataSet.ReadXml(topicListXml);
                    }
                    if (topicDataSet.Tables.Count != 0)
                        topicList = topicDataSet.Tables[0];
                    else
                        topicList = new DataTable();
                }
                else
                    topicList = new DataTable();
            }
            catch
            {
                topicList = new DataTable();
            }

            //声明新的缓存策略接口
            //Discuz.Cache.ICacheStrategy ics = new AggregationCacheStrategy();
            //ics.TimeOut = 300;
            //cache.LoadCacheStrategy(ics);
            cache.AddObject("/Aggregation/TopicByForumId_" + fid, topicList, 300);
            //cache.LoadDefaultCacheStrategy();
            return topicList;
        }

        public static DataTable GetForumAggerationHotTopics()
        {
            //从缓存读取主题列表
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            DataTable hotTopicList = cache.RetrieveObject("/Aggregation/Hottopiclist") as DataTable;
            if (hotTopicList != null)
                return hotTopicList;

            //无缓存查检config文件存在否
            string configPath = HttpContext.Current.Server.MapPath(BaseConfigs.GetForumPath + "config/agg_hottopics.config");
            if (!File.Exists(configPath))
                return new DataTable();

            //从config文件中重建缓存
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(configPath);
                XmlNode node = xmlDoc.SelectSingleNode("/Aggregationinfo/Forum");
                if (node != null)
                {
                    DataSet topicDataSet = new DataSet();
                    using (MemoryStream topicListXml = new MemoryStream(Encoding.UTF8.GetBytes(Regex.Replace(node.InnerXml, "[\x00-\x08|\x0b-\x0c|\x0e-\x1f]", ""))))
                    {
                        topicDataSet.ReadXml(topicListXml);
                    }
                    if (topicDataSet.Tables.Count != 0)
                        hotTopicList = topicDataSet.Tables[0];
                    else
                        hotTopicList = new DataTable();
                }
                else
                    hotTopicList = new DataTable();
            }
            catch
            {
                hotTopicList = new DataTable();
            }

            //声明新的缓存策略接口
            //Discuz.Cache.ICacheStrategy ics = new AggregationCacheStrategy();
            //ics.TimeOut = 300;
            //cache.LoadCacheStrategy(ics);
            cache.AddObject("/Aggregation/Hottopiclist", hotTopicList, 300);
            //cache.LoadDefaultCacheStrategy();
            return hotTopicList;
        }
    }
}
