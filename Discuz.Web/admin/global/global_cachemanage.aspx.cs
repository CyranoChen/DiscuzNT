using System;
using System.IO;
using System.Web.UI;
using Discuz.Cache;
using Discuz.Control;
using Discuz.Forum;

using Discuz.Aggregation;
using Discuz.Common;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 缓存管理
    /// </summary>

    public partial class cachemanage : AdminPage
    {
        private void ReSetDigestTopicList_Click(object sender, EventArgs e)
        {
            #region 重新设置全部版块精华主题列表

            if (this.CheckCookie())
            {
                Caches.ReSetDigestTopicList(16);
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetHotTopicList_Click(object sender, EventArgs e)
        {
            #region 重新设置全部版块热帖主题列表

            if (this.CheckCookie())
            {
                Caches.ReSetHotTopicList(16, 30);
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetAdsList_Click(object sender, EventArgs e)
        {
            #region 重设广告列表

            if (this.CheckCookie())
            {
                Caches.ReSetAdsList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetStatisticsSearchtime_Click(object sender, EventArgs e)
        {
            #region 重新设置用户上一次执行搜索操作的时间

            if (this.CheckCookie())
            {
                Caches.ReSetStatisticsSearchtime();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetStatisticsSearchcount_Click(object sender, EventArgs e)
        {
            #region 重新设置用户在一分钟内搜索的次数

            if (this.CheckCookie())
            {
                Caches.ReSetStatisticsSearchcount();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetCommonAvatarList_Click(object sender, EventArgs e)
        {
            #region 重新设置用户头象列表

            if (this.CheckCookie())
            {
                Caches.ReSetCommonAvatarList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetJammer_Click(object sender, EventArgs e)
        {
            #region 重新设置干扰码字符串

            if (this.CheckCookie())
            {
                Caches.ReSetJammer();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetMagicList_Click(object sender, EventArgs e)
        {
            #region 重新设置魔力列表

            if (this.CheckCookie())
            {
                Caches.ReSetMagicList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetScorePaySet_Click(object sender, EventArgs e)
        {
            #region 重新设置兑换比率的可交易积分策略

            if (this.CheckCookie())
            {
                Caches.ReSetScorePaySet();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetPostTableInfo_Click(object sender, EventArgs e)
        {
            #region 重新设置当前帖子表相关信息

            if (this.CheckCookie())
            {
                Caches.ReSetPostTableInfo();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetTopiclistByFid_Click(object sender, EventArgs e)
        {
            #region 重新设置相应的主题列表

            if (this.CheckCookie())
            {
                if (txtTopiclistFid.Text == "")
                {
                    base.RegisterStartupScript( "", "<script>alert('重新设置相应主题列表的版块参数无效!');window.location.href='global_cachemanage.aspx';</script>");
                    return;
                }
                Caches.ReSetTopiclistByFid(txtTopiclistFid.Text);
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetMGinf_Click(object sender, EventArgs e)
        {
            #region 重新设置管理组信息

            if (this.CheckCookie())
            {
                Caches.ReSetAdminGroupList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetUGinf_Click(object sender, EventArgs e)
        {
            #region 重新设置用户组信息

            if (this.CheckCookie())
            {
                Caches.ReSetUserGroupList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetForumInf_Click(object sender, EventArgs e)
        {
            #region 重新设置版主信息

            if (this.CheckCookie())
            {
                Caches.ReSetModeratorList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetAnnonceList_Click(object sender, EventArgs e)
        {
            #region 重新设置指定时间内的公告列表

            if (this.CheckCookie())
            {
                Caches.ReSetAnnouncementList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetFirstAnnounce_Click(object sender, EventArgs e)
        {
            #region 重新设置第一条公告

            if (this.CheckCookie())
            {
                Caches.ReSetSimplifiedAnnouncementList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetForumDropList_Click(object sender, EventArgs e)
        {
            #region 重新设置版块下拉列表

            if (this.CheckCookie())
            {
                Caches.ReSetForumListBoxOptions();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetSmiles_Click(object sender, EventArgs e)
        {
            #region 重新设置表情

            if (this.CheckCookie())
            {
                Caches.ReSetSmiliesList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetThemeIcon_Click(object sender, EventArgs e)
        {
            #region 重新设置主题图标

            if (this.CheckCookie())
            {
                Caches.ReSetIconsList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetForumBaseSet_Click(object sender, EventArgs e)
        {
            #region 重新设置论坛基本设置

            if (this.CheckCookie())
            {
                Caches.ReSetConfig();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetAddressRefer_Click(object sender, EventArgs e)
        {
            #region 重新设置地址对照表

            if (this.CheckCookie())
            {
                Caches.ReSetSiteUrls();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetForumsStaticInf_Click(object sender, EventArgs e)
        {
            #region 重新设置论坛统计信息

            if (this.CheckCookie())
            {
                Caches.ReSetStatistics();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetAllCache_Click(object sender, EventArgs e)
        {
            #region 更新所有缓存

            if (this.CheckCookie())
            {
                Caches.ReSetAllCache();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetScoreset_Click(object sender, EventArgs e)
        {
            #region 重新设置论坛积分设置

            if (this.CheckCookie())
            {
                Caches.ReSetScoreset();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetAttachSize_Click(object sender, EventArgs e)
        {
            #region 重新设置系统允许的附件类型和大小

            if (this.CheckCookie())
            {
                Caches.ReSetAttachmentTypeArray();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetTemplateDropDown_Click(object sender, EventArgs e)
        {
            #region 重新设置模板列表的下拉框html

            if (this.CheckCookie())
            {
                Caches.ReSetTemplateListBoxOptionsCache();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetOnlineInco_Click(object sender, EventArgs e)
        {
            #region 重新设置在线用户列表图例

            if (this.CheckCookie())
            {
                Caches.ReSetOnlineGroupIconList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetLink_Click(object sender, EventArgs e)
        {
            #region 重新设置友情链接列表

            if (this.CheckCookie())
            {
                Caches.ReSetForumLinkList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetWord_Click(object sender, EventArgs e)
        {
            #region 重新设置脏字过滤列表

            if (this.CheckCookie())
            {
                Caches.ReSetBanWordList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetForumList_Click(object sender, EventArgs e)
        {
            #region 重新设置论坛列表

            if (this.CheckCookie())
            {
                Caches.ReSetForumList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetRss_Click(object sender, EventArgs e)
        {
            #region 重新设置论坛RSS

            if (this.CheckCookie())
            {
                Caches.ReSetRss();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetOnlineUserInfo_Click(object sender, EventArgs e)
        {
            #region 重新设置在线用户信息

            if (this.CheckCookie())
            {
                Caches.ReSetOnlineUserTable();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetRssByFid_Click(object sender, EventArgs e)
        {
            #region 重新设置指定版块RSS

            if (this.CheckCookie())
            {
                if (txtRssfid.Text == "")
                {
                    base.RegisterStartupScript( "", "<script>alert('重新设置指定版块RSS的版块参数无效!');window.location.href='global_cachemanage.aspx';</script>");
                    return;
                }

                Caches.ReSetForumRssXml(Convert.ToInt32(txtRssfid.Text));
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetRssAll_Click(object sender, EventArgs e)
        {
            #region 重新设置论坛整体RSS

            if (this.CheckCookie())
            {
                Caches.ReSetRssXml();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetTemplateIDList_Click(object sender, EventArgs e)
        {
            #region 重新设置论坛模板id列表

            if (this.CheckCookie())
            {
                Caches.ReSetValidTemplateIDList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetValidUserExtField_Click(object sender, EventArgs e)
        {
            #region 重新设置有效的用户表扩展字段

            if (this.CheckCookie())
            {
                Caches.ReSetValidScoreName();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetFlag_Click(object sender, EventArgs e)
        {
            #region 重新设置自定义标签

            if (this.CheckCookie())
            {
                Caches.ReSetCustomEditButtonList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ResetMedalList_Click(object sender, EventArgs e)
        {
            #region 重新设置勋章列表

            if (this.CheckCookie())
            {
                Caches.ReSetMedalsList();
                SubmitReturnInf();
            }

            #endregion
        }

        private void ReSetAggregation_Click(object sender, EventArgs e)
        {
            #region 重新设置聚合
            if (this.CheckCookie())
            {
                AggregationFacade.BaseAggregation.ClearAllDataBind();
                SubmitReturnInf();
            }
            #endregion
        }

        protected void ReSetNavPopupMenu_Click(object sender, EventArgs e)
        {
            #region 重设导航弹出菜单
            if (this.CheckCookie())
            {
                Caches.ReSetNavPopupMenu();
            }
            #endregion
        }

        private void SubmitReturnInf()
        {
            if (this.CheckCookie())
            {
                base.RegisterStartupScript( "PAGE",  "window.location.href='global_cachemanage.aspx';");
            }
        }

        private void ReSetTag_Click(object sender, EventArgs e)
        {
            DNTCache cache = DNTCache.GetCacheService();
            cache.RemoveObject("/Forum/Tag/Hot-" + config.Hottagcount);
        }

        protected void ReSetAlbumCategory_Click(object sender, EventArgs e)
        {
            #region 重新设置相册分类
            if (this.CheckCookie())
            {
                Caches.ResetAlbumCategory();
                SubmitReturnInf();
            }
            #endregion
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.ResetMGinf.Click += new EventHandler(this.ResetMGinf_Click);
            this.ResetUGinf.Click += new EventHandler(this.ResetUGinf_Click);
            this.ResetForumInf.Click += new EventHandler(this.ResetForumInf_Click);
            this.ResetAnnonceList.Click += new EventHandler(this.ResetAnnonceList_Click);
            this.ResetFirstAnnounce.Click += new EventHandler(this.ResetFirstAnnounce_Click);
            this.ResetForumDropList.Click += new EventHandler(this.ResetForumDropList_Click);
            this.ResetSmiles.Click += new EventHandler(this.ResetSmiles_Click);
            this.ResetThemeIcon.Click += new EventHandler(this.ResetThemeIcon_Click);
            this.ResetForumBaseSet.Click += new EventHandler(this.ResetForumBaseSet_Click);
            this.ReSetScoreset.Click += new EventHandler(this.ReSetScoreset_Click);
            this.ResetAddressRefer.Click += new EventHandler(this.ResetAddressRefer_Click);
            this.ResetForumsStaticInf.Click += new EventHandler(this.ResetForumsStaticInf_Click);
            this.ResetAttachSize.Click += new EventHandler(this.ResetAttachSize_Click);
            this.ResetTemplateDropDown.Click += new EventHandler(this.ResetTemplateDropDown_Click);
            this.ResetOnlineInco.Click += new EventHandler(this.ResetOnlineInco_Click);
            this.ResetLink.Click += new EventHandler(this.ResetLink_Click);
            this.ResetWord.Click += new EventHandler(this.ResetWord_Click);
            this.ResetForumList.Click += new EventHandler(this.ResetForumList_Click);
            this.ResetRss.Click += new EventHandler(this.ResetRss_Click);
            this.ResetRssByFid.Click += new EventHandler(this.ResetRssByFid_Click);
            this.ResetRssAll.Click += new EventHandler(this.ResetRssAll_Click);
            this.ResetTemplateIDList.Click += new EventHandler(this.ResetTemplateIDList_Click);
            this.ResetValidUserExtField.Click += new EventHandler(this.ResetValidUserExtField_Click);
            this.ResetOnlineUserInfo.Click += new EventHandler(this.ResetOnlineUserInfo_Click);
            this.ResetAllCache.Click += new EventHandler(this.ResetAllCache_Click);
            this.ResetFlag.Click += new EventHandler(this.ResetFlag_Click);
            this.ResetMedalList.Click += new EventHandler(this.ResetMedalList_Click);
            this.ReSetAdsList.Click += new EventHandler(this.ReSetAdsList_Click);
            this.ReSetStatisticsSearchtime.Click += new EventHandler(this.ReSetStatisticsSearchtime_Click);
            this.ReSetStatisticsSearchcount.Click += new EventHandler(this.ReSetStatisticsSearchcount_Click);
            this.ReSetCommonAvatarList.Click += new EventHandler(this.ReSetCommonAvatarList_Click);
            this.ReSetJammer.Click += new EventHandler(this.ReSetJammer_Click);
            this.ReSetMagicList.Click += new EventHandler(this.ReSetMagicList_Click);
            this.ReSetScorePaySet.Click += new EventHandler(this.ReSetScorePaySet_Click);
            this.ReSetPostTableInfo.Click += new EventHandler(this.ReSetPostTableInfo_Click);
            this.ReSetTopiclistByFid.Click += new EventHandler(this.ReSetTopiclistByFid_Click);
            this.ReSetDigestTopicList.Click += new EventHandler(this.ReSetDigestTopicList_Click);
            this.ReSetHotTopicList.Click += new EventHandler(this.ReSetHotTopicList_Click);
            this.ReSetAggregation.Click += new EventHandler(this.ReSetAggregation_Click);
            this.ReSetTag.Click += new EventHandler(this.ReSetTag_Click);
        }

        #endregion

    }
}