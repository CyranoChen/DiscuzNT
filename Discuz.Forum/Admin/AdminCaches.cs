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
    /// AdminCacheFactory ��ժҪ˵����
    /// </summary>
    public class AdminCaches
    {
        private static void RemoveObject(string key)
        {
            DNTCache.GetCacheService().RemoveObject(key);
        }

        /// <summary>
        /// �������ù�������Ϣ
        ///</summary>
        public static void ReSetAdminGroupList()
        {
            RemoveObject(CacheKeys.FORUM_ADMIN_GROUP_LIST);
        }

        /// <summary>
        /// ���������û�����Ϣ
        ///</summary>
        public static void ReSetUserGroupList()
        {
            RemoveObject(CacheKeys.FORUM_USER_GROUP_LIST);
        }

        /// <summary>
        /// �������ð�����Ϣ
        ///</summary>
        public static void ReSetModeratorList()
        {
            RemoveObject(CacheKeys.FORUM_MODERATOR_LIST);
        }

        /// <summary>
        /// ��������ָ��ʱ���ڵĹ����б�
        ///</summary>
        public static void ReSetAnnouncementList()
        {
            RemoveObject(CacheKeys.FORUM_ANNOUNCEMENT_LIST);
        }

        /// <summary>
        /// �������õ�һ������
        ///</summary>
        public static void ReSetSimplifiedAnnouncementList()
        {
            RemoveObject(CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST);
        }

        /// <summary>
        /// �������ð�������б�
        ///</summary>
        public static void ReSetForumListBoxOptions()
        {
            RemoveObject(CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS);
        }

        /// <summary>
        /// �������ñ���
        ///</summary>
        public static void ReSetSmiliesList()
        {
            RemoveObject(CacheKeys.FORUM_UI_SMILIES_LIST);
            RemoveObject(CacheKeys.FORUM_UI_SMILIES_LIST_WITH_INFO);
        }

        /// <summary>
        /// ������������ͼ��
        ///</summary>
        public static void ReSetIconsList()
        {
            RemoveObject(CacheKeys.FORUM_UI_ICONS_LIST);
        }

        /// <summary>
        /// �����û��Զ����ǩ
        ///</summary>
        public static void ReSetCustomEditButtonList()
        {
            RemoveObject(CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_LIST);
            RemoveObject(CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_INFO);
        }

        /// <summary>
        /// ����������̳��������
        ///</summary>
        public static void ReSetConfig()
        {
            RemoveObject(CacheKeys.FORUM_SETTING);
        }

        /// <summary>
        /// ����������̳����
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
        /// �������õ�ַ���ձ�
        ///</summary>
        public static void ReSetSiteUrls()
        {
            RemoveObject(CacheKeys.FORUM_URLS);
        }

        /// <summary>
        /// ����������̳ͳ����Ϣ
        ///</summary>
        public static void ReSetStatistics()
        {
            RemoveObject(CacheKeys.FORUM_STATISTICS);
        }


        /// <summary>
        /// ��������ϵͳ����ĸ������ͺʹ�С
        ///</summary>
        public static void ReSetAttachmentTypeArray()
        {
            RemoveObject(CacheKeys.FORUM_FORUM_SETTING_ATTACHMENT_TYPE);
        }

        /// <summary>
        /// ģ���б��������html
        ///</summary>
        public static void ReSetTemplateListBoxOptionsCache()
        {
            RemoveObject(CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS_FOR_FORUMINDEX);
            RemoveObject(CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS);
        }

        /// <summary>
        /// �������������û��б�ͼ��
        /// </summary>
        public static void ReSetOnlineGroupIconList()
        {
            RemoveObject(CacheKeys.FORUM_UI_ONLINE_ICON_LIST);
            RemoveObject(CacheKeys.FORUM_ONLINE_ICON_TABLE);
        }

        /// <summary>
        /// �����������������б�
        /// </summary>
        public static void ReSetForumLinkList()
        {
            RemoveObject(CacheKeys.FORUM_FORUM_LINK_LIST);
        }


        /// <summary>
        /// �����������ֹ����б�
        /// </summary>
        public static void ReSetBanWordList()
        {
            RemoveObject(CacheKeys.FORUM_BAN_WORD_LIST);
        }


        /// <summary>
        /// ��̳�б�
        /// </summary>
        public static void ReSetForumList()
        {
            RemoveObject(CacheKeys.FORUM_FORUM_LIST);
        }


        /// <summary>
        /// �����û���Ϣ
        /// </summary>
        public static void ReSetOnlineUserTable()
        {
            ;
        }

        /// <summary>
        /// ��̳����RSS��ָ�����RSS
        /// </summary>
        public static void ReSetRss()
        {
            RemoveObject(CacheKeys.FORUM_RSS);
        }


        /// <summary>
        /// ָ�����RSS
        /// </summary>
        /// <param name="fid">���Id</param>
        public static void ReSetForumRssXml(int fid)
        {
            RemoveObject(string.Format(CacheKeys.FORUM_RSS_FORUM,fid));
        }


        /// <summary>
        /// ��̳����RSS
        /// </summary>
        public static void ReSetRssXml()
        {
            RemoveObject(CacheKeys.FORUM_RSS_INDEX);
        }


        /// <summary>
        /// ģ��id�б�
        /// </summary>
        public static void ReSetValidTemplateIDList()
        {
            RemoveObject(CacheKeys.FORUM_TEMPLATE_ID_LIST);
        }


        /// <summary>
        /// ��Ч���û�����չ�ֶ�
        /// </summary>
        public static void ReSetValidScoreName()
        {
            RemoveObject(CacheKeys.FORUM_VALID_SCORE_NAME);
        }


        /// <summary>
        /// ����ѫ���б�
        /// </summary>
        public static void ReSetMedalsList()
        {
            RemoveObject(CacheKeys.FORUM_UI_MEDALS_LIST);
        }

        /// <summary>
        /// �����������Ӵ������ݱ�ǰ׺
        /// </summary>
        public static void ReSetDBlinkAndTablePrefix()
        {
            RemoveObject(CacheKeys.FORUM_BASE_SETTING_DBCONNECTSTRING);
            RemoveObject(CacheKeys.FORUM_BASE_SETTING_TABLE_PREFIX);
        }

        /// <summary>
        /// �����������ӱ�
        /// </summary>
        public static void ReSetLastPostTableName()
        {
            RemoveObject(CacheKeys.FORUM_LAST_POST_TABLE_NAME);
        }


        /// <summary>
        /// ���������б�
        /// </summary>
        public static void ReSetAllPostTableName()
        {
            RemoveObject(CacheKeys.FORUM_POST_TABLE_NAME);
        }

        /// <summary>
        /// �������б�
        /// </summary>
        public static void ReSetAdsList()
        {
            RemoveObject(CacheKeys.FORUM_ADVERTISEMENTS);
        }

        /// <summary>
        /// ���������û���һ��ִ������������ʱ��
        /// </summary>
        public static void ReSetStatisticsSearchtime()
        {
            RemoveObject(CacheKeys.FORUM_STATISTICS_SEARCHTIME);
        }


        /// <summary>
        /// ���������û���һ�����������Ĵ���
        /// </summary>
        public static void ReSetStatisticsSearchcount()
        {
            RemoveObject(CacheKeys.FORUM_STATISTICS_SEARCHCOUNT);
        }


        /// <summary>
        /// ���������û�ͷ���б�
        /// </summary>
        public static void ReSetCommonAvatarList()
        {
            RemoveObject(CacheKeys.FORUM_COMMON_AVATAR_LIST);
        }

        /// <summary>
        /// �������ø������ַ���
        /// </summary>
        public static void ReSetJammer()
        {
            RemoveObject(CacheKeys.FORUM_UI_JAMMER);
        }

        /// <summary>
        /// ��������ħ���б�
        /// </summary>
        public static void ReSetMagicList()
        {
            RemoveObject(CacheKeys.FORUM_MAGIC_LIST);
        }

        /// <summary>
        /// �������öһ����ʵĿɽ��׻��ֲ���
        /// </summary>
        public static void ReSetScorePaySet()
        {
            RemoveObject(CacheKeys.FORUM_SCORE_PAY_SET);
        }


        /// <summary>
        /// �������õ�ǰ���ӱ������Ϣ
        /// </summary>
        public static void ReSetPostTableInfo()
        {
            RemoveObject(CacheKeys.FORUM_POST_TABLE_NAME);
            RemoveObject(CacheKeys.FORUM_LAST_POST_TABLE_NAME);
        }


        /// <summary>
        /// ����������Ӧ�������б�
        /// </summary>
        /// <param name="fid"></param>
        public static void ReSetTopiclistByFid(string fid)
        {
            RemoveObject(string.Format(CacheKeys.FORUM_TOPIC_LIST_FID,fid));
        }



        /// <summary>
        /// ��������ȫ����龫�������б�
        /// </summary>
        /// <param name="count">��������</param>
        public static void ReSetDigestTopicList(int count)
        {
            ReSetFocusTopicList(count, -1, 0, TopicTimeType.All, TopicOrderType.ID, true);
        }

        //��������ָ����龫�������б�[��δ����]
        public static void ReSetDigestTopicList(int count, int fid, TopicTimeType timetype, TopicOrderType ordertype)
        {
            ReSetFocusTopicList(count, -1, fid, timetype, ordertype, true);
        }

        /// <summary>
        /// ��������ȫ��������������б�
        /// </summary>
        /// <param name="count"></param>
        /// <param name="views"></param>
        public static void ReSetHotTopicList(int count, int views)
        {
            ReSetFocusTopicList(count, views, 0, TopicTimeType.All, TopicOrderType.ID, false);
        }

        //��������ָ��������������б�[��δ����]
        public static void ReSetHotTopicList(int count, int views, int fid, TopicTimeType timetype, TopicOrderType ordertype)
        {
            ReSetFocusTopicList(count, views, fid, timetype, ordertype, false);
        }

        /// <summary>
        /// ����������������б�
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
        /// �������л���
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
        /// ����BaseConfig
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
