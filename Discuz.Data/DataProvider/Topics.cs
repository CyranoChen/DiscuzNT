using System.Data;
using System;
using System.Text;

using Discuz.Common;
using Discuz.Config;
using Discuz.Common.Generic;
using Discuz.Entity;
using Discuz.Cache.Data;

namespace Discuz.Data
{
    public class Topics
    {
        /// <summary>
        /// 是否启用TokyoTyrantCache缓存用户表
        /// </summary>
        public static bool appDBCache = (EntLibConfigs.GetConfig() != null && EntLibConfigs.GetConfig().Cachetopics.Enable);

        public static ICacheTopics ITopicService = appDBCache ? DBCacheService.GetTopicsService() : null;

        /// <summary>
        /// 创建新主题
        /// </summary>
        /// <param name="topicInfo">主题信息</param>
        /// <returns>返回主题ID</returns>
        public static int CreateTopic(TopicInfo topicInfo)
        {
            topicInfo.Tid = DatabaseProvider.GetInstance().CreateTopic(topicInfo);

            if (appDBCache)
                ITopicService.CreateTopic(topicInfo);

            return topicInfo.Tid;
        }


        /// <summary>
        /// 按照用户Id获取其回复过的主题总数
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>回复过的主题总数</returns>
        public static int GetTopicReplyCountbByUserId(int userId)
        {
            return DatabaseProvider.GetInstance().GetTopicReplyCountByUserId(userId);
        }

        /// <summary>
        /// 按照用户Id获取主题总数
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>主题总数</returns>
        public static int GetTopicCountByUserId(int userId)
        {
            return DatabaseProvider.GetInstance().GetTopicCountByUserId(userId);
        }


        /// <summary>
        /// 获得主题信息
        /// </summary>
        /// <param name="tid">要获得的主题ID</param>
        /// <param name="fid">版块ID</param>
        /// <param name="mode">模式选择, 0=当前主题, 1=上一主题, 2=下一主题</param>
        public static TopicInfo GetTopicInfo(int tid, int fid, byte mode)
        {
            TopicInfo topicInfo = null;
            if (appDBCache)
                topicInfo = ITopicService.GetTopicInfo(tid, fid, mode);

            if (topicInfo == null)
            {
                IDataReader reader = DatabaseProvider.GetInstance().GetTopicInfo(tid, fid, mode);
                if (reader.Read())
                    topicInfo = LoadSingleTopicInfo(reader);

                reader.Close();

                //if (appDBCache && topicInfo != null)
                //    ITopicService.CreateTopic(topicInfo);
            }
            return topicInfo;
        }


        /// <summary>
        /// 获得指定的主题列表
        /// </summary>
        /// <param name="tidList">主题ID列表</param>
        /// <param name="displayOrder">displayorder的下限( WHERE [displayorder]>此值)</param>
        /// <returns>主题列表</returns>
        public static DataTable GetTopicList(string tidList, int displayOrder)
        {
            if (!Utils.IsNumericList(tidList))
            {
                return null;
            }
            return DatabaseProvider.GetInstance().GetTopicList(tidList, displayOrder);
        }

        /// <summary>
        /// 获得帖子列表
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="views">最小浏览量</param>
        /// <param name="fid">板块ID</param>
        /// <param name="timeType">期限类型,一天、一周、一月、不限制</param>
        /// <param name="orderType">排序类型,时间倒序、浏览量倒序、最后回复倒序</param>
        /// <param name="visibleFidList">可见论坛id列表</param>
        /// <param name="isDigest">是否精华</param>
        /// <param name="onlyImg">是否只显示有图片的帖子</param>
        /// <returns></returns>
        public static DataTable GetTopicList(int count, int views, int fid, string typeIdList, string timeType, string orderType, string visibleFidList, bool isDigest, bool onlyImg)
        {
            return DatabaseProvider.GetInstance().GetFocusTopicList(count, views, fid, typeIdList, timeType, orderType, visibleFidList, isDigest, onlyImg);
        }

        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="topicInfo">主题信息</param>
        /// <returns>成功返回1，否则返回0</returns>
        public static int UpdateTopic(TopicInfo topicInfo)
        {
            if (appDBCache)
                ITopicService.UpdateTopic(topicInfo);

            return DatabaseProvider.GetInstance().UpdateTopic(topicInfo);
        }

        /// <summary>
        /// 列新主题的回复数
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postTableId">当前帖子分表Id</param>
        public static void UpdateTopicReplyCount(int tid, string postTableId)
        {
            DatabaseProvider.GetInstance().UpdateTopicReplyCount(tid, postTableId);

            if (appDBCache)
                ITopicService.UpdateTopicReplyCount(tid);
        }

        /// <summary>
        /// 更新主题为已被管理
        /// </summary>
        /// <param name="tidList">主题id列表</param>
        /// <param name="moderated">管理操作id</param>
        /// <returns>成功返回1，否则返回0</returns>
        public static int UpdateTopicModerated(string tidList, int moderated)
        {
            if (!Utils.IsNumericList(tidList))
                return 0;

            if (appDBCache)
                ITopicService.UpdateTopicModerated(tidList, moderated);

            return DatabaseProvider.GetInstance().UpdateTopicModerated(tidList, moderated);
        }

        //TODO:没用的方法，早晚给你拿下
        /// <summary>
        /// 将主题设置为隐藏主题
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns></returns>
        public static int UpdateTopicHide(int tid)
        {
            if (appDBCache)
                ITopicService.UpdateTopicReplyCount(tid);

            return DatabaseProvider.GetInstance().UpdateTopicHide(tid);
        }

        /// <summary>
        /// 得到当前版块内正常(未关闭)主题总数
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <returns>主题总数</returns>
        public static int GetTopicCount(int fid)
        {
            return DatabaseProvider.GetInstance().GetTopicCount(fid);
        }

        /// <summary>
        /// 得到当前版块内(包括子版)正常(未关闭)主题总数
        /// </summary>
        /// <param name="subfidList">子版块列表</param>
        /// <returns>主题总数</returns>
        public static int GetTopicCountOfForumWithSub(string subfidList)
        {
            return DatabaseProvider.GetInstance().GetTopicCountOfForumWithSub(subfidList);
        }

        /// <summary>
        /// 得到当前版块内主题总数
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <param name="includeClosedTopic">是否包含关闭的主题</param>
        /// <param name="condition">查询条件</param>
        /// <returns>主题总数</returns>
        public static int GetTopicCount(int fid, bool includeClosedTopic, string condition)
        {
            return DatabaseProvider.GetInstance().GetTopicCount(fid, includeClosedTopic ? -1 : 0, condition);
        }

        /// <summary>
        /// 得到符合条件的主题总数
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>主题总数</returns>
        public static int GetTopicCount(string condition)
        {
            return DatabaseProvider.GetInstance().GetTopicCount(condition);
        }

        /// <summary>
        /// 判断帖子列表是否都在当前板块
        /// </summary>
        /// <param name="tidList">帖子id列表</param>
        /// <param name="fid">版块id</param>
        /// <returns></returns>
        public static int GetTopicCountInForumAndTopicIdList(string tidList, int fid)
        {
            if (!Utils.IsNumericList(tidList))
            {
                return 0;
            }
            return DatabaseProvider.GetInstance().GetTopicCountInForumAndTopicIdList(tidList, fid);
        }

        /// <summary>
        /// 获得主题列表
        /// </summary>
        /// <param name="fid">板块ID</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="tpp">每页主题数</param>
        /// <returns>主题列表</returns>
        public static DataTable GetTopicList(int fid, int pageIndex, int tpp)
        {
            return DatabaseProvider.GetInstance().GetTopicList(fid, pageIndex, tpp);
        }

        /// <summary>
        /// 按照用户Id获取其回复过的主题列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public static Discuz.Common.Generic.List<TopicInfo> GetTopicListByReplyUserId(int userId, int pageIndex, int pageSize)
        {
            Discuz.Common.Generic.List<TopicInfo> list = new Discuz.Common.Generic.List<TopicInfo>();

            if (pageIndex <= 0)
            {
                return list;
            }
            IDataReader reader = DatabaseProvider.GetInstance().GetTopicsByReplyUserId(userId, pageIndex, pageSize);

            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(LoadSingleTopicInfo(reader));
                }
                reader.Close();
            }
            return list;
        }

        /// <summary>
        /// 获取置顶主题列表
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="tids">置顶主题Id列表</param>
        /// <returns></returns>
        public static Discuz.Common.Generic.List<TopicInfo> GetTopTopicList(int fid, int pageSize, int pageIndex, string tidList)
        {
            if (!Utils.IsNumericList(tidList))
                return new List<TopicInfo>();

            Discuz.Common.Generic.List<TopicInfo> list = new Discuz.Common.Generic.List<TopicInfo>();
            if (appDBCache)
                list = ITopicService.GetTopTopicList(fid, pageSize, pageIndex, tidList);

            if (list.Count == 0)
            {
                IDataReader reader = DatabaseProvider.GetInstance().GetTopTopics(fid, pageSize, pageIndex, tidList);
                while (reader.Read())
                {
                    list.Add(LoadSingleTopicInfo(reader));
                }
                reader.Close();
            }
            return list;
        }

        /// <summary>
        /// 获得一般主题信息列表
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <param name="pageSize">每页显示主题数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="startNumber">置顶帖数量</param>
        /// <param name="condition">条件</param>
        /// <returns>主题信息列表</returns>
        public static Discuz.Common.Generic.List<TopicInfo> GetTopicList(int fid, int pageSize, int pageIndex, int startNumber, string condition)
        {
            Discuz.Common.Generic.List<TopicInfo> list = new Discuz.Common.Generic.List<TopicInfo>();
            if (string.IsNullOrEmpty(condition) && appDBCache)
            {
                list = ITopicService.GetTopicList(fid, pageSize, pageIndex, startNumber);
                if (list.Count > 0) return list;
            }

            IDataReader reader = DatabaseProvider.GetInstance().GetTopics(fid, pageSize, pageIndex, startNumber, condition);
            while (reader.Read())
            {
                list.Add(LoadSingleTopicInfo(reader));
            }
            reader.Close();
            return list;
        }

        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <param name="pageSize">每个分页的主题数</param>
        /// <param name="pageIndex">分页页数</param>
        /// <param name="startNumber">置顶帖数量</param>
        /// <param name="condition">条件</param>
        /// <param name="orderFields">排序字段,以","间隔</param>
        /// <param name="sortType">升/降序,0为升序,非0为降序</param>
        /// <returns>主题列表</returns>
        public static Discuz.Common.Generic.List<TopicInfo> GetTopicList(int fid, int pageSize, int pageIndex, int startNumber, string condition, string orderFields, int sortType)
        {
            Discuz.Common.Generic.List<TopicInfo> list = new Discuz.Common.Generic.List<TopicInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetTopicsByDate(fid, pageSize, pageIndex, startNumber, condition, orderFields, sortType);
            while (reader.Read())
            {
                list.Add(LoadSingleTopicInfo(reader));
            }
            reader.Close();

            return list;
        }

        public static List<TopicInfo> GetTopicListByViewsOrReplies(int fid, int pageSize, int pageIndex, int startNumber, string condition, string orderFields, int sortType)
        {
            Discuz.Common.Generic.List<TopicInfo> list = new Discuz.Common.Generic.List<TopicInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetTopicsByViewsOrReplies(fid, pageSize, pageIndex, startNumber, condition, orderFields, sortType);
            while (reader.Read())
            {
                list.Add(LoadSingleTopicInfo(reader));
            }
            reader.Close();

            return list;
        }

        /// <summary>
        /// 对符合新帖,精华帖的页面显示进行查询的函数
        /// </summary>
        /// <param name="pageSize">每个分页的主题数</param>
        /// <param name="pageIndex">分页页数</param>
        /// <param name="startNumber">置顶帖数量</param>
        /// <param name="condition">条件</param>
        /// <param name="orderFields">排序字段,以","间隔</param>
        /// <param name="sortType">升/降序,0为升序,非0为降序</param>
        /// <returns>主题列表</returns>
        public static Discuz.Common.Generic.List<TopicInfo> GetTopicListByCondition(int pageSize, int pageIndex, int startNumber, string condition, string orderFields, int sortType)
        {
            Discuz.Common.Generic.List<TopicInfo> list = new Discuz.Common.Generic.List<TopicInfo>();
            IDataReader reader = null;
            if (orderFields == "")
                reader = DatabaseProvider.GetInstance().GetTopicsByType(pageSize, pageIndex, startNumber, condition, sortType);
            else
                reader = DatabaseProvider.GetInstance().GetTopicsByTypeDate(pageSize, pageIndex, startNumber, condition, orderFields, sortType);

            while (reader.Read())
            {
                list.Add(LoadSingleTopicInfo(reader));
            }
            reader.Close();

            return list;
        }

        /// <summary>
        /// 获取需要审核的主题
        /// </summary>
        /// <param name="fidList">版块ID列表</param>
        /// <param name="tpp">每页主题数</param>
        /// <param name="pageId">页数</param>
        /// <param name="filter">displayorder过滤器</param>
        /// <returns></returns>
        public static Discuz.Common.Generic.List<TopicInfo> GetUnauditNewTopic(string fidList, int tpp, int pageId, int filter)
        {
            Discuz.Common.Generic.List<TopicInfo> list = new Discuz.Common.Generic.List<TopicInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetUnauditNewTopic(fidList, tpp, pageId, filter);
            while (reader.Read())
            {
                list.Add(LoadSingleTopicInfo(reader));
            }
            reader.Close();
            return list;
        }

        /// <summary>
        /// 获取需要关注的列表的数量
        /// </summary>
        /// <param name="fidlist">版块ID列表g</param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns></returns>
        public static int GetAttentionTopicCount(string fidList, string keyword)
        {
            return DatabaseProvider.GetInstance().GetAttentionTopicCount(fidList, keyword);
        }

        /// <summary>
        /// 获得版块最后回复主题ID
        /// </summary>
        /// <param name="forumInfo"></param>
        /// <param name="visibleForum"></param>
        /// <returns></returns>
        public static int GetLastPostTid(ForumInfo forumInfo, string visibleForum)
        {
            return DatabaseProvider.GetInstance().GetLastPostTid(forumInfo, visibleForum);
        }

        /// <summary>
        /// 获取使用同一tag的主题列表
        /// </summary>
        /// <param name="tagid">TagId</param>
        /// <returns></returns>
        public static int GetTopicCountByTagId(int tagId)
        {
            return DatabaseProvider.GetInstance().GetTopicsCountByTag(tagId);
        }

        /// <summary>
        /// 获取使用同一tag的主题列表
        /// </summary>
        /// <param name="tagId">TagId</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public static List<TopicInfo> GetTopicListByTagId(int tagId, int pageIndex, int pageSize)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetTopicListByTag(tagId, pageIndex, pageSize);

            List<TopicInfo> topics = new List<TopicInfo>();

            while (reader.Read())
            {
                TopicInfo topic = new TopicInfo();
                topic.Tid = Utils.StrToInt(reader["tid"], 0);
                topic.Title = reader["title"].ToString();
                topic.Poster = reader["poster"].ToString();
                topic.Posterid = Utils.StrToInt(reader["posterid"], -1);
                topic.Fid = Utils.StrToInt(reader["fid"], 0);
                topic.Postdatetime = reader["postdatetime"].ToString();
                topic.Replies = Utils.StrToInt(reader["replies"], 0);
                topic.Views = Utils.StrToInt(reader["views"], 0);
                topic.Lastposter = reader["lastposter"].ToString();
                topic.Lastposterid = Utils.StrToInt(reader["lastposterid"], -1);
                topic.Lastpost = reader["lastpost"].ToString();
                topics.Add(topic);
            }
            reader.Close();

            return topics;
        }

        /// <summary>
        /// 通过待验证的主题
        /// </summary>
        /// <param name="postTableId">当前帖子分表Id</param>
        /// <param name="tid">主题Id</param>
        public static void PassAuditNewTopic(string postTableId, int tid)
        {
            if (appDBCache)
                ITopicService.PassAuditNewTopic(tid);

            PassAuditNewTopic(postTableId, tid.ToString());
        }


        /// <summary>
        /// 通过待验证的主题
        /// </summary>
        /// <param name="postTableId">当前帖子分表Id</param>
        /// <param name="tid">主题Id</param>
        public static void PassAuditNewTopic(string postTableId, string tidList)
        {
            if (appDBCache)
                ITopicService.PassAuditNewTopic(tidList);

            DatabaseProvider.GetInstance().PassAuditNewTopic(Discuz.Config.BaseConfigs.GetTablePrefix + "posts" + postTableId, tidList);
        }

        /// <summary>
        /// 设置待验证的主题,包括通过,忽略,删除等操作
        /// </summary>
        /// <param name="postTableId">回复表ID</param>
        /// <param name="ignoreTidList">忽略的主题列表</param>
        /// <param name="validateTidList">验证通过的主题列表</param>
        /// <param name="deleteTidList">删除的主题列表</param>
        /// <param name="fidList">版块列表</param>
        public static void PassAuditNewTopic(string postTableId, string ignoreTidList, string validateTidList, string deleteTidList, string fidList)
        {
            if (appDBCache)
                ITopicService.PassAuditNewTopic(ignoreTidList, validateTidList, deleteTidList, fidList);

            DatabaseProvider.GetInstance().PassAuditNewTopic(postTableId, ignoreTidList, validateTidList, deleteTidList, fidList);
        }

        /// <summary>
        /// 获取版主有权限管理的主题数
        /// </summary>
        /// <param name="fidList">版块ID列表</param>
        /// <param name="tidList">主题ID列表</param>
        /// <returns></returns>
        public static int GetModTopicCountByTidList(string fidList, string tidList)
        {
            return DatabaseProvider.GetInstance().GetModTopicCountByTidList(fidList, tidList);
        }

        /// <summary>
        /// 获取需要被关注的主题列表
        /// </summary>
        /// <param name="fidList">版块列表ID</param>
        /// <param name="tpp">分页数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public static List<TopicInfo> GetAttentionTopics(string fidList, int tpp, int pageIndex, string keyword)
        {
            List<TopicInfo> list = new List<TopicInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetAttentionTopics(fidList, tpp, pageIndex, keyword);
            while (reader.Read())
            {
                list.Add(LoadSingleTopicInfo(reader));
            }
            reader.Close();
            return list;
        }

        /// <summary>
        /// 批量更新关注列表
        /// </summary>
        /// <param name="tidList">主题列表</param>
        /// <param name="attention">关注/取消关注(1/0)</param>
        public static void UpdateTopicAttentionByTidList(string tidList, int attention)
        {
            if (!Utils.IsNumericList(tidList))
                return;

            if (appDBCache)
                ITopicService.UpdateTopicAttentionByTidList(tidList, attention);

            DatabaseProvider.GetInstance().UpdateTopicAttentionByTidList(tidList, attention);
        }

        /// <summary>
        /// 批量更新关注列表
        /// </summary>
        /// <param name="fidList">版块列表</param>
        /// <param name="attention">关注/取消关注(1/0)</param>
        /// <param name="datetime">时间</param>
        public static void UpdateTopicAttentionByFidList(string fidList, int attention, string datetime)
        {
            if (!Utils.IsNumericList(fidList))
                return;

            if (appDBCache)
                ITopicService.UpdateTopicAttentionByFidList(fidList, attention, datetime);

            DatabaseProvider.GetInstance().UpdateTopicAttentionByFidList(fidList, attention, datetime);
        }


        /// <summary>
        /// 获取需要审核的主题数量
        /// </summary>
        /// <param name="fidlist">版块ID</param>
        /// <returns></returns>
        public static int GetUnauditNewTopicCount(string fidlist, int filter)
        {
            return DatabaseProvider.GetInstance().GetUnauditNewTopicCount(fidlist, filter);
        }

        /// <summary>
        /// 按照用户Id获取主题列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Discuz.Common.Generic.List<TopicInfo> GetTopicsByUserId(int userId, int pageIndex, int pageSize)
        {
            Discuz.Common.Generic.List<TopicInfo> list = new Discuz.Common.Generic.List<TopicInfo>();

            IDataReader reader = DatabaseProvider.GetInstance().GetTopicsByUserId(userId, pageIndex, pageSize);

            while (reader.Read())
            {
                list.Add(LoadSingleTopicInfo(reader));
            }
            reader.Close();

            return list;
        }

        /// <summary>
        /// 更新主题附件类型
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <param name="attType">附件类型,1普通附件,2为图片附件</param>
        /// <returns></returns>
        public static int UpdateTopicAttachmentType(int tid, int attType)
        {
            if (appDBCache)
                ITopicService.UpdateTopicAttachmentType(tid, attType);

            return DatabaseProvider.GetInstance().UpdateTopicAttachmentType(tid, attType);
        }

        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="topicList">要更新的主题id列表</param>
        /// <param name="fid">版块id</param>
        /// <param name="topicType">要绑定的主题类型</param>
        /// <returns></returns>
        public static int UpdateTopic(string topicList, int fid, int topicType)
        {
            if (appDBCache)
                ITopicService.UpdateTopic(topicList, fid, topicType);

            return DatabaseProvider.GetInstance().UpdateTopic(topicList, fid, topicType);
        }

        /// <summary>
        /// 删除关闭的主题
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <param name="topicList">要更新的主题id列表</param>
        /// <returns></returns>
        public static int UpdateTopic(int fid, string topicList)
        {
            if (appDBCache)
                ITopicService.UpdateTopic(fid, topicList);

            return DatabaseProvider.GetInstance().DeleteClosedTopics(fid, topicList);
        }

        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public static int DeleteTopic(int tid)
        {
            if (appDBCache)
                ITopicService.DeleteTopic(tid);

            return DatabaseProvider.GetInstance().DeleteTopic(tid);
        }


        /// <summary>
        /// 设置主帖
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="tid"></param>
        /// <param name="postid"></param>
        /// <param name="posttableid"></param>
        public static void SetPrimaryPost(string title, int tid, string[] postid)
        {

            DatabaseProvider.GetInstance().SetPrimaryPost(title, tid, postid, Data.PostTables.GetPostTableId(tid));
        }

        /// <summary>
        /// 更新主题最后发帖人Id
        /// </summary>
        /// <param name="tid"></param>
        public static void UpdateTopicLastPosterId(int tid)
        {
            DatabaseProvider.GetInstance().UpdateTopicLastPosterId(tid);

            if (appDBCache)
                ITopicService.UpdateTopicLastPosterId(tid);
        }

        public static string GetTopicCountCondition(out string type, string gettype, int getnewtopic)
        {
            return DatabaseProvider.GetInstance().GetTopicCountCondition(out type, gettype, getnewtopic);
        }

        public static void DeleteTopicByPosterid(int uid)
        {
            if (appDBCache)
                ITopicService.DeleteTopicByPosterid(uid);

            DatabaseProvider.GetInstance().DeleteTopicByPosterid(uid);
        }

        /// <summary>
        /// 获取管理日志被操作的Tid字符串
        /// </summary>
        /// <param name="postDateTime"></param>
        /// <returns></returns>
        public static DataTable GetTidForModeratorManageLogByPostDateTime(DateTime postDateTime)
        {
            return DatabaseProvider.GetInstance().GetModeratorLogByPostDate(postDateTime);
        }

        /// <summary>
        /// 获取主题所属板块Id
        /// </summary>
        /// <param name="tidlist">主题ID列表</param>
        /// <returns></returns>
        public static DataTable GetTopicFidByTid(string tidlist)
        {
            return DatabaseProvider.GetInstance().GetTopicFidByTid(tidlist);
        }

        /// <summary>
        /// 获取主题Id列表
        /// </summary>
        /// <param name="tidlist">主题列表</param>
        /// <param name="fid">版块ID</param>
        /// <returns></returns>
        public static DataTable GetTopicTidByFid(string tidlist, int fid)
        {
            return DatabaseProvider.GetInstance().GetTopicTidByFid(tidlist, fid);
        }

        /// <summary>
        /// 清除主题分类
        /// </summary>
        /// <param name="typeid">主题分类Id</param>
        public static void ClearTopicType(int typeid)
        {
            if (appDBCache)
                ITopicService.ClearTopicType(typeid);

            DatabaseProvider.GetInstance().ClearTopicTopicType(typeid);
        }

        /// <summary>
        /// 获取主题数
        /// </summary>
        /// <param name="postName">分表名称</param>
        /// <param name="forumId">版块Id</param>
        /// <param name="posterList">作者列表</param>
        /// <param name="keyList">关键字列表</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public static int GetTopicListCount(string postName, int forumId, string posterList, string keyList, string startDate, string endDate)
        {
            return DatabaseProvider.GetInstance().GetTopicListCountByCondition(postName, forumId, posterList, keyList, startDate, endDate);
        }

        /// <summary>
        /// 获取聚合首页热帖数
        /// </summary>
        /// <returns></returns>
        public static int GetHotTopicsCount(int fid, int timeBetween)
        {
            return DatabaseProvider.GetInstance().GetHotTopicsCount(fid, timeBetween);
        }

        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="postName">分表名称</param>
        /// <param name="forumId">版块Id</param>
        /// <param name="posterList">作者列表</param>
        /// <param name="keyList">关键字列表</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="currentPage">当前页号</param>
        /// <returns></returns>
        public static DataTable GetTopicList(string postName, int forumId, string posterList, string keyList, string startDate, string endDate, int pageSize, int currentPage)
        {
            return DatabaseProvider.GetInstance().GetTopicListByCondition(postName, forumId, posterList, keyList, startDate, endDate, pageSize, currentPage);
        }

        /// <summary>
        /// 获取聚合首页热列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetHotTopicsList(int pageSize, int pageIndex, int fid, string showType, int timeBetween)
        {
            return DatabaseProvider.GetInstance().GetHotTopicsList(pageSize, pageIndex, fid, showType, timeBetween);
        }

        /// <summary>
        /// 设置主题分类
        /// </summary>
        /// <param name="topiclist">主题ID列表</param>
        /// <param name="value">分类ID</param>
        /// <returns></returns>
        public static bool SetTypeid(string topiclist, int value)
        {
            if (appDBCache)
                ITopicService.SetTypeid(topiclist, value);

            return DatabaseProvider.GetInstance().SetTypeid(topiclist, value);
        }

        /// <summary>
        /// 设置主题属性
        /// </summary>
        /// <param name="topiclist">主题ID列表</param>
        /// <param name="value">主题属性</param>
        /// <returns></returns>
        public static bool SetDisplayorder(string topiclist, int value)
        {
            if (appDBCache)
                ITopicService.SetDisplayorder(topiclist, value);

            return DatabaseProvider.GetInstance().SetDisplayorder(topiclist, value);
        }

        /// <summary>
        /// 更新主题最后回复人
        /// </summary>
        /// <param name="uid">最后回复人的Uid</param>
        /// <param name="newUserName">最后回复人的新用户名</param>
        public static void UpdateTopicLastPoster(int uid, string newUserName)
        {
            if (appDBCache)
                ITopicService.UpdateTopicLastPoster(uid, newUserName);

            DatabaseProvider.GetInstance().UpdateTopicLastPoster(uid, newUserName);
        }

        /// <summary>
        /// 更新主题作者
        /// </summary>
        /// <param name="posterid">作者Id</param>
        /// <param name="poster">作者的新名称</param>
        public static void UpdateTopicPoster(int posterid, string poster)
        {
            if (appDBCache)
                ITopicService.UpdateTopicPoster(posterid, poster);

            DatabaseProvider.GetInstance().UpdateTopicPoster(posterid, poster);
        }

        /// <summary>
        /// 获取用户的最大与最小主题Id
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <returns></returns>
        public static IDataReader GetMaxAndMinTidByUid(int uid)
        {
            return DatabaseProvider.GetInstance().GetMaxAndMinTid(uid);

        }
        /// <summary>
        /// 按条件获取待审核主题列表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable GetAuditTopicList(string condition)
        {
            return DatabaseProvider.GetInstance().AuditTopicBind(condition);
        }
        /// <summary>
        /// 获取主题队列
        /// </summary>
        /// <param name="start_tid">起始主题ID</param>
        /// <param name="end_tid">结束主题ID</param>
        /// <returns></returns>
        public static IDataReader GetTopics(int start_tid, int end_tid)
        {
            return DatabaseProvider.GetInstance().GetTopics(start_tid, end_tid);
        }

        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postcount">帖子数</param>
        /// <param name="lastpostid">最后发帖人</param>
        /// <param name="lastpost">最后发帖时间</param>
        /// <param name="lastposterid">最后发帖人ID</param>
        /// <param name="poster">最后发帖人</param>
        public static void UpdateTopic(int tid, int postcount, int lastpostid, string lastpost, int lastposterid, string poster)
        {
            if (appDBCache)
                ITopicService.UpdateTopic(tid, postcount, lastpostid, lastpost, lastposterid, poster);

            DatabaseProvider.GetInstance().UpdateTopic(tid, postcount, lastpostid, lastpost, lastposterid, poster);
        }

        /// <summary>
        /// 得到论坛中主题总数;
        /// </summary>
        /// <returns>主题总数</returns>
        public static int GetTopicCount()
        {
            if (appDBCache)
                return ITopicService.GetTopicCount();

            return DatabaseProvider.GetInstance().GetTopicCount();
        }

        /// <summary>
        /// 获取TID列表
        /// </summary>
        /// <param name="statcount">获取总数</param>
        /// <param name="lasttid">最后TID</param>
        /// <returns></returns>
        public static IDataReader GetTopicTids(int statcount, int lasttid)
        {
            return DatabaseProvider.GetInstance().GetTopicTids(statcount, lasttid);
        }

        /// <summary>
        /// 获取未审核主题
        /// </summary>
        /// <returns></returns>
        //public static DataTable GetUnauditNewTopic()
        //{
        //    return DatabaseProvider.GetInstance().GetUnauditNewTopic();
        //}

        /// <summary>
        /// 更新我的主题
        /// </summary>
        public static void UpdateMyTopic()
        {
            DatabaseProvider.GetInstance().UpdateMyTopic();
        }

        /// <summary>
        /// 获取搜索主题条件
        /// </summary>
        /// <param name="fid">版块Id</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="displayOrder">显示序号</param>
        /// <param name="digest">精华</param>
        /// <param name="attachment">附件</param>
        /// <param name="poster">作者</param>
        /// <param name="lowerUpper">是否区分大小写</param>
        /// <param name="viewsMin">最小查看数</param>
        /// <param name="viewsMax">最大查看数</param>
        /// <param name="repliesMax">最大回复数</param>
        /// <param name="repliesMin">最小回复数</param>
        /// <param name="rate">售价</param>
        /// <param name="lastPost">最后回复</param>
        /// <param name="postDateTimeStart">起始日期</param>
        /// <param name="postDateTimeEnd">结束日期</param>
        /// <returns></returns>
        public static string GetSearchTopicsCondition(int fid, string keyWord, string displayOrder, string digest, string attachment, string poster,
            bool lowerUpper, string viewsMin, string viewsMax, string repliesMax, string repliesMin, string rate, string lastPost,
            DateTime postDateTimeStart, DateTime postDateTimeEnd)
        {
            return DatabaseProvider.GetInstance().SearchTopics(fid, keyWord, displayOrder, digest, attachment, poster, lowerUpper,
                viewsMin, viewsMax, repliesMax, repliesMin, rate, lastPost, postDateTimeStart, postDateTimeEnd);
        }

        /// <summary>
        /// 获取一定范围内的主题
        /// </summary>
        /// <param name="tagname">标签名称</param>
        /// <param name="from">板块</param>
        /// <param name="end"></param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static DataTable GetTopicNumber(string tagname, int from, int end, int type)
        {
            return DatabaseProvider.GetInstance().GetTopicNumber(tagname, from, end, type);
        }

        /// <summary>
        /// 按条件获取主题
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable GetTopicsByCondition(string condition)
        {
            return DatabaseProvider.GetInstance().GetTopicsByCondition(condition);
        }

        #region Private Methods

        /// <summary>
        /// 加载单个主题对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static TopicInfo LoadSingleTopicInfo(IDataReader reader)
        {
            //TODO:字段查询不同，改查询
            StringBuilder tablefield = new StringBuilder();
            tablefield.Append(",");
            foreach (DataRow dr in reader.GetSchemaTable().Rows)
            {
                tablefield.Append(dr["ColumnName"].ToString().ToLower() + ",");
            }

            TopicInfo topicInfo = new TopicInfo();
            topicInfo.Tid = TypeConverter.ObjectToInt(reader["tid"]);
            topicInfo.Fid = tablefield.ToString().IndexOf(",fid,") >= 0 ? TypeConverter.ObjectToInt(reader["fid"]) : 0;
            topicInfo.Iconid = TypeConverter.ObjectToInt(reader["iconid"]);
            topicInfo.Title = reader["title"].ToString();
            topicInfo.Typeid = TypeConverter.ObjectToInt(reader["typeid"]);
            topicInfo.Readperm = TypeConverter.ObjectToInt(reader["readperm"]);
            topicInfo.Price = TypeConverter.ObjectToInt(reader["price"]);
            topicInfo.Poster = reader["poster"].ToString();
            topicInfo.Posterid = TypeConverter.ObjectToInt(reader["posterid"]);
            topicInfo.Postdatetime = reader["postdatetime"].ToString();
            topicInfo.Lastpost = reader["lastpost"].ToString();
            topicInfo.Lastposter = reader["lastposter"].ToString();
            topicInfo.Lastposterid = TypeConverter.ObjectToInt(reader["LastposterID"]);
            topicInfo.Lastpostid = TypeConverter.ObjectToInt(reader["LastpostID"]);
            topicInfo.Views = TypeConverter.ObjectToInt(reader["views"]);
            topicInfo.Replies = TypeConverter.ObjectToInt(reader["replies"]);
            topicInfo.Displayorder = TypeConverter.ObjectToInt(reader["displayorder"]);
            topicInfo.Highlight = reader["highlight"].ToString();
            topicInfo.Digest = TypeConverter.ObjectToInt(reader["digest"]);
            topicInfo.Rate = TypeConverter.ObjectToInt(reader["rate"]);
            topicInfo.Hide = TypeConverter.ObjectToInt(reader["hide"]);
            topicInfo.Attachment = TypeConverter.ObjectToInt(reader["attachment"]);
            topicInfo.Moderated = tablefield.ToString().IndexOf(",moderated,") >= 0 ? TypeConverter.ObjectToInt(reader["moderated"]) : 0;
            topicInfo.Closed = TypeConverter.ObjectToInt(reader["closed"]);
            topicInfo.Magic = TypeConverter.ObjectToInt(reader["magic"]);
            topicInfo.Identify = tablefield.ToString().IndexOf(",identify,") >= 0 ? TypeConverter.ObjectToInt(reader["identify"]) : 0;
            topicInfo.Special = byte.Parse(reader["special"].ToString());
            topicInfo.Attention = tablefield.ToString().IndexOf(",attention,") >= 0 ? TypeConverter.ObjectToInt(reader["attention"]) : 0;

            return topicInfo;
        }
        #endregion
    }
}
