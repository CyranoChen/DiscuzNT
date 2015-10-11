using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;

namespace Discuz.Web
{
    /// <summary>
    /// 查看主题订阅页面
    /// </summary>
    public class usercpsubscribe : UserCpPage
    {
        #region 页面变量
        /// <summary>
        /// 收藏类型列表
        /// </summary>
        public int typeid = DNTRequest.GetInt("typeid", 0);
        /// <summary>
        /// 收藏夹类型
        /// </summary>
        public FavoriteType type = FavoriteType.ForumTopic;
        /// <summary>
        /// 收藏数
        /// </summary>
        public int favoriteCount = 0;
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";

            if (!IsLogin()) return;

            switch (typeid)
            {
                case 1: type = FavoriteType.Album; break;
                case 2: type = FavoriteType.SpacePost; break;
                case 3: type = FavoriteType.Goods; break;
                default: type = FavoriteType.ForumTopic; break;
            }

            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }
                string titemid = DNTRequest.GetFormString("titemid");

                if (Utils.StrIsNullOrEmpty(titemid) || !Utils.IsNumericList(titemid))
                {
                    AddErrLine("您未选中任何数据信息，当前操作失败！");
                    return;
                }

                if (Favorites.DeleteFavorites(userid, Utils.SplitString(titemid, ","), type) == -1)
                {
                    AddErrLine("参数无效");
                    return;
                }

                SetShowBackLink(false);
                SetUrl("usercpsubscribe.aspx");
                SetMetaRefresh();
                AddMsgLine("删除完毕");
                return;
            }
            else
            {
                favoriteCount = Favorites.GetFavoritesCount(userid, type);
                BindItems(favoriteCount, string.Format("usercpsubscribe.aspx?typeid={0}", typeid));
            }
        }

        public string GetForumName(string fid)
        {
            ForumInfo forumInfo = Forums.GetForumInfo(TypeConverter.StrToInt(fid));
            if (forumInfo != null)
                return Utils.RemoveHtml(forumInfo.Name);
            return string.Empty;
        }
    }
}