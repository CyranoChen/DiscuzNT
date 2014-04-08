using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Plugin.Space;
using Discuz.Plugin.Album;
using Discuz.Plugin.Mall;

namespace Discuz.Web
{
    /// <summary>
    /// 添加收藏页
    /// </summary>
    public class favorites : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 将要收藏的主题信息
        /// </summary>
        public TopicInfo topic;
        /// <summary>
        /// 主题所属版块
        /// </summary>
        public int forumid;
        /// <summary>
        /// 主题所属版块名称
        /// </summary>
        public string forumname = "";
        /// <summary>
        /// 主题Id
        /// </summary>
        public int topicid = DNTRequest.GetInt("topicid", -1);
        /// <summary>
        /// 主题标题
        /// </summary>
        public string topictitle = "";
        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav = "";
        /// <summary>
        /// 将要收藏的相册Id
        /// </summary>
        public int albumid = DNTRequest.GetInt("albumid", -1);
        /// <summary>
        /// 将要收藏的日志Id
        /// </summary>
        public int blogid = DNTRequest.GetInt("postid", -1);
        /// <summary>
        /// 将要收藏的商品Id
        /// </summary>
        public int goodsid = DNTRequest.GetInt("goodsid", -1);
        /// <summary>
        /// 主题所属版块
        /// </summary>
        public ForumInfo forum;
        /// <summary>
        /// 获取referer
        /// </summary>
        string referer = ForumUtils.GetCookie("referer");
        #endregion

        protected override void ShowPage()
        {
            if (userid == -1)
            {
                AddErrLine("你尚未登录");
                return;
            }

            //收藏的是主题
            if (topicid != -1)
            {
                // 获取该主题的信息
                TopicInfo topic = Topics.GetTopicInfo(topicid);
                // 如果该主题不存在
                if (topic == null)
                {
                    AddErrLine("不存在的主题ID");
                    return;
                }

                topictitle = topic.Title;
                forumid = topic.Fid;
                forum = Forums.GetForumInfo(forumid);
                forumname = forum.Name;
                pagetitle = Utils.RemoveHtml(forum.Name);
                forumnav = forum.Pathlist;

                CheckFavorite(FavoriteType.ForumTopic, topicid, "主题");
            }

            //收藏的是相册
            if (albumid != -1)
            {
                AlbumPluginBase apb = AlbumPluginProvider.GetInstance();
                if (apb == null)
                {
                    AddErrLine("未安装相册插件");
                    return;
                }
                if (apb.GetAlbumInfo(albumid) == null)
                {
                    AddErrLine("不存在的相册ID");
                    return;
                }

                CheckFavorite(FavoriteType.Album, albumid, "相册");
            }

            //收藏的是空间文章
            if (blogid != -1)
            {
                SpacePluginBase spb = SpacePluginProvider.GetInstance();
                if (spb == null)
                {
                    AddErrLine("未安装空间插件");
                    return;
                }
                if (spb.GetSpacepostsInfo(blogid) == null)
                {
                    AddErrLine("不存在的文章ID");
                    return;
                }

                CheckFavorite(FavoriteType.SpacePost, blogid, "文章");
            }

            //收藏的是商品
            if (goodsid != -1)
            {
                MallPluginBase mpb = MallPluginProvider.GetInstance();
                if (mpb == null)
                {
                    AddErrLine("未安装交易插件");
                    return;
                }
                if (mpb.GetGoodsInfo(goodsid) == null)
                {
                    AddErrLine("不存在的商品ID");
                    return;
                }

                CheckFavorite(FavoriteType.Goods, goodsid, "商品");
            }
        }

        /// <summary>
        /// 检验收藏信息
        /// </summary>
        /// <param name="favoriteType">收藏夹类型</param>
        /// <param name="id">收藏的信息ID，如:tid, albumid, blogid, goodsid</param>
        /// <param name="favoriteName">收藏类型名称，如：商品，文章，主题，相册</param>
        private void CheckFavorite(FavoriteType favoriteType, int id, string favoriteName)
        {
            // 检查用户是否拥有足够权限                
            if (config.Maxfavorites <= Favorites.GetFavoritesCount(userid, favoriteType))
            {
                AddErrLine("您收藏的" + favoriteName + "数目已经达到系统设置的数目上限");
                return;
            }
            if (Favorites.CheckFavoritesIsIN(userid, id, favoriteType) != 0)
            {
                AddErrLine("您过去已经收藏过该" + favoriteName);
                return;
            }
            if (Favorites.CreateFavorites(userid, id, favoriteType) > 0)
            {
                AddMsgLine("指定" + favoriteName + "已成功添加到收藏夹中");
                SetUrl(referer);
                SetMetaRefresh();
                SetShowBackLink(false);
            }
        }
    }
}
