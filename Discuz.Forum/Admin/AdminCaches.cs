using System;
using System.Data;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config.Provider;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Cache;

namespace Discuz.Forum
{
    /// <summary>
    /// AdminCacheFactory 的摘要说明。
    /// </summary>
    public class AdminCaches
    {
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
            RemoveObject(string.Format(CacheKeys.FORUM_RSS_FORUM,fid));
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
        }


        /// <summary>
        /// 重新设置相应的主题列表
        /// </summary>
        /// <param name="fid"></param>
        public static void ReSetTopiclistByFid(string fid)
        {
            RemoveObject(string.Format(CacheKeys.FORUM_TOPIC_LIST_FID,fid));
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
            ReSetAdminGroupList();

            ReSetUserGroupList();

            ReSetModeratorList();

            ReSetAnnouncementList();

            ReSetSimplifiedAnnouncementList();

            ReSetForumListBoxOptions();

            ReSetSmiliesList();

            ReSetIconsList();

            ReSetCustomEditButtonList();

            ReSetConfig();

            ReSetScoreset();

            ReSetSiteUrls();

            ReSetStatistics();

            ReSetAttachmentTypeArray();

            ReSetTemplateListBoxOptionsCache();

            ReSetOnlineGroupIconList();

            ReSetForumLinkList();

            ReSetBanWordList();

            ReSetForumList();

            //ReSetOnlineUserTable();

            ReSetRss();

            ReSetRssXml();

            ReSetValidTemplateIDList();

            ReSetValidScoreName();

            ReSetMedalsList();

            ReSetDBlinkAndTablePrefix();

            ReSetAllPostTableName();

            ReSetLastPostTableName();

            ReSetAdsList();
            ReSetStatisticsSearchtime();
            ReSetStatisticsSearchcount();
            ReSetCommonAvatarList();
            ReSetJammer();
            ReSetMagicList();
            ReSetScorePaySet();
            ReSetPostTableInfo();
            ReSetDigestTopicList(16);
            ReSetHotTopicList(16, 30);
            ReSetRecentTopicList(16);

            ResetAlbumCategory();

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
            string filename = Discuz.Config.DefaultConfigFileManager.ConfigFilePath;//Utils.GetMapPath("/DNT.config");
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
            {
                ;
            }
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
                {
                    ;
                }
            }
            return false;
        }
    }
}
