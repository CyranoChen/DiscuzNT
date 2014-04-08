using System;
using System.Data;
using Discuz.Common;

using Discuz.Common.Generic;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Aggregation;
using Discuz.Plugin.Album;

namespace Discuz.Web
{
    /// <summary>
    /// 聚合首页
    /// </summary>
    public class website : PageBase
    {
        /// <summary>
        /// 论坛推荐主题
        /// </summary>
        public PostInfo[] postlist = AggregationFacade.ForumAggregation.GetPostListFromFile("Website");
        /// <summary>
        /// 论坛推荐主题排序类型
        /// </summary>
        public TopicOrderType topicordertype = AggregationFacade.ForumAggregation.GetForumAggregationTopicListOrder();
        /// <summary>
        /// 论坛聚合主题
        /// </summary>
        public DataTable topiclist = new DataTable();
        //public DataTable topiclist = AggregationFacade.ForumAggregation.GetForumTopicList();
        /// <summary>
        /// 用户聚合数据
        /// </summary>
        public DataTable userlist;
        /// <summary>
        /// 聚合空间数据
        /// </summary>
        public SpaceConfigInfoExt[] spaceconfigs = AggregationFacade.SpaceAggregation.GetSpaceListFromFile("Website");
        /// <summary>
        /// 聚合相册数据
        /// </summary>
        public AlbumInfo[] albuminfos;
        /// <summary>
        /// 聚合日志数据
        /// </summary>
        public SpaceShortPostInfo[] spacepostlist = AggregationFacade.SpaceAggregation.GetSpacePostList("Website");
        /// <summary>
        /// 最近更新的空间
        /// </summary>
        public DataTable recentupdatespaceList;
        /// <summary>
        /// 友情链接数量
        /// </summary>
        public int forumlinkcount;
        /// <summary>
        /// 公告数量
        /// </summary>
        public int announcementcount = 0;
        /// <summary>
        /// 公告列表
        /// </summary>
        public DataTable announcementlist;
        /// <summary>
        /// 图片轮显数据
        /// </summary>
        public string rotatepicdata = AggregationFacade.BaseAggregation.GetRotatePicData();
        /// <summary>
        /// 聚合图片信息
        /// </summary>
        public PhotoAggregationInfo photoconfig = AggregationFacade.PhotoAggregation.GetPhotoAggregationInfo();
        /// <summary>
        /// 推荐相册列表
        /// </summary>
        public List<AlbumInfo> recommendalbumlist = AggregationFacade.AlbumAggregation.GetRecommandAlbumList("Website");
        /// <summary>
        /// 相册分类列表
        /// </summary>
        public Discuz.Common.Generic.List<AlbumCategoryInfo> albumcategorylist;
        /// <summary>
        /// 图片列表
        /// </summary>
        public Discuz.Common.Generic.List<PhotoInfo> photolist;
        /// <summary>
        /// 焦点相册列表
        /// </summary>
        public Discuz.Common.Generic.List<AlbumInfo> albumlist;

        public ForumAggregationData forumagg = Discuz.Aggregation.AggregationFacade.ForumAggregation;

        public AlbumAggregationData albumagg = Discuz.Aggregation.AggregationFacade.AlbumAggregation;

        public SpaceAggregationData spaceagg = Discuz.Aggregation.AggregationFacade.SpaceAggregation;

        public GoodsAggregationData goodsagg = Discuz.Aggregation.AggregationFacade.GoodsAggregation;

        public string spacerotatepicdata = AggregationFacade.SpaceAggregation.GetRotatePicData();

        public Discuz.Common.Generic.List<PhotoInfo> recommendphotolist = AggregationFacade.AlbumAggregation.GetRecommandPhotoList("Albumindex");
        /// <summary>
        /// 总主题数
        /// </summary>
        public int totaltopic;
        /// <summary>
        /// 总帖子数
        /// </summary>
        public int totalpost;
        /// <summary>
        /// 总用户数
        /// </summary>
        public int totalusers;
        /// <summary>
        /// 今日帖数
        /// </summary>
        public int todayposts;
        /// <summary>
        /// 昨日帖数
        /// </summary>
        public int yesterdayposts;
        /// <summary>
        /// 最高日帖数
        /// </summary>
        public int highestposts;
        /// <summary>
        /// 最高发帖日
        /// </summary>
        public string highestpostsdate;
        /// <summary>
        /// 最新注册的用户名
        /// </summary>
        public string lastusername;
        /// <summary>
        /// 最新注册的用户Id
        /// </summary>
        public int lastuserid;
        /// <summary>
        /// 总在线用户数
        /// </summary>
        public int totalonline;
        /// <summary>
        /// 总在线注册用户数
        /// </summary>
        public int totalonlineuser;
        /// <summary>
        /// 总在线游客数
        /// </summary>
        public int totalonlineguest;
        /// <summary>
        /// 总在线隐身用户数
        /// </summary>
        public int totalonlineinvisibleuser;
        /// <summary>
        /// 最高在线用户数
        /// </summary>
        public string highestonlineusercount;
        /// <summary>
        /// 最高在线用户数发生时间
        /// </summary>
        public string highestonlineusertime;
        /// <summary>
        /// 最新空间评论列表
        /// </summary>
        public DataTable topspacecomments;
        /// <summary>
        /// 标签列表
        /// </summary>
        public TagInfo[] taglist;

        public GoodsinfoCollection goodscoll = new GoodsinfoCollection();


        /// <summary>
        /// 友情链接列表
        /// </summary>
        public DataTable forumlinklist = Caches.GetForumLinkList();
        /// <summary>
        /// 对联广告
        /// </summary>
        public string doublead;
        /// <summary>
        /// 浮动广告
        /// </summary>
        public string floatad;
        /// <summary>
        /// 用户发帖排行
        /// </summary>
        public List<UserPostCountInfo> userPostCountInfoList = new List<UserPostCountInfo>();
        /// <summary>
        /// 可用的扩展积分名称列表
        /// </summary>
        public string score1, score2, score3, score4, score5, score6, score7, score8;
        /// <summary>
        /// 可用的扩展积分名称列表
        /// </summary>
        public string[] score;
        protected override void ShowPage()
        {
            pagetitle = "首页";

            if (config.Rssstatus == 1)
                AddLinkRss("tools/rss.aspx", "最新主题");

            // 得到公告
            announcementlist = Announcements.GetSimplifiedAnnouncementList(nowdatetime, "2999-01-01 00:00:00");
            if (announcementlist != null)
                announcementcount = announcementlist.Rows.Count;

            // 友情链接
            forumlinkcount = forumlinklist.Rows.Count;

            Forums.GetForumIndexCollection(config.Hideprivate, usergroupid, config.Moddisplay, out totaltopic, out totalpost, out todayposts);

            // 获得统计信息
            totalusers = TypeConverter.StrToInt(Statistics.GetStatisticsRowItem("totalusers"));
            lastusername = Statistics.GetStatisticsRowItem("lastusername");
            lastuserid = TypeConverter.StrToInt(Statistics.GetStatisticsRowItem("lastuserid"));
            yesterdayposts = TypeConverter.StrToInt(Statistics.GetStatisticsRowItem("yesterdayposts"));
            highestposts = TypeConverter.StrToInt(Statistics.GetStatisticsRowItem("highestposts"));
            highestpostsdate = Statistics.GetStatisticsRowItem("highestpostsdate").ToString().Trim();
            if (todayposts > highestposts)
            {
                highestposts = todayposts;
                highestpostsdate = DateTime.Now.ToString("yyyy-M-d");
            }
            totalonline = onlineusercount;
            OnlineUsers.GetOnlineUserCollection(out totalonline, out totalonlineguest, out totalonlineuser, out totalonlineinvisibleuser);

            highestonlineusercount = Statistics.GetStatisticsRowItem("highestonlineusercount");
            highestonlineusertime = Statistics.GetStatisticsRowItem("highestonlineusertime");

            if (userid != -1)
            {
                score = Scoresets.GetValidScoreName();
                ShortUserInfo user = Users.GetShortUserInfo(userid);
                score1 = ((decimal)user.Extcredits1).ToString();
                score2 = ((decimal)user.Extcredits2).ToString();
                score3 = ((decimal)user.Extcredits3).ToString();
                score4 = ((decimal)user.Extcredits4).ToString();
                score5 = ((decimal)user.Extcredits5).ToString();
                score6 = ((decimal)user.Extcredits6).ToString();
                score7 = ((decimal)user.Extcredits7).ToString();
                score8 = ((decimal)user.Extcredits8).ToString();
            }
            //相册
            
            if (config.Enablealbum == 1 && AlbumPluginProvider.GetInstance() != null)
                albumcategorylist = AlbumPluginProvider.GetInstance().GetAlbumCategory();

            if (config.Enablespace == 1 && AggregationFacade.SpaceAggregation.GetSpaceTopComments() != null)
                topspacecomments = AggregationFacade.SpaceAggregation.GetSpaceTopComments();

            taglist = (config.Enabletag == 1 ? ForumTags.GetCachedHotForumTags(config.Hottagcount) : new TagInfo[0]);
            doublead = Advertisements.GetDoubleAd("indexad", 0);
            floatad = Advertisements.GetFloatAd("indexad", 0);
        }

    }
}