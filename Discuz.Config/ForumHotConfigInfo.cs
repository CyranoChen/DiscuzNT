using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Config
{
    [Serializable]
    public class ForumHotConfigInfo : IConfigInfo
    {
        private bool _enable = false;
        private ForumHotItemInfoConllection _forumHotCollection = new ForumHotItemInfoConllection();

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        /// <summary>
        /// 论坛热点集合
        /// </summary>
        public ForumHotItemInfoConllection ForumHotCollection
        {
            get { return _forumHotCollection; }
            set { _forumHotCollection = value; }
        }

        public static ForumHotConfigInfo CreateInstance()
        {
            ForumHotConfigInfo configInfo = new ForumHotConfigInfo();
            configInfo.Enable = true;
            //定义默认数据 最新主题
            ForumHotItemInfo newTopicSetting = new ForumHotItemInfo();
            newTopicSetting.Id = 1;
            newTopicSetting.Name = "最新主题";
            newTopicSetting.Datatype = "topics";
            newTopicSetting.Sorttype = "PostDateTime";
            newTopicSetting.Topictitlelength = 20;
            newTopicSetting.Forumnamelength = 10;
            newTopicSetting.Dataitemcount = 13;
            configInfo.ForumHotCollection.Add(newTopicSetting);

            //定义默认数据 热门主题
            ForumHotItemInfo hotTopicSetting = new ForumHotItemInfo();
            hotTopicSetting.Id = 2;
            hotTopicSetting.Name = "热门主题";
            hotTopicSetting.Datatype = "topics";
            hotTopicSetting.Sorttype = "Views";
            hotTopicSetting.Topictitlelength = 20;
            hotTopicSetting.Forumnamelength = 10;
            hotTopicSetting.Dataitemcount = 13;
            configInfo.ForumHotCollection.Add(hotTopicSetting);

            //定义默认数据 精华主题
            ForumHotItemInfo digestTopicSetting = new ForumHotItemInfo();
            digestTopicSetting.Id = 3;
            digestTopicSetting.Name = "精华主题";
            digestTopicSetting.Datatype = "topics";
            digestTopicSetting.Sorttype = "Digest";
            digestTopicSetting.Topictitlelength = 20;
            digestTopicSetting.Forumnamelength = 10;
            digestTopicSetting.Dataitemcount = 13;
            configInfo.ForumHotCollection.Add(digestTopicSetting);

            //定义默认数据 用户发帖排行
            ForumHotItemInfo userTopicRankSetting = new ForumHotItemInfo();
            userTopicRankSetting.Id = 4;
            userTopicRankSetting.Name = "用户发帖排行";
            userTopicRankSetting.Datatype = "users";
            userTopicRankSetting.Sorttype = "posts";
            userTopicRankSetting.Dataitemcount = 20;
            userTopicRankSetting.Datatimetype = "posts";
            configInfo.ForumHotCollection.Add(userTopicRankSetting);

            //定义默认数据 版块发帖排行
            ForumHotItemInfo forumTopicRankSetting = new ForumHotItemInfo();
            forumTopicRankSetting.Id = 5;
            forumTopicRankSetting.Name = "版块发帖排行";
            forumTopicRankSetting.Datatype = "forums";
            forumTopicRankSetting.Sorttype = "posts";
            forumTopicRankSetting.Forumnamelength = 20;
            forumTopicRankSetting.Dataitemcount = 20;
            configInfo.ForumHotCollection.Add(forumTopicRankSetting);

            //定义默认数据 热点图片
            ForumHotItemInfo hotPicSetting = new ForumHotItemInfo();
            hotPicSetting.Id = 6;
            hotPicSetting.Name = "热点图片";
            hotPicSetting.Datatype = "pictures";
            hotPicSetting.Sorttype = "aid";
            hotPicSetting.Dataitemcount = 5;
            configInfo.ForumHotCollection.Add(hotPicSetting);

            return configInfo;
        }
    }
}
