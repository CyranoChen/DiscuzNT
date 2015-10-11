using System;
using System.Text;

namespace Discuz.Entity
{

    /// <summary>
    /// 广告类型
    /// </summary>
    public enum AdType
    {
        /// <summary>
        /// 头部横幅广告
        /// </summary>
        HeaderAd = 0x00,

        /// <summary>
        /// 尾部横幅广告
        /// </summary>
        FooterAd = 0x01,

        /// <summary>
        /// 页内文字广告
        /// </summary>
        PageWordAd = 0x02,

        /// <summary>
        /// 帖内广告
        /// </summary>
        InPostAd = 0x03,

        /// <summary>
        /// 浮动广告
        /// </summary>
        FloatAd = 0x04,

        /// <summary>
        /// 对联广告
        /// </summary>
        DoubleAd = 0x05,

        /// <summary>
        /// Silverlight媒体广告
        /// </summary>
        MediaAd = 0x06,

        /// <summary>
        /// 帖间通栏广告
        /// </summary>
        PostLeaderboardAd = 0x07,

        /// <summary>
        /// 分类间广告
        /// </summary>
        InForumAd = 0x08,

        /// <summary>
        /// 快速编辑器上方广告
        /// </summary>
        QuickEditorAd = 0x09,

        /// <summary>
        /// 快速编辑器背景广告
        /// </summary>
        QuickEditorBgAd = 0x0a,

        /// <summary>
        /// 聚合首页头部广告
        /// </summary>
        WebSiteHeaderAd = 0x0b,

        /// <summary>
        /// 聚合首页热贴下方广告
        /// </summary>
        WebSiteHotTopicAd = 0x0c,

        /// <summary>
        /// 聚合首页发帖排行上方广告
        /// </summary>
        WebSiteUserPostTopAd = 0x0d,

        /// <summary>
        /// 聚合首页推荐版块上方广告
        /// </summary>
        WebSiteRecForumTopAd = 0x0e,

        /// <summary>
        /// 聚合首页推荐版块下方广告
        /// </summary>
        WebSiteRecForumBottomAd = 0x0f,

        /// <summary>
        /// 聚合首页推荐相册下方广告
        /// </summary>
        WebSiteRecAlbumAd = 0x10,

        /// <summary>
        /// 聚合首页底部广告
        /// </summary>
        WebSiteBottomAd = 0x11,

        /// <summary>
        /// 页内横幅广告
        /// </summary>
        PageAd = 0x12
    }
}
